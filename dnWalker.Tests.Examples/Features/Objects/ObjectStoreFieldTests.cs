using dnlib.DotNet;

using dnWalker.Concolic;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IVariable = dnWalker.Symbolic.IVariable;

namespace dnWalker.Tests.Examples.Features.Objects
{
    public class ObjectStoreFieldTests : ExamplesTestBase
    {
        public ObjectStoreFieldTests(ITestOutputHelper output) : base(output)
        {
        }

        [ExamplesTest]
        public void SetFieldToInputPrimitive(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.SetFieldToInputPrimitive");

            result.Iterations.Should().HaveCount(3);

            result.Iterations[0].Output.Trim().Should().Be("Value too low...");
            result.Iterations[1].Exception.Type.FullName.Should().Be("System.NullReferenceException");

            IReadOnlyModel model = result.Iterations[2].SymbolicContext.OutputModel;

            // there should be only 1 heap node
            model.HeapInfo.Nodes.Should().HaveCount(1);
            IReadOnlyObjectHeapNode node = (IReadOnlyObjectHeapNode)model.HeapInfo.Nodes.First();

            node.Fields.Should().HaveCount(1);
            ((PrimitiveValue<double>)node.GetFieldOrDefault(node.Fields.First())).Value.Should().BeGreaterThanOrEqualTo(5.5);
        }

        [ExamplesTest]
        public void SetFieldToInputObject(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.SetFieldToInputObject");

            result.Iterations.Should().HaveCount(3);

            result.Iterations[0].Output.Trim().Should().Be("Value is null");
            result.Iterations[1].Exception.Type.FullName.Should().Be("System.NullReferenceException");

            IReadOnlyModel model = result.Iterations[2].SymbolicContext.OutputModel;

            // there should be 2 heap nodes
            model.HeapInfo.Nodes.Should().HaveCount(2);
            IVariable objVar = Variable.MethodArgument(result.EntryPoint.Parameters[0]);
            IVariable valueVar = Variable.MethodArgument(result.EntryPoint.Parameters[1]);

            IReadOnlyObjectHeapNode objNode = (IReadOnlyObjectHeapNode)model.HeapInfo.GetNode((Location)model.GetValueOrDefault(objVar));
            IValue value = model.GetValueOrDefault(valueVar);

            objNode.Fields.Should().HaveCount(1);
            objNode.GetFieldOrDefault(objNode.Fields.First()).Should().Be(value);
        }

        [ExamplesTest]
        public void SetFieldToFreshPrimitive(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.SetFieldToFreshPrimitive");

            result.Iterations.Should().HaveCount(2);

            result.Iterations[0].Exception.Type.FullName.Should().Be("System.NullReferenceException");
            IReadOnlyModel model = result.Iterations[1].SymbolicContext.OutputModel;

            // there should be only 1 heap node
            model.HeapInfo.Nodes.Should().HaveCount(1);
            IReadOnlyObjectHeapNode node = (IReadOnlyObjectHeapNode)model.HeapInfo.Nodes.First();

            node.Fields.Should().HaveCount(1);
            ((PrimitiveValue<double>)node.GetFieldOrDefault(node.Fields.First())).Value.Should().Be(3.14);
        }

        [ExamplesTest]
        public void SetFieldToFreshObject(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.SetFieldToFreshObject");

            result.Iterations.Should().HaveCount(2);

            result.Iterations[0].Exception.Type.FullName.Should().Be("System.NullReferenceException");

            IReadOnlyModel outModel = result.Iterations[1].SymbolicContext.OutputModel;
            IReadOnlyModel inModel = result.Iterations[1].SymbolicContext.InputModel;

            IVariable objVar = Variable.MethodArgument(result.EntryPoint.Parameters[0]);
            Location objLoc = (Location)outModel.GetValueOrDefault(objVar);
            IReadOnlyObjectHeapNode objNode = (IReadOnlyObjectHeapNode)outModel.HeapInfo.GetNode(objLoc);

            Location newObjLoc = (Location)objNode.GetField("TCField");

            inModel.HeapInfo.Locations.Should().NotContain(newObjLoc);
        }

