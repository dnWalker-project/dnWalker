using dnWalker.Concolic;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Examples.Features.Asserts
{
    public class AssertTests : ExamplesTestBase
    {
        public AssertTests(ITestOutputHelper output) : base(output)
        {
        }

        [ExamplesTest]
        public void MethodWithStaticAssertViolation(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Asserts.MethodsWithAssert.MethodWithStaticAssertViolation");

            // TODO: change ExamplesTestAttibute to enable opt-in/out configurations and target platforms - better than if(buildInfo.IsRelease())
            if (buildInfo.IsRelease())
            {
                // should pass as the Debug.Assert call is not optimized out
                result.Iterations.Should().HaveCount(1);
                // passed assertions are not logged right now, TODO...
                // result.Iterations[0].Statistics.AssertionPasses.Should().Be(0);
                result.Iterations[0].Statistics.AssertionViolations.Should().Be(0);
            }
            else
            {
                result.Iterations.Should().HaveCount(1);
                result.Iterations[0].Statistics.AssertionViolations.Should().Be(1);
            }
        }

        [ExamplesTest]
        public void MethodWithStaticAssertPass(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Asserts.MethodsWithAssert.MethodWithStaticAssertPass");

            // TODO: change ExamplesTestAttibute to enable opt-in/out configurations and target platforms - better than if(buildInfo.IsRelease())
            if (buildInfo.IsRelease())
            {
                // should pass as the Debug.Assert call is not optimized out
                result.Iterations.Should().HaveCount(1);
                // passed assertions are not logged right now, TODO...
                // result.Iterations[0].Statistics.AssertionPasses.Should().Be(0);
                result.Iterations[0].Statistics.AssertionViolations.Should().Be(0);
            }
            else
            {
                result.Iterations.Should().HaveCount(1);
                result.Iterations[0].Statistics.AssertionViolations.Should().Be(0);
            }
        }

        [ExamplesTest]
        public void MethodWithDynamicAssert(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Asserts.MethodsWithAssert.MethodWithDynamicAssert");

            // TODO: change ExamplesTestAttibute to enable opt-in/out configurations and target platforms - better than if(buildInfo.IsRelease())
            if (buildInfo.IsRelease())
            {
                // should pass as the Debug.Assert call is not optimized out
                result.Iterations.Should().HaveCount(1);
                result.Iterations[0].Statistics.AssertionViolations.Should().Be(0);
                // passed assertions are not logged right now, TODO...
                // result.Iterations[0].Statistics.AssertionPasses.Should().Be(0);
            }
            else
            {
                result.Iterations.Should().HaveCount(2);
                result.Iterations[0].Statistics.AssertionViolations.Should().Be(0);
                // result.Iterations[0].Statistics.AssertionPasses.Should().Be(0);
                result.Iterations[1].Statistics.AssertionViolations.Should().Be(1);
                // result.Iterations[1].Statistics.AssertionPasses.Should().Be(1);
            }
        }
    }
}
