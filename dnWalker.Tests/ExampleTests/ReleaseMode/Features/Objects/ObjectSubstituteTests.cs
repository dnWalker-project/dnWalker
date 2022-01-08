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

namespace dnWalker.Tests.ExampleTests.ReleaseMode.Features.Objects
{
    public class ObjectSubstituteTests : ReleaseExamplesTestBase
    {
        public ObjectSubstituteTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void Test_UsingObjectParamterSubstitute_For_FieldAccess()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.InvokeMethodWithFieldAccess");

            //explorer.GetUnhandledException().Should().BeNull();
            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(3);
        }


        [Fact]
        public void Test_ObjectSubstitute_NullEquality()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.BranchIfNull");

            var paths = explorer.PathStore.Paths;

            paths.Count().Should().Be(2);
        }

        [Fact]
        public void Test_ObjectSubstitute_NotNullEquality()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.BranchIfNotNull");

            var paths = explorer.PathStore.Paths;

            paths.Count().Should().Be(2);
        }
    }
}
