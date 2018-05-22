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
            var svSocket = new Server("127.0.0.1", 8000);
            var svWcf = new Wcf("http://localhost:9090");


            string input = Console.ReadLine();
            while (input != "qq")
            {
                var msg = new Message
                {
                    Name = "Server",
                    Text = input,
                };

                Server.connects[0].SendData(msg);
                input = Console.ReadLine();
            }

            svSocket.Shutdown();
            Thread.Sleep(500);
        }
    }
}
