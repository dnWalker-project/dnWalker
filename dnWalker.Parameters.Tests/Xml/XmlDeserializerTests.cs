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
        private static bool AreEquivalent(IPrimitiveValueParameter? p1, IPrimitiveValueParameter? p2)
        {
            if (p1 != null && p2 != null)
            {
                return Equals(p1.GetType(), p2.GetType()) &&
                    p1.Id == p2.Id &&
                    Equals(p1.Value, p2.Value);
            }
            else
            {
                return Equals(p1, p2);
            }
        }

        [Theory]
        [ClassData(typeof(SerializationTestDataProvider.PrimitiveValueParameterProvider))]
        public void Test_PrimitiveValueParameterDeserialization(IPrimitiveValueParameter expectedParameter, string xml)
        {
            AreEquivalent(XElement.Parse(xml).ToParameter() as IPrimitiveValueParameter, expectedParameter).Should().BeTrue();
        }


        [Theory]
        [ClassData(typeof(SerializationTestDataProvider.ObjectParameterProvider))]
        public void Test_ObjectParameterDeserialization(ObjectParameter expected, string xml)
        {
            ObjectParameter? actual = XElement.Parse(xml).ToParameter() as ObjectParameter;

            actual.Should().NotBeNull();

            actual!.Id.Should().Be(expected.Id);
            actual.GetFields().Count().Should().Be(expected.GetFields().Count());


        }
    }
}
