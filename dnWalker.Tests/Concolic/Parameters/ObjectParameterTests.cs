using dnlib.DotNet;

using dnWalker.Concolic.Parameters;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using Parameter = dnWalker.Concolic.Parameters.Parameter;

namespace dnWalker.Tests.Concolic.Parameters
{
    public class ObjectParameterTests : ReferenceTypeParameterTests<ObjectParameter>
    {
        protected override ObjectParameter Create(string name = "p")
        {
            return new ObjectParameter(typeof(object).FullName) { Name = name };
        }

        [Theory]
        [InlineData(typeof(MyClass))]
        [InlineData(typeof(MyItem))]
        public void Test_Type_Is_EquivalentTo_WrappedType(Type systemType)
        {
            var dnLibType = GetType(systemType);

            var objectParameter = new ObjectParameter(dnLibType.FullName) { Name = "SomeObject" };

            objectParameter.TypeName.Should().BeEquivalentTo(dnLibType.FullName);
        }


        [Theory]
        [InlineData(typeof(MyClass))]
        [InlineData(typeof(MyItem))]
        public void UninitializedField_Should_Be_Null(Type systemType)
        {
            var dnLibType = GetType(systemType);

            var objectParameter = new ObjectParameter(dnLibType.FullName) { Name = "SomeObject" };

            objectParameter.TryGetField("field", out var fieldParameter).Should().BeFalse();
            fieldParameter.Should().BeNull();
        }

        [Theory]
        [InlineData(typeof(MyClass))]
        [InlineData(typeof(MyItem))]
        public void InitializedField_Should_Be_SameAs_FieldParameter(Type systemType)
        {
            const string FieldName = "field";

            var dnLibType = GetType(systemType);

            var objectParameter = new ObjectParameter(dnLibType.FullName) { Name = "SomeObject" };

            Parameter fieldParameter = new BooleanParameter() { Value = false };

            objectParameter.SetField(FieldName, fieldParameter);

            objectParameter.TryGetField("field", out var p).Should().BeTrue();
            p.Should().BeSameAs(fieldParameter);
        }

        [Fact]
        public void SettingField_Should_SetName_Of_FieldParameter()
        {
            var objectParameter = new ObjectParameter(GetType(typeof(MyClass)).FullName, "SomeObject");

            Parameter fieldParameter = new DoubleParameter();

            objectParameter.SetField("value", fieldParameter);

            fieldParameter.Name.Should().Be(ParameterName.ConstructField("SomeObject", "value"));
        }

        [Fact]
        public void ChangingName_Should_ChangeName_Of_FieldParmaters()
        {
            var objectParameter = new ObjectParameter(GetType(typeof(MyClass)).FullName, "SomeObject");

            Parameter fieldParameter = new DoubleParameter();

            objectParameter.SetField("value", fieldParameter);

            objectParameter.Name = "AnotherObject";

            fieldParameter.Name.Should().Be(ParameterName.ConstructField("AnotherObject", "value"));
        }
    }
}
