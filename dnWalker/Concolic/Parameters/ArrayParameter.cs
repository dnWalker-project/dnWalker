using dnlib.DotNet;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Parameters
{

    public class ArrayParameter : ReferenceTypeParameter
    {
        public const string LengthParameterName = "#__LENGTH__";

        private readonly Dictionary<int, Parameter> _items = new Dictionary<int, Parameter>();

        protected override void OnNameChanged(string newName)
        {
            // can be invoked by the Parameter constructor => field are not yet initialized

            base.OnNameChanged(newName);

            if (LengthParameter != null)
            {
                LengthParameter.Name = ParameterName.ConstructField(newName, LengthParameterName);
            }

            if (_items != null)
            {
                foreach (var pair in _items)
                {
                    pair.Value.Name = ParameterName.ConstructIndex(newName, pair.Key);
                }
            }
        }

        public bool TryGetItemAt(int index, out Parameter itemParameter)
        {
            //if (TryGetTrait<IndexValueTrait>(t => t.Index == index, out IndexValueTrait field))
            //{
            //    return field.IndexValueParameter;
            //}
            //return null;
            return _items.TryGetValue(index, out itemParameter);
        }

        public void SetItemAt(int index, Parameter parameter)
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

        public Int32Parameter LengthParameter
        {
            get;
        }

        public int? Length
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

        public string ElementTypeName { get; }

        public ArrayParameter(string elementTypeName) : base(elementTypeName + "[]")
        {
            ElementTypeName = elementTypeName;
            LengthParameter = new Int32Parameter();
        }

        public ArrayParameter(string elementTypeName, string name) : base(elementTypeName + "[]", name)
        {
            ElementTypeName = elementTypeName;
            LengthParameter = new Int32Parameter(ParameterName.ConstructField(name, LengthParameterName));
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {

            var dynamicArea = cur.DynamicArea;
            
            if (!IsNull.HasValue || IsNull.Value)
            {
                // dont care or explicit null => return NullReference
                var nullReference = new ObjectReference(0);
                nullReference.SetParameter(this, cur);
                return nullReference;
            }

            var length = Length ?? 0;

            var location = dynamicArea.DeterminePlacement(false);

            ITypeDefOrRef elementType = cur.DefinitionProvider.GetTypeDefinition(ElementTypeName);
            var elementTypeSig = elementType.ToTypeSig();

            var objectReference = dynamicArea.AllocateArray(location, elementType, length);

            var allocatedArray = (AllocatedArray)dynamicArea.Allocations[objectReference];
            allocatedArray.ClearFields(cur);

            if (length > 0)
            {
                for (var i = 0; i < length; ++i)
                {
                    if (!TryGetItemAt(i, out var itemParameter))
                    {
                        itemParameter = ParameterFactory.CreateParameter(elementTypeSig);
                        SetItemAt(i, itemParameter);
                    }
                    var itemDataElement = itemParameter.CreateDataElement(cur);
                    allocatedArray.Fields[i] = itemDataElement;
                }

                //foreach (FieldValueTrait field in Traits.OfType<FieldValueTrait>().Where(t => t.FieldName != LengthParameterName))
                //{
                //    String fieldName = field.FieldName;
                //    Parameter parameter = field.FieldValueParameter;

                //    if (Int32.TryParse(fieldName, out Int32 index) && index < length)
                //    {
                //        IDataElement itemDataElement = parameter.CreateDataElement(cur);
                //        allocatedArray.Fields[index] = itemDataElement;
                //    }
                //}
            }

            objectReference.SetParameter(this, cur);
            return objectReference;
        }

        public override IEnumerable<ParameterExpression> GetParameterExpressions()
        {
            return base.GetParameterExpressions()
                .Concat(LengthParameter.GetParameterExpressions())
                .Concat(_items.Values.SelectMany(itemParameter => itemParameter.GetParameterExpressions()));
        }


        public override bool TryGetChildParameter(string name, out Parameter childParameter)
        {
            if (base.TryGetChildParameter(name, out childParameter)) return true;


            var accessor = ParameterName.GetAccessor(Name, name);
            if (accessor == LengthParameterName)
            {
                childParameter = LengthParameter;
                return true;
            }
            else if (Int32.TryParse(accessor, out var index) && TryGetItemAt(index, out childParameter))
            {
                return true;
            }
            else
            {
                childParameter = null;
                return false;
            }
        }

        //public override String ToString()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendFormat("Name: {0}, ElementType: {1}, IsNull: {2}, Length: {3}", Name, ElementTypeName, IsNull, Length);
            

        //    return sb.ToString();
        //}
    }
}
