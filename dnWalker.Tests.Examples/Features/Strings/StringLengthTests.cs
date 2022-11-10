using dnWalker.Concolic;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Examples.Features.Strings
{
    public class StringLengthTests : MethodsWithStringParameterTestsBase
    {
        public StringLengthTests(ITestOutputHelper output) : base(output)
        {
        }

        [ExamplesTest]
        public void BranchOnLength(BuildInfo buildInfo)
        {
            ExplorationResult result = Run(CreateExplorer(buildInfo));

            result.Iterations.Should().HaveCount(3);
            result.Iterations[0].Exception.Type.FullName.Should().Be("System.NullReferenceException");
            result.Iterations[1].Output.Trim().Should().Be("input.Length <= 5");
            result.Iterations[2].Output.Trim().Should().Be("input.Length > 5");
        }
    }
}
