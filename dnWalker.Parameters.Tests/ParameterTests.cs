using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Parameters.Tests
{

    public abstract class ParameterTests<TParameter> 
        where TParameter : Parameter
    {
        private const int ID = 5;

        protected abstract TParameter Create(int id = ID);



        [Fact]
        public void ParameterWithRootParameterAccessor_ShouldBe_Root()
        {
            TParameter parameter = Create();

            parameter.Accessor = new RootParameterAccessor("x");

            parameter.IsRoot().Should().BeTrue();
        }

        [Fact]
        public void ParametersWithSameID_ShouldEqual()
        {
            TParameter lhs = Create(2);
            TParameter rhs = Create(2);

            lhs.Should().Be(rhs);
        }

        [Fact]
        public void ParametersWithDiffenerID_ShouldNotEqual()
        {
            TParameter lhs = Create(1);
            TParameter rhs = Create(2);

            lhs.Should().NotBe(rhs);
        }

        [Fact]
        public void GetSelfAndDescendants_ShouldContainSelf()
        {
            TParameter p = Create();
            p.GetSelfAndDescendants().Should().Contain(p);
        }
    }
}
