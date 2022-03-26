using dnWalker.Parameters;
using dnWalker.Parameters.Serialization;

using System;

namespace dnWalker.Explorations
{
    public partial class ConcolicExplorationIteration
    {
        private ConcolicExplorationIteration(int iterationNumber,
                                             ConcolicExploration exploration,
                                             IParameterSetInfo baseParameterSet,
                                             IParameterSetInfo executionParameterSet,
                                             DateTime start,
                                             DateTime end,
                                             string pathConstraint, 
                                             string exception, 
                                             string standardOutput, 
                                             string errorOutput)
        {
            IterationNumber = iterationNumber;
            Exploration = exploration ?? throw new ArgumentNullException(nameof(exploration));
            BaseParameterSet = baseParameterSet ?? throw new ArgumentNullException(nameof(baseParameterSet));
            ExecutionParameterSet = executionParameterSet ?? throw new ArgumentNullException(nameof(executionParameterSet));
            Start = start;
            End = end;
            PathConstraint = pathConstraint ?? throw new ArgumentNullException(nameof(pathConstraint));
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
            StandardOutput = standardOutput ?? throw new ArgumentNullException(nameof(standardOutput));
            ErrorOutput = errorOutput ?? throw new ArgumentNullException(nameof(errorOutput));
        }

        public int IterationNumber { get; }
        public ConcolicExploration Exploration { get; }
        public IParameterSetInfo BaseParameterSet { get; }
        public IParameterSetInfo ExecutionParameterSet { get; }
        public DateTime Start { get; }
        public DateTime End { get; }

        public string Exception { get; }
        public string StandardOutput { get; }
        public string ErrorOutput { get; }
        public string PathConstraint { get; }


    }
}
