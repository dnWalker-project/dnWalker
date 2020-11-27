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

                    paths.Count().Should().Be(5);
                    var path = paths.First();
                    path.PathConstraints.Should().HaveCount(4);
                    path.PathConstraintString.Should().Be("(((Not((x < 0)) And Not((y < 0))) And Not((x < y))) And Not((x == 0)))");
                },
                SymbolicArgs.Arg<double>("d"));
        }
    }
}
