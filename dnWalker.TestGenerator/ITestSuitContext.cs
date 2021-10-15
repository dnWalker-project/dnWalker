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
        void CreateProject(String directory, String name);

        void WriteAsTheory(String methodName, IReadOnlyList<dnWalker.Concolic.ExplorationIterationData> cases);
        void WriteAsFacts(String methodName, IReadOnlyList<dnWalker.Concolic.ExplorationIterationData> cases);
    }
}
