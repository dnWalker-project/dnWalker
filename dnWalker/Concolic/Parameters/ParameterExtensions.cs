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
            return parameterStore.RootParameters
                .SelectMany(p => p.GetParameterExpressions())
                .Where(e => e != null)
                .ToList();
        }

        public static ParameterStore InitializeDefaultMethodParameters(this ParameterStore store, MethodDef method)
        {
            var parameterTypes = method.Parameters.Select(p => p.Type).ToArray();
            var parameterNames = method.Parameters.Select(p => p.Name).ToArray();

            return InitializeRootParameters(store, parameterTypes, parameterNames);
        }

        public static ParameterStore InitializeRootParameters(this ParameterStore store, TypeSig[] parameterTypes, string[] parameterNames)
        {
            for (var i = 0; i < parameterTypes.Length; ++i)
            {
                var parameter = ParameterFactory.CreateParameter(parameterTypes[i], parameterNames[i]);
                store.AddParameter(parameter);
            }

            return store;
        }

        public static DataElementList GetMethodParematers(this ParameterStore store, ExplicitActiveState cur, MethodDef method)
        {
            return GetMethodParematers(store, cur, method.Parameters.Select(p => p.Name).ToArray());
        }

        public static DataElementList GetMethodParematers(this ParameterStore store, ExplicitActiveState cur, string[] parameterNames)
        {
            var arguments = cur.StorageFactory.CreateList(parameterNames.Length);

            for (var i = 0; i < parameterNames.Length; ++i)
            {
                if (store.TryGetParameter(parameterNames[i], out var parameter))
                {
                    arguments[i] = parameter.CreateDataElement(cur);
                }
                else
                {
                    throw new Exception("Cannot initialize method parameters, missing parameter: " + parameterNames[i]);
                }

            }

            return arguments;
        }


        //private static TypeDef GetType(String typeName, ExplicitActiveState cur)
        //{
        //    return cur.DefinitionProvider.GetTypeDefinition(typeName);
        //}
        //private static TypeDef GetType(Parameter parameter, ExplicitActiveState cur)
        //{
        //    return GetType(parameter.TypeName, cur);
        //}

    }
}
