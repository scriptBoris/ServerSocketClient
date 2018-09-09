using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Server.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Server
{
    public class TcpServer
    {
        public TcpListener listener;
        public static List<Connect> Connections = new List<Connect>();

        private bool _enabled;
        private Thread _thread;
        private Socket _socket;


        public TcpServer(string url, int port)
        {
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                _enabled = true;
                _thread = new Thread(RegisterEngine);
                _thread.Start();
                Console.WriteLine($"Запуск Socket TCP/IP {url}:{port}... OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Запуск Socket TCP/IP {url}:{port}... Ошибка");
                Console.WriteLine($"{ex.Message}");
            }
        }

        public void BroadcastMessage(string message)
        {
            if (Connections.Count > 0)
            {
                var msg = new Message { Name = "Сервер", Text = message};
                foreach (var connect in Connections)
                    connect.SendData(msg);
            }
            else
                ConsoleExtension.PrintBadInput("Не удалось отправить сообщение. На сервере нет ни одного подключения.");
        }

        public void Shutdown()
        {
            ConsoleExtension.PrintText("Выключение");
            for (int i = Connections.Count; i>0; i--)
            {
                Connections[i-1].Disconnect("Отключен");
            }
            _enabled = false;
            listener.Stop();
            _socket.Close();
            _socket.Dispose();
            Thread.Sleep(100);
        }

        private void RegisterEngine()
        {
            while (_enabled)
            {
                try
                {
                    var tcp = listener.AcceptTcpClient();
                    var connect = new Connect(tcp);
                    connect.EventUpdate += ReceiveMessage;
                    Connections.Add(connect);
                }
                catch (SocketException) 
                {
                    // Исключение, когда выключается listener
                    return;
                }
                catch (Exception ex)
                {
                    ConsoleExtension.PrintError($"Оборвана попытка соединения: {ex.Message}");
                }
            }
        }

        private void ReceiveMessage(object obj, EventNetworkUpdate e)
        {
            dynamic get = JsonConvert.DeserializeObject(e.Data);
            if ((JsonTypes)get.JsonType == JsonTypes.Message)
            {
                var msg = JsonConvert.DeserializeObject<Message>(e.Data);

                if (msg.Id == null)
                    return;

                foreach(var connect in Connections)
                {
                    if (connect.client.Id == msg.Id)
                        continue;
                    
                    connect.SendData(msg);
                }
                ConsoleExtension.PrintMessage(msg.Name, msg.Text);
            }
        }
    }
}
