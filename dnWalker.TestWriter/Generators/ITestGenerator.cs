using dnWalker.Explorations;
using dnWalker.TestWriter.TestModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators
{
    /// <summary>
    /// Generates test classes from the concolic exploration.
    /// </summary>
    public interface ITestGenerator
    {
        IEnumerable<TestClass> GenerateTests(ConcolicExploration exploration);
    }
}
