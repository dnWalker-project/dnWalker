using dnWalker.Concolic;
using dnWalker.Traversal;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.ExampleTests.DebugMode.Features.ReturnValue
{
    public class MethodsWithReturnValue : DebugExamplesTestBase
    {
        public MethodsWithReturnValue(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }


        [Fact]
        public void Test_GetSumIfTrueDiffIfFalse()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();


            explorer.Run("Examples.Concolic.Features.ReturnValue.MethodsWithReturnValue.GetSumIfTrueDiffIfFalse");

            var paths = explorer.PathStore.Paths;

            foreach (Path p in paths)
            {
                Output.WriteLine(p.GetPathInfo());

            }
        }

        [Fact]
        public void Test_ReturnNewIfNull_ItselfOtherwise()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();


            explorer.Run("Examples.Concolic.Features.ReturnValue.MethodsWithReturnValue.ReturnNewIfNull_ItselfOtherwise");

            var paths = explorer.PathStore.Paths;

            foreach (Path p in paths)
            {
                Output.WriteLine(p.GetPathInfo());

            }
        }
    }
}
