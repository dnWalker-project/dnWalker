using dnlib.DotNet;

using dnWalker.Concolic.Traversal;
using dnWalker.Graphs.ControlFlow;
using dnWalker.Symbolic;
using dnWalker.Traversal;

using MMC.State;
using MMC.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic
{
    public class ExplorationResult
    {
        private readonly MethodDef _entryPoint;
        private readonly Coverage _coverage;
        private readonly IReadOnlyList<ExplorationIterationResult> _iterations;
        private readonly ControlFlowGraph _controlFlowGraph;
        private readonly IReadOnlyList<ConstraintTree> _constraintTrees;

        internal ExplorationResult(MethodDef entryPoint, 
            IReadOnlyList<ExplorationIterationResult> iterations, 
            IReadOnlyList<ConstraintTree> constraintTrees, 
            ControlFlowGraph controlFlowGraph)
        {
            _iterations = iterations ?? throw new ArgumentNullException(nameof(iterations));
            _constraintTrees = constraintTrees ?? throw new ArgumentNullException(nameof(constraintTrees));
            _controlFlowGraph = controlFlowGraph ?? throw new ArgumentNullException(nameof(controlFlowGraph));
            _entryPoint = entryPoint ?? throw new ArgumentNullException(nameof(entryPoint));
            _coverage = controlFlowGraph.GetCoverage();
        }


        public Coverage Coverage => _coverage;
        public IReadOnlyList<ExplorationIterationResult> Iterations => _iterations;
        public IReadOnlyList<ConstraintTree> ConstraintTrees => _constraintTrees;
        public ControlFlowGraph ControlFlowGraph => _controlFlowGraph;
        public MethodDef EntryPoint => _entryPoint;
    }


    public class ExplorationIterationResult
    {
        private readonly ExceptionInfo _exception;
        private readonly string _stackTrace;
        private readonly string _output;
        private readonly IReadOnlyList<Segment> _segments;
        private readonly int _iterationNumber;
        private readonly SymbolicContext _symbolicContext;
        private readonly Constraint _precondition;
        private readonly Constraint _postcondition;
        private readonly IReadOnlyList<CILLocation> _visitedNodes;

        internal ExplorationIterationResult(int iterationNumber, Path path, SymbolicContext symbolicContext, Constraint precondition, Constraint postcondition)
        {
            _iterationNumber = iterationNumber;

            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            _exception = path.Exception;
            _stackTrace = path.StackTrace;
            _output = path.Output ?? string.Empty;
            _segments = path.Segments.ToArray();
            _visitedNodes = path.VisitedNodes.ToArray();


            _symbolicContext = symbolicContext ?? throw new ArgumentNullException(nameof(symbolicContext));
            _precondition = precondition ?? throw new ArgumentNullException(nameof(precondition));
            _postcondition = postcondition ?? throw new ArgumentNullException(nameof(postcondition));
        }

        public ExceptionInfo Exception => _exception;
        public string StackTrace => _stackTrace;

        public string Output => _output;

        public IReadOnlyList<Segment> Segments => _segments;

        public SymbolicContext SymbolicContext => _symbolicContext;

        public Constraint Precondition => _precondition;

        public Constraint PostCondition => _postcondition;

        public int IterationNumber => _iterationNumber;

        public IReadOnlyList<CILLocation> VisitedNodes => _visitedNodes;

        public string GetPathInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Input:");
            sb.AppendLine("Output:");
            sb.AppendLine(Output);
            sb.AppendLine("Visited nodes:");
            foreach (var node in _visitedNodes)
            {
                sb.AppendLine(" - " + node.ToString());
            }
            return sb.ToString();
        }
    }
}
