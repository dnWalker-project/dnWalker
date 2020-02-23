using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace dnWalker.Tests.ExampleTests
{
    [Trait("Category", "Examples")]
    public class RandTests : ExamplesTestBase
    {
        public RandTests() : base(Lazy.Value)
        {
        }

        [Fact]
        public void Rand()
        {
            Explore("Examples.Rand.Go",
                c =>
                {
                    c.SetCustomSetting("evaluateRandom", true);
                },
                (ex, stats) =>
                {
                    //ex.Should().BeNull();
                    //stats.Deadlocks.Should().BeGreaterThan(0, "deadlock should have been detected.");
                });
        }
    }
}
