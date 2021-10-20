using dnlib.DotNet;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace dnWalker.Concolic.Parameters
{
    public static partial class ParameterExtensions
    {
        public static List<ParameterExpression> GetParametersAsExpressions(this ParameterStore parameterStore)
        {
            return parameterStore.Parameters
                .Select(p => p.GetExpression())
                .Where(e => e != null)
                .ToList();
        }


        public static ParameterStore InitializeDefaultMethodParameters(this ParameterStore store, ExplicitActiveState cur, MethodDef method)
        {
            for (Int32 i = 0; i < method.Parameters.Count(); ++i)
            {
                dnlib.DotNet.Parameter methodParameter = method.Parameters[i];

                Parameter parameter = ParameterFactory.CreateParameter(methodParameter.Type, methodParameter.Name);
                store.AddParameter(parameter);
            }

            return store;
        }

        public static DataElementList GetMethodParematers(this ParameterStore store, ExplicitActiveState cur, MethodDef method)
        {
            Int32 count = method.Parameters.Count;

            DataElementList arguments = cur.StorageFactory.CreateList(count);

            for (Int32 i = 0; i < count; ++i)
            {
                if (store.TryGetParameter(method.Parameters[i].Name, out Parameter parameter))
                {
                    arguments[i] = parameter.CreateDataElement(cur);
                }
                else
                {
                    throw new Exception("Cannot initialize method parameters, missing parameter: " + method.Parameters[i].Name);
                }

            }

            return arguments;
        }


        private static TypeDef GetType(String typeName, ExplicitActiveState cur)
        {
            return cur.DefinitionProvider.GetTypeDefinition(typeName);
        }
        private static TypeDef GetType(Parameter parameter, ExplicitActiveState cur)
        {
            return GetType(parameter.TypeName, cur);
        }

    }
}
