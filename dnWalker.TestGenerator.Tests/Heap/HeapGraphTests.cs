using dnlib.DotNet;

using dnWalker.Symbolic.Heap;
using dnWalker.TestGenerator.Heap;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.TestGenerator.Tests.Heap
{
    public class HeapGraphTests
    {
        [Fact]
        public void EmptyHeap()
        {
            HeapInfo heap = new HeapInfo();
            HeapGraph graph = heap.CreateGraph();

            graph.Vertices.Should().HaveCount(0);
        }


        [Fact]
        public void ArrayElements()
        {
            ModuleContext ctx = ModuleDef.CreateModuleContext();
            ModuleDef mainModule = ModuleDefMD.Load(typeof(HeapGraphTests).Module);

            HeapInfo heap = new HeapInfo();

            IArrayHeapNode arrayNode = heap.InitializeArray(mainModule.CorLibTypes.Object, 5);
            for (int i = 0; i < 5; ++i)
            {
                IObjectHeapNode objNode = heap.InitializeObject(mainModule.CorLibTypes.Object);
                arrayNode.SetElement(i, objNode.Location);
            }

            HeapGraph graph = heap.CreateGraph();

            graph.VertexCount.Should().Be(6);

            graph.EdgeCount.Should().Be(5);

            HeapVertex arrayVertex = graph.Vertices.First(v => v.Node is IReadOnlyArrayHeapNode);

            graph.TryGetInEdges(arrayVertex, out IEnumerable<HeapEdge> edges).Should().BeTrue();

            edges.Should().AllSatisfy(e => TypeEqualityComparer.Instance.Equals(e.Source.Node.Type, mainModule.CorLibTypes.Object));
        }
    }
}
