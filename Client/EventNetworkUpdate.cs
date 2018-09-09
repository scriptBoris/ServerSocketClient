using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class EventNetworkUpdate
    {
        public string Data { get; }

        public EventNetworkUpdate(dynamic data)
        {
            Data = data;
        }
    }
}
