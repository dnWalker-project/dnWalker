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
    public class LogicTests : InterpreterExtensionTestBase
    {
        public LogicTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }



        [Theory]
        [InlineData(5, 10)]
        public void Test_AND(int x, int y)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_AND__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            xDE.SetExpression(Expression.Parameter(typeof(int), "x"), state);
            yDE.SetExpression(Expression.Parameter(typeof(int), "y"), state);

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(x & y);
            result.TryGetExpression(state, out Expression expr).Should().BeTrue();
            expr.ToString().Should().BeEquivalentTo("(x & y)");
        }

        [Theory]
        [InlineData(5)]
        public void Test_NOT(int x)
        {
            Int4 xDE = new Int4(x);

            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_NOT__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            xDE.SetExpression(Expression.Parameter(typeof(int), "x"), state);

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(~x);
            result.TryGetExpression(state, out Expression expr).Should().BeTrue();
            expr.ToString().Should().BeEquivalentTo("Not(x)");

        }

        [Theory]
        [InlineData(5, 10)]
        public void Test_OR(int x, int y)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_OR__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            xDE.SetExpression(Expression.Parameter(typeof(int), "x"), state);
            yDE.SetExpression(Expression.Parameter(typeof(int), "y"), state);

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(x | y);
            result.TryGetExpression(state, out Expression expr).Should().BeTrue();
            expr.ToString().Should().BeEquivalentTo("(x | y)");
        }

        [Theory]
        [InlineData(5, 10)]
        public void Test_XOR(int x, int y)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_XOR__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            xDE.SetExpression(Expression.Parameter(typeof(int), "x"), state);
            yDE.SetExpression(Expression.Parameter(typeof(int), "y"), state);

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(x ^ y);
            result.TryGetExpression(state, out Expression expr).Should().BeTrue();
            expr.ToString().Should().BeEquivalentTo("(x ^ y)");
        }

        [Theory]
        [InlineData(5, 10)]
        public void Test_SHL(int x, int y)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_SHL__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            xDE.SetExpression(Expression.Parameter(typeof(int), "x"), state);
            yDE.SetExpression(Expression.Parameter(typeof(int), "y"), state);

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(x << y);
            result.TryGetExpression(state, out Expression expr).Should().BeTrue();
            expr.ToString().Should().BeEquivalentTo("(x << y)");
        }

        [Theory]
        [InlineData(5, 10)]
        public void Test_SHR(int x, int y)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_SHR__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            xDE.SetExpression(Expression.Parameter(typeof(int), "x"), state);
            yDE.SetExpression(Expression.Parameter(typeof(int), "y"), state);

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(x >> y);
            result.TryGetExpression(state, out Expression expr).Should().BeTrue();
            expr.ToString().Should().BeEquivalentTo("(x >> y)");
        }

        [Theory]
        [InlineData(5, 10)]
        public void Test_SHR_UN(uint x, int y)
        {
            UnsignedInt4 xDE = new UnsignedInt4(x);
            Int4 yDE = new Int4(y);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_SHR__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            xDE.SetExpression(Expression.Parameter(typeof(uint), "x"), state);
            yDE.SetExpression(Expression.Parameter(typeof(int), "y"), state);

            explorer.Run();

            UnsignedInt4 result = (UnsignedInt4)state.CurrentThread.RetValue;

            result.ToUInt32(CultureInfo.InvariantCulture).Should().Be(x >> y);
            result.TryGetExpression(state, out Expression expr).Should().BeTrue();
            expr.ToString().Should().BeEquivalentTo("(x >> y)");
        }
    }
}
