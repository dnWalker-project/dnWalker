using dnWalker.Concolic;

using FluentAssertions;

namespace dnWalker.Tests.Examples.Features.Objects
{
    public class ObjectAbstractMethodCallTests : ExamplesTestBase
    {
        public ObjectAbstractMethodCallTests(ITestOutputHelper output) : base(output)
        {
        }

        [ExamplesTest]
        public void InvokeAbstractMethodOnAbstractClass(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.InvokeAbstractMethodOnAbstractClass");

            result.Iterations.Should().HaveCount(8);
            result.Iterations[0].Output.Trim().Should().Be("instance is null");
            result.Iterations[1].Output.Trim().Should().Be("instance.AbstractMethod()|0| is null");

            result.Iterations[2].Output.Trim().Should().Be("instance.AbstractMethod()|0|.OtherField < 5\r\ninstance.AbstractMethod()|1| is null");
            result.Iterations[3].Output.Trim().Should().Be("instance.AbstractMethod()|0|.OtherField < 5\r\ninstance.AbstractMethod()|1|.OtherField < 5");

            result.Iterations[4].Output.Trim().Should().Be("instance.AbstractMethod()|0|.OtherField < 5\r\ninstance.AbstractMethod()|1|.OtherField >= 5");

            result.Iterations[5].Output.Trim().Should().Be("instance.AbstractMethod()|0|.OtherField >= 5\r\ninstance.AbstractMethod()|1| is null");
            result.Iterations[6].Output.Trim().Should().Be("instance.AbstractMethod()|0|.OtherField >= 5\r\ninstance.AbstractMethod()|1|.OtherField < 5");

            result.Iterations[7].Output.Trim().Should().Be("instance.AbstractMethod()|0|.OtherField >= 5\r\ninstance.AbstractMethod()|1|.OtherField >= 5");
        }
    }
}
