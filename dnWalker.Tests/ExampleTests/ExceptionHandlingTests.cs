using FluentAssertions;
using MMC.Data;
using System;
using Xunit;

namespace dnWalker.Tests.ExampleTests
{
    [Trait("Category", "Examples")]
    public class ExceptionHandlingTests : ExamplesTestBase
    {
        public ExceptionHandlingTests() : base(Lazy.Value)
        {
        }

        [Theory]
        [InlineData(4, 1)]
        public void CallMethodWithFinally(int x, int y)
        {
            TestAndCompare("Examples.ExceptionHandling.MethodWithFinally", x, y);
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
        [InlineData(4, 0)]
        [InlineData(12, 5)]
        [InlineData(12, 4)]
        public void CallMethodWithConcreteExceptionHandlerWithoutParameter(int x, int y)
        {
            TestAndCompare("Examples.ExceptionHandling.MethodWithConcreteExceptionHandlerWithoutParameter", x, y);
        }
    }
}
