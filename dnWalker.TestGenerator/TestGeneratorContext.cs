using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator
{
    public class TestGeneratorContext
    {
        public TestGeneratorContext(Assembly assembly, ExplorationData explorationData, ExplorationIterationData iterationData)
        {
            SUTAssembly = assembly;
            ExplorationData = explorationData;
            IterationData = iterationData;
        }

        public Assembly SUTAssembly 
        {
            get; 
        }

        public ExplorationData ExplorationData
        {
            get;
        }

        public ExplorationIterationData IterationData
        {
            get;
        }
    }
}
