using dnWalker.Concolic;
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
    public class InterfaceSubstituteTests : DebugExamplesTestBase
    {
        public InterfaceSubstituteTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void Test_InterfaceSubstitute_AbstractMethodSubstitution()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Interfaces.MethodsWithInterfaceParameter.Interface_AbstractMethod");

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
