using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Parameters.Tests
{
    public abstract class ReferenceTypeParameterTests<TParameter> : ParameterTests<TParameter>
        where TParameter : ReferenceTypeParameter
    {

        [Fact]
        public void UnitializedReferenceTypeParameter_IsNull_ShouldBeTrue()
        {
            TParameter parameter = Create();

            parameter.IsNull.Should().BeTrue();
        }

        [Fact]
        public void After_SetReferenceEquals_ReferenceEquals_ShouldBeTrue()
        {
            TParameter parameter = Create(3);
            TParameter refParameter = Create(2);

            parameter.SetReferenceEquals(refParameter);

            parameter.ReferenceEquals(refParameter).Should().BeTrue();
            refParameter.ReferenceEquals(parameter).Should().BeTrue();
        }

        [Fact]
        public void After_ClearReferenceEquals_ReferenceEquals_ShouldBeFalse()
        {
            TParameter parameter = Create(3);
            TParameter refParameter = Create(2);

            parameter.SetReferenceEquals(refParameter);

            parameter.ReferenceEquals(refParameter).Should().BeTrue();
            refParameter.ReferenceEquals(parameter).Should().BeTrue();

            parameter.ClearReferenceEquals(refParameter);

            parameter.ReferenceEquals(refParameter).Should().BeFalse();
            refParameter.ReferenceEquals(parameter).Should().BeFalse();
        }

        [Fact]
        public void After_SetReferenceEquals_WithFalse_ReferenceEquals_ShouldBeFalse()
        {
            TParameter parameter = Create(3);
            TParameter refParameter = Create(2);

            parameter.SetReferenceEquals(refParameter);

            parameter.ReferenceEquals(refParameter).Should().BeTrue();
            refParameter.ReferenceEquals(parameter).Should().BeTrue();

            parameter.SetReferenceEquals(refParameter, false);

            parameter.ReferenceEquals(refParameter).Should().BeFalse();
            refParameter.ReferenceEquals(parameter).Should().BeFalse();
        }
    }
}
