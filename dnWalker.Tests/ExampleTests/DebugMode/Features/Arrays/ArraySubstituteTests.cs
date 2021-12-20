using dnWalker.Concolic;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.ExampleTests.DebugMode.Features.Arrays
{
    public class ArraySubstituteTests : DebugExamplesTestBase
    {
        public ArraySubstituteTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }


        [Fact]
        public void Test_ArraySubstitute_NullEquality()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.BranchIfNull");

            var paths = explorer.PathStore.Paths;

            paths.Count().Should().Be(2);
        }

        [Fact]
        public void Test_ArraySubstitute_NotNullEquality()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.BranchIfNotNull");

            var paths = explorer.PathStore.Paths;

            paths.Count().Should().Be(2);
        }

        [Fact]
        public void Test_ArraySubstitute_LengthLowerThan5()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.BranchIfLengthLowerThan5");

            var paths = explorer.PathStore.Paths;

            paths.Count().Should().Be(3);
        }

        [Fact]
        public void Test_ArraySubstitute_LengthGreaterThan5()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.BranchIfLengthGreaterThan5");

            var paths = explorer.PathStore.Paths;

            paths.Count().Should().Be(3);
        }
    }
}
