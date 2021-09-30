using dnWalker.Symbolic;
using dnWalker.Tests.ExampleTests;
using FluentAssertions;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace dnWalker.Tests.Symbolic
{
    public class MethodParametersFactoryTests : SymbolicExamplesTestBase
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
                    cgf.MaxIterations = -1;
                },
                (explorer) =>
                {
                    //explorer.GetUnhandledException().Should().BeNull();
                    var paths = explorer.PathStore.Paths;

                    foreach(var p in paths)
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
                SymbolicArgs.Arg("x", 10),
                SymbolicArgs.Arg("y", 2));
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
                SymbolicArgs.Arg("x", 0),
                SymbolicArgs.Arg("y", 0));
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
                SymbolicArgs.Arg("x", 0),
                SymbolicArgs.Arg("y", 0));
        }
    }
}
