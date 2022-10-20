using dnWalker.Concolic;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Examples.Features.Strings
{
    public class StringContainsTests : MethodsWithStringParameterTestsBase
    {
        public StringContainsTests(ITestOutputHelper output) : base(output)
        {
        }

        [ExamplesTest]
        public void BranshOnContainsConstString(BuildInfo buildInfo)
        {
            ExplorationResult result = Run(CreateExplorer(buildInfo));

            result.Iterations.Should().HaveCount(3);
            result.Iterations[0].Output.Trim().Should().BeEmpty();
            result.Iterations[0].Exception.Type.Name.Should().Be("NullReferenceException");

            result.Iterations[1].Output.Trim().Should().Be("input doesn't contain HELLO_WORLD");
            result.Iterations[2].Output.Trim().Should().Be("input contains HELLO_WORLD");
        }

        [ExamplesTest]
        public void BranshOnContainsDynamicString(BuildInfo buildInfo)
        {
            ExplorationResult result = Run(CreateExplorer(buildInfo));

            result.Iterations.Should().HaveCount(4);
            result.Iterations[0].Output.Trim().Should().Be("$containee is null or empty");

            result.Iterations[1].Output.Trim().Should().BeEmpty();
            result.Iterations[1].Exception.Type.Name.Should().Be("NullReferenceException");

            result.Iterations[2].Output.Trim().Should().Be("input doesn't contain $containee");
            result.Iterations[3].Output.Trim().Should().Be("input contains $containee");
        }

        [ExamplesTest]
        public void BranchOnContainsConstChar(BuildInfo buildInfo)
        {
            ExplorationResult result = Run(CreateExplorer(buildInfo));

            result.Iterations.Should().HaveCount(3);
            result.Iterations[0].Output.Trim().Should().BeEmpty();
            result.Iterations[0].Exception.Type.Name.Should().Be("NullReferenceException");

            result.Iterations[1].Output.Trim().Should().Be("input doesn't contain 'Q'");
            result.Iterations[2].Output.Trim().Should().Be("input contains 'Q'");
        }

        [ExamplesTest]
        public void BranshOnContainsDynamicChar(BuildInfo buildInfo)
        {
            ExplorationResult result = Run(CreateExplorer(buildInfo));

            result.Iterations.Should().HaveCount(3);
            result.Iterations[0].Output.Trim().Should().BeEmpty();
            result.Iterations[0].Exception.Type.Name.Should().Be("NullReferenceException");

            result.Iterations[1].Output.Trim().Should().Be("input doesn't contain $symbol");
            result.Iterations[2].Output.Trim().Should().Be("input contains $symbol");
        }
    }
}
