using dnWalker.Symbolic;
using dnWalker.Tests.ExampleTests;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Tests.ExampleTests.DebugMode.Features.Arrays
{
    public class ArrayTests : DebugExamplesTestBase
    {
        public ArrayTests(Xunit.Abstractions.ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        { }

        [Fact]
        [Trait("Category", "Concolic")]
        public void Branching_BasedOn_ArrayLength_ShouldEnd_Within_2_Iterations()
        {
            Explore("Examples.Concolic.Features.Arrays.BranchingBasedOnArrayProperties.BranchBasedOnArrayLength",
                (cgf) =>
                {
                    cgf.MaxIterations = 10;
                },
                finished: explorer =>
                {
                    //explorer.GetUnhandledException().Should().BeNull();
                    var paths = explorer.PathStore.Paths;

                    foreach (var p in paths)
                    {
                        System.Console.Out.WriteLine(p.GetPathInfo());
                    }

                    paths.Count().Should().Be(3);
                });
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void Test_Branching_Based_On_ArrayLength_And_Static_Index()
        {
            Explore("Examples.Concolic.Features.Arrays.BranchingBasedOnArrayProperties.BranchBasedOnArrayElementAtStaticIndex",
                (cgf) =>
                {
                    cgf.MaxIterations = 10;
                },
                finished: explorer =>
                {
                    //explorer.GetUnhandledException().Should().BeNull();
                    var paths = explorer.PathStore.Paths;

                    foreach (var p in paths)
                    {
                        System.Console.Out.WriteLine(p.GetPathInfo());
                    }

                    paths.Count().Should().Be(3);
                });
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void Test_Branching_Based_On_ArrayLength_And_Dynamic_Index()
        {
            Explore("Examples.Concolic.Features.Arrays.BranchingBasedOnArrayProperties.BranchBasedOnArrayElementAtDynamicIndex",
                (cgf) =>
                {
                    cgf.MaxIterations = 10;
                },
                finished: explorer =>
                {
                    //explorer.GetUnhandledException().Should().BeNull();
                    var paths = explorer.PathStore.Paths;

                    foreach (var p in paths)
                    {
                        System.Console.Out.WriteLine(p.GetPathInfo());
                    }

                    paths.Count().Should().Be(3);
                });
        }
    }
}
