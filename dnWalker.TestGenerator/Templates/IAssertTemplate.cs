using System;
using System.IO;

using dnlib.DotNet;

namespace dnWalker.TestGenerator.Templates
{
    public interface IAssertTemplate : ITemplate
    {
        void WriteAssertNull(IWriter output, string symbol);
        void WriteAssertNotNull(IWriter output, string symbol);

        void WriteAssertEqual(IWriter output, string actual, string expected);
        void WriteAssertNotEqual(IWriter output, string actual, string expected);

        void WriteAssertSame(IWriter output, string actual, string expected);
        void WriteAssertNotSame(IWriter output, string actual, string expected);

        void WriteAssertExceptionThrown(IWriter output, string delegateSymbol, TypeSig exceptionType);
        void WriteAssertExceptionNotThrown(IWriter output, string delegateSymbol, TypeSig exceptionType);
        void WriteAssertNoExceptionThrown(IWriter output, string delegateSymbol);

    }
}

