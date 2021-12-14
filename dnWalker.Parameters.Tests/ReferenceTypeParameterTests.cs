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
        public void UnitializedReferenceTypeParameter_IsNull_ShouldBeNull()
        {
            TParameter parameter = Create();

            parameter.IsNull.Should().BeNull();
        }

        [Fact]
        public void UnitializedReferenceTypeParameter_GetIsNull_ShouldBeTrue()
        {
            TParameter parameter = Create();

            parameter.GetIsNull().Should().BeTrue();
        }
    }
}
