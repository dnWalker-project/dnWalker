using dnlib.DotNet;

using MMC;
using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Expressions = System.Linq.Expressions;

namespace dnWalker.Concolic.Parameters
{
    public class ObjectParameter : NullableParameter
    {
        public ITypeDefOrRef Type 
        {
            get;
        }
        public ObjectParameter(ITypeDefOrRef type) : base(type.FullName)
        {
            Type = type;
        }

        public ObjectParameter(ITypeDefOrRef type, IEnumerable<ParameterTrait> traits) : base(type.FullName, traits)
        {
            Type = type;
        }

        private static Int32 GetFieldOffset(TypeDef type, String fieldName)
        {
            FieldDef fld = type.FindField(fieldName);
            if (!fld.HasLayoutInfo)
            {
                fld = DefinitionProvider.GetFieldDefinition(fld);
            }


            Int32 typeOffset = 0;
            Boolean matched = false;
            Int32 retval = 0;

            foreach (TypeDef typeDef in DefinitionProvider.InheritanceEnumerator(type))
            {
                /*
				 * We start searching for the right field from the declaringtype,
				 * it is possible that the declaring type does not define fld, therefore
				 * it might be possible that we have to search further for fld in
				 * the inheritance tree, (hence matched), and this continues until
				 * a field is found which has the same offset and the same name 
				 */
                if (typeDef.FullName.Equals(fld.DeclaringType.FullName) || matched)
                {
                    if (fld.FieldOffset < typeDef.Fields.Count
                        && typeDef.Fields[(Int32)fld.FieldOffset].Name.Equals(fld.Name))
                    {
                        retval = (Int32)fld.FieldOffset;
                        break;
                    }

                    matched = true;
                }

                if (typeDef.BaseType != null && typeDef.BaseType.FullName != TypeNames.ObjectTypeName) // if base type is System.Object, stop
                {
                    typeOffset += Math.Max(0, typeDef.Fields.Count - 1);
                }
            }

            retval = typeOffset + retval;

            Debug.Assert(retval >= 0, $"Offset for type {type.FullName} is negative.");

            return retval;
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

            Int32 location = dynamicArea.DeterminePlacement(false);
            ObjectReference objectReference = dynamicArea.AllocateObject(location, Type);
            AllocatedObject allocatedObject = (AllocatedObject)dynamicArea.Allocations[objectReference];
            allocatedObject.ClearFields(cur);

            TypeDef type = Type.ResolveTypeDefThrow();

            foreach (FieldValueTrait fieldValue in Traits.OfType<FieldValueTrait>())
            {
                String fieldName = fieldValue.FieldName;
                Parameter parameter = fieldValue.Value;

                Int32 fieldOffset = GetFieldOffset(type, fieldName);

                IDataElement fieldDataElement = parameter.AsDataElement(cur);
                allocatedObject.Fields[fieldOffset] = fieldDataElement;
            }

            objectReference.SetParameter(this, cur);
            return objectReference;
        }

        public override void SetTraits(IDictionary<String, Object> data, ParameterStore parameterStore)
        {
            if (this.TryGetName(out String name))
            {
                // set everything we can from the "data"
                foreach (KeyValuePair<String, Object> pair in data.Where(p => p.Key.StartsWith(name + "->")))
                {

                }

                // 
            }
        }

    }
}
