﻿using dnlib.DotNet;

using dnWalker.Symbolic;

using System;

namespace dnWalker.Explorations
{
    public partial class ConcolicExplorationIteration
    {
        public class Builder
        {

            public int IterationNumber { get; set; }
            public ConcolicExploration? Exploration { get; set; }
            public IReadOnlyModel? InputModel { get; set; }
            public IReadOnlyModel? OutputModel { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public string? PathConstraint { get; set; } = string.Empty;
            public TypeSig? Exception { get; set; }
            public string? StandardOutput { get; set; } = string.Empty;
            public string? ErrorOutput { get; set; } = string.Empty;
                
            public ConcolicExplorationIteration Build()
            {
                if (Exploration == null) throw new NullReferenceException("The Exploration is NULL");
                if (InputModel == null) throw new NullReferenceException("The BaseParameterSet is NULL");
                if (OutputModel == null) throw new NullReferenceException("The ExecutionParameterSet is NULL");
                if (PathConstraint == null) throw new NullReferenceException("The PathConstraint is NULL");
                if (StandardOutput == null) throw new NullReferenceException("The StandardOutput is NULL");
                if (ErrorOutput == null) throw new NullReferenceException("The ErrorOutput is NULL");

                return new ConcolicExplorationIteration(IterationNumber,
                                                        Exploration,
                                                        InputModel,
                                                        OutputModel,
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
