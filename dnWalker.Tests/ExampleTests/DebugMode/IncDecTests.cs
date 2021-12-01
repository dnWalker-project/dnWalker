using FluentAssertions;
using MMC.Data;
using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.ExampleTests.DebugMode
{
    [Trait("Category", "Examples")]
    public class IncDecTests : DebugExamplesTestBase
    {
        public IncDecTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void CallMethodThreeWithArgument()
        {
            TestAndCompare("IncDec.Three", 4);
        }
    }
}
