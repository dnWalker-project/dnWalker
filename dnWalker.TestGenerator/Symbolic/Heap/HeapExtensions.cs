using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;

using QuikGraph;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Symbolic.Heap
{
    public static class HeapExtensions
    {
        public static HeapGraph CreateGraph(this IReadOnlyHeapInfo heap)
        {
            BidirectionalGraph<Location, HeapEdge> graph = new BidirectionalGraph<Location, HeapEdge>(true, heap.Locations.Count);

            graph.AddVertexRange(heap.Locations);

            foreach (HeapNode node in heap.Nodes)
            {
                AddEdges(node, heap, graph);
            }

            return new HeapGraph(graph, heap);
        }

        private static void AddEdges(HeapNode node, IReadOnlyHeapInfo heap, BidirectionalGraph<Location, HeapEdge> graph)
        {
            switch (node)
            {
                case IReadOnlyArrayHeapNode arrayNode: AddArrayEdges(arrayNode, heap, graph); break;
                case IReadOnlyObjectHeapNode objectNode: AddObjectEdges(objectNode, heap, graph); break;
                default: throw new NotSupportedException("Unexpected heap node type.");
            }

        }

        private static void AddObjectEdges(IReadOnlyObjectHeapNode objectNode, IReadOnlyHeapInfo heap, BidirectionalGraph<Location, HeapEdge> graph)
        {
            if (objectNode.HasFields)
            {
                Location target = objectNode.Location;
                foreach (IField field in objectNode.Fields)
                {
                    IValue value = objectNode.GetFieldOrDefault(field);
                    if (value is Location source && source != Location.Null)
                    {
                        HeapEdge edge = new HeapEdge(source, target, HeapEdgeType.Field);
                        graph.AddEdge(edge);
                    }
                }
            }

            if (objectNode.HasMethodInvocations)
            {
                Location target = objectNode.Location;
                foreach ((IMethod method, int invocation) in objectNode.MethodInvocations)
                {
                    IValue value = objectNode.GetMethodResultOrDefault(method, invocation);
                    if (value is Location source && source != Location.Null)
                    {
                        HeapEdge edge = new HeapEdge(source, target, HeapEdgeType.MethodResult);
                        graph.AddEdge(edge);
                    }
                }
            }
        }

        private static void AddArrayEdges(IReadOnlyArrayHeapNode arrayNode, IReadOnlyHeapInfo heap, BidirectionalGraph<Location, HeapEdge> graph)
        {
            if (arrayNode.HasElements)
            {
                Location target = arrayNode.Location;
                foreach (int index in arrayNode.Indeces)
                {
                    IValue value = arrayNode.GetElementOrDefault(index);
                    if (value is Location source)
                    {
                        if (source != Location.Null)
                        {
                            HeapEdge edge = new HeapEdge(source, target, HeapEdgeType.ArrayElement);
                            graph.AddEdge(edge);
                        }
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
