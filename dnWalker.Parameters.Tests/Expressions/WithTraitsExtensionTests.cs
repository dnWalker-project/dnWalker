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
        public void Test_WithValueOf_Trait([CombinatorialValues(5.5, 6.3)] double oldValue, [CombinatorialValues(5.5, 6.3)] double newValue)
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
        public void Test_RefEquals_Trait_True_LhsIsAliasOfRhs()
        {
            ObjectParameter rhs = new ObjectParameter("MyClass") { IsNull = false };
            IAliasParameter lhs = rhs.CreateAlias();

            lhs.ParameterReferenceEquals(rhs).Should().BeTrue();

            ParameterStore store = new ParameterStore();
            store.AddParameter(lhs);
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

        [Fact]
        public void Test_RefEquals_Trait_True_RhsIsAliasOfLhs()
        {
            ObjectParameter lhs = new ObjectParameter("MyClass") { IsNull = false };
            IAliasParameter rhs = lhs.CreateAlias();

            lhs.ParameterReferenceEquals(rhs).Should().BeTrue();

            ParameterStore store = new ParameterStore();
            store.AddParameter(lhs);
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

        [Fact]
        public void Test_RefEquals_Trait_True_RhsAndLhsAliasSameParameter()
        {
            ObjectParameter refP = new ObjectParameter("MyClass") { IsNull = false };
            IAliasParameter lhs = refP.CreateAlias();
            IAliasParameter rhs = refP.CreateAlias();

            lhs.ParameterReferenceEquals(rhs).Should().BeTrue();

            ParameterStore store = new ParameterStore();
            store.AddParameter(refP);
            store.AddParameter(lhs);
            store.AddParameter(rhs);

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
    }
}
