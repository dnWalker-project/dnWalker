using dnWalker.Symbolic;

using FluentAssertions;

using MMC;
using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.InterpreterExtensionTests.SymbolicExecutionExtension
{
    public class CompareTests : InterpreterExtensionTestBase
    {
        public CompareTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }



        [Theory]
        [InlineData(5, 10)]
        public void Test_CGT(int x, int y)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_CGT__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            xDE.SetExpression(Expression.Parameter(typeof(int), "x"), state);
            yDE.SetExpression(Expression.Parameter(typeof(int), "y"), state);

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToBool().Should().Be(x > y);
            result.TryGetExpression(state, out Expression expr).Should().BeTrue();
            expr.ToString().Should().BeEquivalentTo("(x > y)");
        }

        [Theory]
        [InlineData(5, 10)]
        public void Test_CLT(int x, int y)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_CLT__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            xDE.SetExpression(Expression.Parameter(typeof(int), "x"), state);
            yDE.SetExpression(Expression.Parameter(typeof(int), "y"), state);

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToBool().Should().Be(x < y);
            result.TryGetExpression(state, out Expression expr).Should().BeTrue();
            expr.ToString().Should().BeEquivalentTo("(x < y)");
        }

        [Theory]
        [InlineData(5, 10)]
        public void Test_CEQ(int x, int y)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_CEQ__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            xDE.SetExpression(Expression.Parameter(typeof(int), "x"), state);
            yDE.SetExpression(Expression.Parameter(typeof(int), "y"), state);

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToBool().Should().Be(x == y);
            result.TryGetExpression(state, out Expression expr).Should().BeTrue();
            expr.ToString().Should().BeEquivalentTo("(x == y)");
        }
    }
}
