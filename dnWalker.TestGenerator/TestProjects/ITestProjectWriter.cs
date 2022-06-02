using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.TestProjects
{
    /// <summary>
    /// Provides methods for writing a project file.
    /// </summary>
    public interface ITestProjectWriter
    {
        void WriteProject(TextWriter output, ITestProjectContext context);
    }
}
