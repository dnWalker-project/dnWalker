using System;
using System.IO;

using dnlib.DotNet;

using dnWalker.TestGenerator.Symbols;

namespace dnWalker.TestGenerator.Templates
{
    public interface IAssertTemplate : ITemplate
    {
        void WriteAssertNull(IWriter output, string symbol);
        void WriteAssertNotNull(IWriter output, string symbol);

        void WriteAssertEqual(IWriter output, string leftSymbol, string rightSymbol);
        void WriteAssertNotEqual(IWriter output, string leftSymbol, string rightSymbol);

        void WriteAssertSame(IWriter output, string leftSymbol, string rightSymbol);
        void WriteAssertNotSame(IWriter output, string leftSymbol, string rightSymbol);

        void WriteAssertExceptionThrown(IWriter output, string delegateSymbol, TypeSig exceptionType);
        void WriteAssertExceptionNotThrown(IWriter output, string delegateSymbol, TypeSig exceptionType);
        void WriteAssertNoExceptionThrown(IWriter output, string delegateSymbol);

    }
}

