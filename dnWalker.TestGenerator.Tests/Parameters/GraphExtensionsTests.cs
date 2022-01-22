using dnWalker.TestGenerator.Parameters;

using FluentAssertions;

using QuikGraph;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.TestGenerator.Tests.Parameters
{
    public class GraphExtensionsTests
    {
        public abstract class Vertex
        { 
            public static implicit operator Vertex(int value)
            {
                return new IntVertex(value);
            }
        }

        public class IntVertex : Vertex, IEquatable<IntVertex>
        {
            public IntVertex(int value)
            {
                Value = value;
            }

            public int Value { get; }

            public override bool Equals(object obj)
            {
                return Equals(obj as IntVertex);
            }

            public bool Equals(IntVertex other)
            {
                return other != null &&
                       Value == other.Value;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Value);
            }

            public static bool operator ==(IntVertex left, IntVertex right)
            {
                return EqualityComparer<IntVertex>.Default.Equals(left, right);
            }

            public static bool operator !=(IntVertex left, IntVertex right)
            {
                return !(left == right);
            }
        }

        public class CompositeVertex : Vertex
        {
            public CompositeVertex(IEnumerable<Vertex> inner)
            {
                InnerVertices = inner.ToList();
            }

            public IReadOnlyList<Vertex> InnerVertices { get; }
        }

        public class Graph : AdjacencyGraph<Vertex, Edge<Vertex>>
        { }

        [Fact]
        public void TestCondensation_NoComplexComponents()
        {
            // the graph contains only simple (single element) SCCs

            Graph sourceGraph = new Graph();

            sourceGraph.AddVerticesAndEdge(new Edge<Vertex>(1, 2));
            sourceGraph.AddVerticesAndEdge(new Edge<Vertex>(1, 3));
            sourceGraph.AddVerticesAndEdge(new Edge<Vertex>(4, 3));
            sourceGraph.AddVerticesAndEdge(new Edge<Vertex>(3, 5));

            Graph condensated = sourceGraph.Condensate<Vertex, Graph>(static vs => new CompositeVertex(vs));

            condensated.Should().BeSameAs(sourceGraph);
        }

        [Fact]
        public void TestCondensation_SingleSCC()
        {
            // the graph is a cycle => 1 SCC

            Graph sourceGraph = new Graph();

            sourceGraph.AddVerticesAndEdge(new Edge<Vertex>(1, 2));
            sourceGraph.AddVerticesAndEdge(new Edge<Vertex>(2, 3));
            sourceGraph.AddVerticesAndEdge(new Edge<Vertex>(3, 4));
            sourceGraph.AddVerticesAndEdge(new Edge<Vertex>(4, 1));

            Graph condensated = sourceGraph.Condensate<Vertex, Graph>(static vs => new CompositeVertex(vs));

            condensated.Should().NotBeSameAs(sourceGraph);
            condensated.VertexCount.Should().Be(1);
            condensated.Vertices.First().Should().BeOfType<CompositeVertex>();
            ((CompositeVertex)condensated.Vertices.First()).InnerVertices.Should().BeEquivalentTo(sourceGraph.Vertices);
        }

        [Fact]
        public void Test_Condensation_SingleSCC_WithTails()
        {
            // the graph is a cycle => 1 SCC

            Graph sourceGraph = new Graph();

            sourceGraph.AddVerticesAndEdge(new Edge<Vertex>(1, 2));
            sourceGraph.AddVerticesAndEdge(new Edge<Vertex>(2, 3));
            sourceGraph.AddVerticesAndEdge(new Edge<Vertex>(3, 1));
            sourceGraph.AddVerticesAndEdge(new Edge<Vertex>(4, 1));
            sourceGraph.AddVerticesAndEdge(new Edge<Vertex>(3, 5));

            Graph condensated = sourceGraph.Condensate<Vertex, Graph>(static vs => new CompositeVertex(vs));

            condensated.VertexCount.Should().Be(3);
            condensated.OutEdges(4).Should().HaveCount(1);
            condensated.OutEdges(condensated.Vertices.OfType<CompositeVertex>().First()).Should().HaveCount(1);
            condensated.OutEdges(5).Should().HaveCount(0);
        }

        [Fact]
        public void Test_Condensation_TwoSCCs()
        {

            Graph sourceGraph = new Graph();

            sourceGraph.AddVerticesAndEdge(new Edge<Vertex>(1, 2));
            sourceGraph.AddVerticesAndEdge(new Edge<Vertex>(2, 3));
            sourceGraph.AddVerticesAndEdge(new Edge<Vertex>(3, 1));
            sourceGraph.AddVerticesAndEdge(new Edge<Vertex>(4, 1));
            sourceGraph.AddVerticesAndEdge(new Edge<Vertex>(4, 5));
            sourceGraph.AddVerticesAndEdge(new Edge<Vertex>(5, 6));
            sourceGraph.AddVerticesAndEdge(new Edge<Vertex>(6, 4));

            Graph condensated = sourceGraph.Condensate<Vertex, Graph>(static vs => new CompositeVertex(vs));

            condensated.VertexCount.Should().Be(2);
            condensated.Vertices.Should().AllBeOfType<CompositeVertex>();
        }
    }
}
