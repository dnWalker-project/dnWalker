
using QuikGraph;
using QuikGraph.Algorithms.ConnectedComponents;

using System;
using System.Collections.Generic;
using System.Linq;


namespace dnWalker.TestGenerator.Parameters
{
    public static class GraphExtensions
    {
        public static TGraph Condensate<TVertex, TEdge, TGraph>(this TGraph graph, Func<IEnumerable<TVertex>, TVertex> mergeFunction, Func<TVertex, TVertex, TEdge> edgeFactory)
            where TGraph : IMutableVertexAndEdgeListGraph<TVertex, TEdge>, new()
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            return Condensate(graph, static () => new TGraph(), mergeFunction, edgeFactory);
        }

        public static TGraph Condensate<TVertex, TGraph>(this TGraph graph, Func<IEnumerable<TVertex>, TVertex> mergeFunction)
            where TGraph : IMutableVertexAndEdgeListGraph<TVertex, Edge<TVertex>>, new()
            where TVertex : notnull
        {
            return Condensate(graph, static () => new TGraph(), mergeFunction, static (src, trg) => new Edge<TVertex>(src, trg));
        }

        //public static TGraph Condensate<TVertex, TGraph>(this TGraph graph, Func<TGraph> graphFactory, Func<IEnumerable<TVertex>, TVertex> mergeFunction)
        //    where TGraph : IMutableVertexAndEdgeListGraph<TVertex, Edge<TVertex>>
        //    where TVertex : notnull
        //{
        //    return Condensate(graph, graphFactory, mergeFunction, static (src, trg) => new Edge<TVertex>(src, trg));
        //}

        public static TGraph Condensate<TVertex, TEdge, TGraph>(this TGraph graph, Func<TGraph> graphFactory, Func<IEnumerable<TVertex>, TVertex> mergeFunction, Func<TVertex, TVertex, TEdge> edgeFactory)
            where TGraph : IMutableVertexAndEdgeListGraph<TVertex, TEdge>
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {

            var sccAlg = new StronglyConnectedComponentsAlgorithm<TVertex, TEdge>(graph);

            sccAlg.Compute();

            int componentCount = sccAlg.ComponentCount;

            if (graph.VertexCount == componentCount)
            {
                return graph; // no need to condensate anything
            }

            TGraph condensated = graphFactory();

            BidirectionalGraph<TVertex, TEdge>[] componentGraphs = sccAlg.Graphs;
            Dictionary<TVertex, int> vertex2component = new Dictionary<TVertex, int>(sccAlg.Components);
            TVertex[] component2vertex = new TVertex[componentCount];

            // build the vertices of the condensated graph
            for (int i = 0; i < componentGraphs.Length; ++i)
            {
                if (componentGraphs[i].VertexCount == 1)
                {
                    // a single vertex component => just put the dependency as it is
                    TVertex vertex = componentGraphs[i].Vertices.First();
                    condensated.AddVertex(vertex);
                    component2vertex[i] = vertex;
                }
                else
                {
                    TVertex vertex = mergeFunction(componentGraphs[i].Vertices);
                    condensated.AddVertex(vertex);
                    vertex2component[vertex] = i;
                    component2vertex[i] = vertex;
                }
            }

            // add the edges
            foreach (var e in graph.Edges )
            {
                int srcCmp = vertex2component[e.Source];
                int trgCmp = vertex2component[e.Target];

                if (srcCmp != trgCmp)
                {
                    // this edge is between two SCCs
                    // make a new edge between the condensated vertices
                    condensated.AddEdge(edgeFactory(component2vertex[srcCmp], component2vertex[trgCmp]));
                }
            }

            return condensated;
        }
    }
}
