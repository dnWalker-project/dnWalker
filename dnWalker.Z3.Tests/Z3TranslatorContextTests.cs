using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Expressions.Utils;

using FluentAssertions;

using Microsoft.Z3;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using IVariable = dnWalker.Symbolic.IVariable;
namespace dnWalker.Z3.Tests
{
    public class Z3TranslatorContextTests
    {
        private readonly ICorLibTypes _types = ModuleDefMD.Load(typeof(Z3TranslatorContextTests).Assembly.Modules.First()).CorLibTypes;

        [Fact]
        public void Test_SetupTraitsForInteger()
        {
            IVariable variable = new NamedVar(_types.Int32, "x");

            Z3TranslatorContext context = new Z3TranslatorContext(new Context());

            context.SetupTraits(variable);

            context.GetValueTrait(variable).Should().NotBeNull();
            ((Action)(() => context.GetLocationTrait(variable))).Should().Throw<KeyNotFoundException>();
            ((Action)(() => context.GetLengthTrait(variable))).Should().Throw<KeyNotFoundException>();

            context.Constraints.Should().HaveCount(1);
        }

        [Fact]
        public void Test_SetupTraitsForBoolean()
        {
            IVariable variable = new NamedVar(_types.Boolean, "x");

            Z3TranslatorContext context = new Z3TranslatorContext(new Context());

            context.SetupTraits(variable);

            context.GetValueTrait(variable).Should().NotBeNull();
            ((Action)(() => context.GetLocationTrait(variable))).Should().Throw<KeyNotFoundException>();
            ((Action)(() => context.GetLengthTrait(variable))).Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void Test_SetupTraitsForReal()
        {
            IVariable variable = new NamedVar(_types.Double, "x");

            Z3TranslatorContext context = new Z3TranslatorContext(new Context());

            context.SetupTraits(variable);

            context.GetValueTrait(variable).Should().NotBeNull();
            ((Action)(() => context.GetLocationTrait(variable))).Should().Throw<KeyNotFoundException>();
            ((Action)(() => context.GetLengthTrait(variable))).Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void Test_SetupTraitsForArray()
        {
            IVariable variable = new NamedVar(new SZArraySig(_types.Int32), "x");

            Z3TranslatorContext context = new Z3TranslatorContext(new Context());

            context.SetupTraits(variable);

            context.GetValueTrait(variable).Should().BeOfType<IntExpr>();
            context.GetLocationTrait(variable).Should().BeOfType<IntExpr>();
            context.GetLengthTrait(variable).Should().BeOfType<IntExpr>();

            context.Constraints.Should().HaveCount(3); // location is 0 - long.max, if location is 0 then length must be 0 as well, length must be greater than or equal to 0
        }

        [Fact]
        public void Test_SetupTraitsForString()
        {
            IVariable variable = new NamedVar(_types.String, "x");

            Z3TranslatorContext context = new Z3TranslatorContext(new Context());

            context.SetupTraits(variable);

            context.GetValueTrait(variable).Should().BeOfType<SeqExpr>();
            context.GetLocationTrait(variable).Should().BeOfType<IntExpr>();
            context.GetLengthTrait(variable).Should().BeOfType<IntExpr>();

            context.Constraints.Should().HaveCount(2);
        }

        [Fact]
        public void Test_SetupTraitsForObject()
        {
            IVariable variable = new NamedVar(_types.Object, "x");

            Z3TranslatorContext context = new Z3TranslatorContext(new Context());

            context.SetupTraits(variable);

            context.GetValueTrait(variable).Should().BeOfType<IntExpr>();
            context.GetLocationTrait(variable).Should().BeOfType<IntExpr>();
            ((Action)(() => context.GetLengthTrait(variable))).Should().Throw<KeyNotFoundException>();

            context.Constraints.Should().HaveCount(1);
        }
    }
}
