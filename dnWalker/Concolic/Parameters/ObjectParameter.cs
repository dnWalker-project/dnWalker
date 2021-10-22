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
        public ObjectParameter(string typeName) : base(typeName)
        {
        }

        public ObjectParameter(string typeName, string name) : base(typeName, name)
        {

        }

        private readonly Dictionary<string, Parameter> _fields = new Dictionary<string, Parameter>();

        // from Instruction.cs
        private static int GetFieldOffset(TypeDef type, string fieldName)
        {
            var fld = type.FindField(fieldName);
            if (!fld.HasLayoutInfo)
            {
                fld = DefinitionProvider.GetFieldDefinition(fld);
            }


            var typeOffset = 0;
            var matched = false;
            var retval = 0;

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
                        && typeDef.Fields[(int)fld.FieldOffset].Name.Equals(fld.Name))
                    {
                        retval = (int)fld.FieldOffset;
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

        public bool TryGetField(string fieldName, out Parameter fieldParameter)
        {
            return _fields.TryGetValue(fieldName, out fieldParameter);

            //if (TryGetTrait<FieldValueTrait>(t => t.FieldName == fieldName, out FieldValueTrait field))
            //{
            //    return field.FieldValueParameter;
            //}
            //return null;
        }

        public void SetField(string fieldName, Parameter parameter)
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

        protected override void OnNameChanged(string newName)
        {
            base.OnNameChanged(newName);

            if (_fields != null)
            { 
                foreach (var pair in _fields)
                {
                    pair.Value.Name = ParameterName.ConstructField(newName, pair.Key);
                }
            }
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

            var typeDef = cur.DefinitionProvider.GetTypeDefinition(TypeName);

            var location = dynamicArea.DeterminePlacement(false);
            var objectReference = dynamicArea.AllocateObject(location, typeDef);
            var allocatedObject = (AllocatedObject)dynamicArea.Allocations[objectReference];
            allocatedObject.ClearFields(cur);


            foreach ((var fieldType, string fieldName) in DefinitionProvider.InheritanceEnumerator(typeDef)
                                                                                .SelectMany(td => td.ResolveTypeDef().Fields)
                                                                                .Select(f => (f.FieldType, f.Name)))
            {
                // if we the field was not specified by a parameter, we initialize it to default
                if (!TryGetField(fieldName, out var fieldParameter))
                {
                    fieldParameter = ParameterFactory.CreateParameter(fieldType);
                    SetField(fieldName, fieldParameter);
                }

                var fieldOffset = GetFieldOffset(typeDef, fieldName);

                var fieldDataElement = fieldParameter.CreateDataElement(cur);
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

        public override bool TryGetChildParameter(string name, out Parameter childParameter)
        {
            if (base.TryGetChildParameter(name, out childParameter)) return true;

            var accessor = ParameterName.GetAccessor(Name, name);
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
