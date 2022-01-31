using dnlib.DotNet;

using dnWalker.Parameters;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public partial class TemplateBase
    {

        protected Dictionary<ParameterRef, TypeSignature> VariableTypeLookup { get; } = new Dictionary<ParameterRef, TypeSignature>();
        protected Dictionary<ParameterRef, string> VariableNameLookup { get; } = new Dictionary<ParameterRef, string>();
        protected TestGenerationContext? Context { get; set; }

        public void Initialize(TestGenerationContext context)
        {
            Context = context;

            VariableTypeLookup.Clear();
            VariableNameLookup.Clear();
        }

        protected TypeSignature GetVariableType(IParameter parameter)
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            ParameterRef reference = parameter.Reference;

            if (VariableTypeLookup.TryGetValue(reference, out TypeSignature type))
            {
                return type;
            }

            if (parameter is IPrimitiveValueParameter)
            {
                type = parameter.Type;
            }
            else if (parameter is IObjectParameter)
            {
                type = parameter.Type;
            }
            else if (parameter is IArrayParameter)
            {
                type = parameter.Type;
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

        protected string GetExpression(IParameter parameter)
        {
            if (parameter is IPrimitiveValueParameter primitiveValue)
            {
                return primitiveValue.Value?.ToString() ?? TemplateHelpers.GetDefaultLiteral(parameter.Type);
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

        protected void WriteJoined<T>(string separator, IEnumerable<T> items, Action<T> writeAction)
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
