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
using Xunit.Abstractions;

namespace dnWalker.Tests.ExampleTests.ReleaseMode.Features.Interfaces
{
    public class InterfaceMethodInvokingTests : ReleaseExamplesTestBase
    {
        public InterfaceMethodInvokingTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void Test_UsingIntefaceProxy_For_PureMethods()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();
            explorer.Run("Examples.Concolic.Features.Interfaces.InterfaceMethodInvoking.BranchingBasedOnPureValueProvider");

            //explorer.GetUnhandledException().Should().BeNull();
            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(3);
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void Test_MethodIvokedMultipleTimes()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();
            explorer.Run("Examples.Concolic.Features.Interfaces.InterfaceMethodInvoking.MethodInvokedMultipleTimes");

            //explorer.GetUnhandledException().Should().BeNull();
            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(4);
        }
    }
}