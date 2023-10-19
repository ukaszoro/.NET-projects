using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Udp_file_transfer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args[0] == "send")
                Udp_sender.Send();
            if (args[0] == "listen")
               Udp_listener.StartListener(); 
        }
    }
}
