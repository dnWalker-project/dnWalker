using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public class ObjectParameter : ReferenceTypeParameter, IObjectParameter
    {
        private readonly FieldOwnerImplementation _fields;
        private readonly MethodResolverImplementation _methodResults;

        internal ObjectParameter(IParameterContext context, string type) : base(context)
        {
            Type = type;
            _fields = new FieldOwnerImplementation(Reference, context);
            _methodResults = new MethodResolverImplementation(Reference, context);
        }

        internal ObjectParameter(IParameterContext context, ParameterRef reference, string type) : base(context, reference)
        {
            Type = type;
            _fields = new FieldOwnerImplementation(Reference, context);
            _methodResults = new MethodResolverImplementation(Reference, context);
        }


        public string Type
        {
            get;
        }

        #region IFieldOwner Members
        public IReadOnlyDictionary<string, ParameterRef> GetFields()
        {
            return ((IFieldOwner)_fields).GetFields();
        }

        public bool TryGetField(string fieldName, out ParameterRef fieldRef)
        {
            return ((IFieldOwner)_fields).TryGetField(fieldName, out fieldRef);
        }

        public void SetField(string fieldName, ParameterRef fieldRef)
        {
            ((IFieldOwner)_fields).SetField(fieldName, fieldRef);
        }

        public void ClearField(string fieldName)
        {
            ((IFieldOwner)_fields).ClearField(fieldName);
        }
        #endregion IFieldOwner Members

        #region IMethodResolver Members
        public IReadOnlyDictionary<MethodSignature, ParameterRef[]> GetMethodResults()
        {
            return ((IMethodResolver)_methodResults).GetMethodResults();
        }

        public bool TryGetMethodResult(MethodSignature methodSignature, int invocation, out ParameterRef resultRef)
        {
            return ((IMethodResolver)_methodResults).TryGetMethodResult(methodSignature, invocation, out resultRef);
        }

        public void SetMethodResult(MethodSignature methodSignature, int invocation, ParameterRef resultRef)
        {
            ((IMethodResolver)_methodResults).SetMethodResult(methodSignature, invocation, resultRef);
        }

        public void ClearMethodResult(MethodSignature methodSignature, int invocation)
        {
            ((IMethodResolver)_methodResults).ClearMethodResult(methodSignature, invocation);
        }
        #endregion IMethodResolver Members


        public override ObjectParameter Clone(IParameterContext newContext)
        {
            ObjectParameter objectParameter = new ObjectParameter(newContext, Reference, Type)
            {
                IsNull = IsNull,
                Accessor = Accessor?.Clone()
            };

            _fields.CopyTo(objectParameter._fields);
            _methodResults.CopyTo(objectParameter._methodResults);


            objectParameter.Accessor = Accessor?.Clone();

            return objectParameter;
        }
        public override string ToString()
        {
            return $"ObjectParameter<{Type}>, Reference = {Reference}, IsNull = {IsNull}";
        }
    }

    public static partial class ParameterContextExtensions
    {
        public static IObjectParameter CreateObjectParameter(this IParameterContext context, string type, bool? isNull = null)
        {
            return CreateObjectParameter(context, ParameterRef.Any, type, isNull);
        }

        public static IObjectParameter CreateObjectParameter(this IParameterContext context, ParameterRef reference, string type, bool? isNull = null)
        {
            ObjectParameter parameter = new ObjectParameter(context, reference, type)
            {
                IsNull = isNull
            };

            context.Parameters.Add(parameter.Reference, parameter);

            return parameter;
        }
    }
}
