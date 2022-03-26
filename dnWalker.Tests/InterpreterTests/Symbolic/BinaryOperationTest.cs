using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.InterpreterTests.Symbolic
{
    public class BinaryOperationTest : SymbolicInterpreterTestBase
    {
        public BinaryOperationTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory]
        [InlineData(5.25, 3.5, null, null, null)]
        [InlineData(5.25, 3.5, "x", null, "(x == 3.5)")]
        [InlineData(5.25, 3.5, null, "y", "(5.25 == y)")]
        [InlineData(5.25, 3.5, "x", "y", "(x == y)")]
        public void Test_CEQ__Double(double arg0, double arg1, string name0, string name1, string result)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }


        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 3, "x", null, "(x > 3)")]
        [InlineData(5, 3, null, "y", "(5 > y)")]
        [InlineData(5, 3, "x", "y", "(x > y)")]
        public void Test_CGT__Int32(int arg0, int arg1, string name0, string name1, string result)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }

        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 3, "x", null, "(x > 3)")]
        [InlineData(5, 3, null, "y", "(5 > y)")]
        [InlineData(5, 3, "x", "y", "(x > y)")]
        public void Test_CGT_UN__Int32(int arg0, int arg1, string name0, string name1, string result)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }


        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 3, "x", null, "(x < 3)")]
        [InlineData(5, 3, null, "y", "(5 < y)")]
        [InlineData(5, 3, "x", "y", "(x < y)")]
        public void Test_CLT__Int32(int arg0, int arg1, string name0, string name1, string result)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }

        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 3, "x", null, "(x < 3)")]
        [InlineData(5, 3, null, "y", "(5 < y)")]
        [InlineData(5, 3, "x", "y", "(x < y)")]
        public void Test_CLT_UN__Int32(int arg0, int arg1, string name0, string name1, string result)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }


        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 3, "x", null, "(x + 3)")]
        [InlineData(5, 3, null, "y", "(5 + y)")]
        [InlineData(5, 3, "x", "y", "(x + y)")]
        public void Test_ADD__Int32(int arg0, int arg1, string name0, string name1, string result)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }

        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 3, "x", null, "(x + 3)")]
        [InlineData(5, 3, null, "y", "(5 + y)")]
        [InlineData(5, 3, "x", "y", "(x + y)")]
        public void Test_ADD_OVF__Int32(int arg0, int arg1, string name0, string name1, string result)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }

        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 3, "x", null, "(x + 3)")]
        [InlineData(5, 3, null, "y", "(5 + y)")]
        [InlineData(5, 3, "x", "y", "(x + y)")]
        public void Test_ADD_OVF_UN__Int32(int arg0, int arg1, string name0, string name1, string result)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }


        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 3, "x", null, "(x / 3)")]
        [InlineData(5, 3, null, "y", "(5 / y)")]
        [InlineData(5, 3, "x", "y", "(x / y)")]
        public void Test_DIV__Int32(int arg0, int arg1, string name0, string name1, string result)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }

        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 3, "x", null, "(x / 3)")]
        [InlineData(5, 3, null, "y", "(5 / y)")]
        [InlineData(5, 3, "x", "y", "(x / y)")]
        public void Test_DIV_UN__Int32(int arg0, int arg1, string name0, string name1, string result)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }


        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 3, "x", null, "(x * 3)")]
        [InlineData(5, 3, null, "y", "(5 * y)")]
        [InlineData(5, 3, "x", "y", "(x * y)")]
        public void Test_MUL__Int32(int arg0, int arg1, string name0, string name1, string result)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }

        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 3, "x", null, "(x * 3)")]
        [InlineData(5, 3, null, "y", "(5 * y)")]
        [InlineData(5, 3, "x", "y", "(x * y)")]
        public void Test_MUL_OVF__Int32(int arg0, int arg1, string name0, string name1, string result)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }

        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 3, "x", null, "(x * 3)")]
        [InlineData(5, 3, null, "y", "(5 * y)")]
        [InlineData(5, 3, "x", "y", "(x * y)")]
        public void Test_MUL_OVF_UN__Int32(int arg0, int arg1, string name0, string name1, string result)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }


        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 3, "x", null, "(x % 3)")]
        [InlineData(5, 3, null, "y", "(5 % y)")]
        [InlineData(5, 3, "x", "y", "(x % y)")]
        public void Test_REM__Int32(int arg0, int arg1, string name0, string name1, string result)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }

        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 3, "x", null, "(x % 3)")]
        [InlineData(5, 3, null, "y", "(5 % y)")]
        [InlineData(5, 3, "x", "y", "(x % y)")]
        public void Test_REM_UN__Int32(int arg0, int arg1, string name0, string name1, string result)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }


        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 3, "x", null, "(x - 3)")]
        [InlineData(5, 3, null, "y", "(5 - y)")]
        [InlineData(5, 3, "x", "y", "(x - y)")]
        public void Test_SUB__Int32(int arg0, int arg1, string name0, string name1, string result)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }

        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 3, "x", null, "(x - 3)")]
        [InlineData(5, 3, null, "y", "(5 - y)")]
        [InlineData(5, 3, "x", "y", "(x - y)")]
        public void Test_SUB_OVF__Int32(int arg0, int arg1, string name0, string name1, string result)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }

        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 3, "x", null, "(x - 3)")]
        [InlineData(5, 3, null, "y", "(5 - y)")]
        [InlineData(5, 3, "x", "y", "(x - y)")]
        public void Test_SUB_OVF_UN__Int32(int arg0, int arg1, string name0, string name1, string result)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }


        [Theory]
        [InlineData(true, false, null, null, null)]
        [InlineData(true, false, "x", null, "(x And False)")]
        [InlineData(true, false, null, "y", "(True And y)")]
        [InlineData(true, false, "x", "y", "(x And y)")]
        public void Test_AND__Int32_AsBoolean(bool arg0, bool arg1, string name0, string name1, string result)
        {
            Test("Test_AND__Int32", new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }

        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 3, "x", null, "(x & 3)")]
        [InlineData(5, 3, null, "y", "(5 & y)")]
        [InlineData(5, 3, "x", "y", "(x & y)")]
        public void Test_AND__Int32_AsBitVectors(int arg0, int arg1, string name0, string name1, string result)
        {
            Test("Test_AND__Int32", new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }


        [Theory]
        [InlineData(true, false, null, null, null)]
        [InlineData(true, false, "x", null, "(x Or False)")]
        [InlineData(true, false, null, "y", "(True Or y)")]
        [InlineData(true, false, "x", "y", "(x Or y)")]
        public void Test_OR__Int32_AsBooleans(bool arg0, bool arg1, string name0, string name1, string result)
        {
            Test("Test_OR__Int32", new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }

        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 3, "x", null, "(x | 3)")]
        [InlineData(5, 3, null, "y", "(5 | y)")]
        [InlineData(5, 3, "x", "y", "(x | y)")]
        public void Test_OR__Int32_AsBitVectors(int arg0, int arg1, string name0, string name1, string result)
        {
            Test("Test_OR__Int32", new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }


        [Theory]
        [InlineData(true, false, null, null, null)]
        [InlineData(true, false, "x", null, "(x ^ False)")]
        [InlineData(true, false, null, "y", "(True ^ y)")]
        [InlineData(true, false, "x", "y", "(x ^ y)")]
        public void Test_XOR__Int32(bool arg0, bool arg1, string name0, string name1, string result)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }


        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 3, "x", null, "(x << 3)")]
        [InlineData(5, 3, null, "y", "(5 << y)")]
        [InlineData(5, 3, "x", "y", "(x << y)")]
        public void Test_SHL__Int32(int arg0, int arg1, string name0, string name1, string result)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }


        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 3, "x", null, "(x >> 3)")]
        [InlineData(5, 3, null, "y", "(5 >> y)")]
        [InlineData(5, 3, "x", "y", "(x >> y)")]
        public void Test_SHR__Int32(int arg0, int arg1, string name0, string name1, string result)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }

        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 3, "x", null, "(x >> 3)")]
        [InlineData(5, 3, null, "y", "(5 >> y)")]
        [InlineData(5, 3, "x", "y", "(x >> y)")]
        public void Test_SHR_UN__Int32(int arg0, int arg1, string name0, string name1, string result)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, null, result);
        }
    }
}
