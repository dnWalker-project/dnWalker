using dnlib.DotNet;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;

namespace dnWalker.Concolic.Parameters
{
    public static class ParameterExtensions
    {
        public static Boolean TryGetName(this Parameter parameter, out String name)
        {
            if (parameter.TryGetTrait<NamedParameterTrait>(out NamedParameterTrait trait))
            {
                name = trait.Name;
                return true;
            }
            name = String.Empty;
            return false;
        }

        public static DataElementList GetArguments(this ParameterStore store, ExplicitActiveState cur, ParameterList methodParameters, IDictionary<String, Object> solvedValues)
        {
            store.Parameters.Clear();

            Parameter[] parameters = new Parameter[methodParameters.Count()];
            DataElementList arguments = cur.StorageFactory.CreateList(methodParameters.Count());

            // 1. add named parameters for each parameter in the method && set its traits
            for(Int32 i = 0; i < methodParameters.Count; ++i)
            {
                ref Parameter p = ref parameters[i];

                p = store.AddNamedParameter(methodParameters[i].Name, methodParameters[i].Type.ToTypeDefOrRef());
                p.SetTraits(solvedValues, store);

            }

            // 2. create && setup the DataElementList
            for (Int32 i = 0; i < methodParameters.Count; ++i)
            {
                arguments[i] = parameters[i].AsDataElement(cur);
            }
            return arguments;
        }


        public static List<System.Linq.Expressions.ParameterExpression> GetLeafsAsParameterExpression(this ParameterStore store)
        {
            List<System.Linq.Expressions.ParameterExpression> expressions = new List<System.Linq.Expressions.ParameterExpression>();

            foreach (Parameter p in store.Parameters)
            {
                expressions.AddRange(p.GetParameterExpressions());
            }

            return expressions;
        }
    }
}
