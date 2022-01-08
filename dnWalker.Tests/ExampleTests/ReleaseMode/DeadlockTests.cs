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
    public class DeadlockTests : ReleaseExamplesTestBase
    {
        public DeadlockTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact(Skip = "Not handling multithreading right now.")]
        public void Go()
        {
            MMC.Explorer explorer = GetModelCheckerBuilder("Deadlock.Go").Build();
            explorer.Run();

            explorer.GetUnhandledException().Should().BeNull();
            explorer.Statistics.Deadlocks.Should().BeGreaterThan(0, "deadlock should have been detected.");
        }
    }
}
