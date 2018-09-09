using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var svSocket = new TcpServer("localhost", 8000);
            //var svWcf = new Wcf("http://"+outIP+outPort);
            ConsoleExtension.ServerName = "Сервер";
            Console.WriteLine("Сервер запущен\n");
            ConsoleExtension.ReadLineDrow();
            while (true)
            {
                string inputText = ConsoleExtension.ReadLine();

                if (inputText == null || inputText == "") continue;

                if (inputText == "qq")
                {
                    svSocket.Shutdown();
                    ConsoleExtension.ClearLine();
                    return;
                }

                // Send message
                svSocket.BroadcastMessage(inputText);
            }
        }
    }
}
