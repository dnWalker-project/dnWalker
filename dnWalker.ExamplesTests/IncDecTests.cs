using dnWalker.Tests.ExampleTests;

using FluentAssertions;
using MMC.Data;
using Xunit;
using Xunit.Abstractions;

namespace dnWalker.ExampleTests
{
    [Trait("Category", "Examples")]
    public class IncDecTests : ExamplesTestBase
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
