using dnWalker.Symbolic;
using dnWalker.Tests.ExampleTests;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace dnWalker.Tests.Concolic.Features.Arrays
{
    public class InputTests : SymbolicExamplesTestBase
    {
        public InputTests(Xunit.Abstractions.ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void M1()
        {
            Explore("Examples.Concolic.Features.Arrays.Input.m1",
                null,
                (explorer) =>
                {
                    var paths = explorer.PathStore.Paths;

                    foreach (var p in paths)
                    {
                        System.Console.Out.WriteLine(p.GetPathInfo());
                    }

                    paths.Count().Should().Be(2);
                    var path = paths.First();
                    path.PathConstraints.Should().HaveCount(1);
                    path.PathConstraintString.Should().Be("(-Convert(Convert(-d)) != 10)");
                },
                SymbolicArgs.Arg("d", new char[] { }), 
                SymbolicArgs.Arg<int>("n")); // (char[] c, int n)
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void Calc()
        {
            Explore("Examples.Concolic.Features.Arrays.Array.comp",
                null,
                (explorer) =>
                {
                    var paths = explorer.PathStore.Paths;

                    foreach (var p in paths)
                    {
                        System.Console.Out.WriteLine(p.GetPathInfo());
                    }

                    paths.Count().Should().Be(2);
                    var path = paths.First();
                    path.PathConstraints.Should().HaveCount(1);
                    path.PathConstraintString.Should().Be("(-Convert(Convert(-d)) != 10)");
                },
                SymbolicArgs.Arg<int>("i"),
                SymbolicArgs.Arg("arr", new int[] { })); // (char[] c, int n)
        }

        /*[Fact]
        [Trait("Category", "Concolic")]
        public void Bar()
        {
            Explore("Examples.Concolic.Features.Math.Input.bar",
                null,
                (explorer) =>
                {
                    var paths = explorer.PathStore.Paths;

                    foreach (var p in paths)
                    {
                        System.Diagnostics.Debug.WriteLine(p.GetPathInfo());
                    }

                    paths.Select(p => p.PathConstraintString)
                        .Should()
                        .BeEquivalentTo(
                            "(d1 <= d2)",
                            "((d1 > d2) And Not(((Sqrt(d1) * 0.0383972435438753) < 0)))",
                            "((d1 > d2) And ((Sqrt(d1) * 0.0383972435438753) >= 0))");
                },
                SymbolicArgs.Arg<double>("d1"),
                SymbolicArgs.Arg<double>("d2"));
        }*/
    }
}
