using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkController
{
    // Delegate used for the callMe in the SocketState
    public delegate void NetworkAction(SocketState ss);

    /// <summary>
    /// This class holds all the necessary state to represent a socket connection
    /// Note that all of its fields are public because we are using it like a "struct"
    /// It is a simple collection of fields
    /// </summary>
    public class SocketState
    {
        // The socket used to connect to the server
        public Socket theSocket;
        public int ID;
        // Delegate used call different methods from the controller at different times
        public NetworkAction callMe; 

        // This is the buffer where we will receive data from the socket
        public byte[] messageBuffer = new byte[4096];

        // This is a larger (growable) buffer, in case a single receive does not contain the full message.
        public StringBuilder sb = new StringBuilder();

        /// <summary>
        /// Contructor setting the socket and callMe delegate for the SocketState
        /// </summary>
        /// <param name="s">Input set to theSocket</param>
        /// <param name="na">Input set to the callMe delegate</param>
        public SocketState(Socket s, NetworkAction na)
        {
            theSocket = s;
            callMe = na;
        }
    }

   
    /// <summary>
    /// 
    /// </summary>
    public static class Networking
    {
        public const int DEFAULT_PORT = 2112;

        /// <summary>
        /// Start attempting to connect to a server
        /// </summary>
        /// <param name="ip">The address of the server</param>
        public static Socket ConnectToServer(string ip, NetworkAction callMe)
        {
            IPAddress[] addresslist = Dns.GetHostAddresses(ip);
            // Parse the IP
            IPAddress addr = addresslist[addresslist.Length - 1];

            // Put the socket into a SocketState that also contains the buffer 
            // where data will be received
            SocketState ss = new SocketState(new Socket(addr.AddressFamily, SocketType.Stream,
              ProtocolType.Tcp), callMe);

            // Connect
            // We pass the state to the callback. It will be contained in the IAsyncResult
            ss.theSocket.BeginConnect(addr, DEFAULT_PORT, ConnectedCallback, ss);

            return ss.theSocket;
        }

        /// <summary>
        /// This method is automatically invoked on its own thread when a connection is made.
        /// </summary>
        /// <param name="ar"></param>
        private static void ConnectedCallback(IAsyncResult ar)
        {
            Console.WriteLine("contact from server");

            // Get the SocketState associated with this connection 
            SocketState ss = (SocketState)  ar.AsyncState;

            // This is required to complete the "handshake" with the server. Both parties agree a connection is made.
            // Only EndConnect if the socket is truly connected
            if (ss.theSocket.Connected)
            {
                ss.theSocket.EndConnect(ar);
            }
            
            ss.callMe(ss);
        }

        public static void Send(Socket theSocket, object jsonConv, object p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is invoked on its own thread when data arrives from the server.
        /// </summary>
        /// <param name="ar"></param>
        private static void ReceiveCallback(IAsyncResult ar)
        {
            // Get the SocketState representing the connection on which data was received
            SocketState ss = (SocketState)ar.AsyncState;

            int numBytes = ss.theSocket.EndReceive(ar);

            // Convert the raw bytes to a string
            if (numBytes > 0)
            {
                string message = Encoding.UTF8.GetString(ss.messageBuffer, 0, numBytes);

                lock(ss.sb)
                {
                    ss.sb.Append(message);
                }
            }
            else
            {
                // Close the socket if no data is received
                ss.theSocket.Close();
            }

            ss.callMe(ss);
        }

        /// <summary>
        /// Helper function that the client code will call whenever it wants more data
        /// </summary>
        /// <param name="ss"></param>
        public static void GetData(SocketState ss)
        {
            ss.theSocket.BeginReceive(ss.messageBuffer, 0, ss.messageBuffer.Length, SocketFlags.None, ReceiveCallback, ss);
        }

        /// <summary>
        /// Send data to a server over a socket
        /// </summary>
        /// <param name="s">Socket used to connect to the server</param>
        /// <param name="data">Input string sent to the server</param>
        public static void Send(Socket s, string data)
        {
            // Convert the data to bytes
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            // Send the data to the server
            s.BeginSend(byteData, 0, byteData.Length, 0, SendCallback,  s);
        }

        /// <summary>
        /// This function assists the Send function. 
        /// It extracts the Socket out of the IAsyncResult, and then calls socket.EndSend
        /// </summary>
        /// <param name="ar"></param>
		private static void SendCallback(IAsyncResult ar)
		{
			Socket s = (Socket)ar.AsyncState;

                s.EndSend(ar);

		}

        /// <summary>
        /// Starts a TcpListener to continuously listen for data from clients on a given socket
        /// </summary>
        /// <param name="callMe"></param>
        public static void ServerAwaitingClientLoop(NetworkAction callMe)
        {
            TcpListener lstn = new TcpListener(IPAddress.Any, 11000);

            lstn.Start();
            ConnectionState cs = new ConnectionState();
            cs.listener = lstn;
            cs.callMe = callMe;

            // Send AcceptNewClient as a delegate and the new ConnectionState to accept a new client
            lstn.BeginAcceptSocket(AcceptNewClient, cs);
        }

        /// <summary>
        /// Method called when a client attempts to connect to the server
        /// </summary>
        /// <param name="ar"></param>
        public static void AcceptNewClient(IAsyncResult ar)
        {
            // Get the current connection state
            ConnectionState cs = (ConnectionState)ar.AsyncState;
            Socket socket = cs.listener.EndAcceptSocket(ar);
            // Create a new socket state for the client
            SocketState ss = new SocketState(socket, cs.callMe);
            // Send the new socketstate to the server
            ss.callMe(ss);
            // Open the server to allow other clients to connect to it
            cs.listener.BeginAcceptSocket(AcceptNewClient, cs);
        }
    }

    /// <summary>
    /// Class holding a callMe delegate and a TCPListener allowing
    /// the server to continuously listen for clients' data
    /// </summary>
    public class ConnectionState
    {
        public TcpListener listener;
        public NetworkAction callMe;
    }
}