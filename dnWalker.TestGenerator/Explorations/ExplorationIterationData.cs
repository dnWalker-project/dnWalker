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
        internal ExplorationIterationData(IParameterContext parameterContext, int iterationNumber, string pathConstraint, Exception? exception, string stdandardOutput, string errorOutput)
        {
            ParameterContext = parameterContext ?? throw new ArgumentNullException(nameof(parameterContext));
            PathConstraint = pathConstraint;
            Number = iterationNumber;
            Exception = exception;
            StandardOutput = stdandardOutput ?? throw new ArgumentNullException(nameof(stdandardOutput));
            ErrorOutput = errorOutput ?? throw new ArgumentNullException(nameof(errorOutput));
        }

        public IParameterContext ParameterContext
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
