using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Server.Models;

namespace Server
{
    public class Server
    {
        public TcpListener listener;
        public static List<Connect> connects = new List<Connect>();

        private bool   _enabled = true;
        private Thread _thread;

        private Socket _socket;

        //private TcpClient _client;
        //private static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //private int _port;
        //private string _url;

        public Server(string ulr, int port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //socket.Bind(new IPEndPoint(IPAddress.Any, port));
            //socket.Listen(2);
            //var client = socket.Accept();
            //Console.WriteLine("Register success connection on client");
            //byte[] buffer = new byte[1024];

            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            _thread = new Thread(RegisterEngine);
            _thread.Start();

        }

        public void Shutdown()
        {
            _enabled = false;
            //_thread.Abort();
        }

        //public void SendMessage(TcpClient client, string text)
        //{
        //    byte[] buffer = new byte[text.Length];
        //    buffer = Encoding.ASCII.GetBytes(text);

        //    client.Client.Send(buffer);
        //}

        private void RegisterEngine()
        {
            while (_enabled)
            {
                var client = listener.AcceptTcpClient();
                var connect = new Connect(client);

                var msg = new Message
                {
                    Name = "Server",
                    Text = "_reg",
                };
                connect.SendMessage(msg);

                connects.Add(connect);
            }
        }
    }
}
