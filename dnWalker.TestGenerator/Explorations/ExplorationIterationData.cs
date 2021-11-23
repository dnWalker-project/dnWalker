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
        internal ExplorationIterationData(ParameterStore parameterStore, int iterationNumber, string pathConstraint, Exception? exception, string stdandardOutput, string errorOutput)
        {
            ParameterStore = parameterStore ?? throw new ArgumentNullException(nameof(parameterStore));
            PathConstraint = pathConstraint;
            IterationNumber = iterationNumber;
            Exception = exception;
            StandardOutput = stdandardOutput ?? throw new ArgumentNullException(nameof(stdandardOutput));
            ErrorOutput = errorOutput ?? throw new ArgumentNullException(nameof(errorOutput));
        }

        public ParameterStore ParameterStore 
        {
            get;
        }

        public int IterationNumber
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
