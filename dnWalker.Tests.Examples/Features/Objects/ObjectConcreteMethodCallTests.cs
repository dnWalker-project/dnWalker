using dnWalker.Concolic;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Examples.Features.Objects
{
    public class ObjectConcreteMethodCallTests : ExamplesTestBase
    {
        public ObjectConcreteMethodCallTests(ITestOutputHelper output) : base(output)
        {
        }

        [ExamplesTest]
        public void InvokeMethodWithFieldAccess(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.InvokeMethodWithFieldAccess");

            result.Iterations.Should().HaveCount(3);
            result.Iterations[0].Output.Should().BeNullOrEmpty();
            result.Iterations[1].Output.Trim().Should().Be("instance._myField != 3");
            result.Iterations[2].Output.Trim().Should().Be("instance._myField == 3");
        }
    }
}
