using System;
using System.IO;

using dnlib.DotNet;

namespace dnWalker.TestGenerator.Templates
{
    public interface IActTemplate : ITemplate
    {
        void WriteAct(IWriter output, IMethod method, string? returnSymbol = null);
        void WriteActDelegate(IWriter output, IMethod method, string? returnSymbol = null, string delegateSymbol = "act");
    }
}

