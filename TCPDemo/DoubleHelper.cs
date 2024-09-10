using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPDemo
{
    public class DoubleHelper
    {
        /// <summary>
        /// Uses double.Parse with predetermined parameters to allow for normal negative sign and point decimals
        /// </summary>
        /// <param name="s">The string to parse to a double</param>
        /// <returns></returns>
        public static double Parse(string s)
        {
            NumberFormatInfo fmt = new NumberFormatInfo();
            fmt.NegativeSign = "-";
            fmt.NumberDecimalSeparator = ".";
            return double.Parse(s, fmt);
        }
        public static bool TryParse(string s, out double result)
        {
            NumberFormatInfo fmt = new NumberFormatInfo();
            fmt.NegativeSign = "-";
            fmt.NumberDecimalSeparator = ".";
            return double.TryParse(s, fmt, out result);
        }
    }
}
