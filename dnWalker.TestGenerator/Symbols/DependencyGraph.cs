using System;
using System.Linq;
using System.Collections.Generic;

using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;

using QuikGraph;
using QuikGraph.Algorithms;
using QuikGraph.Utils;

using IVariable = dnWalker.Symbolic.IVariable;

namespace dnWalker.TestGenerator.Symbols
{
    public partial class DependencyGraph
    {
        private readonly List<TemplateSymbol> _symbols;
        private readonly BidirectionalGraph<List<TemplateSymbol>, Edge<List<TemplateSymbol>>> _graph;

        public DependencyGraph(IEnumerable<TemplateSymbol> symbols)
        {
            _symbols = new List<TemplateSymbol>(symbols);

            _graph = BuildGraph(_symbols);
        }


        public IEnumerable<List<TemplateSymbol>> GetRootSymbolGroups()
        {
            return _graph.Roots();
        }

        public IEnumerable<List<TemplateSymbol>> GetIndependentSymbolGroups()
        {
            return _graph.Vertices;
        }

        public IEnumerable<List<TemplateSymbol>> GetSortedSymbolGroups()
        {
            return _graph.TopologicalSort();
        }


    }
}

