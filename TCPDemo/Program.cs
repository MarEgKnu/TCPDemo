// See https://aka.ms/new-console-template for more information
using System.Net.Sockets;
using TCPDemo;

Server server = new Server();
server.Start();

//Console.WriteLine(server.DoOneClient(c.AcceptTcpClient()));

Console.ReadKey();
