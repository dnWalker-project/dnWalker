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
        public const String IsNullParameterName = "#__IS_NULL__";


        public ReferenceTypeParameter(String typeName) : base(typeName)
        {
            IsNullParameter = new BooleanParameter();
        }

        public ReferenceTypeParameter(String typeName, String name) : base(typeName, name)
        {
            IsNullParameter = new BooleanParameter(ParameterName.ConstructField(name, IsNullParameterName));
        }

        public BooleanParameter IsNullParameter
        {
            get;
        }

        protected override void OnNameChanged(String newName)
        {
            IsNullParameter.Name = ParameterName.ConstructField(newName, IsNullParameterName);
        }

        public Boolean? IsNull
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

        protected override Type GetFrameworkType()
        {
            return null;
        }
    }
}
