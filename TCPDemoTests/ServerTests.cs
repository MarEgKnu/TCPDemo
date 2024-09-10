using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCPDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace TCPDemo.Tests
{
    [TestClass()]
    public class ServerTests
    {
        [TestMethod()]
        public void CountWordsTest_3Words()
        {
            int result = Server.CountWords("lol lol lol");
            Assert.AreEqual(3, result);
        }
        [TestMethod()]
        public void CountWordsTest_4Words_2SpacesAndTrailing()
        {
            int result = Server.CountWords(" lol  lol  lol  LOL ");
            Assert.AreEqual(4, result);
        }
        [TestMethod()]
        public void CountWordsTest_0Words_SpacesOnly()
        {
            int result = Server.CountWords("     ");
            Assert.AreEqual(0, result);
        }
        [TestMethod()]
        public void CountWordsTest_3Words_NewLines()
        {
            int result = Server.CountWords("\nLOL\nLol\nLol\n");
            Assert.AreEqual(3, result);
        }
        [TestMethod()]
        public void CountWordsTest_3Words_Tab()
        {
            int result = Server.CountWords("\tLOL\tLol\tLol\t");
            Assert.AreEqual(3, result);
        }
        [TestMethod()]
        public void CountWordsTest_0Words_NewlinesSpacesTab()
        {
            int result = Server.CountWords("\n\n\n  \n \t\t ");
            Assert.AreEqual(0, result);
        }

        //[TestMethod()]
        //public async void DoOneClientTest()
        //{
        //    Server server = new Server();

        //    await server.Start();
        //    TcpClient tcpClient = new TcpClient();


        //}

        //[TestMethod()]
        //public void StartTest()
        //{
        //    Assert.Fail();
        //}
    }
}