using System.Diagnostics.CodeAnalysis;

namespace dnWalker.Parameters
{
    public interface IAliasParameter : IReferenceTypeParameter
    {
        /// <summary>
        /// Gets the referenced parameters. Can be another alias parameter. 
        /// In order to get non-alias parameter, use the <see cref="AliasParameterExtensions.GetReferencedParameter"/> extension method.
        /// </summary>
        public IReferenceTypeParameter ReferencedParameter 
        {
            get; 
            set; 
        }
    }

    public static class AliasParameterExtensions
    {
        public static bool TryDereferenceAs<TParameter>(this IAliasParameter aliasParameter, [NotNullWhen(true)] out TParameter? parameter) where TParameter : IReferenceTypeParameter
        {
            IReferenceTypeParameter referencedParameter = aliasParameter.GetReferencedParameter();

            if (referencedParameter is TParameter p)
            {
                parameter = p;
            }
            else
            {
                parameter = default(TParameter);
            }

            return parameter != null;
        }
        public static IReferenceTypeParameter GetReferencedParameter(this IAliasParameter parameter)
        {
            IReferenceTypeParameter refParameter = parameter.ReferencedParameter;
            while (refParameter is IAliasParameter aliasParameter)
            {
                refParameter = aliasParameter.ReferencedParameter;
            }
            return refParameter;
        }

        public static bool IsAliasOf(this IAliasParameter aliasParameter, IReferenceTypeParameter parameter)
        {
            return aliasParameter.ReferencedParameter.Id == parameter.Id;
        }

        public static bool HasSameAliasAs(this IAliasParameter aliasParameter, IAliasParameter other)
        {
            return aliasParameter.IsAliasOf(other.ReferencedParameter);
        }



        public static IAliasParameter CreateAlias(this IReferenceTypeParameter parameter, ParameterStore store)
        {
            return CreateAlias(parameter, store, parameter.TypeName);
        }

        public static IAliasParameter CreateAlias(this IReferenceTypeParameter parameter, ParameterStore store, string typeName)
        {
            // we do not create alias of an alias => dereference to the referenced parameter
            // should only happen once, but use the while cycle, just in case
            while (parameter is IAliasParameter p)
            {
                parameter = p.ReferencedParameter;
            }

            AliasParameter aliasParameter = new AliasParameter(parameter, typeName);
            store.AddParameter(aliasParameter);
            return aliasParameter;
        }

        public static IAliasParameter CreateAlias(this IReferenceTypeParameter parameter, ParameterStore store, int id)
        {
            return CreateAlias(parameter, store, parameter.TypeName, id);
        }

        public static IAliasParameter CreateAlias(this IReferenceTypeParameter parameter, ParameterStore store, string typeName, int id)
        {
            // we do not create alias of an alias => dereference to the referenced parameter
            
            if (parameter is IAliasParameter ap)
            {
                parameter = ap.GetReferencedParameter();
            }

            AliasParameter aliasParameter = new AliasParameter(parameter, typeName, id);
            store.AddParameter(aliasParameter);
            return aliasParameter;
        }
    }
}