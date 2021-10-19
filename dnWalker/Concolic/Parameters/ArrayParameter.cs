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
        public const String LengthParameterName = "#__LENGTH__";

        public TypeSig ElementType
        {
            get;
        }

        public Parameter GetItemAt(Int32 index)
        {
            if (TryGetTrait<IndexValueTrait>(t => t.Index == index, out IndexValueTrait field))
            {
                return field.Value;
            }
            return null;
        }

        public void SetItemAt(Int32 index, Parameter parameter)
        {
            if (TryGetTrait<IndexValueTrait>(t => t.Index == index, out IndexValueTrait indexField))
            {
                indexField.Value = parameter;
            }
            else
            {
                AddTrait<IndexValueTrait>(new IndexValueTrait(index, parameter));
            }
        }

        public Int32? Length
        {
            get
            {
                if (TryGetTrait<LengthTrait>(out LengthTrait lengthField))
                {
                    return lengthField.Value.Value;
                }
                return null;
            }
        }

        public ArrayParameter(TypeSig elementType) : base(elementType.FullName + "[]")
        {
            ElementType = elementType;
        }

        public ArrayParameter(TypeSig elementType, IEnumerable<ParameterTrait> traits) : base(elementType.FullName + "[]", traits)
        {
            ElementType = elementType;
        }


        public override IDataElement CreateDataElement(ExplicitActiveState cur)
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
            ObjectReference objectReference = dynamicArea.AllocateArray(location, ElementType.ToTypeDefOrRef(), length);

            AllocatedArray allocatedArray = (AllocatedArray)dynamicArea.Allocations[objectReference];
            allocatedArray.ClearFields(cur);

            if (length > 0)
            {
                foreach (FieldValueTrait field in Traits.OfType<FieldValueTrait>().Where(t => t.FieldName != LengthParameterName))
                {
                    String fieldName = field.FieldName;
                    Parameter parameter = field.Value;

                    if (Int32.TryParse(fieldName, out Int32 index) && index < length)
                    {
                        IDataElement itemDataElement = parameter.CreateDataElement(cur);
                        allocatedArray.Fields[index] = itemDataElement;
                    }
                }
            }

            objectReference.SetParameter(this, cur);
            return objectReference;
        }

    }
}
