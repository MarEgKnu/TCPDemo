using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TCPDemo
{
    public static class CommandParser
    {
        // Dictionary to map the command (add, delete etc) to a method which will be called to parse the parameters for each command

        //Current command syntax:
        // add:    add, (property1, property2, property3): (data1, data2, data3)
        // get:    get, index
        // update: update, index, (property1, property2, property3): (data1, data2, data3)
        // delete: delete, index

        public static readonly Dictionary<string, Func<string,string, ParseObject?>> MappedCommands = new Dictionary<string, Func<string, string, ParseObject?>>()
        {
            {"add", ParseCommandAdd },
            {"delete", ParseCommandGetDelete },
            {"get", ParseCommandGetDelete },
            {"update", ParseCommandUpdate },

        };
        private static readonly char[] Alpha = "aAbBcCdDeEfFgGhHiIjJkKlLmMnNoOpPqQrRsStTuUvVwWxXyYzZ".ToCharArray();
        private static readonly char[] Comma = new char[] { ',' };
        private static readonly char[] CommaEndBracket = new char[] { ',', ')' };
        private static readonly char[] StartBracket = new char[] { '(' };
        private static readonly char[] EndBracket = new char[] { ')' };
        private static readonly char[] Semicolon = new char[] { ':' };

        public static ParseObject? ParseCommand(string input)
        {
            string command = null;
            string remainder = input;
            remainder = ExpressionParser.ParseCharExcept(input, Comma, -1, out command);
            if (!MappedCommands.ContainsKey(command))
            {
                throw new ArgumentException($"{command} is not a valid command");
            }
            remainder = ExpressionParser.ParseChar(remainder, Comma, 1, out string _);
            
            // if it throws an argumentexception, treat it as a parse failure and return null
            // index into the command dictionary, and make it call the appropriate method
            return MappedCommands[command](remainder, command);
           
        }
        public static string ParsePropertyList(string input, bool isData, out List<string> result)
        {
            string remainder = input;
            result = new List<string>();
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            while (true)
            {
                // parse a property, till it reaches a comma or a end bracket
                remainder = ExpressionParser.ParseCharExcept(remainder, CommaEndBracket, -1, out string propertyOrData);
                // if the thing being parsed is NOT property data, but rather a property name, check if it is a valid prop name
                // If its not a valid property name, return with nothing parsed
                if (!isData && !Person.ValidProperties.Contains(propertyOrData))
                {
                    result.Clear();
                    return input;
                }
                // add the property to list
                result.Add(propertyOrData);
                // parse the comma that is expected afterwards
                // if there is no comma, and it throws an exception, that means it is the end of the property block, so return
                try
                {
                    remainder = ExpressionParser.ParseChar(remainder, Comma, 1, out string _);
                }
                catch (ArgumentException)
                {
                    return remainder;
                }
                

            }
        }
        public static ParseObject? ParseCommandAdd(string input, string command)
        {
            List<string> propertyList = new List<string>();
            List<string> dataList = new List<string>();
            string remainder = input;
            // remainder = ExpressionParser.ParseChar(remainder, Comma, 1, out string _);
            // parse start bracket
            remainder = ExpressionParser.ParseChar(remainder, StartBracket, 1, out string _);
            // parse list of property names
            remainder = ParsePropertyList(remainder, false, out propertyList);
            // parse endbracket
            remainder = ExpressionParser.ParseChar(remainder, EndBracket, 1, out string _);
            // parse semicolon
            remainder = ExpressionParser.ParseChar(remainder, Semicolon, 1, out string _);
            // parse start bracket
            remainder = ExpressionParser.ParseChar(remainder, StartBracket, 1, out string _);
            // parse list of data
            remainder = ParsePropertyList(remainder, true, out dataList);
            remainder = ExpressionParser.ParseChar(remainder, EndBracket, 1, out string _);
            // turn the two lists into a dictionary of matching keys based on the index
            Dictionary<string, string> propData = propertyList.Zip(dataList, (k,v) => new {k,v}).ToDictionary(x => x.k, x => x.v);
            return new ParseObject() { Command = command, PropertyData = propData, Index = 0 };
        }
        public static ParseObject? ParseCommandUpdate(string input, string command)
        {
            List<string> propertyList = new List<string>();
            List<string> dataList = new List<string>();
            string index = null;
            string remainder = input;
            //remainder = ExpressionParser.ParseChar(remainder, Comma, 1, out string _);
            // parse index num
            remainder = ExpressionParser.ParseCharExcept(remainder, Comma, -1, out index);
            if (!int.TryParse(index, out int indexInt))
            {
                throw new ArgumentException($"Index {index} is not a valid index");
            }
            // parse comma
            remainder = ExpressionParser.ParseChar(remainder, Comma, 1, out string _);
            // parse start bracket
            remainder = ExpressionParser.ParseChar(remainder, StartBracket, 1, out string _);
            // parse list of property names
            remainder = ParsePropertyList(remainder, false, out propertyList);
            // parse endbracket
            remainder = ExpressionParser.ParseChar(remainder, EndBracket, 1, out string _);
            // parse semicolon
            remainder = ExpressionParser.ParseChar(remainder, Semicolon, 1, out string _);
            // parse start bracket
            remainder = ExpressionParser.ParseChar(remainder, StartBracket, 1, out string _);
            // parse list of data
            remainder = ParsePropertyList(remainder, true, out dataList);
            remainder = ExpressionParser.ParseChar(remainder, EndBracket, 1, out string _);
            // turn the two lists into a dictionary of matching keys based on the index
            Dictionary<string, string> propData = propertyList.Zip(dataList, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v);
            return new ParseObject() { Command = command, PropertyData = propData, Index = indexInt };
        }
        public static ParseObject? ParseCommandGetDelete(string input, string command)
        {
            string index = null;
            string remainder = input;
            //remainder = ExpressionParser.ParseChar(remainder, Comma, 1, out string _);
            // parse index num
            remainder = ExpressionParser.ParseCharExcept(remainder, Comma, -1, out index);
            if (!int.TryParse(index, out int indexInt))
            {
                throw new ArgumentException($"Index {index} is not a valid index");
            }
            
            return new ParseObject() { Command = command, PropertyData = null, Index = indexInt };
        }
    }
}
