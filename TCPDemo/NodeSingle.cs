using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TCPDemo
{
    public class NodeSingle : Node
    {
        public double Value { get; set; }
        public override double CalculateResult()
        {
            return Value;
        }
        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is NodeSingle))
            {
                return false;
            }
            NodeSingle nodeSingle = (NodeSingle)obj;
            if (nodeSingle.Value == Value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
