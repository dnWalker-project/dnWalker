using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Expressions.Utils;
using dnWalker.Symbolic.Variables;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using IVariable = dnWalker.Symbolic.IVariable;

namespace dnWalker.Symbolic.Tests.Expressions
{
    public class GatherVariablesTests
    {

        private readonly ICorLibTypes _types = ModuleDefMD.Load(typeof(PrintingTests).Assembly.Modules.First()).CorLibTypes;

        [Fact]
        public void VariableOnly()
        {
            IVariable var = new NamedVariable(_types.Int32, "x");

            Expression expr = Expression.MakeVariable(var);
            ICollection<IVariable> vars = VariableGatherer.GetVariables<HashSet<IVariable>>(expr);

            vars.Should().Contain(var);
        }

        [Fact]
        public void VariableInBinaryOperation()
        {
            IVariable var = new NamedVariable(_types.Int32, "x");

            Expression expr = Expression.MakeAdd(Expression.MakeConstant(_types.Int32, 5), Expression.MakeVariable(var));
            ICollection<IVariable> vars = VariableGatherer.GetVariables<HashSet<IVariable>>(expr);

            vars.Should().Contain(var);
        }

        [Fact]
        public void VariableInUnaryOperation()
        {
            IVariable var = new NamedVariable(_types.Int32, "x");

            Expression expr = Expression.MakeNegate(Expression.MakeMultiply(Expression.MakeConstant(_types.Int32, 5), Expression.MakeVariable(var)));
            ICollection<IVariable> vars = VariableGatherer.GetVariables<HashSet<IVariable>>(expr);

            vars.Should().Contain(var);
        }

        // TODO: not yet implemented
        //[Fact]
        //public void VariableInLength()
        //{
        //    IVariable var = new NamedVariable(new SZArraySig(_types.Int32), "x");
        //    Expression expr = Expression.MakeStringLength(Expression.MakeVariable(var));
        //    ICollection<IVariable> vars = VariableGatherer.GetVariables<HashSet<IVariable>>(expr);

        //    vars.Should().Contain(var);
        //}
    }
}
