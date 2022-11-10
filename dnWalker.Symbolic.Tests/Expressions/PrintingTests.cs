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
using dnWalker.Symbolic.Variables;

namespace dnWalker.Symbolic.Tests.Expressions
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
                data.Add(MakeAdd(MakeConstant(types.Int32, 5), MakeVariable(new NamedVariable(types.Int32, "x"))), "(5 + x)");
                data.Add(MakeSubtract(MakeConstant(types.Int32, 5), MakeVariable(new NamedVariable(types.Int32, "x"))), "(5 - x)");
                data.Add(MakeMultiply(MakeConstant(types.Int32, 5), MakeVariable(new NamedVariable(types.Int32, "x"))), "(5 * x)");
                data.Add(MakeDivide(MakeConstant(types.Int32, 5), MakeVariable(new NamedVariable(types.Int32, "x"))), "(5 / x)");
                data.Add(MakeRemainder(MakeConstant(types.Int32, 5), MakeVariable(new NamedVariable(types.Int32, "x"))), "(5 % x)");
                data.Add(MakeNegate(MakeVariable(new NamedVariable(types.Int32, "x"))), "-x");

                data.Add(MakeAdd(MakeMultiply(MakeConstant(types.Double, 3.14), MakeVariable(new NamedVariable(types.Double, "x"))), MakeConstant(types.Double, 4.0)), "((3.14 * x) + 4)");
            }
            // logic
            {
                data.Add(MakeAnd(MakeConstant(types.Boolean, true), MakeConstant(types.Boolean, false)), "(True & False)");
                data.Add(MakeOr(MakeConstant(types.Boolean, true), MakeVariable(new NamedVariable(types.Boolean, "x"))), "(True | x)");
                data.Add(MakeNot(MakeConstant(types.Boolean, true)), "!True");
            }

            // null expression
            {
                data.Add(MakeConstant(types.Object, null), "null");
                data.Add(MakeNotEqual(MakeVariable(new NamedVariable(types.Object, "instance")), MakeConstant(types.Object, null)), "(instance != null)");
            }

            //// length
            //{
            //    data.Add(Length(Constant("HelloWorld!")), "Length(\"HelloWorld!\")");
            //}

            return data;
        }
    }
}
