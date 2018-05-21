using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Server.Models;

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

            _thread = new Thread(GetDataEngine);
            _thread.Start();
        }

        public void SendMessage(string text)
        {
            byte[] buffer = new byte[text.Length];
            buffer = Encoding.ASCII.GetBytes(text);
            tcpClient.Client.Send(buffer);
        }
        public void SendMessage(Message msg)
        {
            var json = new DataContractJsonSerializer(typeof(Message));
            json.WriteObject(_stream, msg);
        }

        private void EventSimple(Message msg)
        {

            if (msg.Receiver != "")
            {
                try
                {
                    var receiver = Server.connects.Find(x => x.name == msg.Receiver);
                    receiver.SendMessage(msg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Not found user: " + msg.Receiver);
                }
            }
            else
            {
                Console.WriteLine(msg.Name+": "+msg.Text);
                foreach (var cl in Server.connects)
                {
                    if (cl.name == null || cl.name == msg.Name)
                        continue;
                    cl.SendMessage(msg);
                }
            }

        }

        private void GetDataEngine()
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

                    //// Convert byte array to string message. 		
                    string clientMessage = Encoding.ASCII.GetString(incommingData);
                    var msg = new Message();
                    var ms = new MemoryStream(Encoding.ASCII.GetBytes(clientMessage));
                    var ser = new DataContractJsonSerializer(msg.GetType());
                    msg = ser.ReadObject(ms) as Message;

                    EventSimple(msg);

                    //Console.WriteLine("Name: "+msg.Name+" Send: "+msg.Text);
                }
            }
        }
    }
}
