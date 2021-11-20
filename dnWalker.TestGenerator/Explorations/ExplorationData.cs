
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dnWalker.TestGenerator
{
    /// <summary>
    /// Represents data from a single exploration, exported by dnWalker.
    /// </summary>
    public class ExplorationData
    {
        public static ExplorationData FromXml(XElement xml)
        {
            ExplorationData explorationData = new ExplorationData
                (
                    xml.Elements("Iteration").Select(xe => ExplorationIterationData.FromXml(xe)).ToArray(),
                    xml.Attribute(nameof(AssemblyName))?.Value ?? throw new Exception("Exploration data XML must contain 'AssemblyName' attribute."),
                    xml.Attribute(nameof(AssemblyFileName))?.Value ?? throw new Exception("Exploration data XML must contain 'AssemblyFileName' attribute."),
                    xml.Attribute(nameof(MethodSignature))?.Value ?? throw new Exception("Exploration data XML must contain 'MethodSignature' attribute."),
                    bool.Parse(xml.Attribute(nameof(IsStatic))?.Value ?? throw new Exception("Exploration data XML must contain 'IsStatic' attribute."))
                );

            return explorationData;
        }

        internal ExplorationData(ExplorationIterationData[] iterations, string assemblyName, string assemblyFilePath, string fullMethodName, bool isStatic)
        {
            Iterations = iterations ?? throw new ArgumentNullException(nameof(iterations));
            AssemblyName = assemblyName ?? throw new ArgumentNullException(nameof(assemblyName));
            AssemblyFileName = assemblyFilePath ?? throw new ArgumentNullException(nameof(assemblyFilePath));
            MethodSignature = fullMethodName ?? throw new ArgumentNullException(nameof(fullMethodName));
            IsStatic = isStatic;
        }

        public ExplorationIterationData[] Iterations 
        {
            get;
        }

        public string AssemblyName
        {
            get;
        }

        public string AssemblyFileName
        {
            get;
        }

        public string MethodSignature
        {
            get;
        }

        public bool IsStatic
        {
            get;
        }
    }
}
