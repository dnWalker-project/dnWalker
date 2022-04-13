using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.InterpreterTests.Symbolic
{
    public class UnaryBranchTest : SymbolicInterpreterTestBase
    {
        public UnaryBranchTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory]
        [InlineData(0, null, null)]
        [InlineData(1, null, null)]
        [InlineData(0, "x", "(x == 0)")]
        [InlineData(1, "x", "(x != 0)")]
        public void Test_BRFALSE__Int32_AsNumber(int arg, string name, string pathCondition)
        {
            Test("Test_BRFALSE__Int32", new (object, string)[] { (arg, name) }, pathCondition, null);
        }

        [Theory]
        [InlineData(false, null, null)]
        [InlineData(true, null, null)]
        [InlineData(false, "x", "Not(x)")]
        [InlineData(true, "x", "x")]
        public void Test_BRFALSE__Int32_AsBoolean(bool arg, string name, string pathCondition)
        {
            Test("Test_BRFALSE__Int32", new (object, string)[] { (arg, name) }, pathCondition, null);
        }

        [Theory]
        [InlineData(0, null, null)]
        [InlineData(1, null, null)]
        [InlineData(0, "x", "(x == 0)")]
        [InlineData(1, "x", "(x != 0)")]
        public void Test_BRTRUE__Int32_AsNumber(int arg, string name, string pathCondition)
        {
            Test("Test_BRTRUE__Int32", new (object, string)[] { (arg, name) }, pathCondition, null);
        }

        [Theory]
        [InlineData(false, null, null)]
        [InlineData(true, null, null)]
        [InlineData(false, "x", "Not(x)")]
        [InlineData(true, "x", "x")]
        public void Test_BRTRUE__Int32_AsBoolean(bool arg, string name, string pathCondition)
        {
            Test("Test_BRTRUE__Int32", new (object, string)[] { (arg, name) }, pathCondition, null);
        }
    }
}
