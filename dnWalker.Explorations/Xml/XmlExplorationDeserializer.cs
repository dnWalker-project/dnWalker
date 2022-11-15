using dnlib.DotNet;

using dnWalker.Symbolic.Xml;
using dnWalker.TypeSystem;

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
        private readonly ITypeParser _typeParser;
        private readonly IMethodParser _methodParser;
        private readonly XmlModelDeserializer _modelDeserializer;

        public XmlExplorationDeserializer(ITypeParser typeParser, IMethodParser methodParser, XmlModelDeserializer modelDeserializer)
        {
            _typeParser = typeParser;
            _methodParser = methodParser;
            _modelDeserializer = modelDeserializer;
        }

        public ConcolicExploration FromXml(XElement xml)
        {
            return GetExplorationBuilder(xml).Build();
        }

        internal ConcolicExploration.Builder GetExplorationBuilder(XElement xml)
        {
            ConcolicExploration.Builder builder = new ConcolicExploration.Builder();

            builder.AssemblyName = xml.Attribute(XmlTokens.AssemblyName)?.Value ?? throw new MissingAttributeException(nameof(ConcolicExploration), XmlTokens.AssemblyName);
            builder.AssemblyFileName = xml.Attribute(XmlTokens.AssemblyFileName)?.Value ?? throw new MissingAttributeException(nameof(ConcolicExploration), XmlTokens.AssemblyFileName);
            
            string methodSignature = xml.Attribute(XmlTokens.MethodSignature)?.Value ?? throw new MissingAttributeException(nameof(ConcolicExploration), XmlTokens.MethodSignature);
            IMethod method = _methodParser.Parse(methodSignature);
            builder.MethodUnderTest = method;
            
            builder.Solver = xml.Attribute(XmlTokens.Solver)?.Value ?? throw new MissingAttributeException(nameof(ConcolicExploration), XmlTokens.Solver);
            builder.Start = DateTime.ParseExact(xml.Attribute(XmlTokens.Start)?.Value ?? throw new MissingAttributeException(nameof(ConcolicExploration), XmlTokens.Start), XmlTokens.DateTimeFormat, CultureInfo.InvariantCulture);
            builder.End = DateTime.ParseExact(xml.Attribute(XmlTokens.End)?.Value ?? throw new MissingAttributeException(nameof(ConcolicExploration), XmlTokens.Start), XmlTokens.DateTimeFormat, CultureInfo.InvariantCulture);
            builder.Failed = bool.Parse(xml.Attribute(XmlTokens.Failed)?.Value ?? throw new MissingAttributeException(nameof(ConcolicExploration), XmlTokens.Failed));

            foreach (XElement iterationXml in xml.Elements(XmlTokens.Iteration))
            {
                builder.Iterations.Add(GetIterationBuilder(iterationXml, method));
            }

            return builder;
        }

        internal ConcolicExplorationIteration.Builder GetIterationBuilder(XElement xml, IMethod method)
        {
            ConcolicExplorationIteration.Builder builder = new ConcolicExplorationIteration.Builder();

            builder.IterationNumber = int.Parse(xml.Attribute(XmlTokens.Iteration)?.Value ?? throw new MissingAttributeException(nameof(ConcolicExplorationIteration), XmlTokens.Iteration));
            builder.Start = DateTime.ParseExact(xml.Attribute(XmlTokens.Start)?.Value ?? throw new MissingAttributeException(nameof(ConcolicExplorationIteration), XmlTokens.Start), XmlTokens.DateTimeFormat, CultureInfo.InvariantCulture);
            builder.End = DateTime.ParseExact(xml.Attribute(XmlTokens.End)?.Value ?? throw new MissingAttributeException(nameof(ConcolicExplorationIteration), XmlTokens.Start), XmlTokens.DateTimeFormat, CultureInfo.InvariantCulture);
            builder.PathConstraint = xml.Attribute(XmlTokens.PathConstraint)?.Value ?? throw new MissingAttributeException(nameof(ConcolicExplorationIteration), XmlTokens.PathConstraint);

            string? exceptionType = xml.Attribute(XmlTokens.Exception)?.Value;
            if (exceptionType != null) 
            {
                TypeSig exception = _typeParser.Parse(exceptionType);
                builder.Exception = exception;
            }

            builder.StandardOutput = xml.Attribute(XmlTokens.StandardOutput)?.Value ?? string.Empty;
            builder.ErrorOutput = xml.Attribute(XmlTokens.ErrorOutput)?.Value ?? string.Empty;

            XElement inputModelXml = xml.Element(XmlTokens.InputModel) ?? throw new MissingElementException(nameof(ConcolicExplorationIteration), $"{XmlTokens.InputModel}");
            XElement outputModelXml = xml.Element(XmlTokens.OutputModel) ?? throw new MissingElementException(nameof(ConcolicExplorationIteration), $"{XmlTokens.OutputModel}");

            builder.InputModel = _modelDeserializer.FromXml(inputModelXml, method);
            builder.OutputModel = _modelDeserializer.FromXml(outputModelXml, method);

            return builder;
        }
    }
}
