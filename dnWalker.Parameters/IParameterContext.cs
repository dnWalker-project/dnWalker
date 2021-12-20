using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public interface IParameterContext : ICloneable
    {
        object ICloneable.Clone()
        {
            return Clone();
        }

        new IParameterContext Clone();

        IDictionary<ParameterRef, IParameter> Parameters { get; }

        IDictionary<string, ParameterRef> Roots { get; }

    }

    public static partial class ParameterContextExtensions
    {
        public static bool Remove(this IParameterContext context, ParameterRef reference)
        {
            if (reference.TryResolve(context, out IParameter? parameter))
            {
                context.Parameters.Remove(reference);
                if (parameter.Accessor is RootParameterAccessor r)
                {
                    context.Roots.Remove(r.Expression);
                }
                return true;
            }

            return false;
        }
    }
}
