using dnWalker.Concolic;
using dnWalker.Symbolic;
using dnWalker.Tests.ExampleTests;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Tests.Symbolic
{
    public class BranchingTests : SymbolicExamplesTestBase
    {
        public BranchingTests(Xunit.Abstractions.ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void NoBranching_ShouldEnd_Within_1_Iteration()
        {
            Explore("Examples.Concolic.Simple.Branches.NoBranching",
                (cgf) =>
                {
                    cgf.MaxIterations = 1;
                },
                (explorer) =>
                {
                    //explorer.GetUnhandledException().Should().BeNull();
                    var paths = explorer.PathStore.Paths;

                    foreach (var p in paths)
                    {
                        System.Console.Out.WriteLine(p.GetPathInfo());
                    }

                    paths.Count().Should().Be(1);
                },
                SymbolicArgs.Arg("x", 10));
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void SingleBranching_ShouldEnd_Within_2_Iteration()
        {
            Explore("Examples.Concolic.Simple.Branches.SingleBranching",
                (cgf) =>
                {
                    cgf.MaxIterations = 2;
                },
                (explorer) =>
                {
                    //explorer.GetUnhandledException().Should().BeNull();
                    var paths = explorer.PathStore.Paths;

                    foreach (var p in paths)
                    {
                        System.Console.Out.WriteLine(p.GetPathInfo());
                    }

                    paths.Count().Should().Be(2);
                },
                SymbolicArgs.Arg("x", 10));
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void InvertingVariableValue()
        {
            Explore("Examples.Concolic.Simple.Branches.SingleBranchingWithModification",
                (cgf) =>
                {
                    cgf.MaxIterations = 2;
                },
                (explorer) =>
                {
                    //explorer.GetUnhandledException().Should().BeNull();
                    var paths = explorer.PathStore.Paths;

                    foreach (var p in paths)
                    {
                        System.Console.Out.WriteLine(p.GetPathInfo());
                    }

                    paths.Count().Should().Be(2);
                },
                SymbolicArgs.Arg("x", 10));
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void ExceedingMaxNumberOfIterations_Should_Throw()
        {
            Assert.Throws<MaxIterationsExceededException>(() =>
            {
                Explore("Examples.Concolic.Simple.Branches.MultipleBranchingWithMultipleParameters",
                    (cgf) =>
                    {
                        cgf.MaxIterations = 10;
                    },
                    (explorer) =>
                    {
                        //explorer.GetUnhandledException().Should().BeNull();
                        var paths = explorer.PathStore.Paths;

                        foreach (var p in paths)
                        {
                            System.Console.Out.WriteLine(p.GetPathInfo());
                        }

                        paths.Count().Should().Be(4);
                    },
                    SymbolicArgs.Arg("x", 10),
                    SymbolicArgs.Arg("y", -5),
                    SymbolicArgs.Arg("z", 6));
            });
        }
    }
}
