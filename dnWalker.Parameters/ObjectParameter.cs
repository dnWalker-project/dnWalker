using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

using Expressions = System.Linq.Expressions;

namespace dnWalker.Parameters
{
    public class ObjectParameter : ReferenceTypeParameter
    {
        public ObjectParameter(string typeName, string localName) : base(typeName, localName)
        {
        }

        public ObjectParameter(string typeName, string localName, Parameter parent) : base(typeName, localName, parent)
        {
        }

        private readonly Dictionary<string, Parameter> _fields = new Dictionary<string, Parameter>();

        public override IEnumerable<Parameter> GetChildren()
        {
            return _fields.Values.Append(IsNullParameter);
        }

        public IEnumerable<KeyValuePair<string, Parameter>> GetKnownFields()
        {
            return IsNull ? Enumerable.Empty<KeyValuePair<string, Parameter>>() : _fields.AsEnumerable();
        }

        public void SetField(string fieldName, Parameter? fieldValue)
        {
            if (String.IsNullOrWhiteSpace(fieldName))
            {
                throw new ArgumentNullException(nameof(fieldName)); 
            }

            if (fieldValue == null)
            {
                ClearField(fieldName);
                return;
            }

            _fields[fieldName] = fieldValue;

            fieldValue.Parent = this;
        }

        public void ClearField(string fieldName)
        {
            if (_fields.TryGetValue(fieldName, out Parameter? fieldValue))
            {
                fieldValue.Parent = null;
                _fields.Remove(fieldName);
            }
        }

        public bool TryGetField(string fieldName, [NotNullWhen(true)]out Parameter? fieldValue)
        {
            return _fields.TryGetValue(fieldName, out fieldValue);
        }

        public override bool TryGetChild(ParameterName parameterName, [NotNullWhen(true)] out Parameter? parameter)
        {
            if (base.TryGetChild(parameterName, out parameter))
            {
                return true;
            }

            if (parameterName.TryGetField(out string? fieldName))
            {
                return TryGetField(fieldName, out parameter);
            }
            return false;
        }
    }
}
