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
        protected void WriteAct(AssertionSchema schema)
        {
            if (schema == null) throw new ArgumentNullException(nameof(schema));

            switch (schema)
            {
                case ExceptionSchema exception:
                    WriteExceptionAct(exception);
                    break;


                case ReturnValueSchema retValue:
                    WriteReturnValueAct(retValue);
                    break;

                case ArrayElementSchema:
                case ObjectFieldSchema:
                    WriteIgnoreReturnAct();
                    break;

                default: throw new ArgumentException($"Unexpected schema type: {schema.GetType().Name}", nameof(schema));
            }
        }

        private void WriteExceptionAct(ExceptionSchema exception)
        {
            // Action act = () => <Instance>.<Method>(<Parameters>);
            Write("Action act = () => ");

            WriteMethodInvocation(GetMethodInvocationData());

            WriteLine(TemplateHelpers.Semicolon);
        }

        private void WriteReturnValueAct(ReturnValueSchema returnValue)
        {
            // TODO: some smart logic process to handle it?
            string resultName = "result";
            Write($"var {resultName} = ");

            WriteMethodInvocation(GetMethodInvocationData());

            WriteLine(TemplateHelpers.Semicolon);
        }

        private void WriteIgnoreReturnAct()
        {
            WriteMethodInvocation(GetMethodInvocationData());
            WriteLine(TemplateHelpers.Semicolon);
        }

        private MethodInvocationData GetMethodInvocationData()
        {
            MethodSignature method = Context.MethodSignature;

            IReadOnlyParameterSet baseSet = Context.BaseSet;

            MethodInvocationData.Builder builder = method.IsStatic ?
                MethodInvocationData.Builder.GetStatic(method) :
                MethodInvocationData.Builder.GetInstance(method, GetVariableName(baseSet.GetThis() ?? throw new Exception("Could not find 'this' parameter.")));

            string[] parameters = method.ParameterNames;

            for (int i = 0; i < parameters.Length; ++i)
            {
                string argName = parameters[i];
                IParameter argParam = baseSet.GetRoot(argName) ?? throw new Exception($"Could not find the '{argName}' argument parameter.");
                // or just builder.Positional(argName); ??
                builder.Positional(GetExpression(argParam));
            }

            return builder.Build();
        }
    }
}
