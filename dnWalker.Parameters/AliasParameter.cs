using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    //public class AliasParameter : Parameter, IAliasParameter
    //{
    //    public AliasParameter(IParameterContext context, ParameterRef referencedParameter) : base(context)
    //    {
    //        ReferencedParameter = referencedParameter;
    //    }

    //    public AliasParameter(IParameterContext context, ParameterRef reference, ParameterRef referencedParameter) : base(context, reference)
    //    {
    //        ReferencedParameter = referencedParameter;
    //    }

    //    public ParameterRef ReferencedParameter 
    //    {
    //        get; 
    //    }

    //    public override AliasParameter Clone(IParameterContext newContext)
    //    {
    //        AliasParameter aliasParameter = new AliasParameter(newContext, Reference, ReferencedParameter)
    //        {
    //            Accessor = this.Accessor?.Clone()
    //        };

    //        return aliasParameter;
    //    }

    //    public override string ToString()
    //    {
    //        return $"AliasParameter, Reference = {Reference}, ReferencedParameter = {ReferencedParameter}, IsNull = {((IAliasParameter)this).IsNull}";
    //    }
    //}

    //public static partial class ParameterContextExtensions
    //{
    //    public static IAliasParameter CreateAliasParameter(this IParameterContext context, ParameterRef referencedParameter)
    //    {
    //        return CreateAliasParameter(context, ParameterRef.Any, referencedParameter);
    //    }

    //    public static IAliasParameter CreateAliasParameter(this IParameterContext context, ParameterRef reference, ParameterRef referencedParameter)
    //    {
    //        AliasParameter parameter = new AliasParameter(context, reference, referencedParameter);

    //        context.Parameters.Add(parameter.Reference, parameter);

    //        return parameter;
    //    }

    //    public static bool RefEquals(this IParameterContext context, ParameterRef lhs, ParameterRef rhs)
    //    {
    //        if (lhs == rhs) return true;

    //        if (lhs.TryResolve(context, out IAliasParameter? lhsAlias) &&
    //            rhs.TryResolve(context, out IAliasParameter? rhsAlias))
    //        {
    //            return lhsAlias.ReferencedParameter == rhsAlias.ReferencedParameter;
    //        }

    //        if (lhs.TryResolve(context, out lhsAlias) &&
    //            rhs.TryResolve(context, out IReferenceTypeParameter? rhsRefType))
    //        {
    //            return lhsAlias.ReferencedParameter == rhsRefType.Reference;
    //        }

    //        if (lhs.TryResolve(context, out IReferenceTypeParameter? lhsRefType) &&
    //            rhs.TryResolve(context, out rhsAlias))
    //        {
    //            return lhsRefType.Reference == rhsAlias.ReferencedParameter;
    //        }

    //        return false;
    //    }
    //}
}
