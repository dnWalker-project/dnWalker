using dnlib.DotNet;

using dnWalker.Concolic;
using dnWalker.Concolic.Traversal;
using dnWalker.Symbolic;
using dnWalker.Traversal;

using FluentAssertions;

using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.Tests.Examples.Features.PrimitiveValues
{
    public class PrimitiveValuesTests : ExamplesTestBase
    {
        public PrimitiveValuesTests(ITestOutputHelper output) : base(output)
        {
        }

        [ExamplesTest]
        public void NoBranch(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.PrimitiveValues.MethodsWithPrimitiveValueParameter.NoBranch");

            result.Iterations.Should().HaveCount(1);
            result.Iterations[0].Output.Trim().Should().Be("no branching");
        }

        [ExamplesTest]
        public void BranchIfPositive(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.PrimitiveValues.MethodsWithPrimitiveValueParameter.BranchIfPositive");

            result.Iterations.Should().HaveCount(2);
            result.Iterations[0].Output.Trim().Should().Be("x <= 0");
            result.Iterations[1].Output.Trim().Should().Be("x > 0");
        }

        [ExamplesTest]
        public void NestedBranching(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.PrimitiveValues.MethodsWithPrimitiveValueParameter.NestedBranching");

            result.Iterations.Should().HaveCount(4);
            result.Iterations[0].Output.Trim().Should().Be("(x <= 0)\r\n(x >= -3)");
            result.Iterations[1].Output.Trim().Should().Be("(x <= 0)\r\n(x < -3)");
            result.Iterations[2].Output.Trim().Should().Be("(x > 0)\r\n(x >= 5)");
            result.Iterations[3].Output.Trim().Should().Be("(x > 0)\r\n(x < 5)");
        }

        [ExamplesTest]
        public void NestedBranchingUnSat(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.PrimitiveValues.MethodsWithPrimitiveValueParameter.NestedBranchingUnsat");

            result.Iterations.Should().HaveCount(3);
            result.Iterations[0].Output.Trim().Should().Be("(x <= 0)\r\n(x >= -3)");
            result.Iterations[1].Output.Trim().Should().Be("(x <= 0)\r\n(x < -3)");
            result.Iterations[2].Output.Trim().Should().Be("(x > 0)\r\n(x >= -5)");
        }

        [ExamplesTest]
        public void MultipleBranchingWithStateChanges(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.PrimitiveValues.MethodsWithPrimitiveValueParameter.MultipleBranchingWithStateChanges");

            result.Iterations.Should().HaveCount(4);
            result.Iterations[0].Output.Trim().Should().Be("x >= y");
            result.Iterations[1].Output.Trim().Should().Be("x < y");
            // in this order, because solver tends to "saturate" values to their limits
            // so instead of satisfying 'x < 0' with 'x := -1', it returns 'x := int.MinValue', as such, 'x + 10 < 0' (which is in fact 'y')
            result.Iterations[2].Output.Trim().Should().Be("x < 0 => x = x + 10\r\nx < y"); 
            result.Iterations[3].Output.Trim().Should().Be("x < 0 => x = x + 10\r\nx >= y");
        }

        [ExamplesTest]
        public void MultipleBranchingWithoutStateChanges(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.PrimitiveValues.MethodsWithPrimitiveValueParameter.MultipleBranchingWithoutStateChanges");

            result.Iterations.Should().HaveCount(4);
            result.Iterations[0].Output.Trim().Should().Be("x >= y");
            result.Iterations[1].Output.Trim().Should().Be("x < y");
            result.Iterations[2].Output.Trim().Should().Be("x < 0\r\nx < y");
            result.Iterations[3].Output.Trim().Should().Be("x < 0\r\nx >= y");
        }

        [ExamplesTest]
        public void DivideByZero(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.PrimitiveValues.MethodsWithPrimitiveValueParameter.DivideByZero");

            MethodDef entryPoint = result.EntryPoint;

            result.Iterations.Should().HaveCount(3);
            result.Iterations[0].SymbolicContext.InputModel.TryGetValue(Variable.MethodArgument(entryPoint.Parameters[0]), out IValue? xValue).Should().BeTrue();
            xValue.Should().BeOfType<PrimitiveValue<int>>();
            ((PrimitiveValue<int>)xValue!).Value.Should().Be(0);
            result.Iterations[0].Exception.Type.FullName.Should().Be("System.DivideByZeroException");

            result.Iterations[1].SymbolicContext.InputModel.TryGetValue(Variable.MethodArgument(entryPoint.Parameters[0]), out xValue).Should().BeTrue();
            xValue.Should().BeOfType<PrimitiveValue<int>>();
            ((PrimitiveValue<int>)xValue!).Value.Should().NotBe(0);
            
            result.Iterations[1].Output.Trim().Should().Be("60 / x <= 10");
            result.Iterations[2].Output.Trim().Should().Be("60 / x > 10");
        }

        [ExamplesTest]
        public void RemainderOfZero(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.PrimitiveValues.MethodsWithPrimitiveValueParameter.RemainderOfZero");

            MethodDef entryPoint = result.EntryPoint;

            result.Iterations.Should().HaveCount(3);
            result.Iterations[0].SymbolicContext.InputModel.TryGetValue(Variable.MethodArgument(entryPoint.Parameters[0]), out IValue? xValue).Should().BeTrue();
            xValue.Should().BeOfType<PrimitiveValue<int>>();
            ((PrimitiveValue<int>)xValue!).Value.Should().Be(0);
            result.Iterations[0].Exception.Type.FullName.Should().Be("System.DivideByZeroException");

            result.Iterations[1].SymbolicContext.InputModel.TryGetValue(Variable.MethodArgument(entryPoint.Parameters[0]), out xValue).Should().BeTrue();
            xValue.Should().BeOfType<PrimitiveValue<int>>();
            ((PrimitiveValue<int>)xValue!).Value.Should().NotBe(0);

            result.Iterations[1].Output.Trim().Should().Be("60 % x > 3");
            result.Iterations[2].Output.Trim().Should().Be("60 % x <= 3");

        }
    }
}
