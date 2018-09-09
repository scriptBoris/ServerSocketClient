using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    static public class NetworkExtension
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

        static public string GetDataLength(string json)
        {
            int length = json.Length;
            if (length > 10000)
            {
                return null;
            }
            var str = new StringBuilder(5);
            var lengthStr = new StringBuilder(length.ToString() );

            for (int i = 5; (lengthStr.Length < i); i--)
            {
                str.Append('0');
            }
            str.Append(lengthStr);

            return str.ToString();
        }
    }
}
