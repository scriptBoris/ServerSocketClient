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
            while(input != "qq")
            {
                var split = input.Split('_');
                if (split != null && split.Length>=2)
                {
                    try
                    {
                        string msg = "";
                        for (int i = 1; i <= split.Length - 1; i++) msg += split[i] + " ";
                        var receiver = sv.connects.Find(x => x.name == split[0]);
                        receiver.SendMessage(msg);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
                else
                {
                    //try
                    //{
                    //    string msg = "";
                    //    for (int i = 1; i <= split.Length - 1; i++) msg += split[i] + " ";
                    //    var receiver = sv.connects.Find(x => x.name == split[0]);
                    //    receiver.SendMessage(msg);
                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine("Error: " + ex.Message);
                    //}
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
