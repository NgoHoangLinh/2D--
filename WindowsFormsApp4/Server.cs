using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApp4
{
    class Server
    {
        // Thread signal.
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        static List<Client> clients = new List<Client>();
        static Random random = new Random();

        static int enemyID = 0;
        static int shellID = 0;

        public Server()
        {
            
        }
        public void Main(object o)
        {
            StartListening();
            Console.ReadLine();
        }
        public static void StartListening()
        {
            Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            //Console.Title = "Server";

            try
            {
                socket.Bind(new IPEndPoint(IPAddress.Any, 2048));
                socket.Listen(2);

                while (true)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    Console.WriteLine("Waiting for a connection...");
                    socket.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        socket);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public static void AcceptCallback(IAsyncResult ar)
        {
            //////////////////////////////////////////////////////////////////////////////////////////////
            // Signal the main thread to continue.

            allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = handler;
            //handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
            //    new AsyncCallback(ReadCallback), state);
            Client client = new Client(handler);

            HandleClientHelper a = new HandleClientHelper();

            Thread thread = new Thread(a.HandleReceive);
            client.thread = thread;
            client.a = a;
            client.deleteStatus = false;
            clients.Add(client);
            thread.Start(handler);
        }
        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            foreach (Client c in clients)
            {
                if (c.socket.RemoteEndPoint == handler.RemoteEndPoint)
                {
                    c.a.receiveDone.Set();
                }
            }

            // Read data from the client socket. 
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                state.sb.Clear();
                // There  might be more data, so store the data received so far.
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read 
                // more data.
                content = state.sb.ToString();

                if (content == "<END>")
                {
                    foreach (Client c in clients)
                    {
                        if (c.socket == handler)
                        {
                            Console.WriteLine("Abort " + handler.RemoteEndPoint.ToString());
                            c.thread.Abort();
                            c.deleteStatus = true;
                        }
                    }

                    List<Client> tmpC = new List<Client>();
                    for (int i = 0; i < clients.Count; i++)
                    {
                        if (!clients[i].deleteStatus)
                        {
                            tmpC.Add(clients[i]);
                        }
                    }
                    clients = tmpC;

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();

                }
                else if (content.Contains("<MESSAGE>"))
                {
                    // All the data has been read from the 
                    // client. Display it on the console.
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        content.Length, content);
                    // Echo the data back to the client.
                    Console.WriteLine("Received from " + handler.RemoteEndPoint.ToString());

                    string[] strs = content.Split(' ');

                    foreach (Client c in clients)
                    {
                        if (c.ID.ToString() == strs[1])
                        {
                            Send(c.socket, content);
                        }
                        
                    }
                }
                else if (content.StartsWith("0"))
                {
                    // All the data has been read from the 
                    // client. Display it on the console.
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        content.Length, content);
                    // Echo the data back to the client.
                    Console.WriteLine("Received from " + handler.RemoteEndPoint.ToString());
                    Random random = new Random();
                    int id = random.Next(0, 1001);

                    foreach (Client c in clients)
                    {
                        if (c.socket != handler)
                        {
                            Send(c.socket, "1 " + id);
                        }
                        else
                        {
                            c.ID = id;
                            Send(c.socket, "0 " + id);
                        }
                    }

                }
                else if (content.StartsWith("<POSITION>"))
                {
                    // All the data has been read from the 
                    // client. Display it on the console.
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        content.Length, content);
                    // Echo the data back to the client.
                    Console.WriteLine("Received from " + handler.RemoteEndPoint.ToString());
                    /*
                    foreach(Client c in clients)
                    {
                        Send(c.socket, content);
                    }
                    */
                    foreach (Client c in clients)
                    {
                        if (c.socket != handler)
                        {
                            Send(c.socket, content);
                        }
                    }

                }
                else if (content.StartsWith("<ENEMY_CREATE>"))
                {
                    // All the data has been read from the 
                    // client. Display it on the console.
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        content.Length, content);
                    // Echo the data back to the client.
                    Console.WriteLine("Received from " + handler.RemoteEndPoint.ToString());
                    /*
                    foreach(Client c in clients)
                    {
                        Send(c.socket, content);
                    }
                    */
                    foreach (Client c in clients)
                    {
                        Send(c.socket, content + " " + enemyID);
                    }
                    enemyID++;
                }
                else if (content.StartsWith("<SHOOT>"))
                {
                    // All the data has been read from the 
                    // client. Display it on the console.
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        content.Length, content);
                    // Echo the data back to the client.
                    Console.WriteLine("Received from " + handler.RemoteEndPoint.ToString());
                    /*
                    foreach(Client c in clients)
                    {
                        Send(c.socket, content);
                    }
                    */
                    foreach (Client c in clients)
                    {
                        if (c.socket != handler)
                        {
                            Send(c.socket, content + " 1 " + shellID);
                        }
                        else
                        {
                            Send(c.socket, content + " 0 " + shellID);
                        }
                    }
                    shellID++;
                }
                else if (content.StartsWith("<ENEMY_KILLED>"))
                {
                    // All the data has been read from the 
                    // client. Display it on the console.
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        content.Length, content);
                    // Echo the data back to the client.
                    Console.WriteLine("Received from " + handler.RemoteEndPoint.ToString());
                    /*
                    foreach(Client c in clients)
                    {
                        Send(c.socket, content);
                    }
                    */
                    foreach (Client c in clients)
                    {
                        Send(c.socket, content);
                    }
                }
                else if (content.IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the 
                    // client. Display it on the console.
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        content.Length, content);
                    // Echo the data back to the client.
                    Send(handler, content);

                }
                else
                {
                    // Not all data received. Get more.
                    Console.WriteLine("Not all data received. Get more.");
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }

            }
        }
        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                /*
                handler.Shutdown(SocketShutdown.Both);
                
                handler.Close();
                */



            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
