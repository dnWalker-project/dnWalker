using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Parameters.Tests
{
    public abstract class MethodResolverParameterTests<TParameter> : ReferenceTypeParameterTests<TParameter>
        where TParameter : MethodResolverParameter
    {
        private static readonly MethodSignature MyMethodSignature = "System.Boolean MyClass::MyMethod()";

        [Fact]
        public void After_SetMethodResult_ResultHas_MethodResultAccessor()
        {
            TParameter parameter = Create();
            IParameter result = new BooleanParameter();


            parameter.SetMethodResult(MyMethodSignature, 2, result);

            result.Accessor.Should().NotBeNull();
            result.Accessor.Should().BeOfType<MethodResultParameterAccessor>();
        }

        [Theory]
        [InlineData("MyNamespace.MyClass")]
        public void After_SetField_AccessorMethodSignatureAndInvocation_ShouldBe_MethodSignatureAndInvocation(string typeName)
        {
            TParameter parameter = Create();
            IParameter result = new BooleanParameter();

            parameter.SetMethodResult(MyMethodSignature, 2, result);

            MethodResultParameterAccessor accessor = (MethodResultParameterAccessor)result.Accessor!;

            accessor.MethodSignature.Should().Be(MyMethodSignature);
            accessor.Invocation.Should().Be(2);
        }

        [Fact]
        public void After_SetMethodResult_ResultIsWithin_GetChildren()
        {
            TParameter parameter = Create();
            IParameter result = new BooleanParameter();

            parameter.SetMethodResult(MyMethodSignature, 2, result);

            parameter.GetChildren().Should().Contain(result);
        }

        [Fact]
        public void After_SetMethodResult_ResultIsWithin_GetMethodResults()
        {
            TParameter parameter = Create();
            IParameter result = new BooleanParameter();


            parameter.SetMethodResult(MyMethodSignature, 2, result);

            IParameter?[] resultsForMS = parameter.GetMethodResults().First(p => p.Key == MyMethodSignature).Value;

            resultsForMS.Length.Should().Be(3);
            resultsForMS[0].Should().BeNull();
            resultsForMS[1].Should().BeNull();
            resultsForMS[2].Should().Be(result);
        }

        [Fact]
        public void After_ClearMethodResult_GetMethodResults_IsEmpty()
        {
            TParameter parameter = Create();
            IParameter result = new BooleanParameter();


            parameter.SetMethodResult(MyMethodSignature, 2, result);

            parameter.ClearMethodResult(MyMethodSignature, 2);

            IParameter?[] resultsForMS = parameter.GetMethodResults().First(p => p.Key == MyMethodSignature).Value;

            resultsForMS.Length.Should().Be(0);
        }

        [Fact]
        public void Initialized_MethodResult_ShouldBeSameAs_Initializer()
        {
            TParameter parameter = Create();
            IParameter result = new BooleanParameter();

            parameter.SetMethodResult(MyMethodSignature, 2, result);

            result.Accessor.Should().BeOfType<MethodResultParameterAccessor>();
            result.Accessor!.Parent.Should().BeSameAs(parameter);
        }
    }
}
