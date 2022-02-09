using dnWalker.TypeSystem;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Parameters.Tests
{
    public class ParameterCloning_Tests : TestBase
    {

        private readonly string MyField = "MyField";
        private readonly string OtherField = "OtherField";
        //private readonly string ObjectType = "MyObject";
        //private readonly string ArrayElementType = "MyObject";
        //private readonly string StructType = "MyStruct";

        private readonly MethodSignature Signature_ToString;
        private readonly MethodSignature Signature_WriteLine;

        private readonly TypeSignature ObjectType;
        private readonly TypeSignature TextWriterType;

        private readonly int Invocation_First = 0;
        private readonly int Invocation_Second = 1;
        private readonly int Invocation_Third = 2;

        private readonly int MyIndex = 2;
        private readonly int MyOtherIndex = 5;

        public ParameterCloning_Tests()
        {
            MethodTranslator methodTranslator = new MethodTranslator(DefinitionProvider);
            TypeTranslator typeTranslator = new TypeTranslator(DefinitionProvider);

            Signature_ToString = methodTranslator.FromString("System.String System.Object::ToString()");
            Signature_WriteLine = methodTranslator.FromString("System.Void System.IO.TextWriter::WriteLine(System.String)");

            ObjectType = typeTranslator.FromString("System.Object");
            TextWriterType = typeTranslator.FromString("System.IO.TextWriter");
        }


        [Fact]
        public void ClonedParameter_HasSameReference()
        {
            IParameterContext context = new ParameterContext(DefinitionProvider);

            IParameterSet fistSet = new ParameterSet(context);
            IParameter p = fistSet.CreateInt32Parameter();

            IParameterSet secondSet = new ParameterSet(context);

            IParameter pClone = p.Clone(secondSet);

            p.Reference.Should().Be(pClone.Reference);
        }

        [Fact]
        public void ExecutionContext_HasSameReferences()
        {
            IParameterContext context = new ParameterContext(DefinitionProvider);

            IBaseParameterSet baseSet = new BaseParameterSet(context);

            ParameterRef[] refsOrig = Enumerable.Range(0, 10)
                .Select(i => baseSet.CreateInt32Parameter().Reference)
                .ToArray();

            IExecutionParameterSet execitionSet = baseSet.CreateExecutionSet();

            execitionSet.Parameters.Keys.Should().BeEquivalentTo(refsOrig);
        }

        [Fact]
        public void ClonedParameters_HaveNewContext()
        {
            IParameterContext context = new ParameterContext(DefinitionProvider);

            IBaseParameterSet baseSet = new BaseParameterSet(context);


            ParameterRef[] refs = Enumerable.Range(0, 10)
                .Select(i => baseSet.CreateInt32Parameter().Reference)
                .ToArray();

            IExecutionParameterSet executionSet = baseSet.CreateExecutionSet();

            executionSet.Parameters.Keys.Should().BeEquivalentTo(refs);

            foreach (ParameterRef r in refs)
            {
                IParameter p = refs[0].Resolve(executionSet)!;
                //ReferenceEquals(p.Context, baseSetClone).Should().BeTrue();
                p.Set.Should().BeSameAs(executionSet);
            }
        }

        [Fact]
        public void ExecutionContext_KeepsFieldAccessors()
        {
            IParameterContext context = new ParameterContext(DefinitionProvider);

            IBaseParameterSet baseSet = new BaseParameterSet(context);
            IObjectParameter objectParameter = baseSet.CreateObjectParameter(ObjectType, isNull: false);
            IInt32Parameter fieldParameter = baseSet.CreateInt32Parameter();
            objectParameter.SetField(MyField, fieldParameter);

            IExecutionParameterSet baseSetClone = baseSet.CreateExecutionSet();
            IObjectParameter objectParameterClone = (IObjectParameter)baseSetClone.Parameters[objectParameter.Reference];
            
            objectParameterClone.TryGetField(MyField, out IInt32Parameter? fieldParameterClone1).Should().BeTrue();
            IInt32Parameter? fieldParameterClone2 = fieldParameter.Reference.Resolve<IInt32Parameter>(baseSetClone);

            ReferenceEquals(fieldParameterClone1, fieldParameterClone2).Should().BeTrue();
            fieldParameterClone1.Should().BeSameAs(fieldParameterClone2);

            fieldParameterClone1!.Accessors.Should().HaveCount(fieldParameter.Accessors.Count);
            fieldParameterClone1!.Accessors[0].Should().BeOfType<FieldParameterAccessor>();
            ((FieldParameterAccessor)fieldParameterClone1!.Accessors[0]).FieldName.Should().Be(MyField);
        }

        [Fact]
        public void ExecutionContext_KeepsItemAccessors()
        {
            IParameterContext context = new ParameterContext(DefinitionProvider);

            IBaseParameterSet baseSet = new BaseParameterSet(context);
            IArrayParameter arrayParameter = baseSet.CreateArrayParameter(ObjectType, isNull: false);
            IInt32Parameter itemParameter = baseSet.CreateInt32Parameter();
            arrayParameter.SetItem(MyIndex, itemParameter);

            IExecutionParameterSet baseSetClone = baseSet.CreateExecutionSet();
            IArrayParameter arrayParameterClone = (IArrayParameter)baseSetClone.Parameters[arrayParameter.Reference];

            arrayParameterClone.TryGetItem(MyIndex, out IInt32Parameter? itemParameterClone1).Should().BeTrue();
            IInt32Parameter? itemParameterClone2 = itemParameter.Reference.Resolve<IInt32Parameter>(baseSetClone);

            ReferenceEquals(itemParameterClone1, itemParameterClone2).Should().BeTrue();
            itemParameterClone1.Should().BeSameAs(itemParameterClone2);

            itemParameterClone1!.Accessors.Should().HaveCount(itemParameter.Accessors.Count);
            itemParameterClone1!.Accessors[0].Should().BeOfType<ItemParameterAccessor>();
            ((ItemParameterAccessor)itemParameterClone1!.Accessors[0]).Index.Should().Be(MyIndex);
        }

        [Fact]
        public void ExecutionContext_KeepsMethodResultAccessors()
        {
            IParameterContext context = new ParameterContext(DefinitionProvider);

            IBaseParameterSet baseSet = new BaseParameterSet(context);
            IObjectParameter objectParameter = baseSet.CreateObjectParameter(ObjectType, isNull: false);
            IInt32Parameter methodResultParameter = baseSet.CreateInt32Parameter();
            objectParameter.SetMethodResult(Signature_ToString, Invocation_Second, methodResultParameter);

            IExecutionParameterSet baseSetClone = baseSet.CreateExecutionSet();
            IObjectParameter objectParameterClone = (IObjectParameter)baseSetClone.Parameters[objectParameter.Reference];

            objectParameterClone.TryGetMethodResult(Signature_ToString, Invocation_Second, out IInt32Parameter? methodResultParameterClone1).Should().BeTrue();
            IInt32Parameter? methodResultParameterClone2 = methodResultParameter.Reference.Resolve<IInt32Parameter>(baseSetClone);

            ReferenceEquals(methodResultParameterClone1, methodResultParameterClone2).Should().BeTrue();
            methodResultParameterClone1.Should().BeSameAs(methodResultParameterClone2);

            methodResultParameterClone1!.Accessors.Should().HaveCount(methodResultParameter.Accessors.Count);
            methodResultParameterClone1!.Accessors[0].Should().BeOfType<MethodResultParameterAccessor>();
            ((MethodResultParameterAccessor)methodResultParameterClone1!.Accessors[0]).MethodSignature.Should().Be(Signature_ToString);
            ((MethodResultParameterAccessor)methodResultParameterClone1!.Accessors[0]).Invocation.Should().Be(Invocation_Second);
        }
    }
}
