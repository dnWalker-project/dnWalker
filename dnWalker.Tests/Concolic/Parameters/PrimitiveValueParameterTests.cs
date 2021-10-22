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
    public abstract class PrimitiveValueParameterTests<TParameter, TValue>
        where TValue : struct
        where TParameter : PrimitiveValueParameter<TValue>
    {
        protected abstract TParameter Create(string name = "p");

        [Fact]
        public void Before_SettingeValue_ValueIs_Null()
        {
            var p = Create("p");
            p.Value.Should().BeNull();
        }

        [Fact]
        public void GetParameterExpressions_Returns_Single_ParameterExpression()
        {
            var p = Create("p");

            var exprs = p.GetParameterExpressions().ToList();
            exprs.Should().HaveCount(1);
        }

        [Fact]
        public void PrimitiveValueParameter_Has_SingleExpression()
        {
            var p = Create("p");
            p.HasSingleExpression.Should().BeTrue();
        }

        [Fact]
        public void ParamterExpression_Has_Correct_Name()
        {
            var p = Create("p");

            var expr = p.GetSingleParameterExpression();
            expr.Name.Should().Be("p");
        }

        [Fact]
        public void ParamterExpression_Has_Correct_Type()
        {
            var p = Create("p");

            var expr = p.GetSingleParameterExpression();
            expr.Type.Should().Be(typeof(TValue));
        }
    }
}
