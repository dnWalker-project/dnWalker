using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using dnWalker.Parameters;
using dnWalker.Parameters.Xml;

using Xunit;

using FluentAssertions;

namespace dnWalker.Parameters.Tests.Xml
{
    public class XmlSerializerTests
    {

        [Theory]
        [ClassData(typeof(SerializationTestDataProvider.PrimitiveValueParameterProvider))]
        public void Test_PrimitiveValueParameterSerialization(Parameter parameter, string expectedXml)
        {
            parameter.ToXml().ToString().ToLower().Should().BeEquivalentTo(expectedXml.ToLower());
        }


        [Theory]
        [ClassData(typeof(SerializationTestDataProvider.ObjectParameterProvider))]
        public void Test_ObjectParameterSerialization(Parameter parameter, string expectedXml)
        {
            string xml = parameter.ToXml().ToString();
            xml.ToLower().Should().BeEquivalentTo(expectedXml.ToLower());
        }
    }
}
