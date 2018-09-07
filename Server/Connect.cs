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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Server
{
    public class Connect
    {
        private string _ip;
        private TcpClient _tcpClient;
        private NetworkStream _stream;
        private System.Timers.Timer _timer;

        public Connect(TcpClient client)
        {
            _tcpClient = client;
            _stream = client.GetStream();

            _timer = new System.Timers.Timer(25);
            _timer.Elapsed += GetDataEngine;
            _timer.Start();

            _ip = ((IPEndPoint)_tcpClient.Client.RemoteEndPoint).Address.ToString();
            ConsoleExtension.PrintText(_ip + " подключился");
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
            TcpServer.connects.Remove(this);
            ConsoleExtension.PrintText(_ip + " отключился");
        }
        public void Disconnect(string msg)
        {
            _timer.Stop();
            _timer.Dispose();
            _tcpClient.Client.Disconnect(false);
            _tcpClient.Client.Dispose();
            _tcpClient.Close();
            _stream.Close();
            _stream.Dispose();
            TcpServer.connects.Remove(this);
            ConsoleExtension.PrintText(_ip + msg);
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
            ConsoleExtension.PrintError("Ошибка отправки данных. Отправляемый объект не имеет тип JsonType.");
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
        /// Извлекает данные из потока стрима
        /// </summary>
        /// <param name="data">Поток стрима представленный в виде строки</param>
        /// <param name="dataOut">Тело JSON в виде строки</param>
        /// <param name="transportDataOut">Системная строка, по ней определяют тип JSON объекта</param>
        private void GetDataType(string data, out string dataOut, out string transportDataOut)
        {
            int space = 0;
            string transportData = "";

            while (space < 99)
            {
                if (data[space] == '{')
                {
                    transportData = data.Substring(0,space);
                    break;
                }
                space++;
            }

            if (space > 0 && space < 99)
            {
                dataOut          = data.Substring(space);
                transportDataOut = transportData;
            }
            else if (space == 0)
            {
                Console.WriteLine("Не удалось получить JSON объект");
                dataOut          = "ERROR";
                transportDataOut = "ERROR";
            }
            else
            {
                Console.WriteLine("Не верные данные");
                dataOut          = "ERROR";
                transportDataOut = "ERROR";
            }
        }

        /// <summary>
        /// Цикл, в котором читаем стрим и таким образом узнаем о входящих данных.
        /// </summary>
        private void GetDataEngine(Object obj, EventArgs e)
        {
            const int fixLength = 52;
            byte[] buffer = new byte[fixLength];

            try
            {
                _stream.Read(buffer, 0, fixLength);
                //Array.Copy(buffer, 0, incFullData, 0, length);
                ConsoleExtension.PrintText(NetworkExtension.GetModel(buffer) );

                //dynamic jobj = JObject.Parse();
                //if (length > 0)
                //{
                //    string transportData = "";
                //    string jsonData   = "";
                //    GetDataType(stringData, out jsonData, out transportData);

                //    var msg = new JavaScriptSerializer().Deserialize<Message>(jsonData);
                //    Console.WriteLine($"{msg.Name}: {msg.Text}");
                //}
            }
            catch (Exception ex)
            {
                ConsoleExtension.PrintError($"Ошибка при попытке получить данные от {_ip}:");
                ConsoleExtension.PrintError(ex.Message);
            }
        }

        private void OnReceivedMessage(Message msgIn)
        {
            string text = $"{msgIn.Name}: {msgIn.Text}";
            Console.WriteLine(text);
        }

        private void ExtractData(JsonTypes jsonTypes, string data)
        {
            var obj = new JavaScriptSerializer().Deserialize<Message>(data);

            OnReceivedMessage(obj);
        }
    }
}
