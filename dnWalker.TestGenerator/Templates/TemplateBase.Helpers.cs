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
        private readonly Dictionary<ParameterRef, string> _variableNameLookupBase = new Dictionary<ParameterRef, string>();
        private readonly Dictionary<ParameterRef, string> _variableNameLookupExec = new Dictionary<ParameterRef, string>();

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
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            if (_currentSchema != null && 
                _currentSchema.TryGetName(parameter, out string name))
            {
                return name;
            }

            //if (ReferenceEquals(parameter.Set, Context.BaseSet) ||
            //    parameter is IStringParameter ||
            //    parameter is IPrimitiveValueParameter)
            if (ReferenceEquals(parameter.Set, Context.BaseSet) || 
                parameter is IPrimitiveValueParameter)
            {
                return GetInVariableName(parameter);
            }
            else if (ReferenceEquals(parameter.Set, Context.ExecutionSet))
            {
                return GetOutVariableName(parameter);
            }
            else
            {
                throw new Exception("THe parameter belongs to an unexpected set.");
            }
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
                name = $"out_{arg.Expression}";
            }
            else
            {
                name = $"var_out_{reference}";
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
