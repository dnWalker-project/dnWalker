using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators.Assert
{
    internal class SimpleAssertPrimitives : IAssertPrimitives
    {
        public bool TryWriteAssertNull(ITestContext context, IWriter output, string symbol)
        {
            output.WriteLine($"System.Diagnostics.Debug.Assert({symbol} == null)");
            return true;
        }

        public bool TryWriteAssertNotNull(ITestContext context, IWriter output, string symbol)
        {
            output.WriteLine($"System.Diagnostics.Debug.Assert({symbol} != null)");
            return true;
        }

        public bool TryWriteAssertEqual(ITestContext context, IWriter output, string actual, string expected)
        {
            output.WriteLine($"System.Diagnostics.Debug.Assert({actual} == {expected})");
            return true;
        }

        public bool TryWriteAssertNotEqual(ITestContext context, IWriter output, string actual, string expected)
        {
            output.WriteLine($"System.Diagnostics.Debug.Assert({actual} != {expected})");
            return true;
        }

        public bool TryWriteAssertSame(ITestContext context, IWriter output, string actual, string expected)
        {
            output.WriteLine($"System.Diagnostics.Debug.Assert(ReferenceEquals({actual}, {expected}))");
            return true;
        }

        public bool TryWriteAssertNotSame(ITestContext context, IWriter output, string actual, string expected)
        {
            output.WriteLine($"System.Diagnostics.Debug.Assert(!ReferenceEquals({actual}, {expected}))");
            return true;
        }

        public bool TryWriteAssertExceptionThrown(ITestContext context, IWriter output, string delegateSymbol, string exceptionType)
        {
            output.WriteLine("try");
            output.WriteLine("{");
            output.Indent++;
            output.WriteLine($"{delegateSymbol}()");
            output.WriteLine($"System.Diagnostics.Debug.Fail()");
            output.Indent--;
            output.WriteLine("}");
            output.WriteLine("catch (Exception e)");
            output.WriteLine("{");
            output.Indent++;
            output.WriteLine($"System.Diagnostics.Debug.Assert(e is {exceptionType})");
            output.Indent--;
            output.WriteLine("}");

            return true;
        }

        public bool TryWriteAssertExceptionNotThrown(ITestContext context, IWriter output, string delegateSymbol, string exceptionType)
        {
            output.WriteLine("try");
            output.WriteLine("{");
            output.Indent++;
            output.WriteLine($"{delegateSymbol}()");
            output.WriteLine($"System.Diagnostics.Debug.Fail()");
            output.Indent--;
            output.WriteLine("}");
            output.WriteLine("catch (Exception e)");
            output.WriteLine("{");
            output.Indent++;
            output.WriteLine($"System.Diagnostics.Debug.Assert(e is not {exceptionType})");
            output.Indent--;
            output.WriteLine("}");

            return true;
        }

        public bool TryWriteAssertNoExceptionThrown(ITestContext context, IWriter output, string delegateSymbol)
        {
            output.WriteLine("try");
            output.WriteLine("{");
            output.Indent++;
            output.WriteLine($"{delegateSymbol}()");
            output.Indent--;
            output.WriteLine("}");
            output.WriteLine("catch");
            output.WriteLine("{");
            output.Indent++;
            output.WriteLine($"System.Diagnostics.Debug.Fail()");
            output.Indent--;
            output.WriteLine("}");

            return true;
        }

        public bool TryWriteAssertOfType(ITestContext context, IWriter output, string objectSymbol, string expectedType)
        {
            output.WriteLine($"System.Diagnostics.Debug.Assert({objectSymbol} is {expectedType})");
            return true;
        }

        public bool TryWriteAssertNotOfType(ITestContext context, IWriter output, string objectSymbol, string expectedType)
        {
            output.WriteLine($"System.Diagnostics.Debug.Assert({objectSymbol} is not {expectedType})");
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
    }
}
