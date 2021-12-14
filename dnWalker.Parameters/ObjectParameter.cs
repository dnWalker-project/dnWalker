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

        public override IParameter ShallowCopy(ParameterStore store, int id)
        {
            ObjectParameter objectParameter = new ObjectParameter(TypeName, id);
            objectParameter.IsNull = IsNull;

            foreach (KeyValuePair<string, IParameter> fieldInfo in GetFields())
            {
                if (fieldInfo.Value is IReferenceTypeParameter refType)
                {
                    objectParameter.SetField(fieldInfo.Key, refType.CreateAlias(store));
                }
                else if (fieldInfo.Value is IPrimitiveValueParameter valueType)
                {
                    objectParameter.SetField(fieldInfo.Key, valueType.ShallowCopy(store));
                }

            }

            foreach (KeyValuePair<MethodSignature, IParameter?[]> methodResultInfo in GetMethodResults())
            {
                IParameter?[] results = methodResultInfo.Value;
                for (int i = 0; i < results.Length; ++i)
                {
                    if (results[i] is IReferenceTypeParameter refType)
                    {
                        objectParameter.SetMethodResult(methodResultInfo.Key, i, refType.CreateAlias(store));
                    }
                    else if (results[i] is IPrimitiveValueParameter valueType)
                    {
                        objectParameter.SetMethodResult(methodResultInfo.Key, i, valueType.ShallowCopy(store));
                    }
                }
            }

            return objectParameter;
        }
    }
}
