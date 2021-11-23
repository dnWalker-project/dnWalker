using dnWalker.Tests.ExampleTests;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.Concolic.Features.Objects
{
    public class ObjectSubstituteTests : SymbolicExamplesTestBase2
    {
        public ObjectSubstituteTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void Test_UsingObjectParamterSubstitute_For_FieldAccess()
        {
            Explore("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.InvokeMethodWithFieldAccess",
                (cfg) =>
                {
                    cfg.MaxIterations = 10;
                },
                (explorer) =>
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
