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
    public abstract class TestClass
    {
        public object PublicField;
        private object PrivateField;

        public abstract object Run();
    }

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
            ModuleDef mainModule = ModuleDefMD.Load(typeof(HeapGraphTests).Module, ctx);

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

            HeapVertex arrayVertex = graph.GetVertex(arrayNode);

            graph.TryGetInEdges(arrayVertex, out IEnumerable<HeapEdge> edges).Should().BeTrue();

            edges.Should().AllSatisfy(e => TypeEqualityComparer.Instance.Equals(e.Source.Node.Type, mainModule.CorLibTypes.Object));
        }

        [Fact]
        public void ArrayElementsMultiEdge()
        {
            ModuleContext ctx = ModuleDef.CreateModuleContext();
            ModuleDef mainModule = ModuleDefMD.Load(typeof(HeapGraphTests).Module, ctx);

            HeapInfo heap = new HeapInfo();

            IArrayHeapNode arrayNode = heap.InitializeArray(mainModule.CorLibTypes.Object, 5);
            IObjectHeapNode objNode = heap.InitializeObject(mainModule.CorLibTypes.Object);
            for (int i = 0; i < 5; ++i)
            {
                arrayNode.SetElement(i, objNode.Location);
            }

            HeapGraph graph = heap.CreateGraph();

            graph.VertexCount.Should().Be(2);
            graph.EdgeCount.Should().Be(5);

            HeapVertex arrayVertex = graph.GetVertex(arrayNode);

            graph.TryGetInEdges(arrayVertex, out IEnumerable<HeapEdge> edges).Should().BeTrue();

            edges.Should().AllSatisfy(e => TypeEqualityComparer.Instance.Equals(e.Source.Node.Type, mainModule.CorLibTypes.Object));
        }

        [Fact]
        public void InstanceField()
        {
            ModuleContext ctx = ModuleDef.CreateModuleContext();
            ModuleDef mainModule = ModuleDefMD.Load(typeof(HeapGraphTests).Module, ctx);

            HeapInfo heap = new HeapInfo();

            TypeDef testClassTD = mainModule.Find("dnWalker.TestGenerator.Tests.Heap.TestClass", false);
            TypeSig testClassSig = testClassTD
                .ToTypeSig();


            IObjectHeapNode objNode = heap.InitializeObject(testClassSig);
            IObjectHeapNode pubNode = heap.InitializeObject(mainModule.CorLibTypes.Object);
            IObjectHeapNode prvNode = heap.InitializeObject(mainModule.CorLibTypes.Object);

            objNode.SetField(testClassTD.GetField("PublicField"), pubNode.Location);
            objNode.SetField(testClassTD.GetField("PrivateField"), prvNode.Location);

            HeapGraph graph = heap.CreateGraph();

            graph.VertexCount.Should().Be(3);
            graph.EdgeCount.Should().Be(2);

            HeapVertex objVertex = graph.GetVertex(objNode);
            HeapVertex pubVertex = graph.GetVertex(pubNode);
            HeapVertex prvVertex = graph.GetVertex(prvNode);

            graph.TryGetEdge(pubVertex, objVertex, out HeapEdge e1).Should().BeTrue();
            graph.TryGetEdge(prvVertex, objVertex, out HeapEdge e2).Should().BeTrue();

            e1.Type.Should().Be(HeapEdgeType.Field);
            e2.Type.Should().Be(HeapEdgeType.Field);
        }

        [Fact]
        public void MethodResult()
        {
            ModuleContext ctx = ModuleDef.CreateModuleContext();
            ModuleDef mainModule = ModuleDefMD.Load(typeof(HeapGraphTests).Module, ctx);

            HeapInfo heap = new HeapInfo();

            TypeDef testClassTD = mainModule.Find("dnWalker.TestGenerator.Tests.Heap.TestClass", false);
            TypeSig testClassSig = testClassTD
                .ToTypeSig();


            IObjectHeapNode objNode = heap.InitializeObject(testClassSig);
            IObjectHeapNode res1Node = heap.InitializeObject(mainModule.CorLibTypes.Object);
            IObjectHeapNode res2Node = heap.InitializeObject(mainModule.CorLibTypes.Object);

            objNode.SetMethodResult(testClassTD.FindMethod("Run", MethodSig.CreateInstance(mainModule.CorLibTypes.Object)), 1, res1Node.Location);
            objNode.SetMethodResult(testClassTD.FindMethod("Run", MethodSig.CreateInstance(mainModule.CorLibTypes.Object)), 2, res2Node.Location);

            HeapGraph graph = heap.CreateGraph();

            graph.VertexCount.Should().Be(3);
            graph.EdgeCount.Should().Be(2);

            HeapVertex objVertex = graph.GetVertex(objNode);
            HeapVertex res1Vertex = graph.GetVertex(res1Node);
            HeapVertex res2Vertex = graph.GetVertex(res2Node);

            graph.TryGetEdge(res1Vertex, objVertex, out HeapEdge e1).Should().BeTrue();
            graph.TryGetEdge(res2Vertex, objVertex, out HeapEdge e2).Should().BeTrue();

            e1.Type.Should().Be(HeapEdgeType.MethodResult);
            e2.Type.Should().Be(HeapEdgeType.MethodResult);
        }
    }
}
