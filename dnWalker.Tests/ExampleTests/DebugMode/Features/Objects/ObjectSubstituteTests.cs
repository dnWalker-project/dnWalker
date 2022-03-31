using dnWalker.Concolic;
using dnWalker.Parameters;
using dnWalker.Tests.ExampleTests;
using dnWalker.Traversal;

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

            IExecutionParameterSet execSet = explorer.ParameterStore.ExecutionSets[1];

            // there should be exactly 2 parameters
            // 0x00000001 - object, type signature = TestClass
            // 0x00000002 - double, value = unspecified
            // 0x0...01 - field OtherField should have value of 0x0...02

            IObjectParameter objParam = execSet.Parameters[0x00000001] as IObjectParameter;
            objParam.Should().NotBeNull();

            objParam.TryGetField("OtherField", out IParameter fp).Should().BeTrue();

            IDoubleParameter dbl = fp as IDoubleParameter;
            dbl.Should().NotBeNull();
            dbl.Reference.Should().Be((ParameterRef)0x00000002);
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

            IExecutionParameterSet execSet = explorer.ParameterStore.ExecutionSets[1];
            IBaseParameterSet baseSet = execSet.BaseSet;

            // there should be exactly 2 parameters
            // 0x00000001 - object, type signature = TestClass
            // 0x00000002 - double, value = unspecified
            // 0x0...01 - field OtherField should have value of 0x0...02

            IObjectParameter objParam = execSet.Parameters[0x00000001] as IObjectParameter;
            objParam.Should().NotBeNull();

            objParam.TryGetField("OtherField", out IParameter fp).Should().BeTrue();

            IDoubleParameter dbl = fp as IDoubleParameter;
            dbl.Should().NotBeNull();
            dbl.Reference.Should().NotBe((ParameterRef)0x00000002);
            baseSet.Parameters.Should().NotContainKey(dbl.Reference);
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

            IExecutionParameterSet execSet = explorer.ParameterStore.ExecutionSets[1];
            IBaseParameterSet baseSet = execSet.BaseSet;

            IObjectParameter objParam = execSet.Parameters[0x00000001] as IObjectParameter;
            objParam.Should().NotBeNull();

            objParam.TryGetField("TCField", out IObjectParameter fp).Should().BeTrue();

            fp.GetIsNull().Should().BeFalse();
            
            baseSet.Parameters.Should().NotContainKey(fp.Reference);
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

            IExecutionParameterSet execSet = explorer.ParameterStore.ExecutionSets[1];
            IBaseParameterSet baseSet = execSet.BaseSet;


            IObjectParameter objParam = execSet.Parameters[0x00000001] as IObjectParameter;
            objParam.Should().NotBeNull();

            objParam.TryGetField("PrimitiveArray", out IArrayParameter fp).Should().BeTrue();

            fp.GetIsNull().Should().BeFalse();

            baseSet.Parameters.Should().NotContainKey(fp.Reference);

            fp.Type.IsSZArray.Should().BeTrue();
            fp.Type.ElementType.IsInt32.Should().BeTrue();
            fp.GetLength().Should().Be(3);
            fp.TryGetItem(0, out IInt32Parameter i0).Should().BeTrue();
            fp.TryGetItem(1, out IInt32Parameter i1).Should().BeTrue();
            fp.TryGetItem(2, out IInt32Parameter i2).Should().BeTrue();


            baseSet.Parameters.Should().ContainKey(i0.Reference);
            baseSet.Parameters.Should().NotContainKey(i1.Reference);
            baseSet.Parameters.Should().NotContainKey(i2.Reference);

            i0.GetValue().Should().Be(0);
            i1.GetValue().Should().Be(-1);
            i2.GetValue().Should().Be(1);

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

            paths.Count().Should().Be(2);

            IExecutionParameterSet execSet = explorer.ParameterStore.ExecutionSets[1];
            IBaseParameterSet baseSet = execSet.BaseSet;

            IObjectParameter objParam = execSet.Parameters[0x00000001] as IObjectParameter;
            objParam.Should().NotBeNull();

            objParam.TryGetField("RefArray", out IArrayParameter fp).Should().BeTrue();
            fp.GetIsNull().Should().BeFalse();
            fp.GetLength().Should().Be(3);
            fp.TryGetItem(0, out IObjectParameter o0).Should().BeTrue();
            fp.TryGetItem(1, out IObjectParameter o1).Should().BeTrue();
            fp.TryGetItem(2, out IObjectParameter o2).Should().BeTrue();

            o0.Should().BeSameAs(objParam);
            o1.GetIsNull().Should().BeTrue();
            o2.GetIsNull().Should().BeFalse();
            baseSet.Parameters.Should().NotContainKey(o2.Reference);
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

            IExecutionParameterSet execSet = explorer.ParameterStore.ExecutionSets[1];
            IBaseParameterSet baseSet = execSet.BaseSet;

            IObjectParameter objParam = execSet.Parameters[0x00000001] as IObjectParameter;
            objParam.Should().NotBeNull();

            objParam.TryGetField("PrimitiveArray", out IArrayParameter primitiveArray).Should().BeTrue();
            objParam.TryGetField("RefArray", out IArrayParameter refArray).Should().BeTrue();
            objParam.TryGetField("TCField", out IObjectParameter o).Should().BeTrue();

            primitiveArray.GetIsNull().Should().BeTrue();
            refArray.GetIsNull().Should().BeTrue();
            o.GetIsNull().Should().BeTrue();

            baseSet.Parameters.Should().NotContainKey(primitiveArray.Reference);
            baseSet.Parameters.Should().NotContainKey(refArray.Reference);
            baseSet.Parameters.Should().NotContainKey(o.Reference);
        }
    }
}
