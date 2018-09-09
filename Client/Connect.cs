using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Client.Models;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using Server;

namespace Client
{
    public enum RequestType
    {
        Name,
        Message,
    }

    public class Connect
    {
        public User User;
        public RequestType request;

        public readonly string url;
        public readonly int port;

        private bool _networkEnabled;
        private TcpClient _server;
        private StreamReader _read;
        private StreamWriter _write;
        private System.Timers.Timer _timer;
        private System.Timers.Timer _timerRegister;

        public NetworkDataHandler EventUpdate;
        public delegate void NetworkDataHandler(object sender, EventNetworkUpdate e);

        //private Socket    _socket;

        public Connect(string url, int port, User user)
        {
            //_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //_socket.Connect(IPAddress.Parse(url), port);
            User = user;
            _server = new TcpClient();

            try
            {
                _server.Connect(url, port);
            }
            catch (SocketException)
            {
                Console.WriteLine($"Не удалось подключится к {url}: {port}");
                return;
            }

            if (_server.Connected)
            {
                _networkEnabled = true;
                _read = new StreamReader(_server.GetStream());
                _write = new StreamWriter(_server.GetStream());

                _timer = new System.Timers.Timer(25);
                _timer.Elapsed += DataEngine;
                _timer.Start();

                var registerData = new RegisterData { Name = user.Name };
                SendData(registerData);

                EventUpdate += ReceiveMessage;
                EventUpdate += TryRegister;
            }
        }

        public void TryRegister(object obj, EventNetworkUpdate e)
        {
            dynamic get = JsonConvert.DeserializeObject(e.Data);
            if ((JsonTypes)get.JsonType == JsonTypes.RegisterData)
            {
                var response = JsonConvert.DeserializeObject<RegisterData>(e.Data);

                if (response.Code == 0)
                {
                    User.Id = response.Id;
                    ConsoleExtension.PrintText("Регистрация на сервере успешно произведена.");
                }
                else
                {
                    ConsoleExtension.PrintError("Не удалось зарегистрироваться на сервере: " + response.Description);
                }
                EventUpdate -= TryRegister;
            }
        }

        public void SendData(object obj)
        {
            if (_networkEnabled == false)
            {
                ConsoleExtension.PrintError("Не удалось отправить сообщение. Соединение с сервером отсутствует.");
                return;
            }

            string json = JsonConvert.SerializeObject(obj);
            //string length = NetworkExtension.GetDataLength(json);
            //byte[] buffer = new byte[json.Length + length.Length];

            //buffer = Encoding.ASCII.GetBytes(length + json);
            //_stream.Flush();
            //_stream.Write(buffer, 0, 5 + json.Length);
            //_stream.Flush();

            _write.WriteLine(json);
            _write.Flush();
        }

        public void DataEngine(object obj, EventArgs e)
        {
            if (_networkEnabled == false) return;

            string data = null;

            try
            {
                data = GetData(_read);
                if (data == null)
                    return;
                else if (data == "") //TODO Не нашел лучшего способа определения диссконекта, когда сервер использует программное отключение
                    ThrowDisconnect("Разрыв соединения с сервером");
                else
                    EventUpdate?.Invoke(this, new EventNetworkUpdate(data));
            }
            catch (IOException)
            {
                ThrowDisconnect("Разрыв соединения с сервером");
            }
        }

        /// <summary>
        ///     Отключает клиент от сервера
        /// </summary>
        private void ThrowDisconnect(string msg)
        {
            if (_networkEnabled == false) return;

            _networkEnabled = false;
            _timer.Stop();
            _timer.Dispose();

            _server.Close();

            _read.Close();
            _read.Dispose();

            _write.Close();
            _write.Dispose();

            ConsoleExtension.PrintError(msg);
        }

        public void ReceiveMessage(object obj, EventNetworkUpdate e)
        {
            dynamic get = JsonConvert.DeserializeObject(e.Data);
            if ((JsonTypes)get.JsonType == JsonTypes.Message)
            {
                var msg = JsonConvert.DeserializeObject<Message>(e.Data);
                ConsoleExtension.PrintMessage(msg.Name, msg.Text);
            }
        }

        private string GetData(StreamReader stream)
        {
            var builder = new StringBuilder();
            builder.Append(stream.ReadLine());

            return builder.ToString();
        }
    }
}
