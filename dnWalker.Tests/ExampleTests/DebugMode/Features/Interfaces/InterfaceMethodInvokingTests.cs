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

namespace dnWalker.Tests.ExampleTests.DebugMode.Features.Interfaces
{
    public class InterfaceMethodInvokingTests : DebugExamplesTestBase
    {
        public InterfaceMethodInvokingTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void Test_UsingIntefaceProxy_For_PureMethods()
        {
            Explore("Examples.Concolic.Features.Interfaces.InterfaceMethodInvoking.BranchingBasedOnPureValueProvider",
                initializeConfig: cfg =>
                {
                    cfg.MaxIterations = 10;
                },
                finished: explorer =>
                {
                    //explorer.GetUnhandledException().Should().BeNull();
                    var paths = explorer.PathStore.Paths;

                    foreach (var p in paths)
                    {
                        System.Console.Out.WriteLine(p.GetPathInfo());
                    }

                    paths.Count().Should().Be(3);
                });
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void Test_MethodIvokedMultipleTimes()
        {
            Explore("Examples.Concolic.Features.Interfaces.InterfaceMethodInvoking.MethodInvokedMultipleTimes",
                initializeConfig: cfg =>
                {
                    cfg.MaxIterations = 10;
                },
                finished: explorer =>
                {
                    //explorer.GetUnhandledException().Should().BeNull();
                    var paths = explorer.PathStore.Paths;

                    foreach (var p in paths)
                    {
                        System.Console.Out.WriteLine(p.GetPathInfo());
                    }

                    paths.Count().Should().Be(3);
                });
        }
    }
}