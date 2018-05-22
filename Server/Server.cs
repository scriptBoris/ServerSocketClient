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

        private bool _enabled;
        private Thread _thread;

        private Socket _socket;

        //private TcpClient _client;
        //private static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //private int _port;
        //private string _url;

        public Server(string ulr, int port)
        {
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                _thread = new Thread(RegisterEngine);
                _thread.Start();
                _enabled = true;
                Console.WriteLine("Socket: OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Socket: FAIL\n"+ex.Message);
            }
        }

        public void Shutdown()
        {
            _enabled = false;
            _socket.Close();
            _socket.Dispose();
        }

        private void RegisterEngine()
        {
            while (_enabled)
            {
                var client = listener.AcceptTcpClient();
                var connect = new Connect(client);

                connect.SendData(new Message { Text = "Hello!", });

                connects.Add(connect);
            }
        }
    }
}
