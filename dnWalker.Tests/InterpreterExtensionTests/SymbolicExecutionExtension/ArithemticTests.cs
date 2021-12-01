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
    public class ArithemticTests : InterpreterExtensionTestBase
    {
        public ArithemticTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory, CombinatorialData]
        public void Test_ADD([CombinatorialValues(5, 10)]int x, [CombinatorialValues(5, 10)] int y, [CombinatorialValues(true, false)] bool isXSymb, [CombinatorialValues(true, false)] bool isYSymb, [CombinatorialValues("Test_ADD__Int32", "Test_ADD_OVF_UN__Int32")]string method)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass." + method)
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            if (isXSymb) xDE.SetExpression(Expression.Parameter(typeof(int), "x"), state);
            if (isYSymb) yDE.SetExpression(Expression.Parameter(typeof(int), "y"), state);

            explorer.Run();
            
            Int4 result = (Int4) state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(x + y);
            result.TryGetExpression(state, out Expression expr).Should().Be(isXSymb || isYSymb);

            if (isXSymb && isYSymb)  expr.ToString().Should().BeEquivalentTo($"(x + y)");
            if (isXSymb && !isYSymb) expr.ToString().Should().BeEquivalentTo($"(x + {y})");
            if (!isXSymb && isYSymb) expr.ToString().Should().BeEquivalentTo($"({x} + y)");
        }

        [Theory, CombinatorialData]
        public void Test_DIV([CombinatorialValues(5, 10)] double x, [CombinatorialValues(5, 10)] double y, [CombinatorialValues("Test_DIV__Double")] string method)
        {
            Float8 xDE = new Float8(x);
            Float8 yDE = new Float8(y);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass." + method)
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            xDE.SetExpression(Expression.Parameter(typeof(double), "x"), state);
            yDE.SetExpression(Expression.Parameter(typeof(double), "y"), state);

            explorer.Run();

            Float8 result = (Float8)state.CurrentThread.RetValue;

            result.ToDouble(CultureInfo.InvariantCulture).Should().Be(x / y);
            result.TryGetExpression(state, out Expression expr).Should().BeTrue();
            expr.ToString().Should().BeEquivalentTo("(x / y)");
        }

        [Theory, CombinatorialData]
        public void Test_MUL([CombinatorialValues(5, 10)] double x, [CombinatorialValues(5, 10)] double y, [CombinatorialValues("Test_MUL__Double")] string method)
        {
            Float8 xDE = new Float8(x);
            Float8 yDE = new Float8(y);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass." + method)
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            xDE.SetExpression(Expression.Parameter(typeof(double), "x"), state);
            yDE.SetExpression(Expression.Parameter(typeof(double), "y"), state);

            explorer.Run();

            Float8 result = (Float8)state.CurrentThread.RetValue;

            result.ToDouble(CultureInfo.InvariantCulture).Should().Be(x * y);
            result.TryGetExpression(state, out Expression expr).Should().BeTrue();
            expr.ToString().Should().BeEquivalentTo("(x * y)");
        }

        [Theory, CombinatorialData]
        public void Test_REM([CombinatorialValues(5, 10)] int x, [CombinatorialValues(5, 10)] int y, [CombinatorialValues("Test_REM__Int32")] string method)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass." + method)
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            xDE.SetExpression(Expression.Parameter(typeof(int), "x"), state);
            yDE.SetExpression(Expression.Parameter(typeof(int), "y"), state);

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(x % y);
            result.TryGetExpression(state, out Expression expr).Should().BeTrue();
            expr.ToString().Should().BeEquivalentTo("(x % y)");
        }

        [Theory, CombinatorialData]
        public void Test_SUB([CombinatorialValues(5, 10)] int x, [CombinatorialValues(5, 10)] int y, [CombinatorialValues("Test_SUB__Int32")] string method)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass." + method)
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            xDE.SetExpression(Expression.Parameter(typeof(int), "x"), state);
            yDE.SetExpression(Expression.Parameter(typeof(int), "y"), state);

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(x - y);
            result.TryGetExpression(state, out Expression expr).Should().BeTrue();
            expr.ToString().Should().BeEquivalentTo("(x - y)");
        }

        [Theory]
        [InlineData(5)]
        public void Test_NEG(int x)
        {
            Int4 xDE = new Int4(x);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_NEG__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            xDE.SetExpression(Expression.Parameter(typeof(int), "x"), state);

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(-x);
            result.TryGetExpression(state, out Expression expr).Should().BeTrue();
            expr.ToString().Should().BeEquivalentTo("-x");
        }
    }
}
