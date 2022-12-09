using dnlib.DotNet;

using dnWalker.Explorations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators
{
    public interface ITestClassNamingStrategy
    {
        string GetClassName(IMethod methodUnderTest);

        string GetNamespaceName(IMethod methodUnderTest);
    }
}
