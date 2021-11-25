using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.ExampleTests.ReleaseMode
{
    [Trait("Category", "Examples")]
    public class RandTests : ReleaseExamplesTestBase
    {
        public RandTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void Rand()
        {
            Explore("Examples.Rand.Go",
                c =>
                {
                    c.MaxIterations = 1;
                    c.SetCustomSetting("evaluateRandom", true);
                },
                finished: explorer =>
                {
                    var paths = explorer.PathStore.Paths;
                    paths.Should().HaveCount(6);
                    paths.Where(p => p.Exception != null).Should().HaveCount(2);
                    paths.Select(p => p.Length).Should().AllBeEquivalentTo(3);
                });
        }
    }
}
