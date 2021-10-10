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
    public class ArrayTests : SymbolicExamplesTestBase
    {
        public ArrayTests(Xunit.Abstractions.ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        { }

        [Fact]
        [Trait("Category", "Concolic")]
        public void Branching_BasedOn_ArrayLength_ShouldEnd_Within_2_Iterations()
        {
            Explore("Examples.Concolic.Simple.Arrays.BranchBasedOnArrayLength",
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
                SymbolicArgs.Arg("array", new Int32[] { 1, 5, 10, 15 }));
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void Test_Branching_Based_On_ArrayLength_And_Static_Index()
        {
            Explore("Examples.Concolic.Simple.Arrays.BranchBasedOnArrayElementAtStaticIndex",
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

                    paths.Count().Should().Be(2);
                },
                SymbolicArgs.Arg("array", new Int32[] { 1, 5, 10 }));
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void Test_Branching_Based_On_ArrayLength_And_Dynamic_Index()
        {
            Explore("Examples.Concolic.Simple.Arrays.BranchBasedOnArrayElementAtDynamicIndex",
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

                    paths.Count().Should().Be(2);
                },
                SymbolicArgs.Arg("array", new Int32[] { 1, 5, 10 }),
                SymbolicArgs.Arg("index", 2));
        }
    }
}
