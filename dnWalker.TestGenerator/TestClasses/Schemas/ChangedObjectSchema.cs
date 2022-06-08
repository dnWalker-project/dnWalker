
using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;
using dnWalker.TestGenerator.Templates;

using System;
using System.Collections.Generic;
using System.Linq;

namespace dnWalker.TestGenerator.TestClasses.Schemas
{
    public class ChangedObjectSchema : TestSchema
    {
        private readonly Location _location;

        public ChangedObjectSchema(Location location, ITestClassContext context) : base(context)
        {
            _location = location;
        }

        public Location Location
        {
            get
            {
                return _location;
            }
        }

        public override void WriteTestMethodBody(IWriter output, ITemplateProvider templates)
        {
            IReadOnlyModel inputModel = InputModel;
            IReadOnlyModel outputModel = OutputModel;
            IMethod method = Method;

            Location location = Location;

            // arrange
            IDictionary<Location, string> locationNames = templates.ArrangeTemplate.WriteArrange(output, inputModel, method);

            // act
            // TODO: do not hardcode the instance and return value symbols!
            templates.ActTemplate.WriteAct(output, method, "objectUnderTest", "result");

            // assert
            string baseName = location.GetLiteral(locationNames);
            foreach ((IField field, IValue? previous, IValue current) in GetChangedFields())
            {
                TypeSig fieldType = field.FieldSig.Type;

                string selector = $"{baseName}.{field.Name}";

                if (fieldType.IsString())
                {
                    if ((StringValue)current == StringValue.Null)
                    {
                        templates.AssertTemplate.WriteAssertNull(output, selector);
                    }
                    else
                    {
                        templates.AssertTemplate.WriteAssertEqual(output, selector, current.GetLiteral(locationNames));
                    }
                }
                else if (fieldType.IsPrimitive)
                {
                    templates.AssertTemplate.WriteAssertEqual(output, selector, current.GetLiteral(locationNames));
                }
                else if (fieldType.IsArray || fieldType.IsSZArray)
                {
                    Location currentLoc = (Location)current;
                    if (currentLoc == Location.Null)
                    {
                        templates.AssertTemplate.WriteAssertNull(output, selector);
                    }
                    else
                    {
                        templates.AssertTemplate.WriteAssertNotNull(output, selector);
                            
                        IReadOnlyArrayHeapNode currentArray = (IReadOnlyArrayHeapNode)outputModel.HeapInfo.GetNode(currentLoc);

                        if (inputModel.HeapInfo.TryGetNode(currentLoc, out _))
                        {
                            templates.AssertTemplate.WriteAssertSame(output, selector, currentLoc.GetLiteral(locationNames));
                        }
                        else
                        {
                            // a fresh array
                            templates.AssertTemplate.WriteAssertNotNull(output, selector);
                            templates.AssertTemplate.WriteAssertLength(output, selector, currentArray.Length.ToString());
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
                        templates.AssertTemplate.WriteAssertNull(output, selector);
                    }
                    else
                    {
                        templates.AssertTemplate.WriteAssertNotNull(output, selector);


                        IReadOnlyObjectHeapNode currentObject = (IReadOnlyObjectHeapNode)outputModel.HeapInfo.GetNode(currentLoc);
                        if (inputModel.HeapInfo.TryGetNode(currentLoc, out _))
                        {
                            templates.AssertTemplate.WriteAssertSame(output, selector, currentLoc.GetLiteral(locationNames));
                        }
                        else
                        {
                            // a fresh object
                            templates.AssertTemplate.WriteAssertNotNull(output, selector);
                        }
                        {
                            // TODO: write assert equivalence... how deep?
                        }
                    }
                }
            }
        }


        private IEnumerable<(IField field, IValue? previous, IValue current)> GetChangedFields()
        {
            IReadOnlyObjectHeapNode inputNode = (IReadOnlyObjectHeapNode)InputModel.HeapInfo.GetNode(Location);
            IReadOnlyObjectHeapNode outputNode = (IReadOnlyObjectHeapNode)OutputModel.HeapInfo.GetNode(Location);

            return outputNode.Fields.Select(fld =>
            {
                IValue current = outputNode.GetFieldOrDefault(fld);

                inputNode.TryGetField(fld, out IValue? previous);

                return (fld, previous, current);
            });

        }

    }
}
