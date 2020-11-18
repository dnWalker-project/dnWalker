using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace dnWalker.Tests.ExampleTests.Simple
{
    [Trait("Category", "Examples")]
    public class BranchesTests : ExamplesTestBase
    {
        public BranchesTests() : base(Lazy.Value)
        {
        }

        [Fact]
        public void CaptureOutputTest1()
        {
            Explore("Examples.Branches.Branch",
                null,
                (explorer) =>
                {
                    explorer.GetUnhandledException().Should().BeNull();
                    explorer.GetExploredPaths().Count().Should().Be(1);
                    var output = explorer.GetExploredPaths().First().Output;
                    output.Should().Be($"X=1{Environment.NewLine}Y=1{Environment.NewLine}Z=1{Environment.NewLine}");
                });
        }
    }
}
