using FluentAssertions;

using MMC.Data;

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
            MMC.Explorer explorer = GetModelCheckerBuilder("Deadlock.Go").WithArgs(Array.Empty<IDataElement>()).BuildAndRun();
            explorer.GetUnhandledException().Should().BeNull();
            explorer.Statistics.Deadlocks.Should().BeGreaterThan(0, "deadlock should have been detected.");
        }
    }
}
