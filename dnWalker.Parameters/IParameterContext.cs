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
        
        string Name { get; set; }

    }

    public static partial class ParameterContextExtensions
    {
        public static bool Remove(this IParameterContext context, ParameterRef reference)
        {
            if (reference.TryResolve(context, out IParameter? parameter))
            {
                context.Parameters.Remove(reference);

                // remove it from all owners and roots
                foreach (var accessor in parameter.Accessors)
                {
                    if (accessor is ItemParameterAccessor item)
                    {
                        item.ParentRef.Resolve<IItemOwnerParameter>(context)?.ClearItem(item.Index);
                    }
                    else if (accessor is FieldParameterAccessor field)
                    {
                        field.ParentRef.Resolve<IFieldOwnerParameter>(context)?.ClearField(field.FieldName);
                    }
                    else if (accessor is MethodResultParameterAccessor method)
                    {
                        method.ParentRef.Resolve<IMethodResolverParameter>(context)?.ClearMethodResult(method.MethodSignature, method.Invocation);
                    }
                    else if (accessor is RootParameterAccessor root)
                    {
                        context.Roots.Remove(root.Expression);
                    }
                    else
                    {
                        // unexpected parameter accessor
                    }
                }


                //if (parameter.Accessor is RootParameterAccessor r)
                //{
                //    context.Roots.Remove(r.Expression);
                //}
                return true;
            }

            return false;
        }
    }
}
