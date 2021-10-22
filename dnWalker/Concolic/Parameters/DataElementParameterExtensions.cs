using dnWalker.Symbolic;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Parameters
{
    public static class DataElementParameterExtensions
    {
        public static bool TryGetParameter(this IDataElement dataElement, ExplicitActiveState cur, out Parameter parameter)
        {
            return cur.PathStore.CurrentPath.TryGetObjectAttribute<Parameter>(dataElement, "parameter", out parameter);
        }

        public static void SetParameter(this IDataElement dataElement, Parameter parameter, ExplicitActiveState cur)
        {
            cur.PathStore.CurrentPath.SetObjectAttribute<Parameter>(dataElement, "parameter", parameter);

            // TODO: redo it somehow...
            if (parameter.HasSingleExpression)
            {
                dataElement.SetExpression(parameter.GetSingleParameterExpression(), cur);
            }
        }

        public static bool IsInterfaceParameter(this IDataElement dataElement, ExplicitActiveState cur, out InterfaceParameter interfaceParameter)
        {
            if (TryGetParameter(dataElement, cur, out var p) && p is InterfaceParameter ip)
            {
                interfaceParameter = ip;
                return true;
            }
            else
            {
                interfaceParameter = null;
                return false;
            }
        }

        public static int GetCallCount(this IDataElement dataElement, string methodName, ExplicitActiveState cur)
        {
            var callCount = 0;

            if (!cur.PathStore.CurrentPath.TryGetObjectAttribute<Dictionary<string, int>>(dataElement, "call_counts", out var callCounts))
            {
                SetCallCount(dataElement, methodName, cur, callCount);
                return callCount;
            }

            if (!callCounts.TryGetValue(methodName, out callCount))
            {
                callCount = 0;
                callCounts[methodName] = callCount;
            }

            return callCount;
        }

        public static void SetCallCount(this IDataElement dataElement, string methodName, ExplicitActiveState cur, int callCount)
        {
            if (!cur.PathStore.CurrentPath.TryGetObjectAttribute<Dictionary<string, int>>(dataElement, "call_counts", out var callCounts))
            {
                callCounts = new Dictionary<string, int>();
                cur.PathStore.CurrentPath.SetObjectAttribute(dataElement, "call_counts", callCounts);
            }

            callCounts[methodName] = callCount;
        }

        public static bool IsReferenceTypeParametery(this IDataElement dataElement, ExplicitActiveState cur, out ReferenceTypeParameter referenceTypeParameter)
        {
            if (TryGetParameter(dataElement, cur, out var p) && p is ReferenceTypeParameter rp)
            {
                referenceTypeParameter = rp;
                return true;
            }
            else
            {
                referenceTypeParameter = null;
                return false;
            }
        }


        public static bool IsObjectParameter(this IDataElement dataElement, ExplicitActiveState cur, out ObjectParameter objectParameter)
        {
            if (TryGetParameter(dataElement, cur, out var p) && p is ObjectParameter op)
            {
                objectParameter = op;
                return true;
            }
            else
            {
                objectParameter = null;
                return false;
            }
        }

        public static bool IsArrayParameter(this IDataElement dataElement, ExplicitActiveState cur, out ArrayParameter arrayParameter)
        {
            if (TryGetParameter(dataElement, cur, out var p) && p is ArrayParameter ap)
            {
                arrayParameter = ap;
                return true;
            }
            else
            {
                arrayParameter = null;
                return false;
            }
        }
    }
}
