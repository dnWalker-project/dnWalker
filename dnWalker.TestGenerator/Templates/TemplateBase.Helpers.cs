using dnWalker.Parameters;
using dnWalker.TestGenerator.Reflection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public partial class TemplateBase
    {
        protected Dictionary<ParameterRef, Type> VariableTypeLookup { get; } = new Dictionary<ParameterRef, Type>();
        protected Dictionary<ParameterRef, string> VariableNameLookup { get; } = new Dictionary<ParameterRef, string>();

        public void Initialize()
        {
            VariableTypeLookup.Clear();
            VariableNameLookup.Clear();
        }

        protected Type GetVariableType(IParameter parameter)
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            ParameterRef reference = parameter.Reference;

            if (VariableTypeLookup.TryGetValue(reference, out Type? type))
            {
                return type;
            }

            if (parameter is IPrimitiveValueParameter primitive)
            {
                type = AppDomain.CurrentDomain.GetType(primitive.Type) ?? throw new Exception("Could not find the type!");
            }
            else if (parameter is IObjectParameter obj)
            {
                type = AppDomain.CurrentDomain.GetType(obj.Type) ?? throw new Exception("Could not find the type!");
            }
            else if (parameter is IArrayParameter arr)
            {
                type = AppDomain.CurrentDomain.GetType(arr.ElementType)?.MakeArrayType() ?? throw new Exception("Could not find the type!");
            }

            else
            {
                throw new Exception("Unexpected parameter type!");
            }

            VariableTypeLookup[reference] = type;
            return type;
        }

        protected string GetVariableName(IParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            ParameterRef reference = parameter.Reference;

            if (VariableNameLookup.TryGetValue(reference, out string? name))
            {
                return name;
            }

            // do the name guessing


            name = $"var_{reference}";

            VariableNameLookup[reference] = name;

            return name;
        }

        protected string GetExpression(IParameter parameter, Type type)
        {
            if (parameter is IPrimitiveValueParameter primitiveValue)
            {
                return primitiveValue.Value?.ToString() ?? TemplateHelpers.GetDefaultLiteral(type);
            }
            else if(parameter is IReferenceTypeParameter rp && rp.GetIsNull())
            {
                return TemplateHelpers.Null;
            }
            else
            {
                return GetVariableName(parameter);
            }
        }

        protected void WriteJoint<T>(string separator, IEnumerable<T> items, Action<T> writeAction)
        {
            {
                T? item = items.FirstOrDefault();
                if (item == null) return;
                writeAction(item);
            }

            foreach (T item in items.Skip(1))
            {
                Write(separator);
                writeAction(item);
            }
        }
    }
}
