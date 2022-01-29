using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Parameters.Tests
{
    public class MethodResolverImplementationTests
    {
        private static readonly MethodSignature Signature_ToString = "System.String System.Object::ToString()";
        private static readonly MethodSignature Signature_WriteLine = "System.Void System.IO.TextWriter::WriteLine(System.String)";

        private static readonly int Invocation_First = 0;
        private static readonly int Invocation_Second = 1;
        private static readonly int Invocation_Third = 2;

        private static ParameterRef OwnerRef = 5;

        [Fact]
        public void After_SetMethodResult_TryGetMethodResult_WillOutputTheValue()
        {
            IParameterContext context = new BaseParameterContext();
            MethodResolverImplementation methodResolver = new MethodResolverImplementation(OwnerRef, context);
            IParameter methodResult = context.CreateInt32Parameter();

            methodResolver.TryGetMethodResult(Signature_ToString, Invocation_Second, out _).Should().BeFalse("Check assumptions.");

            methodResolver.SetMethodResult(Signature_ToString, Invocation_Second, methodResult.Reference);

            methodResolver.TryGetMethodResult(Signature_ToString, Invocation_Second, out ParameterRef fieldRef).Should().BeTrue();

            fieldRef.Should().Be(methodResult.Reference);
        }

        [Fact]
        public void TryGetUninitializedMethodResult_ReturnsFalse()
        {
            IParameterContext context = new BaseParameterContext();
            MethodResolverImplementation methodResolver = new MethodResolverImplementation(OwnerRef, context);

            methodResolver.TryGetMethodResult(Signature_ToString, Invocation_Second, out _).Should().BeFalse();
        }

        [Fact]
        public void TryGetInitializedMethodResult_ReturnsTrue()
        {
            IParameterContext context = new BaseParameterContext();
            MethodResolverImplementation methodResolver = new MethodResolverImplementation(OwnerRef, context);
            IParameter methodResult = context.CreateInt32Parameter();

            methodResolver.TryGetMethodResult(Signature_ToString, Invocation_Second, out _).Should().BeFalse("Check assumptions.");

            methodResolver.SetMethodResult(Signature_ToString, Invocation_Second, methodResult.Reference);

            methodResolver.TryGetMethodResult(Signature_ToString, Invocation_Second, out _).Should().BeTrue();
        }

        [Fact]
        public void TryGetClearedMethodResult_ReturnsFalse()
        {
            IParameterContext context = new BaseParameterContext();
            MethodResolverImplementation methodResolver = new MethodResolverImplementation(OwnerRef, context);
            IParameter methodResult = context.CreateInt32Parameter();

            methodResolver.SetMethodResult(Signature_ToString, Invocation_Second, methodResult.Reference);

            methodResolver.TryGetMethodResult(Signature_ToString, Invocation_Second, out _).Should().BeTrue("Check assumptions.");

            methodResolver.ClearMethodResult(Signature_ToString, Invocation_Second);

            methodResolver.TryGetMethodResult(Signature_ToString, Invocation_Second, out _).Should().BeFalse();
        }

        [Fact]
        public void GetMethodResults_IsNotNull()
        {
            IParameterContext context = new BaseParameterContext();
            MethodResolverImplementation methodResolver = new MethodResolverImplementation(OwnerRef, context);

            methodResolver.GetMethodResults().Should().NotBeNull();
        }

        [Fact]
        public void After_SetMethodResult_ValueWillBeInGetMethodResultsDictionary()
        {
            IParameterContext context = new BaseParameterContext();
            MethodResolverImplementation methodResolver = new MethodResolverImplementation(OwnerRef, context);
            IParameter methodResult = context.CreateInt32Parameter();

            methodResolver.SetMethodResult(Signature_ToString, Invocation_Second, methodResult.Reference);

            methodResolver.TryGetMethodResult(Signature_ToString, Invocation_Second, out _).Should().BeTrue("Check assumptions.");

            methodResolver.GetMethodResults()[Signature_ToString].Should().Contain(methodResult.Reference);
        }

        [Fact]
        public void After_ClearMethodResult_ValueWillNotBeInGetMethodResultsDictionary()
        {
            IParameterContext context = new BaseParameterContext();
            MethodResolverImplementation methodResolver = new MethodResolverImplementation(OwnerRef, context);
            IParameter methodResult = context.CreateInt32Parameter();

            methodResolver.SetMethodResult(Signature_ToString, Invocation_Second, methodResult.Reference);

            methodResolver.TryGetMethodResult(Signature_ToString, Invocation_Second, out _).Should().BeTrue("Check assumptions.");

            methodResolver.GetMethodResults()[Signature_ToString].Should().Contain(methodResult.Reference, "Check assumptions.");

            methodResolver.ClearMethodResult(Signature_ToString, Invocation_Second);

            methodResolver.TryGetMethodResult(Signature_ToString, Invocation_Second, out _).Should().BeFalse("Check assumptions.");

            methodResolver.GetMethodResults()[Signature_ToString].Should().NotContain(methodResult.Reference);
        }

        [Fact]
        public void After_SetMethodResult_MethodResultValueWillHave_MethodResultParameterAccess()
        {
            IParameterContext context = new BaseParameterContext();
            MethodResolverImplementation methodResolver = new MethodResolverImplementation(OwnerRef, context);
            IParameter methodResult = context.CreateInt32Parameter();

            methodResolver.SetMethodResult(Signature_ToString, Invocation_Second, methodResult.Reference);

            methodResolver.TryGetMethodResult(Signature_ToString, Invocation_Second, out _).Should().BeTrue("Check assumptions.");

            methodResult.Accessors.Should().HaveCountGreaterThanOrEqualTo(1);
            methodResult.Accessors[0].Should().BeOfType<MethodResultParameterAccessor>();
            ((MethodResultParameterAccessor)methodResult.Accessors[0]).ParentRef.Should().Be(OwnerRef);
            ((MethodResultParameterAccessor)methodResult.Accessors[0]).MethodSignature.Should().Be(Signature_ToString);
            ((MethodResultParameterAccessor)methodResult.Accessors[0]).Invocation.Should().Be(Invocation_Second);
        }

        [Fact]
        public void Setting_Invocation_ShouldNot_Set_EarlierOrLaterInvocation()
        {
            IParameterContext context = new BaseParameterContext();
            MethodResolverImplementation methodResolver = new MethodResolverImplementation(OwnerRef, context);
            IParameter firstMethodResult = context.CreateInt32Parameter();
            IParameter secondMethodResult = context.CreateInt32Parameter();
            IParameter thirdMethodResult = context.CreateInt32Parameter();

            methodResolver.SetMethodResult(Signature_ToString, Invocation_Second, secondMethodResult.Reference);
            methodResolver.TryGetMethodResult(Signature_ToString, Invocation_Second, out _).Should().BeTrue("Check assumptions.");


            methodResolver.TryGetMethodResult(Signature_ToString, Invocation_First, out _).Should().BeFalse();
            methodResolver.TryGetMethodResult(Signature_ToString, Invocation_Third, out _).Should().BeFalse();
        }
    }
}
