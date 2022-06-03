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

            // arrange
            IDictionary<Location, string> locationNames = templates.ArrangeTemplate.WriteArrange(output, inputModel, method);
 
            string delegateSymbol = ((string)method.Name).FirstCharToLower();

            // act
            // TODO: pass this name by some other means, in order to avoid collision!!!
            templates.ActTemplate.WriteActDelegate(output, method, "objectUnderTest", null, delegateSymbol);

            // assert
            if (exceptionType == null)
            {
                templates.AssertTemplate.WriteAssertNoExceptionThrown(output, delegateSymbol);
            }
            else
            {
                templates.AssertTemplate.WriteAssertExceptionThrown(output, delegateSymbol, exceptionType);
            }
        }
    }
}
