using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApp4
{
    public class Client
    {
        public Socket socket { get; set; }
        public int ID { get; set; }

        public Thread thread { get; set; }
        public HandleClientHelper a { get; set; }

        public bool deleteStatus { get; set; }

        public Client(Socket socket)
        {
            this.socket = socket;
        }
    }
}
