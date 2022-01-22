using dnWalker.Parameters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Parameters
{
    public class Dependency
    {
        public Dependency(ParameterRef reference)
        {
            Reference = reference;
        }

        public ParameterRef Reference { get; }
    }

    public class Relation : QuikGraph.Edge<Dependency>
    {

        public Relation(Dependency parent, Dependency child, ParameterAccessor accessor) : base(parent, child)
        {
            Accessor = accessor;
        }
        public ParameterAccessor Accessor { get; }
    }

    public class DependencyGraph : QuikGraph.AdjacencyGraph<Dependency, Relation>
    {
        private readonly IReadOnlyParameterContext _context;

        private DependencyGraph(IReadOnlyParameterContext context) 
        {
            _context = context;
        }

        public static DependencyGraph Build(IReadOnlyParameterContext context)
        {
            Dictionary<ParameterRef, Dependency> _lookup = context.Parameters.Keys.ToDictionary(r => r, r => new Dependency(r));

            DependencyGraph graph = new DependencyGraph(context);
            foreach (var key in _lookup)
            {
                graph.AddVertex(key.Value);
            }

            foreach (var p in context.Parameters.Values)
            {
                foreach (var accessor in p.Accessors)
            }

            return graph;
        }
    }
}
