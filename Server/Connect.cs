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
using System.Timers;
using System.Net;

namespace Server
{
    public class Connect
    {
        private string _ip;
        private TcpClient _tcpClient;
        private NetworkStream _stream;
        private Thread _thread;
        private System.Timers.Timer _timer;

        public Connect(TcpClient client)
        {
            _tcpClient = client;
            _stream = client.GetStream();

            _thread = new Thread(GetDataEngine);
            _thread.Start();

            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += PingConnect;
            _timer.Start();

            _ip = ((IPEndPoint)_tcpClient.Client.RemoteEndPoint).Address.ToString();
            Console.WriteLine(_ip+" has been connected");
        }

        public void Disconnect()
        {
            _timer.Stop();
            _timer.Dispose();
            _tcpClient.Client.Disconnect(false);
            _tcpClient.Client.Dispose();
            _tcpClient.Close();
            _stream.Close();
            _stream.Dispose();
            Server.connects.Remove(this);
            Console.WriteLine(_ip+" has been disconnected");
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

        /// <summary>
        /// Отправляем клиентам 1 бит (ASCII строку "!") и таким образом убеждаемся что связь не потеряна.
        /// TIMER - срабатывает с переодическим интервалом!
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private void PingConnect(Object obj, EventArgs e)
        {
            try
            {
                string ping = "!";
                byte[] buffer = new byte[1];
                buffer = Encoding.ASCII.GetBytes(ping);
                _tcpClient.Client.Send(buffer);
            }
            catch (Exception)
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Цикл, в котором читаем стрим и таким образом узнаем о входящих данных.
        /// </summary>
        private void GetDataEngine()
        {
            int length;
            byte[] buffer = new byte[1024];

            while (_tcpClient.Connected)
            {
                try
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
                    }
                }
                catch (Exception)
                {
                    
                }
            }
        }
    }
}
