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
    public class RegisterData
    {
        [JsonRequired]
        [DataMember]
        public static JsonTypes JsonType = JsonTypes.RegisterData;
        [DataMember]
        public string Name;
        [DataMember]
        public string Id;
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public string Description { get; set; }

    }
}
