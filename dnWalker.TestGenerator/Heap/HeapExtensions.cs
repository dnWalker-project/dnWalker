using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;

using QuikGraph;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Heap
{
    public static class HeapExtensions
    {
        public static HeapGraph CreateGraph(this IReadOnlyHeapInfo heap)
        {
            Dictionary<Location, HeapVertex> vertexLookup = heap.Nodes.ToDictionary(n => n.Location, n => new HeapVertex(heap, n));

            BidirectionalGraph<HeapVertex, HeapEdge> graph = new BidirectionalGraph<HeapVertex, HeapEdge>(true, vertexLookup.Count);

            graph.AddVertexRange(vertexLookup.Values);

            foreach (HeapNode node in heap.Nodes)
            {
                AddEdges(node, heap, graph, vertexLookup);
            }

            return new HeapGraph(graph);
        }

        private static void AddEdges(HeapNode node, IReadOnlyHeapInfo heap, BidirectionalGraph<HeapVertex, HeapEdge> graph, IReadOnlyDictionary<Location, HeapVertex> vertexLookup)
        {
            switch (node)
            {
                case IReadOnlyArrayHeapNode arrayNode: AddArrayEdges(arrayNode, heap, graph, vertexLookup); break;
                case IReadOnlyObjectHeapNode objectNode: AddObjectEdges(objectNode, heap, graph, vertexLookup); break;
                default: throw new NotSupportedException("Unexpected heap node type.");
            }

        }

        private static void AddObjectEdges(IReadOnlyObjectHeapNode objectNode, IReadOnlyHeapInfo heap, BidirectionalGraph<HeapVertex, HeapEdge> graph, IReadOnlyDictionary<Location, HeapVertex> vertexLookup)
        {
            if (objectNode.HasFields)
            {
                HeapVertex target = vertexLookup[objectNode.Location];
                foreach (IField field in objectNode.Fields)
                {
                    IValue value = objectNode.GetField(field);
                    if (value is Location fieldLocation)
                    {
                        HeapVertex source = vertexLookup[fieldLocation];
                        HeapEdge edge = new HeapEdge(source, target, HeapEdgeType.Field);
                        graph.AddEdge(edge);
                    }
                }
            }

            if (objectNode.HasMethodInvocations)
            {
                HeapVertex target = vertexLookup[objectNode.Location];
                foreach ((IMethod method, int invocation) in objectNode.MethodInvocations)
                {
                    IValue value = objectNode.GetMethodResult(method, invocation);
                    if (value is Location resultLocation)
                    {
                        HeapVertex source = vertexLookup[resultLocation];
                        HeapEdge edge = new HeapEdge(source, target, HeapEdgeType.MethodResult);
                        graph.AddEdge(edge);
                    }
                }
            }
        }

        private static void AddArrayEdges(IReadOnlyArrayHeapNode arrayNode, IReadOnlyHeapInfo heap, BidirectionalGraph<HeapVertex, HeapEdge> graph, IReadOnlyDictionary<Location, HeapVertex> vertexLookup)
        {
            if (arrayNode.HasElements)
            {
                HeapVertex target = vertexLookup[arrayNode.Location];
                foreach (int index in arrayNode.Indeces)
                {
                    IValue value = arrayNode.GetElement(index);
                    if (value is Location elementLocation)
                    {
                        HeapVertex source = vertexLookup[elementLocation];
                        HeapEdge edge = new HeapEdge(source, target, HeapEdgeType.ArrayElement);
                        graph.AddEdge(edge);
                    }
                    else
                    {
                        // a non location value means all of the values are non location...
                        break;
                    }
                }
            }

        }
    }
}
