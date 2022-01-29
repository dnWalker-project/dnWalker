using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public interface IFieldOwner
    {
        IReadOnlyDictionary<string, ParameterRef> GetFields();

        bool TryGetField(string fieldName, out ParameterRef fieldRef);
        void SetField(string fieldName, ParameterRef fieldRef);
        void ClearField(string fieldName);

        void MoveTo(IFieldOwner other)
        {
            foreach(KeyValuePair<string, ParameterRef> kvp in GetFields().ToList())
            {
                if (kvp.Value == ParameterRef.Empty) continue;

                // make copy of the fields because we might edit the returned dictionary
                other.SetField(kvp.Key, kvp.Value);
                ClearField(kvp.Key);
            }
        }
    }

    public interface IFieldOwnerParameter : IFieldOwner, IParameter
    {
    }

    public static class FieldOwnerParameterExtensions
    {
        public static bool TryGetField(this IFieldOwnerParameter fieldOwner, string fieldName, [NotNullWhen(true)] out IParameter? parameter)
        {
            if (fieldOwner.TryGetField(fieldName, out ParameterRef reference) &&
                reference.TryResolve(fieldOwner.Set, out parameter))
            {
                return true;
            }

            parameter = null;
            return false;
        }

        public static bool TryGetField<TParameter>(this IFieldOwnerParameter fieldOwner, string fieldName, [NotNullWhen(true)] out TParameter? parameter)
            where TParameter : class, IParameter
        {
            if (fieldOwner.TryGetField(fieldName, out ParameterRef reference) &&
                reference.TryResolve(fieldOwner.Set, out parameter))
            {
                return true;
            }

            parameter = null;
            return false;
        }

        public static void SetField(this IFieldOwnerParameter fieldOwner, string fieldName, IParameter? parameter)
        {
            fieldOwner.SetField(fieldName, parameter?.Reference ?? ParameterRef.Empty);
        }

        public static IReadOnlyDictionary<string, IParameter> GetFields(this IFieldOwnerParameter fieldOwner)
        {
            IReadOnlyDictionary<string, ParameterRef> refs = fieldOwner.GetFields();
            
            return new Dictionary<string, IParameter>(refs.Where(p => p.Value != ParameterRef.Empty).Select(p => KeyValuePair.Create(p.Key, p.Value.Resolve(fieldOwner.Set)!)));
        }
    }
}
