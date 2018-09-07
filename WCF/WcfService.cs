using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCF
{
    public class WcfService : IWcfService
    {
        public bool SetContent(string content)
        {
            return true;
        }

        public string Ping()
        {
            return "Hello world";
        }

        public string Echo(string echo)
        {
            return echo;
        }
    }
}
