using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WCF;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var svSocket = new TcpServer("localhost", 8000);
            //var svWcf = new Wcf("http://"+outIP+outPort);

            Console.WriteLine("Сервер запущен\n");
            Console.Write(">Сервер: ");
            Console.ForegroundColor = ConsoleColor.White;
            while (true)
            {
                string inputText = Console.ReadLine();
                Console.ResetColor();
                if (inputText == null || inputText == "") continue;

                if (inputText == "qq")
                {
                    svSocket.Shutdown();
                    return;
                }

                // Send message
                svSocket.BroadcastMessage(inputText);
            }
        }
    }
}
