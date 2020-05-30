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

            ClientStateObject state;
            Socket handler = (Socket)o;
            
            while (true)
            {
                receiveDone.Reset();
                state = new ClientStateObject();
                state.workSocket = handler;
                Console.WriteLine("                                  HandleReceive loop" + handler.RemoteEndPoint.ToString());
                handler.BeginReceive(state.buffer, 0, ClientStateObject.BufferSize, 0,
                    new AsyncCallback(ClientUnit.ReceiveCallback), state);
                receiveDone.WaitOne();

            }

        }
    }
}
