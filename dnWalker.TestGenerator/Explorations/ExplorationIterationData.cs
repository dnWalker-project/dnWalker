using dnWalker.Parameters;
using dnWalker.Parameters.Xml;

using System;
using System.Xml.Linq;

namespace dnWalker.TestGenerator
{
    /// <summary>
    /// Represents data from a single exploration iteration.
    /// </summary>
    public class ExplorationIterationData
    {
        internal ExplorationIterationData(IParameterSet startingParameterContext, IParameterSet endingParameterContext, int iterationNumber, string pathConstraint, Exception? exception, string stdandardOutput, string errorOutput)
        {
            StartingParameterContext = startingParameterContext ?? throw new ArgumentNullException(nameof(startingParameterContext));
            EndingParameterContext = endingParameterContext ?? throw new ArgumentNullException(nameof(endingParameterContext));
            PathConstraint = pathConstraint;
            Number = iterationNumber;
            Exception = exception;
            StandardOutput = stdandardOutput ?? throw new ArgumentNullException(nameof(stdandardOutput));
            ErrorOutput = errorOutput ?? throw new ArgumentNullException(nameof(errorOutput));
        }

        public IParameterSet StartingParameterContext
        {
            get;
        }

        public IParameterSet EndingParameterContext
        {
            get;
        }

        public int Number
        {
            get;
        }

        public Exception? Exception
        {
            get;
        }

        public string StandardOutput
        {
            get;
        }

        public string ErrorOutput
        {
            get;
        }

        public string PathConstraint 
        {
            get;
        }
    }
}
