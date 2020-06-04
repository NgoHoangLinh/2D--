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
        //Сокет клиетнта
        public Socket socket { get; set; }
        //ID клиента
        public int ID { get; set; }
        //Поток обрабатывающий входящие сообщения от клиента
        public Thread thread { get; set; }
        //класс обрабатывающий входящие сообщения от клиента
        public HandleClientHelper a { get; set; }
        
        public bool deleteStatus { get; set; }

        public Client(Socket socket)
        {
            this.socket = socket;
        }
    }
}
