using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators
{
    public class BasicTestProjectNamingStrategy : ITestProjectNamingStrategy
    {
        public string GetProjectName(IModule moduleUnderTest)
        {
            string moduleName = moduleUnderTest.Name;

            // remove the suffix
            if (moduleName.EndsWith(".exe") || moduleName.EndsWith(".dll"))
            {
                moduleName = moduleName.Substring(0, moduleName.Length - 4);
            }

            return $"{moduleName}.Tests";
        }
    }
}
