using dnWalker.Concolic;
using dnWalker.Traversal;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.ExampleTests.DebugMode.Features.Strings
{
    public class StringSubstituteTests : DebugExamplesTestBase
    {
        public StringSubstituteTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void Test_BranchOnStringLength()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();


            explorer.Run("Examples.Concolic.Features.Strings.MethodsWithStringParameter.BranchOnLength");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(3);
            paths[0].PathConstraintString.Should().Be("N0x00000001");
            paths[1].PathConstraintString.Should().Be("(Not(N0x00000001) And (V0x00000001.get_Length() <= 5))");
            paths[2].PathConstraintString.Should().Be("(Not(N0x00000001) And (V0x00000001.get_Length() > 5))");
        }

        [Fact]
        public void Test_BranchOnEquality()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();


            explorer.Run("Examples.Concolic.Features.Strings.MethodsWithStringParameter.BranchOnEquality");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(2);
            paths[0].PathConstraintString.Should().Be("(V0x00000001 != \"hello world\")");
            paths[1].PathConstraintString.Should().Be("(V0x00000001 == \"hello world\")");
        }

        [Fact]
        public void Test_BranchOnPrefix()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();


            explorer.Run("Examples.Concolic.Features.Strings.MethodsWithStringParameter.BranchOnPrefix");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(3);
            paths[0].PathConstraintString.Should().Be("N0x00000001");
            paths[1].PathConstraintString.Should().Be("(Not(N0x00000001) And Not(V0x00000001.StartsWith(\"some prefix\")))");
            paths[2].PathConstraintString.Should().Be("(Not(N0x00000001) And V0x00000001.StartsWith(\"some prefix\"))");
        }

        [Fact]
        public void Test_BranchOnSuffix()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();


            explorer.Run("Examples.Concolic.Features.Strings.MethodsWithStringParameter.BranchOnSuffix");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(3);
            paths[0].PathConstraintString.Should().Be("N0x00000001");
            paths[1].PathConstraintString.Should().Be("(Not(N0x00000001) And Not(V0x00000001.EndsWith(\"some suffix\")))");
            paths[2].PathConstraintString.Should().Be("(Not(N0x00000001) And V0x00000001.EndsWith(\"some suffix\"))");
        }

        [Fact]
        public void Test_BranchOnContains()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();


            explorer.Run("Examples.Concolic.Features.Strings.MethodsWithStringParameter.BranchOnContains");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(3);
            paths[0].PathConstraintString.Should().Be("N0x00000001");
            paths[1].PathConstraintString.Should().Be("(Not(N0x00000001) And Not(V0x00000001.Contains(\"hello world\")))");
            paths[2].PathConstraintString.Should().Be("(Not(N0x00000001) And V0x00000001.Contains(\"hello world\"))");
        }

        [Fact]
        public void Test_BranchOnSubstringEquality()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();


            explorer.Run("Examples.Concolic.Features.Strings.MethodsWithStringParameter.BranchOnSubstringEquality");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(4);
            paths[0].PathConstraintString.Should().Be("N0x00000001");
            paths[1].PathConstraintString.Should().Be("(Not(N0x00000001) And (V0x00000001.get_Length() < 20))");
            paths[2].PathConstraintString.Should().Be("((Not(N0x00000001) And (V0x00000001.get_Length() >= 20)) And (V0x00000001.Substring(5, 10) != \"AAAAAAAAAA\"))");
            paths[3].PathConstraintString.Should().Be("((Not(N0x00000001) And (V0x00000001.get_Length() >= 20)) And (V0x00000001.Substring(5, 10) == \"AAAAAAAAAA\"))");
        }
    }
}
