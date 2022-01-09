using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public class StructParameter : Parameter, IFieldOwner, IStructParameter
    {
        private readonly FieldOwnerImplementation _fields;

        internal StructParameter(IParameterContext context, string type) : base(context)
        {
            _fields = new FieldOwnerImplementation(Reference, Context);
            Type = type;
        }

        internal StructParameter(IParameterContext context, ParameterRef reference, string type) : base(context, reference)
        {
            _fields = new FieldOwnerImplementation(Reference, Context);
            Type = type;
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

        public string Type
        {
            get;
        }

        public override StructParameter CloneData(IParameterContext newContext)
        {
            StructParameter structParameter = new StructParameter(newContext, Reference, Type);
            _fields.CopyTo(structParameter._fields);

            //structParameter.Accessor = Accessor?.Clone();

            //foreach (var a in Accessors.Select(ac => ac.Clone()))
            //{
            //    structParameter.Accessors.Add(a);
            //}

            return structParameter;
        }

        public override string ToString()
        {
            return $"StructParameter<{Type}>, Reference = {Reference}";
        }
    }

    public static partial class ParameterContextExtensions
    {
        public static IStructParameter CreateStructParameter(this IParameterContext context, string type)
        {
            return CreateStructParameter(context, ParameterRef.Any, type);
        }

        public static IStructParameter CreateStructParameter(this IParameterContext context, ParameterRef reference, string type)
        {
            StructParameter parameter = new StructParameter(context, reference, type);

            context.Parameters.Add(parameter.Reference, parameter);

            return parameter;
        }
    }
}
