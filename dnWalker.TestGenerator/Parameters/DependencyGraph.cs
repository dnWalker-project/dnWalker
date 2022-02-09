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
    internal abstract class Dependency
    {
        protected Dependency()
        {
        }

    }

    internal class ComplexDependency : Dependency
    {
        internal ComplexDependency(IEnumerable<Dependency> innerDependencies)
        {
            InnerDependencies = innerDependencies.ToList(); ;
        }

        internal IReadOnlyList<Dependency> InnerDependencies { get; }

        internal IEnumerable<IParameter> GetParameters()
        {
            return Enumerable.Concat
                (
                    InnerDependencies.OfType<SimpleDependency>().Select(sd => sd.Parameter),
                    InnerDependencies.OfType<ComplexDependency>().SelectMany(cd => cd.GetParameters())
                );
        }
    }

    internal class SimpleDependency : Dependency
    {

        internal SimpleDependency(IParameter parameter)
        {
            Parameter = parameter;
        }
        internal IParameter Parameter { get; }

    }

    internal class DependencyGraph
    {
        private readonly IReadOnlyParameterSet _set;
        private readonly AdjacencyGraph<Dependency, Edge<Dependency>> _graph;

        private DependencyGraph(IReadOnlyParameterSet set, AdjacencyGraph<Dependency, Edge<Dependency>> graph) 
        {
            _set = set;
            _graph = graph;
        }

        internal static DependencyGraph Build(IReadOnlyParameterSet set)
        {

            Dictionary<ParameterRef, Dependency> lookup = set.Parameters

                // choose only non primitive value parameters - will be replaced by a constant dependency
                .ToDictionary(p => p.Key, p => (Dependency) new SimpleDependency(p.Value));

            AdjacencyGraph<Dependency, Edge<Dependency>> bigGraph = new AdjacencyGraph<Dependency, Edge<Dependency>>();

            foreach (var key in lookup)
            {
                bigGraph.AddVertex(key.Value);
            }

            foreach (var p in set.Parameters.Values)
            {
                Dependency src = lookup[p.Reference];
                // the parameter can depend on other parameters => normal behaviour
                foreach (ParentChildParameterAccessor parentChild in p.Accessors.OfType<ParentChildParameterAccessor>())
                {
                    Dependency trg = lookup[parentChild.ParentRef];
                    bigGraph.AddEdge(new Edge<Dependency>(src, trg));
                }
            }

            DependencyGraph dependencyGraph = new DependencyGraph(set, bigGraph.Condensate<Dependency, AdjacencyGraph<Dependency, Edge<Dependency>>>(static (cyclicDependencies) => new ComplexDependency(cyclicDependencies)));
            return dependencyGraph;
        }

        internal IEnumerable<Dependency> GetRootDependencies()
        {
            return _graph.Roots();
        }

        internal IEnumerable<Dependency> GetDependencies()
        {
            return _graph.Vertices;
        }

        internal IEnumerable<Dependency> GetSortedDependencies()
        {
            return _graph.TopologicalSort();
        }
    }
}
