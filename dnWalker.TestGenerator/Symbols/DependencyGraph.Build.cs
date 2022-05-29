using System;
using System.Linq;
using System.Collections.Generic;


using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;

using QuikGraph;
using QuikGraph.Algorithms.ConnectedComponents;

using SymbolVertex = System.Collections.Generic.List<dnWalker.TestGenerator.Symbols.TemplateSymbol>;
using SymbolEdge = QuikGraph.Edge<System.Collections.Generic.List<dnWalker.TestGenerator.Symbols.TemplateSymbol>>;
using SymbolGraph = QuikGraph.BidirectionalGraph<System.Collections.Generic.List<dnWalker.TestGenerator.Symbols.TemplateSymbol>, QuikGraph.Edge<System.Collections.Generic.List<dnWalker.TestGenerator.Symbols.TemplateSymbol>>>;


using SimpleSymbolVertex = dnWalker.TestGenerator.Symbols.TemplateSymbol;
using SimpleSymbolEdge = QuikGraph.Edge<dnWalker.TestGenerator.Symbols.TemplateSymbol>;
using SimpleSymbolGraph = QuikGraph.BidirectionalGraph<dnWalker.TestGenerator.Symbols.TemplateSymbol, QuikGraph.Edge<dnWalker.TestGenerator.Symbols.TemplateSymbol>>;


namespace dnWalker.TestGenerator.Symbols
{
    public partial class DependencyGraph
    {

        private static SymbolGraph BuildGraph(List<TemplateSymbol> symbols)
        {
            SimpleSymbolGraph graph = new SimpleSymbolGraph();

            graph.AddVertexRange(symbols);

            Dictionary<Location, TemplateSymbol> locToSymbol = new Dictionary<Location, TemplateSymbol>();

            foreach (TemplateSymbol symbol in symbols)
            {
                if (symbol.Value is Location location)
                {
                    locToSymbol.Add(location, symbol);
                }
            }

            foreach ((Location location, TemplateSymbol symbol) in locToSymbol.Select(static p => (p.Key, p.Value)))
            {
                IReadOnlyHeapNode heapNode = symbol.Model.HeapInfo.GetNode(location);
                foreach (Location dependency in GetDependencies(heapNode))
                {
                    SimpleSymbolVertex dependencySymbol = locToSymbol[dependency];
                    SimpleSymbolEdge edge = new SimpleSymbolEdge(dependencySymbol, symbol);
                    graph.AddEdge(edge);
                }
            }

            return Condensate(graph);


            static IEnumerable<Location> GetDependencies(IReadOnlyHeapNode heapNode)
            {
                if (heapNode is IReadOnlyArrayHeapNode arrayNode)
                {
                    foreach (int index in arrayNode.Indeces)
                    {
                        IValue value = arrayNode.GetElement(index);
                        if (value is Location location) yield return location;
                        yield break;
                    }
                }
                else if (heapNode is IReadOnlyObjectHeapNode objectNode)
                {
                    foreach (IField fld in objectNode.Fields)
                    {
                        IValue value = objectNode.GetField(fld);
                        if (value is Location location) yield return location;
                    }
                    foreach ((IMethod method, int invocation) in objectNode.MethodInvocations)
                    {
                        IValue value = objectNode.GetMethodResult(method, invocation);
                        if (value is Location location) yield return location;
                    }
                }
                yield break;
            }

            static SymbolGraph Condensate(SimpleSymbolGraph simpleGraph)
            {
                var sccAlg = new StronglyConnectedComponentsAlgorithm<SimpleSymbolVertex, SimpleSymbolEdge>(simpleGraph);

                sccAlg.Compute();

                int componentCount = sccAlg.ComponentCount;

                SymbolGraph condensated = new SymbolGraph();

                SimpleSymbolGraph[] componentGraphs = sccAlg.Graphs;
                Dictionary<SimpleSymbolVertex, int> vertex2component = new Dictionary<SimpleSymbolVertex, int>(sccAlg.Components);
                SymbolVertex[] component2vertex = new SymbolVertex[componentCount];

                // build the vertices of the condensated graph
                for (int i = 0; i < componentGraphs.Length; ++i)
                {
                    if (componentGraphs[i].VertexCount == 1)
                    {
                        // a single vertex component => just put the dependency as it is
                        SymbolVertex vertex = new SymbolVertex { componentGraphs[i].Vertices.First() };
                        condensated.AddVertex(vertex);
                        component2vertex[i] = vertex;
                    }
                    else
                    {
                        SymbolVertex vertex = new SymbolVertex(componentGraphs[i].Vertices);
                        condensated.AddVertex(vertex);
                        //vertex2component[vertex] = i;
                        component2vertex[i] = vertex;
                    }
                }

                // add the edges
                foreach (var e in simpleGraph.Edges)
                {
                    int srcCmp = vertex2component[e.Source];
                    int trgCmp = vertex2component[e.Target];

                    if (srcCmp != trgCmp)
                    {
                        // this edge is between two SCCs
                        // make a new edge between the condensated vertices
                        condensated.AddEdge(new SymbolEdge(component2vertex[srcCmp], component2vertex[trgCmp]));
                    }
                }

                return condensated;
            }
        }
    }
}

