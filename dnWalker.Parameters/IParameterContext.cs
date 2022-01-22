using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public interface IParameterContext : IReadOnlyParameterContext, ICloneable
    {
        object ICloneable.Clone()
        {
            return Clone();
        }

        new IParameterContext Clone();

        new IDictionary<ParameterRef, IParameter> Parameters { get; }

        new IDictionary<string, ParameterRef> Roots { get; }

        //IReadOnlyDictionary<ParameterRef, IParameter> IReadOnlyParameterContext.Parameters
        //{
        //    get
        //    {
        //        return new ReadOnlyDictionary<ParameterRef, IParameter>(Parameters);
        //    }
        //}

        //IReadOnlyDictionary<string, ParameterRef> IReadOnlyParameterContext.Roots
        //{
        //    get
        //    {
        //        return new ReadOnlyDictionary<string, ParameterRef>(Roots);
        //    }
        //}
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
