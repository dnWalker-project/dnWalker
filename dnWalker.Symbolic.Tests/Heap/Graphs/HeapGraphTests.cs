using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic.Heap.Graphs;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Symbolic.Tests.Heap.Graphs
{

    public class HeapGraphTests
    {
        private abstract class TestClass
        {
            public object PublicField;
            private object PrivateField;

            public abstract object Run();
            public abstract object Run(string name, TestClass other);

            public static int MagicNumber;
        }

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
            Location[] elementNodes = new Location[5];

            for (int i = 0; i < 5; ++i)
            {
                IObjectHeapNode objNode = heap.InitializeObject(mainModule.CorLibTypes.Object);
                arrayNode.SetElement(i, objNode.Location);
                elementNodes[i] = objNode.Location;
            }

            HeapGraph graph = heap.CreateGraph();

            graph.VertexCount.Should().Be(6);
            graph.EdgeCount.Should().Be(5);

            graph.TryGetInEdges(arrayNode.Location, out IEnumerable<HeapEdge> edges).Should().BeTrue();

            edges.Select(static e => e.Source).Should().Contain(elementNodes);
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

            graph.TryGetInEdges(arrayNode.Location, out IEnumerable<HeapEdge> edges).Should().BeTrue();

            edges.Should().AllSatisfy(e => e.Source.Equals(objNode.Location));
        }

        [Fact]
        public void InstanceField()
        {
            ModuleContext ctx = ModuleDef.CreateModuleContext();
            ModuleDef mainModule = ModuleDefMD.Load(typeof(HeapGraphTests).Module, ctx);

            HeapInfo heap = new HeapInfo();

            TypeDef testClassTD = mainModule.Find("dnWalker.TestGenerator.Tests.Heap.HeapGraphTests/TestClass", false);
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

            graph.TryGetEdge(pubNode.Location, objNode.Location, out HeapEdge e1).Should().BeTrue();
            graph.TryGetEdge(prvNode.Location, objNode.Location, out HeapEdge e2).Should().BeTrue();

            e1.Type.Should().Be(HeapEdgeType.Field);
            e2.Type.Should().Be(HeapEdgeType.Field);
        }

        [Fact]
        public void MethodResult()
        {
            ModuleContext ctx = ModuleDef.CreateModuleContext();
            ModuleDef mainModule = ModuleDefMD.Load(typeof(HeapGraphTests).Module, ctx);

            HeapInfo heap = new HeapInfo();

            TypeDef testClassTD = mainModule.Find("dnWalker.TestGenerator.Tests.Heap.HeapGraphTests/TestClass", false);
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

            graph.TryGetEdge(res1Node.Location, objNode.Location, out HeapEdge e1).Should().BeTrue();
            graph.TryGetEdge(res2Node.Location, objNode.Location, out HeapEdge e2).Should().BeTrue();

            e1.Type.Should().Be(HeapEdgeType.MethodResult);
            e2.Type.Should().Be(HeapEdgeType.MethodResult);
        }

        [Fact]
        public void GraphCondensationIndependentNodes()
        {
            ModuleContext ctx = ModuleDef.CreateModuleContext();
            ModuleDef mainModule = ModuleDefMD.Load(typeof(HeapGraphTests).Module, ctx);

            HeapInfo heap = new HeapInfo();

            TypeDef testClassTD = mainModule.Find("dnWalker.TestGenerator.Tests.Heap.HeapGraphTests/TestClass", false);
            TypeSig testClassSig = testClassTD
                .ToTypeSig();

            IObjectHeapNode node1 = heap.InitializeObject(testClassSig);
            IObjectHeapNode node2 = heap.InitializeObject(testClassSig);
            IObjectHeapNode node3 = heap.InitializeObject(testClassSig);
            IObjectHeapNode node4 = heap.InitializeObject(testClassSig);

            HeapGraph graph = heap.CreateGraph();

            IReadOnlyList<DependencyGroup> groups = graph.GetDependencyGroups();

            graph.VertexCount.Should().Be(4);
            graph.EdgeCount.Should().Be(0);

            groups.Should().HaveCount(4);

            groups.Should().BeEquivalentTo(new DependencyGroup[]
            {
                new DependencyGroup(new IReadOnlyHeapNode[] { node1 }),
                new DependencyGroup(new IReadOnlyHeapNode[] { node2 }),
                new DependencyGroup(new IReadOnlyHeapNode[] { node3 }),
                new DependencyGroup(new IReadOnlyHeapNode[] { node4 })
            });
        }


        [Fact]
        public void GraphCondensationSimpleDependenciesOnly()
        {
            ModuleContext ctx = ModuleDef.CreateModuleContext();
            ModuleDef mainModule = ModuleDefMD.Load(typeof(HeapGraphTests).Module, ctx);

            HeapInfo heap = new HeapInfo();

            TypeDef testClassTD = mainModule.Find("dnWalker.TestGenerator.Tests.Heap.HeapGraphTests/TestClass", false);
            TypeSig testClassSig = testClassTD
                .ToTypeSig();

            IField pubFld = testClassTD.GetField("PublicField");
            IField prvFld = testClassTD.GetField("PrivateField");

            IObjectHeapNode node1 = heap.InitializeObject(testClassSig);
            IObjectHeapNode node2 = heap.InitializeObject(testClassSig);
            IObjectHeapNode node3 = heap.InitializeObject(testClassSig);
            IObjectHeapNode node4 = heap.InitializeObject(testClassSig);

            node3.SetField(pubFld, node2.Location);
            node3.SetField(prvFld, node4.Location);

            node2.SetField(pubFld, node1.Location);
            node2.SetField(prvFld, node1.Location);

            node4.SetField(pubFld, node2.Location);
            node4.SetField(prvFld, node1.Location);


            HeapGraph graph = heap.CreateGraph();

            IReadOnlyList<DependencyGroup> groups = graph.GetDependencyGroups();

            graph.VertexCount.Should().Be(4);
            graph.EdgeCount.Should().Be(6);

            groups.Should().HaveCount(4);

            groups[0].Should().BeEquivalentTo(new IReadOnlyHeapNode[] { node1 });
            groups[1].Should().BeEquivalentTo(new IReadOnlyHeapNode[] { node2 });
            groups[2].Should().BeEquivalentTo(new IReadOnlyHeapNode[] { node4 });
            groups[3].Should().BeEquivalentTo(new IReadOnlyHeapNode[] { node3 });
        }


        [Fact]
        public void GraphCondensationOneComplexDependency()
        {
            ModuleContext ctx = ModuleDef.CreateModuleContext();
            ModuleDef mainModule = ModuleDefMD.Load(typeof(HeapGraphTests).Module, ctx);

            HeapInfo heap = new HeapInfo();

            TypeDef testClassTD = mainModule.Find("dnWalker.TestGenerator.Tests.Heap.HeapGraphTests/TestClass", false);
            TypeSig testClassSig = testClassTD
                .ToTypeSig();

            IField pubFld = testClassTD.GetField("PublicField");
            IField prvFld = testClassTD.GetField("PrivateField");

            IObjectHeapNode node1 = heap.InitializeObject(testClassSig);
            IObjectHeapNode node2 = heap.InitializeObject(testClassSig);
            IObjectHeapNode node3 = heap.InitializeObject(testClassSig);
            IObjectHeapNode node4 = heap.InitializeObject(testClassSig);

            node1.SetField(prvFld, node4.Location);
            node2.SetField(prvFld, node1.Location);
            node3.SetField(prvFld, node2.Location);
            node4.SetField(prvFld, node3.Location);

            node3.SetField(pubFld, node1.Location);
            node2.SetField(pubFld, node4.Location);


            HeapGraph graph = heap.CreateGraph();

            IReadOnlyList<DependencyGroup> groups = graph.GetDependencyGroups();

            graph.VertexCount.Should().Be(4);
            graph.EdgeCount.Should().Be(6);

            groups.Should().HaveCount(1);

            groups[0].Should().Contain(new IReadOnlyHeapNode[] { node1, node2, node3, node4 });
        }


        [Fact]
        public void GraphCondensationComplexDependenciesOnly()
        {
            ModuleContext ctx = ModuleDef.CreateModuleContext();
            ModuleDef mainModule = ModuleDefMD.Load(typeof(HeapGraphTests).Module, ctx);

            HeapInfo heap = new HeapInfo();

            TypeDef testClassTD = mainModule.Find("dnWalker.TestGenerator.Tests.Heap.HeapGraphTests/TestClass", false);
            TypeSig testClassSig = testClassTD
                .ToTypeSig();

            IField pubFld = testClassTD.GetField("PublicField");
            IField prvFld = testClassTD.GetField("PrivateField");

            IObjectHeapNode node1 = heap.InitializeObject(testClassSig);
            IObjectHeapNode node2 = heap.InitializeObject(testClassSig);
            IObjectHeapNode node3 = heap.InitializeObject(testClassSig);
            IObjectHeapNode node4 = heap.InitializeObject(testClassSig);

            node1.SetField(prvFld, node2.Location);
            node2.SetField(prvFld, node1.Location);

            node3.SetField(prvFld, node4.Location);
            node4.SetField(prvFld, node3.Location);

            node3.SetField(pubFld, node2.Location);

            HeapGraph graph = heap.CreateGraph();

            IReadOnlyList<DependencyGroup> groups = graph.GetDependencyGroups();

            graph.VertexCount.Should().Be(4);
            graph.EdgeCount.Should().Be(5);

            groups.Should().HaveCount(2);

            groups[0].Should().Contain(new IReadOnlyHeapNode[] { node1, node2 });
            groups[1].Should().Contain(new IReadOnlyHeapNode[] { node3, node4 });
        }


        [Fact]
        public void GraphCondensationComplexAndSimpleDependencies()
        {
            ModuleContext ctx = ModuleDef.CreateModuleContext();
            ModuleDef mainModule = ModuleDefMD.Load(typeof(HeapGraphTests).Module, ctx);

            HeapInfo heap = new HeapInfo();

            TypeDef testClassTD = mainModule.Find("dnWalker.TestGenerator.Tests.Heap.HeapGraphTests/TestClass", false);
            TypeSig testClassSig = testClassTD
                .ToTypeSig();

            IField pubFld = testClassTD.GetField("PublicField");
            IField prvFld = testClassTD.GetField("PrivateField");

            IObjectHeapNode node1 = heap.InitializeObject(testClassSig);
            IObjectHeapNode node2 = heap.InitializeObject(testClassSig);
            IObjectHeapNode node3 = heap.InitializeObject(testClassSig);
            IObjectHeapNode node4 = heap.InitializeObject(testClassSig);

            node2.SetField(pubFld, node1.Location);
            

            node2.SetField(prvFld, node4.Location);
            node3.SetField(prvFld, node2.Location);
            node4.SetField(prvFld, node3.Location);

            HeapGraph graph = heap.CreateGraph();

            IReadOnlyList<DependencyGroup> groups = graph.GetDependencyGroups();

            graph.VertexCount.Should().Be(4);
            graph.EdgeCount.Should().Be(4);

            groups.Should().HaveCount(2);

            groups[0].Should().Contain(new IReadOnlyHeapNode[] { node1});
            groups[1].Should().Contain(new IReadOnlyHeapNode[] { node2, node3, node4 });
        }
    }
}
