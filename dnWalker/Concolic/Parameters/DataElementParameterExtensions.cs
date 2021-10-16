using dnWalker.Symbolic;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
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
            System.Linq.Expressions.ParameterExpression pExpr = parameter.GetParameterExpressions().First();
            dataElement.SetExpression(pExpr, cur);
        }

        public static Boolean IsInterfaceProxy(this ObjectReference objectReference, ExplicitActiveState cur)
        {
            return TryGetParameter(objectReference, cur, out Parameter p) && p is InterfaceParameter;
        }
    }
}
