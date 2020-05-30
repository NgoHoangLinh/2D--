using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApp4
{
    public class HandleClientHelper
    {
        public ManualResetEvent receiveDone = new ManualResetEvent(false);

        public void HandleReceive(object o)
        {

            StateObject state = new StateObject();
            Socket handler = (Socket)o;
            state.workSocket = handler;
            while (true)
            {
                receiveDone.Reset();

                Console.WriteLine("HandleReceive loop" + handler.RemoteEndPoint.ToString());
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(Server.ReadCallback), state);
                receiveDone.WaitOne();

            }

        }
    }
}
