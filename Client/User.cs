using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class User
    {
        /// <summary>
        ///     GUID
        /// </summary>
        public string Id { get; set; }
        public string Name { get; }

        public User(string name)
        {
            Name = name;
        }
    }
}
