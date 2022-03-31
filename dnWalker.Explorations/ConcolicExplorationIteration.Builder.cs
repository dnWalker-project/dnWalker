﻿using dnWalker.Parameters;
using dnWalker.Parameters.Serialization;

using System;

namespace dnWalker.Explorations
{
    public partial class ConcolicExplorationIteration
    {
        public class Builder
        {

            public int IterationNumber { get; set; }
            public ConcolicExploration? Exploration { get; set; }
            public IParameterSetInfo? BaseParameterSet { get; set; }
            public IParameterSetInfo? ExecutionParameterSet { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public string? PathConstraint { get; set; }
            public string? Exception { get; set; }
            public string? StandardOutput { get; set; }
            public string? ErrorOutput { get; set; }

            public ConcolicExplorationIteration Build()
            {
                if (Exploration == null) throw new NullReferenceException("The Exploration is NULL");
                if (BaseParameterSet == null) throw new NullReferenceException("The BaseParameterSet is NULL");
                if (ExecutionParameterSet == null) throw new NullReferenceException("The ExecutionParameterSet is NULL");
                if (PathConstraint == null) throw new NullReferenceException("The PathConstraint is NULL");
                if (Exception == null) throw new NullReferenceException("The Exception is NULL");
                if (StandardOutput == null) throw new NullReferenceException("The StandardOutput is NULL");
                if (ErrorOutput == null) throw new NullReferenceException("The ErrorOutput is NULL");

                return new ConcolicExplorationIteration(IterationNumber,
                                                        Exploration,
                                                        BaseParameterSet,
                                                        ExecutionParameterSet,
                                                        Start,
                                                        End,
                                                        PathConstraint,
                                                        Exception,
                                                        StandardOutput,
                                                        ErrorOutput);
            }
        }
    }
}
