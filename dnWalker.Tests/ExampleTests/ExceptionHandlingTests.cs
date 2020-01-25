using FluentAssertions;
using MMC.Data;
using System;
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
        [InlineData(4, 1, false)]
        public void CallMethodWithFinally(int x, int y, bool expectedResult)
        {
            var retValue = Test("Examples.ExceptionHandling.MethodWithFinally", x, y);
            retValue.Should().BeOfType<Int4>();
            var value = (Int4)retValue;
            (value.Value != 0).Should().Be(expectedResult, value.Value.ToString());
        }

        [Fact]
        public void CallMethodWithFinallyWithException()
        {
            Exception ex = Assert.Throws<Exception>(() =>
            {
                var retValue = Test("Examples.ExceptionHandling.MethodWithFinally", 4, 0);
            });
        }

        [Theory]
        [InlineData(4, 0, false)]
        [InlineData(12, 5, false)]
        [InlineData(12, 4, true)]
        public void CallMethodWithConcreteExceptionHandlerWithoutParameter(int x, int y, bool expectedResult)
        {
            var retValue = Test("Examples.ExceptionHandling.MethodWithConcreteExceptionHandlerWithoutParameter", x, y);
            retValue.Should().BeOfType<Int4>();
            var value = (Int4)retValue;
            (value.Value != 0).Should().Be(expectedResult, value.Value.ToString());
        }
    }
}
