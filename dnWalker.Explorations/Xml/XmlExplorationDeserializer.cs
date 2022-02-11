using dnWalker.Parameters.Serialization.Xml;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dnWalker.Explorations.Xml
{
    public class XmlExplorationDeserializer
    {
        public class MissingElementException : Exception
        {
            public MissingElementException(string context, string elementName)
            {
                ElementName = elementName;
                Context = context;
            }

            protected MissingElementException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
                ElementName = info.GetString(nameof(ElementName)) ?? string.Empty;
                Context = info.GetString(nameof(Context)) ?? string.Empty;
            }

            public string ElementName { get; }

            public string Context { get; }

            public override string Message
            {
                get
                {
                    return $"'{Context}' XML must contain an '{ElementName}' element.";
                }
            }
        }
        public class MissingAttributeException : Exception
        {
            public MissingAttributeException(string context, string attributeName)
            {
                AttributeName = attributeName;
                Context = context;
            }

            protected MissingAttributeException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
                AttributeName = info.GetString(nameof(AttributeName)) ?? string.Empty;
                Context = info.GetString(nameof(Context)) ?? string.Empty;
            }

            public string AttributeName { get; }

            public string Context { get; }

            public override string Message
            {
                get
                {
                    return $"'{Context}' XML must contain an '{AttributeName}' attribute.";
                }
            }
        }

        public ConcolicExploration GetExploration(XElement xml)
        {
            return GetExplorationBuilder(xml).Build();
        }

        internal ConcolicExploration.Builder GetExplorationBuilder(XElement xml)
        {
            ConcolicExploration.Builder builder = new ConcolicExploration.Builder();

            builder.AssemblyName = xml.Attribute(XmlTokens.AssemblyName)?.Value ?? throw new MissingAttributeException(nameof(ConcolicExploration), XmlTokens.AssemblyName);
            builder.AssemblyFileName = xml.Attribute(XmlTokens.AssemblyFileName)?.Value ?? throw new MissingAttributeException(nameof(ConcolicExploration), XmlTokens.AssemblyFileName);
            builder.MethodSignature = xml.Attribute(XmlTokens.MethodSignature)?.Value ?? throw new MissingAttributeException(nameof(ConcolicExploration), XmlTokens.MethodSignature);
            builder.Solver = xml.Attribute(XmlTokens.Solver)?.Value ?? throw new MissingAttributeException(nameof(ConcolicExploration), XmlTokens.Solver);
            builder.Start = DateTime.ParseExact(xml.Attribute(XmlTokens.Start)?.Value ?? throw new MissingAttributeException(nameof(ConcolicExploration), XmlTokens.Start), XmlTokens.DateTimeFormat, CultureInfo.InvariantCulture);
            builder.End = DateTime.ParseExact(xml.Attribute(XmlTokens.End)?.Value ?? throw new MissingAttributeException(nameof(ConcolicExploration), XmlTokens.Start), XmlTokens.DateTimeFormat, CultureInfo.InvariantCulture);
            builder.Failed = bool.Parse(xml.Attribute(XmlTokens.Failed)?.Value ?? throw new MissingAttributeException(nameof(ConcolicExploration), XmlTokens.Failed));

            foreach (XElement iterationXml in xml.Elements(XmlTokens.Iteration))
            {
                builder.Iterations.Add(GetIterationBuilder(iterationXml));
            }

            return builder;
        }

        internal ConcolicExplorationIteration.Builder GetIterationBuilder(XElement xml)
        {
            ConcolicExplorationIteration.Builder builder = new ConcolicExplorationIteration.Builder();

            builder.IterationNumber = int.Parse(xml.Attribute(XmlTokens.Iteration)?.Value ?? throw new MissingAttributeException(nameof(ConcolicExplorationIteration), XmlTokens.Iteration));
            builder.Start = DateTime.ParseExact(xml.Attribute(XmlTokens.Start)?.Value ?? throw new MissingAttributeException(nameof(ConcolicExplorationIteration), XmlTokens.Start), XmlTokens.DateTimeFormat, CultureInfo.InvariantCulture);
            builder.End = DateTime.ParseExact(xml.Attribute(XmlTokens.End)?.Value ?? throw new MissingAttributeException(nameof(ConcolicExplorationIteration), XmlTokens.Start), XmlTokens.DateTimeFormat, CultureInfo.InvariantCulture);
            builder.PathConstraint = xml.Attribute(XmlTokens.PathConstraint)?.Value ?? throw new MissingAttributeException(nameof(ConcolicExplorationIteration), XmlTokens.PathConstraint);

            builder.Exception = xml.Attribute(XmlTokens.Exception)?.Value ?? string.Empty;
            builder.StandardOutput = xml.Attribute(XmlTokens.StandardOutput)?.Value ?? string.Empty;
            builder.ErrorOutput = xml.Attribute(XmlTokens.ErrorOutput)?.Value ?? string.Empty;

            XElement baseXml = xml.Element(XmlTokens.BaseSet)?.Element(Parameters.Xml.XmlTokens.XmlSet) ?? throw new MissingElementException(nameof(ConcolicExplorationIteration), $"{XmlTokens.BaseSet}/{Parameters.Xml.XmlTokens.XmlSet}");
            XElement execXml = xml.Element(XmlTokens.ExecutionSet)?.Element(Parameters.Xml.XmlTokens.XmlSet) ?? throw new MissingElementException(nameof(ConcolicExplorationIteration), $"{XmlTokens.ExecutionSet}/{Parameters.Xml.XmlTokens.XmlSet}");

            builder.BaseParameterSet = new XmlParameterSetInfo(baseXml);
            builder.ExecutionParameterSet = new XmlParameterSetInfo(execXml);

            return builder;
        }
    }
}
