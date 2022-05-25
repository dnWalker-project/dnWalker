using dnWalker.Concolic;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Examples.Features.Objects
{
    public class ObjectNullabilityTests : ExamplesTestBase
    {
        public ObjectNullabilityTests(ITestOutputHelper output) : base(output)
        {
        }

        [ExamplesTest]
        public void BranchIfNull(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.BranchIfNull");

            result.Iterations.Should().HaveCount(2);
            result.Iterations[0].Output.Trim().Should().Be("instance is null");
            result.Iterations[1].Output.Trim().Should().Be("instance is not null");
        }

        [ExamplesTest]
        public void BranchIfNotNull(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.BranchIfNotNull");

            result.Iterations.Should().HaveCount(2);
            result.Iterations[0].Output.Trim().Should().Be("instance is null");
            result.Iterations[1].Output.Trim().Should().Be("instance is not null");
        }

        [ExamplesTest]
        public void InvokeMethodWithFieldAccessWithoutNullCheck(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.BranchIfNotNull");

            result.Iterations.Should().HaveCount(3);
            result.Iterations[0].Exception.Type.FullName.Should().Be("System.NullReferenceException");
            result.Iterations[1].Output.Trim().Should().Be("instance:_myField != 3");
            result.Iterations[2].Output.Trim().Should().Be("instance:_myField == 3");
        }
    }
}
