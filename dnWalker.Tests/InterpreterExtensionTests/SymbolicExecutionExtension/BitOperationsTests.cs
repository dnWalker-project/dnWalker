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
    public class BitOperationsTests : InterpreterExtensionTestBase
    {
        public BitOperationsTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory, CombinatorialData]
        public void Test_AND([CombinatorialValues(5, 10)] int x, [CombinatorialValues(5, 10)] int y, [CombinatorialValues(true, false)] bool isXSymb, [CombinatorialValues(true, false)] bool isYSymb)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_AND__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            if (isXSymb) xDE.SetExpression(state, NamedInt("x"));
            if (isYSymb) yDE.SetExpression(state, NamedInt("y"));

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(x & y);
            result.TryGetExpression(state, out Expression expr).Should().Be(isXSymb || isYSymb);

            if (isXSymb && isYSymb) expr.ToString().Should().BeEquivalentTo($"(x & y)");
            if (isXSymb && !isYSymb) expr.ToString().Should().BeEquivalentTo($"(x & {y})");
            if (!isXSymb && isYSymb) expr.ToString().Should().BeEquivalentTo($"({x} & y)");
        }

        [Theory, CombinatorialData]
        public void Test_NOT([CombinatorialValues(5, 10)] int x, [CombinatorialValues(true, false)] bool isXSymb)
        {
            Int4 xDE = new Int4(x);

            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_NOT__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            if (isXSymb) xDE.SetExpression(state, NamedInt("x"));

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(~x);
            result.TryGetExpression(state, out Expression expr).Should().Be(isXSymb);

            if (isXSymb) expr.ToString().Should().BeEquivalentTo("!x");
        }

        [Theory, CombinatorialData]
        public void Test_OR([CombinatorialValues(5, 10)] int x, [CombinatorialValues(5, 10)] int y, [CombinatorialValues(true, false)] bool isXSymb, [CombinatorialValues(true, false)] bool isYSymb)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_OR__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            if (isXSymb) xDE.SetExpression(state, NamedInt("x"));
            if (isYSymb) yDE.SetExpression(state, NamedInt("y"));

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(x | y);
            result.TryGetExpression(state, out Expression expr).Should().Be(isXSymb || isYSymb);

            if (isXSymb && isYSymb) expr.ToString().Should().BeEquivalentTo($"(x | y)");
            if (isXSymb && !isYSymb) expr.ToString().Should().BeEquivalentTo($"(x | {y})");
            if (!isXSymb && isYSymb) expr.ToString().Should().BeEquivalentTo($"({x} | y)");
        }

        [Theory, CombinatorialData]
        public void Test_XOR([CombinatorialValues(5, 10)] int x, [CombinatorialValues(5, 10)] int y, [CombinatorialValues(true, false)] bool isXSymb, [CombinatorialValues(true, false)] bool isYSymb)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_XOR__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            if (isXSymb) xDE.SetExpression(state, NamedInt("x"));
            if (isYSymb) yDE.SetExpression(state, NamedInt("y"));

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(x ^ y);
            result.TryGetExpression(state, out Expression expr).Should().Be(isXSymb || isYSymb);

            if (isXSymb && isYSymb) expr.ToString().Should().BeEquivalentTo($"(x ^ y)");
            if (isXSymb && !isYSymb) expr.ToString().Should().BeEquivalentTo($"(x ^ {y})");
            if (!isXSymb && isYSymb) expr.ToString().Should().BeEquivalentTo($"({x} ^ y)");
        }

        [Theory, CombinatorialData]
        public void Test_SHL([CombinatorialValues(5, 10)] int x, [CombinatorialValues(5, 10)] int y, [CombinatorialValues(true, false)] bool isXSymb, [CombinatorialValues(true, false)] bool isYSymb)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_SHL__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            if (isXSymb) xDE.SetExpression(state, NamedInt("x"));
            if (isYSymb) yDE.SetExpression(state, NamedInt("y"));

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(x << y);
            result.TryGetExpression(state, out Expression expr).Should().Be(isXSymb || isYSymb);

            if (isXSymb && isYSymb) expr.ToString().Should().BeEquivalentTo($"(x << y)");
            if (isXSymb && !isYSymb) expr.ToString().Should().BeEquivalentTo($"(x << {y})");
            if (!isXSymb && isYSymb) expr.ToString().Should().BeEquivalentTo($"({x} << y)");
        }

        [Theory, CombinatorialData]
        public void Test_SHR([CombinatorialValues(5, 10)] int x, [CombinatorialValues(5, 10)] int y, [CombinatorialValues(true, false)] bool isXSymb, [CombinatorialValues(true, false)] bool isYSymb)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_SHR__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            if (isXSymb) xDE.SetExpression(state, NamedInt("x"));
            if (isYSymb) yDE.SetExpression(state, NamedInt("y"));

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(x >> y);
            result.TryGetExpression(state, out Expression expr).Should().Be(isXSymb || isYSymb);

            if (isXSymb && isYSymb) expr.ToString().Should().BeEquivalentTo($"(x >> y)");
            if (isXSymb && !isYSymb) expr.ToString().Should().BeEquivalentTo($"(x >> {y})");
            if (!isXSymb && isYSymb) expr.ToString().Should().BeEquivalentTo($"({x} >> y)");
        }

        [Theory, CombinatorialData]
        public void Test_SHR_UN([CombinatorialValues(5, 10)] uint x, [CombinatorialValues(5, 10)] int y, [CombinatorialValues(true, false)] bool isXSymb, [CombinatorialValues(true, false)] bool isYSymb)
        {
            UnsignedInt4 xDE = new UnsignedInt4(x);
            Int4 yDE = new Int4(y);


            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_SHR__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            if (isXSymb) xDE.SetExpression(state, NamedInt("x"));
            if (isYSymb) yDE.SetExpression(state, NamedInt("y"));

            explorer.Run();

            UnsignedInt4 result = (UnsignedInt4)state.CurrentThread.RetValue;

            result.ToUInt32(CultureInfo.InvariantCulture).Should().Be(x >> y);
            result.TryGetExpression(state, out Expression expr).Should().Be(isXSymb || isYSymb);

            if (isXSymb && isYSymb) expr.ToString().Should().BeEquivalentTo($"(x >> y)");
            if (isXSymb && !isYSymb) expr.ToString().Should().BeEquivalentTo($"(x >> {y})");
            if (!isXSymb && isYSymb) expr.ToString().Should().BeEquivalentTo($"({x} >> y)");
        }
    }
}
