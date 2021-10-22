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
        void CreateProject(string directory, string name);

        void WriteAsTheory(string methodName, IReadOnlyList<dnWalker.Concolic.ExplorationIterationData> cases);
        void WriteAsFacts(string methodName, IReadOnlyList<dnWalker.Concolic.ExplorationIterationData> cases);
    }
}
