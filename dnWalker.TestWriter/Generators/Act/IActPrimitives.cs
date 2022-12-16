using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators.Act
{
    /// <summary>
    /// Writes the act part of the unit test.
    /// </summary>
    public interface IActPrimitives : IPrimitives
    {
        bool TryWriteAct(ITestContext context, IWriter output, string? returnSymbol = null);
        bool TryWriteActDelegate(ITestContext context, IWriter output, string? returnSymbol = null, string delegateSymbol = "act");
    }
}
