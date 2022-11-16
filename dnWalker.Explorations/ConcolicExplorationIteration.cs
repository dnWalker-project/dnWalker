using dnlib.DotNet;

using dnWalker.Symbolic;

using System;

namespace dnWalker.Explorations
{
    public partial class ConcolicExplorationIteration
    {
        private ConcolicExplorationIteration(int iterationNumber,
                                             ConcolicExploration exploration,
                                             IReadOnlyModel inputModel,
                                             IReadOnlyModel outputModel,
                                             DateTime start,
                                             DateTime end,
                                             string pathConstraint,
                                             TypeSig? exception, 
                                             string standardOutput, 
                                             string errorOutput)
        {
            IterationNumber = iterationNumber;
            Exploration = exploration ?? throw new ArgumentNullException(nameof(exploration));
            InputModel = inputModel ?? throw new ArgumentNullException(nameof(inputModel));
            OutputModel = outputModel ?? throw new ArgumentNullException(nameof(outputModel));
            Start = start;
            End = end;
            PathConstraint = pathConstraint ?? throw new ArgumentNullException(nameof(pathConstraint));
            Exception = exception;
            StandardOutput = standardOutput ?? throw new ArgumentNullException(nameof(standardOutput));
            ErrorOutput = errorOutput ?? throw new ArgumentNullException(nameof(errorOutput));
        }

        public int IterationNumber { get; }
        public ConcolicExploration Exploration { get; }
        public IReadOnlyModel InputModel { get; }
        public IReadOnlyModel OutputModel { get; }
        public DateTime Start { get; }
        public DateTime End { get; }

        public TypeSig? Exception { get; }
        public string StandardOutput { get; }
        public string ErrorOutput { get; }
        public string PathConstraint { get; }


    }
}
