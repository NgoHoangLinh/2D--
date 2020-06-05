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
    class ClientUnit
    {
        // The port number for the remote device.
        private const int port = 11000;

        // ManualResetEvent instances signal completion.
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent localReceiveDone = new ManualResetEvent(false);
        public static ManualResetEvent moveDone = new ManualResetEvent(false);

        // The response from the remote device.
        private static String response = String.Empty;

        public static ClientSideHandleHelper a = null;
        public static Thread thread = null;

        private static int ID = -1;
        public static int x = 0, y = 0, otherX = 0, otherY = 0;

        public static bool connected = false;
        public static Socket socket;

        public enum PacketInfo
        {
            ID, Position, EnemyCreate, Shoot, EnemyKilled, Message, Disconnect
        }

        public void Main(object o)
        {
            string s = (string)o;
            string[] s1 = s.Split(' ');
            StartClient(s1[0], Int32.Parse(s1[1]));
        }

        private static void StartClient(string hostIP, int port)
        {
            //IPHostEntry ipHostInfo = Dns.GetHostEntry(hostIP);
            //IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(hostIP), port);

            // Create a TCP/IP socket.
            socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);


            //socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Подключение к серверу...");

            //socket.BeginConnect(hostIP, 2048, new AsyncCallback(ConnectCallback), socket);
            socket.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), socket);

            connectDone.WaitOne();

            a = new ClientSideHandleHelper();
            thread = new Thread(a.HandleReceive);
            thread.Start(socket);
            //socket.Connect("127.0.0.1", 2048);

            Console.WriteLine("Response ID ...");
            //Send test data to the remote device.
            Send(socket, PacketInfo.ID);

            sendDone.WaitOne();

            localReceiveDone.WaitOne();

            while (true)
            {
                moveDone.WaitOne();
            }
            
        }
        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.
                connected = true;
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket 
                // from the asynchronous state object.
                Console.WriteLine("Получение данных...");
                ClientStateObject state = (ClientStateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    state.sb.Clear();
                    // There might be more data, so store the data received so far.
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    // Get the rest of the data.
                    //client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    //    new AsyncCallback(ReceiveCallback), state);
                    Console.WriteLine(state.sb.ToString());
                    if (state.sb.ToString().StartsWith("0"))
                    {
                        response = state.sb.ToString().Split(' ')[1];
                        ID = Int32.Parse(state.sb.ToString().Split(' ')[1]);

                        // Write the response to the console.
                        Console.WriteLine("Response received. ID : {0}", ID);
                        if (Form1.isServer)
                        {
                            Form1.player = new Player(0, 0, Resource1.Player1);
                        }
                        else
                        {
                            Form1.player = new Player(0, 50, Resource1.Player2);
                        }
                        Form1.player.ID = ID;
                        Console.WriteLine("                                                  Player ID: " + Form1.player.ID);
                    }
                    if (state.sb.ToString().StartsWith("1"))
                    {
                        response = state.sb.ToString().Split(' ')[1];
                        ID = Int32.Parse(state.sb.ToString().Split(' ')[1]);

                        // Write the response to the console.
                        Console.WriteLine("Response received. another ID : {0}", ID);
                        if (!Form1.isServer)
                        {
                            Form1.anotherPlayer = new Player(0, 0, Resource1.Player1);
                        }
                        else
                        {
                            Form1.anotherPlayer = new Player(0, 50, Resource1.Player2);
                            Form1.timerBlock = false;
                        }
                        Form1.anotherPlayer.ID = ID;
                        Send(socket, PacketInfo.Message, ID + " " + Form1.player.x + " " + Form1.player.y + " " + Form1.player.ID);
                    }
                    else if (state.sb.ToString().StartsWith("<POSITION>"))
                    {
                        response = state.sb.ToString();
                        string[] strs = state.sb.ToString().Split(' ');

                        Form1.anotherPlayer.x = Int32.Parse(strs[1]);
                        Form1.anotherPlayer.y = Int32.Parse(strs[2]);

                        Console.WriteLine("Response received. X position {0}, Y position {1}", Form1.anotherPlayer.x, Form1.anotherPlayer.y);
                    }
                    else if (state.sb.ToString().StartsWith("<MESSAGE>"))
                    {
                        response = state.sb.ToString();
                        string[] strs = state.sb.ToString().Split(' ');

                        if (!Form1.isServer)
                        {
                            Form1.anotherPlayer = new Player(0, 0, Resource1.Player1);
                        }
                        else
                        {
                            Form1.anotherPlayer = new Player(0, 50, Resource1.Player2);
                        }
                        Form1.anotherPlayer.x = Int32.Parse(strs[2]);
                        Form1.anotherPlayer.y = Int32.Parse(strs[3]);
                        Form1.anotherPlayer.ID = Int32.Parse(strs[4]);

                    }
                    else if (state.sb.ToString().StartsWith("<ENEMY_CREATE>"))
                    {
                        response = state.sb.ToString();
                        string[] strs = state.sb.ToString().Split(' ');

                        Form1.instance.enemies.Add(Form1.instance.createNewEnemy(Int32.Parse(strs[6]), Int32.Parse(strs[1]), Int32.Parse(strs[2]), Int32.Parse(strs[3]), Int32.Parse(strs[4]), Int32.Parse(strs[5])));
                    }
                    else if (state.sb.ToString().StartsWith("<SHOOT>"))
                    {
                        response = state.sb.ToString();
                        string[] strs = state.sb.ToString().Split(' ');

                        if (Form1.player.ID.ToString() == strs[1])
                        {
                            Form1.instance.shoot(Int32.Parse(strs[2]), Form1.player.ID);
                        }
                        else
                        {
                            Form1.instance.shootAnotherPlayer(Int32.Parse(strs[2]), Form1.anotherPlayer.ID); //TODO переделать id пули
                        }
                    }
                    else if (state.sb.ToString().StartsWith("<ENEMY_KILLED>"))
                    {
                        response = state.sb.ToString();
                        string[] strs = state.sb.ToString().Split(' '); //TODO нужно правильно разбить входящее сообщение
                        List<Enemy> enemies = new List<Enemy>();
                        int counter = 0;
                        for (int i = 1; i < (strs.Length / 2) + 1; i++)
                        {
                            Enemy en = Form1.instance.enemies.Find(e => e.enemyID == Int32.Parse(strs[i]));
                            if(en != null)
                            {
                                en.killedBy = Int32.Parse(strs[i + 1]);
                                counter = i + 2;
                                enemies.Add(en);
                                i++;
                            }
                            else
                            {
                                counter = i + 2;
                                i++;
                            }
                            
                        }

                        List<Shell> shells = new List<Shell>();
                        
                        for (int i = counter; i < (strs.Length); i++)
                        {
                            shells.Add(Form1.instance.shells.Find(s => s.shellID == Int32.Parse(strs[i])));
                        }

                        foreach (Enemy enemy in enemies)
                        {
                            Form1.instance.enemies.Remove(enemy);
                            if (enemy != null)
                            {
                                if (enemy.killedBy == Form1.player.ID)
                                {
                                    Form1.player.score += enemy.killBonus;
                                    Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Kill by Server");
                                }
                                else
                                {
                                    Form1.anotherPlayer.score += enemy.killBonus;
                                    Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Kill by another player by Server");
                                }
                            }
                        }
                        foreach (Shell shell in shells)
                        {
                            if (Form1.instance.shells.Count > 0)
                            {
                                Form1.instance.shells.Remove(shell);
                            }
                        }
                    }

                    a.receiveDone.Set();
                    localReceiveDone.Set();
                }
                else
                {
                    // All the data has arrived; put it in response.
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                    }
                    // Signal that all bytes have been received.
                    //receiveDone.Set();
                    a.receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void Send(Socket client, PacketInfo pi)
        {
            if (pi == PacketInfo.ID)
            {
                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.ASCII.GetBytes("0");

                // Begin sending the data to the remote device.
                client.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), client);
            }
            else if (pi == PacketInfo.Position)
            {
                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.ASCII.GetBytes("<POSITION> " + Form1.player.x + " " + Form1.player.y);

                // Begin sending the data to the remote device.
                client.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), client);
            }
            else if (pi == PacketInfo.EnemyCreate)
            {
                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.ASCII.GetBytes("<ENEMY_CREATE>");

                // Begin sending the data to the remote device.
                client.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), client);
            }
            else if (pi == PacketInfo.Shoot)
            {
                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.ASCII.GetBytes("<SHOOT>");

                // Begin sending the data to the remote device.
                client.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), client);
            }
            else if (pi == PacketInfo.Disconnect)
            {
                try
                {
                    // Convert the string data to byte data using ASCII encoding.
                    byte[] byteData = Encoding.ASCII.GetBytes("<END>");

                    // Begin sending the data to the remote device.
                    client.BeginSend(byteData, 0, byteData.Length, 0,
                        new AsyncCallback(SendCallback), client);
                }
                catch
                {

                }
            }

        }
        public static void Send(Socket client, PacketInfo pi, string message)
        {
            if (pi == PacketInfo.Message)
            {
                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.ASCII.GetBytes("<MESSAGE> " + message);

                // Begin sending the data to the remote device.
                client.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), client);
            }
            if (pi == PacketInfo.EnemyKilled)
            {
                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.ASCII.GetBytes("<ENEMY_KILLED>" + message);

                // Begin sending the data to the remote device.
                client.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), client);
            }
            if (pi == PacketInfo.EnemyCreate)
            {
                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.ASCII.GetBytes("<ENEMY_CREATE>" + message);

                // Begin sending the data to the remote device.
                client.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), client);
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;
                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.
                sendDone.Set();
                moveDone.Set();
                Form1.disconEvt.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
