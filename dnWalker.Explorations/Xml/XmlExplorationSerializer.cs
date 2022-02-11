using dnWalker.Parameters.Serialization.Xml;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using static dnWalker.Explorations.Xml.XmlTokens;

namespace dnWalker.Explorations.Xml
{
    public class XmlExplorationSerializer
    {
        public XElement ToXml(ConcolicExploration exploration)
        {
            XElement xml = new XElement(Exploration);
            xml.SetAttributeValue(AssemblyName, exploration.AssemblyName);
            xml.SetAttributeValue(AssemblyFileName, exploration.AssemblyFileName);
            xml.SetAttributeValue(MethodSignature, exploration.MethodSignature);
            xml.SetAttributeValue(Solver, exploration.Solver);
            xml.SetAttributeValue(Start, exploration.Start.ToString(DateTimeFormat, CultureInfo.InvariantCulture));
            xml.SetAttributeValue(End, exploration.End.ToString(DateTimeFormat, CultureInfo.InvariantCulture));
            xml.SetAttributeValue(Failed, exploration.Failed);

            foreach (ConcolicExplorationIteration i in exploration.Iterations)
            {
                xml.Add(ToXml(i));
            }

            return xml;
        }

        private XElement ToXml(ConcolicExplorationIteration iteration)
        {
            XElement xml = new XElement(Iteration);

            xml.SetAttributeValue(Iteration, iteration.IterationNumber);
            xml.SetAttributeValue(Start, iteration.Start.ToString(DateTimeFormat, CultureInfo.InvariantCulture));
            xml.SetAttributeValue(End, iteration.End.ToString(DateTimeFormat, CultureInfo.InvariantCulture));
            xml.SetAttributeValue(PathConstraint, iteration.PathConstraint);
            xml.SetAttributeValue(XmlTokens.Exception, iteration.Exception);
            xml.SetAttributeValue(StandardOutput, iteration.StandardOutput);
            xml.SetAttributeValue(ErrorOutput, iteration.ErrorOutput);

            if (iteration.BaseParameterSet is not XmlParameterSetInfo baseInfo)
            {
                throw new InvalidOperationException("Cannot serialize this IParameterSetInfo into XML. Expected XmlParameterSetInfo.");
            }
            if (iteration.ExecutionParameterSet is not XmlParameterSetInfo execInfo)
            {
                throw new InvalidOperationException("Cannot serialize this IParameterSetInfo into XML. Expected XmlParameterSetInfo.");
            }

            XElement baseXml = new XElement(BaseSet, baseInfo.Xml);
            xml.Add(baseXml);

            XElement execXml = new XElement(ExecutionSet, execInfo.Xml);
            xml.Add(execXml);

            return xml;
        }
    }
}
