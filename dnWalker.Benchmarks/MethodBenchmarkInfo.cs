using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Benchmarks
{
    public class MethodBenchmarkInfo
    {
        public static MethodBenchmarkInfo Failed(string methodName) => new MethodBenchmarkInfo(methodName, Array.Empty<TimeSpan>(), Array.Empty<TimeSpan>(), 0, 0, false);

        private readonly TimeSpan[] _explorationDurations;
        private readonly TimeSpan[] _testGenDurations;
        private readonly string _methodName;
        private readonly int _iterationCount;
        private readonly int _testCount;
        private readonly bool _passed;

        public MethodBenchmarkInfo(string methodName, IEnumerable<TimeSpan> explDurations, IEnumerable<TimeSpan> testGenDurations, int iterationCount, int testCound, bool passed = true)
        {
            _explorationDurations = explDurations.ToArray();
            _testGenDurations = testGenDurations.ToArray();
            _methodName = methodName;
            _iterationCount = iterationCount;
            _testCount = testCound;
            _passed = passed;
        }

        public IReadOnlyList<TimeSpan> ExplorationDurations => _explorationDurations;
        public IReadOnlyList<TimeSpan> TestGenerationDurations => _testGenDurations;

        public int IterationCount => _iterationCount;

        public int TestCount => _testCount;

        public string MethodName => _methodName;

        public bool Passed => _passed;
    }
}
