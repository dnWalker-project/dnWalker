using dnlib.DotNet;

using dnWalker.Parameters;
using dnWalker.TestGenerator.TestClasses;
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

        private readonly Dictionary<ParameterRef, TypeSignature> _variableTypeLookup = new Dictionary<ParameterRef, TypeSignature>();
        private readonly Dictionary<ParameterRef, string> _variableNameLookup = new Dictionary<ParameterRef, string>();
        protected TestClassContext? Context { get; set; }

        protected void Initialize(TestClassContext context)
        {
            Context = context;

            _variableTypeLookup.Clear();
            _variableNameLookup.Clear();
        }

        protected TypeSignature GetVariableType(IParameter parameter)
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            ParameterRef reference = parameter.Reference;

            if (_variableTypeLookup.TryGetValue(reference, out TypeSignature type))
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

            _variableTypeLookup[reference] = type;
            return type;
        }

        protected string GetVariableName(IParameter parameter)
        {
            void GetAccessors(out ReturnValueParameterAccessor? retVal, out MethodArgumentParameterAccessor? arg)
            {
                retVal = null;
                arg = null;
                foreach (ParameterAccessor acc in parameter.Accessors)
                {
                    if (acc is ReturnValueParameterAccessor rv)
                    {
                        retVal = rv;
                    }
                    if (acc is MethodArgumentParameterAccessor a)
                    {
                        arg = a;
                        break;
                    }
                }
            }

            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            ParameterRef reference = parameter.Reference;

            if (_variableNameLookup.TryGetValue(reference, out string? name))
            {
                return name;
            }

            // do the name guessing
            // 1) method argument => the name of the argument
            // 2) return value => result
            GetAccessors(out var retValue, out var arg);

            if (arg != null)
            {
                name = arg.Expression;
            }
            else if (retValue != null)
            {
                // TODO: change it...
                name = "result";
            }
            else
            {
                name = $"var_{reference}";
            }

            _variableNameLookup[reference] = name;

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
            T[] itemsArray = items.ToArray();

            if (itemsArray.Length == 0) return;

            {
                T item = itemsArray[0];
                writeAction(item);
            }

            for (int i = 1; i < itemsArray.Length; ++i)
            {
                Write(separator);
                writeAction(itemsArray[i]);
            }
        }
    }
}
