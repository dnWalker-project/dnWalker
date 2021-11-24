using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.ExampleTests.DebugMode
{
    public class DeadlockTests : DebugExamplesTestBase
    {
        public DeadlockTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void Go()
        {
            ExploreModelChecker("Deadlock.Go",
                null,
                (ex, stats) =>
                {
                    ex.Should().BeNull();
                    stats.Deadlocks.Should().BeGreaterThan(0, "deadlock should have been detected.");
                });
        }
    }
}
