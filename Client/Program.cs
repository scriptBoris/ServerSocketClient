using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Client.Models;

namespace Client
{
    class Program
    {
        //static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Main(string[] args)
        {
            Console.WriteLine("Введите адрес подключения (например: \"192.168.1.28:8000\")");
            string adress = Console.ReadLine();
            Console.WriteLine("Введите ваше имя:");
            ConsoleExtension.UserName = Console.ReadLine();

            User user = new User(ConsoleExtension.UserName);
            var split = adress.Split(':');
            var connect = new Connect(split[0], Convert.ToInt32(split[1]), user);
            ConsoleExtension.ReadLineDrow();
            while (true)
            {
                string inputText = ConsoleExtension.ReadLine();
                if (inputText == "qq")
                {
                    return;
                }

                var message = new Message { Name = user.Name, Text = inputText, Id = user.Id };
                connect.SendData(message);

                //var split = inputText.Split('_');

                //if (split != null  &&  split.Length > 1)
                //{
                //    message.Receiver = split[0];
                //    message.Text = split[1];
                //    connect.SendData(message);
                //    // send data
                //}
                //else
                //{
                //    message.Text = inputText;
                //    connect.SendData(message);
                //    // send data
                //}
            }
        }
    }
}
