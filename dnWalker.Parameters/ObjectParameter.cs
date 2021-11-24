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

        public ObjectParameter(string typeName, string localName, Parameter? owner) : base(typeName, localName, owner)
        {
        }

        private readonly Dictionary<string, Parameter> _fields = new Dictionary<string, Parameter>();

        public override IEnumerable<Parameter> GetOwnedParameters()
        {
            //return base.GetOwnedParameters().Concat(_fields.Values);
            return _fields.Values.Append(IsNullParameter);
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

            fieldValue.Owner = this;
        }

        public void ClearField(string fieldName)
        {
            if (_fields.TryGetValue(fieldName, out Parameter? fieldValue))
            {
                fieldValue.Owner = null;
                _fields.Remove(fieldName);
            }
        }

        public bool TryGetField(string fieldName, [NotNullWhen(true)]out Parameter? fieldValue)
        {
            return _fields.TryGetValue(fieldName, out fieldValue);
        }
    }
}
