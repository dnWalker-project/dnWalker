using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators.Assert
{
    public interface IAssertPrimitives : IPrimitives
    {
        bool TryWriteAssertNull(ITestContext context, IWriter output, string symbol);
        bool TryWriteAssertNotNull(ITestContext context, IWriter output, string symbol);

        bool TryWriteAssertEqual(ITestContext context, IWriter output, string actual, string expected);
        bool TryWriteAssertNotEqual(ITestContext context, IWriter output, string actual, string expected);

        bool TryWriteAssertSame(ITestContext context, IWriter output, string actual, string expected);
        bool TryWriteAssertNotSame(ITestContext context, IWriter output, string actual, string expected);

        public bool TryWriteAssertLength(ITestContext context, IWriter output, string arraySymbol, string expected)
        {
            string arrayLengthSymbol = $"{arraySymbol}.Length";
            return TryWriteAssertEqual(context, output, arrayLengthSymbol, expected);
        }
        public bool TryWriteAssertCount(ITestContext context, IWriter output, string collectionSymbol, string expected)
        {
            string arrayLengthSymbol = $"{collectionSymbol}.Count()";
            return TryWriteAssertEqual(context, output, arrayLengthSymbol, expected);
        }

        bool TryWriteAssertExceptionThrown(ITestContext context, IWriter output, string delegateSymbol, string exceptionType);
        bool TryWriteAssertExceptionNotThrown(ITestContext context, IWriter output, string delegateSymbol, string exceptionType);
        bool TryWriteAssertNoExceptionThrown(ITestContext context, IWriter output, string delegateSymbol);

        bool TryWriteAssertOfType(ITestContext context, IWriter output, string objectSymbol, string expectedType);
        bool TryWriteAssertNotOfType(ITestContext context, IWriter output, string objectSymbol, string expectedType);

        bool TryWriteAssertEquivalent(ITestContext context, IWriter output, string objectSymbol, string expetedSymbol);
        bool TryWriteAssertNotEquivalent(ITestContext context, IWriter output, string objectSymbol, string expetedSymbol);
    }
}
