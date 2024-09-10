using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TCPDemo
{
    public class ExpressionParser
    {
        public static readonly HashSet<char> Digits = new HashSet<char>()
        {
            {'0'}, {'1'},{'2'},{'3'},{'4'}, {'5'}, {'6' }, {'7'}, {'8'}, {'9'}
        };
        public static readonly char[] DigitsAndParanthesies = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '(' };
        
        private static readonly string RegexSubNode = "([0-9]+\\.[0-9]+|[0-9]+),([0-9]+\\.[0-9]+|[0-9]+)";
        private static readonly string RegexNode = "";

        private static readonly char[] Alpha = "aAbBcCdDeEfFgGhHiIjJkKlLmMnNoOpPqQrRsStTuUvVwWxXyYzZ".ToCharArray(); 

        public static string TrimExpression(string expression)
        {
            if (expression == null)
            {
                return null;
            }
            return expression
                .Replace(" ", "")
                .Replace("\n", "")
                .Replace("\t", "")
                .Replace("\r", "")
                .ToLower();
        }

        //public static Node ParseNode(string input)
        //{
        //    Node node = new Node();
        //    node.Nodes = new List<Node>();
        //    string remainder = ParseCharExcept(input, DigitsAndParanthesies, -1, out string result);
        //    if (!string.IsNullOrEmpty(result))
        //    {
        //        node.Type = result;
        //        remainder = ParseChar(remainder, new char[] { '(' }, 1, out result);
        //        if (!string.IsNullOrEmpty(result))
        //        {
        //            node.Nodes.Add(ParseNode(remainder));
        //        }
        //    }
        //    else
        //    {
        //        remainder = ParseIntOrDouble(remainder, -1, out string result1);
        //        if (!string.IsNullOrEmpty(result1))
        //        {
        //            remainder = ParseChar(remainder, new char[] { ',' }, -1, out result);
        //            remainder = ParseIntOrDouble(remainder, -1, out string result2);
        //            remainder = ParseChar(remainder, new char[] { ')' }, -1, out result);
        //            node.Nodes = new List<Node>()
        //            {
        //                new Node() { Value = double.Parse(result1, NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo) },
        //                new Node() { Value = double.Parse(result2, NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo) }

        //            };
        //        }
        //    }
        //    return node;
              
        //}

        public static string ParseNodeMulti(string str, out Node result)
        {
            string remainder = str;
            remainder = ParseNodeSingle(str, out Node subResult1);
            // if the result node is null, it means it can't be parsed as single node.
            // if it is not null, it has been sucessfully parsed, so return
            if (subResult1 != null)
            {

                //remainder = ParseNodeMulti(remainder, out Node subResult2);
                result = subResult1;
                return remainder;
            }
            //try parsing as pure multi with brackets
            else
            {
                // try to parse a comma, if not possible then continue, otherwise parse it
                try
                {
                    remainder = ParseChar(str, new char[] { ',' }, 1, out string _);
                }
                catch (ArgumentException)
                {
                    remainder = str;
                }
                // parses characters up untill the first bracket
                remainder = ParseCharExcept(remainder, new char[] { '(' }, -1, out string resultStr);
                // checks if the parsed string is a valid token, if it isn't then return null
                if (!Node.MappedTokens.ContainsKey(resultStr))
                {
                    result = null;
                    return str;
                }
                // check if the parsing throws any argumentexceptions, if so return null
                try
                {
                    // parses the first bracket
                    remainder = ParseChar(remainder, new char[] { '(' }, 1, out _);
                    // recursively calls itself to parse the next 2 bits, either a single or multi node
                    remainder = ParseNodeMulti(remainder, out subResult1);
                    remainder = ParseNodeMulti(remainder, out Node subResult2);
                    // check if the returns are valid, if not return null
                    if (subResult2 == null || subResult1 == null)
                    {
                        result = null;
                        return str;
                    }
                    result = new NodeMulti() { Type = resultStr, Node1 = subResult1, Node2 = subResult2 };
                    // parses the end bracket
                    remainder = ParseChar(remainder, new char[] { ')' }, 1, out _);
                    return remainder;
                }
                catch (ArgumentException)
                {
                    result = null;
                    return str;
                }
                
            }
        }

        public static string ParseNodeSingle(string str, out Node result )
        {
            string remainder = str;
            result = null;
            // try to parse a comma, if not possible then continue, otherwise parse it
            try
            {
                remainder = ParseChar(str, new char[] { ',' }, 1, out string _);
            }
            catch (ArgumentException)
            {
                remainder = str;
            }
            try
            {
                remainder = ParseIntOrDouble(remainder, -1, out string parseResult);
                // if the parse result is empty or null, that means nothing was parsed
                if (string.IsNullOrEmpty(parseResult))
                {
                    // return input as remainder, indicating nothing was parsed
                    return str;
                }
                // if something was parsed, convert it to double and create new Node obj with the parsed number in it
                double parsedNum = DoubleHelper.Parse(parseResult);
                result = new NodeSingle() { Value = parsedNum };
                return remainder;

            }
            // if it throws an argumentexception that means the parsing has failed, return the input
            catch (ArgumentException ex)
            {
                return str;
            }
        }

        public static string ParseIntOrDouble(string input, int amount, out string result)
        {
            string remainder = input;
            result = "";
            if (input == null)
            {
                return null;
            }
            bool hasSeperator = false;
            bool hasMinus = false;
            foreach (char c in input)
            {
                if (amount == 0)
                {
                    break;
                }
                if (Digits.Contains(c))
                {
                    result += c;
                    amount--;
                }
                else if ((c == '.' && hasSeperator == true) ||
                         (c == '-' && hasMinus == true))
                {
                    break;
                }
                else if (c == '.')
                {
                    result += c;
                    amount--;
                    hasSeperator = true;
                }
                else if (c == '-')
                {
                    result += c;
                    amount--;
                    hasMinus = true;
                }
                else if (amount < 0 && !Digits.Contains(c))
                {
                    break;
                }
                else
                {
                    throw new ArgumentException($"Could not parse the string: {input}");
                }
            }
            // double check its legit
            bool canBeParsed = DoubleHelper.TryParse(result, out double p) ||
                int.TryParse(result, out int s);
            if (!canBeParsed)
            {
                result = "";
                return remainder;
            }
            remainder = input.Remove(0, result.Length);
            return remainder;
            
        }
        /// <summary>
        /// Parses a number of characters from a string, the allowed characters are input from parameter
        /// </summary>
        /// <param name="input">The amount of characters to be parsed, if negative its as many as possible</param>
        /// <param name="chars"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        /// 
        public static string ParseChar(string input, char[] chars, int amount, out string result)
        {
            result = "";
            if (input == null || chars == null)
            {
                return null;
            }
            foreach (char c in input)
            {
                if (amount == 0)
                {
                    break;
                }
                if (chars.Contains(c)) 
                {
                    result += c;
                    amount--;
                }
                else if (amount < 0 && !chars.Contains(c))
                {
                    break;
                }
                else
                {
                    throw new ArgumentException($"Could not parse the string: {input}");
                }
            }
            string remainder = input.Remove(0, result.Length);
            return remainder;
        }


        public static string ParseCharExcept(string input, char[] chars, int amount, out string result)
        {
            result = "";
            if (input == null || chars == null)
            {
                return null;
            }
            foreach (char c in input)
            {
                if (amount == 0)
                {
                    break;
                }
                if (!chars.Contains(c))
                {
                    result += c;
                    amount--;
                }
                else if (amount < 0 && chars.Contains(c))
                {
                    break;
                }
                else
                {
                    throw new ArgumentException($"Could not parse the string: {input}");
                }
            }
            string remainder = input.Remove(0, result.Length);
            return remainder;

            //public static string ParseString(string input, string pattern)
            //{
            //    if (input == null || pattern == null)
            //    {
            //        return null;
            //    } 
            //    string result = "";
            //    foreach (char c in input)
            //    {
            //        if (amount == 0)
            //        {
            //            break;
            //        }
            //        if (chars.Contains(c))
            //        {
            //            result += c;
            //            amount--;
            //        }
            //        else if (amount > 0 && !chars.Contains(c))
            //        {
            //            break;
            //        }
            //        else
            //        {
            //            throw new ArgumentException($"Could not parse the string: {input}")
            //        }
            //    }
            //    return result

            //
        }
    }
}
