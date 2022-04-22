using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using dnlib.DotNet;

using Xunit;
using FluentAssertions;
using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic;
using System.Reflection;

using static dnWalker.Symbolic.Expressions.Expression;

namespace dnWalker.Tests.Symbolic.Expressions
{
    public class PrintingTests
    {
        [Theory]
        [MemberData(nameof(GetExpressions))]
        public void TestPrinting(Expression expression, string expected)
        {
            expression.ToString().Should().Be(expected);
        }

        public static TheoryData<Expression, string> GetExpressions()
        {
            ModuleDef module = ModuleDefMD.Load(typeof(PrintingTests).Assembly.Modules.First());
            ICorLibTypes types = module.CorLibTypes;


            TheoryData<Expression, string> data = new TheoryData<Expression, string>();

            // arithmetics
            {
                data.Add(Add(Constant(5), Variable(new NamedVar(types.Int32, "x"))), "(5 + x)");
                data.Add(Subtract(Constant(5), Variable(new NamedVar(types.Int32, "x"))), "(5 - x)");
                data.Add(Multiply(Constant(5), Variable(new NamedVar(types.Int32, "x"))), "(5 * x)");
                data.Add(Divide(Constant(5), Variable(new NamedVar(types.Int32, "x"))), "(5 / x)");
                data.Add(Remainder(Constant(5), Variable(new NamedVar(types.Int32, "x"))), "(5 % x)");
                data.Add(Negate(Variable(new NamedVar(types.Int32, "x"))), "-x");

                data.Add(Add(Multiply(Constant(3.14), Variable(new NamedVar(types.Double, "x"))), Constant(4.0)), "((3.14 * x) + 4)");
            }
            // logic
            {
                data.Add(And(True, False), "(True & False)");
                data.Add(Or(True, Variable(new NamedVar(types.Boolean, "x"))), "(True | x)");
                data.Add(Not(True), "!True");
            }

            // null expression
            {
                data.Add(Null, "null");
                data.Add(NotEquals(Variable(new NamedVar(types.Object, "instance")), Null), "(instance != null)");
            }

            // length
            {
                data.Add(Length(Constant("HelloWorld!")), "Length(\"HelloWorld!\")");
            }

            return data;
        }
    }
}
