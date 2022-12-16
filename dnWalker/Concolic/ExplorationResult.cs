using dnlib.DotNet;

using dnWalker.Concolic.Traversal;
using dnWalker.Explorations;
using dnWalker.Graphs.ControlFlow;
using dnWalker.Symbolic;
using dnWalker.Traversal;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dnWalker.Concolic
{
    public class ExplorationResult
    {
        private readonly DateTime _start;
        private readonly DateTime _end;

        private readonly MethodDef _entryPoint;
        private readonly Coverage _coverage;
        private readonly IReadOnlyList<ExplorationIterationResult> _iterations;
        private readonly MethodTracer _methodTracer;
        private readonly ISolver _solver;
        private readonly IExplorationStrategy _strategy;
        private readonly IReadOnlyList<ConstraintTree> _constraintTrees;

        internal ExplorationResult(MethodDef entryPoint,
            IReadOnlyList<ExplorationIterationResult> iterations,
            IReadOnlyList<ConstraintTree> constraintTrees,
            MethodTracer methodTracer,
            ISolver solver,
            IExplorationStrategy strategy,
            DateTime start,
            DateTime end)
        {
            _iterations = iterations ?? throw new ArgumentNullException(nameof(iterations));
            _constraintTrees = constraintTrees ?? throw new ArgumentNullException(nameof(constraintTrees));
            _methodTracer = methodTracer ?? throw new ArgumentNullException(nameof(methodTracer));
            _solver = solver;
            _strategy = strategy;
            _entryPoint = entryPoint ?? throw new ArgumentNullException(nameof(entryPoint));
            _coverage = methodTracer.GetCoverage();
            _start = start;
            _end = end;
        }


        public Coverage Coverage => _coverage;
        public IReadOnlyList<ExplorationIterationResult> Iterations => _iterations;
        public IReadOnlyList<ConstraintTree> ConstraintTrees => _constraintTrees;
        public ControlFlowGraph ControlFlowGraph => _methodTracer.Graph;
        public MethodTracer MethodTracer => _methodTracer;
        public MethodDef EntryPoint => _entryPoint;

        public DateTime Start => _start;

        public DateTime End => _end;

        public ISolver Solver => _solver;

        public IExplorationStrategy Strategy => _strategy;

        public ConcolicExploration.Builder ToExplorationData()
        {
            ConcolicExploration.Builder builder = new ConcolicExploration.Builder()
            {
                AssemblyFileName = _entryPoint.Module.Location,
                AssemblyName = _entryPoint.Module.Name,
                MethodUnderTest = _entryPoint,
                Solver = _solver.GetType().Name,
                Strategy = _strategy.GetType().Name,
                Start = _start,
                End = _end
            };

            builder.Iterations.AddRange(_iterations.Select(it => it.ToExplorationData()));
            return builder;
        }
    }
}
