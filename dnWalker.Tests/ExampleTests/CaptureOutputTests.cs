using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace dnWalker.Tests.ExampleTests
{
    [Trait("Category", "Examples")]
    public class CaptureOutputTests : ExamplesTestBase
    {
        public CaptureOutputTests() : base(Lazy.Value)
        {
        }

        [Fact]
        public void CallMethodThreeWithArgument()
        {
            Explore("Examples.CaptureOutput.Capture1",
                c => {},
                (ex, stats) =>
                {
                    //ex.Should().BeNull();
                    //stats.Deadlocks.Should().BeGreaterThan(0, "deadlock should have been detected.");
                });
        }
    }
}
