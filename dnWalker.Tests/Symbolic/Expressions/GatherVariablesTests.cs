using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Expressions.Utils;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using IVariable = dnWalker.Symbolic.IVariable;

namespace dnWalker.Tests.Symbolic.Expressions
{
    public class GatherVariablesTests
    {

        private readonly ICorLibTypes _types = ModuleDefMD.Load(typeof(PrintingTests).Assembly.Modules.First()).CorLibTypes;

        [Fact]
        public void VariableOnly()
        {
            IVariable var = new NamedVar(_types.Int32, "x");

            Expression expr = Expression.Variable(var);
            ICollection<IVariable> vars = VariableGatherer.GetVariables(expr);

            vars.Should().Contain(var);
        }

        [Fact]
        public void VariableInBinaryOperation()
        {
            IVariable var = new NamedVar(_types.Int32, "x");

            Expression expr = Expression.Add(Expression.Constant(5), Expression.Variable(var));
            ICollection<IVariable> vars = VariableGatherer.GetVariables(expr);

            vars.Should().Contain(var);
        }

        [Fact]
        public void VariableInUnaryOperation()
        {
            IVariable var = new NamedVar(_types.Int32, "x");

            Expression expr = Expression.Negate(Expression.Multiply(Expression.Constant(5), Expression.Variable(var)));
            ICollection<IVariable> vars = VariableGatherer.GetVariables(expr);

            vars.Should().Contain(var);
        }

        [Fact]
        public void VariableInLength()
        {
            IVariable var = new NamedVar(new SZArraySig(_types.Int32), "x");
            Expression expr = Expression.Length(Expression.Variable(var));
            ICollection<IVariable> vars = VariableGatherer.GetVariables(expr);

            vars.Should().Contain(var);
        }

        [Fact]
        public void VariableInLocation()
        {
            IVariable var = new NamedVar(new SZArraySig(_types.String), "x");
            Expression expr = Expression.Location(Expression.Variable(var));
            ICollection<IVariable> vars = VariableGatherer.GetVariables(expr);

            vars.Should().Contain(var);
        }
    }
}
