using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using FluentAssertions;

using MMC;
using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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


        [Theory, CombinatorialData]
        public void Test_CGT([CombinatorialValues(5, 10)] int x, [CombinatorialValues(5, 10)] int y, [CombinatorialValues(true, false)] bool isXSymb, [CombinatorialValues(true, false)] bool isYSymb)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_CGT__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            if (isXSymb) xDE.SetExpression(NamedInt("x"), state);
            if (isYSymb) yDE.SetExpression(NamedInt("y"), state);

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToBool().Should().Be(x > y);
            result.TryGetExpression(state, out Expression expr).Should().Be(isXSymb || isYSymb);

            if (isXSymb && isYSymb) expr.ToString().Should().BeEquivalentTo($"(x > y)");
            if (isXSymb && !isYSymb) expr.ToString().Should().BeEquivalentTo($"(x > {y})");
            if (!isXSymb && isYSymb) expr.ToString().Should().BeEquivalentTo($"({x} > y)");
        }

        [Theory, CombinatorialData]
        public void Test_CLT([CombinatorialValues(5, 10)] int x, [CombinatorialValues(5, 10)] int y, [CombinatorialValues(true, false)] bool isXSymb, [CombinatorialValues(true, false)] bool isYSymb)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_CLT__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            if (isXSymb) xDE.SetExpression(NamedInt("x"), state);
            if (isYSymb) yDE.SetExpression(NamedInt("y"), state);

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToBool().Should().Be(x < y);
            result.TryGetExpression(state, out Expression expr).Should().Be(isXSymb || isYSymb);

            if (isXSymb && isYSymb) expr.ToString().Should().BeEquivalentTo($"(x < y)");
            if (isXSymb && !isYSymb) expr.ToString().Should().BeEquivalentTo($"(x < {y})");
            if (!isXSymb && isYSymb) expr.ToString().Should().BeEquivalentTo($"({x} < y)");
        }

        [Theory, CombinatorialData]
        public void Test_CEQ([CombinatorialValues(5, 10)] int x, [CombinatorialValues(5, 10)] int y, [CombinatorialValues(true, false)] bool isXSymb, [CombinatorialValues(true, false)] bool isYSymb)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_CEQ__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            if (isXSymb) xDE.SetExpression(NamedInt("x"), state);
            if (isYSymb) yDE.SetExpression(NamedInt("y"), state);

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToBool().Should().Be(x == y);
            result.TryGetExpression(state, out Expression expr).Should().Be(isXSymb || isYSymb);

            if (isXSymb && isYSymb) expr.ToString().Should().BeEquivalentTo($"(x == y)");
            if (isXSymb && !isYSymb) expr.ToString().Should().BeEquivalentTo($"(x == {y})");
            if (!isXSymb && isYSymb) expr.ToString().Should().BeEquivalentTo($"({x} == y)");
        }
    }
}
