using dnlib.DotNet;
using dnWalker.Symbolic;
using dnWalker.TestWriter.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators.Schemas.Exceptions
{
    internal class ExceptionSchema : TestSchema
    {
        public ExceptionSchema(ITestContext testContext) : base(testContext)
        {
        }

        public override void Write(ITestTemplate testTemplate, IWriter output)
        {
            ITestContext context = TestContext;
            TypeSig? exceptionType = context.Iteration.Exception;
            IReadOnlyModel inputModel = context.GetInputModel();
            IMethod method = context.GetMethod();

            // arrange
            testTemplate.WriteArrange(context, output);

            string delegateSymbol = ((string)method.Name).FirstCharToLower();
            testTemplate.WriteActDelegate(context, output, null, delegateSymbol);

            // assert
            if (exceptionType == null)
            {
                testTemplate.WriteAssertNoExceptionThrown(context, output, delegateSymbol);
            }
            else
            {
                testTemplate.WriteAssertExceptionThrown(context, output, delegateSymbol, exceptionType.GetNameOrAlias());
            }
        }
    }
}
