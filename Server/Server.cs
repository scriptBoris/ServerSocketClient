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
    public class TcpServer
    {
        public TcpListener listener;
        public static List<Connect> connects = new List<Connect>();

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

                _thread = new Thread(RegisterEngine);
                _thread.Start();
                _enabled = true;
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
            if (connects.Count > 0)
            {
                var msg = new Message { Name = "Сервер", Text = message};
                foreach (var connect in connects)
                    connect.SendData(msg);
            }
            else
                ConsoleExtension.PrintBadInput("Не удалось отправить сообщение. На сервере нет ни одного подключения.");
        }

        public void Shutdown()
        {
            ConsoleExtension.ClearLine();
            Console.ResetColor();
            Console.WriteLine("Выключение...");
            if (connects.Count > 0)
            {
                foreach (var connect in connects) { connect.Disconnect(); }
            }
            listener.Stop();
            _enabled = false;
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
                    connect.SendData(new Message { Text = "Добро пожаловать на сервер!", });
                    connects.Add(connect);
                }
                catch (SocketException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    ConsoleExtension.PrintError($"Оборвана попытка соединения: {ex.Message}");
                }
            }
        }
    }
}
