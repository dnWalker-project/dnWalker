using dnWalker.Parameters.Xml;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Xunit;

namespace dnWalker.Parameters.Tests.Xml
{
    public class XmlDeserializerTests
    {
        private static bool AreEquivalent(PrimitiveValueParameter? p1, PrimitiveValueParameter? p2)
        {
            if (p1 != null && p2 != null)
            {
                return Equals(p1.GetType(), p2.GetType()) &&
                    p1.FullName == p2.FullName &&
                    Equals(p1.GetValue(), p2.GetValue());
            }
            else
            {
                return Equals(p1, p2);
            }
        }

        [Theory]
        [ClassData(typeof(SerializationTestDataProvider.PrimitiveValueParameterProvider))]
        public void Test_PrimitiveValueParameterDeserialization(PrimitiveValueParameter expectedParameter, string xml)
        {
            AreEquivalent(XElement.Parse(xml).ToParameter() as PrimitiveValueParameter, expectedParameter).Should().BeTrue();
        }


        [Theory]
        [ClassData(typeof(SerializationTestDataProvider.ObjectParameterProvider))]
        public void Test_ObjectParameterDeserialization(ObjectParameter expected, string xml)
        {
            ObjectParameter? actual = XElement.Parse(xml).ToParameter() as ObjectParameter;

            actual.Should().NotBeNull();

            actual!.FullName.Should().Be(expected.FullName);
            actual.GetKnownFields().Count().Should().Be(expected.GetKnownFields().Count());


        }
    }
}
