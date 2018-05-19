using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Main(string[] args)
        {
            socket.Connect(IPAddress.Parse("127.0.0.1"), 8000);
            Console.WriteLine("Client are ready");

            string text = Console.ReadLine();
            byte[] buffer = Encoding.ASCII.GetBytes(text);

            socket.Send(buffer);
            Console.ReadLine();
        }
    }
}
