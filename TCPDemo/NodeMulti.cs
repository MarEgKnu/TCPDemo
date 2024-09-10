using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TCPDemo
{
    public class NodeMulti : Node
    {

        public Node Node1 { get; set; }

        public Node Node2 { get; set; }

        public string Type { get; set; }
        public override double CalculateResult()
        {
            double result = 0;
            result = MappedTokens[Type](Node1.CalculateResult(), Node2.CalculateResult());
            return result;
        }
        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is NodeMulti))
            {
                return false;
            }
            NodeMulti nodeMulti = (NodeMulti)obj;
            if (nodeMulti.Type == Type && 
                nodeMulti.Node1.Equals(Node1) &&
                nodeMulti.Node2.Equals(Node2))
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
