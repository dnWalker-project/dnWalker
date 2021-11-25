using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Parameters.Tests
{
    public abstract class ReferenceTypeParameterTests<TParameter>
        where TParameter : ReferenceTypeParameter
    {
        protected abstract TParameter Create(string name = "p");

        [Fact]
        public void UnitializedReferenceTypeParameter_IsNull_Should_Be_True()
        {
            var parameter = Create("p");

            parameter.IsNull.Should().BeTrue();
        }

    }
}
