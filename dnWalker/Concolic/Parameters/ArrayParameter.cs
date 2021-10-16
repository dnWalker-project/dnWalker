using dnlib.DotNet;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Parameters
{

    public class ArrayParameter : NullableParameter
    {
        private const String LengthFieldName = "__LENGTH__";

        public ITypeDefOrRef ElementType
        {
            get;
        }

        public Parameter GetItemAt(Int32 index)
        {
            String fieldName = index.ToString();

            if (TryGetTrait<FieldValueTrait>(f => f.FieldName == fieldName, out FieldValueTrait field))
            {
                return field.Value;
            }
            return null;
        }

        public void SetItemAt(Int32 index, Parameter parameter)
        {
            String fieldName = index.ToString();

            if (TryGetTrait<FieldValueTrait>(f => f.FieldName == fieldName, out FieldValueTrait field))
            {
                field.Value = parameter;
            }
            else
            {
                AddTrait<FieldValueTrait>(new FieldValueTrait(ElementType.FullName, fieldName, parameter));
            }
        }

        public Int32? Length
        {
            get
            {
                if (TryGetTrait<FieldValueTrait>(t => t.FieldName == LengthFieldName, out FieldValueTrait lengthField) &&
                    lengthField.Value.TryGetTrait<ValueTrait>(out ValueTrait lenghtValue))
                {
                    if (lenghtValue.Value is Int32 length)
                    {
                        return length;
                    }
                    else
                    {
                        // throw some error?
                    }
                }
                return null;
            }
            set
            {
                if (!value.HasValue)
                {
                    // remove current trait, if such exists
                    if (TryGetTrait<FieldValueTrait>(t => t.FieldName == LengthFieldName, out FieldValueTrait lengthField))
                    {
                        Traits.Remove(lengthField);
                    }
                }
                else
                {
                    // update or add a new trait
                    if (TryGetTrait<FieldValueTrait>(t => t.FieldName == LengthFieldName, out FieldValueTrait lengthField))
                    {
                        lengthField.Value = new Int32Parameter(value.Value);
                    }
                    else
                    {
                        AddTrait(new FieldValueTrait(TypeNames.Int32TypeName, LengthFieldName, new Int32Parameter(value.Value)));
                    }
                }
            }
        }

        public ArrayParameter(ITypeDefOrRef elementType) : base(elementType.FullName + "[]")
        {
            ElementType = elementType;
        }

        public ArrayParameter(ITypeDefOrRef elementType, IEnumerable<ParameterTrait> traits) : base(elementType.FullName + "[]", traits)
        {
            ElementType = elementType;
        }


        public override IDataElement AsDataElement(ExplicitActiveState cur)
        {

            DynamicArea dynamicArea = cur.DynamicArea;
            
            if (!IsNull.HasValue || IsNull.Value)
            {
                // dont care or explicit null => return NullReference
                ObjectReference nullReference = new ObjectReference(0);
                nullReference.SetParameter(this, cur);
                return nullReference;
            }

            Int32 length = Length ?? 0;

            Int32 location = dynamicArea.DeterminePlacement(false);
            ObjectReference objectReference = dynamicArea.AllocateArray(location, ElementType, length);

            AllocatedArray allocatedArray = (AllocatedArray)dynamicArea.Allocations[objectReference];
            allocatedArray.ClearFields(cur);

            if (length > 0)
            {
                foreach (FieldValueTrait field in Traits.OfType<FieldValueTrait>().Where(t => t.FieldName != LengthFieldName))
                {
                    String fieldName = field.FieldName;
                    Parameter parameter = field.Value;

                    if (Int32.TryParse(fieldName, out Int32 index) && index < length)
                    {
                        IDataElement itemDataElement = parameter.AsDataElement(cur);
                        allocatedArray.Fields[index] = itemDataElement;
                    }
                }
            }

            objectReference.SetParameter(this, cur);
            return objectReference;
        }

    }
}
