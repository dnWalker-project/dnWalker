using dnWalker.Parameters;
using dnWalker.TestGenerator.Parameters;
using dnWalker.TestGenerator.TestClasses;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public partial class TemplateBase
    {
        protected void WriteArrange(AssertionSchema schema)
        {
            if (schema == null) throw new ArgumentNullException(nameof(schema));

            WriteArrange(Context.BaseSet);

            List<IParameter> execSubSet = new List<IParameter>();
            switch (schema)
            {
                case ExceptionSchema exception:
                    // there will be no value/identity assertions => no need to initialize anything else
                    break;

                case ArrayElementSchema arrayElement:
                    // if the changed parameter is within the base set - no need to do anything more
                    // otherwise, we need to arrange the expected values and do the identity or recursive value comparison
                    foreach (int position in arrayElement.Positions)
                    {
                        if (arrayElement.OutputState.TryGetItem(position, out ParameterRef pRef))
                        {
                            if (!Context.BaseSet.Parameters.ContainsKey(pRef))
                            {
                                execSubSet.Add(pRef.Resolve(Context.ExecutionSet) ?? throw new Exception("Could not resolve the parameter."));
                            }
                        }
                    }
                    break;

                case ObjectFieldSchema objectField:
                    // if the changed parameter is within the base set - no need to do anything more
                    // otherwise, we need to arrange the expected values and do the identity or recursive value comparison
                    foreach (string field in objectField.Fields)
                    {
                        if (objectField.OutputState.TryGetField(field, out ParameterRef pRef))
                        {
                            if (!Context.BaseSet.Parameters.ContainsKey(pRef))
                            {
                                execSubSet.Add(pRef.Resolve(Context.ExecutionSet) ?? throw new Exception("Could not resolve the parameter."));
                            }
                        }
                    }
                    break;

                case ReturnValueSchema retValue:
                    // if the return value is within the base set - no need to do anything else
                    // otherwise, we need to arrange the expected values and do the 
                    ParameterRef retRef = retValue.ReturnValue.Reference;
                    if (!Context.BaseSet.Parameters.ContainsKey(retRef))
                    {
                        execSubSet.Add(retValue.ReturnValue);
                    }
                    break;

                default: throw new ArgumentException($"Unexpected schema type: {schema.GetType().Name}", nameof(schema));
            }

            if (execSubSet.Count > 0)
            {
                WriteArrange(execSubSet, Context.ExecutionSet);
            }
        }

        protected void WriteArrange(IReadOnlyParameterSet set)
        {
            DependencyGraph dependencyGraph = DependencyGraph.Build(set);
            WriteArrange(dependencyGraph);
        }

        protected void WriteArrange(IEnumerable<IParameter> parameters, IReadOnlyParameterSet set)
        {
            IParameter[] paramArray = parameters.ToArray();

            if (paramArray.Length == 0) return;

            // build the full Dependency Graph
            DependencyGraph dependencyGraph = DependencyGraph.Build(set);

            // write arrange for all dependencies of this dependency
            dependencyGraph = dependencyGraph.GetDependencySubGraph(paramArray);

            WriteArrange(dependencyGraph);
        }

        private void WriteArrange(DependencyGraph dependencyGraph)
        {
            Dependency[] dependencies = dependencyGraph.GetSortedDependencies().ToArray();

            foreach (Dependency dependency in dependencies)
            {
                switch (dependency)
                {
                    case SimpleDependency simple: WriteArrangeSimpleDepencency(simple); break;
                    case ComplexDependency complex: WriteArrangeComplesDepencency(complex); break;

                    // in future - more dependency types, for example some kind of file system or interprocess/network IO
                    default: throw new NotSupportedException("Unexpected dependency.");
                }

                WriteLine(string.Empty);
            }
        }
    }
}
