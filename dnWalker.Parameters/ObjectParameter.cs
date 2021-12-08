using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

using Expressions = System.Linq.Expressions;

namespace dnWalker.Parameters
{
    public class ObjectParameter : MethodResolverParameter, IObjectParameter
    {
        public ObjectParameter(string typeName) : base(typeName)
        {
        }

        public ObjectParameter(string typeName, int id) : base(typeName, id)
        {
        }

        private readonly Dictionary<string, IParameter> _fields = new Dictionary<string, IParameter>();

        public IEnumerable<KeyValuePair<string, IParameter>> GetFields()
        {
            return _fields.AsEnumerable();
        }

        public bool TryGetField(string fieldName, [NotNullWhen(true)] out IParameter? parameter)
        {
            return _fields.TryGetValue(fieldName, out parameter);
        }

        public void SetField(string fieldName, IParameter? parameter)
        {
            ClearField(fieldName);

            if (parameter != null)
            {
                _fields[fieldName] = parameter;
                parameter.Accessor = new FieldParameterAccessor(fieldName, this);
            }
        }

        public void ClearField(string fieldName)
        {
            if (_fields.TryGetValue(fieldName, out IParameter? parameter))
            {
                _fields.Remove(fieldName);
                parameter.Accessor = null;
            }
        }

        public override IEnumerable<IParameter> GetChildren()
        {
            return Enumerable.Concat
                   (
                        GetMethodResults().SelectMany(mr => mr.Value).Where(mr => mr != null).Select(mr => mr!),
                        GetFields().Select(f => f.Value)
                   );
        }
    }
}
