using FluentAssertions;
using MMC.Data;
using Xunit;

namespace dnWalker.Tests.ExampleTests
{
    [Trait("Category", "Examples")]
    public class IncDecTests : ExamplesTestBase
    {
        public IncDecTests() : base(Lazy.Value)
        {
        }

        [Fact]
        public void CallMethodThreeWithArgument()
        {
            TestAndCompare("IncDec.Three", 4);
        }
    }
}
