using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Expressions = System.Linq.Expressions;

namespace dnWalker.Concolic.Parameters
{
    public abstract class NullableParameter : Parameter
    {
        public const String IsNullFieldName = "__IS_NULL__";

        public NullableParameter(String typeName) : base(typeName)
        {
        }

        public NullableParameter(String typeName, IEnumerable<ParameterTrait> traits) : base(typeName, traits)
        {
        }

        public Boolean? IsNull
        {
            get
            {
                if (TryGetTrait<IsNullTrait>(out IsNullTrait isNullField))
                {
                    return isNullField.Value;
                }
                return null;
            }
            set
            {
                if (!value.HasValue)
                {
                    // remove current trait, if such exists
                    if (TryGetTrait<IsNullTrait>(out IsNullTrait isNullField))
                    {
                        Traits.Remove(isNullField);
                    }
                }
                else
                {
                    // update or add a new trait
                    if (TryGetTrait<IsNullTrait>(out IsNullTrait isNullField))
                    {
                        isNullField.Value = value.Value;
                    }
                    else
                    {
                        AddTrait(new IsNullTrait(value.Value));
                    }
                }
            }
        }

        public override IEnumerable<Expressions.ParameterExpression> GetParameterExpressions()
        {
            if (this.TryGetName(out String name))
            {
                yield return Expressions.Expression.Parameter(typeof(Boolean), name + "->" + IsNullFieldName);
            }

            yield break;
        }
    }
}
