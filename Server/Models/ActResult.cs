using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    [Serializable]
    public class ActResult
    {
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public string Description { get; set; }

        public ActResult(int code, string desc)
        {
            Code = code;
            Description = desc;
        }
    }
}
