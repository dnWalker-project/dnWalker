using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    internal class FieldOwnerImplementation : IFieldOwner
    {
        private readonly ParameterRef _ownerRef;
        private readonly IParameterContext _context;

        private readonly Dictionary<string, ParameterRef> _fields = new Dictionary<string, ParameterRef>();

        public FieldOwnerImplementation(ParameterRef ownerRef, IParameterContext context)
        {
            _ownerRef = ownerRef;
            _context = context;
        }

        public IReadOnlyDictionary<string, ParameterRef> GetFields()
        {
            return _fields;
        }

        public bool TryGetField(string fieldName, out ParameterRef fieldRef)
        {
            return _fields.TryGetValue(fieldName, out fieldRef) && fieldRef != ParameterRef.Empty;
        }

        public void SetField(string fieldName, ParameterRef fieldRef)
        {
            if (_fields.ContainsKey(fieldName)) ClearField(fieldName);

            _fields[fieldName] = fieldRef;

            if (fieldRef.TryResolve(_context, out IParameter? p))
            {
                p.Accessor = new FieldParameterAccessor(fieldName, _ownerRef);
            }
            else
            {
                // throw new Exception("Trying to set field with an unknown parameter!");
            }
        }

        public void ClearField(string fieldName)
        {
            if (_fields.TryGetValue(fieldName, out ParameterRef fieldRef) &&
                fieldRef.TryResolve(_context, out IParameter? fieldParameter))
            {
                fieldParameter.Accessor = null;
            }
            _fields[fieldName] = ParameterRef.Empty;
        }

        public void CopyTo(FieldOwnerImplementation other)
        {
            foreach (var field in _fields)
            {
                other._fields.Add(field.Key, field.Value);
            }
        }
    }
}
