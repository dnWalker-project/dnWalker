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

namespace dnWalker.Tests.InterpreterExtensionTests.BranchProducerExtension
{
    public class BranchEqualTests : InterpreterExtensionTestBase
    {
        public BranchEqualTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        // B** instructions - returns 8 if passes, 0 if not

        [Theory, CombinatorialData]
        public void Test_BEQ([CombinatorialValues(5, 10)] int x, [CombinatorialValues(5, 10)] int y, [CombinatorialValues(true, false)] bool isXSymb, [CombinatorialValues(true, false)] bool isYSymb)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);

            bool shouldBranch = x == y;

            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_BEQ__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            if (isXSymb) xDE.SetExpression(Expression.Parameter(typeof(int), "x"), state);
            if (isYSymb) yDE.SetExpression(Expression.Parameter(typeof(int), "y"), state);

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(shouldBranch ? 8 : 0);

            if (isXSymb || isYSymb)
            {
                string constraint = explorer.PathStore.CurrentPath.PathConstraint.ToString();

                if (isXSymb && isYSymb) constraint.Should().Be(shouldBranch ? $"(x == y)" : $"(x != y)");
                if (isXSymb && !isYSymb) constraint.Should().Be(shouldBranch ? $"(x == {y})" : $"(x != {y})");
                if (!isXSymb && isYSymb) constraint.Should().Be(shouldBranch ? $"({x} == y)" : $"({x} != y)");
            }
        }

        [Theory, CombinatorialData]
        public void Test_BGE([CombinatorialValues(5, 10)] int x, [CombinatorialValues(5, 10)] int y, [CombinatorialValues(true, false)] bool isXSymb, [CombinatorialValues(true, false)] bool isYSymb)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);

            bool shouldBranch = x >= y;

            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_BGE__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            if (isXSymb) xDE.SetExpression(Expression.Parameter(typeof(int), "x"), state);
            if (isYSymb) yDE.SetExpression(Expression.Parameter(typeof(int), "y"), state);

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(shouldBranch ? 8 : 0);

            if (isXSymb || isYSymb)
            {
                string constraint = explorer.PathStore.CurrentPath.PathConstraint.ToString();

                if (isXSymb && isYSymb) constraint.Should().Be(shouldBranch ? $"(x >= y)" : $"(x < y)");
                if (isXSymb && !isYSymb) constraint.Should().Be(shouldBranch ? $"(x >= {y})" : $"(x < {y})");
                if (!isXSymb && isYSymb) constraint.Should().Be(shouldBranch ? $"({x} >= y)" : $"({x} < y)");
            }
        }

        [Theory, CombinatorialData]
        public void Test_BGT([CombinatorialValues(5, 10)] int x, [CombinatorialValues(5, 10)] int y, [CombinatorialValues(true, false)] bool isXSymb, [CombinatorialValues(true, false)] bool isYSymb)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);

            bool shouldBranch = x > y;

            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_BGT__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            if (isXSymb) xDE.SetExpression(Expression.Parameter(typeof(int), "x"), state);
            if (isYSymb) yDE.SetExpression(Expression.Parameter(typeof(int), "y"), state);

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(shouldBranch ? 8 : 0);

            if (isXSymb || isYSymb)
            {
                string constraint = explorer.PathStore.CurrentPath.PathConstraint.ToString();

                if (isXSymb && isYSymb) constraint.Should().Be(shouldBranch ? $"(x > y)" : $"(x <= y)");
                if (isXSymb && !isYSymb) constraint.Should().Be(shouldBranch ? $"(x > {y})" : $"(x <= {y})");
                if (!isXSymb && isYSymb) constraint.Should().Be(shouldBranch ? $"({x} > y)" : $"({x} <= y)");
            }
        }

        [Theory, CombinatorialData]
        public void Test_BLE([CombinatorialValues(5, 10)] int x, [CombinatorialValues(5, 10)] int y, [CombinatorialValues(true, false)] bool isXSymb, [CombinatorialValues(true, false)] bool isYSymb)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);

            bool shouldBranch = x <= y;

            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_BLE__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            if (isXSymb) xDE.SetExpression(Expression.Parameter(typeof(int), "x"), state);
            if (isYSymb) yDE.SetExpression(Expression.Parameter(typeof(int), "y"), state);

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(shouldBranch ? 8 : 0);

            if (isXSymb || isYSymb)
            {
                string constraint = explorer.PathStore.CurrentPath.PathConstraint.ToString();

                if (isXSymb && isYSymb) constraint.Should().Be(shouldBranch ? $"(x <= y)" : $"(x > y)");
                if (isXSymb && !isYSymb) constraint.Should().Be(shouldBranch ? $"(x <= {y})" : $"(x > {y})");
                if (!isXSymb && isYSymb) constraint.Should().Be(shouldBranch ? $"({x} <= y)" : $"({x} > y)");
            }
        }

        [Theory, CombinatorialData]
        public void Test_BLT([CombinatorialValues(5, 10)] int x, [CombinatorialValues(5, 10)] int y, [CombinatorialValues(true, false)] bool isXSymb, [CombinatorialValues(true, false)] bool isYSymb)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);

            bool shouldBranch = x < y;

            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_BLT__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            if (isXSymb) xDE.SetExpression(Expression.Parameter(typeof(int), "x"), state);
            if (isYSymb) yDE.SetExpression(Expression.Parameter(typeof(int), "y"), state);

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(shouldBranch ? 8 : 0);

            if (isXSymb || isYSymb)
            {
                string constraint = explorer.PathStore.CurrentPath.PathConstraint.ToString();

                if (isXSymb && isYSymb) constraint.Should().Be(shouldBranch ? $"(x < y)" : $"(x >= y)");
                if (isXSymb && !isYSymb) constraint.Should().Be(shouldBranch ? $"(x < {y})" : $"(x >= {y})");
                if (!isXSymb && isYSymb) constraint.Should().Be(shouldBranch ? $"({x} < y)" : $"({x} >= y)");
            }
        }

        [Theory, CombinatorialData]
        public void Test_BNE_UN([CombinatorialValues(5, 10)] int x, [CombinatorialValues(5, 10)] int y, [CombinatorialValues(true, false)] bool isXSymb, [CombinatorialValues(true, false)] bool isYSymb)
        {
            Int4 xDE = new Int4(x);
            Int4 yDE = new Int4(y);

            bool shouldBranch = x != y;

            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE, yDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_BNE_UN__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            if (isXSymb) xDE.SetExpression(Expression.Parameter(typeof(int), "x"), state);
            if (isYSymb) yDE.SetExpression(Expression.Parameter(typeof(int), "y"), state);

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(shouldBranch ? 8 : 0);

            if (isXSymb || isYSymb)
            {
                string constraint = explorer.PathStore.CurrentPath.PathConstraint.ToString();

                if (isXSymb && isYSymb) constraint.Should().Be(shouldBranch ? $"(x != y)" : $"(x == y)");
                if (isXSymb && !isYSymb) constraint.Should().Be(shouldBranch ? $"(x != {y})" : $"(x == {y})");
                if (!isXSymb && isYSymb) constraint.Should().Be(shouldBranch ? $"({x} != y)" : $"({x} == y)");
            }
        }

        [Theory, CombinatorialData]
        public void Test_BRFALSE([CombinatorialValues(true, false)] bool x, [CombinatorialValues(true, false)] bool isXSymb)
        {
            Int4 xDE = new Int4(x ? 1 : 0);

            bool shouldBranch = !x;

            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_BRFALSE__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            if (isXSymb) xDE.SetExpression(Expression.Parameter(typeof(int), "x"), state);

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(shouldBranch ? 1 : 0);

            if (isXSymb)
            {
                string constraint = explorer.PathStore.CurrentPath.PathConstraint.ToString();
                constraint.Should().Be(shouldBranch ? $"Not(x)" : $"x");
            }
        }

        [Theory, CombinatorialData]
        public void Test_BRTRUE([CombinatorialValues(true, false)] bool x, [CombinatorialValues(true, false)] bool isXSymb)
        {
            Int4 xDE = new Int4(x ? 1 : 0);

            bool shouldBranch = x;

            Explorer explorer = GetModelCheckerBuilder()
                .SetArgs(xDE)
                .SetMethod("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.Test_BRTRUE__Int32")
                .Build();

            ExplicitActiveState state = explorer.ActiveState;
            if (isXSymb) xDE.SetExpression(Expression.Parameter(typeof(int), "x"), state);

            explorer.Run();

            Int4 result = (Int4)state.CurrentThread.RetValue;

            result.ToInt32(CultureInfo.InvariantCulture).Should().Be(shouldBranch ? 1 : 0);

            if (isXSymb)
            {
                string constraint = explorer.PathStore.CurrentPath.PathConstraint.ToString();
                constraint.Should().Be(shouldBranch ? $"x" : $"Not(x)");
            }
        }
    }
}
