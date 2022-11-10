using dnWalker.Concolic;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;


namespace dnWalker.Tests.Examples.Features.Arrays
{
    public class ArrayNullabilityTests : ExamplesTestBase
    {
        public ArrayNullabilityTests(ITestOutputHelper output) : base(output)
        {
        }

        [ExamplesTest]
        public void BranchIfNull(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.BranchIfNull");

            result.Iterations.Should().HaveCount(2);

            result.Iterations[0].SymbolicContext.InputModel.HeapInfo.IsEmpty().Should().BeTrue();
            result.Iterations[0].Output.Trim().Should().Be("instance is null");
            result.Iterations[1].SymbolicContext.InputModel.HeapInfo.IsEmpty().Should().BeFalse();
            result.Iterations[1].SymbolicContext.InputModel.HeapInfo.Locations.Should().HaveCount(1);
            result.Iterations[1].Output.Trim().Should().Be("instance is not null");
        }

        [ExamplesTest]
        public void BranchIfNotNull(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.BranchIfNotNull");

            result.Iterations.Should().HaveCount(2);

            result.Iterations[0].SymbolicContext.InputModel.HeapInfo.IsEmpty().Should().BeTrue();
            result.Iterations[0].Output.Trim().Should().Be("instance is null");
            result.Iterations[1].SymbolicContext.InputModel.HeapInfo.IsEmpty().Should().BeFalse();
            result.Iterations[1].SymbolicContext.InputModel.HeapInfo.Locations.Should().HaveCount(1);
            result.Iterations[1].Output.Trim().Should().Be("instance is not null");
        }
    }
}
