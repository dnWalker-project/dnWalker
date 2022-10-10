using dnWalker.Concolic;

using FluentAssertions;

using Microsoft.VisualStudio.TestPlatform.Utilities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Examples
{
    public class CaptureOutputTests : ExamplesTestBase
    {
        public CaptureOutputTests(ITestOutputHelper output) : base(output)
        {
        }

        [ExamplesTest]
        public void CaptureOutputTest1(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.CaptureOutput.Capture1");
            result.Iterations.Should().HaveCount(1);

            result.Iterations[0].Output.Should().Be($"X=1{Environment.NewLine}Y=1{Environment.NewLine}Z=1{Environment.NewLine}");
        }

        [ExamplesTest]
        public void CaptureOutputTest2(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.CaptureOutput.Capture2");
            result.Iterations.Should().HaveCount(1);

            result.Iterations[0].Output.Should().Be($"X=2{Environment.NewLine}X=2{Environment.NewLine}");
        }
    }
}
