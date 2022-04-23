using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.InterpreterTests.Symbolic
{
    public class UnaryOperationTest : SymbolicInterpreterTestBase
    {
        public UnaryOperationTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory]
        [InlineData(0, null, null)]
        [InlineData(1, null, null)]
        [InlineData(0, "x", "!x")]
        [InlineData(1, "x", "!x")]
        public void Test_NOT__Int32_AsBitVector(int arg, string name, string result)
        {
            Test("Test_NOT__Int32", new (object, string)[] { (arg, name) }, null, result);
        }

        [Theory]
        [InlineData(false, null, null)]
        [InlineData(true, null, null)]
        [InlineData(false, "x", "!x")]
        [InlineData(true, "x", "!x")]
        public void Test_NOT__Int32_AsBoolean(bool arg, string name, string result)
        {
            Test("Test_NOT__Int32", new (object, string)[] { (arg, name) }, null, result);
        }

        [Theory]
        [InlineData(-5, null, null)]
        [InlineData(5, null, null)]
        [InlineData(-5, "x", "-x")]
        [InlineData(5, "x", "-x")]
        public void Test_NEG__Int32(int arg, string name, string result)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, result);
        }
    }
}
