using dnWalker.Tests.ExampleTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace dnWalker.Tests.Interpreter
{
    [Trait("Category", "Interpreter")]
    public class IntepreterTests : ExamplesTestBase
    {
        public IntepreterTests() : base(Lazy.Value)
        {
        }

        protected override object Test(string methodName, params object[] args)
        {
            methodName = "Examples.Interpreter.IntepreterTests." + methodName;
            return base.Test(methodName, args);
        }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-1.0d)]
        [InlineData(1.0d)]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        [InlineData(double.NaN)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.Epsilon)]
        [InlineData(2.0d)]
        [InlineData(3.0d)]
        [InlineData(4.0d)]
        [InlineData(5.0d)]
        [InlineData(6.0d)]
        public void Test_SWITCH__6_Double(object arg0) { Test("Test_SWITCH__6_Double", arg0); }
    }
}
