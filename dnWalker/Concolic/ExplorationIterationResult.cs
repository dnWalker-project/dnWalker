using dnlib.DotNet;

using dnWalker.Explorations;
using dnWalker.Symbolic;
using dnWalker.Traversal;

using MMC;
using MMC.State;
using MMC.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dnWalker.Concolic
{
    public class ExplorationIterationResult
    {
        private readonly DateTime _start;
        private readonly DateTime _end;

        private readonly ExceptionInfo _exception;
        private readonly string _stackTrace;
        private readonly string _output;
        private readonly IReadOnlyList<Segment> _segments;
        private readonly int _iterationNumber;
        private readonly SymbolicContext _symbolicContext;
        private readonly Constraint _precondition;
        private readonly Constraint _postcondition;
        private readonly IReadOnlyList<CILLocation> _visitedNodes;
        private readonly IReadOnlyStatistics _statistics;

        internal ExplorationIterationResult(int iterationNumber, Path path, SymbolicContext symbolicContext, Constraint precondition, Constraint postcondition, IReadOnlyStatistics statistics, DateTime start, DateTime end)
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
            _statistics = statistics ?? throw new ArgumentNullException(nameof(statistics));
            _start = start;
            _end = end;
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

        public IReadOnlyStatistics Statistics => _statistics;

        public DateTime Start => _start;

        public DateTime End => _end;

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

        public ConcolicExplorationIteration.Builder ToExplorationData()
        {
            ConcolicExplorationIteration.Builder builder = new ConcolicExplorationIteration.Builder()
            {
                StandardOutput = _output,
                ErrorOutput = string.Empty,
                Exception = _exception?.Type.ToTypeSig(),
                InputModel = _symbolicContext.InputModel,
                OutputModel = _symbolicContext.OutputModel,
                IterationNumber = _iterationNumber,
                PathConstraint = _postcondition.ToString(),
                Start = _start,
                End = _end,
            };


            return builder;
        }
    }
}
