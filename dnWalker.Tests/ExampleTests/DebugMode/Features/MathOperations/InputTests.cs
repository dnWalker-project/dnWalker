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

namespace dnWalker.Tests.ExampleTests.DebugMode.Features.MathOperations
{
    public class InputTests : DebugExamplesTestBase
    {
        public InputTests(Xunit.Abstractions.ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void Foo()
        {
            IExplorer explorer = GetConcolicExplorerBuilder().Build();
            explorer.Run("Examples.Concolic.Features.MathOperations.Input.foo");

            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(2);
            var path = paths.First();
            path.PathConstraints.Should().HaveCount(1);
            path.PathConstraintString.Should().Be("(-Convert(Convert(-d)) != 10)");
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void Bar()
        {
            IExplorer explorer = GetConcolicExplorerBuilder().Build();
            explorer.Run("Examples.Concolic.Features.MathOperations.Input.bar");

            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Select(p => p.PathConstraintString)
                .Should()
                .BeEquivalentTo(
                    "(d1 <= d2)",
                    "((d1 > d2) And Not(((Sqrt(d1) * 0.0383972435438753) < 0)))",
                    "((d1 > d2) And ((Sqrt(d1) * 0.0383972435438753) >= 0))");
        }
    }
}
