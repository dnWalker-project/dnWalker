using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Parameters.Tests
{
    public class ObjectParameterTests : ReferenceTypeParameterTests<ObjectParameter>
    {
        protected override ObjectParameter Create(String name = "p")
        {
            return new ObjectParameter(typeof(Object).FullName) { Name = name };
        }

        [Theory]
        [InlineData(typeof(MyClass))]
        [InlineData(typeof(MyItem))]
        public void Test_Type_Is_EquivalentTo_WrappedType(Type systemType)
        {
            ObjectParameter objectParameter = new ObjectParameter(systemType.FullName) { Name = "SomeObject" };

            objectParameter.TypeName.Should().BeEquivalentTo(systemType.FullName);
        }


        [Theory]
        [InlineData(typeof(MyClass))]
        [InlineData(typeof(MyItem))]
        public void UninitializedField_Should_Be_Null(Type systemType)
        {
            ObjectParameter objectParameter = new ObjectParameter(systemType.FullName) { Name = "SomeObject" };

            objectParameter.TryGetField("field", out Parameter fieldParameter).Should().BeFalse();
            fieldParameter.Should().BeNull();
        }

        [Theory]
        [InlineData(typeof(MyClass))]
        [InlineData(typeof(MyItem))]
        public void InitializedField_Should_Be_SameAs_FieldParameter(Type systemType)
        {
            const String FieldName = "field";

            ObjectParameter objectParameter = new ObjectParameter(systemType.FullName) { Name = "SomeObject" };

            Parameter fieldParameter = new BooleanParameter() { Value = false };

            objectParameter.SetField(FieldName, fieldParameter);

            objectParameter.TryGetField("field", out Parameter p).Should().BeTrue();
            p.Should().BeSameAs(fieldParameter);
        }


        [Fact]
        public void SettingField_Should_SetName_Of_FieldParameter()
        {
            var objectParameter = new ObjectParameter(typeof(MyClass).FullName, "SomeObject");

            Parameter fieldParameter = new DoubleParameter();

            objectParameter.SetField("value", fieldParameter);

            fieldParameter.Name.Should().Be(ParameterNameUtils.ConstructField("SomeObject", "value"));
        }

        [Fact]
        public void ChangingName_Should_ChangeName_Of_FieldParmaters()
        {
            var objectParameter = new ObjectParameter(typeof(MyClass).FullName, "SomeObject");

            Parameter fieldParameter = new DoubleParameter();

            objectParameter.SetField("value", fieldParameter);

            objectParameter.Name = "AnotherObject";

            fieldParameter.Name.Should().Be(ParameterNameUtils.ConstructField("AnotherObject", "value"));
        }
    }
}
