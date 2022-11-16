using dnlib.DotNet;

using dnWalker.Explorations;
using dnWalker.Symbolic;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;

namespace dnWalker.TestGenerator.TestClasses
{
    public partial class TestClassContext
    {
        public class Builder
        {
            public Builder()
            {
            }


            internal static Builder NewEmpty()
            {
                Builder builder = new Builder();

                builder._iterationNumber = 0;
                builder._method = null;
                builder._assemblyName = string.Empty;
                builder._assemblyFileName = string.Empty;
                builder._pathConstraint = string.Empty;
                builder._standardOutput = string.Empty;
                builder._errorOutput = string.Empty;
                builder._exception = null;

                return builder;
            }

            private int _iterationNumber;
            private IMethod? _method;
            private string? _assemblyName;
            private string? _assemblyFileName;
            private IReadOnlyModel? _inputModel;
            private IReadOnlyModel? _outputModel;
            private string? _pathConstraint;
            private string? _standardOutput;
            private string? _errorOutput;
            private TypeSig? _exception;

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
            public IMethod? Method
            {
                get
                {
                    return _method;
                }

                set
                {
                    _method = value;
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
            public IReadOnlyModel? InputModel
            {
                get
                {
                    return _inputModel;
                }

                set
                {
                    _inputModel = value;
                }
            }
            public IReadOnlyModel? OutputModel
            {
                get
                {
                    return _outputModel;
                }

                set
                {
                    _outputModel = value;
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
            public TypeSig? Exception
            {
                get
                {
                    return _exception;
                }
                set
                {
                    _exception = value;
                }
            }

            public ITestClassContext Build()
            {
                if (_method == null) throw new NullReferenceException($"'{nameof(Method)}' is NULL");
                if (_assemblyName == null) throw new NullReferenceException($"'{nameof(AssemblyName)}' is NULL");
                if (_assemblyFileName == null) throw new NullReferenceException($"'{nameof(AssemblyFileName)}' is NULL");
                if (_inputModel == null) throw new NullReferenceException($"'{nameof(TestClassContext.InputModel)}' is NULL");
                if (_outputModel == null) throw new NullReferenceException($"'{nameof(TestClassContext.OutputModel)}' is NULL");
                if (_pathConstraint == null) throw new NullReferenceException($"'{nameof(ErrorOutput)}' is NULL");
                if (_standardOutput == null) throw new NullReferenceException($"'{nameof(ErrorOutput)}' is NULL");
                if (_errorOutput == null) throw new NullReferenceException($"'{nameof(ErrorOutput)}' is NULL");
                //if (_exception == null) throw new NullReferenceException($"'{nameof(Exception)}' is NULL");

                return new TestClassContext(_iterationNumber,
                                            _method,
                                            _assemblyName,
                                            _assemblyFileName,
                                            _inputModel,
                                            _outputModel,
                                            _pathConstraint,
                                            _standardOutput,
                                            _errorOutput,
                                            _exception);
            }

        }

        public static IReadOnlyList<ITestClassContext> FromExplorationData(params ConcolicExploration[] data)
        {
            List<ITestClassContext> result = new List<ITestClassContext>();

            foreach (ConcolicExploration exploration in data)
            {
                // these three services provides execution agnostic data => can be shared in between "explorations"
                IDomain domain = Domain.LoadFromFile(exploration.AssemblyFileName);
                IDefinitionProvider definitionProvider = new DefinitionProvider(domain);

                IMethodParser methodTranslator = new MethodParser(definitionProvider);
                ITypeParser typeTranslator = new TypeParser(definitionProvider);
                IMethod methodSignature = exploration.MethodUnderTest;

                foreach (ConcolicExplorationIteration iteration in exploration.Iterations)
                {
                    Builder builder = new Builder()
                    {
                        IterationNumber = iteration.IterationNumber,
                        Method = methodSignature,
                        AssemblyFileName = exploration.AssemblyFileName,
                        AssemblyName = exploration.AssemblyName,
                        InputModel = iteration.InputModel,
                        OutputModel = iteration.OutputModel,
                        ErrorOutput = iteration.ErrorOutput ?? string.Empty,
                        StandardOutput = iteration.StandardOutput ?? string.Empty,
                        Exception = iteration.Exception,
                        PathConstraint = iteration.PathConstraint ?? string.Empty,
                    };

                    result.Add(builder.Build());
                }
            }

            return result;
        }
    }
}
