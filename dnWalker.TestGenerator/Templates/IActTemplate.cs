using System;
using System.IO;

using dnWalker.TestGenerator.Symbols;

using dnlib.DotNet;

namespace dnWalker.TestGenerator.Templates
{
    public interface IActTemplate
    {
        void WriteAct(TextWriter output, IMethod method, TemplateSymbol[] arguments);
    }
}

