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
            return parameterStore.Parameters.Values
                .Select(p => p.GetExpression())
                .Where(e => e != null)
                .ToList();
        }


        public static ParameterStore InitializeDefaultMethodParameters(this ParameterStore store, ExplicitActiveState cur, MethodDef method)
        {
            for (Int32 i = 0; i < method.Parameters.Count(); ++i)
            {
                dnlib.DotNet.Parameter methodParameter = method.Parameters[i];

                Parameter parameter = ParameterFactory.CreateParameter(methodParameter.Name, methodParameter.Type.ToTypeDefOrRef());
                store.AddParameter(parameter);
            }

            return store;
        }

        public static DataElementList GetMethodParmaters(this ParameterStore store, ExplicitActiveState cur, MethodDef method)
        {
            Int32 count = method.Parameters.Count;

            DataElementList arguments = cur.StorageFactory.CreateList(count);

            for (Int32 i = 0; i < count; ++i)
            {
                arguments[i] = store.Parameters[method.Parameters[i].Name].CreateDataElement(cur);
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
