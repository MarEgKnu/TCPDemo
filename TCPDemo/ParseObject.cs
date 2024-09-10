using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPDemo
{
    public class ParseObject
    {
        public string Command { get; set; }

        public int Index { get; set; }

        //public List<string?> PropertyName { get; set; }

        public Dictionary<string, string> PropertyData { get; set; }

        //public List<string?> Data { get; set; }

       
    }
}
