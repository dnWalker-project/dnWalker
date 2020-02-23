using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace dnWalker.Tests.ExampleTests
{
    [Trait("Category", "Examples")]
    public class DeadlockTests : ExamplesTestBase
    {
        public DeadlockTests() : base(Lazy.Value)
        {
        }

        [Fact]
        public void Go()
        {
            Explore("Deadlock.Go",
                null,
                (ex, stats) => 
                {
                    ex.Should().BeNull();
                    stats.Deadlocks.Should().BeGreaterThan(0, "deadlock should have been detected.");
                });
        }
    }
}
