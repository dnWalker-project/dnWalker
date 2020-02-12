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
            var retValue = Test("Examples.ExceptionHandling.MethodWithFinally", out var ex, 4, 0);
            ex.Should().NotBeNull();
        }

        [Theory]
        [InlineData(4, 0)]
        [InlineData(12, 5)]
        [InlineData(12, 4)]
        public void CallMethodWithConcreteExceptionHandlerWithoutParameter(int x, int y)
        {
            TestAndCompare("Examples.ExceptionHandling.MethodWithConcreteExceptionHandlerWithoutParameter", x, y);
        }

        [Theory]
        [InlineData(4, 0)]
        [InlineData(12, 5)]
        [InlineData(12, 4)]
        public void MethodWithoutCatch(int x, int y)
        {
            TestAndCompare("Examples.ExceptionHandling.MethodWithoutCatch", x, y);
        }
    }
}
