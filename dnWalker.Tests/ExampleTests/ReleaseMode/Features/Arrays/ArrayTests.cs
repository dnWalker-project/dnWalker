﻿using dnWalker.Concolic;
using dnWalker.Symbolic;
using dnWalker.Tests.ExampleTests;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Tests.ExampleTests.ReleaseMode.Features.Arrays
{
    public class ArrayTests : ReleaseExamplesTestBase
    {
        public ArrayTests(Xunit.Abstractions.ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        { }

        [Fact]
        [Trait("Category", "Concolic")]
        public void Branching_BasedOn_ArrayLength_ShouldEnd_Within_2_Iterations()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();


            explorer.Run("Examples.Concolic.Features.Arrays.BranchingBasedOnArrayProperties.BranchBasedOnArrayLength");

            //explorer.GetUnhandledException().Should().BeNull();
            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(3);
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void Test_Branching_Based_On_ArrayLength_And_Static_Index()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Arrays.BranchingBasedOnArrayProperties.BranchBasedOnArrayElementAtStaticIndex");

            //explorer.GetUnhandledException().Should().BeNull();
            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(3);
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void Test_Branching_Based_On_ArrayLength_And_Dynamic_Index()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Arrays.BranchingBasedOnArrayProperties.BranchBasedOnArrayElementAtDynamicIndex");

            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(3);
        }
    }
}
