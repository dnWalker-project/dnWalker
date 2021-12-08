using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Parameters.Tests
{
    public class ObjectParameterTests : MethodResolverParameterTests<ObjectParameter>
    {
        protected override ObjectParameter Create(int id)
        {
            return new ObjectParameter(typeof(MyClass).FullName!, id);
        }

        [Theory]
        [InlineData("MyNamespace.MyClass")]
        public void TypeName_ShouldBeEqual_ToTheConstructorParameter(string typeName)
        {
            ObjectParameter objectParameter = new ObjectParameter(typeName);

            objectParameter.TypeName.Should().BeEquivalentTo(typeName);
        }


        [Theory]
        [InlineData("MyNamespace.MyClass")]
        public void UninitializedField_Should_Be_Null(string typeName)
        {
            ObjectParameter objectParameter = new ObjectParameter(typeName);

            objectParameter.TryGetField("field", out IParameter? fieldParameter).Should().BeFalse();
            fieldParameter.Should().BeNull();
        }

        [Theory]
        [InlineData("MyNamespace.MyClass")]
        public void InitializedField_Should_Be_SameAs_FieldParameter(string typeName)
        {
            const string fieldName = "field";

            ObjectParameter objectParameter = new ObjectParameter(typeName);

            Parameter fieldParameter = new BooleanParameter(false);

            objectParameter.SetField(fieldName, fieldParameter);

            objectParameter.TryGetField(fieldName, out IParameter? p).Should().BeTrue();
            p.Should().BeSameAs(fieldParameter);
        }

        [Theory]
        [InlineData("MyNamespace.MyClass")]
        public void After_SetField_Accessor_ShouldBe_FieldAccessor(string typeName)
        {
            const String FieldName = "field";

            ObjectParameter objectParameter = new ObjectParameter(typeName);

            Parameter fieldParameter = new BooleanParameter(false);

            objectParameter.SetField(FieldName, fieldParameter);

            fieldParameter.Accessor.Should().BeOfType<FieldParameterAccessor>();
        }

        [Theory]
        [InlineData("MyNamespace.MyClass")]
        public void After_SetField_AccessorFieldName_ShouldBe_FieldName(string typeName)
        {
            const String FieldName = "field";

            ObjectParameter objectParameter = new ObjectParameter(typeName);

            Parameter fieldParameter = new BooleanParameter(false);

            objectParameter.SetField(FieldName, fieldParameter);

            FieldParameterAccessor accessor = (FieldParameterAccessor)fieldParameter.Accessor!;

            accessor.FieldName.Should().Be(FieldName);
        }

        [Theory]
        [InlineData("MyNamespace.MyClass")]
        public void After_SetField_AccessorParent_ShouldBeSameAs_Object(string typeName)
        {
            const String FieldName = "field";

            ObjectParameter objectParameter = new ObjectParameter(typeName);

            Parameter fieldParameter = new BooleanParameter(false);

            objectParameter.SetField(FieldName, fieldParameter);

            fieldParameter.Accessor!.Parent.Should().BeSameAs(objectParameter);
        }
    }
}
