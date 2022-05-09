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

namespace dnWalker.Tests.ExampleTests.DebugMode.Features.Arrays
{
    public class InputTests : DebugExamplesTestBase
    {
        public InputTests(Xunit.Abstractions.ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void M1()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(3)
                .Build();

            explorer.Run("Examples.Concolic.Features.Arrays.Input.m1");

            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(2);
            var path = paths.First();
            //path.PathConstraints.Should().HaveCount(1);
            path.PathConstraintString.Should().Be("(-Convert(Convert(-d)) != 10)");
        }

        [Fact]
        public void Calc()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(3)
                .Build();

            explorer.Run("Examples.Concolic.Features.Arrays.Array.comp");

            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(2);
            var path = paths.First();
            //path.PathConstraints.Should().HaveCount(1);
            path.PathConstraintString.Should().Be("(-Convert(Convert(-d)) != 10)");
        }
    }
}
