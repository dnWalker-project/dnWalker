using dnWalker.Concolic;
using dnWalker.Parameters;
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

            Fail("Check the input/output models.");
        }

        [Fact]
        public void Test_DirectFieldAccess()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.DirectFieldAccess");

            //explorer.GetUnhandledException().Should().BeNull();
            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(3);

            Fail("Check the input/output models.");
        }

        [Fact]
        public void Test_ObjectSubstitute_NullEquality()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();


            explorer.Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.BranchIfNull");

            var paths = explorer.PathStore.Paths;

            foreach (var path in paths)
            {
                Output.WriteLine(path.GetPathInfo());
            }

            paths.Count().Should().Be(2);

            Fail("Check the input/output models.");
        }

        [Fact]
        public void Test_ObjectSubstitute_NotNullEquality()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();


            explorer.Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.BranchIfNotNull");

            var paths = explorer.PathStore.Paths;

            foreach (var path in paths)
            {
                Output.WriteLine(path.GetPathInfo());
            }

            paths.Count().Should().Be(2);

            Fail("Check the input/output models.");
        }

        [Fact]
        public void Test_ObjectSubstitute_LoadFieldAccess()
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

            Fail("Check the input/output models.");
        }

        [Fact]
        public void Test_ObjectSubstitute_ConcreteMethodInvocation()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();


            explorer.Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.AbstractClass_ConcreteMethod");

            //explorer.GetUnhandledException().Should().BeNull();
            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(3);

            Fail("Check the input/output models.");
        }

        [Fact]
        public void Test_ObjectSubstitute_AbstractMethodSubstitution()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();


            explorer.Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.AbstractClass_AbstractMethod");

            //explorer.GetUnhandledException().Should().BeNull();
            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(6);

            Fail("Check the input/output models.");
        }

        [Fact]
        public void Test_ObjectSubstitute_ArgumentIdentityComparer()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();


            explorer.Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.ArgumentIdentityComparer");

            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(4);

            Fail("Check the input/output models.");
        }

        [Fact]
        public void Test_ObjectSubstitute_EditFieldsBasedOnArgumentIdenity()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();


            explorer.Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.EditFieldsBasedOnArgumentIdenity");

            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(5);

            Fail("Check the input/output models.");
        }

        [Fact]
        public void Test_StoreField_NotCreatingNewParameter()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.SetFieldValue_InputParameter");

            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(2);

            Fail("Check the input/output models.");
        }

        [Fact]
        public void Test_StoreField_CreatingNewParameter_Primitive()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.SetFieldValue_ConstructedParameter_Primitive");

            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(2);

            Fail("Check the input/output models.");
        }

        [Fact]
        public void Test_StoreField_CreatingNewParameter_Object()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.SetFieldValue_ConstructedParameter_Object");

            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(2);

            Fail("Check the input/output models.");
        }

        [Fact]
        public void Test_StoreField_CreatingNewParameter_PrimitiveArray()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.SetFieldValue_ConstructedParameter_PrimitiveArray");

            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(2);

            Fail("Check the input/output models.");

        }

        [Fact]
        public void Test_StoreField_CreatingNewParameter_RefArray()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.SetFieldValue_ConstructedParameter_RefArray");

            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count.Should().Be(2);

            Fail("Check the input/output models.");
        }

        [Fact]
        public void Test_StoreField_CreatingNewParameter_Null()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.SetFieldValue_ConstructedParameter_Null");

            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(2);

            Fail("Check the input/output models.");
        }
    }
}
