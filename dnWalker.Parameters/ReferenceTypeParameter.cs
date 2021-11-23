﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Expressions = System.Linq.Expressions;

namespace dnWalker.Parameters
{
    public abstract class ReferenceTypeParameter : Parameter
    {
        public const String IsNullParameterName = "#__IS_NULL__";


        public ReferenceTypeParameter(String typeName) : base(typeName)
        {
            IsNullParameter = new BooleanParameter() { Value = true };
        }

        public ReferenceTypeParameter(String typeName, String name) : base(typeName, name)
        {
            IsNullParameter = new BooleanParameter(ParameterName.ConstructField(name, IsNullParameterName)) { Value = true };
        }

        public BooleanParameter IsNullParameter
        {
            get;
        }

        /// <summary>
        /// Invoked when the parameter name changes. Updates the <see cref="ReferenceTypeParameter.IsNullParameter"/>
        /// </summary>
        /// <param name="newName"></param>
        protected override void OnNameChanged(String newName)
        {
            if (IsNullParameter != null)
            {
                IsNullParameter.Name = ParameterName.ConstructField(newName, IsNullParameterName);
            }
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

        public override IEnumerable<ParameterExpression> GetParameterExpressions()
        {
            return IsNullParameter.GetParameterExpressions();
        }

        public override Boolean HasSingleExpression => false;

        public override ParameterExpression GetSingleParameterExpression() => null;

        public override Boolean TryGetChildParameter(String name, out Parameter childParameter)
        {
            String accessor = ParameterName.GetAccessor(Name, name);
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
