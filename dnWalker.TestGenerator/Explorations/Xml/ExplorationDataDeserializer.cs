using dnWalker.Parameters.Xml;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dnWalker.TestGenerator.Explorations.Xml
{
    public static class ExplorationDataDeserializer
    {
        public static ExplorationData ToExplorationData(this XElement xml)
        {
            ExplorationData explorationData = new ExplorationData
                (
                    xml.Elements("Iteration").Select(xe => xe.ToExplorationIterationData()).ToArray(),
                    xml.Attribute(nameof(ExplorationData.AssemblyName))?.Value ?? throw new Exception("Exploration data XML must contain 'AssemblyName' attribute."),
                    xml.Attribute(nameof(ExplorationData.AssemblyFileName))?.Value ?? throw new Exception("Exploration data XML must contain 'AssemblyFileName' attribute."),
                    xml.Attribute(nameof(ExplorationData.MethodSignature))?.Value ?? throw new Exception("Exploration data XML must contain 'MethodSignature' attribute."),
                    bool.Parse(xml.Attribute(nameof(ExplorationData.IsStatic))?.Value ?? throw new Exception("Exploration data XML must contain 'IsStatic' attribute."))
                );

            return explorationData;
        }
        public static ExplorationIterationData ToExplorationIterationData(this XElement xml)
        {
            ExplorationIterationData iterationData = new ExplorationIterationData
                (
                    xml.Element(XmlTokens.XmlBaseParameterContext)?.ToBaseParameterContext() ?? throw new Exception("Exploration iteration XML must have a 'Iteration/StartingState' element."),
                    xml.Element(XmlTokens.XmlExecutionParameterContext)?.ToExecutionParameterContext() ?? throw new Exception("Exploration iteration XML must have a 'Iteration/EndingState' element."),
                    int.Parse(xml.Attribute(nameof(ExplorationIterationData.Number))?.Value ?? throw new Exception("Exploration iteration XML must have a 'Number' attribute.")),
                    xml.Attribute(nameof(ExplorationIterationData.PathConstraint))?.Value ?? string.Empty,
                    null, // create exception from the XML if there is one
                    xml.Element(nameof(ExplorationIterationData.StandardOutput))?.Value ?? string.Empty,
                    xml.Element(nameof(ExplorationIterationData.ErrorOutput))?.Value ?? string.Empty
                );

            return iterationData;
        }
    }
}
