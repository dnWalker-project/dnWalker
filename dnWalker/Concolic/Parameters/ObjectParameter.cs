using dnlib.DotNet;

using MMC;
using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

using Expressions = System.Linq.Expressions;

namespace dnWalker.Concolic.Parameters
{
    public class ObjectParameter : ReferenceTypeParameter
    {
        public ObjectParameter(String typeName) : base(typeName)
        {
        }

        public ObjectParameter(String typeName, String name) : base(typeName, name)
        {

        }

        private readonly Dictionary<String, Parameter> _fields = new Dictionary<String, Parameter>();

        // from Instruction.cs
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

        public Boolean TryGetField(String fieldName, out Parameter fieldParameter)
        {
            return _fields.TryGetValue(fieldName, out fieldParameter);

            //if (TryGetTrait<FieldValueTrait>(t => t.FieldName == fieldName, out FieldValueTrait field))
            //{
            //    return field.FieldValueParameter;
            //}
            //return null;
        }

        public void SetField(String fieldName, Parameter parameter)
        {
            _fields[fieldName] = parameter;
            if (HasName()) parameter.Name = ParameterName.ConstructField(Name, fieldName);

            //if (TryGetTrait<FieldValueTrait>(t => t.FieldName == fieldName, out FieldValueTrait field))
            //{
            //    field.FieldValueParameter = parameter;
            //}
            //else
            //{
            //    field = new FieldValueTrait(fieldName, parameter);
            //}
            //parameter.Name = ParameterName.ConstructField(Name, fieldName);
        }

        protected override void OnNameChanged(String newName)
        {
            base.OnNameChanged(newName);

            if (_fields != null)
            {
                foreach (KeyValuePair<String, Parameter> pair in _fields)
                {
                    pair.Value.Name = ParameterName.ConstructField(newName, pair.Key);
                }
            }
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

            TypeDef typeDef = cur.DefinitionProvider.GetTypeDefinition(TypeName);

            Int32 location = dynamicArea.DeterminePlacement(false);
            ObjectReference objectReference = dynamicArea.AllocateObject(location, typeDef);
            AllocatedObject allocatedObject = (AllocatedObject)dynamicArea.Allocations[objectReference];
            allocatedObject.ClearFields(cur);


            foreach ((TypeSig fieldType, String fieldName) in DefinitionProvider.InheritanceEnumerator(typeDef)
                                                                                .SelectMany(td => td.ResolveTypeDef().Fields)
                                                                                .Select(f => (f.FieldType, f.Name)))
            {
                if (!TryGetField(fieldName, out Parameter fieldParameter))
                {
                    fieldParameter = ParameterFactory.CreateParameter(fieldType);
                    SetField(fieldName, fieldParameter);
                }

                Int32 fieldOffset = GetFieldOffset(typeDef, fieldName);

                IDataElement fieldDataElement = fieldParameter.CreateDataElement(cur);
                allocatedObject.Fields[fieldOffset] = fieldDataElement;
            }

            //foreach (FieldValueTrait fieldValue in Traits.OfType<FieldValueTrait>())
            //{
            //    String fieldName = fieldValue.FieldName;
            //    Parameter parameter = fieldValue.FieldValueParameter;

            //    Int32 fieldOffset = GetFieldOffset(type, fieldName);

            //    IDataElement fieldDataElement = parameter.CreateDataElement(cur);
            //    allocatedObject.Fields[fieldOffset] = fieldDataElement;
            //}

            objectReference.SetParameter(this, cur);
            return objectReference;
        }

        public override IEnumerable<ParameterExpression> GetParameterExpressions()
        {
            return base.GetParameterExpressions()
                .Concat(_fields.Values.SelectMany(fieldParameter => fieldParameter.GetParameterExpressions()));
        }

        public override Boolean TryGetChildParameter(String name, out Parameter childParameter)
        {
            if (base.TryGetChildParameter(name, out childParameter)) return true;

            String accessor = ParameterName.GetAccessor(Name, name);
            if (TryGetField(accessor, out childParameter))
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
