using dnWalker.Parameters.Expressions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters.Expressions
{
    public static partial class ParameterStoreExtensions
    {

        public static ParameterStore WithTraits(this ParameterStore store, IEnumerable<ParameterTrait> traits)
        {
            // store.BeginChange();

            ParameterTrait[] traitsArray = traits.ToArray();
            Array.Sort(traitsArray, static (x, y) =>
            {
                ParameterExpression ex = x.Expression;
                ParameterExpression ey = y.Expression;

                switch ((ex, ey))
                {
                    // value of, lenght of, is null - all must be handled before any ref equals
                    case (ValueOfParameterExpression _, RefEqualsParameterExpression _):
                    case (LengthOfParameterExpression _, RefEqualsParameterExpression _):
                    case (IsNullParameterExpression _, RefEqualsParameterExpression _):
                        return -1;

                    case (RefEqualsParameterExpression _, RefEqualsParameterExpression _):
                    default:
                        // left is ! refEquals, right is ! RefEquals
                        return 0;

                    case (RefEqualsParameterExpression _, _):
                        return 1;
                }
            });

            for (int i = 0; i < traitsArray.Length; ++i)
            {
                if (!traitsArray[i].TryApplyTo(store))
                {
                    // store.DiscardChanges();
                    // throw some error?
                }
            }

            //store.EndChange();

            return store;
        }

        
    }
}
