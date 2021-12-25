using dnWalker.Concolic;
using dnWalker.Parameters;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Tests.ExampleTests.DebugMode.Symbolic
{

    public class MethodParametersFactoryTests : DebugExamplesTestBase
    {
        public MethodParametersFactoryTests(Xunit.Abstractions.ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void ArgsTest()
        {
            IExplorer explorer = GetConcolicExplorerBuilder().SetMaxIterations(10).Build();
            explorer.Run("Examples.Concolic.Simple.Branches.Branch", Args().Set("x", 10).Set("y", 2));
            
            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(5);
            //paths.Select(p => p.PathConstraintString)
            paths.Select(p => p.GetConstraintStringWithAccesses(explorer.ParameterStore.BaseContext))
                .Should()
                .BeEquivalentTo(
                    "((((x >= 0) And (y >= 0)) And (x >= y)) And (x != 0))",
                    "((((x >= 0) And (y >= 0)) And (x >= y)) And (x == 0))",
                    "(((x >= 0) And (y >= 0)) And (x < y))",
                    "(((x >= 0) And (y < 0)) And (x < -y))",
                    "((((x < 0) And (y < 0)) And (-x >= -y)) And (-x != 0))");
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void ArgsEdgesCoverageTest()
        {
            IExplorer explorer = GetConcolicExplorerBuilder().SetMaxIterations(10).Build();
            explorer.Run("Examples.Concolic.Simple.Branches.Branch", Args().Set("x", 10).Set("y", 2));

            explorer.PathStore.Coverage.Edges.Should().Be(1d);
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void ArgsNodesCoverageTest()
        {
            IExplorer explorer = GetConcolicExplorerBuilder().SetMaxIterations(10).Build();
            explorer.Run("Examples.Concolic.Simple.Branches.Branch", Args().Set("x", 10).Set("y", 2));

            explorer.PathStore.Coverage.Nodes.Should().Be(1d);
        }
    }
}
