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
        public void UnitializedReferenceTypeParameter_IsNull_Should_Be_True()
        {
            TParameter parameter = Create("p");

            parameter.IsNull.Should().BeTrue();
        }

        [Fact]
        public void IsNullParameter_IsNotNull()
        {
            TParameter parameter = Create("p");

            parameter.IsNullParameter.Should().NotBeNull();
        }

    }
}
