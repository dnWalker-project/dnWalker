using dnWalker.Concolic;
using dnWalker.Traversal;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.ExampleTests.DebugMode.Features.Strings
{
    public class StringSubstituteTests : DebugExamplesTestBase
    {
        public StringSubstituteTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void Test_BranchOnStringLength()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();


            explorer.Run("Examples.Concolic.Features.Strings.MethodsWithStringParameter.BranchBasedOnLength");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(3);
            paths[0].PathConstraintString.Should().Be("N0x00000001");
            paths[1].PathConstraintString.Should().Be("(Not(N0x00000001) And (V0x00000001.get_Length() <= 5))");
            paths[2].PathConstraintString.Should().Be("(Not(N0x00000001) And (V0x00000001.get_Length() > 5))");
        }
    }
}
