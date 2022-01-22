using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Parameters.Tests
{
    public class CloneTests
    {
        private static readonly string MyField = "MyField";
        private static readonly string OtherField = "OtherField";
        private static readonly string ObjectType = "MyObject";
        private static readonly string ArrayElementType = "MyObject";
        private static readonly string StructType = "MyStruct";

        private static readonly MethodSignature Signature_ToString = "System.String System.Object::ToString()";
        private static readonly MethodSignature Signature_WriteLine = "System.Void System.IO.TextWriter::WriteLine(System.String)";

        private static readonly int Invocation_First = 0;
        private static readonly int Invocation_Second = 1;
        private static readonly int Invocation_Third = 2;

        private static readonly int MyIndex = 2;
        private static readonly int MyOtherIndex = 5;

        [Fact]
        public void ClonedParameter_HasSameReference()
        {
            IParameterContext context = new ParameterContext();
            IParameter p = context.CreateInt32Parameter();

            IParameterContext newContext = new ParameterContext();

            IParameter pClone = p.Clone(newContext);

            p.Reference.Should().Be(pClone.Reference);
        }

        [Fact]
        public void ClonedContext_HasSameReferences()
        {
            IParameterContext context = new ParameterContext();

            ParameterRef[] refsOrig = Enumerable.Range(0, 10)
                .Select(i => context.CreateInt32Parameter().Reference)
                .ToArray();

            IParameterContext contextClone = context.Clone();

            contextClone.Parameters.Keys.Should().BeEquivalentTo(refsOrig);
        }

        [Fact]
        public void ClonedParameters_HaveNewContext()
        {
            IParameterContext context = new ParameterContext();


            ParameterRef[] refs = Enumerable.Range(0, 10)
                .Select(i => context.CreateInt32Parameter().Reference)
                .ToArray();

            IParameterContext contextClone = context.Clone();

            contextClone.Parameters.Keys.Should().BeEquivalentTo(refs);

            foreach (ParameterRef r in refs)
            {
                IParameter p = refs[0].Resolve(contextClone)!;
                //ReferenceEquals(p.Context, contextClone).Should().BeTrue();
                p.Context.Should().BeSameAs(contextClone);
            }
        }

        //[Fact]
        //public void ClonedContext_HasHigherGeneration()
        //{
        //    ParameterContext ctx = new ParameterContext();
        //    ParameterContext ctxClone = (ParameterContext)ctx.Clone();

        //    ctxClone.Generation.Should().BeGreaterThan(ctx.Generation);
        //}

        [Fact]
        public void ClonedContext_KeepsFieldAccessors()
        {
            IParameterContext context = new ParameterContext();
            IObjectParameter objectParameter = context.CreateObjectParameter(ObjectType, false);
            IInt32Parameter fieldParameter = context.CreateInt32Parameter();
            objectParameter.SetField(MyField, fieldParameter);

            IParameterContext contextClone = context.Clone();
            IObjectParameter objectParameterClone = (IObjectParameter)contextClone.Parameters[objectParameter.Reference];
            
            objectParameterClone.TryGetField(MyField, out IInt32Parameter? fieldParameterClone1).Should().BeTrue();
            IInt32Parameter? fieldParameterClone2 = fieldParameter.Reference.Resolve<IInt32Parameter>(contextClone);

            ReferenceEquals(fieldParameterClone1, fieldParameterClone2).Should().BeTrue();
            fieldParameterClone1.Should().BeSameAs(fieldParameterClone2);

            fieldParameterClone1!.Accessors.Should().HaveCount(fieldParameter.Accessors.Count);
            fieldParameterClone1!.Accessors[0].Should().BeOfType<FieldParameterAccessor>();
            ((FieldParameterAccessor)fieldParameterClone1!.Accessors[0]).FieldName.Should().Be(MyField);
        }

        [Fact]
        public void ClonedContext_KeepsItemAccessors()
        {
            IParameterContext context = new ParameterContext();
            IArrayParameter arrayParameter = context.CreateArrayParameter(ArrayElementType, false);
            IInt32Parameter itemParameter = context.CreateInt32Parameter();
            arrayParameter.SetItem(MyIndex, itemParameter);

            IParameterContext contextClone = context.Clone();
            IArrayParameter arrayParameterClone = (IArrayParameter)contextClone.Parameters[arrayParameter.Reference];

            arrayParameterClone.TryGetItem(MyIndex, out IInt32Parameter? itemParameterClone1).Should().BeTrue();
            IInt32Parameter? itemParameterClone2 = itemParameter.Reference.Resolve<IInt32Parameter>(contextClone);

            ReferenceEquals(itemParameterClone1, itemParameterClone2).Should().BeTrue();
            itemParameterClone1.Should().BeSameAs(itemParameterClone2);

            itemParameterClone1!.Accessors.Should().HaveCount(itemParameter.Accessors.Count);
            itemParameterClone1!.Accessors[0].Should().BeOfType<ItemParameterAccessor>();
            ((ItemParameterAccessor)itemParameterClone1!.Accessors[0]).Index.Should().Be(MyIndex);
        }

        [Fact]
        public void ClonedContext_KeepsMethodResultAccessors()
        {
            IParameterContext context = new ParameterContext();
            IObjectParameter objectParameter = context.CreateObjectParameter(ObjectType, false);
            IInt32Parameter methodResultParameter = context.CreateInt32Parameter();
            objectParameter.SetMethodResult(Signature_ToString, Invocation_Second, methodResultParameter);

            IParameterContext contextClone = context.Clone();
            IObjectParameter objectParameterClone = (IObjectParameter)contextClone.Parameters[objectParameter.Reference];

            objectParameterClone.TryGetMethodResult(Signature_ToString, Invocation_Second, out IInt32Parameter? methodResultParameterClone1).Should().BeTrue();
            IInt32Parameter? methodResultParameterClone2 = methodResultParameter.Reference.Resolve<IInt32Parameter>(contextClone);

            ReferenceEquals(methodResultParameterClone1, methodResultParameterClone2).Should().BeTrue();
            methodResultParameterClone1.Should().BeSameAs(methodResultParameterClone2);

            methodResultParameterClone1!.Accessors.Should().HaveCount(methodResultParameter.Accessors.Count);
            methodResultParameterClone1!.Accessors[0].Should().BeOfType<MethodResultParameterAccessor>();
            ((MethodResultParameterAccessor)methodResultParameterClone1!.Accessors[0]).MethodSignature.Should().Be(Signature_ToString);
            ((MethodResultParameterAccessor)methodResultParameterClone1!.Accessors[0]).Invocation.Should().Be(Invocation_Second);
        }
    }
}
