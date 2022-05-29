using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;

using dnWalker.TestGenerator.Symbols;

using System;
using System.IO;
using System.Collections.Generic;

namespace dnWalker.TestGenerator.Templates
{
    public interface IArrangeTemplate
    {
        void WriteArrange(TextWriter output, IEnumerable<TemplateSymbol> symbols);
    }
}

