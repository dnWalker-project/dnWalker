using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using dnWalker.Parameters.Expressions;
using FluentAssertions;

namespace dnWalker.Parameters.Tests.Expressions
{
    public class ParametricExpressiongTests
    {
        [Theory]
        [InlineData("V12345678", typeof(ValueOfParametricExpression), 0x12345678)]
        [InlineData("R123456780BCDEF01", typeof(RefEqualsParametricExpression), 0x12345678, 0x0BCDEF01)]
        [InlineData("L12345678", typeof(LengthOfParametricExpression), 0x12345678)]
        [InlineData("N12345678", typeof(IsNullParametricExpression), 0x12345678)]
        public void TestParsing_Valid(string str, Type expressionType, params int[] paramIds)
        {
            ParametricExpression expr = ParametricExpression.Parse(str);

            expr.Should().BeOfType(expressionType);

            if (expr is UnaryParametricExpression unary)
            {
                unary.Operand.Should().Be((ParameterReference)paramIds[0]);
            }
            else if (expr is BinaryParametricExpression binary)
            {
                binary.Lhs.Should().Be((ParameterReference)paramIds[0]);
                binary.Rhs.Should().Be((ParameterReference)paramIds[1]);
            }
            else
            {
                Assert.True(false, "Expression is neither unary nor binary type");
            }

        }

        [Theory]
        [InlineData("v12345678")] // invalid identifer - not capital
        [InlineData("V0345678")]  // invalid parameter - too short parameter id
        [InlineData("R0345678")]  // expecting 2 ids, not only one
        [InlineData("R03456781234")]  // second id is too short
        public void TestParsing_Invalid(string str)
        {
            ParametricExpression.TryParse(str, out _).Should().BeFalse();
            Assert.Throws<FormatException>(() => ParametricExpression.Parse(str));
        }

        [Theory]
        [InlineData("V12345678")]
        [InlineData("R123456780BCDEF01")]
        [InlineData("L12345678")]
        [InlineData("N12345678")]
        public void TestStringRepresentation(string str)
        {
            ParametricExpression expr = ParametricExpression.Parse(str);

            string strRepresentation = expr.ToString()!.ToUpper();

            strRepresentation.Should().Be(str);
        }
    }
}
