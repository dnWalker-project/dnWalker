using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator
{
    /// <summary>
    /// Represents a test suit context used to generate test project, test fixtures and test cases.
    /// </summary>
    public interface ITestSuitContext
    {
        /// <summary>
        /// Creates and sets up a new .csproj file.
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="directory"></param>
        void CreateProject(string projectName, string? directory = null);

        void WriteTest(CodeWriter codeWriter, TestData testData);
    }
}
