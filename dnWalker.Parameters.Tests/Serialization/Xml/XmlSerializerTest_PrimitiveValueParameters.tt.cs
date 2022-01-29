﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using dnWalker.Parameters;
using dnWalker.Parameters.Serialization.Xml;

using FluentAssertions;

using Xunit;

namespace dnWalker.Parameters.Tests.Serialization.Xml
{
    public class XmlSerializerTest_PrimitiveValueParameters : TestBase
    {

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.Boolean\" Reference=\"0x00000000\" Value=\"True\" />", 0, true)]  
        [InlineData("<PrimitiveValue Type=\"System.Boolean\" Reference=\"0x00000001\" Value=\"False\" />", 1, false)]
        public void Test_BooleanParameter(string xml, int intRef, bool value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            IParameterSet set = new ParameterSet(context);

            XmlSerializer ser = new XmlSerializer();

            IBooleanParameter? p = set.CreateBooleanParameter(reference);
            p.Value = value;

            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);

            XElement serialization = ser.ToXml(p);
            XNode.DeepEquals(serialization, XElement.Parse(xml)).Should().BeTrue();
        }

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.Char\" Reference=\"0x00000000\" Value=\"U+0000\" />", 0, '\0')]  
        [InlineData("<PrimitiveValue Type=\"System.Char\" Reference=\"0x00000001\" Value=\"U+000A\" />", 1, '\n')]  
        [InlineData("<PrimitiveValue Type=\"System.Char\" Reference=\"0x00000002\" Value=\"U+0061\" />", 2, 'a')]  
        [InlineData("<PrimitiveValue Type=\"System.Char\" Reference=\"0x00000003\" Value=\"U+0000\" />", 3, char.MinValue)]  
        [InlineData("<PrimitiveValue Type=\"System.Char\" Reference=\"0x00000004\" Value=\"U+FFFF\" />", 4, char.MaxValue)]
        public void Test_CharParameter(string xml, int intRef, char value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            IParameterSet set = new ParameterSet(context);

            XmlSerializer ser = new XmlSerializer();

            ICharParameter? p = set.CreateCharParameter(reference);
            p.Value = value;

            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);

            XElement serialization = ser.ToXml(p);
            XNode.DeepEquals(serialization, XElement.Parse(xml)).Should().BeTrue();
        }

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.Byte\" Reference=\"0x00000000\" Value=\"0\" />", 0, byte.MinValue)]  
        [InlineData("<PrimitiveValue Type=\"System.Byte\" Reference=\"0x00000001\" Value=\"255\" />", 1, byte.MaxValue)]  
        [InlineData("<PrimitiveValue Type=\"System.Byte\" Reference=\"0x00000002\" Value=\"127\" />", 2, 127)]
        public void Test_ByteParameter(string xml, int intRef, byte value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            IParameterSet set = new ParameterSet(context);

            XmlSerializer ser = new XmlSerializer();

            IByteParameter? p = set.CreateByteParameter(reference);
            p.Value = value;

            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);

            XElement serialization = ser.ToXml(p);
            XNode.DeepEquals(serialization, XElement.Parse(xml)).Should().BeTrue();
        }

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.SByte\" Reference=\"0x00000000\" Value=\"-128\" />", 0, sbyte.MinValue)]  
        [InlineData("<PrimitiveValue Type=\"System.SByte\" Reference=\"0x00000001\" Value=\"127\" />", 1, sbyte.MaxValue)]  
        [InlineData("<PrimitiveValue Type=\"System.SByte\" Reference=\"0x00000002\" Value=\"63\" />", 2, 63)]  
        [InlineData("<PrimitiveValue Type=\"System.SByte\" Reference=\"0x00000003\" Value=\"-63\" />", 3, -63)]
        public void Test_SByteParameter(string xml, int intRef, sbyte value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            IParameterSet set = new ParameterSet(context);

            XmlSerializer ser = new XmlSerializer();

            ISByteParameter? p = set.CreateSByteParameter(reference);
            p.Value = value;

            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);

            XElement serialization = ser.ToXml(p);
            XNode.DeepEquals(serialization, XElement.Parse(xml)).Should().BeTrue();
        }

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.Int16\" Reference=\"0x00000000\" Value=\"-32768\" />", 0, short.MinValue)]  
        [InlineData("<PrimitiveValue Type=\"System.Int16\" Reference=\"0x00000001\" Value=\"32767\" />", 1, short.MaxValue)]  
        [InlineData("<PrimitiveValue Type=\"System.Int16\" Reference=\"0x00000002\" Value=\"1023\" />", 2, 1023)]  
        [InlineData("<PrimitiveValue Type=\"System.Int16\" Reference=\"0x00000003\" Value=\"-1023\" />", 3, -1023)]
        public void Test_Int16Parameter(string xml, int intRef, short value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            IParameterSet set = new ParameterSet(context);

            XmlSerializer ser = new XmlSerializer();

            IInt16Parameter? p = set.CreateInt16Parameter(reference);
            p.Value = value;

            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);

            XElement serialization = ser.ToXml(p);
            XNode.DeepEquals(serialization, XElement.Parse(xml)).Should().BeTrue();
        }

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.Int32\" Reference=\"0x00000000\" Value=\"-2147483648\" />", 0, int.MinValue)]  
        [InlineData("<PrimitiveValue Type=\"System.Int32\" Reference=\"0x00000001\" Value=\"2147483647\" />", 1, int.MaxValue)]  
        [InlineData("<PrimitiveValue Type=\"System.Int32\" Reference=\"0x00000002\" Value=\"1023\" />", 2, 1023)]  
        [InlineData("<PrimitiveValue Type=\"System.Int32\" Reference=\"0x00000003\" Value=\"-1023\" />", 3, -1023)]
        public void Test_Int32Parameter(string xml, int intRef, int value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            IParameterSet set = new ParameterSet(context);

            XmlSerializer ser = new XmlSerializer();

            IInt32Parameter? p = set.CreateInt32Parameter(reference);
            p.Value = value;

            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);

            XElement serialization = ser.ToXml(p);
            XNode.DeepEquals(serialization, XElement.Parse(xml)).Should().BeTrue();
        }

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.Int64\" Reference=\"0x00000000\" Value=\"-9223372036854775808\" />", 0, long.MinValue)]  
        [InlineData("<PrimitiveValue Type=\"System.Int64\" Reference=\"0x00000001\" Value=\"9223372036854775807\" />", 1, long.MaxValue)]  
        [InlineData("<PrimitiveValue Type=\"System.Int64\" Reference=\"0x00000002\" Value=\"1023\" />", 2, 1023)]  
        [InlineData("<PrimitiveValue Type=\"System.Int64\" Reference=\"0x00000003\" Value=\"-1023\" />", 3, -1023)]
        public void Test_Int64Parameter(string xml, int intRef, long value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            IParameterSet set = new ParameterSet(context);

            XmlSerializer ser = new XmlSerializer();

            IInt64Parameter? p = set.CreateInt64Parameter(reference);
            p.Value = value;

            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);

            XElement serialization = ser.ToXml(p);
            XNode.DeepEquals(serialization, XElement.Parse(xml)).Should().BeTrue();
        }

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.UInt16\" Reference=\"0x00000000\" Value=\"0\" />", 0, ushort.MinValue)]  
        [InlineData("<PrimitiveValue Type=\"System.UInt16\" Reference=\"0x00000001\" Value=\"65535\" />", 1, ushort.MaxValue)]  
        [InlineData("<PrimitiveValue Type=\"System.UInt16\" Reference=\"0x00000002\" Value=\"1023\" />", 2, 1023)]
        public void Test_UInt16Parameter(string xml, int intRef, ushort value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            IParameterSet set = new ParameterSet(context);

            XmlSerializer ser = new XmlSerializer();

            IUInt16Parameter? p = set.CreateUInt16Parameter(reference);
            p.Value = value;

            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);

            XElement serialization = ser.ToXml(p);
            XNode.DeepEquals(serialization, XElement.Parse(xml)).Should().BeTrue();
        }

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.UInt32\" Reference=\"0x00000000\" Value=\"0\" />", 0, uint.MinValue)]  
        [InlineData("<PrimitiveValue Type=\"System.UInt32\" Reference=\"0x00000001\" Value=\"4294967295\" />", 1, uint.MaxValue)]  
        [InlineData("<PrimitiveValue Type=\"System.UInt32\" Reference=\"0x00000002\" Value=\"1023\" />", 2, 1023u)]
        public void Test_UInt32Parameter(string xml, int intRef, uint value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            IParameterSet set = new ParameterSet(context);

            XmlSerializer ser = new XmlSerializer();

            IUInt32Parameter? p = set.CreateUInt32Parameter(reference);
            p.Value = value;

            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);

            XElement serialization = ser.ToXml(p);
            XNode.DeepEquals(serialization, XElement.Parse(xml)).Should().BeTrue();
        }

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.UInt64\" Reference=\"0x00000000\" Value=\"0\" />", 0, ulong.MinValue)]  
        [InlineData("<PrimitiveValue Type=\"System.UInt64\" Reference=\"0x00000001\" Value=\"18446744073709551615\" />", 1, ulong.MaxValue)]  
        [InlineData("<PrimitiveValue Type=\"System.UInt64\" Reference=\"0x00000002\" Value=\"1023\" />", 2, 1023ul)]
        public void Test_UInt64Parameter(string xml, int intRef, ulong value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            IParameterSet set = new ParameterSet(context);

            XmlSerializer ser = new XmlSerializer();

            IUInt64Parameter? p = set.CreateUInt64Parameter(reference);
            p.Value = value;

            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);

            XElement serialization = ser.ToXml(p);
            XNode.DeepEquals(serialization, XElement.Parse(xml)).Should().BeTrue();
        }

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.Single\" Reference=\"0x00000000\" Value=\"1023\" />", 0, 1023)]  
        [InlineData("<PrimitiveValue Type=\"System.Single\" Reference=\"0x00000001\" Value=\"-1023\" />", 1, -1023)]  
        [InlineData("<PrimitiveValue Type=\"System.Single\" Reference=\"0x00000002\" Value=\"INF\" />", 2, float.PositiveInfinity)]  
        [InlineData("<PrimitiveValue Type=\"System.Single\" Reference=\"0x00000003\" Value=\"-INF\" />", 3, float.NegativeInfinity)]
        public void Test_SingleParameter(string xml, int intRef, float value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            IParameterSet set = new ParameterSet(context);

            XmlSerializer ser = new XmlSerializer();

            ISingleParameter? p = set.CreateSingleParameter(reference);
            p.Value = value;

            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);

            XElement serialization = ser.ToXml(p);
            XNode.DeepEquals(serialization, XElement.Parse(xml)).Should().BeTrue();
        }

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.Double\" Reference=\"0x00000000\" Value=\"1023\" />", 0, 1023)]  
        [InlineData("<PrimitiveValue Type=\"System.Double\" Reference=\"0x00000001\" Value=\"-1023\" />", 1, -1023)]  
        [InlineData("<PrimitiveValue Type=\"System.Double\" Reference=\"0x00000002\" Value=\"INF\" />", 2, double.PositiveInfinity)]  
        [InlineData("<PrimitiveValue Type=\"System.Double\" Reference=\"0x00000003\" Value=\"-INF\" />", 3, double.NegativeInfinity)]
        public void Test_DoubleParameter(string xml, int intRef, double value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            IParameterSet set = new ParameterSet(context);

            XmlSerializer ser = new XmlSerializer();

            IDoubleParameter? p = set.CreateDoubleParameter(reference);
            p.Value = value;

            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);

            XElement serialization = ser.ToXml(p);
            XNode.DeepEquals(serialization, XElement.Parse(xml)).Should().BeTrue();
        }
    }
}

