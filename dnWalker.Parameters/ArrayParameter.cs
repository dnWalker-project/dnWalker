using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{

    public class ArrayParameter : ReferenceTypeParameter
    {
        public const String LengthParameterName = "#__LENGTH__";

        private readonly Dictionary<Int32, Parameter> _items = new Dictionary<Int32, Parameter>();

        protected override void OnNameChanged(String newName)
        {
            base.OnNameChanged(newName);

            if (LengthParameter != null)
            {
                LengthParameter.Name = ParameterName.ConstructField(newName, LengthParameterName);
            }

            if (_items != null)
            {
                foreach (KeyValuePair<Int32, Parameter> pair in _items)
                {
                    pair.Value.Name = ParameterName.ConstructIndex(newName, pair.Key);
                }
            }
        }

        public Boolean TryGetItemAt(Int32 index, out Parameter itemParameter)
        {
            //if (TryGetTrait<IndexValueTrait>(t => t.Index == index, out IndexValueTrait field))
            //{
            //    return field.IndexValueParameter;
            //}
            //return null;
            return _items.TryGetValue(index, out itemParameter);
        }

        public void SetItemAt(Int32 index, Parameter parameter)
        {
            if (HasName()) parameter.Name = ParameterName.ConstructIndex(Name, index);

            _items[index] = parameter;

            //if (TryGetTrait<IndexValueTrait>(t => t.Index == index, out IndexValueTrait indexField))
            //{
            //    indexField.IndexValueParameter = parameter;
            //}
            //else
            //{
            //    AddTrait<IndexValueTrait>(new IndexValueTrait(index, parameter));
            //}
        }

        public IEnumerable<KeyValuePair<Int32, Parameter>> GetKnownItems()
        {
            return _items;
        }

        public Int32Parameter LengthParameter
        {
            get;
        }

        public Int32? Length
        {
            get { return LengthParameter.Value; }
            set { LengthParameter.Value = value; }
            //get
            //{
            //    if (TryGetTrait<LengthTrait>(out LengthTrait lengthField))
            //    {
            //        return lengthField.LengthParameter.Value;
            //    }
            //    return null;
            //}
            //set
            //{
            //    // if value is null => we are clearing it
            //    if (!value.HasValue)
            //    {
            //        if (TryGetTrait(out LengthTrait trait))
            //        {
            //            Traits.Remove(trait);
            //        }
            //    }
            //    // update current trait or add a new one
            //    else
            //    {
            //        if (TryGetTrait(out LengthTrait trait))
            //        {
            //            trait.LengthParameter.Value = value;
            //        }
            //        else
            //        {
            //            trait = new LengthTrait(value.Value);
            //            AddTrait(trait);
            //        }
            //    }
            //}
        }

        public String ElementTypeName { get; }

        public ArrayParameter(String elementTypeName) : base(elementTypeName + "[]")
        {
            ElementTypeName = elementTypeName;
            LengthParameter = new Int32Parameter();
        }

        public ArrayParameter(String elementTypeName, String name) : base(elementTypeName + "[]", name)
        {
            ElementTypeName = elementTypeName;
            LengthParameter = new Int32Parameter(ParameterName.ConstructField(name, LengthParameterName));
        }

        public override IEnumerable<ParameterExpression> GetParameterExpressions()
        {
            return base.GetParameterExpressions()
                .Concat(LengthParameter.GetParameterExpressions())
                .Concat(_items.Values.SelectMany(itemParameter => itemParameter.GetParameterExpressions()));
        }


        public override Boolean TryGetChildParameter(String name, out Parameter childParameter)
        {
            if (base.TryGetChildParameter(name, out childParameter)) return true;


            String accessor = ParameterName.GetAccessor(Name, name);
            if (accessor == LengthParameterName)
            {
                childParameter = LengthParameter;
                return true;
            }
            else if (Int32.TryParse(accessor, out Int32 index) && TryGetItemAt(index, out childParameter))
            {
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
