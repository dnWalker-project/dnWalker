using FluentAssertions;

using MMC;
using MMC.Data;
using System;
using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.ExampleTests.DebugMode
{
    [Trait("Category", "Examples")]
    public class ExceptionHandlingTests : DebugExamplesTestBase
    {
        public ExceptionHandlingTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
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
            IModelCheckerExplorerBuilder builder = GetModelCheckerBuilder("Examples.ExceptionHandling.MethodWithFinally")
                .SetArgs( new IDataElement[]
            {
                new Int4(4),
                new Int4(0)
            });

            Explorer explorer = builder.Build();
            explorer.Run();
            explorer.GetUnhandledException().Should().NotBeNull();
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
