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

        [Theory]
        [ClassData(typeof(SerializationTestDataProvider.PrimitiveValueParameterProvider))]
        public void Test_PrimitiveValueParameterDeserialization(Parameter expectedParameter, string xml)
        {
            XElement.Parse(xml).ToParameter().Should().Be(expectedParameter);
        }


        [Theory]
        [ClassData(typeof(SerializationTestDataProvider.ObjectParameterProvider))]
        public void Test_ObjectParameterDeserialization(Parameter expectedParameter, string xml)
        {
            XElement.Parse(xml).ToParameter().Should().Be(expectedParameter);
        }
    }
}
