using dnWalker.Parameters;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.TestClasses
{
    public partial class TestClassContext
    {
        public IDefinitionProvider DefinitionProvider
        {
            get;
        }

        public int IterationNumber
        {
            get;
        }

        public MethodSignature Method
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

        public IReadOnlyParameterSet BaseParameterSet
        {
            get;
        }

        public IReadOnlyParameterSet ExecutionParameterSet
        {
            get;
        }

        public string PathConstraint
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
    }
}
