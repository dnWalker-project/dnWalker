using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;

using dnWalker.TestGenerator.Symbols;

using System;
using System.IO;
using System.Collections.Generic;
using dnWalker.Symbolic.Variables;
using System.Linq;
using dnlib.DotNet;
using System.Diagnostics;

namespace dnWalker.TestGenerator.Templates
{
    public interface IArrangeTemplate : ITemplate
    {
        /// <summary>
        /// Writes code which arranges the provided model.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="model"></param>
        /// <returns>A mapping from heap location to a variable name.</returns>
        IDictionary<Location, string> WriteArrange(IWriter output, IReadOnlyModel model);
    }
}

