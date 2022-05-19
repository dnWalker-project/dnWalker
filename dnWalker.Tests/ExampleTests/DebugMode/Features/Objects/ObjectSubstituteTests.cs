using dnlib.DotNet;

using dnWalker.Concolic;
using dnWalker.Concolic.Traversal;
using dnWalker.Parameters;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;
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

            foreach (var path in paths)
            {
                Output.WriteLine(path.GetPathInfo());
            }

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

            foreach (var path in paths)
            {
                Output.WriteLine(path.GetPathInfo());
            }

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

            MethodDef method = explorer.EntryPoint.ResolveMethodDef();
            IReadOnlyList<Path> paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(2);

            // tested method
            // public static void SetFieldValue_InputParameter(TestClass obj, double value)
            // {
            //    if (obj == null) return;
            //    obj.OtherField = value;
            // }

            {
                // first iteration
                // there should be only 1 initialized variable - "obj" as the parameter "value" has never been loaded
                IReadOnlyModel inputModel = paths[0].GetSymbolicContext().InputModel;

                inputModel.TryGetValue(Variable.MethodArgument(method.Parameters[0]), out IValue objValue).Should().BeTrue();
                inputModel.TryGetValue(Variable.MethodArgument(method.Parameters[1]), out _).Should().BeFalse();

                objValue.Should().Be(Location.Null);
            }
            {
                // second iteration
                IReadOnlyModel inputModel = paths[1].GetSymbolicContext().InputModel;
                IReadOnlyModel outputModel = paths[1].GetSymbolicContext().OutputModel;

                // there should be 2 initialized variables
                // "obj" - @1
                // "value" - double
                // output model: @1 should point to a node which have defined field "OtherField"

                inputModel.TryGetValue(Variable.MethodArgument(method.Parameters[0]), out IValue objValue).Should().BeTrue();
                inputModel.TryGetValue(Variable.MethodArgument(method.Parameters[1]), out IValue valueValue).Should().BeTrue();

                objValue.Should().NotBe(Location.Null);
                valueValue.Should().Be(PrimitiveValue<double>.Default);

                outputModel.HeapInfo.TryGetNode((Location)objValue, out var node).Should().BeTrue();
                node.Should().BeOfType<IReadOnlyObjectHeapNode>();
                IReadOnlyObjectHeapNode objNode = (IReadOnlyObjectHeapNode)node;

                objNode.Fields.Should().HaveCount(1);

                IField fld = objNode.Fields.First();
                fld.Name.Should().Be("OtherField");

                objNode.GetField(fld).Should().Be(PrimitiveValue<double>.Default);

                // we have lost information - the obj.OtherField := value -> should be provided within post condition or something...
            }
        }

        [Fact]
        public void Test_StoreField_CreatingNewParameter_Primitive()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.SetFieldValue_ConstructedParameter_Primitive");

            MethodDef method = explorer.EntryPoint.ResolveMethodDef();
            IReadOnlyList<Path> paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(2);

            //public static void SetFieldValue_ConstructedParameter_Primitive(TestClass obj, double value)
            //{
            //    if (obj == null) return;

            //    obj.OtherField = value * 2;
            //}

            {
                // first iteration
                // there should be only 1 initialized variable - "obj" as the parameter "value" has never been loaded
                IReadOnlyModel inputModel = paths[0].GetSymbolicContext().InputModel;

                inputModel.TryGetValue(Variable.MethodArgument(method.Parameters[0]), out IValue objValue).Should().BeTrue();
                inputModel.TryGetValue(Variable.MethodArgument(method.Parameters[1]), out _).Should().BeFalse();

                objValue.Should().Be(Location.Null);
            }
            {
                // second iteration
                IReadOnlyModel inputModel = paths[1].GetSymbolicContext().InputModel;
                IReadOnlyModel outputModel = paths[1].GetSymbolicContext().OutputModel;

                // there should be 2 initialized variables
                // "obj" - @1
                // "value" - double
                // output model: @1 should point to a node which have defined field "OtherField"

                inputModel.TryGetValue(Variable.MethodArgument(method.Parameters[0]), out IValue objValue).Should().BeTrue();
                inputModel.TryGetValue(Variable.MethodArgument(method.Parameters[1]), out IValue valueValue).Should().BeTrue();

                objValue.Should().NotBe(Location.Null);
                valueValue.Should().Be(PrimitiveValue<double>.Default);

                outputModel.HeapInfo.TryGetNode((Location)objValue, out var node).Should().BeTrue();
                node.Should().BeOfType<IReadOnlyObjectHeapNode>();
                IReadOnlyObjectHeapNode objNode = (IReadOnlyObjectHeapNode)node;

                objNode.Fields.Should().HaveCount(1);

                IField fld = objNode.Fields.First();
                fld.Name.Should().Be("OtherField");

                objNode.GetField(fld).Should().Be(PrimitiveValue<double>.Default);

                // we have lost information - the obj.OtherField := value -> should be provided within post condition or something...
            }
        }

        [Fact]
        public void Test_StoreField_CreatingNewParameter_Object()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.SetFieldValue_ConstructedParameter_Object");

            MethodDef method = explorer.EntryPoint.ResolveMethodDef();
            IReadOnlyList<Path> paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(2);

            //public static void SetFieldValue_ConstructedParameter_Object(TestClass obj)
            //{
            //    if (obj == null) return;

            //    obj.TCField = new TestClass();
            //}

            {
                // first iteration
                // there should be only 1 initialized variable - "obj" as the parameter "value" has never been loaded
                IReadOnlyModel inputModel = paths[0].GetSymbolicContext().InputModel;

                inputModel.TryGetValue(Variable.MethodArgument(method.Parameters[0]), out IValue objValue).Should().BeTrue();
                inputModel.TryGetValue(Variable.MethodArgument(method.Parameters[1]), out _).Should().BeFalse();

                objValue.Should().Be(Location.Null);
            }
            {
                // second iteration
                IReadOnlyModel inputModel = paths[1].GetSymbolicContext().InputModel;
                IReadOnlyModel outputModel = paths[1].GetSymbolicContext().OutputModel;

                // there should be 2 initialized variables
                // "obj" - @1
                // output model: @1 should point to a node which have defined field "TCField" with value @2

                inputModel.TryGetValue(Variable.MethodArgument(method.Parameters[0]), out IValue objValue).Should().BeTrue();

                objValue.Should().NotBe(Location.Null);

                outputModel.HeapInfo.TryGetNode((Location)objValue, out IReadOnlyHeapNode objNode).Should().BeTrue();

                TypeDef objType = objNode.Type.ToTypeDefOrRef().ResolveTypeDefThrow();
                IField tcField = objType.Fields.First(f => f.Name == "TCField");

                IValue tcFieldValue = ((IReadOnlyObjectHeapNode)objNode).GetField(tcField);
                tcFieldValue.Should().BeOfType<Location>().And.NotBe(Location.Null);

                outputModel.HeapInfo.TryGetNode((Location)tcFieldValue, out IReadOnlyHeapNode newNode).Should().BeTrue();
                newNode.Should().BeOfType<IReadOnlyObjectHeapNode>();
            }
        }

        [Fact]
        public void Test_StoreField_CreatingNewParameter_PrimitiveArray()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.SetFieldValue_ConstructedParameter_PrimitiveArray");

            MethodDef method = explorer.EntryPoint.ResolveMethodDef();
            IReadOnlyList<Path> paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(2);

            //public static void SetFieldValue_ConstructedParameter_PrimitiveArray(TestClass obj, int i)
            //{
            //    if (obj == null) return;

            //    obj.PrimitiveArray = new int[] { i, i - 1, i + 1 };
            //}

            {
                // first iteration
                // there should be only 1 initialized variable - "obj" as the parameter "value" has never been loaded
                IReadOnlyModel inputModel = paths[0].GetSymbolicContext().InputModel;

                inputModel.TryGetValue(Variable.MethodArgument(method.Parameters[0]), out IValue objValue).Should().BeTrue();
                inputModel.TryGetValue(Variable.MethodArgument(method.Parameters[1]), out _).Should().BeFalse();

                objValue.Should().Be(Location.Null);
            }
            {
                // second iteration
                IReadOnlyModel inputModel = paths[1].GetSymbolicContext().InputModel;
                IReadOnlyModel outputModel = paths[1].GetSymbolicContext().OutputModel;

                // there should be 2 initialized variables
                // "obj" - @1
                // output model: @1 should point to a node which have defined field "PrimitiveArray" with value @2
                // obj.PrimitiveArray[0] :=  0
                // obj.PrimitiveArray[1] := -1
                // obj.PrimitiveArray[2] :=  1

                inputModel.TryGetValue(Variable.MethodArgument(method.Parameters[0]), out IValue objValue).Should().BeTrue();

                objValue.Should().NotBe(Location.Null);

                outputModel.HeapInfo.TryGetNode((Location)objValue, out IReadOnlyHeapNode objNode).Should().BeTrue();

                Location newArrLoc = (Location)((IReadOnlyObjectHeapNode)objNode).GetField(((IReadOnlyObjectHeapNode)objNode).Fields.First());

                IReadOnlyArrayHeapNode newArrNode = (IReadOnlyArrayHeapNode)outputModel.HeapInfo.GetNode(newArrLoc);

                newArrNode.Length.Should().Be(3);
                newArrNode.GetElement(0).Should().Be(new PrimitiveValue<int>(0));
                newArrNode.GetElement(1).Should().Be(new PrimitiveValue<int>(-1));
                newArrNode.GetElement(2).Should().Be(new PrimitiveValue<int>(1));
            }

        }

        [Fact]
        public void Test_StoreField_CreatingNewParameter_RefArray()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.SetFieldValue_ConstructedParameter_RefArray");

            MethodDef method = explorer.EntryPoint.ResolveMethodDef();
            IReadOnlyList<Path> paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(2);

            //public static void SetFieldValue_ConstructedParameter_RefArray(TestClass obj)
            //{
            //    if (obj == null) return;

            //    obj.RefArray = new TestClass[] { obj, null, new TestClass() };
            //}

            {
                // first iteration
                // there should be only 1 initialized variable - "obj" as the parameter "value" has never been loaded
                IReadOnlyModel inputModel = paths[0].GetSymbolicContext().InputModel;

                inputModel.TryGetValue(Variable.MethodArgument(method.Parameters[0]), out IValue objValue).Should().BeTrue();
                inputModel.TryGetValue(Variable.MethodArgument(method.Parameters[1]), out _).Should().BeFalse();

                objValue.Should().Be(Location.Null);
            }
            {
                // second iteration
                IReadOnlyModel inputModel = paths[1].GetSymbolicContext().InputModel;
                IReadOnlyModel outputModel = paths[1].GetSymbolicContext().OutputModel;

                // there should be 2 initialized variables
                // "obj" - @1
                // output model: @1 should point to a node which have defined field "RefArray" with value @2
                //               @3 should point to a fresh node
                // obj.PrimitiveArray[0] := @1
                // obj.PrimitiveArray[1] := null
                // obj.PrimitiveArray[2] := @3

                inputModel.TryGetValue(Variable.MethodArgument(method.Parameters[0]), out IValue objValue).Should().BeTrue();

                objValue.Should().NotBe(Location.Null);

                outputModel.HeapInfo.TryGetNode((Location)objValue, out IReadOnlyHeapNode objNode).Should().BeTrue();

                Location newArrLoc = (Location)((IReadOnlyObjectHeapNode)objNode).GetField(((IReadOnlyObjectHeapNode)objNode).Fields.First());

                IReadOnlyArrayHeapNode newArrNode = (IReadOnlyArrayHeapNode)outputModel.HeapInfo.GetNode(newArrLoc);

                newArrNode.Length.Should().Be(3);
                newArrNode.GetElement(0).Should().Be(objValue);
                newArrNode.GetElement(1).Should().Be(Location.Null);
                newArrNode.GetElement(2).Should().Be(outputModel.HeapInfo.Locations.First(l => l != (Location)objValue && l != newArrLoc));
            }
        }

        [Fact]
        public void Test_StoreField_CreatingNewParameter_Null()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.SetFieldValue_ConstructedParameter_Null");

            MethodDef method = explorer.EntryPoint.ResolveMethodDef();
            IReadOnlyList<Path> paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(2);

            //public static void SetFieldValue_ConstructedParameter_Null(TestClass obj)
            //{
            //    if (obj == null) return;

            //    obj.PrimitiveArray = null;
            //    obj.RefArray = null;
            //    obj.TCField = null;
            //}

            {
                // first iteration
                // there should be only 1 initialized variable - "obj" as the parameter "value" has never been loaded
                IReadOnlyModel inputModel = paths[0].GetSymbolicContext().InputModel;

                inputModel.TryGetValue(Variable.MethodArgument(method.Parameters[0]), out IValue objValue).Should().BeTrue();
                inputModel.TryGetValue(Variable.MethodArgument(method.Parameters[1]), out _).Should().BeFalse();

                objValue.Should().Be(Location.Null);
            }
            {
                // second iteration
                IReadOnlyModel inputModel = paths[1].GetSymbolicContext().InputModel;
                IReadOnlyModel outputModel = paths[1].GetSymbolicContext().OutputModel;

                // there should be 2 initialized variables
                // "obj" - @1
                // output model: @1 should point to a node which have defined field "RefArray" with value @2
                //               @3 should point to a fresh node
                // obj.PrimitiveArray[0] := @1
                // obj.PrimitiveArray[1] := null
                // obj.PrimitiveArray[2] := @3

                inputModel.TryGetValue(Variable.MethodArgument(method.Parameters[0]), out IValue objValue).Should().BeTrue();

                objValue.Should().NotBe(Location.Null);

                outputModel.HeapInfo.TryGetNode((Location)objValue, out IReadOnlyHeapNode objNode).Should().BeTrue();

                IReadOnlyObjectHeapNode node = (IReadOnlyObjectHeapNode)objNode;
                node.Fields.Should().HaveCount(3);
                foreach (IField fld in node.Fields)
                {
                    node.GetField(fld).Should().Be(Location.Null);
                }

            }
        }
    }
}
