using dnWalker.Parameters;
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
        private readonly MethodSignature _methodSignature;
        private readonly string _assemblyName;
        private readonly string _assemblyFileName;
        private readonly IParameterContext _parameterContext;
        private readonly IReadOnlyParameterSet _baseSet;
        private readonly IReadOnlyParameterSet _executionSet;
        private readonly string _pathConstraint;
        private readonly string _standardOutput;
        private readonly string _errorOutput;
        private readonly TypeSignature _exception;

        public TestClassContext(ITestGeneratorConfiguration configuration,
                                int iterationNumber,
                                MethodSignature methodSignature,
                                string assemblyName,
                                string assemblyFileName,
                                IParameterContext parameterContext,
                                IReadOnlyParameterSet baseSet,
                                IReadOnlyParameterSet executionSet,
                                string pathConstraint,
                                string standardOutput,
                                string errorOutput,
                                TypeSignature exception)
        {
            Configuration = configuration;

            _iterationNumber = iterationNumber;
            _methodSignature = methodSignature;
            _assemblyName = assemblyName ?? throw new ArgumentNullException(nameof(assemblyName));
            _assemblyFileName = assemblyFileName ?? throw new ArgumentNullException(nameof(assemblyFileName));
            _parameterContext = parameterContext ?? throw new ArgumentNullException(nameof(parameterContext));
            _baseSet = baseSet ?? throw new ArgumentNullException(nameof(baseSet));
            _executionSet = executionSet ?? throw new ArgumentNullException(nameof(executionSet));
            _pathConstraint = pathConstraint ?? throw new ArgumentNullException(nameof(pathConstraint));
            _standardOutput = standardOutput ?? throw new ArgumentNullException(nameof(standardOutput));
            _errorOutput = errorOutput ?? throw new ArgumentNullException(nameof(errorOutput));
            _exception = exception;
        }

        public IDefinitionProvider DefinitionProvider
        {
            get
            {
                return _parameterContext.DefinitionProvider;
            }
        }

        public int IterationNumber
        {
            get
            {
                return _iterationNumber;
            }
        }

        public MethodSignature MethodSignature
        {
            get
            {
                return _methodSignature;
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

        public IParameterContext ParameterContext
        {
            get
            {
                return _parameterContext;
            }
        }

        public IReadOnlyParameterSet BaseSet
        {
            get
            {
                return _baseSet;
            }
        }

        public IReadOnlyParameterSet ExecutionSet
        {
            get
            {
                return _executionSet;
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

        public TypeSignature Exception
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
                return _exception != TypeSignature.Empty;
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

        public ITestGeneratorConfiguration Configuration
        {
            get;
        }
    }
}
