using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.TestClasses
{
    public partial class TestClassContext : ITestClassContext
    {
        private readonly int _iterationNumber;
        private readonly IMethod _IMethod;
        private readonly string _assemblyName;
        private readonly string _assemblyFileName;
        private readonly IReadOnlyModel _inputModel;
        private readonly IReadOnlyModel _outputModel;
        private readonly string _pathConstraint;
        private readonly string _standardOutput;
        private readonly string _errorOutput;
        private readonly TypeSig _exception;
        private readonly List<string> _usings = new List<string>();
        public TestClassContext(int iterationNumber,
                                IMethod IMethod,
                                string assemblyName,
                                string assemblyFileName,
                                IReadOnlyModel inputModel,
                                IReadOnlyModel outputModel,
                                string pathConstraint,
                                string standardOutput,
                                string errorOutput,
                                TypeSig exception)
        {
            _iterationNumber = iterationNumber;
            _IMethod = IMethod;
            _assemblyName = assemblyName ?? throw new ArgumentNullException(nameof(assemblyName));
            _assemblyFileName = assemblyFileName ?? throw new ArgumentNullException(nameof(assemblyFileName));
            _inputModel = inputModel ?? throw new ArgumentNullException(nameof(inputModel));
            _outputModel = outputModel ?? throw new ArgumentNullException(nameof(outputModel));
            _pathConstraint = pathConstraint ?? throw new ArgumentNullException(nameof(pathConstraint));
            _standardOutput = standardOutput ?? throw new ArgumentNullException(nameof(standardOutput));
            _errorOutput = errorOutput ?? throw new ArgumentNullException(nameof(errorOutput));
            _exception = exception;
        }

        public int IterationNumber
        {
            get
            {
                return _iterationNumber;
            }
        }

        public IMethod Method
        {
            get
            {
                return _IMethod;
            }
        }

        public string AssemblyName
        {
            get
            {
                return _assemblyName;
            }
        }

        public string AssemblyFileName
        {
            get
            {
                return _assemblyFileName;
            }
        }

        public IReadOnlyModel InputModel
        {
            get
            {
                return _inputModel;
            }
        }

        public IReadOnlyModel OutputModel
        {
            get
            {
                return _outputModel;
            }
        }

        public string PathConstraint
        {
            get
            {
                return _pathConstraint;
            }
        }

        public string StandardOutput
        {
            get
            {
                return _standardOutput;
            }
        }

        public string ErrorOutput
        {
            get
            {
                return _errorOutput;
            }
        }

        public TypeSig Exception
        {
            get
            {
                return _exception;
            }
        }

        public bool IsFaulted
        {
            get
            {
                return _exception != null;
            }
        }

        // TODO: generate name from the exploration data
        public string TestNamespaceName
        {
            get;
        } = "TestNamespace";

        // TODO: generate name from the exploration data
        public string TestClassName
        {
            get;
        } = "TestClass";

        public IList<string> Usings
        {
            get
            {
                return _usings;
            }
        }
    }
}
