using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators.Schemas.ChangedObject
{
    internal class ChangedObjectSchema : TestSchema
    {
        private readonly Location _objectLocation;

        public ChangedObjectSchema(Location objectLocation, ITestContext testContext) : base(testContext)
        {
            _objectLocation = objectLocation;
        }

        public Location ObjectLocation
        {
            get
            {
                return _objectLocation;
            }
        }

        public override void Write(ITestTemplate testTemplate, IWriter output)
        {
            ITestContext context = TestContext;
            IReadOnlyModel inputModel = context.GetInputModel();
            IReadOnlyModel outputModel = context.GetOutputModel();
            IMethod method = context.GetMethod();

            Location location = _objectLocation;

            // arrange
            testTemplate.WriteArrange(context, output);

            // act - ignore the result
            testTemplate.WriteAct(context, output);

            // assert
            string baseName = context.GetLiteral(location)!;
            foreach ((IField field, IValue? previous, IValue current) in GetChangedFields())
            {
                TypeSig fieldType = field.FieldSig.Type;

                // !!!! what if the field is private??? - the selector could be different...
                // - based on the IArrangePrimitive - WriteArrangeSelect(instanceSymbol, field) - private object may do it nicely...
                string selector = $"{baseName}.{field.Name}";

                if (fieldType.IsString())
                {
                    if ((StringValue)current == StringValue.Null)
                    {
                        testTemplate.WriteAssertNull(context, output, selector);
                    }
                    else
                    {
                        testTemplate.WriteAssertEqual(context, output, selector, context.GetLiteral(current));
                    }
                }
                else if (fieldType.IsPrimitive)
                {
                    testTemplate.WriteAssertEqual(context, output, selector, context.GetLiteral(current));
                }
                else if (fieldType.IsArray || fieldType.IsSZArray)
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


                        IReadOnlyObjectHeapNode currentObject = (IReadOnlyObjectHeapNode)outputModel.HeapInfo.GetNode(currentLoc);
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

            IEnumerable<(IField field, IValue? previous, IValue current)> GetChangedFields()
            {
                IReadOnlyObjectHeapNode inputNode = (IReadOnlyObjectHeapNode)inputModel.HeapInfo.GetNode(location);
                IReadOnlyObjectHeapNode outputNode = (IReadOnlyObjectHeapNode)outputModel.HeapInfo.GetNode(location);

                return outputNode.Fields
                    .Select(fld =>
                    {
                        IValue current = outputNode.GetFieldOrDefault(fld);

                        inputNode.TryGetField(fld, out IValue? previous);

                        return (fld, previous, current);
                    })
                    .Where(((IField fld, IValue? prev, IValue cur)t) => !t.cur.Equals(t.prev));

            }
        }
    }
}
