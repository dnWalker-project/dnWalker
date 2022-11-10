using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Variables;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Z3.Tests
{
    public class Z3SolverTests : DnlibProvider
    {
        private class TestClass
        {
            public int A;
            public TestClass? RefField;
        }

        private class OtherTestClass
        {
            public int Y;
            public TestClass? RefField;
        }

        private readonly TypeDef _testClassTD;
        private readonly TypeDef _otherTestClassTD;

        public Z3SolverTests()
        {
            _testClassTD = Module.Find("dnWalker.Z3.Tests.Z3SolverTests/TestClass", false);
            _otherTestClassTD = Module.Find("dnWalker.Z3.Tests.Z3SolverTests/OtherTestClass", false);
        }


        [Fact]
        public void PointerEqualityOfSameType_SAT()
        {
            // (x == y) && (typeof(x) == typeof(y)) - SAT
            Constraint constraint = new Constraint();

            NamedVariable x = new NamedVariable(_testClassTD.ToTypeSig(), "x");
            NamedVariable y = new NamedVariable(_testClassTD.ToTypeSig(), "y");

            constraint.AddEqualConstraint(Expressions.MakeVariable(x), Expressions.MakeVariable(y));

            Z3Solver solver = new Z3Solver();
            IModel? model = solver.Solve(constraint);

            model!.Should().NotBeNull();
            model.TryGetValue(x, out IValue? vx).Should().BeTrue();
            model.TryGetValue(y, out IValue? vy).Should().BeTrue();

            vx.Should().Be(vy).And.Be(Location.Null);
        }

        [Fact]
        public void PointerEqualityOfDifferentTypesNotNull_UNSAT()
        {
            // (x == y) && (typeof(x) != typeof(y) && (x != null)) - UNSAT

            Constraint constraint = new Constraint();

            NamedVariable x = new NamedVariable(_testClassTD.ToTypeSig(), "x");
            NamedVariable y = new NamedVariable(_otherTestClassTD.ToTypeSig(), "y");

            Expression exprX = Expressions.MakeVariable(x);
            Expression exprY = Expressions.MakeVariable(y);

            constraint.AddEqualConstraint(exprX, exprY);
            constraint.AddNotEqualConstraint(exprX, Expressions.NullExpression);

            Z3Solver solver = new Z3Solver();
            IModel? model = solver.Solve(constraint);

            model!.Should().BeNull();
        }

        [Fact]
        public void PointerEqualityOfDifferentTypesMayBeNull_SAT_AndNull()
        {
            // (x == y) && (typeof(x) != typeof(y)) - SAT
            Constraint constraint = new Constraint();

            NamedVariable x = new NamedVariable(_testClassTD.ToTypeSig(), "x");
            NamedVariable y = new NamedVariable(_otherTestClassTD.ToTypeSig(), "y");

            Expression exprX = Expressions.MakeVariable(x);
            Expression exprY = Expressions.MakeVariable(y);

            constraint.AddEqualConstraint(exprX, exprY);
            

            Z3Solver solver = new Z3Solver();
            IModel? model = solver.Solve(constraint);

            model!.Should().NotBeNull();
            model.TryGetValue(x, out IValue? vx).Should().BeTrue();
            model.TryGetValue(y, out IValue? vy).Should().BeTrue();

            vx.Should().Be(vy).And.Be(Location.Null);
        }

        [Fact]
        public void PointerEqualityNullAndNotNull_UNSAT()
        {
            // (x == y) && (y == z) && (x == null) && (z != null) - UNSAT
            Constraint constraint = new Constraint();

            NamedVariable x = new NamedVariable(_testClassTD.ToTypeSig(), "x");
            NamedVariable y = new NamedVariable(_testClassTD.ToTypeSig(), "y");
            NamedVariable z = new NamedVariable(_testClassTD.ToTypeSig(), "z");

            Expression exprX = Expressions.MakeVariable(x);
            Expression exprY = Expressions.MakeVariable(y);
            Expression exprZ = Expressions.MakeVariable(z);

            constraint.AddEqualConstraint(exprX, exprY);
            constraint.AddEqualConstraint(exprY, exprZ);
            
            constraint.AddEqualConstraint(exprX, Expressions.NullExpression);
            constraint.AddNotEqualConstraint(exprZ, Expressions.NullExpression);

            Z3Solver solver = new Z3Solver();
            IModel? model = solver.Solve(constraint);

            model!.Should().BeNull();
        }

        [Fact]
        public void AttributeEqualityForcedByRootEquality()
        {
            // (x == y) && (x.A != y.A) - UNSAT

            Constraint constraint = new Constraint();

            NamedVariable x = new NamedVariable(_testClassTD.ToTypeSig(), "x");
            NamedVariable y = new NamedVariable(_testClassTD.ToTypeSig(), "y");

            Expression exprX = Expressions.MakeVariable(x);
            Expression exprY = Expressions.MakeVariable(y);

            Expression exprXA = Expressions.MakeVariable(Variable.InstanceField(x, _testClassTD.FindField("A")));
            Expression exprYA = Expressions.MakeVariable(Variable.InstanceField(y, _testClassTD.FindField("A")));

            constraint.AddEqualConstraint(exprX, exprY);
            constraint.AddNotEqualConstraint(exprXA, exprYA);

            Z3Solver solver = new Z3Solver();
            IModel? model = solver.Solve(constraint);

            model!.Should().BeNull();
        }

        [Fact]
        public void MemberVariableForcesRootVariableToNotBeNull()
        {
            // (x == y) && (x.A == y.A) - SAT

            Constraint constraint = new Constraint();

            NamedVariable x = new NamedVariable(_testClassTD.ToTypeSig(), "x");
            NamedVariable y = new NamedVariable(_testClassTD.ToTypeSig(), "y");

            Expression exprX = Expressions.MakeVariable(x);
            Expression exprY = Expressions.MakeVariable(y);

            Expression exprXA = Expressions.MakeVariable(Variable.InstanceField(x, _testClassTD.FindField("A")));
            Expression exprYA = Expressions.MakeVariable(Variable.InstanceField(y, _testClassTD.FindField("A")));

            constraint.AddEqualConstraint(exprX, exprY);
            constraint.AddEqualConstraint(exprXA, exprYA);

            Z3Solver solver = new Z3Solver();
            IModel? model = solver.Solve(constraint);

            model!.Should().NotBeNull();
            model.TryGetValue(x, out IValue? vx).Should().BeTrue();
            model.TryGetValue(y, out IValue? vy).Should().BeTrue();

            vx.Should().Be(vy).And.NotBe(Location.Null);
        }
    }
}
