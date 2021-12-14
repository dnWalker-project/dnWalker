using dnlib.DotNet;

using dnWalker.DataElements;
using dnWalker.Parameters.Expressions;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DnParameter = dnlib.DotNet.Parameter;

namespace dnWalker.Parameters
{
    public static partial class ParameterStoreExtensions
    {
        public static void EnsureMethodParameters(this ParameterStore store, MethodDef method)
        {
            if (method.HasThis)
            {
                if (!store.TryGetRootParameter(ParameterStore.ThisName, out IParameter parameter))
                {
                    // it is not specified => make it default
                    parameter = ParameterFactory.CreateParameter(method.DeclaringType.ToTypeSig());
                    store.AddRootParameter(ParameterStore.ThisName, parameter);
                }
            }

            foreach (DnParameter p in method.Parameters)
            {
                if (!store.TryGetRootParameter(p.Name, out IParameter parameter))
                {
                    // it is not specified => make it default
                    parameter = ParameterFactory.CreateParameter(p.Type);
                    store.AddRootParameter(p.Name, parameter);
                }
            }
        }

        public static DataElementList CreateMethodArguments(this ParameterStore store, MethodDef method, ExplicitActiveState cur)
        {
            int parameterCount = method.GetParamCount() + (method.HasThis ? 1 : 0);
            DataElementList arguments = cur.StorageFactory.CreateList(parameterCount);

            int idx = 0;

            if (method.HasThis)
            {
                if (store.TryGetRootParameter(ParameterStore.ThisName, out IParameter parameter))
                {
                    arguments[idx++] = parameter.AsDataElement(cur);
                }
                else
                {
                    throw new Exception("Hidden THIS parameter is not defined in the store.");
                }
            }

            foreach (DnParameter p in method.Parameters)
            {
                if (store.TryGetRootParameter(p.Name, out IParameter parameter))
                {
                    arguments[idx++] = parameter.AsDataElement(cur);
                }
                else
                {
                    throw new Exception($"Parameter {p.Name} is not defined in the store.");
                }
            }

            return arguments;
        }

        public static void SetReturnValue(this ParameterStore store, ReturnValue retValue, ExplicitActiveState cur)
        {
            // TODO
        }

        private class ExprSorter : IComparer<KeyValuePair<ParameterExpression, object>>
        {
            public int Compare(KeyValuePair<ParameterExpression, object> x, KeyValuePair<ParameterExpression, object> y)
            {
                // return -1 if expr1 <  expr2 => we want to handle expr1 before expr2
                // return  0 if expr1 == expr2 => we do not care which one we handle first
                // return  1 if expr1 >  expr2 => we want to handle expr2 before expr1

                ParameterExpression px = x.Key;
                ParameterExpression py = x.Key;

                switch ((px, py))
                {
                    // value of, lenght of, is null - all must be handled before any ref equals
                    case (ValueOfParameterExpression _, RefEqualsParameterExpression _):
                    case (LengthOfParameterExpression _, RefEqualsParameterExpression _):
                    case (IsNullParameterExpression _, RefEqualsParameterExpression _):
                        return -1;

                    case (RefEqualsParameterExpression _, RefEqualsParameterExpression _):
                    default:
                        // left is ! refEquals, right is ! RefEquals
                        return 0;

                    case (RefEqualsParameterExpression _, _):
                        return 1;
                }
            }

            private static readonly Lazy<ExprSorter> _lazy = new Lazy<ExprSorter>();

            public static ExprSorter Instance { get { return _lazy.Value; } }
        }

        public static ParameterStore NextGeneration(this ParameterStore store, IDictionary<string, object> rawData)
        {
            ParameterStore nextGeneration = new ParameterStore();

            KeyValuePair<ParameterExpression, object>[] data = rawData
                .Select(p => KeyValuePair.Create(ParameterExpression.Parse(p.Key), p.Value))
                .ToArray();

            Array.Sort(data, ExprSorter.Instance);

            return nextGeneration;

            // we now have data in order in which we want to handle the parametric expression


            //foreach (KeyValuePair<string, object> pair in data)
            //{
            //    string key = pair.Key;

            //    ParametricExpression expr = ParametricExpression.Parse(key);

            //}

            //foreach (IParameter parameter in toPrune
            //    .Select(i => { store.TryGetParameter(i, out IParameter parameter); return parameter; })
            //    .Where(parameter => parameter != null))
            //{
            //    store.PruneParameter(parameter);
            //}
        }
    }
}
