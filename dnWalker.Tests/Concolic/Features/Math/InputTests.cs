using dnWalker.Symbolic;
using dnWalker.Tests.ExampleTests;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace dnWalker.Tests.Concolic.Features.Math
{
    public class InputTests : SymbolicExamplesTestBase
    {
        [Fact]
        public void ArgsTest()
        {
            Explore("Examples.Concolic.Features.Math.Input.foo",
                null,
                (explorer) =>
                {
                    var paths = explorer.PathStore.Paths;

                    foreach (var p in paths)
                    {
                        System.Diagnostics.Debug.WriteLine(p.GetPathInfo());
                    }

                    paths.Count().Should().Be(2);
                    var path = paths.First();
                    path.PathConstraints.Should().HaveCount(1);
                    path.PathConstraintString.Should().Be("Not((-Convert(Convert(-d)) == 10))");
                },
                SymbolicArgs.Arg<double>("d"));
        }
    }
}
