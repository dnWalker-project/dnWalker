using dnWalker.TestWriter.Generators;
using dnWalker.TestWriter.Generators.Assert;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Xunit
{
    public class XunitAssertions : IAssertPrimitives
    {
        public bool TryWriteAssertNull(ITestContext context, IWriter output, string symbol)
        {
            output.WriteLine($"Assert.Null({symbol});");
            return true;
        }

        public bool TryWriteAssertNotNull(ITestContext context, IWriter output, string symbol)
        {
            output.WriteLine($"Assert.NotNull({symbol});");
            return true;
        }

        public bool TryWriteAssertEqual(ITestContext context, IWriter output, string actual, string expected)
        {
            output.WriteLine($"Assert.Equal({expected}, {actual});");
            return true;
        }

        public bool TryWriteAssertNotEqual(ITestContext context, IWriter output, string actual, string expected)
        {
            output.WriteLine($"Assert.NotEqual({expected}, {actual});");
            return true;
        }

        public bool TryWriteAssertSame(ITestContext context, IWriter output, string actual, string expected)
        {
            output.WriteLine($"Assert.Same({expected}, {actual});");
            return true;
        }

        public bool TryWriteAssertNotSame(ITestContext context, IWriter output, string actual, string expected)
        {
            output.WriteLine($"Assert.NotSame({expected}, {actual});");
            return true;
        }

        public bool TryWriteAssertExceptionThrown(ITestContext context, IWriter output, string delegateSymbol, string exceptionType)
        {
            output.WriteLine($"Assert.Throws<{exceptionType}>({delegateSymbol});");
            return true;
        }

        public bool TryWriteAssertExceptionNotThrown(ITestContext context, IWriter output, string delegateSymbol, string exceptionType)
        {
            output.WriteLine($"Assert.IsNotType<{exceptionType}>(Record.Exception({delegateSymbol}));");
            return true;
        }

        public bool TryWriteAssertNoExceptionThrown(ITestContext context, IWriter output, string delegateSymbol)
        {
            output.WriteLine($"Assert.Null(Record.Exception({delegateSymbol}));");
            return true;
        }

        public bool TryWriteAssertOfType(ITestContext context, IWriter output, string objectSymbol, string expectedType)
        {
            output.WriteLine($"Assert.IsType<{expectedType}>({objectSymbol});");
            return true;
        }

        public bool TryWriteAssertNotOfType(ITestContext context, IWriter output, string objectSymbol, string expectedType)
        {
            output.WriteLine($"Assert.IsNotType<{expectedType}>({objectSymbol});");
            return true;
        }

        public bool TryWriteAssertEquivalent(ITestContext context, IWriter output, string objectSymbol, string expetedSymbol)
        {
            return false;
        }

        public bool TryWriteAssertNotEquivalent(ITestContext context, IWriter output, string objectSymbol, string expetedSymbol)
        {
            return false;
        }

        private static readonly string[] _ns = new[] { "Xunit" };

        public IEnumerable<string> Namespaces => _ns;
    }
}
