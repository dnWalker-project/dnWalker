using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.ExampleTests.ReleaseMode
{
    [Trait("Category", "PatternMatching")]
    public class PatternMatchingTests : ReleaseExamplesTestBase
    {
        public PatternMatchingTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        protected override void TestAndCompare(string methodName, params object[] args)
        {
            methodName = "Examples.Interpreter.PatternMatching." + methodName;
            base.TestAndCompare(methodName, args);
        }

        [Theory]
        [InlineData("circle")]
        [InlineData("square")]
        [InlineData("large-circle")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("unknown")]
        public void CreateShape(string shapeName)
        {
            TestAndCompare("CreateShape", shapeName);
        }

        /*public void ComputeAreaModernIs()
        {

        }*/
    }
}
