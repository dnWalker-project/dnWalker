using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators
{
    public interface ITestProjectNamingStrategy
    {
        string GetProjectName(IModule moduleUnderTest);
    }
}
