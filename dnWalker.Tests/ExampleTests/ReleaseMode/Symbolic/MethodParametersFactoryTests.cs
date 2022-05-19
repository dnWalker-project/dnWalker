﻿using dnWalker.Concolic;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Tests.ExampleTests.ReleaseMode.Symbolic
{

    public class MethodParametersFactoryTests : ReleaseExamplesTestBase
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
            paths.Select(p => p.PathConstraintString)
                .Should()
                .BeEquivalentTo(
                    "((((x >= 0) And (y >= 0)) And (x >= y)) And (x != 0))",
                    "((((x >= 0) And (y >= 0)) And (x >= y)) And (x == 0))",
                    "(((x >= 0) And (y >= 0)) And (x < y))",
                    "(((x >= 0) And (y < 0)) And (x < 1))",
                    "((x < 0) And (y < 0))");
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void ArgsEdgesCoverageTest()
        {
            IExplorer explorer = GetConcolicExplorerBuilder().SetMaxIterations(10).Build();
            explorer.Run("Examples.Concolic.Simple.Branches.Branch", Args().Set("x", 10).Set("y", 2));

            explorer.PathStore.GetCoverage().Edges.Should().Be(1d);
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void ArgsNodesCoverageTest()
        {
            IExplorer explorer = GetConcolicExplorerBuilder().SetMaxIterations(10).Build();
            explorer.Run("Examples.Concolic.Simple.Branches.Branch", Args().Set("x", 10).Set("y", 2));

            explorer.PathStore.GetCoverage().Nodes.Should().Be(1d);
        }
    }
}
