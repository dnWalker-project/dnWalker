using dnlib.DotNet;

using dnWalker.Explorations;
using dnWalker.Symbolic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators
{
    /// <summary>
    /// Represents information about a single test.
    /// </summary>
    public interface ITestContext
    {
        ConcolicExplorationIteration Iteration { get; }

        IDictionary<Location, SymbolContext> LocationMapping { get; }
        IDictionary<string, SymbolContext> SymbolMapping { get; }
    }
}
