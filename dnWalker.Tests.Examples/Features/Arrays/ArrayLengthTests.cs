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
    public class ArrayLengthTests : ExamplesTestBase
    {
        public ArrayLengthTests(ITestOutputHelper output) : base(output)
        {
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
    }
}
