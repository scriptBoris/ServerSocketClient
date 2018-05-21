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
            var sv = new Server("127.0.0.1", 8000);
            Console.WriteLine("Server are ready");



            string input = Console.ReadLine();
            while (input != "qq")
            {
                var msg = new Message { Name = "Server", };
                var split = input.Split('_');

                if (split != null && split.Length >= 2)
                {
                    try
                    {
                        var receiver = Server.connects.Find(x => x.name == split[0]);

                        msg.Receiver = receiver.name;
                        msg.Text = split[1];

                        receiver.SendMessage(msg);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Not found user: " + split[0]);
                    }
                }
                else
                {
                    msg.Name = "Server";
                    msg.Text = input;
                    foreach (var cl in Server.connects)
                    {
                        if (cl.name == null)
                            //continue;
                        cl.SendMessage(msg);
                    }
                }
                //var connect = sv.connects[0];
                //sv.SendMessage(connect.tcpClient, input);

                input = Console.ReadLine();
            }

            sv.Shutdown();
            Console.WriteLine("Shutdown server");
            Thread.Sleep(500);
        }
    }
}
