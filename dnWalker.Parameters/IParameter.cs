using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public interface IParameter
    {

        IParameterContext Context { get; }

        ParameterRef Reference { get; }

        IParameter Clone(IParameterContext newContext);

        ParameterAccessor? Accessor { get; set; }
    }

    public static class ParameterExtensions
    {
        public static bool IsRoot(this IParameter parameter)
        {
            return parameter.Accessor is ParameterAccessor;
        }

        public static bool IsRoot<TAccessor>(this IParameter parameter, [NotNullWhen(true)] out TAccessor? rootAccessor)
            where TAccessor : ParameterAccessor
        {
            rootAccessor = parameter.Accessor as TAccessor;
            return rootAccessor != null;
        }

        public static bool IsField(this IParameter parameter, [NotNullWhen(true)] out IFieldOwnerParameter? fieldOwner, [NotNullWhen(true)] out string? fieldName)
        {
            if (parameter.Accessor is FieldParameterAccessor fieldAccessor &&
                fieldAccessor.ParentRef.TryResolve(parameter.Context, out fieldOwner))
            {
                fieldName = fieldAccessor.FieldName;
                return true;
            }

            fieldOwner = null;
            fieldName = null;
            return false;
        }

        public static bool IsItem(this IParameter parameter, [NotNullWhen(true)] out IItemOwnerParameter? itemOwner, out int index)
        {
            if (parameter.Accessor is ItemParameterAccessor itemAccessor &&
                itemAccessor.ParentRef.TryResolve(parameter.Context, out itemOwner))
            {
                index = itemAccessor.Index;
                return true;
            }

            itemOwner = null;
            index = 0;
            return false;
        }

        public static bool IsMethodResult(this IParameter parameter, [NotNullWhen(true)] out IMethodResolverParameter? methodResolver, out MethodSignature methodSignature, out int invocation)
        {
            if (parameter.Accessor is MethodResultParameterAccessor methodResultAccessor &&
                methodResultAccessor.ParentRef.TryResolve(parameter.Context, out methodResolver))
            {
                invocation = methodResultAccessor.Invocation;
                methodSignature = methodResultAccessor.MethodSignature;
                return true;
            }

            methodResolver = null;
            methodSignature = MethodSignature.Empty;
            invocation = 0;
            return false;
        }

        public static string GetAccessString(this IParameter parameter)
        {
            if (parameter.Accessor == null) return string.Empty;

            return parameter.Accessor.GetAccessString(parameter.Context);
        }
    }
}
