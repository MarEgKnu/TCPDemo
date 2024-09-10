using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCPDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TCPDemo.Tests
{
    [TestClass()]
    public class NodeTests
    {
        [DataRow(4)]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-1.5)]
        [DataRow(10.4)]
        [DataTestMethod]
        [TestMethod()]
        public void CalculateResultTest_Singles(double value)
        {
            double expected = value;
            Node testNode = new NodeSingle() { Value = value };

            double actual = testNode.CalculateResult();

            Assert.AreEqual(expected, actual);  
        }
        [DataRow(4, 5, "add", 9)]
        [DataRow(0, 7, "multiply", 0)]
        [DataRow(-1, 5, "divide", -0.2)]
        [DataRow(-10.5, 5 , "subtract", -15.5)]
        [DataRow(10.4, 9, "add", 19.4)]
        [DataTestMethod]
        [TestMethod()]
        public void CalculateResultTest_1LevelMulti(double value1, double value2, string type, double expected)
        {
            Node testNode = new NodeMulti()
            {
                Type = type,
                Node1 = new NodeSingle() { Value = value1 },
                Node2 = new NodeSingle() { Value = value2 },
            };

            double actual = testNode.CalculateResult();

            Assert.AreEqual(expected, actual);
        }
        [DataRow("add", "divide", 5.0, 2, "multiply", 19, -2, -35.5)]
        [DataRow("multiply", "add", 0, 2, "subtract", 10, -4.3, 28.6)]
        [DataRow("divide", "add", 13, 2, "multiply", 10, 20.0, 0.075)]
        [DataRow("subtract", "divide", 15, 3.0, "multiply", 9, 24, -211)]
        [DataTestMethod]
        [TestMethod()]
        public void CalculateResultTest_2LevelMulti(string type1, string type2, double value1_1, double value1_2, string type3, double value2_1, double value2_2 ,  double expected)
        {
            Node testNode = new NodeMulti()
            {
                Type = type1,
                Node1 = new NodeMulti()
                {
                    Type = type2,
                    Node1 = new NodeSingle() { Value = value1_1 },
                    Node2 = new NodeSingle() { Value = value1_2 }
                },
                Node2 = new NodeMulti()
                {
                    Type = type3,
                    Node1 = new NodeSingle() { Value = value2_1 },
                    Node2 = new NodeSingle() { Value = value2_2 }
                }
            };

            double actual = testNode.CalculateResult();

            Assert.AreEqual(expected, actual);
        }
    }
}