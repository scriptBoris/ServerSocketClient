using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Server.Models;
using System.Runtime.Serialization.Json;

namespace Client
{
    public enum RequestType
    {
        Name,
        Message,
    }

    public class Connect
    {
        public User user;
        public RequestType request;

        //public StringBuilder String = new StringBuilder();

        public readonly string url;
        public readonly int port;

        private bool _enabled = true;
        private TcpClient _server;
        private NetworkStream _stream;
        private Thread _thread;

        //private Socket    _socket;

        public Connect(string url, int port, User newUser)
        {
            //_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //_socket.Connect(IPAddress.Parse(url), port);
            user = newUser;
            _server = new TcpClient();
            _server.Connect(url, port);

            if (_server.Connected)
            {
                //_stream = _server.GetStream();
                //_thread = new Thread(DataEngine);
                //_thread.Start();
            }

            //string text = Console.ReadLine();
            //byte[] buffer = Encoding.ASCII.GetBytes(text);

            //socket.Send(buffer);
            //Console.ReadLine();
        }

        private void EventSimple(Message msg)
        {
            if (msg.Name == "Server")
            {
                if (msg.Text == "_reg")
                {
                    var msgOut = new Message
                    {
                        Name = user.name,
                        Text = user.name,
                    };
                    SendMessage(msgOut);
                    return;
                }
                else
                    Console.WriteLine("Server say: " + msg.Text);
            }
            else
            {
                Console.WriteLine(msg.Name + ": " + msg.Text);
            }
            //else if (input == "Server say: ")
            //{

            //}
        }

        public void SendMessage(string text)
        {
            byte[] buffer = new byte[text.Length];
            buffer = Encoding.ASCII.GetBytes(text);
            _server.Client.Send(buffer);
            ///_stream.Write(buffer, 0, text.Length);
        }
        public void SendMessage(object obj)
        {
            var json = new DataContractJsonSerializer(typeof(object));
            string data = 
            json.WriteObject(_stream, data);
            //byte[] buffer = new byte[json.Length];
        }

        public void DataEngine()
        {
            //while (_server.Connected && _enabled)
            //{
            //    if (_stream.DataAvailable)
            //    {
            //        Byte[] buffer = new Byte[1024];
            //        int length = _stream.Read(buffer, 0, buffer.Length);
            //        var incommingData = new byte[length];

            //        Array.Copy(buffer, 0, incommingData, 0, length);
            //        string serverMessage = Encoding.ASCII.GetString(incommingData);

            //        var msg = new Message();
            //        var ms = new MemoryStream(Encoding.ASCII.GetBytes(serverMessage));
            //        var ser = new DataContractJsonSerializer(msg.GetType());
            //        msg = ser.ReadObject(ms) as Message;

            //        EventSimple(msg);

            //        //Console.WriteLine("Server say: " + serverMessage);
            //    }
            //}
        }
    }
}
