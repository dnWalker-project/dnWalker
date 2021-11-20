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
        public static ExplorationIterationData FromXml(XElement xml)
        {
            ExplorationIterationData iterationData = new ExplorationIterationData
                (
                    xml.Element(nameof(Parameters))?.ToParameterStore() ?? throw new Exception("Exploration iteration XML must have a 'Iteration/Parameters' element."),
                    int.Parse(xml.Attribute(nameof(IterationNumber))?.Value ?? throw new Exception("Exploration iteration XML must have a 'IterationNumber' attribute.")),
                    xml.Attribute(nameof(PathConstraint))?.Value ?? string.Empty,
                    null, // create exception from the XML if there is one
                    xml.Element(nameof(StandardOutput))?.Value ?? string.Empty,
                    xml.Element(nameof(ErrorOutput))?.Value ?? string.Empty
                );

            return iterationData;
        }

        internal ExplorationIterationData(ParameterStore parameters, int iterationNumber, string pathConstraint, Exception? exception, string stdandardOutput, string errorOutput)
        {
            Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            PathConstraint = pathConstraint;
            IterationNumber = iterationNumber;
            Exception = exception;
            StandardOutput = stdandardOutput ?? throw new ArgumentNullException(nameof(stdandardOutput));
            ErrorOutput = errorOutput ?? throw new ArgumentNullException(nameof(errorOutput));
        }

        public ParameterStore Parameters 
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
