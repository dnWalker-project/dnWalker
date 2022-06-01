using System;
using System.IO;

using dnWalker.TestGenerator.Symbols;

using dnlib.DotNet;

namespace dnWalker.TestGenerator.Templates
{
    public interface IActTemplate : ITemplate
    {
        void WriteAct(IWriter output, IMethod method, string[] argumentSymbols, string? returnSymbol = null);
        void WriteActDelegate(IWriter output, IMethod method, string[] argumentSymbols, string? returnSymbol = null, string delegateSymbol = "act");
    }
}

