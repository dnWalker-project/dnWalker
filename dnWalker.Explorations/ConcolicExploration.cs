using dnlib.DotNet;

using System;
using System.Collections.Generic;

namespace dnWalker.Explorations
{
    public partial class ConcolicExploration
    {
        private readonly List<ConcolicExplorationIteration> _iterations = new List<ConcolicExplorationIteration>();

        private ConcolicExploration(string assemblyName, string assemblyFileName, IMethod methodUnderTest, string solver, DateTime start, DateTime end, bool failed)
        {
            AssemblyName = assemblyName ?? throw new ArgumentNullException(nameof(assemblyName));
            AssemblyFileName = assemblyFileName ?? throw new ArgumentNullException(nameof(assemblyFileName));
            MethodUnderTest = methodUnderTest ?? throw new ArgumentNullException(nameof(methodUnderTest));
            Solver = solver ?? throw new ArgumentNullException(nameof(solver));
            Start = start;
            End = end;
            Failed = failed;
        }

        public IReadOnlyList<ConcolicExplorationIteration> Iterations 
        {
            get
            {
                return _iterations;
            }
        }

        public string AssemblyName { get; }
        public string AssemblyFileName { get; }
        public IMethod MethodUnderTest { get; }
        public string Solver { get; }
        public DateTime Start { get; }
        public DateTime End { get; }
        public bool Failed { get; }
    }
}
