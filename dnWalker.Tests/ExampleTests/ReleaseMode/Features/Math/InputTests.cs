using dnWalker.Symbolic;
using dnWalker.Tests.ExampleTests;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace dnWalker.Tests.ExampleTests.ReleaseMode.Features.Math
{
    public class InputTests : ReleaseExamplesTestBase
    {
        public InputTests(Xunit.Abstractions.ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void Foo()
        {
            Explore("Examples.Concolic.Features.Math.Input.foo",
                null,
                finished: explorer =>
                {
                    var paths = explorer.PathStore.Paths;

                    foreach (var p in paths)
                    {
                        System.Diagnostics.Debug.WriteLine(p.GetPathInfo());
                    }

                    paths.Count().Should().Be(2);
                    var path = paths.First();
                    path.PathConstraints.Should().HaveCount(1);
                    path.PathConstraintString.Should().Be("(-Convert(Convert(-d)) != 10)");
                });
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void Bar()
        {
            Explore("Examples.Concolic.Features.Math.Input.bar",
                null,
                finished: explorer =>
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
                });
        }
    }
}
