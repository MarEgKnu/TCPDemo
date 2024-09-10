using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TCPDemo
{
    public class Server
    {

        private const int PORT = 7;
        private static readonly HashSet<string> AllowedTokens = new HashSet<string>
        {
            { "add" },
            {"divide" },
            {"subtract" },
            {"multiply" }

        };
        private static readonly Dictionary<string, Func<int, int, int>> MappedTokens = new Dictionary<string, Func<int, int, int>>
        {
            {"add",  (int a, int b) => a+b },
            {"divide", (int a, int b) => a/b },
            {"subtract", (int a, int b) => a-b },
            {"multiply", (int a, int b ) => a*b }
        };

        public static int CountWords(string str)
        {
            string[] split = str.Split(new char[] { ' ', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            return split.Length;
        }
        public static string SplitWords(string str)
        {
            string result = "";
            string[] split = str.Split(new char[] { ' ', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in split)
            {
                result += s + "\n";
            }
            return result;
        }
        public static string[] SplitWordsArray(string str)
        {
            string[] split = str.Split(new char[] { ' ', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            return split;
        }

        public int? ParseExpression(string[] expression)
        {
            if (expression == null)
            {
                return null;
            }
            if (MappedTokens.ContainsKey(expression[0].ToLower()) && expression.Length >= 3)  
            {
                try
                {
                    int result = MappedTokens[expression[0].ToLower()](int.Parse(expression[1]), int.Parse(expression[2]));
                    return result;

                    //switch (expression[0].ToLower())
                    //{
                    //    case "add":
                    //        return int.Parse(expression[1]) + int.Parse(expression[2]);

                    //    case "divide":
                    //        return int.Parse(expression[1]) / int.Parse(expression[2]);
                    //    case "subtract":
                    //        return int.Parse(expression[1]) - int.Parse(expression[2]);
                    //    case "multiply":
                    //        return int.Parse(expression[1]) * int.Parse(expression[2]);
                    //    default:
                    //        throw new FormatException($"Invald format for {expression[0]} arguments");

                    //}
                    
                }
                catch (FormatException e)
                {
                    throw new FormatException($"Invald format for {expression[0]} arguments");
                }
                
                
            }
            else
            {
                return null;
            }
            
            
        }

        public string? DoOneClient(TcpClient socket)
        {
           

            // åbner for tekst strings
            StreamReader sr = new StreamReader(socket.GetStream());
            StreamWriter sw = new StreamWriter(socket.GetStream());

            // læser linje fra nettet
            string l = sr.ReadLine();

            //Console.WriteLine("Modtaget " + l);

            // skriver linje tilbage
            //sw.WriteLine(l?.ToUpper());
            //int? resultOfExpression = null;
            //try
            //{
            //    resultOfExpression = ParseExpression(SplitWordsArray(l));
            //    if (resultOfExpression != null)
            //    {
            //        sw.Write($"Result: {resultOfExpression}");
            //    }
            //    else
            //    {
            //        sw.Write(l);
            //    }
            //}
            //catch (FormatException e)
            //{
            //    sw.Write(e.Message);
            //}
            //catch (DivideByZeroException e)
            //{
            //    sw.Write("Cannot divide by zero");
            //}

            //string _ = ExpressionParser.ParseNodeMulti(ExpressionParser.TrimExpression(l), out Node result);
            //if (result != null)
            //{
            //    sw.Write(result.CalculateResult());
            //}
            //else
            //{
            //    sw.Write(l);
            //}

            // if an argumentexception is caught, that means the parsing has failed, catch it and write the error message to the stream
            try
            {
                // trim the input string before it is passed to the method
                ParseObject? parseObj = CommandParser.ParseCommand(ExpressionParser.TrimExpression(l));
                Person? person = PersonRepo.ProcessObject(parseObj);
                if (person == null)
                {
                    sw.Write("Could not find the given index, or syntax is invalid");
                }
                else
                {
                    sw.Write("Command executed sucessfully");
                }


            }
            catch (ArgumentException e)
            {
                sw.Write(e.Message);
            }

            sw.Flush();

            sr?.Close();
            sw?.Close();

            if (l == null)
            {
                return null;
            }
            //return CountWords(l);
            //return l.ToUpper();
            return SplitWords(l);

        }
        public void Start()
        {
            // definerer server med port num
            TcpListener server = new TcpListener(PORT);
            server.Start();
            Console.WriteLine($"Server Started på port {PORT}");
            // venter på client

            while (true)
            {
                TcpClient socket = server.AcceptTcpClient();
                Task.Run(() =>
                {
                    TcpClient tempSocket = socket;
                    Console.WriteLine(DoOneClient(socket));
                }

                );
            }
            
            server.Stop();

        }
    }
}
