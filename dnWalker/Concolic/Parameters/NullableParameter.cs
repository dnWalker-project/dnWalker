using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Expressions = System.Linq.Expressions;

namespace dnWalker.Concolic.Parameters
{
    public abstract class NullableParameter : Parameter
    {
        public const String IsNullParameterName = "#__IS_NULL__";

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
                    return isNullField.Value.Value;
                }
                return null;
            }
        }

        protected override Type GetFrameworkType()
        {
            return null;
        }
    }
}
