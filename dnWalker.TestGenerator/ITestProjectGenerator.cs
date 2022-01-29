using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator
{
    /// <summary>
    /// Provides methods for generating a project directory and file.
    /// </summary>
    public interface ITestProjectGenerator
    {
        void GenerateProject(ITestProjectContext context);
    }
}
