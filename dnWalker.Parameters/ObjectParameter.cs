using dnWalker.TypeSystem;

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

        internal ObjectParameter(IParameterSet set, TypeSignature type) : base(set, type)
        {
            _fields = new FieldOwnerImplementation(Reference, set);
            _methodResults = new MethodResolverImplementation(Reference, set);
        }

        internal ObjectParameter(IParameterSet set, TypeSignature type, ParameterRef reference) : base(set, type, reference)
        {
            _fields = new FieldOwnerImplementation(Reference, set);
            _methodResults = new MethodResolverImplementation(Reference, set);
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


        public override ObjectParameter CloneData(IParameterSet newContext)
        {
            ObjectParameter objectParameter = new ObjectParameter(newContext, Type, Reference)
            {
                IsNull = IsNull,
            };

            //foreach (var a in Accessors.Select(ac => ac.Clone()))
            //{
            //    objectParameter.Accessors.Add(a);
            //}

            _fields.CopyTo(objectParameter._fields);
            _methodResults.CopyTo(objectParameter._methodResults);


            return objectParameter;
        }
        public override string ToString()
        {
            return $"ObjectParameter<{Type}>, Reference = {Reference}, IsNull = {IsNull}";
        }
    }

    public static partial class ParameterContextExtensions
    {
        public static IObjectParameter CreateObjectParameter(this IParameterSet set, TypeSignature type, bool? isNull = null)
        {
            return CreateObjectParameter(set, type, set.GetParameterRef(), isNull);
        }

        public static IObjectParameter CreateObjectParameter(this IParameterSet set, TypeSignature type, ParameterRef reference, bool? isNull = null)
        {
            ObjectParameter parameter = new ObjectParameter(set, type, reference)
            {
                IsNull = isNull
            };

            set.Parameters.Add(parameter.Reference, parameter);

            return parameter;
        }
    }
}
