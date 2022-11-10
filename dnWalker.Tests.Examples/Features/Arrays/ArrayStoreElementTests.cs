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

using Xunit.Abstractions;

namespace dnWalker.Tests.Examples.Features.Arrays
{
    public class ArrayStoreElementTests : ExamplesTestBase
    {
        public ArrayStoreElementTests(ITestOutputHelper output) : base(output)
        {
        }

        [ExamplesTest]
        public void SetElementToRefInput(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.SetElementToRefInput");

            result.Iterations.Should().HaveCount(4);

            result.Iterations[0].Output.Trim().Should().Be("Value is null");
            result.Iterations[1].Exception.Type.FullName.Should().Be("System.NullReferenceException");
            result.Iterations[2].Exception.Type.FullName.Should().Be("System.IndexOutOfRangeException");
            result.Iterations[3].Exception.Should().BeNull();

            IReadOnlyModel inModel = result.Iterations[3].SymbolicContext.InputModel;
            IReadOnlyModel outModel = result.Iterations[3].SymbolicContext.OutputModel;

            inModel.HeapInfo.Locations.Should().HaveCount(2);
            outModel.HeapInfo.Locations.Should().HaveCount(2);

            IVariable arrVar = Variable.MethodArgument(result.EntryPoint.Parameters[0]);
            IVariable valueVar = Variable.MethodArgument(result.EntryPoint.Parameters[1]);

            Location arrLoc = (Location)outModel.GetValueOrDefault(arrVar);
            Location valueLoc = (Location)outModel.GetValueOrDefault(valueVar);

            inModel.HeapInfo.Locations.Should().Contain(arrLoc).And.Contain(valueLoc);
            outModel.HeapInfo.Locations.Should().Contain(arrLoc).And.Contain(valueLoc);

            IReadOnlyArrayHeapNode arrNode = (IReadOnlyArrayHeapNode)outModel.HeapInfo.GetNode(arrLoc);
            arrNode.GetElementOrDefault(2).Should().Be(valueLoc);

            arrNode = (IReadOnlyArrayHeapNode)inModel.HeapInfo.GetNode(arrLoc);
            arrNode.GetElementOrDefault(2).Should().Be(Location.Null);
        }

        [ExamplesTest]
        public void SetElementToFreshObject(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.SetElementToFreshObject");

            result.Iterations.Should().HaveCount(3);

            result.Iterations[0].Exception.Type.FullName.Should().Be("System.NullReferenceException");
            result.Iterations[1].Exception.Type.FullName.Should().Be("System.IndexOutOfRangeException");
            result.Iterations[2].Exception.Should().BeNull();

            IReadOnlyModel inModel = result.Iterations[2].SymbolicContext.InputModel;
            IReadOnlyModel outModel = result.Iterations[2].SymbolicContext.OutputModel;

            inModel.HeapInfo.Locations.Should().HaveCount(1);
            outModel.HeapInfo.Locations.Should().HaveCount(2);

            IVariable arrVar = Variable.MethodArgument(result.EntryPoint.Parameters[0]);
            Location arrLoc = (Location)outModel.GetValueOrDefault(arrVar);

            IReadOnlyArrayHeapNode arrNode = (IReadOnlyArrayHeapNode)outModel.HeapInfo.GetNode(arrLoc);
            Location newLoc = (Location)arrNode.GetElementOrDefault(2);

            outModel.HeapInfo.Locations.Should().Contain(newLoc);
        }

        [ExamplesTest]
        public void SetElementToFreshPrimitive(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.SetElementToFreshPrimitive");

            result.Iterations.Should().HaveCount(3);

            IReadOnlyModel model = result.Iterations[2].SymbolicContext.OutputModel;

            model.HeapInfo.Nodes.Should().HaveCount(1);
            IReadOnlyArrayHeapNode arrNode = (IReadOnlyArrayHeapNode)model.HeapInfo.Nodes.First();

            arrNode.Length.Should().BeGreaterThanOrEqualTo(3);
            arrNode.GetElementOrDefault(0).Should().Be(ValueFactory.GetValue(2));
            arrNode.GetElementOrDefault(1).Should().Be(ValueFactory.GetValue(4));
            arrNode.GetElementOrDefault(2).Should().Be(ValueFactory.GetValue(5));
        }

        [ExamplesTest]
        public void SetElementToFreshArray(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.SetElementToFreshArray");

            result.Iterations.Should().HaveCount(3);

            result.Iterations[0].Exception.Type.FullName.Should().Be("System.NullReferenceException");
            result.Iterations[1].Exception.Type.FullName.Should().Be("System.IndexOutOfRangeException");
            result.Iterations[2].Exception.Should().BeNull();

            IReadOnlyModel inModel = result.Iterations[2].SymbolicContext.InputModel;
            IReadOnlyModel outModel = result.Iterations[2].SymbolicContext.OutputModel;

            inModel.HeapInfo.Locations.Should().HaveCount(1);
            outModel.HeapInfo.Locations.Should().HaveCount(2);

            IVariable arrVar = Variable.MethodArgument(result.EntryPoint.Parameters[0]);
            Location arrLoc = (Location)outModel.GetValueOrDefault(arrVar);

            IReadOnlyArrayHeapNode arrNode = (IReadOnlyArrayHeapNode)outModel.HeapInfo.GetNode(arrLoc);
            Location newLoc = (Location)arrNode.GetElementOrDefault(2);

            outModel.HeapInfo.Locations.Should().Contain(newLoc);
        }

        [ExamplesTest]
        public void SetElementToNull(BuildInfo buildInfo)
        {
            ExplorationResult result = CreateExplorer(buildInfo).Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.SetElementToNull");

            result.Iterations.Should().HaveCount(4);

            result.Iterations[0].Exception.Type.FullName.Should().Be("System.NullReferenceException");
            result.Iterations[0].SymbolicContext.InputModel.HeapInfo.IsEmpty().Should().BeTrue();
            result.Iterations[1].Exception.Type.FullName.Should().Be("System.IndexOutOfRangeException");
            result.Iterations[2].Exception.Should().BeNull();
            result.Iterations[2].Output.Trim().Should().Be("arr[1] == null");
            result.Iterations[3].Exception.Should().BeNull();

            IReadOnlyModel inModel = result.Iterations[3].SymbolicContext.InputModel;
            IReadOnlyModel outModel = result.Iterations[3].SymbolicContext.OutputModel;

            inModel.HeapInfo.Locations.Should().HaveCount(2);
            outModel.HeapInfo.Locations.Should().HaveCount(2);

            IVariable arrVar = Variable.MethodArgument(result.EntryPoint.Parameters[0]);
            Location arrLoc = (Location)inModel.GetValueOrDefault(arrVar);

            IReadOnlyArrayHeapNode arrNode = (IReadOnlyArrayHeapNode)inModel.HeapInfo.GetNode(arrLoc);
            arrNode.GetElementOrDefault(1).Should().NotBe(Location.Null);
            
            arrNode = (IReadOnlyArrayHeapNode)outModel.HeapInfo.GetNode(arrLoc);
            arrNode.GetElementOrDefault(1).Should().Be(Location.Null);
        }
    }
}
