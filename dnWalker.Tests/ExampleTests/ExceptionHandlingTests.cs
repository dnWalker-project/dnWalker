using FluentAssertions;
using MMC.Data;
using Xunit;

namespace dnWalker.Tests.ExampleTests
{
    [Trait("Category", "Examples")]
    public class ExceptionHandlingTests : TestBase
    {
        public ExceptionHandlingTests() : base(@"..\..\..\Examples\bin\debug\Examples.exe")
        {
        }

        [Theory]
        [InlineData(4, 0, false)]
        [InlineData(4, 1, true)]
        public void CallMethodWithFinally(int x, int y, bool expectedResult)
        {
            var retValue = (Int8)Test("Examples.ExceptionHandling.MethodWithFinally", x, y);
            (retValue.Value > 0).Should().Be(expectedResult);
        }
    }
}
