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
namespace AC
{
    // <summary>
    /// Function for 'Recieve world This should recieve all of the users and spreadsheets from the server
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ReceiveAdminMessage
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
        [JsonProperty]
        public List<String> users;
        [JsonProperty]
        public Dictionary<String, String> user;
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ShutDownServer
    {
        [JsonProperty]
        private String type;

        public ShutDownServer()
        {
            this.type = "shutdown";
        }
    }

    /// <summary>
    /// Send information to server letting it know it is an admin tool
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class AdminFirstMessage
    {
        [JsonProperty]
        private String type;
        [JsonProperty]
        private String name;
        [JsonProperty]              //Possibly Optional
        private String username;

        public AdminFirstMessage(String username, String filename)
        {
            this.username = username;
            this.name = filename;
            this.type = "adminOpen";
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class RecieveUsers
    {
        [JsonProperty]
        private String type;

        public RecieveUsers()
        {
            this.type = "get_users";
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class RecieveSpreadsheets
    {
        [JsonProperty]
        private String type;

        public RecieveSpreadsheets()
        {
            this.type = "get_spreadsheets";
        }
    }


    //Delete then call open
    [JsonObject(MemberSerialization.OptIn)]
    public class EditUser
    {
        [JsonProperty]
        private String type;
        [JsonProperty]
        private String newUser;
        [JsonProperty]
        private String newPass;
        [JsonProperty]
        private String oldUser;

        public EditUser(String oldUsername, String newUsername, String newPassword)
        {
            this.newUser = newUsername;
            this.newPass = newPassword;
            this.oldUser = oldUsername;
            this.type = "adminEditUser";
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class AddUser
    {
        [JsonProperty]
        private String type;
        [JsonProperty]
        private String username;
        [JsonProperty]
        private String password;
        [JsonProperty]
        private String name;

        public AddUser(String username, String password, String filename)
        {
            this.username = username;
            this.password = password;
            this.name = filename;
            this.type = "open";
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class DeleteUser
    {
        [JsonProperty]
        private String type;
        [JsonProperty]
        private String username;

        public DeleteUser(String userToDelete)
        {
            this.username = userToDelete;
            this.type = "delete_user";
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class AddSpreadsheet
    {
        [JsonProperty]
        private String type;
        [JsonProperty]
        private String spreadsheet;

        public AddSpreadsheet(String spreadsheetToAdd)
        {
            this.spreadsheet = spreadsheetToAdd;
            this.type = "adminAddSpreadsheet";
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class DeleteSpreadsheet
    {
        [JsonProperty]
        private String type;
        [JsonProperty]
        private String name;

        public DeleteSpreadsheet(String spreadsheetToDelete)
        {
            this.name = spreadsheetToDelete;
            this.type = "delete_sheet";
        }
    }

    /////////
    // Referenced From Sheet Controller
    // Original JSon  [Most of this need not be used]
    /////////
    ///
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

    [JsonObject(MemberSerialization.OptIn)]
    public class UsersRecieved
    {
        //[JsonProperty]
        //public List<Dictionary<String,String>> N;
        [JsonProperty]
        public int code;
        [JsonProperty]
        public String u;
        [JsonProperty]
        public String p;
        [JsonProperty]
        public Dictionary<String, String> N;
        [JsonProperty]
        public List<String> users;
        [JsonProperty]
        public Dictionary<String, String> user;
    }

    /// <summary>
    /// Controller giving the spreadsheet clients access to the server
    /// </summary>
    public class AdminController
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
        public event ConnectionHandler ResetConnection;

        // Delegate/event used to pass the playerID and worldSize to the form
        public delegate void SetupHandler(List<String>Spreadsheets);
        public event SetupHandler SendSetupToView;


        public delegate void UpdateHandler(Dictionary<String, String> files);
        public event UpdateHandler UpdateSpreadsheet;


        // Delegate/event used to pass json data to the form
        public delegate void ModifyWorldHandler(JObject obj);
        public event ModifyWorldHandler SendToView;

        public event ElapsedEventHandler UpdateScore;

        /// <summary>
        /// Constructor for the game controller setting each key boolean to false
        /// </summary>
        public AdminController()
        {

        }


        public void SendServerInfo(String filename, String username)
        {

            AdminFirstMessage info = new AdminFirstMessage(username, filename);
            string check = JsonConvert.SerializeObject(info);
            Console.WriteLine(check);
            Networking.Send(socket, check);
        }

        public void RequestUsers()
        {
            RecieveUsers request = new RecieveUsers();
            string check = JsonConvert.SerializeObject(request);
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
                InformViewConnectionFailed();
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

            ReceivedMessage m = JsonConvert.DeserializeObject<ReceivedMessage>(message);
            UsersRecieved n = JsonConvert.DeserializeObject<UsersRecieved>(message);
            if(n != null)
            {
                if ((n.N != null))
                {
                    UpdateSpreadsheet(parseUserList(message));
                }
                else if (m != null)
                {
                    if (m.type == "list")
                    {
                        SendSetupToView(m.spreadsheets);
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

        static Dictionary<string, string> parseUserList(string j)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            string userName = "";
            for (int i = 0; i < j.Length; i++)
            {
                if (j[i] == '"')
                {
                    //if you find a quote
                    if (j[i + 1] == 'u' && j[i + 2] == '"')
                    {
                        i += 5;
                        StringBuilder b = new StringBuilder(16);
                        for (int ii = i; j[ii] != '"'; ++ii)
                        {
                            Console.WriteLine("in loop");
                            b.Append(j[ii]);
                        }
                        userName = b.ToString();

                        Console.WriteLine(b.ToString() + "\n");

                    }
                    else if (j[i + 1] == 'p' && j[i + 2] == '"')
                    {
                        i += 5;
                        StringBuilder b = new StringBuilder(16);
                        for (int ii = i; j[ii] != '"'; ++ii)
                            b.Append(j[ii]);
                        Console.WriteLine(userName + " " + b.ToString() + "\n");
                        d.Add(userName, b.ToString());
                    }
                }

            }
            return d;
        }

        /// <summary>
        /// Instantiates a connection with the server
        /// </summary>
        /// <param name="ip">The host address of the server</param>
        /// <param name="name">The playerID sent to the server</param>
        public void Connect(String ip)
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
        ///             ||| TO DO!!!  |||
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

            //Possible edited version to original
            //ReceiveAdminMessage l = JsonConvert.DeserializeObject<ReceiveAdminMessage>(message);
            //SendSetupToView(l.users, l.spreadsheets);
            //send l to form.cs->look at sendsetuptoview
            
            ReceivedMessage l = JsonConvert.DeserializeObject<ReceivedMessage>(message);

            if(l.type == "list")
            {
                SendSetupToView(l.spreadsheets);
            }
            
        }

        #region Admin Tool Functions

        /*
        public EditUser(String oldUsername, String newUsername, String oldPassword, String newPassword)
        
        public DeleteUser(String userToDelete)
        
        public AddSpreadsheet(String spreadsheetToAdd)
        
        public DeleteSpreadsheet(String spreadsheetToDelete)
             */

        /// <summary>
        /// Add a new user to the server
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void AddNewUser(String username, String password, String filename)
        {
            AddUser user = new AddUser(username, password, filename);
            string check = JsonConvert.SerializeObject(user);
            Networking.Send(socket, check);
        }

        public void EditThisUser(String oldUsername, String newUsername, String newPassword, String filename)
        {
            DeleteThisUser(oldUsername);
            AddNewUser(newUsername, newPassword, filename);
        }

        public void DeleteThisUser(String username)
        {
            DeleteUser user = new DeleteUser(username);
            string check = JsonConvert.SerializeObject(user);
            Networking.Send(socket, check);
        }

        public void AddThisSpreadsheet(String Spreadsheetname)
        {
            AddSpreadsheet spreadsheet = new AddSpreadsheet(Spreadsheetname);
            string check = JsonConvert.SerializeObject(spreadsheet);
            Networking.Send(socket, check);
        }

        public void DeleteThisSpreadsheet(String Spreadsheetname)
        {
            DeleteSpreadsheet spreadsheet = new DeleteSpreadsheet(Spreadsheetname);
            string check = JsonConvert.SerializeObject(spreadsheet);
            Networking.Send(socket, check);
        }

        public void ShutDownThisServer()
        {
            ShutDownServer sDS = new ShutDownServer();
            string check = JsonConvert.SerializeObject(sDS);
            Networking.Send(socket, check);
        }

        public void UpdateUsersAndSpreadsheets()
        {

        }

        #endregion

        public void UpdateServer(String cellID, String contents, HashSet<String> d)
        {
            EditMessage message;
            if (Double.TryParse(contents, out double result))
            {
                message = new EditMessage(cellID, result, d);
            }
            else
            {
                message = new EditMessage(cellID, contents, d);
            }

            string check = JsonConvert.SerializeObject(message);
            Networking.Send(socket, check);
        }

        public void Revert(String cellID)
        {
            Networking.Send(socket, JsonConvert.SerializeObject(new RevertMessage(cellID)));
        }

        public void Undo()
        {
            Networking.Send(socket, JsonConvert.SerializeObject(new UndoMessage()));
        }
    }
}
