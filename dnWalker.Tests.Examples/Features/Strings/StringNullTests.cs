using dnWalker.Concolic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;
using System.Runtime.CompilerServices;

namespace dnWalker.Tests.Examples.Features.Strings
{
    public class StringNullTests : MethodsWithStringParameterTestsBase
    {
        public StringNullTests(ITestOutputHelper output) : base(output)
        {
        }

        [ExamplesTest]
        public void BranchOnNull(BuildInfo buildInfo)
        {
            ExplorationResult result = Run(CreateExplorer(buildInfo));

            result.Iterations.Should().HaveCount(2);
            result.Iterations[0].Output.Trim().Should().Be("input is null");
            result.Iterations[1].Output.Trim().Should().Be("input is not null");
        }

        [ExamplesTest]
        public void BranchOnNullOrEmpty(BuildInfo buildInfo)
        {
            ExplorationResult result = Run(CreateExplorer(buildInfo));

            result.Iterations.Should().HaveCount(3);
            result.Iterations[0].Output.Trim().Should().Be("input is null or empty");
            result.Iterations[1].Output.Trim().Should().Be("input is null or empty");
            result.Iterations[2].Output.Trim().Should().Be("input is not null and not empty");
        }
    }
}
