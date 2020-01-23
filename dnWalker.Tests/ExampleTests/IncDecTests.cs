using FluentAssertions;
using MMC.Data;
using Xunit;

namespace dnWalker.Tests.ExampleTests
{
    [Trait("Category", "Examples")]
    public class IncDecTests : TestBase
    {
        public IncDecTests() : base(@"..\..\..\Examples\bin\debug\Examples.exe")
        {
        }

        [Fact]
        public void CallMethodThreeWithArgument()
        {
            var retValue = (Int8)Test("IncDec.Three", 4);
            retValue.Value.Should().Be(4 * 12);
        }
    }
}
