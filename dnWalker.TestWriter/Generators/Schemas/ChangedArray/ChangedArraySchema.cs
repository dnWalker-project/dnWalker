using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators.Schemas.ChangedArray
{
    internal class ChangedArraySchema : TestSchema
    {
        private readonly Location _arrayLocation;

        public ChangedArraySchema(Location arrayLocation, ITestContext context) : base(context)
        {
            _arrayLocation = arrayLocation;
        }

        public Location ArrayLocation
        {
            get
            {
                return _arrayLocation;
            }
        }

        public override void Write(ITestTemplate testTemplate, IWriter output)
        {
            ITestContext context = TestContext;
            IReadOnlyModel inputModel = context.GetInputModel();
            IReadOnlyModel outputModel = context.GetOutputModel();
            IMethod method = context.GetMethod();

            Location arrayLocation = _arrayLocation;

            // arrange
            testTemplate.WriteArrange(context, output);

            // act - ignore the result
            testTemplate.WriteAct(context, output);

            // assert
            SymbolContext theArraySymbol = context.GetSymbolContext(arrayLocation)!;
            string baseName = theArraySymbol.Literal;

            TypeSig elementType = ((IReadOnlyArrayHeapNode)outputModel.HeapInfo.GetNode(arrayLocation)).ElementType;

            foreach ((int index, IValue? previous, IValue current) in GetChangedElements())
            {
                string selector = $"{baseName}[{index}]";

                if (elementType.IsString())
                {
                    if ((StringValue)current == StringValue.Null)
                    {
                        testTemplate.WriteAssertNull(context, output, selector);
                    }
                    else
                    {
                        testTemplate.WriteAssertEqual(context, output, selector, context.GetLiteral(current)!);
                    }
                }
                else if (elementType.IsPrimitive)
                {
                    testTemplate.WriteAssertEqual(context, output, selector, context.GetLiteral(current)!);
                }
                else if (elementType.IsArray || elementType.IsSZArray)
                {
                    Location currentLoc = (Location)current;
                    if (currentLoc == Location.Null)
                    {
                        testTemplate.WriteAssertNull(context, output, selector);
                    }
                    else
                    {
                        testTemplate.WriteAssertNotNull(context, output, selector);

                        IReadOnlyArrayHeapNode currentArray = (IReadOnlyArrayHeapNode)outputModel.HeapInfo.GetNode(currentLoc);

                        if (inputModel.HeapInfo.TryGetNode(currentLoc, out _))
                        {
                            testTemplate.WriteAssertSame(context, output, selector, context.GetLiteral(currentLoc)!);
                        }
                        else
                        {
                            // a fresh array
                            testTemplate.WriteAssertNotNull(context, output, selector);
                            testTemplate.WriteAssertLength(context, output, selector, currentArray.Length.ToString());
                        }
                        {
                            // TODO: write assert equivalence... how deep?
                        }
                    }
                }
                else
                {
                    Location currentLoc = (Location)current;
                    if (currentLoc == Location.Null)
                    {
                        testTemplate.WriteAssertNull(context, output, selector);
                    }
                    else
                    {
                        testTemplate.WriteAssertNotNull(context, output, selector);


                        IReadOnlyArrayHeapNode currentObject = (IReadOnlyArrayHeapNode)outputModel.HeapInfo.GetNode(currentLoc);
                        if (inputModel.HeapInfo.TryGetNode(currentLoc, out _))
                        {
                            testTemplate.WriteAssertSame(context, output, selector, context.GetLiteral(currentLoc)!);
                        }
                        else
                        {
                            // a fresh object
                            testTemplate.WriteAssertNotNull(context, output, selector);
                        }
                        {
                            // TODO: write assert equivalence... how deep?
                        }
                    }
                }
            }

            IEnumerable<(int index, IValue? previous, IValue current)> GetChangedElements()
            {
                IReadOnlyArrayHeapNode inputNode = (IReadOnlyArrayHeapNode)inputModel.HeapInfo.GetNode(arrayLocation);
                IReadOnlyArrayHeapNode outputNode = (IReadOnlyArrayHeapNode)outputModel.HeapInfo.GetNode(arrayLocation);

                return outputNode.Indeces
                    .Select(index =>
                    {
                        IValue current = outputNode.GetElementOrDefault(index);

                        inputNode.TryGetElement(index, out IValue? previous);

                        return (index, previous, current);
                    })
                    .Where(((int idx, IValue? prev, IValue cur)t) => !t.cur.Equals(t.prev));
            }
        }
    }
}
