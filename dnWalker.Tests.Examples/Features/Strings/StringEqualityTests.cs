using dnWalker.Concolic;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Examples.Features.Strings
{
    public class StringEqualityTests : MethodsWithStringParameterTestsBase
    {
        public StringEqualityTests(ITestOutputHelper output) : base(output)
        {
        }

        [ExamplesTest]
        public void BranchOnConstEquality(BuildInfo buildInfo)
        {
            ExplorationResult result = Run(CreateExplorer(buildInfo));

            result.Iterations.Should().HaveCount(2);
            result.Iterations[0].Output.Trim().Should().Be("input is not hello world");
            result.Iterations[1].Output.Trim().Should().Be("input is hello world");
        }

        [ExamplesTest]
        public void BranchOnNonConstEquality(BuildInfo buildInfo)
        {
            ExplorationResult result = Run(CreateExplorer(buildInfo));

            result.Iterations.Should().HaveCount(4);
            result.Iterations[0].Output.Trim().Should().Be("input1 or input2 is null");
            result.Iterations[1].Output.Trim().Should().Be("input1 or input2 is null");

            result.Iterations[2].Output.Trim().Should().Be("input1 != input2");
            result.Iterations[3].Output.Trim().Should().Be("input1 == input2"); 
        }

        [ExamplesTest]
        public void BranchOnCharAtStaticIndexConstEquality(BuildInfo buildInfo)
        {
            ExplorationResult result = Run(CreateExplorer(buildInfo));

            result.Iterations.Should().HaveCount(5);
            result.Iterations[0].Exception.Type.FullName.Should().Be("System.NullReferenceException"); // input is null
            result.Iterations[1].Exception.Type.FullName.Should().Be("System.IndexOutOfRangeException"); // input.Length == 0 && 3 > 0
            result.Iterations[2].Output.Trim().Should().Be("input[$index] != 'a'"); // input is a string with length at least 4 but not 'a' at index 3
            result.Iterations[3].Output.Trim().Should().Be("input[$index] == 'a'"); // input is a string with length at least 4 and 'a' at index 3
        }
    }
}
