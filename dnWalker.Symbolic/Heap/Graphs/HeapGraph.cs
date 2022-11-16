using QuikGraph;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QuikGraph.Algorithms;
using QuikGraph.Algorithms.ConnectedComponents;
using QuikGraph.Algorithms.TopologicalSort;

namespace dnWalker.Symbolic.Heap.Graphs
{
    public class HeapGraph : IBidirectionalGraph<Location, HeapEdge>
    {
        private readonly IReadOnlyHeapInfo _heap;

        private IReadOnlyList<DependencyGroup>? _groups;
        private readonly BidirectionalGraph<Location, HeapEdge> _graph;

        internal HeapGraph(BidirectionalGraph<Location, HeapEdge> graph, IReadOnlyHeapInfo heap)
        {
            _graph = graph;
            _heap = heap;

        }

        public IReadOnlyList<DependencyGroup> GetDependencyGroups()
        {
            if (_groups == null)
            {
                // condensate the graph:
                // 1) find condensation (SSCs)
                StronglyConnectedComponentsAlgorithm<Location, HeapEdge> sccAlg = new StronglyConnectedComponentsAlgorithm<Location, HeapEdge>(this);
                sccAlg.Compute();
                int componentCount = sccAlg.ComponentCount;
                IDictionary<Location, int> components = sccAlg.Components;

                List<Location>[] groupBuilders = new List<Location>[componentCount];
                for (int i = 0; i < componentCount; ++i)
                {
                    groupBuilders[i] = new List<Location>();
                }

                foreach ((Location location, int component) in components)
                {
                    groupBuilders[component].Add(location);
                }

                DependencyGroup[] groups = Build(groupBuilders, _heap);

                AdjacencyGraph<DependencyGroup, Edge<DependencyGroup>> depGraph = new AdjacencyGraph<DependencyGroup, Edge<DependencyGroup>>(false, groups.Length);
                depGraph.AddVertexRange(groups);

                // 2) build edges between the SSCs
                foreach (HeapEdge e in _graph.Edges)
                {
                    int srcCmp = components[e.Source];
                    int trgCmp = components[e.Target];

                    if (srcCmp != trgCmp)
                    {
                        depGraph.AddEdge(new Edge<DependencyGroup>(groups[srcCmp], groups[trgCmp]));
                    }
                }

                // do topological ordering of the condensated graph
                _groups = TopologicalSort(depGraph);
            }
            return _groups;

            static DependencyGroup[] Build(List<Location>[] builders, IReadOnlyHeapInfo heap)
            {
                DependencyGroup[] groups = new DependencyGroup[builders.Length];
                for (int i = 0; i < groups.Length; ++i)
                {
                    groups[i] = new DependencyGroup(builders[i].Select(loc => heap.GetNode(loc)));
                }
                return groups;
            }
            static DependencyGroup[] TopologicalSort(AdjacencyGraph<DependencyGroup, Edge<DependencyGroup>> depGraph)
            {
                //return depGraph.TopologicalSort().ToArray();
                TopologicalSortAlgorithm<DependencyGroup, Edge<DependencyGroup>> tsAlg = new TopologicalSortAlgorithm<DependencyGroup, Edge<DependencyGroup>>(depGraph, depGraph.VertexCount);
                tsAlg.Compute();
                return tsAlg.SortedVertices;

            }
        }

        #region IBidirectionalGraph<Location, HeapEdge>
        public bool IsVerticesEmpty
        {
            get
            {
                return ((IVertexSet<Location>)_graph).IsVerticesEmpty;
            }
        }

        public int VertexCount
        {
            get
            {
                return ((IVertexSet<Location>)_graph).VertexCount;
            }
        }

        public IEnumerable<Location> Vertices
        {
            get
            {
                return ((IVertexSet<Location>)_graph).Vertices;
            }
        }

        public bool ContainsEdge(HeapEdge edge)
        {
            return ((IEdgeSet<Location, HeapEdge>)_graph).ContainsEdge(edge);
        }

        public bool IsEdgesEmpty
        {
            get
            {
                return ((IEdgeSet<Location, HeapEdge>)_graph).IsEdgesEmpty;
            }
        }

        public int EdgeCount
        {
            get
            {
                return ((IEdgeSet<Location, HeapEdge>)_graph).EdgeCount;
            }
        }

        public IEnumerable<HeapEdge> Edges
        {
            get
            {
                return ((IEdgeSet<Location, HeapEdge>)_graph).Edges;
            }
        }

        public bool IsInEdgesEmpty(Location vertex)
        {
            return ((IBidirectionalIncidenceGraph<Location, HeapEdge>)_graph).IsInEdgesEmpty(vertex);
        }

        public int InDegree(Location vertex)
        {
            return ((IBidirectionalIncidenceGraph<Location, HeapEdge>)_graph).InDegree(vertex);
        }

        public IEnumerable<HeapEdge> InEdges(Location vertex)
        {
            return ((IBidirectionalIncidenceGraph<Location, HeapEdge>)_graph).InEdges(vertex);
        }

        public bool TryGetInEdges(Location vertex, out IEnumerable<HeapEdge> edges)
        {
            return ((IBidirectionalIncidenceGraph<Location, HeapEdge>)_graph).TryGetInEdges(vertex, out edges);
        }

        public HeapEdge InEdge(Location vertex, int index)
        {
            return ((IBidirectionalIncidenceGraph<Location, HeapEdge>)_graph).InEdge(vertex, index);
        }

        public int Degree(Location vertex)
        {
            return ((IBidirectionalIncidenceGraph<Location, HeapEdge>)_graph).Degree(vertex);
        }

        public bool ContainsEdge(Location source, Location target)
        {
            return ((IIncidenceGraph<Location, HeapEdge>)_graph).ContainsEdge(source, target);
        }

        public bool TryGetEdge(Location source, Location target, out HeapEdge edge)
        {
            return ((IIncidenceGraph<Location, HeapEdge>)_graph).TryGetEdge(source, target, out edge);
        }

        public bool TryGetEdges(Location source, Location target, out IEnumerable<HeapEdge> edges)
        {
            return ((IIncidenceGraph<Location, HeapEdge>)_graph).TryGetEdges(source, target, out edges);
        }

        public bool IsOutEdgesEmpty(Location vertex)
        {
            return ((IImplicitGraph<Location, HeapEdge>)_graph).IsOutEdgesEmpty(vertex);
        }

        public int OutDegree(Location vertex)
        {
            return ((IImplicitGraph<Location, HeapEdge>)_graph).OutDegree(vertex);
        }

        public IEnumerable<HeapEdge> OutEdges(Location vertex)
        {
            return ((IImplicitGraph<Location, HeapEdge>)_graph).OutEdges(vertex);
        }

        public bool TryGetOutEdges(Location vertex, out IEnumerable<HeapEdge> edges)
        {
            return ((IImplicitGraph<Location, HeapEdge>)_graph).TryGetOutEdges(vertex, out edges);
        }

        public HeapEdge OutEdge(Location vertex, int index)
        {
            return ((IImplicitGraph<Location, HeapEdge>)_graph).OutEdge(vertex, index);
        }

        public bool IsDirected
        {
            get
            {
                return ((IGraph<Location, HeapEdge>)_graph).IsDirected;
            }
        }

        public bool AllowParallelEdges
        {
            get
            {
                return ((IGraph<Location, HeapEdge>)_graph).AllowParallelEdges;
            }
        }

        public bool ContainsVertex(Location vertex)
        {
            return ((IImplicitVertexSet<Location>)_graph).ContainsVertex(vertex);
        }
        #endregion IBidirectionalGraph<Location, HeapEdge>
    }
}
