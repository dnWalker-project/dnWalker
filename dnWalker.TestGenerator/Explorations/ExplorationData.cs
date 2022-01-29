
using dnWalker.Parameters;
using dnWalker.TypeSystem;

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
        internal ExplorationData(ExplorationIterationData[] iterations, string assemblyName, string assemblyFilePath, MethodSignature methodSignature, bool isStatic)
        {
            Iterations = iterations ?? throw new ArgumentNullException(nameof(iterations));
            AssemblyName = assemblyName ?? throw new ArgumentNullException(nameof(assemblyName));
            AssemblyFileName = assemblyFilePath ?? throw new ArgumentNullException(nameof(assemblyFilePath));
            MethodSignature = methodSignature;
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

        public MethodSignature MethodSignature
        {
            get;
        }

        public bool IsStatic
        {
            get;
        }
    }
}
