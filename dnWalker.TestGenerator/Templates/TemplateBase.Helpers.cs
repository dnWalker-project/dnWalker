using dnlib.DotNet;

using dnWalker.Parameters;
using dnWalker.TestGenerator.TestClasses;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public partial class TemplateBase
    {

        private readonly Dictionary<ParameterRef, TypeSignature> _variableTypeLookup = new Dictionary<ParameterRef, TypeSignature>();
        private readonly Dictionary<ParameterRef, string> _variableNameLookupBase = new Dictionary<ParameterRef, string>();
        private readonly Dictionary<ParameterRef, string> _variableNameLookupExec = new Dictionary<ParameterRef, string>();

        private readonly Dictionary<TypeSignature, int> _typeCounter = new Dictionary<TypeSignature, int>();

        private ITestClassContext? _context = null;
        protected ITestClassContext Context
        {
            get
            {
                return _context ?? throw new InvalidOperationException("The template is not initialized.");
            }
        }

        private AssertionSchema? _currentSchema = null;

        protected void BeginSchema(AssertionSchema schema)
        {
            _currentSchema = schema;
        }

        protected void EndSchema()
        {
            _currentSchema = null;
        }

        protected AssertionSchema? CurrentSchema
        {
            get { return _currentSchema; }
        }

        protected void Initialize(ITestClassContext context)
        {
            _context = context;

            GenerationEnvironment.Clear();
            _variableTypeLookup.Clear();
            _variableNameLookupBase.Clear();
            _variableNameLookupExec.Clear();
            _typeCounter.Clear();
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

        private bool TryGetCachedVariableName(IParameter parameter, [NotNullWhen(true)]out string? name)
        {
            name = null;
            return parameter is IPrimitiveValueParameter && _variableNameLookupBase.TryGetValue(parameter.Reference, out name);
        }

        private string GetTypeAsVariableName(TypeSignature signature)
        {
            string typeNameOrAlias = TemplateHelpers.GetTypeNameOrAlias(signature);
            if (char.IsUpper(typeNameOrAlias[0]))
            {
                typeNameOrAlias = char.ToLower(typeNameOrAlias[0]) + typeNameOrAlias.Substring(1);
            }
            return typeNameOrAlias;
        }

        private string GetTypeVariableName(TypeSignature signature)
        {
            string typeString = GetTypeAsVariableName(signature);

            if (_typeCounter.TryGetValue(signature, out int value))
            {
                _typeCounter[signature] = value + 1;
                return typeString + (value + 1);
            }
            else
            {
                _typeCounter[signature] = 0;
                return typeString + 0;
            }
        }

        private string GetActVariableName(IParameter parameter)
        {
            ParameterRef reference = parameter.Reference;

            if (_variableNameLookupBase.TryGetValue(reference, out string? name)) return name; //already cached

            if (parameter.IsRoot(out MethodArgumentParameterAccessor[] argAccessor))
            {
                // 1.1) direct method argument => argument name (the first one if multiple)
                name = argAccessor[0].Expression;
            }
            else
            {
                // 1.2) indirect method argument (i.g. method result, field/element value etc) => <typename><cnt>
                name = GetTypeVariableName(GetVariableType(parameter));
            }

            _variableNameLookupBase[reference] = name;
            return name;
        }

        private string GetAssertVariableName(IParameter parameter)
        {
            ParameterRef reference = parameter.Reference;

            if (_variableNameLookupExec.TryGetValue(reference, out string? name)) return name; //already cached

            if (parameter.IsRoot(out MethodArgumentParameterAccessor[] argAccessor))
            {
                // 2.0) the parameter is actually a method argument
                name = argAccessor[0].Expression;
            }
            else if (_currentSchema != null && _currentSchema.TryGetName(parameter, out name))
            {
                // 2.1) based on the assertion schema => handles the distinction between expected field, element, return value etc
            }
            else
            {
                // 2.2) <typename><cnt>
                name = GetTypeVariableName(GetVariableType(parameter));
            }

            _variableNameLookupExec[reference] = name;
            return name;
        }

        protected string GetVariableName(IParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            string? name = null;

            // scenarios:
            // 1) naming a variable used for the ACT
            // 2) naming a variable used for the ASSERT

            if (ReferenceEquals(parameter.Set, Context.BaseSet))
            {
                // base set
                name = GetActVariableName(parameter);
            }
            else
            {
                // execution set
                name = GetAssertVariableName(parameter);
            }
            return name;


            // if (TryGetCachedVariableName(parameter, out name)) return name;

            // if (_currentSchema != null && 
            //     _currentSchema.TryGetName(parameter, out name))
            // {
            //     return name;
            // }

            // //if (ReferenceEquals(parameter.Set, Context.BaseSet) ||
            // //    parameter is IStringParameter ||
            // //    parameter is IPrimitiveValueParameter)
            // if (ReferenceEquals(parameter.Set, Context.BaseSet))
            // {
            //     return GetInVariableName(parameter);
            // }
            // else if (ReferenceEquals(parameter.Set, Context.ExecutionSet))
            // {
            //     return GetOutVariableName(parameter);
            // }
            // else
            // {
            //     throw new Exception("THe parameter belongs to an unexpected set.");
            // }
        }

        private static void GetAccessors(IParameter parameter, out ReturnValueParameterAccessor? retVal, out MethodArgumentParameterAccessor? arg)
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
                    //break;
                }
            }
        }
        private string GetInVariableName(IParameter parameter)
        {
            ParameterRef reference = parameter.Reference;

            if (_variableNameLookupBase.TryGetValue(reference, out string? name))
            {
                return name;
            }

            // do the name guessing
            // 1) method argument => the name of the argument
            // 2) return value => result
            GetAccessors(parameter, out var retValue, out var arg);

            if (arg != null)
            {
                name = arg.Expression;
            }
            //else if (retValue != null)
            //{
            //    // TODO: change it...
            //    name = "result";
            //}
            else
            {
                name = $"var_{reference}";
            }

            _variableNameLookupBase[reference] = name;

            return name;
        }

        private string GetOutVariableName(IParameter parameter)
        {
            ParameterRef reference = parameter.Reference;

            if (_variableNameLookupExec.TryGetValue(reference, out string? name))
            {
                return name;
            }

            // do the name guessing
            // 1) method argument => the name of the argument
            // 2) return value => result
            GetAccessors(parameter, out var retValue, out var arg);

            if (arg != null)
            {
                if (parameter is IPrimitiveValueParameter)
                {
                    name = arg.Expression;
                }
                else
                {
                    name = $"out_{arg.Expression}";
                }
            }
            else
            {
                if (parameter is IPrimitiveValueParameter)
                {
                    name = $"var_{reference}";
                }
                else
                {
                    name = $"var_out_{reference}";
                }
            }

            _variableNameLookupExec[reference] = name;

            return name;
        }

        protected string GetExpression(IParameter parameter)
        {
            if (Context.Configuration.PreferLiteralsOverVariables)
            {
                if (parameter is IPrimitiveValueParameter primitiveValue)
                {
                    return primitiveValue.Value?.ToString() ?? TemplateHelpers.GetDefaultLiteral(parameter.Type);
                }
                else if (parameter is IReferenceTypeParameter rp && rp.GetIsNull())
                {
                    return TemplateHelpers.Null;
                }
            }

            return GetVariableName(parameter);
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

        protected void PushIndent()
        {
            base.PushIndent(TemplateHelpers.Indent);
        }
    }
}
