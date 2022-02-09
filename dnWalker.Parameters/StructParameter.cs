using dnWalker.TypeSystem;

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

        internal StructParameter(IParameterSet set, TypeSignature type) : base(set, type)
        {
            _fields = new FieldOwnerImplementation(Reference, Set);
        }

        internal StructParameter(IParameterSet set, TypeSignature type, ParameterRef reference) : base(set, type, reference)
        {
            _fields = new FieldOwnerImplementation(Reference, Set);
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

        public override StructParameter CloneData(IParameterSet newContext)
        {
            StructParameter structParameter = new StructParameter(newContext, Type, Reference);
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
        public static IStructParameter CreateStructParameter(this IParameterSet context, TypeSignature type)
        {
            return CreateStructParameter(context, type, context.GetParameterRef());
        }

        public static IStructParameter CreateStructParameter(this IParameterSet context, TypeSignature type, ParameterRef reference)
        {
            StructParameter parameter = new StructParameter(context, type, reference);

            context.Parameters.Add(parameter.Reference, parameter);

            return parameter;
        }
    }
}
