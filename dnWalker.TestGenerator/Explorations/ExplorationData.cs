
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
