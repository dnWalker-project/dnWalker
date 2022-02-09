using dnWalker.Explorations;
using dnWalker.Parameters;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;

namespace dnWalker.TestGenerator.TestClasses
{
    public partial class TestClassContext
    {
        public class Builder
        {
            private int _iterationNumber;
            private MethodSignature _methodSignature;
            private string? _assemblyName;
            private string? _assemblyFileName;
            private IParameterContext? _parameterContext;
            private IReadOnlyParameterSet? _baseSet;
            private IReadOnlyParameterSet? _executionSet;
            private string? _pathConstraint;
            private string? _standardOutput;
            private string? _errorOutput;

            public int IterationNumber
            {
                get
                {
                    return _iterationNumber;
                }

                set
                {
                    _iterationNumber = value;
                }
            }
            public MethodSignature MethodSignature
            {
                get
                {
                    return _methodSignature;
                }

                set
                {
                    _methodSignature = value;
                }
            }
            public string? AssemblyName
            {
                get
                {
                    return _assemblyName;
                }

                set
                {
                    _assemblyName = value;
                }
            }
            public string? AssemblyFileName
            {
                get
                {
                    return _assemblyFileName;
                }

                set
                {
                    _assemblyFileName = value;
                }
            }
            public IParameterContext? ParameterContext
            {
                get
                {
                    return _parameterContext;
                }

                set
                {
                    _parameterContext = value;
                }
            }
            public IReadOnlyParameterSet? BaseSet
            {
                get
                {
                    return _baseSet;
                }

                set
                {
                    _baseSet = value;
                }
            }
            public IReadOnlyParameterSet? ExecutionSet
            {
                get
                {
                    return _executionSet;
                }

                set
                {
                    _executionSet = value;
                }
            }
            public string? PathConstraint
            {
                get
                {
                    return _pathConstraint;
                }

                set
                {
                    _pathConstraint = value;
                }
            }
            public string? StandardOutput
            {
                get
                {
                    return _standardOutput;
                }

                set
                {
                    _standardOutput = value;
                }
            }
            public string? ErrorOutput
            {
                get
                {
                    return _errorOutput;
                }

                set
                {
                    _errorOutput = value;
                }
            }

            public TestClassContext Build()
            {
                if (_assemblyName == null) throw new NullReferenceException("AssemblyName is NULL");
                if (_assemblyFileName == null) throw new NullReferenceException("AssemblyFileName is NULL");
                if (_parameterContext == null) throw new NullReferenceException("AssemblyName is NULL");
                if (_baseSet == null) throw new NullReferenceException("AssemblyName is NULL");
                if (_executionSet == null) throw new NullReferenceException("AssemblyName is NULL");
                if (_pathConstraint == null) throw new NullReferenceException("AssemblyName is NULL");
                if (_standardOutput == null) throw new NullReferenceException("AssemblyName is NULL");
                if (_errorOutput == null) throw new NullReferenceException("AssemblyName is NULL");

                return new TestClassContext(_iterationNumber,
                                            _methodSignature,
                                            _assemblyName,
                                            _assemblyFileName,
                                            _parameterContext,
                                            _baseSet,
                                            _executionSet,
                                            _pathConstraint,
                                            _standardOutput,
                                            _errorOutput);
            }

        }

        public static IReadOnlyList<TestClassContext> FromExplorationData(params ConcolicExploration[] data)
        {
            List<TestClassContext> result = new List<TestClassContext>();

            foreach (ConcolicExploration exploration in data)
            {
                // these three services provides execution agnostic data => can be shared in between "explorations"
                IDomain domain = Domain.LoadFromFile(exploration.AssemblyFileName);
                IDefinitionProvider definitionProvider = new DefinitionProvider(domain);
                IParameterContext context = new ParameterContext(definitionProvider);

                IMethodTranslator methodTranslator = new MethodTranslator(definitionProvider);
                MethodSignature methodSignature = methodTranslator.FromString(exploration.MethodSignature);

                foreach (ConcolicExplorationIteration iteration in exploration.Iterations)
                {
                    Builder builder = new Builder()
                    {
                        IterationNumber = iteration.IterationNumber,
                        MethodSignature = methodSignature,
                        AssemblyFileName = exploration.AssemblyFileName,
                        AssemblyName = exploration.AssemblyName,
                        BaseSet = iteration.BaseParameterSet.Construct(context),
                        ExecutionSet = iteration.ExecutionParameterSet.Construct(context),
                        //ErrorOutput = iteration.ErrorOutput,
                        //StandardOutput = iteration.StandardOutput,
                    };

                    result.Add(builder.Build());
                }
            }

            return result;
        }
    }
}
