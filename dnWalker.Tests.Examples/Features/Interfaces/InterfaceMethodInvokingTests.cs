using dnWalker.Concolic;

using FluentAssertions;


namespace dnWalker.Tests.Examples.Features.Interfaces
{
    public class InterfaceMethodInvokingTests : ExamplesTestBase
    {
        public InterfaceMethodInvokingTests(ITestOutputHelper output) : base(output)
        {
        }

        [ExamplesTest]
        public void InvokeAbstractMethodOnAbstractClass(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Interfaces.MethodsWithInterfaceParameter.InvokeInterfaceMethod");

            result.Iterations.Should().HaveCount(5);
            result.Iterations[0].Output.Trim().Should().Be("instance is null");

            result.Iterations[1].Output.Trim().Should().Be("instance.AbstractMethod()|0| != 3" + Environment.NewLine + "instance.AbstractMethod()|1| <= 134");
            result.Iterations[2].Output.Trim().Should().Be("instance.AbstractMethod()|0| != 3" + Environment.NewLine + "instance.AbstractMethod()|1| > 134");


            result.Iterations[3].Output.Trim().Should().Be("instance.AbstractMethod()|0| == 3" + Environment.NewLine + "instance.AbstractMethod()|1| <= 134");
            result.Iterations[4].Output.Trim().Should().Be("instance.AbstractMethod()|0| == 3" + Environment.NewLine + "instance.AbstractMethod()|1| > 134");
        }
    }
}
