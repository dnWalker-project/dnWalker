using dnWalker.Concolic.Parameters;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Tests.Concolic.Parameters
{
    public abstract class ReferenceTypeParameterTests<TParameter> : dnlibTypeTestBase
        where TParameter : ReferenceTypeParameter
    {
        protected abstract TParameter CreateParameter();

        //[Fact]
        //public void UnitializeNullableParameter_Should_Not_Have_IsNull_Trait()
        //{
        //    TParameter parameter = CreateParameter();

        //    parameter.TryGetTrait(out IsNullTrait trait).Should().BeFalse();

        //}

        [Fact]
        public void UnitializeNullableParameter_IsNull_Should_Be_Null()
        {
            TParameter parameter = CreateParameter();

            parameter.IsNull.Should().BeNull();
        }

        //[Theory]
        //[InlineData(true)]
        //[InlineData(false)]
        //public void AfterSetting_IsNull_IsNullTrait_ShouldBe_Available_With_Correct_Value(Boolean value)
        //{
        //    TParameter parameter = CreateParameter();
        //    parameter.IsNull = value;

        //    parameter.TryGetTrait(out IsNullTrait trait).Should().BeTrue();
        //    trait.IsNullParameter.Value.Should().Be(value);
        //}
    }
}
