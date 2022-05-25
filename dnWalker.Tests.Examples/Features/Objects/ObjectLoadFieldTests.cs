using dnWalker.Concolic;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Examples.Features.Objects
{
    public class ObjectLoadFieldTests : ExamplesTestBase
    {
        public ObjectLoadFieldTests(ITestOutputHelper output) : base(output)
        {
        }

        [ExamplesTest]
        public void DirectFieldAccess(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.DirectFieldAccess");

            result.Iterations.Should().HaveCount(3);
            result.Iterations[0].Output.Trim().Should().Be("instance is null");
            result.Iterations[1].Output.Trim().Should().Be("instance.OtherField != 10");
            result.Iterations[2].Output.Trim().Should().Be("instance.OtherField == 10");
        }
    }
}
