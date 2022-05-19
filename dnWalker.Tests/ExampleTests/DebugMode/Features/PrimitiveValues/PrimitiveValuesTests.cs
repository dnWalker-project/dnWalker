using dnWalker.Concolic;
using dnWalker.Concolic.Traversal;
using dnWalker.Traversal;

using FluentAssertions;

using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.ExampleTests.DebugMode.Features.PrimitiveValues
{
    public class PrimitiveValuesTests : DebugExamplesTestBase
    {
        public PrimitiveValuesTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void Test_NoBranch()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.PrimitiveValues.MethodsWithPrimitiveValueParameter.NoBranch");

            PathStore pathStore = explorer.PathStore;

            pathStore.Paths.Should().HaveCount(1);
            pathStore.Paths[0].Output.Trim().Should().Be("no branching");
        }

        [Fact]
        public void Test_BranchIfPositive()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.PrimitiveValues.MethodsWithPrimitiveValueParameter.BranchIfPositive");

            PathStore pathStore = explorer.PathStore;

            pathStore.Paths.Should().HaveCount(2);
            pathStore.Paths[0].Output.Trim().Should().Be("x <= 0");
            pathStore.Paths[1].Output.Trim().Should().Be("x > 0");
        }

        [Fact]
        public void Test_NestedBranching()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.PrimitiveValues.MethodsWithPrimitiveValueParameter.NestedBranching");

            PathStore pathStore = explorer.PathStore;

            pathStore.Paths.Should().HaveCount(4);
            pathStore.Paths[0].Output.Trim().Should().Be("(x <= 0)\r\n(x >= -3)");
            pathStore.Paths[1].Output.Trim().Should().Be("(x <= 0)\r\n(x < -3)");
            pathStore.Paths[2].Output.Trim().Should().Be("(x > 0)\r\n(x >= 5)");
            pathStore.Paths[3].Output.Trim().Should().Be("(x > 0)\r\n(x < 5)");
        }

        [Fact]
        public void Test_NestedBranchingUnsat()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.PrimitiveValues.MethodsWithPrimitiveValueParameter.NestedBranchingUnsat");

            PathStore pathStore = explorer.PathStore;

            pathStore.Paths.Should().HaveCount(3);
            pathStore.Paths[0].Output.Trim().Should().Be("(x <= 0)\r\n(x >= -3)");
            pathStore.Paths[1].Output.Trim().Should().Be("(x <= 0)\r\n(x < -3)");
            pathStore.Paths[2].Output.Trim().Should().Be("(x > 0)\r\n(x >= -5)");
        }

        [Fact]
        public void Test_MultipleBranchingWithStateChanges()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.PrimitiveValues.MethodsWithPrimitiveValueParameter.MultipleBranchingWithStateChanges");

            PathStore pathStore = explorer.PathStore;

            pathStore.Paths.Should().HaveCount(4);

        }

        [Fact]
        public void Test_MultipleBranchingWithoutStateChanges()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.PrimitiveValues.MethodsWithPrimitiveValueParameter.MultipleBranchingWithoutStateChanges");

            PathStore pathStore = explorer.PathStore;

            pathStore.Paths.Should().HaveCount(3);

        }
    }
}
