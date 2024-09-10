using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCPDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPDemo.Tests
{
    [TestClass()]
    public class ExpressionParserTests
    {
        [TestMethod()]
        public void ParseIntOrDouble_Sucess_UnlimitedAmountDouble()
        {
            string toBeParsed = "5.0GGGG";
            string expected = "5.0";
            string expectedRemainder = "GGGG";

            string remainder = ExpressionParser.ParseIntOrDouble(toBeParsed, -1, out string result);
            Assert.AreEqual(expected, result);
            Assert.AreEqual(expectedRemainder, remainder);

        }
        [TestMethod()]
        public void ParseIntOrDouble_Failure_TwoSeperators_UnlimitedAmount()
        {
            string toBeParsed = "5..0ggg";
            string expected = "5.";
            string expectedRemainder = ".0ggg";

            string remainder = ExpressionParser.ParseIntOrDouble(toBeParsed, -1, out string result);
            Assert.AreEqual(expected, result);
            Assert.AreEqual(expectedRemainder, remainder);
        }
        [TestMethod()]
        public void ParseIntOrDouble_Sucess_UnlimitedAmountInt()
        {
            string toBeParsed = "5GGG";
            string expected = "5";
            string expectedRemainder = "GGG";

            string remainder = ExpressionParser.ParseIntOrDouble(toBeParsed, -1, out string result);
            Assert.AreEqual(expected, result);
            Assert.AreEqual(expectedRemainder, remainder);

        }
        [TestMethod()]
        public void ParseIntOrDouble_Sucess_UnlimitedAmountIntNegativeInt()
        {
            string toBeParsed = "-56GGGg";
            string expected = "-56";
            string expectedRemainder = "GGGg";

            string remainder = ExpressionParser.ParseIntOrDouble(toBeParsed, -1, out string result);
            Assert.AreEqual(expected, result);
            Assert.AreEqual(expectedRemainder, remainder);

        }
        [TestMethod()]
        public void ParseIntOrDouble_Sucess_UnlimitedAmountIntNegativeDouble()
        {
            string toBeParsed = "-5.6GGGg";
            string expected = "-5.6";
            string expectedRemainder = "GGGg";

            string remainder = ExpressionParser.ParseIntOrDouble(toBeParsed, -1, out string result);
            Assert.AreEqual(expected, result);
            Assert.AreEqual(expectedRemainder, remainder);

        }
        [TestMethod()]
        public void ParseIntOrDouble_Sucess_NoNumberAfterDecimal()
        {
            string toBeParsed = "4.";
            string expected = "4.";
            string expectedRemainder = "";

            string remainder = ExpressionParser.ParseIntOrDouble(toBeParsed, -1, out string result);
            Assert.AreEqual(expected, result);
            Assert.AreEqual(expectedRemainder, remainder);

        }
        [TestMethod()]
        public void ParseIntOrDouble_Sucess_NoNumberBeforeDecimal()
        {
            string toBeParsed = ".4";
            string expected = ".4";
            string expectedRemainder = "";

            string remainder = ExpressionParser.ParseIntOrDouble(toBeParsed, -1, out string result);
            Assert.AreEqual(expected, result);
            Assert.AreEqual(expectedRemainder, remainder);

        }
        [TestMethod()]
        public void ParseIntOrDouble_Fail_MinusInMiddle()
        {
            string toBeParsed = "4-4";
            string expected = "";
            string expectedRemainder = "4-4";

            string remainder = ExpressionParser.ParseIntOrDouble(toBeParsed, -1, out string result);
            Assert.AreEqual(expected, result);
            Assert.AreEqual(expectedRemainder, remainder);

        }
        [TestMethod()]
        public void ParseIntOrDouble_Fail_OnlyMinus()
        {
            string toBeParsed = "-";
            string expected = "";
            string expectedRemainder = "-";

            string remainder = ExpressionParser.ParseIntOrDouble(toBeParsed, -1, out string result);
            Assert.AreEqual(expected, result);
            Assert.AreEqual(expectedRemainder, remainder);

        }

        [TestMethod()]
        public void ParseIntOrDouble_Failure_WrongStartingChar()
        {
            string toBeParsed = "G5GGG";
            string expected = "";
            string expectedRemainder = toBeParsed;

            string remainder = ExpressionParser.ParseIntOrDouble(toBeParsed, -1, out string result);
            Assert.AreEqual(expected, result);
            Assert.AreEqual(expectedRemainder, remainder);
        }
        [TestMethod()]
        public void ParseCharExcept_Sucess_AddThenParenthesies()
        {
            string toBeParsed = "add(5,1)";
            string expected = "add";
            string expectedRemainder = "(5,1)";

            string remainder = ExpressionParser.ParseCharExcept(toBeParsed, new char[] {'('},-1, out string result);
            Assert.AreEqual(expected, result);
            Assert.AreEqual(expectedRemainder, remainder);
        }

        [TestMethod()]
        public void ParseCharExcept_Fail_NothingParsed()
        {
            string toBeParsed = "(5,1)add";
            string expected = "";
            string expectedRemainder = toBeParsed;

            string remainder = ExpressionParser.ParseCharExcept(toBeParsed, new char[] { '(' }, -1, out string result);
            Assert.AreEqual(expected, result);
            Assert.AreEqual(expectedRemainder, remainder);
        }
        [TestMethod()]
        [DataRow("1", 1)]
        [DataRow("0", 0)]
        [DataRow("-1", -1)]
        [DataRow("14", 14)]
        [DataTestMethod]
        public void ParseNodeSingleTest_Sucess_Int(string toBeParsed, int expectedValue)
        {
            Node expected = new NodeSingle() { Value = expectedValue };
            string remainder = ExpressionParser.ParseNodeSingle(toBeParsed, out Node actual);
            Assert.IsTrue(expected.Equals(actual));
            Assert.AreEqual("", remainder);

        }
        [TestMethod()]
        [DataRow("1.1", 1.1)]
        [DataRow("0.2", 0.2)]
        [DataRow("-1.3", -1.3)]
        [DataRow("14.9", 14.9)]
        [DataTestMethod]
        public void ParseNodeSingleTest_Sucess_Double(string toBeParsed, double expectedValue)
        {
            Node expected = new NodeSingle() { Value = expectedValue };
            string remainder = ExpressionParser.ParseNodeSingle(toBeParsed, out Node actual);
            Assert.IsTrue(expected.Equals(actual));
            Assert.AreEqual("", remainder);

        }
        [TestMethod()]
        [DataRow(",1.1", 1.1)]
        [DataRow(",0.2", 0.2)]
        [DataRow(",-1.3", -1.3)]
        [DataRow(",14.9", 14.9)]
        [DataTestMethod]
        public void ParseNodeSingleTest_Sucess_Double_CommaBefore(string toBeParsed, double expectedValue)
        {
            Node expected = new NodeSingle() { Value = expectedValue };
            string remainder = ExpressionParser.ParseNodeSingle(toBeParsed, out Node actual);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual("", remainder);

        }
        [TestMethod()]
        [DataRow("G1.1")]
        [DataRow("c.2")]
        [DataRow("h-1.C3")]
        [DataRow("l14.9")]
        [DataTestMethod]
        public void ParseNodeSingleTest_Fail_CantParseRetursnNull(string toBeParsed)
        {
            
            string remainder = ExpressionParser.ParseNodeSingle(toBeParsed, out Node actual);
            Assert.IsNull(actual);
            Assert.AreEqual(toBeParsed, remainder);

        }
        [TestMethod()]
        [DataRow("1.1)", 1.1, ")")]
        [DataRow("0.2,", 0.2, ",")]
        [DataRow("-1.3H", -1.3, "H")]
        [DataRow("14.9R", 14.9, "R")]
        [DataTestMethod]
        public void ParseNodeSingleTest_Sucess_Double_DontParseLastChar(string toBeParsed, double expectedValue, string expectedRemainder)
        {
            Node expected = new NodeSingle() { Value = expectedValue };
            string remainder = ExpressionParser.ParseNodeSingle(toBeParsed, out Node actual);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedRemainder, remainder);
        }
        [TestMethod()]
        [DataRow("add(5.5,6.7)", 5.5, 6.7, "add")]
        [DataRow("add(-5.5,-6.7)", -5.5, -6.7, "add")]
        [DataRow("multiply(5.5,-8.7)", 5.5, -8.7, "multiply")]
        [DataRow("add(0.0,6.7)", 0.0, 6.7, "add")]
        [DataRow("subtract(0,2)", 0, 2, "subtract")]
        [DataRow("add(-2,3)", -2, 3, "add")]
        [DataRow("multiply(0.0,-3)", 0.0, -3, "multiply")]
        [DataRow("divide(4.5,6)", 4.5, 6, "divide")]
        [DataRow("subtract(-1100,4)", -1100, 4, "subtract")]
        [DataRow("divide(4,2.3)", 4, 2.3, "divide")]
        [DataTestMethod]
        public void ParseNodeMultiTest_Sucess_OneLevel_NoRemainder(string toBeParsed, double expectedValue1, double expectedValue2, string type)
        {
            Node expected = new NodeMulti()
            {
                Node1 = new NodeSingle() { Value = expectedValue1 },
                Node2 = new NodeSingle() { Value = expectedValue2 },
                Type = type
            };
            string expectedRemainder = "";
            string remainder = ExpressionParser.ParseNodeMulti(toBeParsed, out Node actual);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedRemainder, remainder);

        }
        [TestMethod()]
        [DataRow("add(add(5,4),6.7)", 5, 4, "add", "add", 6.7)]
        [DataRow("add(subtract(4.5,6.7),5.4)", 4.5, 6.7, "add", "subtract", 5.4)]
        [DataRow("multiply(divide(4,6.7),-8.7)", 4, 6.7, "multiply", "divide", -8.7)]
        [DataRow("add(multiply(3.5,6.7),6.7)", 3.5, 6.7, "add", "multiply", 6.7)]
        [DataRow("subtract(add(1,2),2)", 1, 2, "subtract", "add", 2)]
        [DataRow("add(divide(4,6.4),3)", 4, 6.4, "add", "divide", 3)]
        [DataRow("multiply(subtract(3,6.7),-3)", 3, 6.7, "multiply", "subtract", -3)]
        [DataRow("divide(multiply(4,7),6)", 4, 7, "divide", "multiply", 6)]
        [DataRow("subtract(subtract(5,6),4)", 5, 6, "subtract", "subtract", 4)]
        [DataRow("divide(add(5,6.7),2.3)", 5, 6.7, "divide", "add", 2.3)]
        [DataTestMethod]
        public void ParseNodeMultiTest_Sucess_OneTwoLevels_NoRemainder(string toBeParsed, double expectedValue1, double expectedValue2, string type, string type2, double expectedValue3)
        {
            Node expected = new NodeMulti()
            {
                Node1 = new NodeMulti()
                {
                    Type = type2,
                    Node1 = new NodeSingle() { Value = expectedValue1 },
                    Node2 = new NodeSingle() { Value = expectedValue2 }

                },
                Node2 = new NodeSingle() { Value = expectedValue3 },
                Type = type
            };

            string expectedRemainder = "";
            string remainder = ExpressionParser.ParseNodeMulti(toBeParsed, out Node actual);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedRemainder, remainder);

        }
        [TestMethod()]
        [DataRow("divide(subtract(0,6),add(5.6,-7))", "divide", "subtract", 0, 6, "add", 5.6, -7)]
        [DataRow("multiply(subtract(-5,6.7),divide(-7.0,7))", "multiply", "subtract", -5, 6.7, "divide", -7.0, 7)]
        [DataRow("divide(add(5,6.7),divide(7.0,7))", "divide", "add", 5, 6.7, "divide", 7.0, 7)]
        [DataTestMethod]
        public void ParseNodeMultiTest_Sucess_TwoLevels_NoRemainder(string toBeParsed, string type1, string type2, double value1_1, double value1_2, string type3, double value2_1, double value2_2 )
        {
            Node expected = new NodeMulti()
            {
                Type = type1,
                Node1 = new NodeMulti()
                {
                    Type = type2,
                    Node1 = new NodeSingle() { Value = value1_1 },
                    Node2 = new NodeSingle() { Value = value1_2 },
                },
                Node2 = new NodeMulti()
                {
                    Type = type3,
                    Node1 = new NodeSingle() { Value = value2_1 },
                    Node2 = new NodeSingle() { Value = value2_2 },
                }

                
            };

            string expectedRemainder = "";
            string remainder = ExpressionParser.ParseNodeMulti(toBeParsed, out Node actual);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedRemainder, remainder);

        }
        [TestMethod()]
        [DataRow("divide(subtract(0,6),add,(5.6,-7))GGGC", "divide", "subtract", 0, 6, "add", 5.6, -7)]
        [DataRow("divide(subtract(0,6),add,(5.6,-7)))", "divide", "subtract", 0, 6, "add", 5.6, -7)]
        [DataRow("divide(subtract(0,6),ad,(5.6,-7))", "divide", "subtract", 0, 6, "add", 5.6, -7)]
        [DataRow("divide(subtract(0,6)),add,(5.6,-7))", "divide", "subtract", 0, 6, "add", 5.6, -7)]
        [DataRow("divide(subtract(0,6),add,(5.6,-7))", "divide", "subtract", 0, 6, "add", 5.6, -7)]
        [DataRow("multiply((subtract(-5,6.7),divide(-7.0,7))", "multiply", "subtract", -5, 6.7, "divide", -7.0, 7)]
        [DataRow("divi(add(5,6.7),divide(7.0,7))", "divide", "add", 5, 6.7, "divide", 7.0, 7)]
        [DataRow("divide(add(5,6.7),,divide(7.0,7))", "divide", "add", 5, 6.7, "divide", 7.0, 7)]
        [DataTestMethod]
        public void ParseNodeMultiTest_Fail_TwoLevels_CantParseReturnNull(string toBeParsed, string type1, string type2, double value1_1, double value1_2, string type3, double value2_1, double value2_2)
        {
            Node expected = null;

            string expectedRemainder = toBeParsed;
            string remainder = ExpressionParser.ParseNodeMulti(toBeParsed, out Node actual);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedRemainder, remainder);

        }

    }
}