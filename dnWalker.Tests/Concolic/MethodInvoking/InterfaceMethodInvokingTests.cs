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

namespace dnWalker.Tests.Concolic.MethodInvoking
{
    public class InterfaceMethodInvokingTests : SymbolicExamplesTestBase
    {
        public InterfaceMethodInvokingTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void TestSupplyDummyObject_InsteadOfInterfaceType()
        {
            Explore("Examples.Concolic.MethodInvoking.InterfaceMethodInvoking.BranchingBasedOnPureValueProvider",
                (cgf) =>
                {
                },
                (explorer) =>
                {
                    //explorer.GetUnhandledException().Should().BeNull();
                    var paths = explorer.PathStore.Paths;

                    foreach (var p in paths)
                    {
                        System.Console.Out.WriteLine(p.GetPathInfo());
                    }

                    paths.Count().Should().Be(2);
                },
                SymbolicArgs.Arg("x", new Object()));
        }
    }
}
