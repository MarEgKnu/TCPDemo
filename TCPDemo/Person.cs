using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPDemo
{

    public class Person
    {
        public static HashSet<string> ValidProperties = new HashSet<string>()
        {
            "name", "address", "phone"
        };
        public string Name { get; set; }
        public string Address { get; set; }

        public string Phone { get; set; }
    }
}
