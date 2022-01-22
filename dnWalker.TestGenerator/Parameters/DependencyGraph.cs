using dnWalker.Parameters;

using QuikGraph;
using QuikGraph.Algorithms;
using QuikGraph.Utils;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace dnWalker.TestGenerator.Parameters
{
    public abstract class Dependency
    {
        protected Dependency()
        {
        }

    }

    public class ComplexDependency : Dependency
    {
        public ComplexDependency(IEnumerable<Dependency> innerDependencies)
        {
            InnerDependencies = new List<Dependency>(innerDependencies);
        }

        public IReadOnlyList<Dependency> InnerDependencies { get; }
    }

    public class SimpleDependency : Dependency
    {
        public SimpleDependency(IParameter parameter)
        {
            Parameter = parameter;
        }
        public IParameter Parameter { get; }
    }

    public class DependencyGraph
    {
        private readonly IReadOnlyParameterContext _context;
        private readonly AdjacencyGraph<Dependency, Edge<Dependency>> _graph;

        private DependencyGraph(IReadOnlyParameterContext context, AdjacencyGraph<Dependency, Edge<Dependency>> graph) 
        {
            _context = context;
            _graph = graph;
        }

        public static DependencyGraph Build(IReadOnlyParameterContext context)
        {

            Dictionary<ParameterRef, Dependency> lookup = context.Parameters

                // choose only non primitive value parameters - will be replaced by a constant dependency
                .ToDictionary(p => p.Key, p => (Dependency) new SimpleDependency(p.Value));

            AdjacencyGraph<Dependency, Edge<Dependency>> bigGraph = new AdjacencyGraph<Dependency, Edge<Dependency>>();

            foreach (var key in lookup)
            {
                bigGraph.AddVertex(key.Value);
            }

            foreach (var p in context.Parameters.Values)
            {
                Dependency src = lookup[p.Reference];
                // the parameter can depend on other parameters => normal behaviour
                foreach (ParentChildParameterAccessor parentChild in p.Accessors.OfType<ParentChildParameterAccessor>())
                {
                    Dependency trg = lookup[parentChild.ParentRef];
                    bigGraph.AddEdge(new Edge<Dependency>(src, trg));
                }
            }

            DependencyGraph dependencyGraph = new DependencyGraph(context, bigGraph.Condensate<Dependency, AdjacencyGraph<Dependency, Edge<Dependency>>>(static (cyclicDependencies) => new ComplexDependency(cyclicDependencies)));
            return dependencyGraph;
        }

        public IEnumerable<Dependency> GetRootDependencies()
        {
            return _graph.Roots();
        }

        public IEnumerable<Dependency> GetDependencies()
        {
            return _graph.Vertices;
        }

        public IEnumerable<Dependency> GetSortedDependencies()
        {
            return _graph.TopologicalSort();
        }
    }
}
