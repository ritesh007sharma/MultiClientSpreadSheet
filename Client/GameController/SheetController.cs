using System;
using NetworkController;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Windows.Input;
using System.Text;
using System.Timers;
using Newtonsoft.Json;

// 155.98.111.79
// 155.98.111.67
namespace SC
{
    [JsonObject(MemberSerialization.OptIn)]
    public class OpenMessage
    {
        [JsonProperty]
        private String type;
        [JsonProperty]
        private String name;
        [JsonProperty]
        private String username;
        [JsonProperty]
        private String password;

        public OpenMessage(String username, String password, String filename)
        {
            this.username = username;
            this.password = password;
            this.name = filename;
            this.type = "open";
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class EditMessage
    {
        [JsonProperty]
        private String type;
        [JsonProperty]
        private String cell;
        [JsonProperty]
        private object value;
        [JsonProperty]
        private ISet<String> dependencies;

        public EditMessage(String cell, object value, ISet<String> d)
        {
            this.cell = cell;
            this.value = value;
            this.dependencies = d;
            this.type = "edit";
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class UndoMessage
    {
        [JsonProperty]
        private String type;

        public UndoMessage()
        {
            this.type = "undo";
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class RevertMessage
    {
        [JsonProperty]
        private String type;
        [JsonProperty]
        private String cell;

        public RevertMessage(String cell)
        {
            this.cell = cell;
            this.type = "revert";
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ReceivedMessage
    {
        [JsonProperty]
        public String type;
        [JsonProperty]
        public int code;
        [JsonProperty]
        public String source;
        [JsonProperty]
        public List<String> spreadsheets;
        [JsonProperty]
        public Dictionary<String, String> spreadsheet;
    }



    /// <summary>
    /// Controller giving the spreadsheet clients access to the server
    /// </summary>
    public class SheetController
    {

        // The socket used to connect to the server
        private Socket socket;

        // Delegate/event used to prompt the world to update in the form
        public delegate void UpdateScreenHandler();
        public event UpdateScreenHandler PromptViewRedraw;

        // Delegate/events used to prompt the form if the server has connected
        public delegate void ConnectionHandler();
        public event ConnectionHandler InformViewConnectionFound;
        public event ConnectionHandler InformViewConnectionFailed;

        // Delegate/event used to pass the playerID and worldSize to the form
        public delegate void SetupHandler(List<String> files);
        public event SetupHandler SendSetupToView;

       
        public delegate void UpdateHandler(Dictionary<String,String> files);
        public event UpdateHandler UpdateSpreadsheet;

        public delegate void ErrorHandler();
        public event ErrorHandler ErrorCodeOne;

        public delegate void ErrorTwoHandler(String cell);
        public event ErrorTwoHandler ErrorCodeTwo;

        public delegate void EstablishConnection();
        public event EstablishConnection ResetConnection;


        // Delegate/event used to pass json data to the form
        public delegate void ModifyWorldHandler(JObject obj);
        public event ModifyWorldHandler SendToView;

        public event ElapsedEventHandler UpdateScore;

        /// <summary>
        /// Constructor for the game controller setting each key boolean to false
        /// </summary>
        public SheetController()
        {

        }


        public void SendServerInfo(String filename, String username, String password)
        {

            OpenMessage info = new OpenMessage(username, password, filename);
            string check = JsonConvert.SerializeObject(info) + "\n\n";
            Console.WriteLine(check);
            Networking.Send(socket, check);
        }
        


        /// <summary>
        /// Establishes connection with the server
        /// </summary>
        /// <param name="ss">The socket state used to connect with the server</param>
        public void FirstContact(SocketState ss)
        {

            // Check if the Socket is really connected
            if (ss.theSocket.Connected)
            {

                // Set the call me method in the socketstate to ReceiveStartup
                ss.callMe = ReceiveStartup;

                Networking.Send(ss.theSocket, "");

                try
                {
                    Networking.GetData(ss);
                }
                catch (Exception)
                {
                    ResetConnection();
                }
                // Inform the form that a connection has been found
                //      InformViewConnectionFound();
            }
            else
            {
                // If not connected, bring up connection failed dialog box and reprompt
                ResetConnection();
            }
        }

        /// <summary>
        /// Delegate used to get the startup data from the server
        /// </summary>
        /// <param name="ss">Socket state used to pull data from the server</param>
        private void ReceiveStartup(SocketState ss)
        {
            // Call function to extract ID and World size
            ExtractFirstData(ss);

            // Set ReceiveWorld as the delegate called by the network controller
            ss.callMe = ReceiveWorld;

            try
            {
                Networking.GetData(ss);
            }
            catch (Exception)
            {
                ResetConnection();
            }
        }

        /// <summary>
        /// Method that extracts data from the messagebuffer in the socket state
        /// </summary>
        /// <param name="ss">Socket state whose message buffer data is extracted from</param>
        public void ReceiveWorld(SocketState ss)
        {
            string message;

            lock (ss.sb)
            {
                message = (ss.sb.ToString());
                
                // Clear the string builder in the socket state
                ss.sb.Remove(0, ss.sb.Length);
            }
            string[] parts = message.Split(new [] { "\n\n" }, StringSplitOptions.None);

            foreach (string p in parts)
            {
                ReceivedMessage m = JsonConvert.DeserializeObject<ReceivedMessage>(p);
                if (m != null)
                {
                    if (m.type == "error")
                    {
                        if(m.code == 1)
                        {
                            ErrorCodeOne();
                        }
                        if (m.code == 2)
                        {
                            ErrorCodeTwo(m.source);
                        }
                    }
                    else
                    {
                        UpdateSpreadsheet(m.spreadsheet);
                    }
                }
            }
            
            try
            {
                Networking.GetData(ss);
            }
            catch (Exception)
            {
                ResetConnection();
            }
        }

        /// <summary>
        /// Instantiates a connection with the server
        /// </summary>
        /// <param name="ip">The host address of the server</param>
        /// <param name="name">The playerID sent to the server</param>
        public void Connect(String ip, String username, String password)
        {

            // If the name or ip is invalid, inform the view that the connection failed
            if (ip.Length == 0 /*|| username.Length == 0 || password.Length == 0*/)
            {
                InformViewConnectionFailed();
            }
            else
            {
                try
                {
                    // Try connecting to the server
                    socket = Networking.ConnectToServer(ip, FirstContact);

                }
                catch (Exception)
                {
                    InformViewConnectionFailed();
                }
            }
        }

        /// <summary>
        /// Method used to extract the world size and playerID from the server
        /// </summary>
        /// <param name="ss"></param>
        private void ExtractFirstData(SocketState ss)
        {
            string message;

            lock (ss.sb)
            {
                message = (ss.sb.ToString());
                
                // Clear the string builder in the socket state
                ss.sb.Remove(0, ss.sb.Length);
            }

            ReceivedMessage l = JsonConvert.DeserializeObject<ReceivedMessage>(message);
      

            SendSetupToView(l.spreadsheets);
        }


        public void UpdateServer(String cellID, String contents, HashSet<String> d)
        {
            EditMessage message;
            if (Double.TryParse(contents, out double result)) {

                message = new EditMessage(cellID, result, d);
            }
          
            else
            {
                message = new EditMessage(cellID, contents, d);
            }

            string check = JsonConvert.SerializeObject(message) + "\n\n";
            Networking.Send(socket, check);
        }

        public void Revert(String cellID)
        {
            Networking.Send(socket, JsonConvert.SerializeObject(new RevertMessage(cellID)) + "\n\n");
        }

        public void Undo()
        {
            Networking.Send(socket, JsonConvert.SerializeObject(new UndoMessage()) + "\n\n");
        }
    }
}
