using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public interface IReferenceTypeParameter : IParameter
    {
        bool? IsNull { get; set; }
    }

    public static class ReferenceTypeParameterExtensions
    {
        public static bool ParameterReferenceEquals(this IReferenceTypeParameter lhs, IReferenceTypeParameter rhs)
        {
            // two parameters can reference equal iff one is alias of another or they are both alias of same, reference type parameter

            // possibilities:
            // 1) lhs ia alias, rhs is alias => lhs aliases same parameter as rhs
            // 2) lhs is alias, rhs is RefType => lhs is alias of rhs
            // 3) lhs is RefType, rhs is alias => rhs is alias of lhs

            IAliasParameter? lhsAlias = lhs as IAliasParameter;
            IAliasParameter? rhsAlias = rhs as IAliasParameter;

            // 1)
            if (lhsAlias != null && rhsAlias != null)
            {
                return lhsAlias.HasSameAliasAs(rhsAlias) && 
                       lhsAlias.TryDereferenceAs(out IReferenceTypeParameter _); // only reference type parameters can have reference equality
            }

            // 2)
            else if (lhsAlias != null) // && rhsAlias == null)
            {
                return lhsAlias.IsAliasOf(rhs);
            }

            // 3)
            else if (rhsAlias != null)
            {
                return rhsAlias.IsAliasOf(lhs);
            }

            // any other options => both are ref types - cannot happen, one must always be alias of the other
            //                   => primitive value parameters - cannot have ref equals
            return false;
        }

        public static bool GetIsNull(this IReferenceTypeParameter referenceType)
        {
            return referenceType.IsNull ?? true;
        }
    }
}
