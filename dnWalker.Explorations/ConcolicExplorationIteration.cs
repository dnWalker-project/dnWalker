using dnWalker.Parameters;
using dnWalker.Parameters.Serialization;

using System;

namespace dnWalker.Explorations
{
    public partial class ConcolicExplorationIteration
    {
        private ConcolicExplorationIteration(int iterationNumber, ConcolicExploration exploration, IParameterSetInfo baseParameterSet, IParameterSetInfo executionParameterSet, DateTime start, DateTime end, string pathConstraint)
        {
            IterationNumber = iterationNumber;
            Exploration = exploration ?? throw new ArgumentNullException(nameof(exploration));
            BaseParameterSet = baseParameterSet ?? throw new ArgumentNullException(nameof(baseParameterSet));
            ExecutionParameterSet = executionParameterSet ?? throw new ArgumentNullException(nameof(executionParameterSet));
            Start = start;
            End = end;
            PathConstraint = pathConstraint ?? throw new ArgumentNullException(nameof(pathConstraint));
        }

        public int IterationNumber { get; }
        public ConcolicExploration Exploration { get; }
        public IParameterSetInfo BaseParameterSet { get; }
        public IParameterSetInfo ExecutionParameterSet { get; }
        public DateTime Start { get; }
        public DateTime End { get; }

        public string PathConstraint { get; }


    }
}
