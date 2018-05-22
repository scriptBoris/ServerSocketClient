using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Server.Models
{
    [DataContract]
    [Serializable]
    public class Message
    {
        public static JsonTypes JsonType = JsonTypes.Message;
        [DataMember]
        public int Request;
        [DataMember]
        public int Response;
        [DataMember]
        public string Name;
        [DataMember]
        public string Text;
        [DataMember]
        public string Receiver;
    }
}
