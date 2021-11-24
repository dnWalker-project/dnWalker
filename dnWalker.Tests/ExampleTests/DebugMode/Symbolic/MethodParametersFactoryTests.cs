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
            Explore("Examples.Concolic.Simple.Branches.Branch",
                (cgf) =>
                {
                    cgf.MaxIterations = 100;
                },
                (explorer) =>
                {
                    //explorer.GetUnhandledException().Should().BeNull();
                    var paths = explorer.PathStore.Paths;

                    foreach (var p in paths)
                    {
                        System.Console.Out.WriteLine(p.GetPathInfo());
                    }

                    paths.Count().Should().Be(5);
                    paths.Select(p => p.PathConstraintString)
                        .Should()
                        .BeEquivalentTo(
                            "((((x >= 0) And (y >= 0)) And (x >= y)) And (x != 0))",
                            "((((x >= 0) And (y >= 0)) And (x >= y)) And (x == 0))",
                            "(((x >= 0) And (y >= 0)) And (x < y))",
                            "(((x >= 0) And (y < 0)) And (x < 1))",
                            "((x < 0) And (y < 0))");
                },
                data: new Dictionary<string, object> 
                {
                    ["x"] = 10, 
                    ["y"] = 2
                });
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void ArgsEdgesCoverageTest()
        {
            Explore("Examples.Concolic.Simple.Branches.Branch",
                null,
                (explorer) =>
                {
                    explorer.PathStore.Coverage.Edges.Should().Be(1d);
                },
                data: new Dictionary<string, object>
                {
                    ["x"] = 0,
                    ["y"] = 0
                });
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void ArgsNodesCoverageTest()
        {
            Explore("Examples.Concolic.Simple.Branches.Branch",
                null,
                (explorer) =>
                {
                    explorer.PathStore.Coverage.Nodes.Should().Be(1d);
                },
                data: new Dictionary<string, object>
                {
                    ["x"] = 0,
                    ["y"] = 0
                });
        }
    }
}
