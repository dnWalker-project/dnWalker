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

        public TestClassContext(int iterationNumber,
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
                return _exception == TypeSignature.Empty;
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

        public IEnumerable<string> GetNamespaces()
        {
            HashSet<string> nsSet = new HashSet<string>();

            foreach (IParameter p in _executionSet.Parameters.Values)
            {
                nsSet.Add(p.Type.Namespace);
            }

            if (MethodSignature.IsStatic)
            {
                nsSet.Add(MethodSignature.DeclaringType.Namespace);
            }

            if (IsFaulted)
            {
                nsSet.Add(Exception.Namespace);
            }

            string[] ns = nsSet.ToArray();
            Array.Sort(ns);
            return ns;
        }

        public IEnumerable<AssertionSchema> GetSchemas()
        {
            List<AssertionSchema> schemas = new List<AssertionSchema>();
            if (IsFaulted)
            {
                schemas.Add(new ExceptionSchema(Exception));
                
                // a faulted execution - we do not care for any other schemas
                return schemas;
            }

            // check the return type
            if (!MethodSignature.ReturnType.IsVoid && !IsFaulted)
            {
                if (!_executionSet.TryGetReturnValue(out IParameter? rv))
                {
                    throw new Exception("Could not find the return value.");
                }

                // there is a return value
                schemas.Add(new ReturnValueSchema(rv));

            }

            IReadOnlyParameterSet inSet = _baseSet;
            IReadOnlyParameterSet outSet = _executionSet;

            foreach (ParameterRef r in inSet.Parameters.Keys)
            {
                if (r.TryResolve(inSet, out IObjectParameter? inObj) && 
                    r.TryResolve(outSet, out IObjectParameter? outObj))
                {
                    string[] changedFields = GetChangedFields(inObj, outObj).ToArray();
                    if (changedFields.Length > 0)
                    {
                        ObjectFieldSchema schema = new ObjectFieldSchema(r, inSet, outSet, changedFields);
                        schemas.Add(schema);
                    }
                }

                else if (r.TryResolve(inSet, out IArrayParameter? inArr) &&
                         r.TryResolve(outSet, out IArrayParameter? outArr))
                {
                    int[] changedPositions = GetChangedPositions(inArr, outArr).ToArray();
                    if (changedPositions.Length > 0)
                    {
                        ArrayElementSchema schema = new ArrayElementSchema(r, inSet, outSet, changedPositions);
                        schemas.Add(schema);
                    }
                }
            }

            return schemas;
        }


        private static IEnumerable<string> GetChangedFields(IObjectParameter input, IObjectParameter output)
        {
            List<string> changedFields = new List<string>();

            IReadOnlyDictionary<string, ParameterRef> inFields = input.GetFields();
            IReadOnlyDictionary<string, ParameterRef> outFields = output.GetFields();

            // we know that any field that is in the inFields set must be in the outFields set
            // - we need to find all fields which are in the outFields and not in the inFields (i.a. some new fields were assigned)
            // - we need to find all fields which have different value in outFields than in the inFields (i.a. some fields were overwritten)

            foreach (KeyValuePair<string, ParameterRef> fi in outFields)
            {
                string fieldName = fi.Key;
                if (!inFields.ContainsKey(fieldName))
                {
                    // a newly assigned field
                    changedFields.Add(fieldName);
                }
                else if (fi.Value != inFields[fieldName])
                {
                    // a changed field
                    changedFields.Add(fieldName);
                }
            }

            return changedFields;
        }

        private static IEnumerable<int> GetChangedPositions(IArrayParameter input, IArrayParameter output)
        {
            List<int> changedPositions = new List<int>();

            ParameterRef[] inItems = input.GetItems();
            ParameterRef[] outItems = output.GetItems();

            for (int i = 0; i < inItems.Length; ++i)
            {
                if (inItems[i] != outItems[i])
                {
                    changedPositions.Add(i);
                }
            }

            return changedPositions;
        }
    }
}
