using dnWalker.Concolic.Parameters;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Tests.Concolic.Parameters
{
    public abstract class ReferenceTypeParameterTests<TParameter> : dnlibTypeTestBase
        where TParameter : ReferenceTypeParameter
    {
        protected abstract TParameter Create(string name = "p");

        
        [Fact]
        public void UnitializedReferenceTypeParameter_IsNull_Should_Be_True()
        {
            var parameter = Create("p");

            parameter.IsNull.Should().BeTrue();
        }

        [Fact]
        public void ReferenceTypeParameter_Has_AtLeast_One_ParameterExpression()
        {
            var parameter = Create("p");

            parameter.GetParameterExpressions().Should().HaveCountGreaterOrEqualTo(1);
        }

        [Fact]
        public void ReferenceTypeParameter_Has_IsNullParameterExpression()
        {
            var parameter = Create("p");

            var exprs = parameter.GetParameterExpressions().ToList();

            Assert.Contains(exprs, pe => pe.Name == ParameterName.ConstructField("p", ReferenceTypeParameter.IsNullParameterName));
        }

        [Fact]
        public void ChangingName_Changes_IsNullParameter_Name()
        {
            var parameter = Create("p");

            parameter.Name = "new_name";

            parameter.IsNullParameter.Name.Should().BeEquivalentTo(ParameterName.ConstructField("new_name", ReferenceTypeParameter.IsNullParameterName));
        }
    }
}
