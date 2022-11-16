using dnlib.DotNet;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators.Schemas.ReturnValue
{
    internal class ReturnValueSchema : TestSchema
    {
        public ReturnValueSchema(ITestContext testContext) : base(testContext)
        {
        }

        public override void Write(ITestTemplate testTemplate, IWriter output)
        {
            ITestContext context = TestContext;
            IReadOnlyModel inputModel = context.GetInputModel();
            IReadOnlyModel outputModel = context.GetOutputModel();
            IMethod method = context.GetMethod();

            if (!outputModel.TryGetReturnValue(method, out IValue? expected)) throw new Exception("The model does not contain the return value.");

            // arrange
            testTemplate.WriteArrange(context, output);

            // act
            string returnSymbol = context.GetReturnSymbol();
            testTemplate.WriteAct(context, output, returnSymbol);

            // assert
            // based on the return value

            TypeSig retType = method.MethodSig.RetType;
            if (retType.IsPrimitive || retType.IsString())
            {
                // we can assert equality
                // expected is not location => GetLiteral cannot be null
                string expectedLiteral = context.GetLiteral(expected)!;
                testTemplate.WriteAssertEqual(context, output, returnSymbol, expectedLiteral);
            }
            else
            {
                // reference type - a bit more complex...
                Location expectedLocation = (Location)expected;
                // we may assert:
                // - is null
                if (expectedLocation == Location.Null)
                {
                    testTemplate.WriteAssertNull(context, output, returnSymbol);
                    return;
                }
                testTemplate.WriteAssertNotNull(context, output, returnSymbol);

                bool isFresh = true;

                // - identity to some already arranged object
                if (inputModel.HeapInfo.TryGetNode(expectedLocation, out _))
                {
                    // expected must have been already arranged => GetLiteral cannot be null
                    string expectedLiteral = context.GetLiteral(expected)!;
                    testTemplate.WriteAssertSame(context, output, returnSymbol, expectedLiteral);
                    isFresh = false;
                }
                // TODO: - the object is a fresh one - we may assert equivalence => we need to arrange part of the output model...

                IReadOnlyHeapNode resultNode = outputModel.HeapInfo.GetNode(expectedLocation);

                switch (resultNode)
                {
                    case IReadOnlyObjectHeapNode objResult: WriteAssertObjectTraits(testTemplate, output, objResult, returnSymbol); break;
                    case IReadOnlyArrayHeapNode arrResult: WriteAssertArrayTraits(testTemplate, output, arrResult, returnSymbol, !isFresh); break;
                }
            }
        }

        private void WriteAssertArrayTraits(ITestTemplate testTemplate, IWriter output, IReadOnlyArrayHeapNode arrResult, string resultLiteral, bool isInput)
        {
            ITestContext context = TestContext;

            if (!isInput)
            {
                // only for fresh variable
                testTemplate.WriteAssertLength(context, output, resultLiteral, arrResult.Length.ToString());
            }

            foreach (int index in arrResult.Indeces)
            {
                IValue elementValue = arrResult.GetElementOrDefault(index);
                string selectorLiteral = $"{resultLiteral}[{index}]";

                if (elementValue is Location elemLoc)
                {
                    SymbolContext? elemSymbol = context.GetSymbolContext(elemLoc);

                    if (elemSymbol != null)
                    {
                        // input argument, assert same as
                        testTemplate.WriteAssertSame(context, output, selectorLiteral, elemSymbol.Literal);
                    }
                    else
                    {
                        // fresh value, we could arrange entire heap graph and do equivalence check - way too complex...
                        // assert not null
                        testTemplate.WriteAssertNotNull(context, output, selectorLiteral);
                    }
                }
                else
                {
                    // the element literal cannot be null, that happens only if the element is location!!!
                    string elementLiteral = context.GetLiteral(elementValue)!; 
                    testTemplate.WriteAssertEqual(context, output, selectorLiteral, elementLiteral);
                }
            }
        }

        private void WriteAssertObjectTraits(ITestTemplate testTemplate, IWriter output, IReadOnlyObjectHeapNode objResult, string resultLiteral)
        {
            ITestContext context = TestContext;

            foreach (IField fld in objResult.Fields)
            {
                IValue fldValue = objResult.GetFieldOrDefault(fld);

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
                    SymbolContext? fldSymbol = context.GetSymbolContext(fldLoc);
                    if (fldSymbol != null)
                    {
                        // input argument, assert same as
                        testTemplate.WriteAssertSame(context, output, selectorLiteral, fldSymbol.Literal);
                    }
                    else
                    {
                        // fresh value, we could arrange entire heap graph and do equivalence check - way too complex...
                        // assert not null
                        testTemplate.WriteAssertNotNull(context, output, selectorLiteral);
                    }
                }
                else
                {
                    // the field literal cannot be null, that happens only if the element is location!!!
                    string fldLiteral = context.GetLiteral(fldValue)!;
                    testTemplate.WriteAssertEqual(context, output, selectorLiteral, fldLiteral);
                }
            }
        }
    }
}
