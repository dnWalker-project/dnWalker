using dnWalker.Concolic;
using dnWalker.Symbolic;

using FluentAssertions;

using Microsoft.VisualStudio.TestPlatform.Utilities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Examples.Features.ReturnValue
{
    public class ReturnValueTests : ExamplesTestBase
    {
        public ReturnValueTests(ITestOutputHelper output) : base(output)
        {
        }

        [ExamplesTest]
        public void GetSumIfTrueDiffIfFalse(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.ReturnValue.MethodsWithReturnValue.GetSumIfTrueDiffIfFalse");

            result.Iterations.Should().HaveCount(4);
            // TODO: assert return values, check the return value expressions
        }

        [ExamplesTest]
        public void ReturnTheArgument(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.ReturnValue.MethodsWithReturnValue.ReturnTheArgument");

            result.Iterations.Should().HaveCount(1);
            // TODO: assert return value is same as the argument, check the return value expression
        }

        [ExamplesTest]
        public void ReturnNewIfNull_ItselfOtherwise(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.ReturnValue.MethodsWithReturnValue.ReturnNewIfNull_ItselfOtherwise");

            result.Iterations.Should().HaveCount(2);
            // TODO: assert return value is null vs new instance
        }
    }
}
