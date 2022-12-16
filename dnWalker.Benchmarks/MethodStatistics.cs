using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Benchmarks
{
    public record MethodStatistics
        (
            string Name, 
            double ExplorationDuration, 
            double ExplorationDurationDelta,
            double GenerationDuration,
            double GenerationDurationDelta,
            double OkPaths,
            double OkPathsDelta,
            double ErrorPaths,
            double ErrorPathsDelta,
            double DontKnowPaths,
            double DontKnowPathsDelta,
            double Tests,
            double TestsDelta,
            bool Passed = true)
    {
        public static MethodStatistics Failed(string name)
        {
            return new MethodStatistics(name, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false);
        }
    }
}
