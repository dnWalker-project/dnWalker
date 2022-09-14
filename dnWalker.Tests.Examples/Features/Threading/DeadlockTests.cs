using dnWalker.Concolic;

using FluentAssertions;

using MMC;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Examples.Features.Threading
{
    public class DeadlockTests : ExamplesTestBase
    {
        public DeadlockTests(ITestOutputHelper output) : base(output)
        {
        }

        [ExamplesTest]
        public void Go(BuildInfo buildInfo)
        {
            var result = CreateExplorer(buildInfo).Run("Deadlock.Go");

            result.Iterations.Should().HaveCount(1);
            result.Iterations[0].Exception.Should().BeNull();
            result.Iterations[0].Statistics.Deadlocks.Should().BeGreaterThan(0);
        }
    }
}
