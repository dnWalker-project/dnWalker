using dnlib.DotNet;

using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Expressions.Parsing;
using dnWalker.Symbolic.Variables;
using dnWalker.TypeSystem;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Symbolic.Tests.Expressions.Parsing
{
    public class ExpressionParserTests
    {
        private readonly ExpressionFactory _ef;
        private readonly ICorLibTypes _types;

        public ExpressionParserTests()
        {
            ModuleDef md = ModuleDefMD.Load(typeof(ExpressionParserTests).Module);
            _types = md.CorLibTypes;
            _ef = new CustomModuleExpressionFactory(md);
        }


        [Fact]
        public void Test()
        {
            string input = "(((x + y) / 2) == 5)";

            NamedVariable x = new NamedVariable(_types.Int32, "x");
            NamedVariable y = new NamedVariable(_types.Double, "y");

            Expression expected = _ef.MakeEqual(
                _ef.MakeDivide(
                    _ef.MakeAdd(
                        _ef.MakeVariable(x),
                        _ef.MakeVariable(y)),
                    _ef.MakeIntegerConstant(2)),
                _ef.MakeIntegerConstant(5));

            Dictionary<string, TypeSig> freeVars = new Dictionary<string, TypeSig>() { ["x"] = _types.Int32, ["y"] = _types.Double };

            ExpressionParser parser = new ExpressionParser(freeVars, _ef);
            Expression? parsed = parser.Parse(input);
            parsed.Should().BeEquivalentTo(expected);
        }
    }
}
