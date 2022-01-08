using dnWalker.Concolic;
using dnWalker.Concolic.Traversal;
using dnWalker.Parameters;
using dnWalker.Symbolic;
using dnWalker.Tests.ExampleTests;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.ExampleTests.ReleaseMode
{
    public class BranchingTests : ReleaseExamplesTestBase
    {
        public BranchingTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }


        [Fact]
        public void NoBranching_ShouldEnd_Within_1_Iteration()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(1)
                .Build();

            explorer.Run("Examples.Concolic.Simple.Branches.NoBranching", Args().Set("x", 10));

            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(1);
        }

        [Fact]
        public void SingleBranching_ShouldEnd_Within_2_Iteration()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(2)
                .Build();

            explorer.Run("Examples.Concolic.Simple.Branches.SingleBranching", Args().Set("x", 10));

            //explorer.GetUnhandledException().Should().BeNull();
            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(2);
        }

        [Fact]
        public void InvertingVariableValue()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(2)
                .Build();

            explorer.Run("Examples.Concolic.Simple.Branches.SingleBranchingWithModification", Args().Set("x", 10));

            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count.Should().Be(2);
        }

        [Fact]
        public void ExceedingMaxNumberOfIterations_Should_Throw()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(2)
                .Build();


            Assert.Throws<MaxIterationsExceededException>(() => explorer.Run("Examples.Concolic.Simple.Branches.MultipleBranchingWithMultipleParameters", Args().Set("x", 10).Set("y", -5).Set("z", 6)));
        }

        // comparing variable "x" against 5

        // comparing variable "x" against 5

        [Fact]
        public void Test_Branch_Equals_TruePathFirst()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(5)
                .Build();

            explorer.Run("Examples.Concolic.Simple.Branches.Branch_Equals", Args().Set("x", 5));

            PathStore pathStore = explorer.PathStore;
            pathStore.Paths.Count().Should().Be(2);

            ParameterStore store = explorer.ParameterStore;

            String[] pathConstraints = pathStore.Paths.Select(p => p.GetConstraintStringWithAccesses(store.BaseContext)).ToArray();

            pathConstraints[0].Should().Be(@"(x == 5)");
            pathConstraints[1].Should().Be(@"(x != 5)");
        }

        [Fact]
        public void Test_Branch_Equals_FalsePathFirst()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(5)
                .Build();

            explorer.Run("Examples.Concolic.Simple.Branches.Branch_Equals", Args().Set("x", 4));

            PathStore pathStore = explorer.PathStore;
            pathStore.Paths.Count().Should().Be(2);

            ParameterStore store = explorer.ParameterStore;

            String[] pathConstraints = pathStore.Paths.Select(p => p.GetConstraintStringWithAccesses(store.BaseContext)).ToArray();

            pathConstraints[0].Should().Be(@"(x != 5)");
            pathConstraints[1].Should().Be(@"(x == 5)");
        }

        [Fact]
        public void Test_Branch_NotEquals_TruePathFirst()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(5)
                .Build();

            explorer.Run("Examples.Concolic.Simple.Branches.Branch_NotEquals", Args().Set("x", 4));

            PathStore pathStore = explorer.PathStore;
            pathStore.Paths.Count().Should().Be(2);

            ParameterStore store = explorer.ParameterStore;

            String[] pathConstraints = pathStore.Paths.Select(p => p.GetConstraintStringWithAccesses(store.BaseContext)).ToArray();

            pathConstraints[0].Should().Be(@"(x != 5)");
            pathConstraints[1].Should().Be(@"(x == 5)");
        }

        [Fact]
        public void Test_Branch_NotEquals_FalsePathFirst()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(5)
                .Build();

            explorer.Run("Examples.Concolic.Simple.Branches.Branch_NotEquals", Args().Set("x", 5));

            PathStore pathStore = explorer.PathStore;
            pathStore.Paths.Count().Should().Be(2);

            ParameterStore store = explorer.ParameterStore;

            String[] pathConstraints = pathStore.Paths.Select(p => p.GetConstraintStringWithAccesses(store.BaseContext)).ToArray();

            pathConstraints[0].Should().Be(@"(x == 5)");
            pathConstraints[1].Should().Be(@"(x != 5)");
        }

        [Fact]
        public void Test_Branch_GreaterThan_TruePathFirst()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(5)
                .Build();

            explorer.Run("Examples.Concolic.Simple.Branches.Branch_GreaterThan", Args().Set("x", 7));

            PathStore pathStore = explorer.PathStore;
            pathStore.Paths.Count().Should().Be(2);

            ParameterStore store = explorer.ParameterStore;

            String[] pathConstraints = pathStore.Paths.Select(p => p.GetConstraintStringWithAccesses(store.BaseContext)).ToArray();

            pathConstraints[0].Should().Be(@"(x > 5)");
            pathConstraints[1].Should().Be(@"(x <= 5)");

        }

        [Fact]
        public void Test_Branch_GreaterThan_FalsePathFirst()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(5)
                .Build();

            explorer.Run("Examples.Concolic.Simple.Branches.Branch_GreaterThan", Args().Set("x", 4));

            PathStore pathStore = explorer.PathStore;
            pathStore.Paths.Count().Should().Be(2);

            ParameterStore store = explorer.ParameterStore;

            String[] pathConstraints = pathStore.Paths.Select(p => p.GetConstraintStringWithAccesses(store.BaseContext)).ToArray();

            pathConstraints[0].Should().Be(@"(x <= 5)");
            pathConstraints[1].Should().Be(@"(x > 5)");
        }

        [Fact]
        public void Test_Branch_GreaterThanOrEquals_TruePathFirst() // this throws MaxIterationExceeded
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(5)
                .Build();

            explorer.Run("Examples.Concolic.Simple.Branches.Branch_GreaterThanOrEquals", Args().Set("x", 7));

            PathStore pathStore = explorer.PathStore;
            pathStore.Paths.Count().Should().Be(2);

            ParameterStore store = explorer.ParameterStore;

            String[] pathConstraints = pathStore.Paths.Select(p => p.GetConstraintStringWithAccesses(store.BaseContext)).ToArray();

            pathConstraints[0].Should().Be(@"(x >= 5)");
            pathConstraints[1].Should().Be(@"(x < 5)");
        }

        [Fact]
        public void Test_Branch_GreaterThanOrEquals_FalsePathFirst() // this throws MaxIterationExceeded
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(5)
                .Build();

            explorer.Run("Examples.Concolic.Simple.Branches.Branch_GreaterThanOrEquals", Args().Set("x", 4));

            PathStore pathStore = explorer.PathStore;
            pathStore.Paths.Count().Should().Be(2);

            ParameterStore store = explorer.ParameterStore;

            String[] pathConstraints = pathStore.Paths.Select(p => p.GetConstraintStringWithAccesses(store.BaseContext)).ToArray();

            pathConstraints[0].Should().Be(@"(x < 5)");
            pathConstraints[1].Should().Be(@"(x >= 5)");
        }

        [Fact]
        public void Test_Branch_LowerThan_TruePathFirst()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(5)
                .Build();

            explorer.Run("Examples.Concolic.Simple.Branches.Branch_LowerThan", Args().Set("x", 4));

            PathStore pathStore = explorer.PathStore;
            pathStore.Paths.Count().Should().Be(2);

            ParameterStore store = explorer.ParameterStore;

            String[] pathConstraints = pathStore.Paths.Select(p => p.GetConstraintStringWithAccesses(store.BaseContext)).ToArray();

            pathConstraints[0].Should().Be(@"(x < 5)");
            pathConstraints[1].Should().Be(@"(x >= 5)");
        }

        [Fact]
        public void Test_Branch_LowerThan_FalsePathFirst()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(5)
                .Build();

            explorer.Run("Examples.Concolic.Simple.Branches.Branch_LowerThan", Args().Set("x", 7));

            PathStore pathStore = explorer.PathStore;
            pathStore.Paths.Count().Should().Be(2);

            ParameterStore store = explorer.ParameterStore;

            String[] pathConstraints = pathStore.Paths.Select(p => p.GetConstraintStringWithAccesses(store.BaseContext)).ToArray();

            pathConstraints[0].Should().Be(@"(x >= 5)");
            pathConstraints[1].Should().Be(@"(x < 5)");
        }

        [Fact]
        public void Test_Branch_LowerThanOrEquals_TruePathFirst() // this throws MaxIterationExceeded
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(5)
                .Build();

            explorer.Run("Examples.Concolic.Simple.Branches.Branch_LowerThanOrEquals", Args().Set("x", 4));

            PathStore pathStore = explorer.PathStore;
            pathStore.Paths.Count().Should().Be(2);

            ParameterStore store = explorer.ParameterStore;

            String[] pathConstraints = pathStore.Paths.Select(p => p.GetConstraintStringWithAccesses(store.BaseContext)).ToArray();

            pathConstraints[0].Should().Be(@"(x <= 5)");
            pathConstraints[1].Should().Be(@"(x > 5)");
        }

        [Fact]
        public void Test_Branch_LowerThanOrEquals_FalsePathFirst() // this throws MaxIterationExceeded
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(5)
                .Build();

            explorer.Run("Examples.Concolic.Simple.Branches.Branch_LowerThanOrEquals", Args().Set("x", 7));

            PathStore pathStore = explorer.PathStore;
            pathStore.Paths.Count().Should().Be(2);

            ParameterStore store = explorer.ParameterStore;

            String[] pathConstraints = pathStore.Paths.Select(p => p.GetConstraintStringWithAccesses(store.BaseContext)).ToArray();

            pathConstraints[0].Should().Be(@"(x > 5)");
            pathConstraints[1].Should().Be(@"(x <= 5)");
        }
    }
}
