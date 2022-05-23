using dnWalker.Concolic;
using dnWalker.Concolic.Traversal;
using dnWalker.Traversal;

using FluentAssertions;

using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.Tests.Examples.Features.PrimitiveValues
{
    public class PrimitiveValuesTests : ExamplesTestBase
    {
        public PrimitiveValuesTests(ITestOutputHelper output) : base(output)
        {
        }

        [ExamplesTest]
        public void Test_NoBranch(BuildInfo buildInfo)
        {
            IExplorer explorer = CreateExplorer(buildInfo);

            ExplorationResult result = explorer.Run("Examples.Concolic.Features.PrimitiveValues.MethodsWithPrimitiveValueParameter.NoBranch");

            result.Iterations.Should().HaveCount(1);
        }
    }
}
