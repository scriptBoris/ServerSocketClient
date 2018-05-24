using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Chat
{
    internal class Client : Chat
    {
        internal string _name;

        internal Client(string name)
        {
            _name = name;
            Clients.Add(this);
        }
    }
}
