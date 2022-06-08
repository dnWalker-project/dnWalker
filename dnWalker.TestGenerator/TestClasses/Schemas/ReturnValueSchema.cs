
using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;
using dnWalker.TestGenerator.Templates;

using System;
using System.Collections.Generic;
using System.Diagnostics;

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

                bool isFresh = true;

                // - identity to some already arranged object
                if (inputModel.HeapInfo.TryGetNode(expectedLocation, out _))
                {
                    templates.AssertTemplate.WriteAssertSame(output, "result", expectedLiteral);
                    isFresh = false;
                }
                // TODO: - the object is a fresh one - we may assert equivalence => we need to arrange part of the output model...

                IReadOnlyHeapNode resultNode = outputModel.HeapInfo.GetNode(expectedLocation);

                switch (resultNode)
                {
                    case IReadOnlyObjectHeapNode objResult: WriteAssertObjectTraits(output, templates, objResult, "result", locationNames); break;
                    case IReadOnlyArrayHeapNode arrResult: WriteAssertArrayTraits(output, templates, arrResult, "result", locationNames, !isFresh); break;
                }
            }
        }

        private static void WriteAssertArrayTraits(IWriter output, ITemplateProvider templates, IReadOnlyArrayHeapNode arrResult, string resultLiteral, IDictionary<Location, string> locationNames, bool isInput)
        {
            if (!isInput)
            {
                // only for fresh variable
                templates.AssertTemplate.WriteAssertLength(output, resultLiteral, arrResult.Length.ToString());
            }

            foreach (int index in arrResult.Indeces)
            {
                IValue elementValue = arrResult.GetElement(index);
                string selectorLiteral = $"{resultLiteral}[{index}]";
                
                if (elementValue is Location fldLoc)
                {
                    if (locationNames.TryGetValue(fldLoc, out string? fldLiteral))
                    {
                        // input argument, assert same as
                        templates.AssertTemplate.WriteAssertSame(output, selectorLiteral, fldLiteral);
                    }
                    else
                    {
                        // fresh value, we could arrange entire heap graph and do equivalence check - way too complex...
                        // assert not null
                        templates.AssertTemplate.WriteAssertNotNull(output, selectorLiteral);
                    }
                }
                else
                {
                    string elementLiteral = elementValue.GetLiteral(locationNames);
                    templates.AssertTemplate.WriteAssertEqual(output, selectorLiteral, elementLiteral);
                }
            }
        }

        private static void WriteAssertObjectTraits(IWriter output, ITemplateProvider templates, IReadOnlyObjectHeapNode objResult, string resultLiteral, IDictionary<Location, string> locationNames)
        {
            foreach (IField fld in objResult.Fields)
            {
                IValue fldValue = objResult.GetField(fld);

                FieldDef fldDef = fld.ResolveFieldDefThrow();
                string selectorLiteral;
                if (fldDef.IsPublic)
                {
                    selectorLiteral = $"{resultLiteral}.{fld.Name}";
                }
                else
                {
                    // TODO: write type...
                    throw new InvalidOperationException("Cannot assert private fields for now. Too lazy to solve type string writing...");
                }


                if (fldValue is Location fldLoc)
                {
                    if (locationNames.TryGetValue(fldLoc, out string? fldLiteral))
                    {
                        // input argument, assert same as
                        templates.AssertTemplate.WriteAssertSame(output, selectorLiteral, fldLiteral);
                    }
                    else
                    {
                        // fresh value, we could arrange entire heap graph and do equivalence check - way too complex...
                        // assert not null
                        templates.AssertTemplate.WriteAssertNotNull(output, selectorLiteral);
                    }
                }
                else
                {
                    string fldLiteral = fldValue.GetLiteral(locationNames);
                    templates.AssertTemplate.WriteAssertEqual(output, selectorLiteral, fldLiteral);
                }
            }
        }
    }
}
