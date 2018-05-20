using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Server.Models;

namespace Client
{
    class Program
    {
        //static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Main(string[] args)
        {
            string input = "";
            byte[] buffer;

            Console.WriteLine("Write your name");
            User user = new User(Console.ReadLine());

            try
            {
                var connect = new Connect("localhost", 8000, user);
                Console.WriteLine("Client are ready");

                input = Console.ReadLine();
                while (input != "qq")
                {
                    var message = new Message { Name = user.name };

                    var split = input.Split('_');

                    if (split != null  &&  split.Length > 1)
                    {
                        message.Receiver = split[0];
                        message.Text = split[1];
                        connect.SendMessage(message);
                    }
                    else
                    {
                        message.Text = input;
                        connect.SendMessage(message);
                    }

                    input = Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Failed connect, try again");
                Console.ReadKey();
                Main(null);
            }
        }
    }
}
