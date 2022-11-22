using dnWalker.Symbolic.Expressions.Parsing;

using FluentAssertions;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

using static dnWalker.Symbolic.Expressions.Parsing.ExpressionParser;

using TokenType = dnWalker.Symbolic.Expressions.Parsing.ExpressionParser.TokenType;

namespace dnWalker.Symbolic.Tests.Expressions.Parsing
{
    public class TokenizerTests
    {

        [Theory]
        [InlineData(" true ", "true", TokenType.BoolConstant)]
        [InlineData(" false ", "false", TokenType.BoolConstant)]
        [InlineData(" 0.0 ", "0.0", TokenType.DecimalConstant)]
        [InlineData(" .0 ", ".0", TokenType.DecimalConstant)]
        [InlineData(" 0 ", "0", TokenType.IntegerConstant)]
        [InlineData(" 99954315 ", "99954315", TokenType.IntegerConstant)]
        [InlineData(" x ", "x", TokenType.Variable)]
        [InlineData(" _x ", "_x", TokenType.Variable)]
        [InlineData(" _x_y ", "_x_y", TokenType.Variable)]
        [InlineData(" x1 ", "x1", TokenType.Variable)]
        [InlineData(" 'helo world' ", "'helo world'", TokenType.StringConstant)]
        [InlineData(" '' ", "''", TokenType.StringConstant)]
        [InlineData(" null ", "null", TokenType.Null)]
        [InlineData(" ( ", "(", TokenType.OpenBracket)]
        [InlineData(" ) ", ")", TokenType.CloseBracket)]
        [InlineData(" && ", "&&", TokenType.And)]
        [InlineData(" || ", "||", TokenType.Or)]
        [InlineData(" ! ", "!", TokenType.Not)]
        [InlineData(" + ", "+", TokenType.Add)]
        [InlineData(" - ", "-", TokenType.Substract)]
        [InlineData(" * ", "*", TokenType.Multiply)]
        [InlineData(" / ", "/", TokenType.Divide)]
        [InlineData(" % ", "%", TokenType.Modulo)]
        [InlineData(" == ", "==", TokenType.Equals)]
        [InlineData(" != ", "!=", TokenType.NotEquals)]
        [InlineData(" > ", ">", TokenType.GreaterThan)]
        [InlineData(" >= ", ">=", TokenType.GreaterThanOrEqual)]
        [InlineData(" < ", "<", TokenType.LessThan)]
        [InlineData(" <= ", "<=", TokenType.LessThanOrEqual)]
        public void TestSingleToken(string input, string value, TokenType tokenType)
        {
            Tokenizer<TokenType> tokenizer = new Tokenizer<TokenType>(ExpressionParser.TokenDefinitions);

            IList<Tokenizer<TokenType>.Token> tokens = tokenizer.Tokenize(input);

            tokens.Should().HaveCount(1);
            tokens[0].Value.Should().Be(value);
            tokens[0].Type.Should().Be(tokenType);
        }

        [Theory]
        [InlineData("k#jk")]
        public void TestInvalidToken(string input)
        {
            Tokenizer<TokenType> tokenizer = new Tokenizer<TokenType>(ExpressionParser.TokenDefinitions);

            IList<Tokenizer<TokenType>.Token> tokens = tokenizer.Tokenize(input);

            tokens.Last().Type.Should().Be(TokenType.Invalid);
        }
    }
}
