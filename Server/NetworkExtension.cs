using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    static internal class NetworkExtension
    {
        static public string GetModel(byte[] buffer)
        {
            string l0 = Encoding.UTF8.GetString(new [] { buffer[0] });
            string l1 = Encoding.UTF8.GetString(new [] { buffer[1] });
            int length = Convert.ToInt32(l0 + l1);

            string model = "";
            for (int i = 2; i <= 18; i++)
            {
                model += Encoding.UTF8.GetString(new[] { buffer[i] });
            }
            return model;
        }
    }
}
