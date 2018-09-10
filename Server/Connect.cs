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
using Server.Chat;

namespace Server
{
    public class Connect
    {
        public Client client;
        private string _ip;

        private TcpClient _tcpClient;
        private StreamReader _read;
        private StreamWriter _write;
        private System.Timers.Timer _timer;
        private System.Timers.Timer _timerRegister;

        public delegate void NetworkDataHandler(object sender, EventNetworkUpdate e);
        public event NetworkDataHandler EventUpdate;

        public bool IsConnected { get; private set; }

        public Connect(TcpClient client)
        {
            IsConnected = true;
            _tcpClient = client;

            _read = new StreamReader(client.GetStream() );
            _write = new StreamWriter(client.GetStream() );

            _timer = new System.Timers.Timer(25);
            _timer.Elapsed += GetDataEngine;
            _timer.Start();


            _timerRegister = new System.Timers.Timer(5000);
            _timerRegister.Elapsed += WaitingRegister;
            _timerRegister.Start();

            this.client = new Client();
            EventUpdate += GetDataRegister;

            _ip = ((IPEndPoint)_tcpClient.Client.RemoteEndPoint).Address.ToString();
            //ConsoleExtension.PrintText(_ip + " подключился");
        }

        public void Disconnect(string msg)
        {
            if (IsConnected == false) return;

            IsConnected = false;
            _timer.Stop();
            _timer.Dispose();

            if (_timerRegister.Enabled)
                _timerRegister.Stop();
            _timerRegister.Dispose();

            _tcpClient.Close();

            _read.Close();
            _read.Dispose();
            _write.Close();
            _write.Dispose();

            TcpServer.Connections.Remove(this);

            if (msg != null)
                ConsoleExtension.PrintText($"{client.Name ?? _ip}" + " " + msg);
        }

        public void SendData(dynamic obj)
        {
            string json = JsonConvert.SerializeObject(obj);

            _write.WriteLine(json);
            _write.Flush();
        }
        
        /// <summary>
        ///     Чтение входящих данных
        /// </summary>
        private void GetDataEngine(object obj, EventArgs e)
        {
            if (IsConnected == false) return;

            try
            {
                string data = GetData(_read);

                if (data == null)
                    return;
                else
                {
                    // Запускаем событие
                    EventUpdate?.Invoke(this, new EventNetworkUpdate(data));
                }
            }
            catch (IOException)
            {
                Disconnect("разрыв соединения.");
            }
        }

        // Above level interaction to TCP/IP socket
        private string GetData(StreamReader stream)
        {
            var builder = new StringBuilder();
            builder.Append(stream.ReadLine() );

            return builder.ToString();
        }

        private void GetDataRegister(object obj, EventNetworkUpdate e)
        {
            dynamic get = JsonConvert.DeserializeObject(e.Data);
            if ((JsonTypes)get.JsonType == JsonTypes.RegisterData)
            {
                var data = JsonConvert.DeserializeObject<RegisterData>(e.Data);

                if (data.Name == null)
                    return;

                var conflict = TcpServer.Connections.Find(x => x.client.Name == data.Name);
                if (conflict == null)
                {
                    client.Name = data.Name;
                    client.Id = Guid.NewGuid().ToString();
                    SendData(new RegisterData { Id = client.Id, Code = 0});

                    ConsoleExtension.PrintText(data.Name + " зарегистрирован на сервере.");
                }
                else
                {
                    SendData(new RegisterData { Code = 1, Description = "пользователь с таким именем уже существует."} );
                    Disconnect(null);
                }
                EventUpdate -= GetDataRegister;

            }
        }

        private void WaitingRegister(object obj, EventArgs e)
        {
            if (client.Id == null)
                Disconnect("разрыв соединения с не зарегистрированным клиентом.");
            _timerRegister.Stop();

            EventUpdate -= GetDataRegister;
        }
    }
}
