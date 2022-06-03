
using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.TestGenerator.Templates;

using System;
using System.Collections.Generic;

namespace dnWalker.TestGenerator.TestClasses.Schemas
{
    public class ReturnValueSchema : TestSchema
    {
        public ReturnValueSchema(ITestClassContext context) : base(context)
        {
        }

        public override void Write(IWriter output, ITemplateProvider templates)
        {
            IReadOnlyModel inputModel = InputModel;
            IReadOnlyModel outputModel = OutputModel;
            IMethod method = Method;

            if (!outputModel.TryGetReturnValue(method, out IValue? expected)) throw new Exception("The model does not contain the return value.");

            // arrange
            IDictionary<Location, string> locationNames =  templates.ArrangeTemplate.WriteArrange(output, inputModel, method);

            // act
            // TODO: do not hardcode the instance and return value symbols!
            templates.ActTemplate.WriteAct(output, method, "objectUnderTest", "result");

            // assert
            // based on the return value

            TypeSig retType = method.MethodSig.RetType;
            string expectedLiteral = expected.GetLiteral(locationNames);
            if (retType.IsPrimitive || retType.IsString())
            {
                // we can assert equality
                templates.AssertTemplate.WriteAssertEqual(output, "result", expectedLiteral);
            }
            else
            {
                // reference type - a bit more complex...
                Location expectedLocation = (Location)expected;
                // we may assert:
                // - is null
                if (expectedLocation == Location.Null)
                {
                    templates.AssertTemplate.WriteAssertNull(output, "result");
                    return;
                }
                templates.AssertTemplate.WriteAssertNotNull(output, "result");

                // - identity to some already arranged object
                if (inputModel.HeapInfo.TryGetNode(expectedLocation, out _))
                {
                    templates.AssertTemplate.WriteAssertSame(output, "result", expectedLiteral);
                }
                // TODO: - the object is a fresh one - we may assert equivalency => we need to arrange part of the output model...
            }
        }
    }
}
