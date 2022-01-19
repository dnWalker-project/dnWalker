using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Parameters.Tests
{
    public class ItemOwnerImplementationTests
    {
        private static readonly int MyIndex = 2;
        private static readonly int MyOtherIndex = 5;

        private static readonly ParameterRef OwnerRef = 5;

        [Fact]
        public void After_SetItem_TryGetItem_WillOutputTheValue()
        {
            IParameterContext context = new BaseParameterContext();
            ItemOwnerImplementation itemOwner = new ItemOwnerImplementation(OwnerRef, context);
            IParameter itemValue = context.CreateInt32Parameter();


            itemOwner.TryGetItem(MyIndex, out _).Should().BeFalse("Check assumptions.");

            itemOwner.SetItem(MyIndex, itemValue.Reference);

            itemOwner.TryGetItem(MyIndex, out ParameterRef itemRef).Should().BeTrue();

            itemRef.Should().Be(itemValue.Reference);
        }

        [Fact]
        public void TryGetUninitializedItem_ReturnsFalse()
        {
            IParameterContext context = new BaseParameterContext();
            ItemOwnerImplementation itemOwner = new ItemOwnerImplementation(OwnerRef, context);

            itemOwner.TryGetItem(MyIndex, out _).Should().BeFalse();
        }

        [Fact]
        public void TryGetInitializedItem_ReturnsTrue()
        {
            IParameterContext context = new BaseParameterContext();
            ItemOwnerImplementation itemOwner = new ItemOwnerImplementation(5, context);
            IParameter itemValue = context.CreateInt32Parameter();

            itemOwner.TryGetItem(MyIndex, out _).Should().BeFalse("Check assumptions.");

            itemOwner.SetItem(MyIndex, itemValue.Reference);

            itemOwner.TryGetItem(MyIndex, out _).Should().BeTrue();
        }

        [Fact]
        public void TryGetClearedItem_ReturnsFalse()
        {
            IParameterContext context = new BaseParameterContext();
            ItemOwnerImplementation itemOwner = new ItemOwnerImplementation(OwnerRef, context);
            IParameter itemValue = context.CreateInt32Parameter();

            itemOwner.SetItem(MyIndex, itemValue.Reference);

            itemOwner.TryGetItem(MyIndex, out _).Should().BeTrue("Check assumptions.");

            itemOwner.ClearItem(MyIndex);

            itemOwner.TryGetItem(MyIndex, out _).Should().BeFalse();
        }

        [Fact]
        public void GetItems_IsNotNull()
        {
            IParameterContext context = new BaseParameterContext();
            ItemOwnerImplementation itemOwner = new ItemOwnerImplementation(OwnerRef, context);

            itemOwner.GetItems().Should().NotBeNull();
        }

        [Fact]
        public void After_SetItem_ValueWillBeInGetItemsDictionary()
        {
            IParameterContext context = new BaseParameterContext();
            ItemOwnerImplementation itemOwner = new ItemOwnerImplementation(OwnerRef, context);
            IParameter itemValue = context.CreateInt32Parameter();

            itemOwner.SetItem(MyIndex, itemValue.Reference);

            itemOwner.TryGetItem(MyIndex, out _).Should().BeTrue("Check assumptions.");

            itemOwner.GetItems().Should().Contain(itemValue.Reference);
        }

        [Fact]
        public void After_ClearItem_ValueWillNotBeInGetItemsDictionary()
        {
            IParameterContext context = new BaseParameterContext();
            ItemOwnerImplementation itemOwner = new ItemOwnerImplementation(OwnerRef, context);
            IParameter itemValue = context.CreateInt32Parameter();

            itemOwner.SetItem(MyIndex, itemValue.Reference);

            itemOwner.TryGetItem(MyIndex, out _).Should().BeTrue("Check assumptions.");

            itemOwner.GetItems()[MyIndex].Should().Be(itemValue.Reference, "Check assumptions.");

            itemOwner.ClearItem(MyIndex);

            itemOwner.TryGetItem(MyIndex, out _).Should().BeFalse("Check assumptions.");


            itemOwner.GetItems().Should().NotContain(itemValue.Reference);
        }

        [Fact]
        public void After_SetItem_ItemValueWillHave_ItemParameterAccess()
        {
            IParameterContext context = new BaseParameterContext();
            ItemOwnerImplementation itemOwner = new ItemOwnerImplementation(OwnerRef, context);
            IParameter itemValue = context.CreateInt32Parameter();

            itemOwner.SetItem(MyIndex, itemValue.Reference);

            itemOwner.TryGetItem(MyIndex, out _).Should().BeTrue("Check assumptions.");

            itemValue.Accessors.Should().HaveCountGreaterThanOrEqualTo(1);
            itemValue.Accessors[0].Should().BeOfType<ItemParameterAccessor>();
            ((ItemParameterAccessor)itemValue.Accessors[0]).ParentRef.Should().Be(OwnerRef);
            ((ItemParameterAccessor)itemValue.Accessors[0]).Index.Should().Be(MyIndex);
        }

        [Fact]
        public void Setting_MyItem_ShouldNot_Set_OtherItem()
        {
            IParameterContext context = new BaseParameterContext();
            ItemOwnerImplementation itemOwner = new ItemOwnerImplementation(OwnerRef, context);
            IParameter itemValue = context.CreateInt32Parameter();

            itemOwner.SetItem(MyIndex, itemValue.Reference);

            itemOwner.TryGetItem(MyIndex, out _).Should().BeTrue("Check assumptions.");

            itemOwner.TryGetItem(MyOtherIndex, out _).Should().BeFalse();
        }

        [Fact]
        public void Clearing_MyItem_ShouldNot_Clear_OtherItem()
        {
            IParameterContext context = new BaseParameterContext();
            ItemOwnerImplementation itemOwner = new ItemOwnerImplementation(OwnerRef, context);
            IParameter myItemValue = context.CreateInt32Parameter();
            IParameter otherItemValue = context.CreateInt32Parameter();

            itemOwner.SetItem(MyIndex, myItemValue.Reference);
            itemOwner.SetItem(MyOtherIndex, otherItemValue.Reference);

            itemOwner.TryGetItem(MyIndex, out _).Should().BeTrue("Check assumptions.");
            itemOwner.TryGetItem(MyOtherIndex, out _).Should().BeTrue("Check assumptions.");

            itemOwner.ClearItem(MyIndex);

            itemOwner.TryGetItem(MyIndex, out _).Should().BeFalse("Check assumptions.");
            itemOwner.TryGetItem(MyOtherIndex, out _).Should().BeTrue();
        }
    }
}
