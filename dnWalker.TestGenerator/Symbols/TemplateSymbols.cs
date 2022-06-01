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

using SymbolGroup = System.Collections.Generic.List<dnWalker.TestGenerator.Symbols.TemplateSymbol>;
using SymbolEdge = QuikGraph.Edge<System.Collections.Generic.List<dnWalker.TestGenerator.Symbols.TemplateSymbol>>;
using SymbolGraph = QuikGraph.BidirectionalGraph<System.Collections.Generic.List<dnWalker.TestGenerator.Symbols.TemplateSymbol>, QuikGraph.Edge<System.Collections.Generic.List<dnWalker.TestGenerator.Symbols.TemplateSymbol>>>;

namespace dnWalker.TestGenerator.Symbols
{
    public partial class TemplateSymbols
    {
        private readonly TemplateSymbol[] _symbols;
        private readonly SymbolGraph _graph;
        private readonly Dictionary<Location, TemplateSymbol> _valueMapping;

        private TemplateSymbols(SymbolGraph symbolGraph)
        {
            _graph = symbolGraph;
            _symbols = symbolGraph.Vertices.SelectMany(static v => v).ToArray();
            _valueMapping = _symbols
                .Where(s => s.Value is Location)
                .ToDictionary(s => (Location)s.Value);
        }


        public IEnumerable<SymbolGroup> GetRootSymbolGroups()
        {
            return _graph.Roots();
        }

        public IEnumerable<SymbolGroup> GetIndependentSymbolGroups()
        {
            return _graph.Vertices;
        }

        public IEnumerable<SymbolGroup> GetSortedSymbolGroups()
        {
            return _graph.TopologicalSort();
        }

        public TemplateSymbol GetSymbol(Location location)
        {
            return _valueMapping[location];
        }
    }
}

