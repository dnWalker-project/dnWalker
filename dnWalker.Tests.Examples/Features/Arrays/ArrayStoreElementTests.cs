using dnWalker.Concolic;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.Tests.Examples.Features.Arrays
{
    public class ArrayStoreElementTests : ExamplesTestBase
    {
        public ArrayStoreElementTests(ITestOutputHelper output) : base(output)
        {
        }

        [ExamplesTest]
        public void SetElementToInput(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.SetElementToInput");

            Debug.Fail("Add output model checks");
        }

        [ExamplesTest]
        public void SetElementToFreshObject(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.SetElementToInput");

            Debug.Fail("Add output model checks");
        }

        [ExamplesTest]
        public void SetElementToFreshPrimitive(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.SetElementToFreshPrimitive");

            Debug.Fail("Add output model checks");
        }

        [ExamplesTest]
        public void SetElementToFreshArray(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.SetElementToFreshArray");

            Debug.Fail("Add output model checks");
        }

        [ExamplesTest]
        public void SetElementToNull(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.SetElementToNull");

            Debug.Fail("Add output model checks");
        }
    }
}
