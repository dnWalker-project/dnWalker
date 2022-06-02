using QuikGraph;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;

namespace dnWalker.TestGenerator.Heap
{
    public class HeapGraph : IBidirectionalGraph<HeapVertex, HeapEdge>
    {
        private List<DependencyGroup>? _groups;
        private readonly BidirectionalGraph<HeapVertex, HeapEdge> _graph;
        private readonly Dictionary<Location, HeapVertex> _vertexLookup;

        internal HeapGraph(BidirectionalGraph<HeapVertex, HeapEdge> graph)
        {
            _graph = graph;
            _vertexLookup = graph.Vertices.ToDictionary(static v => v.Node.Location);

        }

        public IReadOnlyList<DependencyGroup> GetDependencyGroups()
        {
            if (_groups != null) return _groups;

            _groups = new List<DependencyGroup>();



            return _groups;
        }

        public HeapVertex GetVertex(IReadOnlyHeapNode heapNode)
        {
            return GetVertex(heapNode.Location);
        }
        public HeapVertex GetVertex(Location location)
        {
            return _vertexLookup[location];
        }


        #region IBidirectionalGraph<HeapVertex, HeapEdge>
        public bool IsVerticesEmpty
        {
            get
            {
                return ((IVertexSet<HeapVertex>)_graph).IsVerticesEmpty;
            }
        }

        public int VertexCount
        {
            get
            {
                return ((IVertexSet<HeapVertex>)_graph).VertexCount;
            }
        }

        public IEnumerable<HeapVertex> Vertices
        {
            get
            {
                return ((IVertexSet<HeapVertex>)_graph).Vertices;
            }
        }

        public bool ContainsEdge(HeapEdge edge)
        {
            return ((IEdgeSet<HeapVertex, HeapEdge>)_graph).ContainsEdge(edge);
        }

        public bool IsEdgesEmpty
        {
            get
            {
                return ((IEdgeSet<HeapVertex, HeapEdge>)_graph).IsEdgesEmpty;
            }
        }

        public int EdgeCount
        {
            get
            {
                return ((IEdgeSet<HeapVertex, HeapEdge>)_graph).EdgeCount;
            }
        }

        public IEnumerable<HeapEdge> Edges
        {
            get
            {
                return ((IEdgeSet<HeapVertex, HeapEdge>)_graph).Edges;
            }
        }

        public bool IsInEdgesEmpty(HeapVertex vertex)
        {
            return ((IBidirectionalIncidenceGraph<HeapVertex, HeapEdge>)_graph).IsInEdgesEmpty(vertex);
        }

        public int InDegree(HeapVertex vertex)
        {
            return ((IBidirectionalIncidenceGraph<HeapVertex, HeapEdge>)_graph).InDegree(vertex);
        }

        public IEnumerable<HeapEdge> InEdges(HeapVertex vertex)
        {
            return ((IBidirectionalIncidenceGraph<HeapVertex, HeapEdge>)_graph).InEdges(vertex);
        }

        public bool TryGetInEdges(HeapVertex vertex, out IEnumerable<HeapEdge> edges)
        {
            return ((IBidirectionalIncidenceGraph<HeapVertex, HeapEdge>)_graph).TryGetInEdges(vertex, out edges);
        }

        public HeapEdge InEdge(HeapVertex vertex, int index)
        {
            return ((IBidirectionalIncidenceGraph<HeapVertex, HeapEdge>)_graph).InEdge(vertex, index);
        }

        public int Degree(HeapVertex vertex)
        {
            return ((IBidirectionalIncidenceGraph<HeapVertex, HeapEdge>)_graph).Degree(vertex);
        }

        public bool ContainsEdge(HeapVertex source, HeapVertex target)
        {
            return ((IIncidenceGraph<HeapVertex, HeapEdge>)_graph).ContainsEdge(source, target);
        }

        public bool TryGetEdge(HeapVertex source, HeapVertex target, out HeapEdge edge)
        {
            return ((IIncidenceGraph<HeapVertex, HeapEdge>)_graph).TryGetEdge(source, target, out edge);
        }

        public bool TryGetEdges(HeapVertex source, HeapVertex target, out IEnumerable<HeapEdge> edges)
        {
            return ((IIncidenceGraph<HeapVertex, HeapEdge>)_graph).TryGetEdges(source, target, out edges);
        }

        public bool IsOutEdgesEmpty(HeapVertex vertex)
        {
            return ((IImplicitGraph<HeapVertex, HeapEdge>)_graph).IsOutEdgesEmpty(vertex);
        }

        public int OutDegree(HeapVertex vertex)
        {
            return ((IImplicitGraph<HeapVertex, HeapEdge>)_graph).OutDegree(vertex);
        }

        public IEnumerable<HeapEdge> OutEdges(HeapVertex vertex)
        {
            return ((IImplicitGraph<HeapVertex, HeapEdge>)_graph).OutEdges(vertex);
        }

        public bool TryGetOutEdges(HeapVertex vertex, out IEnumerable<HeapEdge> edges)
        {
            return ((IImplicitGraph<HeapVertex, HeapEdge>)_graph).TryGetOutEdges(vertex, out edges);
        }

        public HeapEdge OutEdge(HeapVertex vertex, int index)
        {
            return ((IImplicitGraph<HeapVertex, HeapEdge>)_graph).OutEdge(vertex, index);
        }

        public bool IsDirected
        {
            get
            {
                return ((IGraph<HeapVertex, HeapEdge>)_graph).IsDirected;
            }
        }

        public bool AllowParallelEdges
        {
            get
            {
                return ((IGraph<HeapVertex, HeapEdge>)_graph).AllowParallelEdges;
            }
        }

        public bool ContainsVertex(HeapVertex vertex)
        {
            return ((IImplicitVertexSet<HeapVertex>)_graph).ContainsVertex(vertex);
        }
        #endregion IBidirectionalGraph<HeapVertex, HeapEdge>
    }
}
