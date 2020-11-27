using dnWalker.Symbolic;
using dnWalker.Tests.ExampleTests;
using FluentAssertions;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace dnWalker.Tests.Symbolic
{
    public class MethodParametersFactoryTests : SymbolicExamplesTestBase
    {
        public MethodParametersFactoryTests()
        {
        }

        [Fact]
        public void ArgsTest()
        {
            Explore("Examples.Simple.Branches.Branch",
                null,
                (explorer) =>
                {
                    //explorer.GetUnhandledException().Should().BeNull();
                    var paths = explorer.PathStore.Paths;

                    foreach(var p in paths)
                    {
                        System.Diagnostics.Debug.WriteLine(p.GetPathInfo());
                    }

                    paths.Count().Should().Be(5);
                    var path = paths.First();
                    path.PathConstraints.Should().HaveCount(4);
                    path.PathConstraintString.Should().Be("(((Not((x < 0)) And Not((y < 0))) And Not((x < y))) And Not((x == 0)))");
                },
                SymbolicArgs.Arg<int>("x", 10),
                SymbolicArgs.Arg("y", 2));
        }
    }
}
