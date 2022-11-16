using dnWalker.Explorations;
using dnWalker.Symbolic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators
{
    internal class TestContext : ITestContext
    {
        private readonly ConcolicExplorationIteration _iteration;
        private readonly IDictionary<Location, SymbolContext> _locationMapping = new Dictionary<Location, SymbolContext>();
        private readonly IDictionary<string, SymbolContext> _symbolMapping = new Dictionary<string, SymbolContext>();

        public TestContext(ConcolicExplorationIteration iteration)
        {
            _iteration = iteration;
        }

        public IDictionary<string, SymbolContext> SymbolMapping
        {
            get
            {
                return _symbolMapping;
            }
        }

        public IDictionary<Location, SymbolContext> LocationMapping
        {
            get
            {
                return _locationMapping;
            }
        }

        public ConcolicExplorationIteration Iteration
        {
            get
            {
                return _iteration;
            }
        }
    }
}
