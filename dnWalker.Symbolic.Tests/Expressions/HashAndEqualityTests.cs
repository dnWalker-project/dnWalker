using dnlib.DotNet;

using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Variables;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Symbolic.Tests.Expressions
{
    public class HashAndEqualityTests
    {
        [Theory]
        [MemberData(nameof(EquivalentExpressions))]
        public void Test_EquivalentExpressionsHasSameHash(Expression x1, Expression x2)
        {
            x1.GetHashCode().Should().Be(x2.GetHashCode());
        }

        [Theory]
        [MemberData(nameof(EquivalentExpressions))]
        public void Test_EquivalentExpressionsAreEqual(Expression x1, Expression x2)
        {
            x1.Equals(x2).Should().BeTrue();
        }

        public static TheoryData<Expression, Expression> EquivalentExpressions()
        {
            ICorLibTypes types = ModuleDefMD.Load(typeof(HashAndEqualityTests).Module).CorLibTypes;

            TheoryData<Expression, Expression> data = new TheoryData<Expression, Expression>();

            // identical primitive expressions
            // constants
            {
                Expression constExpr = Expression.MakeConstant(types.Int32, 5);
                data.Add(constExpr, constExpr);
                
                constExpr = Expression.MakeConstant(types.Double, -1.5);
                data.Add(constExpr, constExpr);
                
                constExpr = Expression.MakeConstant(types.Object, null);
                data.Add(constExpr, constExpr);
                
                constExpr = Expression.MakeConstant(types.String, null);
                data.Add(constExpr, constExpr);
            }
            // variables
            {
                Expression varExpr = Expression.MakeVariable(new NamedVariable(types.Int32, "x"));
                data.Add(varExpr, varExpr);
            }

            // equivalent primitive expressions
            // constants
            {
                data.Add(Expression.MakeConstant(types.Int32, 5), Expression.MakeConstant(types.Int32, 5));

                data.Add(Expression.MakeConstant(types.Double, -1.5), Expression.MakeConstant(types.Double, -1.5));

                data.Add(Expression.MakeConstant(types.Object, null), Expression.MakeConstant(types.Object, null));

                data.Add(Expression.MakeConstant(types.String, null), Expression.MakeConstant(types.String, null));
            }
            // variables
            {
                data.Add(Expression.MakeVariable(new NamedVariable(types.Int32, "x")), Expression.MakeVariable(new NamedVariable(types.Int32, "x")));
            }

            // identical complex expressions
            {
                Expression eqExpr = Expression.MakeEqual(
                    Expression.MakeConstant(types.Int32, 5),
                    Expression.MakeConstant(types.Int32, 5));
                data.Add(eqExpr, eqExpr);

                eqExpr = Expression.MakeEqual(
                    Expression.MakeConstant(types.Double, -1.5),
                    Expression.MakeVariable(new NamedVariable(types.Double, "x")));
                data.Add(eqExpr, eqExpr);
            }

            // equivalent complex expressions
            {
                Expression eqExpr1 = Expression.MakeEqual(
                    Expression.MakeConstant(types.Int32, 5),
                    Expression.MakeConstant(types.Int32, 5));
                Expression eqExpr2 = Expression.MakeEqual(
                    Expression.MakeConstant(types.Int32, 5),
                    Expression.MakeConstant(types.Int32, 5));
                data.Add(eqExpr1, eqExpr2);

                eqExpr1 = Expression.MakeEqual(
                    Expression.MakeConstant(types.Double, -1.5),
                    Expression.MakeVariable(new NamedVariable(types.Double, "x")));
                eqExpr2 = Expression.MakeEqual(
                    Expression.MakeConstant(types.Double, -1.5),
                    Expression.MakeVariable(new NamedVariable(types.Double, "x")));
                data.Add(eqExpr1, eqExpr2);

                eqExpr1 = Expression.MakeEqual(
                    Expression.MakeVariable(new NamedVariable(types.Double, "y")),
                    Expression.MakeVariable(new NamedVariable(types.Double, "x")));
                eqExpr2 = Expression.MakeEqual(
                    Expression.MakeVariable(new NamedVariable(types.Double, "y")),
                    Expression.MakeVariable(new NamedVariable(types.Double, "x")));
                data.Add(eqExpr1, eqExpr2);
            }

            return data;
        }
    }
}
