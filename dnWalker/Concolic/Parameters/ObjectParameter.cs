//using dnlib.DotNet;

//using MMC;
//using MMC.Data;
//using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

//using Expressions = System.Linq.Expressions;

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

        public IEnumerable<KeyValuePair<String, Parameter>> GetKnownFields()
        {
            return _fields;
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

        //public override IDataElement CreateDataElement(ExplicitActiveState cur)
        //{
        //    DynamicArea dynamicArea = cur.DynamicArea;

            

        //    if (!IsNull.HasValue || IsNull.Value)
        //    {
        //        // dont care or explicit null => return NullReference
        //        ObjectReference nullReference = new ObjectReference(0);
        //        nullReference.SetParameter(this, cur);
        //        return nullReference;
        //    }

        //    TypeDef typeDef = cur.DefinitionProvider.GetTypeDefinition(TypeName);

        //    Int32 location = dynamicArea.DeterminePlacement(false);
        //    ObjectReference objectReference = dynamicArea.AllocateObject(location, typeDef);
        //    AllocatedObject allocatedObject = (AllocatedObject)dynamicArea.Allocations[objectReference];
        //    allocatedObject.ClearFields(cur);


        //    foreach ((TypeSig fieldType, String fieldName) in DefinitionProvider.InheritanceEnumerator(typeDef)
        //                                                                        .SelectMany(td => td.ResolveTypeDef().Fields)
        //                                                                        .Select(f => (f.FieldType, f.Name)))
        //    {
        //        if (!TryGetField(fieldName, out Parameter fieldParameter))
        //        {
        //            fieldParameter = ParameterFactory.CreateParameter(fieldType);
        //            SetField(fieldName, fieldParameter);
        //        }

        //        Int32 fieldOffset = GetFieldOffset(typeDef, fieldName);

        //        IDataElement fieldDataElement = fieldParameter.CreateDataElement(cur);
        //        allocatedObject.Fields[fieldOffset] = fieldDataElement;
        //    }

        //    //foreach (FieldValueTrait fieldValue in Traits.OfType<FieldValueTrait>())
        //    //{
        //    //    String fieldName = fieldValue.FieldName;
        //    //    Parameter parameter = fieldValue.FieldValueParameter;

        //    //    Int32 fieldOffset = GetFieldOffset(type, fieldName);

        //    //    IDataElement fieldDataElement = parameter.CreateDataElement(cur);
        //    //    allocatedObject.Fields[fieldOffset] = fieldDataElement;
        //    //}

        //    objectReference.SetParameter(this, cur);
        //    return objectReference;
        //}

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
