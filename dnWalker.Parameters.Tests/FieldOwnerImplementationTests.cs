using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Parameters.Tests
{
    public class FieldOwnerImplementationTests
    {
        private static readonly string MyField = "MyField";
        private static readonly string OtherField = "OtherField";

        private static readonly ParameterRef OwnerRef = 5;

        [Fact]
        public void After_SetField_TryGetField_WillOutputTheValue()
        {
            IParameterContext context = new BaseParameterContext();
            FieldOwnerImplementation fieldOwner = new FieldOwnerImplementation(OwnerRef, context);
            IParameter fieldValue = context.CreateInt32Parameter();


            fieldOwner.TryGetField(MyField, out _).Should().BeFalse("Check assumptions.");

            fieldOwner.SetField(MyField, fieldValue.Reference);

            fieldOwner.TryGetField(MyField, out ParameterRef fieldRef).Should().BeTrue();

            fieldRef.Should().Be(fieldValue.Reference);
        }

        [Fact]
        public void TryGetUninitializedField_ReturnsFalse()
        {
            IParameterContext context = new BaseParameterContext();
            FieldOwnerImplementation fieldOwner = new FieldOwnerImplementation(OwnerRef, context);

            fieldOwner.TryGetField(MyField, out _).Should().BeFalse();
        }

        [Fact]
        public void TryGetInitializedField_ReturnsTrue()
        {
            IParameterContext context = new BaseParameterContext();
            FieldOwnerImplementation fieldOwner = new FieldOwnerImplementation(5, context);
            IParameter fieldValue = context.CreateInt32Parameter();

            fieldOwner.TryGetField(MyField, out _).Should().BeFalse("Check assumptions.");

            fieldOwner.SetField(MyField, fieldValue.Reference);

            fieldOwner.TryGetField(MyField, out _).Should().BeTrue();
        }

        [Fact]
        public void TryGetClearedField_ReturnsFalse()
        {
            IParameterContext context = new BaseParameterContext();
            FieldOwnerImplementation fieldOwner = new FieldOwnerImplementation(OwnerRef, context);
            IParameter fieldValue = context.CreateInt32Parameter();

            fieldOwner.SetField(MyField, fieldValue.Reference);

            fieldOwner.TryGetField(MyField, out _).Should().BeTrue("Check assumptions.");

            fieldOwner.ClearField(MyField);

            fieldOwner.TryGetField(MyField, out _).Should().BeFalse();
        }

        [Fact]
        public void GetFields_IsNotNull()
        {
            IParameterContext context = new BaseParameterContext();
            FieldOwnerImplementation fieldOwner = new FieldOwnerImplementation(OwnerRef, context);

            fieldOwner.GetFields().Should().NotBeNull();
        }

        [Fact]
        public void After_SetField_ValueWillBeInGetFieldsDictionary()
        {
            IParameterContext context = new BaseParameterContext();
            FieldOwnerImplementation fieldOwner = new FieldOwnerImplementation(OwnerRef, context);
            IParameter fieldValue = context.CreateInt32Parameter();

            fieldOwner.SetField(MyField, fieldValue.Reference);

            fieldOwner.TryGetField(MyField, out _).Should().BeTrue("Check assumptions.");

            fieldOwner.GetFields().Should().Contain(KeyValuePair.Create<string, ParameterRef>(MyField, fieldValue.Reference));
        }

        [Fact]
        public void After_ClearField_ValueWillNotBeInGetFieldsDictionary()
        {
            IParameterContext context = new BaseParameterContext();
            FieldOwnerImplementation fieldOwner = new FieldOwnerImplementation(OwnerRef, context);
            IParameter fieldValue = context.CreateInt32Parameter();

            fieldOwner.SetField(MyField, fieldValue.Reference);

            fieldOwner.TryGetField(MyField, out _).Should().BeTrue("Check assumptions.");

            fieldOwner.GetFields().Should().Contain(KeyValuePair.Create<string, ParameterRef>(MyField, fieldValue.Reference), "Check assumptions.");

            fieldOwner.ClearField(MyField);

            fieldOwner.TryGetField(MyField, out _).Should().BeFalse("Check assumptions.");


            fieldOwner.GetFields().Should().NotContain(KeyValuePair.Create<string, ParameterRef>(MyField, fieldValue.Reference));
        }

        [Fact]
        public void After_SetField_FieldValueWillHave_FieldParameterAccess()
        {
            IParameterContext context = new BaseParameterContext();
            FieldOwnerImplementation fieldOwner = new FieldOwnerImplementation(OwnerRef, context);
            IParameter fieldValue = context.CreateInt32Parameter();

            fieldOwner.SetField(MyField, fieldValue.Reference);

            fieldOwner.TryGetField(MyField, out _).Should().BeTrue("Check assumptions.");

            fieldValue.Accessors.Should().HaveCountGreaterThanOrEqualTo(1);
            fieldValue.Accessors[0].Should().BeOfType<FieldParameterAccessor>();
            ((FieldParameterAccessor)fieldValue.Accessors[0]).ParentRef.Should().Be(OwnerRef);
            ((FieldParameterAccessor)fieldValue.Accessors[0]).FieldName.Should().Be(MyField);
        }

        [Fact]
        public void Setting_MyField_ShouldNot_Set_OtherField()
        {
            IParameterContext context = new BaseParameterContext();
            FieldOwnerImplementation fieldOwner = new FieldOwnerImplementation(OwnerRef, context);
            IParameter fieldValue = context.CreateInt32Parameter();

            fieldOwner.SetField(MyField, fieldValue.Reference);

            fieldOwner.TryGetField(MyField, out _).Should().BeTrue("Check assumptions.");

            fieldOwner.TryGetField(OtherField, out _).Should().BeFalse();
        }

        [Fact]
        public void Clearing_MyField_ShouldNot_Clear_OtherField()
        {
            IParameterContext context = new BaseParameterContext();
            FieldOwnerImplementation fieldOwner = new FieldOwnerImplementation(OwnerRef, context);
            IParameter myFieldValue = context.CreateInt32Parameter();
            IParameter otherFieldValue = context.CreateInt32Parameter();

            fieldOwner.SetField(MyField, myFieldValue.Reference);
            fieldOwner.SetField(OtherField, otherFieldValue.Reference);

            fieldOwner.TryGetField(MyField, out _).Should().BeTrue("Check assumptions.");
            fieldOwner.TryGetField(OtherField, out _).Should().BeTrue("Check assumptions.");

            fieldOwner.ClearField(MyField);

            fieldOwner.TryGetField(MyField, out _).Should().BeFalse("Check assumptions.");
            fieldOwner.TryGetField(OtherField, out _).Should().BeTrue();
        }
    }
}
