using dnWalker.Concolic;

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
    [Trait("Category", "Examples")]
    public class RandTests : ExamplesTestBase
    {
        public RandTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void Rand()
        {
            IExplorer explorer = GetConcolicExplorerBuilder().Build();
            explorer.GetConfiguration().SetCustomSetting("evaluateRandom", true);

            explorer.Run("Examples.Rand.Go");

            var paths = explorer.PathStore.Paths;
            paths.Should().HaveCount(6);
            paths.Where(p => p.Exception != null).Should().HaveCount(2);
            paths.Select(p => p.Length).Should().AllBeEquivalentTo(3);
        }
    }
}
