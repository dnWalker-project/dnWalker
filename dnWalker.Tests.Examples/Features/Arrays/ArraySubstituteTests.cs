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
    public class ArraySubstituteTests : ExamplesTestBase
    {
        public ArraySubstituteTests(ITestOutputHelper output) : base(output)
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

        [ExamplesTest]
        public void BranchIfLengthLessThan5(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.BranchIfLengthLessThan5");

            result.Iterations.Should().HaveCount(3);

            result.Iterations[0].Output.Trim().Should().Be("instance is null || length is greater than 5");
            result.Iterations[1].Output.Trim().Should().Be("instance is not null && length is less than 5");
            result.Iterations[2].Output.Trim().Should().Be("instance is null || length is greater than 5");
        }

        [ExamplesTest]
        public void BranchIfLengthGreaterThan5(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.BranchIfLengthGreaterThan5");

            result.Iterations.Should().HaveCount(3);

            result.Iterations[0].Output.Trim().Should().Be("instance is null || length is less than 5");
            result.Iterations[1].Output.Trim().Should().Be("instance is null || length is less than 5");
            result.Iterations[2].Output.Trim().Should().Be("instance is not null && length is greater than 5");
        }

        [ExamplesTest]
        public void BranchIfItemAtStaticIndexIsGreaterThan5(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.BranchIfItemAtStaticIndexIsGreaterThan5");

            result.Iterations.Should().HaveCount(4);

            result.Iterations[0].Output.Trim().Should().Be("array is null");
            result.Iterations[1].Output.Trim().Should().Be("length is less than 4");
            result.Iterations[2].Output.Trim().Should().Be("array[3] <= 5");
            result.Iterations[3].Output.Trim().Should().Be("array[3] > 5");
        }

        [ExamplesTest]
        // fails due to not yet handling exception edges when building constraint tree
        public void BranchIfItemAtDynamicIndexIsGreaterThan5(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.BranchIfItemAtDynamicIndexIsGreaterThan5");

            foreach (var it in result.Iterations)
            {
                Output.WriteLine(it.GetPathInfo());
            }

            result.Iterations.Should().HaveCount(5);

            result.Iterations[0].Output.Trim().Should().Be("array is null");
            result.Iterations[1].Output.Trim().Should().Be("length is less than index");
            result.Iterations[2].Exception.Type.FullName.Should().Be("System.IndexOutOfRangeException");
            result.Iterations[3].Output.Trim().Should().Be("array[index] <= 5");
            result.Iterations[4].Output.Trim().Should().Be("array[index] > 5");
        }
    }
}
