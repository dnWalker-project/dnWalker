using dnWalker.Symbolic;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public static partial class ParameterExtensions
    {
        public static void SetParameter(this IDataElement dataElement, IParameter parameter, ExplicitActiveState cur)
        {
            cur.PathStore.CurrentPath.SetObjectAttribute(dataElement, "parameter", parameter);

            if (parameter is IPrimitiveValueParameter primitiveValueParameter)
            {
                // set value parameter expression as well
                Expression e = primitiveValueParameter.GetValueExpression(cur);
                dataElement.SetExpression(e, cur);
            }
        }

        public static bool TryGetParameter(this IDataElement dataElement, ExplicitActiveState cur, [NotNullWhen(true)] out IParameter parameter)
        {
            return cur.PathStore.CurrentPath.TryGetObjectAttribute(dataElement, "parameter", out parameter);
        }

        public static bool TryGetParameter<TParameter>(this IDataElement dataElement, ExplicitActiveState cur, [NotNullWhen(true)] out TParameter arrayParameter)
            where TParameter : class, IParameter
        {
            TryGetParameter(dataElement, cur, out IParameter p);
            arrayParameter = p as TParameter;
            return p != null;
        }
    }
}
