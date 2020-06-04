using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApp4
{
    public class ClientSideHandleHelper
    {
        public ManualResetEvent receiveDone = new ManualResetEvent(false);

        public void HandleReceive(object o)
        {

            ClientStateObject state = new ClientStateObject();
            Socket handler = (Socket)o;
            state.workSocket = handler;

            while (true)
            {
                receiveDone.Reset();
               
                Console.WriteLine("               Client Side                   HandleReceive loop" + handler.RemoteEndPoint.ToString());
                try
                {
                    handler.BeginReceive(state.buffer, 0, ClientStateObject.BufferSize, 0,
                    new AsyncCallback(ClientUnit.ReceiveCallback), state);
                }
                catch
                {

                }
                receiveDone.WaitOne();
            }

        }
    }
}
