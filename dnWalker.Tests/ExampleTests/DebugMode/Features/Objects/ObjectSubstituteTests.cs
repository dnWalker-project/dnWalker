using dnWalker.Tests.ExampleTests;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.ExampleTests.DebugMode.Features.Objects
{
    public class ObjectSubstituteTests : DebugExamplesTestBase
    {
        public ObjectSubstituteTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void Test_UsingObjectParamterSubstitute_For_FieldAccess()
        {
            Explore("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.InvokeMethodWithFieldAccess",
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
