using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPDemo
{
    public abstract class Node
    {
        public static readonly Dictionary<string, Func<double, double, double>> MappedTokens = new Dictionary<string, Func<double, double, double>>
        {
            {"add",  (double a, double b) => a+b },
            {"divide", (double a, double b) => a/b },
            {"subtract", (double a, double b) => a-b },
            {"multiply", (double a, double b ) => a*b },
            {"power", (double a, double b) => Math.Pow(a, b) }
        };

        public abstract double CalculateResult();
    }
}
