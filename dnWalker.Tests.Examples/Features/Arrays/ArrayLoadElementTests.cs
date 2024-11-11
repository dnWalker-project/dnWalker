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
    public class ArrayLoadElementTests : ExamplesTestBase
    {
        public ArrayLoadElementTests(ITestOutputHelper output) : base(output)
        {
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
