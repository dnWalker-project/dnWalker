using dnWalker.Concolic;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Examples.Features.MathOperations
{
    public class MathOperationsTests : ExamplesTestBase
    {
        public MathOperationsTests(ITestOutputHelper output) : base(output)
        {
        }


        [ExamplesTest]
        public void WithMin(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.MathOperations.MethodsWithMath.WithMin");


        }


        [ExamplesTest]
        public void WithMax(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.MathOperations.MethodsWithMath.WithMax");


        }


        [ExamplesTest]
        public void WithAbs(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.MathOperations.MethodsWithMath.WithAbs");


        }
    }
}
