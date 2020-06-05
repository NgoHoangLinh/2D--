using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApp4
{
    class Server
    {
        // Thread signal.
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public static bool serverStarted = false;

        public static List<Client> clients = new List<Client>();
        static Random random = new Random();
        static int port;
        static int enemyID = 0;
        static int shellID = 0;

        public Server()
        {
            
        }
        public void Main(object o)
        {
            port = Int32.Parse((string)o);
            StartListening();
            Console.ReadLine();
        }
        public static void StartListening()
        {
            
            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.
            // The DNS name of the computer
            // running the listener is "host.contoso.com".
            //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());//Dns.GetHostName());
            
            //IPAddress ipAddress = ipHostInfo.AddressList[0];
            //Console.WriteLine(ipAddress);
            //IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);

            // Create a TCP/IP socket.
            Socket socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach(IPAddress ip in localIPs)
            {
                if(ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Console.WriteLine(ip.ToString());
                }
            }


            //Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            //Console.Title = "Server";

            try
            {
                socket.Bind(new IPEndPoint(IPAddress.Any, port));  //создаёт точку подключения
                //socket.Bind(localEndPoint);  //создаёт точку подключения
                socket.Listen(2);

                while (true) //проходит цикл при новом подключении
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    Console.WriteLine("Waiting for a connection...");
                    socket.BeginAccept(
                        new AsyncCallback(AcceptCallback), socket); //начинаем принимать входящие подключения

                    // Wait until a connection is made before continuing.

                    serverStarted = true;
                    allDone.WaitOne(); //усыпляет поток
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public static void AcceptCallback(IAsyncResult ar) //выполняется при новом подключнии
        {
            //////////////////////////////////////////////////////////////////////////////////////////////
            // Signal the main thread to continue.

            allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;

            //Сокет удалённого подключения
            Socket handler = listener.EndAccept(ar);

            // Create the state object. Нужен для сохранения информации о клиенте и его сообщении
            StateObject state = new StateObject();
            state.workSocket = handler;
            
            Client client = new Client(handler);

            HandleClientHelper a = new HandleClientHelper();

            Thread thread = new Thread(a.HandleReceive);
            client.thread = thread;
            client.a = a;
            client.deleteStatus = false;
            clients.Add(client);
            thread.Start(handler);

        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void ReadCallback(IAsyncResult ar) //выполняет считывание сообщения. Входядий параметр - StateObject
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            int bytesRead;
            try //отлавливает ошибку с сокетом. Внутри выполняется извлечение сообщения
            {
                foreach (Client c in clients)
                {
                    if (c.socket.RemoteEndPoint == handler.RemoteEndPoint)
                    {
                        c.a.receiveDone.Set(); //выводим из сна поток, обрабатывающий сообщения он конкретного клиента (handler)
                    }
                }
                // Read data from the client socket. 
                bytesRead = handler.EndReceive(ar);
            }
            catch
            {
                foreach (Client c in clients)
                {
                    if (c.socket == handler)
                    {
                        Console.WriteLine("Abort " + handler.RemoteEndPoint.ToString());
                        c.thread.Abort(); //прерываем поток
                        c.deleteStatus = true;
                    }
                }
                return;
            }
            

            if (bytesRead > 0)
            {
                state.sb.Clear(); //очищает стоку StringBuilder в StateObject 
                Console.WriteLine("*******************************Text before append: " + state.sb.ToString());
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead)); //в строку StringBuilder записываем входящее сообщение

                content = state.sb.ToString(); //записываем сообщение из StringBuilder в String

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
                else if (content.Contains("<MESSAGE>")) //<MESSAGE> RESEIVER23124 X Y SENDER123123
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
                else if (content.StartsWith("0")) //0 клиент запросил id
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
                            Send(c.socket, "1 " + id);//1 newAnotherPlayerID
                        }
                        else
                        {
                            c.ID = id;
                            Send(c.socket, "0 " + id); //0 newID
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
                    
                    int shooterID = clients.Find(c => c.socket == handler).ID;
                    foreach (Client c in clients)
                    {
                        Send(c.socket, content + " " + shooterID + " " + shellID);
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
                    
                    foreach (Client c in clients)
                    {
                        if (c.socket != handler)
                        {
                            Send(c.socket, content);
                        }
                    }
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

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
