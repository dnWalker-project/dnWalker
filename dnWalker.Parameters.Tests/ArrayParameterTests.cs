using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Parameters.Tests
{
    public class ArrayParameterTests : ReferenceTypeParameterTests<ArrayParameter>
    {
        protected override ArrayParameter Create(int id = 5)
        {
            return new ArrayParameter(typeof(int).FullName!, id);
        }

        [Fact]
        public void After_SetItem_LengthIsGreaterThanTheIndex()
        {
            ArrayParameter array = new ArrayParameter("System.Boolean");

            IParameter itemParameter = new BooleanParameter(false);

            array.SetItem(5, itemParameter);

            array.Length.Should().BeGreaterThan(5);
        }

        [Fact]
        public void GetItems_ReturnsAnArrayWithSameLength_As_LengthProperty()
        {
            ArrayParameter array = new ArrayParameter("System.Boolean");
            array.Length = 10;

            IParameter?[] items = array.GetItems();
            items.Length.Should().Be(10);
        }

        [Fact]
        public void After_SetItem_ItemAccessor_ShouldBe_ItemAccessor()
        {
            ArrayParameter array = new ArrayParameter("System.Boolean");
            IParameter itemParameter = new BooleanParameter(false);

            array.SetItem(5, itemParameter);

            itemParameter.Accessor.Should().BeOfType<ItemParameterAccessor>();
        }

        [Fact]
        public void After_SetItem_ItemAccessorParent_ShouldBe_SameAsArray()
        {
            ArrayParameter array = new ArrayParameter("System.Boolean");
            IParameter itemParameter = new BooleanParameter(false);

            array.SetItem(5, itemParameter);

            ItemParameterAccessor parameterAccessor = (ItemParameterAccessor)itemParameter.Accessor!;
            parameterAccessor.Parent.Should().BeSameAs(array);
        }

        [Fact]
        public void After_SetItem_ItemAccessorIndex_ShouldBe_Index()
        {
            ArrayParameter array = new ArrayParameter("System.Boolean");
            IParameter itemParameter = new BooleanParameter(false);

            array.SetItem(5, itemParameter);

            ItemParameterAccessor parameterAccessor = (ItemParameterAccessor)itemParameter.Accessor!;
            parameterAccessor.Index.Should().Be(5);
        }
    }
}
