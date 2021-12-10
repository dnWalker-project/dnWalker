using dnWalker.Tests.ExampleTests;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.ExampleTests
{
    public class DeadlockTests : ExamplesTestBase
    {
        public DeadlockTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void Go()
        {
            MMC.Explorer explorer = GetModelCheckerBuilder("Deadlock.Go").BuildAndRun();
            explorer.GetUnhandledException().Should().BeNull();
            explorer.Statistics.Deadlocks.Should().BeGreaterThan(0, "deadlock should have been detected.");
        }
    }
}
