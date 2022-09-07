
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.Tests.Interpreter.Symbolic
{
    public class SymbolicInterpreterTest : SymbolicTestBase
    {
        public SymbolicInterpreterTest(ITestOutputHelper output) : base(output)
        {
        }


        [Theory]
        [InlineData(new object[] {5, 6}, new string?[] {null, null}, null, "True")]
        [InlineData(new object[] {5, 6}, new string?[] {"a", null}, "(a + 6)", "True")]
        [InlineData(new object[] {5, 6}, new string?[] {null, "b"}, "(5 + b)", "True")]
        [InlineData(new object[] {5, 6}, new string?[] {"a", "b"}, "(a + b)", "True")]
        public void Test_ADD__Int32(object[] args, string?[] argNames, string resultExpr, string pathConstraint) 
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }
    }
}
