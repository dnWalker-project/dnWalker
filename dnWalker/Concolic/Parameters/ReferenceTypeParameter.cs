using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Expressions = System.Linq.Expressions;

namespace dnWalker.Concolic.Parameters
{
    public abstract class ReferenceTypeParameter : Parameter
    {
        public const string IsNullParameterName = "#__IS_NULL__";


        public ReferenceTypeParameter(string typeName) : base(typeName)
        {
            IsNullParameter = new BooleanParameter() { Value = true };
        }

        public ReferenceTypeParameter(string typeName, string name) : base(typeName, name)
        {
            IsNullParameter = new BooleanParameter(ParameterName.ConstructField(name, IsNullParameterName), true);
        }

        public BooleanParameter IsNullParameter
        {
            get;
        }

        protected override void OnNameChanged(string newName)
        {
            // can be invoked by the Parameter constructor => IsNullParameter is not yet initialized
            // but will be invoked only if the constructor ReferenceTypeParameter(String,String) is invoked, e.g. IsNullParameter will be initialized with proper na,e

            if (IsNullParameter != null) 
            {
                IsNullParameter.Name = ParameterName.ConstructField(newName, IsNullParameterName);
            }
        }

        public bool? IsNull
        {
            get
            {
                return IsNullParameter.Value;
            }
            set
            {
                IsNullParameter.Value = value;
            }
            //get
            //{
            //    if (TryGetTrait<IsNullTrait>(out IsNullTrait isNullField))
            //    {
            //        return isNullField.IsNullParameter.Value;
            //    }
            //    return null;
            //}
            //set
            //{
            //    // if value is null => we are clearing it
            //    if (!value.HasValue)
            //    {
            //        if (TryGetTrait(out IsNullTrait trait))
            //        {
            //            Traits.Remove(trait);
            //        }
            //    }
            //    // update current trait or add a new one
            //    else
            //    {
            //        if (TryGetTrait(out IsNullTrait trait))
            //        {
            //            trait.IsNullParameter.Value = value;
            //        }
            //        else
            //        {
            //            trait = new IsNullTrait(value.Value);
            //            AddTrait(trait);
            //        }
            //    }
            //}
        }

        public override IEnumerable<ParameterExpression> GetParameterExpressions()
        {
            return IsNullParameter.GetParameterExpressions();
        }

        public override bool HasSingleExpression => false;

        public override ParameterExpression GetSingleParameterExpression() => null;

        public override bool TryGetChildParameter(string name, out Parameter childParameter)
        {
            var accessor = ParameterName.GetAccessor(Name, name);
            if (accessor == IsNullParameterName)
            {
                childParameter = IsNullParameter;
                return true;
            }
            else
            {
                childParameter = null;
                return false;
            }
        }
    }
}
