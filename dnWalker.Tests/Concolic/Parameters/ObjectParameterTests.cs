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
        protected override ObjectParameter Create(String name = "p")
        {
            return new ObjectParameter(typeof(Object).FullName) { Name = name };
        }

        [Theory]
        [InlineData(typeof(MyClass))]
        [InlineData(typeof(MyItem))]
        public void Test_Type_Is_EquivalentTo_WrappedType(Type systemType)
        {
            TypeSig dnLibType = GetType(systemType);

            ObjectParameter objectParameter = new ObjectParameter(dnLibType.FullName) { Name = "SomeObject" };

            objectParameter.TypeName.Should().BeEquivalentTo(dnLibType.FullName);
        }


        [Theory]
        [InlineData(typeof(MyClass))]
        [InlineData(typeof(MyItem))]
        public void UninitializedField_Should_Be_Null(Type systemType)
        {
            TypeSig dnLibType = GetType(systemType);

            ObjectParameter objectParameter = new ObjectParameter(dnLibType.FullName) { Name = "SomeObject" };

            objectParameter.TryGetField("field", out Parameter fieldParameter).Should().BeFalse();
            fieldParameter.Should().BeNull();
        }

        [Theory]
        [InlineData(typeof(MyClass))]
        [InlineData(typeof(MyItem))]
        public void InitializedField_Should_Be_SameAs_FieldParameter(Type systemType)
        {
            const String FieldName = "field";

            TypeSig dnLibType = GetType(systemType);

            ObjectParameter objectParameter = new ObjectParameter(dnLibType.FullName) { Name = "SomeObject" };

            Parameter fieldParameter = new BooleanParameter() { Value = false };

            objectParameter.SetField(FieldName, fieldParameter);

            objectParameter.TryGetField("field", out Parameter p).Should().BeTrue();
            p.Should().BeSameAs(fieldParameter);
        }
    }
}
