using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public enum ResponseTypes
    {
        Name,
        Message,
    }

    public class Connect
    {
        public string name;
        public ResponseTypes response;
        public TcpClient tcpClient;

        private NetworkStream _stream;
        private Thread _thread;

        public Connect(TcpClient client)
        {
            tcpClient = client;
            _stream = client.GetStream();

            _thread = new Thread(QueueEngine);
            _thread.Start();
        }

        public void SendMessage(string text)
        {
            byte[] buffer = new byte[text.Length];
            buffer = Encoding.ASCII.GetBytes(text);
            tcpClient.Client.Send(buffer);
        }

        private void EventSimple(string text)
        {
            if (response == ResponseTypes.Name)
            {
                name = text;
                response = ResponseTypes.Message;
                Console.WriteLine("New connected user: " + name);
            }
            else
            {
                Console.WriteLine(name + ": " + text);
            }
        }

        private void QueueEngine()
        {
            int length;
            byte[] buffer = new byte[1024];

            while (tcpClient.Connected)
            {
                length = _stream.Read(buffer, 0, buffer.Length);
                if (length > 0)
                {
                    var incommingData = new byte[length];
                    Array.Copy(buffer, 0, incommingData, 0, length);

                    // Convert byte array to string message. 		
                    string clientMessage = Encoding.ASCII.GetString(incommingData);
                    EventSimple(clientMessage);
                    //Console.WriteLine($"Client message: {clientMessage}");
                }
            }
        }
    }
}
