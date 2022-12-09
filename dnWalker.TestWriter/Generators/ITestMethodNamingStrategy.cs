using dnlib.DotNet;

using dnWalker.Explorations;
using dnWalker.TestWriter.Generators.Schemas;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators
{
    public interface ITestMethodNamingStrategy
    {
        /// <summary>
        /// Produces name of the test method. Will not ensure distinct names.
        /// </summary>
        /// <param name="methodUnderTest"></param>
        /// <param name="testSchema"></param>
        /// <returns></returns>
        string GetMethodName(IMethod methodUnderTest, ITestSchema testSchema);
    }
}
