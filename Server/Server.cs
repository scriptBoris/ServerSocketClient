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

        public Server(string url, int port)
        {
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                _thread = new Thread(RegisterEngine);
                _thread.Start();
                _enabled = true;
                Console.WriteLine($"Socket TCP/IP {url}:{port}... OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Socket TCP/IP {url}:{port}... FAIL");
                Console.WriteLine($"{ex.Message}");
            }
        }

        public void Shutdown()
        {
            if (connects.Count > 0)
            {
                foreach (var connect in connects) { connect.Disconnect(); }
            }
            listener.Stop();
            _enabled = false;
            _socket.Close();
            _socket.Dispose();
            Console.WriteLine("Shutdown...");
            Thread.Sleep(300);
        }

        private void RegisterEngine()
        {
            while (_enabled)
            {
                try
                {
                    var tcp = listener.AcceptTcpClient();

                    var connect = new Connect(tcp);
                    connect.SendData(new Message { Text = "Hello!", });
                    connects.Add(connect);
                }
                catch (Exception)
                {
                    Console.WriteLine("Server are abort input connect");
                }
            }
        }
    }
}
