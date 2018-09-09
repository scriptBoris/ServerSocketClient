using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace Client.Models
{
    [DataContract]
    [Serializable]
    public class Message
    {
        [DataMember]
        public static JsonTypes JsonType = JsonTypes.Message;
        [DataMember]
        public string Id;
        [DataMember]
        public string Name;
        [DataMember]
        public string Text;
        [DataMember]
        public string Receiver;
    }
}
