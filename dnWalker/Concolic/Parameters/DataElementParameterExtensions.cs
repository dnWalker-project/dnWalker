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
        public static Boolean TryGetParameter(this IDataElement dataElement, ExplicitActiveState cur, out Parameter parameter)
        {
            return cur.PathStore.CurrentPath.TryGetObjectAttribute<Parameter>(dataElement, "parameter", out parameter);
        }

        public static void SetParameter(this IDataElement dataElement, Parameter parameter, ExplicitActiveState cur)
        {
            cur.PathStore.CurrentPath.SetObjectAttribute<Parameter>(dataElement, "parameter", parameter);

            // TODO: redo it somehow...
            ParameterExpression pExpr = parameter.GetExpression();

            if (pExpr != null)
            {
                dataElement.SetExpression(pExpr, cur);
            }

        }

        public static Boolean IsInterfaceParameter(this ObjectReference objectReference, ExplicitActiveState cur, out InterfaceParameter interfaceParameter)
        {
            if (TryGetParameter(objectReference, cur, out Parameter p) && p is InterfaceParameter ip)
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
        public static Boolean IsObjectParameter(this ObjectReference objectReference, ExplicitActiveState cur, out ObjectParameter objectParameter)
        {
            if (TryGetParameter(objectReference, cur, out Parameter p) && p is ObjectParameter op)
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
    }
}