        [ExamplesTest]
        public void SetFieldToFreshPrimitiveArray(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.SetFieldToFreshPrimitiveArray");

            result.Iterations.Should().HaveCount(3);

            result.Iterations[0].Output.Trim().Should().Be("Value too low...");
            result.Iterations[1].Exception.Type.FullName.Should().Be("System.NullReferenceException");

            IReadOnlyModel outModel = result.Iterations[2].SymbolicContext.OutputModel;
            IReadOnlyModel inModel = result.Iterations[2].SymbolicContext.InputModel;

            // there should be 2 heap nodes
            outModel.HeapInfo.Nodes.Should().HaveCount(2);
            IVariable objVar = Variable.MethodArgument(result.EntryPoint.Parameters[0]);
            IReadOnlyObjectHeapNode objNode = (IReadOnlyObjectHeapNode)outModel.HeapInfo.GetNode((Location)outModel.GetValueOrDefault(objVar));

            IReadOnlyArrayHeapNode arrNode = (IReadOnlyArrayHeapNode)outModel.HeapInfo.GetNode((Location)objNode.GetFieldOrDefault(objNode.Fields.First()));

            IVariable iVar = Variable.MethodArgument(result.EntryPoint.Parameters[1]);
            int iValue = ((PrimitiveValue<int>)outModel.GetValueOrDefault(iVar)).Value;

            arrNode.Indeces.Should().HaveCount(3);

            arrNode.GetElementOrDefault(0).Should().Be(ValueFactory.GetValue(iValue));
            arrNode.GetElementOrDefault(1).Should().Be(ValueFactory.GetValue(iValue - 1));
            arrNode.GetElementOrDefault(2).Should().Be(ValueFactory.GetValue(iValue + 1));

            inModel.HeapInfo.Locations.Should().NotContain(arrNode.Location);
        }

        [ExamplesTest]
        public void SetFieldToFreshRefArray(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.SetFieldToFreshRefArray");

            result.Iterations.Should().HaveCount(2);

            result.Iterations[0].Exception.Type.FullName.Should().Be("System.NullReferenceException");

            IReadOnlyModel outModel = result.Iterations[1].SymbolicContext.OutputModel;
            IReadOnlyModel inModel = result.Iterations[1].SymbolicContext.InputModel;

            outModel.HeapInfo.Nodes.Should().HaveCount(3);

            IReadOnlyHeapNode[] nodes = outModel.HeapInfo.Nodes.ToArray();
            Array.Sort(nodes, static (a, b) => a.Location.Value.CompareTo(b.Location.Value));

            nodes[0].Should().BeAssignableTo<IReadOnlyObjectHeapNode>();
            nodes[1].Should().BeAssignableTo<IReadOnlyArrayHeapNode>();
            nodes[2].Should().BeAssignableTo<IReadOnlyObjectHeapNode>();

            IVariable objVar = Variable.MethodArgument(result.EntryPoint.Parameters[0]);
            Location objLoc = (Location)outModel.GetValueOrDefault(objVar);
            objLoc.Should().Be(nodes[0].Location);
            IReadOnlyObjectHeapNode objNode = (IReadOnlyObjectHeapNode)nodes[0];

            Location arrLoc = (Location)objNode.GetField("RefArray");
            arrLoc.Should().Be(nodes[1].Location);
            IReadOnlyArrayHeapNode arrNode = (IReadOnlyArrayHeapNode)nodes[1];

            arrNode.GetElementOrDefault(0).Should().Be(objLoc);
            arrNode.GetElementOrDefault(1).Should().Be(Location.Null);
            arrNode.GetElementOrDefault(2).Should().Be(nodes[2].Location);

            inModel.HeapInfo.Locations.Should().NotContain(arrLoc);
            inModel.HeapInfo.Locations.Should().NotContain(nodes[2].Location);
        }

        [ExamplesTest]
        public void SetFieldToNull(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Objects.MethodsWithObjectParameter.SetFieldToNull");

            result.Iterations.Should().HaveCount(2);

            result.Iterations[0].Exception.Type.FullName.Should().Be("System.NullReferenceException");

            IReadOnlyModel outModel = result.Iterations[1].SymbolicContext.OutputModel;
            IReadOnlyObjectHeapNode objNode = (IReadOnlyObjectHeapNode)outModel.HeapInfo.Nodes.First();

            objNode.Fields.Should().HaveCount(3);
            foreach (IField fld in objNode.Fields)
            {
                objNode.GetFieldOrDefault(fld).Should().Be(Location.Null);
            }
        }
    }
}
