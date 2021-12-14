using dnWalker.Parameters.Expressions;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Parameters.Tests.Expressions
{
    public class WithTraitsExtensionTests
    {
        [Theory, CombinatorialData]
        public void Test_ValueOf_Trait([CombinatorialValues(5.5, 6.3)] double oldValue, [CombinatorialValues(5.5, 6.3)] double newValue)
        {
            DoubleParameter parameter = new DoubleParameter(oldValue);
            parameter.Value.Should().Be(oldValue);

            ParameterStore store = new ParameterStore();
            store.AddParameter(parameter);

            ValueOfParameterExpression valueOf = ParameterExpression.MakeValueOf(parameter);
            ParameterTrait valueOfTrait = new ParameterTrait(valueOf, newValue);

            store.WithTraits(new ParameterTrait[] { valueOfTrait });

            parameter.Value.Should().Be(newValue);
        }

        [Fact]
        public void Test_ValueOf_ParameterNotInStore()
        {
            double oldValue = 5.5;
            double newValue = 6.5;

            DoubleParameter parameter = new DoubleParameter(oldValue);
            parameter.Value.Should().Be(oldValue);

            ParameterStore store = new ParameterStore();

            ValueOfParameterExpression valueOf = ParameterExpression.MakeValueOf(parameter);
            ParameterTrait valueOfTrait = new ParameterTrait(valueOf, newValue);

            Assert.Throws<Exception>(() => store.WithTraits(new ParameterTrait[] { valueOfTrait }));
        }

        [Theory, CombinatorialData]
        public void Test_IsNull_Trait([CombinatorialValues(true, false)]bool oldIsNullValue, [CombinatorialValues(true, false)] bool newIsNullValue)
        {
            ObjectParameter parameter = new ObjectParameter("MyClass") { IsNull = oldIsNullValue };
            parameter.GetIsNull().Should().Be(oldIsNullValue); 

            ParameterStore store = new ParameterStore();
            store.AddParameter(parameter);

            IsNullParameterExpression isNull = ParameterExpression.MakeIsNull(parameter);
            ParameterTrait isNullTrait = new ParameterTrait(isNull, newIsNullValue);

            store.WithTraits(new ParameterTrait[] { isNullTrait });

            parameter.GetIsNull().Should().Be(newIsNullValue);
        }

        [Fact]
        public void Test_IsNull_ParameterNotInStore()
        {
            ObjectParameter parameter = new ObjectParameter("MyClass") { IsNull = false };
            parameter.GetIsNull().Should().Be(false);

            ParameterStore store = new ParameterStore();

            IsNullParameterExpression isNull = ParameterExpression.MakeIsNull(parameter);
            ParameterTrait isNullTrait = new ParameterTrait(isNull, true);

            Assert.Throws<Exception>(() => store.WithTraits(new ParameterTrait[] { isNullTrait }));
        }

        [Theory, CombinatorialData]
        public void Test_LengthOf_Trait([CombinatorialValues(3, 10)] int oldLengthValue, [CombinatorialValues(3, 10)] int newLengthValue)
        {
            ArrayParameter parameter = new ArrayParameter("MyClass") { Length = oldLengthValue };
            parameter.GetLength().Should().Be(oldLengthValue);

            ParameterStore store = new ParameterStore();
            store.AddParameter(parameter);

            LengthOfParameterExpression lengthOf = ParameterExpression.MakeLengthOf(parameter);
            ParameterTrait lengthTrait = new ParameterTrait(lengthOf, newLengthValue);

            store.WithTraits(new ParameterTrait[] { lengthTrait });

            parameter.GetLength().Should().Be(newLengthValue);
        }

        [Fact]
        public void Test_LengthOf_ParameterNotInStore()
        {
            ArrayParameter parameter = new ArrayParameter("MyClass") { Length = 0 };
            parameter.GetLength().Should().Be(0);

            ParameterStore store = new ParameterStore();

            LengthOfParameterExpression lengthOf = ParameterExpression.MakeLengthOf(parameter);
            ParameterTrait lengthTrait = new ParameterTrait(lengthOf, 10);

            Assert.Throws<Exception>(() => store.WithTraits(new ParameterTrait[] { lengthTrait }));
        }

        // tests for traits with RefEquals expression
        // 1 result is TRUE
        // 1.1 lhs is an alias of rhs
        // => should do nothing
        [Fact]
        public void Test_RefEquals_Trait_True_LhsIsAliasOfRhs()
        {
            ParameterStore store = new ParameterStore();

            ObjectParameter rhs = new ObjectParameter("MyClass") { IsNull = false };
            IAliasParameter lhs = rhs.CreateAlias(store);

            lhs.ParameterReferenceEquals(rhs).Should().BeTrue();

            store.AddParameter(rhs);

            RefEqualsParameterExpression refEquals = ParameterExpression.MakeRefEquals(lhs, rhs);
            ParameterTrait refEqualsTrait = new ParameterTrait(refEquals, true);

            store.WithTraits(new ParameterTrait[] { refEqualsTrait });

            // lhs / rhs relationship has not changed
            lhs.ParameterReferenceEquals(rhs).Should().BeTrue();

            // store is unchanged !!
            IParameter[] storedParameters = store.GetAllParameters().ToArray();
            storedParameters.Should().HaveCount(2);
            storedParameters.Should().Contain(lhs);
            storedParameters.Should().Contain(rhs);
        }

        // 1.2 rhs is an alias of lhs
        // => should do nothing
        [Fact]
        public void Test_RefEquals_Trait_True_RhsIsAliasOfLhs()
        {
            ParameterStore store = new ParameterStore();

            ObjectParameter lhs = new ObjectParameter("MyClass") { IsNull = false };
            IAliasParameter rhs = lhs.CreateAlias(store);

            lhs.ParameterReferenceEquals(rhs).Should().BeTrue();

            store.AddParameter(lhs);

            RefEqualsParameterExpression refEquals = ParameterExpression.MakeRefEquals(lhs, rhs);
            ParameterTrait refEqualsTrait = new ParameterTrait(refEquals, true);

            store.WithTraits(new ParameterTrait[] { refEqualsTrait });

            // lhs / rhs relationship has not changed
            lhs.ParameterReferenceEquals(rhs).Should().BeTrue();

            // store is unchanged !!
            IParameter[] storedParameters = store.GetAllParameters().ToArray();
            storedParameters.Should().HaveCount(2);
            storedParameters.Should().Contain(lhs);
            storedParameters.Should().Contain(rhs);
        }

        // 1.3 both lhs and rhs are aliaes of the same parameter
        // => should do nothing
        [Fact]
        public void Test_RefEquals_Trait_True_RhsAndLhsAliasOfTheSameParameter()
        {
            ParameterStore store = new ParameterStore();

            ObjectParameter refP = new ObjectParameter("MyClass") { IsNull = false };
            IAliasParameter lhs = refP.CreateAlias(store);
            IAliasParameter rhs = refP.CreateAlias(store);

            lhs.ParameterReferenceEquals(rhs).Should().BeTrue();

            store.AddParameter(refP);

            RefEqualsParameterExpression refEquals = ParameterExpression.MakeRefEquals(lhs, rhs);
            ParameterTrait refEqualsTrait = new ParameterTrait(refEquals, true);

            store.WithTraits(new ParameterTrait[] { refEqualsTrait });

            // lhs / rhs relationship has not changed
            lhs.ParameterReferenceEquals(rhs).Should().BeTrue();

            // store is unchanged !!
            IParameter[] storedParameters = store.GetAllParameters().ToArray();
            storedParameters.Should().HaveCount(3);
            storedParameters.Should().Contain(refP);
            storedParameters.Should().Contain(lhs);
            storedParameters.Should().Contain(rhs);
        }

        // 1.4 lhs is an alias but not of rhs
        // => should change the lhs referenced parameter
        [Fact]
        public void Test_RefEquals_Trait_True_LhsIsNotAliasOfRhs()
        {
            ParameterStore store = new ParameterStore();

            ObjectParameter refP = new ObjectParameter("MyClass") { IsNull = false };
            ObjectParameter rhs = new ObjectParameter("MyClass") { IsNull = false };
            store.AddParameter(refP);
            store.AddParameter(rhs);

            IAliasParameter lhs = refP.CreateAlias(store);

            lhs.ParameterReferenceEquals(rhs).Should().BeFalse();
            lhs.IsAliasOf(refP).Should().BeTrue();

            RefEqualsParameterExpression refEquals = ParameterExpression.MakeRefEquals(lhs, rhs);
            ParameterTrait refEqualsTrait = new ParameterTrait(refEquals, true);

            store.WithTraits(new ParameterTrait[] { refEqualsTrait });

            // lhs should no longer be an alias of refP
            lhs.IsAliasOf(refP).Should().BeFalse();
            lhs.IsAliasOf(rhs).Should().BeTrue();

            lhs.ParameterReferenceEquals(rhs).Should().BeTrue();
        }

        // 1.5 rhs is an alias but not of lhs
        // => should change the rhs referenced parameter
        [Fact]
        public void Test_RefEquals_Trait_True_RhsIsNotAliasOfLhs()
        {
            ParameterStore store = new ParameterStore();

            ObjectParameter refP = new ObjectParameter("MyClass") { IsNull = false };
            ObjectParameter lhs = new ObjectParameter("MyClass") { IsNull = false };
            store.AddParameter(refP);
            store.AddParameter(lhs);

            IAliasParameter rhs = refP.CreateAlias(store);

            rhs.ParameterReferenceEquals(lhs).Should().BeFalse();
            rhs.IsAliasOf(refP).Should().BeTrue();

            RefEqualsParameterExpression refEquals = ParameterExpression.MakeRefEquals(lhs, rhs);
            ParameterTrait refEqualsTrait = new ParameterTrait(refEquals, true);

            store.WithTraits(new ParameterTrait[] { refEqualsTrait });

            // lhs should no longer be an alias of refP
            rhs.IsAliasOf(refP).Should().BeFalse();
            rhs.IsAliasOf(lhs).Should().BeTrue();

            lhs.ParameterReferenceEquals(rhs).Should().BeTrue();
        }

        // 1.6 both lhs and rhs are aliases but of different parameters
        // => should change the lhs referenced parameter
        [Fact]
        public void Test_RefEquals_Trait_True_LhsAndRhsAreAliases()
        {
            ParameterStore store = new ParameterStore();

            ObjectParameter refL = new ObjectParameter("MyClass");
            ObjectParameter refR = new ObjectParameter("MyClass");
            store.AddParameter(refL);
            store.AddParameter(refR);

            IAliasParameter lhs = refL.CreateAlias(store);
            IAliasParameter rhs = refR.CreateAlias(store);

            lhs.IsAliasOf(refL).Should().BeTrue();
            rhs.IsAliasOf(refR).Should().BeTrue();
            lhs.IsAliasOf(refR).Should().BeFalse();
            rhs.IsAliasOf(refL).Should().BeFalse();

            RefEqualsParameterExpression refEquals = ParameterExpression.MakeRefEquals(lhs, rhs);
            ParameterTrait refEqualsTrait = new ParameterTrait(refEquals, true);

            store.WithTraits(new ParameterTrait[] { refEqualsTrait });

            // LHS should be alias of refR and no longer be an alias of refL
            // RHS should be alias of refR and still not be an alias of refL

            lhs.IsAliasOf(refL).Should().BeFalse();
            rhs.IsAliasOf(refR).Should().BeTrue();
            lhs.IsAliasOf(refR).Should().BeTrue();
            rhs.IsAliasOf(refL).Should().BeFalse();

            lhs.ParameterReferenceEquals(rhs).Should().BeTrue();
        }

        // 1.7 both lhs and rhs are not an alias
        // 1.7.1 lhs is object parameter, rhs is object parameter
        // => all method results and fields of lhs should be transfered to rhs (no overwriting)
        // => all aliases of lhs point to rhs
        // => lhs is removed from the store and a new alias parameter with the same id is added, points to rhs

        // 1.7.2 lhs is object parameter, rhs is array parameter
        // => should fail

        // 1.7.3 lhs is array parameter, rhs is object parameter
        // => should fail

        // 1.7.4 lhs is array parameter, rhs is array parameter
        // => all items of lhs should be transfered to rhs (no overwriting)
        // => all aliases of lhs point to rhs
        // => lhs is removed from the store and a new alias parameter with the same id is added, points to rhs

        // 2 result is FALSE
        // 2.1 lhs is an alias of rhs
        // => should create a shallowcopy of rhs with same id as lhs and replace the lhs parameter in the store

        // 2.2 rhs is an alias of lhs
        // => should create a shallowcopy of lhs with same id as rhs and replace the rhs parameter in the store

        // 2.3 both lhs and rhs are aliases of the same parameter
        // => should create a shallowcopy of rhs with same id as lhs and replace the lhs parameter in the store

        // 2.4 lhs is an alias but not of rhs
        // => should do nothing

        // 2.5 rhs is an alias but not of lhs
        // => should do nothing

        // 2.6 both lhs and rhs are aliases but of different parameters
        // => should do nothing

        // 2.7 both lhs and rhs are not an alias
        // => should no nothing
    }
}
