using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        private static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private int _port;
        private string _url;

        public Server(string ulr, int port)
        {
            socket.Bind(new IPEndPoint(IPAddress.Any, port));
            socket.Listen(10);
            var client = socket.Accept();
            Console.WriteLine("Register success connection on client");
            byte[] buffer = new byte[1024];
            client.Receive(buffer);
            Console.WriteLine(Encoding.ASCII.GetString(buffer));
            Console.ReadLine();
        }
    }
}
