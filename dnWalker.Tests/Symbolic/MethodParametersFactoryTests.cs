using dnWalker.Symbolic;
using dnWalker.Tests.ExampleTests;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace dnWalker.Tests.Symbolic
{
    public class MethodParametersFactoryTests : ExamplesTestBase
    {
        public MethodParametersFactoryTests() : base(Lazy.Value)
        {
        }

        [Fact]
        public void ArgsTest()
        {
            Explore("Examples.Simple.Branches.Branch",
                null,
                (explorer) =>
                {
                    explorer.GetUnhandledException().Should().BeNull();
                    explorer.GetExploredPaths().Count().Should().Be(1);
                    var output = explorer.GetExploredPaths().First().Output;
                    output.Should().Be($"X=1{Environment.NewLine}Y=1{Environment.NewLine}Z=1{Environment.NewLine}");
                },
                SymbolicArgs.Arg<int>("x"), 
                Args.Arg(2));
        }
    }
}
