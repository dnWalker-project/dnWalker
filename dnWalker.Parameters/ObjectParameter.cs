using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

using Expressions = System.Linq.Expressions;

namespace dnWalker.Parameters
{
    public class ObjectParameter : ReferenceTypeParameter, IEquatable<ObjectParameter>
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

        /// <summary>
        /// Invoked when the parameter name changes. Updates names of the <see cref="ReferenceTypeParameter.IsNullParameter"/> and fields parameters.
        /// </summary>
        /// <param name="newName"></param>
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

        public override IEnumerable<Parameter> GetChildrenParameters()
        {
            return _fields.Values.Append(IsNullParameter);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ObjectParameter);
        }

        public bool Equals(ObjectParameter other)
        {
            bool isNull = IsNull.HasValue ? IsNull.Value : true;

            return other != null &&
                   Name == other.Name &&
                   IsNull == other.IsNull &&
                   TypeName == other.TypeName &&
                   (isNull || _fields.Count == other._fields.Count) &&
                   (isNull || _fields.All(p => other._fields.TryGetValue(p.Key, out Parameter otherField) && Equals(p.Value, otherField)));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, TypeName, IsNull, _fields);
        }

        public static bool operator ==(ObjectParameter left, ObjectParameter right)
        {
            return EqualityComparer<ObjectParameter>.Default.Equals(left, right);
        }

        public static bool operator !=(ObjectParameter left, ObjectParameter right)
        {
            return !(left == right);
        }

    }
}
