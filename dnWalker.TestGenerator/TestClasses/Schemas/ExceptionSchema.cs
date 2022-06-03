using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.TestGenerator.Templates;

using System.Collections.Generic;

namespace dnWalker.TestGenerator.TestClasses.Schemas
{
    public class ExceptionSchema : TestSchema
    {
        public ExceptionSchema(ITestClassContext context) : base(context)
        {
            ExceptionType = context.Exception;
        }

        public TypeSig? ExceptionType { get; }

        public override void Write(IWriter output, ITemplateProvider templates)
        {
            TypeSig? exceptionType = ExceptionType;
            IReadOnlyModel inputModel = InputModel;
            IMethod method = Method;

            // TODO: setup the input symbols
            IDictionary<Location, string> locationNames = templates.ArrangeTemplate.WriteArrange(output, inputModel, method);

            templates.ActTemplate.WriteActDelegate(output, method, null, method.Name);

            if (exceptionType == null)
            {
                templates.AssertTemplate.WriteAssertNoExceptionThrown(output, method.Name);
            }
            else
            {
                templates.AssertTemplate.WriteAssertExceptionThrown(output, method.Name, exceptionType);
            }
        }
    }
}
