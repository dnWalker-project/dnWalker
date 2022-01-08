using FluentAssertions;
using MMC.Data;
using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.ExampleTests.ReleaseMode
{
    [Trait("Category", "Examples")]
    public class IncDecTests : ReleaseExamplesTestBase
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
