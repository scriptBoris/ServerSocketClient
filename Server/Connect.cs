using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Server.Models;

namespace Server
{
    public class Connect
    {
        private TcpClient _tcpClient;
        private NetworkStream _stream;
        private Thread _thread;

        public Connect(TcpClient client)
        {
            _tcpClient = client;
            _stream = client.GetStream();

            _thread = new Thread(GetDataEngine);
            _thread.Start();
        }

        public void SendMessage(string text)
        {
            byte[] buffer = new byte[text.Length];
            buffer = Encoding.ASCII.GetBytes(text);
            _tcpClient.Client.Send(buffer);
        }

        public void SendMessage(Message msg)
        {
            var json = new DataContractJsonSerializer(typeof(Message));
            json.WriteObject(_stream, msg);
        }

        public void SendData(Object obj)
        {

            var fields = new List<FieldInfo>(obj.GetType().GetFields());
            foreach (var field in fields)
            {
                if (field != null && field.Name == "JsonType")
                {
                    var sJson = (JsonTypes)field.GetValue(obj);
                    var iJson = (int)sJson;
                    var json = new JavaScriptSerializer().Serialize(obj);

                    byte[] buffer = new byte[json.Length];
                    buffer = Encoding.ASCII.GetBytes(iJson + json);
                    _tcpClient.Client.Send(buffer);

                    return;
                }
            }
            throw new Exception("SendData not found field with JSON enum");
        }

        private void EventSimple(Message msg)
        {

        }

        private void Gets(Object o)
        {

        }

        private void GetDataEngine()
        {
            int length;
            byte[] buffer = new byte[1024];

            while (_tcpClient.Connected)
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
