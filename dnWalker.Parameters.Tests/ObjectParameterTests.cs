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
            return new ObjectParameter("MyNamespace.MyClass", name);
        }

        [Theory]
        [InlineData("MyNamespace.MyClass")]
        public void TypeName_ShouldBeEqual_ToTheConstructorParameter(string typeName)
        {
            ObjectParameter objectParameter = new ObjectParameter(typeName, "SomeObject");

            objectParameter.TypeName.Should().BeEquivalentTo(typeName);
        }


        [Theory]
        [InlineData("MyNamespace.MyClass")]
        public void UninitializedField_Should_Be_Null(string typeName)
        {
            ObjectParameter objectParameter = new ObjectParameter(typeName, "SomeObject");

            objectParameter.TryGetField("field", out Parameter? fieldParameter).Should().BeFalse();
            fieldParameter.Should().BeNull();
        }

        [Theory]
        [InlineData("MyNamespace.MyClass")]
        public void InitializedField_Should_Be_SameAs_FieldParameter(string typeName)
        {
            const string fieldName = "field";

            ObjectParameter objectParameter = new ObjectParameter(typeName, "SomeObject");

            Parameter fieldParameter = new BooleanParameter(fieldName, false );

            objectParameter.SetField(fieldName, fieldParameter);

            objectParameter.TryGetField(fieldName, out Parameter? p).Should().BeTrue();
            p.Should().BeSameAs(fieldParameter);
        }

        [Theory]
        [InlineData("MyNamespace.MyClass")]
        public void SettingField_WillSetParent_OfTheFieldParameter(string typeName)
        {
            const String FieldName = "field";

            ObjectParameter objectParameter = new ObjectParameter(typeName, "SomeObject");

            Parameter fieldParameter = new BooleanParameter(FieldName, false);

            objectParameter.SetField(FieldName, fieldParameter);

            fieldParameter.Parent.Should().BeSameAs(objectParameter);
        }


        [Theory]
        [InlineData("MyNamespace.MyClass")]
        public void SettingField_Should_SetName_Of_FieldParameter(string typeName)
        {
            var objectParameter = new ObjectParameter(typeName, "SomeObject");

            Parameter fieldParameter = new DoubleParameter("value", 0);

            objectParameter.SetField("value", fieldParameter);

            fieldParameter.FullName.ToString().Should().Be($"SomeObject{ParameterName.Delimiter}value");
        }
    }
}
