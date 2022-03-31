using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.InterpreterTests.Symbolic
{
    public class BinaryBranchTest : SymbolicInterpreterTestBase
    {
        public BinaryBranchTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        //TODO: BEQ, BNE_UN for object references, e.i. null checking...

        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 5, null, null, null)]
        [InlineData(5, 3, "x", null, "(x != 3)")]
        [InlineData(5, 5, "x", null, "(x == 5)")]
        [InlineData(5, 3, null, "y", "(5 != y)")]
        [InlineData(5, 5, null, "y", "(5 == y)")]
        [InlineData(5, 3, "x", "y", "(x != y)")]
        [InlineData(5, 5, "x", "y", "(x == y)")]
        public void Test_BEQ__Int32(int arg0, int arg1, string name0, string name1, string pathCondition)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, pathCondition, null);
        }

        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 5, null, null, null)]
        [InlineData(5, 3, "x", null, "(x != 3)")]
        [InlineData(5, 5, "x", null, "(x == 5)")]
        [InlineData(5, 3, null, "y", "(5 != y)")]
        [InlineData(5, 5, null, "y", "(5 == y)")]
        [InlineData(5, 3, "x", "y", "(x != y)")]
        [InlineData(5, 5, "x", "y", "(x == y)")]
        public void Test_BEQ_S__Int32(int arg0, int arg1, string name0, string name1, string pathCondition)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, pathCondition, null);
        }


        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 5, null, null, null)]
        [InlineData(5, 3, "x", null, "(x > 3)")]
        [InlineData(5, 5, "x", null, "(x <= 5)")]
        [InlineData(5, 3, null, "y", "(5 > y)")]
        [InlineData(5, 5, null, "y", "(5 <= y)")]
        [InlineData(5, 3, "x", "y", "(x > y)")]
        [InlineData(5, 5, "x", "y", "(x <= y)")]
        public void Test_BGT__Int32(int arg0, int arg1, string name0, string name1, string pathCondition)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, pathCondition, null);
        }

        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 5, null, null, null)]
        [InlineData(5, 3, "x", null, "(x > 3)")]
        [InlineData(5, 5, "x", null, "(x <= 5)")]
        [InlineData(5, 3, null, "y", "(5 > y)")]
        [InlineData(5, 5, null, "y", "(5 <= y)")]
        [InlineData(5, 3, "x", "y", "(x > y)")]
        [InlineData(5, 5, "x", "y", "(x <= y)")]
        public void Test_BGT_S__Int32(int arg0, int arg1, string name0, string name1, string pathCondition)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, pathCondition, null);
        }

        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 5, null, null, null)]
        [InlineData(5, 3, "x", null, "(x > 3)")]
        [InlineData(5, 5, "x", null, "(x <= 5)")]
        [InlineData(5, 3, null, "y", "(5 > y)")]
        [InlineData(5, 5, null, "y", "(5 <= y)")]
        [InlineData(5, 3, "x", "y", "(x > y)")]
        [InlineData(5, 5, "x", "y", "(x <= y)")]
        public void Test_BGT_UN__Int32(int arg0, int arg1, string name0, string name1, string pathCondition)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, pathCondition, null);
        }

        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 5, null, null, null)]
        [InlineData(5, 3, "x", null, "(x > 3)")]
        [InlineData(5, 5, "x", null, "(x <= 5)")]
        [InlineData(5, 3, null, "y", "(5 > y)")]
        [InlineData(5, 5, null, "y", "(5 <= y)")]
        [InlineData(5, 3, "x", "y", "(x > y)")]
        [InlineData(5, 5, "x", "y", "(x <= y)")]
        public void Test_BGT_UN_S__Int32(int arg0, int arg1, string name0, string name1, string pathCondition)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, pathCondition, null);
        }


        [Theory]
        [InlineData(3, 5, null, null, null)]
        [InlineData(5, 5, null, null, null)]
        [InlineData(3, 5, "x", null, "(x < 5)")]
        [InlineData(5, 5, "x", null, "(x >= 5)")]
        [InlineData(3, 5, null, "y", "(3 < y)")]
        [InlineData(5, 5, null, "y", "(5 >= y)")]
        [InlineData(3, 5, "x", "y", "(x < y)")]
        [InlineData(5, 5, "x", "y", "(x >= y)")]
        public void Test_BGE__Int32(int arg0, int arg1, string name0, string name1, string pathCondition)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, pathCondition, null);
        }

        [Theory]
        [InlineData(3, 5, null, null, null)]
        [InlineData(5, 5, null, null, null)]
        [InlineData(3, 5, "x", null, "(x < 5)")]
        [InlineData(5, 5, "x", null, "(x >= 5)")]
        [InlineData(3, 5, null, "y", "(3 < y)")]
        [InlineData(5, 5, null, "y", "(5 >= y)")]
        [InlineData(3, 5, "x", "y", "(x < y)")]
        [InlineData(5, 5, "x", "y", "(x >= y)")]
        public void Test_BGE_S__Int32(int arg0, int arg1, string name0, string name1, string pathCondition)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, pathCondition, null);
        }

        [Theory]
        [InlineData(3, 5, null, null, null)]
        [InlineData(5, 5, null, null, null)]
        [InlineData(3, 5, "x", null, "(x < 5)")]
        [InlineData(5, 5, "x", null, "(x >= 5)")]
        [InlineData(3, 5, null, "y", "(3 < y)")]
        [InlineData(5, 5, null, "y", "(5 >= y)")]
        [InlineData(3, 5, "x", "y", "(x < y)")]
        [InlineData(5, 5, "x", "y", "(x >= y)")]
        public void Test_BGE_UN__Int32(int arg0, int arg1, string name0, string name1, string pathCondition)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, pathCondition, null);
        }

        [Theory]
        [InlineData(3, 5, null, null, null)]
        [InlineData(5, 5, null, null, null)]
        [InlineData(3, 5, "x", null, "(x < 5)")]
        [InlineData(5, 5, "x", null, "(x >= 5)")]
        [InlineData(3, 5, null, "y", "(3 < y)")]
        [InlineData(5, 5, null, "y", "(5 >= y)")]
        [InlineData(3, 5, "x", "y", "(x < y)")]
        [InlineData(5, 5, "x", "y", "(x >= y)")]
        public void Test_BGE_UN_S__Int32(int arg0, int arg1, string name0, string name1, string pathCondition)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, pathCondition, null);
        }


        [Theory]
        [InlineData(3, 5, null, null, null)]
        [InlineData(5, 5, null, null, null)]
        [InlineData(3, 5, "x", null, "(x < 5)")]
        [InlineData(5, 5, "x", null, "(x >= 5)")]
        [InlineData(3, 5, null, "y", "(3 < y)")]
        [InlineData(5, 5, null, "y", "(5 >= y)")]
        [InlineData(3, 5, "x", "y", "(x < y)")]
        [InlineData(5, 5, "x", "y", "(x >= y)")]
        public void Test_BLT__Int32(int arg0, int arg1, string name0, string name1, string pathCondition)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, pathCondition, null);
        }

        [Theory]
        [InlineData(3, 5, null, null, null)]
        [InlineData(5, 5, null, null, null)]
        [InlineData(3, 5, "x", null, "(x < 5)")]
        [InlineData(5, 5, "x", null, "(x >= 5)")]
        [InlineData(3, 5, null, "y", "(3 < y)")]
        [InlineData(5, 5, null, "y", "(5 >= y)")]
        [InlineData(3, 5, "x", "y", "(x < y)")]
        [InlineData(5, 5, "x", "y", "(x >= y)")]
        public void Test_BLT_S__Int32(int arg0, int arg1, string name0, string name1, string pathCondition)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, pathCondition, null);
        }

        [Theory]
        [InlineData(3, 5, null, null, null)]
        [InlineData(5, 5, null, null, null)]
        [InlineData(3, 5, "x", null, "(x < 5)")]
        [InlineData(5, 5, "x", null, "(x >= 5)")]
        [InlineData(3, 5, null, "y", "(3 < y)")]
        [InlineData(5, 5, null, "y", "(5 >= y)")]
        [InlineData(3, 5, "x", "y", "(x < y)")]
        [InlineData(5, 5, "x", "y", "(x >= y)")]
        public void Test_BLT_UN__Int32(int arg0, int arg1, string name0, string name1, string pathCondition)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, pathCondition, null);
        }

        [Theory]
        [InlineData(3, 5, null, null, null)]
        [InlineData(5, 5, null, null, null)]
        [InlineData(3, 5, "x", null, "(x < 5)")]
        [InlineData(5, 5, "x", null, "(x >= 5)")]
        [InlineData(3, 5, null, "y", "(3 < y)")]
        [InlineData(5, 5, null, "y", "(5 >= y)")]
        [InlineData(3, 5, "x", "y", "(x < y)")]
        [InlineData(5, 5, "x", "y", "(x >= y)")]
        public void Test_BLT_UN_S__Int32(int arg0, int arg1, string name0, string name1, string pathCondition)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, pathCondition, null);
        }


        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 5, null, null, null)]
        [InlineData(5, 3, "x", null, "(x > 3)")]
        [InlineData(5, 5, "x", null, "(x <= 5)")]
        [InlineData(5, 3, null, "y", "(5 > y)")]
        [InlineData(5, 5, null, "y", "(5 <= y)")]
        [InlineData(5, 3, "x", "y", "(x > y)")]
        [InlineData(5, 5, "x", "y", "(x <= y)")]
        public void Test_BLE__Int32(int arg0, int arg1, string name0, string name1, string pathCondition)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, pathCondition, null);
        }

        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 5, null, null, null)]
        [InlineData(5, 3, "x", null, "(x > 3)")]
        [InlineData(5, 5, "x", null, "(x <= 5)")]
        [InlineData(5, 3, null, "y", "(5 > y)")]
        [InlineData(5, 5, null, "y", "(5 <= y)")]
        [InlineData(5, 3, "x", "y", "(x > y)")]
        [InlineData(5, 5, "x", "y", "(x <= y)")]
        public void Test_BLE_S__Int32(int arg0, int arg1, string name0, string name1, string pathCondition)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, pathCondition, null);
        }

        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 5, null, null, null)]
        [InlineData(5, 3, "x", null, "(x > 3)")]
        [InlineData(5, 5, "x", null, "(x <= 5)")]
        [InlineData(5, 3, null, "y", "(5 > y)")]
        [InlineData(5, 5, null, "y", "(5 <= y)")]
        [InlineData(5, 3, "x", "y", "(x > y)")]
        [InlineData(5, 5, "x", "y", "(x <= y)")]
        public void Test_BLE_UN__Int32(int arg0, int arg1, string name0, string name1, string pathCondition)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, pathCondition, null);
        }

        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 5, null, null, null)]
        [InlineData(5, 3, "x", null, "(x > 3)")]
        [InlineData(5, 5, "x", null, "(x <= 5)")]
        [InlineData(5, 3, null, "y", "(5 > y)")]
        [InlineData(5, 5, null, "y", "(5 <= y)")]
        [InlineData(5, 3, "x", "y", "(x > y)")]
        [InlineData(5, 5, "x", "y", "(x <= y)")]
        public void Test_BLE_UN_S__Int32(int arg0, int arg1, string name0, string name1, string pathCondition)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, pathCondition, null);
        }


        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 5, null, null, null)]
        [InlineData(5, 3, "x", null, "(x != 3)")]
        [InlineData(5, 5, "x", null, "(x == 5)")]
        [InlineData(5, 3, null, "y", "(5 != y)")]
        [InlineData(5, 5, null, "y", "(5 == y)")]
        [InlineData(5, 3, "x", "y", "(x != y)")]
        [InlineData(5, 5, "x", "y", "(x == y)")]
        public void Test_BNE_UN__Int32(int arg0, int arg1, string name0, string name1, string pathCondition)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, pathCondition, null);
        }

        [Theory]
        [InlineData(5, 3, null, null, null)]
        [InlineData(5, 5, null, null, null)]
        [InlineData(5, 3, "x", null, "(x != 3)")]
        [InlineData(5, 5, "x", null, "(x == 5)")]
        [InlineData(5, 3, null, "y", "(5 != y)")]
        [InlineData(5, 5, null, "y", "(5 == y)")]
        [InlineData(5, 3, "x", "y", "(x != y)")]
        [InlineData(5, 5, "x", "y", "(x == y)")]
        public void Test_BNE_UN_S__Int32(int arg0, int arg1, string name0, string name1, string pathCondition)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg0, name0), (arg1, name1) }, pathCondition, null);
        }
    }
}
