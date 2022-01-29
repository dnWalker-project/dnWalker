using System;
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
    public class XmlDeserializerTest_PrimitiveValueParameters : TestBase
    {

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.Boolean\" Reference=\"0x00000000\" Value=\"True\" />", 0, true)]  
        [InlineData("<PrimitiveValue Type=\"System.Boolean\" Reference=\"0x00000001\" Value=\"False\" />", 1, false)]
        public void TestNotNull_BooleanParameter(string xml, int intRef, bool value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);

            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            IBooleanParameter? p = des.ToParameter(XElement.Parse(xml), set) as IBooleanParameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);
        }

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.Char\" Reference=\"0x00000000\" Value=\"U+0000\" />", 0, '\0')]  
        [InlineData("<PrimitiveValue Type=\"System.Char\" Reference=\"0x00000001\" Value=\"U+000A\" />", 1, '\n')]  
        [InlineData("<PrimitiveValue Type=\"System.Char\" Reference=\"0x00000002\" Value=\"U+0061\" />", 2, 'a')]  
        [InlineData("<PrimitiveValue Type=\"System.Char\" Reference=\"0x00000003\" Value=\"U+0000\" />", 3, char.MinValue)]  
        [InlineData("<PrimitiveValue Type=\"System.Char\" Reference=\"0x00000004\" Value=\"U+FFFF\" />", 4, char.MaxValue)]
        public void TestNotNull_CharParameter(string xml, int intRef, char value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);

            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            ICharParameter? p = des.ToParameter(XElement.Parse(xml), set) as ICharParameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);
        }

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.Byte\" Reference=\"0x00000000\" Value=\"0\" />", 0, byte.MinValue)]  
        [InlineData("<PrimitiveValue Type=\"System.Byte\" Reference=\"0x00000001\" Value=\"255\" />", 1, byte.MaxValue)]  
        [InlineData("<PrimitiveValue Type=\"System.Byte\" Reference=\"0x00000002\" Value=\"127\" />", 2, 127)]
        public void TestNotNull_ByteParameter(string xml, int intRef, byte value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);

            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            IByteParameter? p = des.ToParameter(XElement.Parse(xml), set) as IByteParameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);
        }

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.SByte\" Reference=\"0x00000000\" Value=\"-128\" />", 0, sbyte.MinValue)]  
        [InlineData("<PrimitiveValue Type=\"System.SByte\" Reference=\"0x00000001\" Value=\"127\" />", 1, sbyte.MaxValue)]  
        [InlineData("<PrimitiveValue Type=\"System.SByte\" Reference=\"0x00000002\" Value=\"63\" />", 2, 63)]  
        [InlineData("<PrimitiveValue Type=\"System.SByte\" Reference=\"0x00000003\" Value=\"-63\" />", 3, -63)]
        public void TestNotNull_SByteParameter(string xml, int intRef, sbyte value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);

            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            ISByteParameter? p = des.ToParameter(XElement.Parse(xml), set) as ISByteParameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);
        }

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.Int16\" Reference=\"0x00000000\" Value=\"-32768\" />", 0, short.MinValue)]  
        [InlineData("<PrimitiveValue Type=\"System.Int16\" Reference=\"0x00000001\" Value=\"32767\" />", 1, short.MaxValue)]  
        [InlineData("<PrimitiveValue Type=\"System.Int16\" Reference=\"0x00000002\" Value=\"1023\" />", 2, 1023)]  
        [InlineData("<PrimitiveValue Type=\"System.Int16\" Reference=\"0x00000003\" Value=\"-1023\" />", 3, -1023)]
        public void TestNotNull_Int16Parameter(string xml, int intRef, short value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);

            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            IInt16Parameter? p = des.ToParameter(XElement.Parse(xml), set) as IInt16Parameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);
        }

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.Int32\" Reference=\"0x00000000\" Value=\"-2147483648\" />", 0, int.MinValue)]  
        [InlineData("<PrimitiveValue Type=\"System.Int32\" Reference=\"0x00000001\" Value=\"2147483647\" />", 1, int.MaxValue)]  
        [InlineData("<PrimitiveValue Type=\"System.Int32\" Reference=\"0x00000002\" Value=\"1023\" />", 2, 1023)]  
        [InlineData("<PrimitiveValue Type=\"System.Int32\" Reference=\"0x00000003\" Value=\"-1023\" />", 3, -1023)]
        public void TestNotNull_Int32Parameter(string xml, int intRef, int value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);

            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            IInt32Parameter? p = des.ToParameter(XElement.Parse(xml), set) as IInt32Parameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);
        }

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.Int64\" Reference=\"0x00000000\" Value=\"-9223372036854775808\" />", 0, long.MinValue)]  
        [InlineData("<PrimitiveValue Type=\"System.Int64\" Reference=\"0x00000001\" Value=\"9223372036854775807\" />", 1, long.MaxValue)]  
        [InlineData("<PrimitiveValue Type=\"System.Int64\" Reference=\"0x00000002\" Value=\"1023\" />", 2, 1023)]  
        [InlineData("<PrimitiveValue Type=\"System.Int64\" Reference=\"0x00000003\" Value=\"-1023\" />", 3, -1023)]
        public void TestNotNull_Int64Parameter(string xml, int intRef, long value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);

            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            IInt64Parameter? p = des.ToParameter(XElement.Parse(xml), set) as IInt64Parameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);
        }

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.UInt16\" Reference=\"0x00000000\" Value=\"0\" />", 0, ushort.MinValue)]  
        [InlineData("<PrimitiveValue Type=\"System.UInt16\" Reference=\"0x00000001\" Value=\"65535\" />", 1, ushort.MaxValue)]  
        [InlineData("<PrimitiveValue Type=\"System.UInt16\" Reference=\"0x00000002\" Value=\"1023\" />", 2, 1023)]
        public void TestNotNull_UInt16Parameter(string xml, int intRef, ushort value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);

            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            IUInt16Parameter? p = des.ToParameter(XElement.Parse(xml), set) as IUInt16Parameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);
        }

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.UInt32\" Reference=\"0x00000000\" Value=\"0\" />", 0, uint.MinValue)]  
        [InlineData("<PrimitiveValue Type=\"System.UInt32\" Reference=\"0x00000001\" Value=\"4294967295\" />", 1, uint.MaxValue)]  
        [InlineData("<PrimitiveValue Type=\"System.UInt32\" Reference=\"0x00000002\" Value=\"1023\" />", 2, 1023u)]
        public void TestNotNull_UInt32Parameter(string xml, int intRef, uint value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);

            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            IUInt32Parameter? p = des.ToParameter(XElement.Parse(xml), set) as IUInt32Parameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);
        }

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.UInt64\" Reference=\"0x00000000\" Value=\"0\" />", 0, ulong.MinValue)]  
        [InlineData("<PrimitiveValue Type=\"System.UInt64\" Reference=\"0x00000001\" Value=\"18446744073709551615\" />", 1, ulong.MaxValue)]  
        [InlineData("<PrimitiveValue Type=\"System.UInt64\" Reference=\"0x00000002\" Value=\"1023\" />", 2, 1023ul)]
        public void TestNotNull_UInt64Parameter(string xml, int intRef, ulong value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);

            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            IUInt64Parameter? p = des.ToParameter(XElement.Parse(xml), set) as IUInt64Parameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);
        }

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.Single\" Reference=\"0x00000000\" Value=\"1023\" />", 0, 1023)]  
        [InlineData("<PrimitiveValue Type=\"System.Single\" Reference=\"0x00000001\" Value=\"-1023\" />", 1, -1023)]  
        [InlineData("<PrimitiveValue Type=\"System.Single\" Reference=\"0x00000002\" Value=\"INF\" />", 2, float.PositiveInfinity)]  
        [InlineData("<PrimitiveValue Type=\"System.Single\" Reference=\"0x00000003\" Value=\"-INF\" />", 3, float.NegativeInfinity)]
        public void TestNotNull_SingleParameter(string xml, int intRef, float value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);

            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            ISingleParameter? p = des.ToParameter(XElement.Parse(xml), set) as ISingleParameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);
        }

        [Theory]  
        [InlineData("<PrimitiveValue Type=\"System.Double\" Reference=\"0x00000000\" Value=\"1023\" />", 0, 1023)]  
        [InlineData("<PrimitiveValue Type=\"System.Double\" Reference=\"0x00000001\" Value=\"-1023\" />", 1, -1023)]  
        [InlineData("<PrimitiveValue Type=\"System.Double\" Reference=\"0x00000002\" Value=\"INF\" />", 2, double.PositiveInfinity)]  
        [InlineData("<PrimitiveValue Type=\"System.Double\" Reference=\"0x00000003\" Value=\"-INF\" />", 3, double.NegativeInfinity)]
        public void TestNotNull_DoubleParameter(string xml, int intRef, double value)
        {
            ParameterRef reference = intRef;

            IParameterContext context = new ParameterContext(DefinitionProvider);

            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            IDoubleParameter? p = des.ToParameter(XElement.Parse(xml), set) as IDoubleParameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeTrue();
            p.Value.Value.Should().Be(value);
        }


        [Theory]
        [InlineData("<PrimitiveValue Type=\"System.Boolean\" Reference=\"0x00000000\" Value=\"Unknown\" />", 0, null)]
        public void TestNull_BooleanParameter(string xml, int intRef, bool value)
        {
            ParameterRef reference = intRef;
            
            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            IBooleanParameter? p = des.ToParameter(XElement.Parse(xml), set) as IBooleanParameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeFalse();
        }

        [Theory]
        [InlineData("<PrimitiveValue Type=\"System.Char\" Reference=\"0x00000001\" Value=\"Unknown\" />", 1, null)]
        public void TestNull_CharParameter(string xml, int intRef, char value)
        {
            ParameterRef reference = intRef;
            
            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            ICharParameter? p = des.ToParameter(XElement.Parse(xml), set) as ICharParameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeFalse();
        }

        [Theory]
        [InlineData("<PrimitiveValue Type=\"System.Byte\" Reference=\"0x00000002\" Value=\"Unknown\" />", 2, null)]
        public void TestNull_ByteParameter(string xml, int intRef, byte value)
        {
            ParameterRef reference = intRef;
            
            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            IByteParameter? p = des.ToParameter(XElement.Parse(xml), set) as IByteParameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeFalse();
        }

        [Theory]
        [InlineData("<PrimitiveValue Type=\"System.SByte\" Reference=\"0x00000003\" Value=\"Unknown\" />", 3, null)]
        public void TestNull_SByteParameter(string xml, int intRef, sbyte value)
        {
            ParameterRef reference = intRef;
            
            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            ISByteParameter? p = des.ToParameter(XElement.Parse(xml), set) as ISByteParameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeFalse();
        }

        [Theory]
        [InlineData("<PrimitiveValue Type=\"System.Int16\" Reference=\"0x00000004\" Value=\"Unknown\" />", 4, null)]
        public void TestNull_Int16Parameter(string xml, int intRef, short value)
        {
            ParameterRef reference = intRef;
            
            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            IInt16Parameter? p = des.ToParameter(XElement.Parse(xml), set) as IInt16Parameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeFalse();
        }

        [Theory]
        [InlineData("<PrimitiveValue Type=\"System.Int32\" Reference=\"0x00000005\" Value=\"Unknown\" />", 5, null)]
        public void TestNull_Int32Parameter(string xml, int intRef, int value)
        {
            ParameterRef reference = intRef;
            
            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            IInt32Parameter? p = des.ToParameter(XElement.Parse(xml), set) as IInt32Parameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeFalse();
        }

        [Theory]
        [InlineData("<PrimitiveValue Type=\"System.Int64\" Reference=\"0x00000006\" Value=\"Unknown\" />", 6, null)]
        public void TestNull_Int64Parameter(string xml, int intRef, long value)
        {
            ParameterRef reference = intRef;
            
            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            IInt64Parameter? p = des.ToParameter(XElement.Parse(xml), set) as IInt64Parameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeFalse();
        }

        [Theory]
        [InlineData("<PrimitiveValue Type=\"System.UInt16\" Reference=\"0x00000007\" Value=\"Unknown\" />", 7, null)]
        public void TestNull_UInt16Parameter(string xml, int intRef, ushort value)
        {
            ParameterRef reference = intRef;
            
            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            IUInt16Parameter? p = des.ToParameter(XElement.Parse(xml), set) as IUInt16Parameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeFalse();
        }

        [Theory]
        [InlineData("<PrimitiveValue Type=\"System.UInt32\" Reference=\"0x00000008\" Value=\"Unknown\" />", 8, null)]
        public void TestNull_UInt32Parameter(string xml, int intRef, uint value)
        {
            ParameterRef reference = intRef;
            
            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            IUInt32Parameter? p = des.ToParameter(XElement.Parse(xml), set) as IUInt32Parameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeFalse();
        }

        [Theory]
        [InlineData("<PrimitiveValue Type=\"System.UInt64\" Reference=\"0x00000009\" Value=\"Unknown\" />", 9, null)]
        public void TestNull_UInt64Parameter(string xml, int intRef, ulong value)
        {
            ParameterRef reference = intRef;
            
            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            IUInt64Parameter? p = des.ToParameter(XElement.Parse(xml), set) as IUInt64Parameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeFalse();
        }

        [Theory]
        [InlineData("<PrimitiveValue Type=\"System.Single\" Reference=\"0x0000000A\" Value=\"Unknown\" />", 10, null)]
        public void TestNull_SingleParameter(string xml, int intRef, float value)
        {
            ParameterRef reference = intRef;
            
            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            ISingleParameter? p = des.ToParameter(XElement.Parse(xml), set) as ISingleParameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeFalse();
        }

        [Theory]
        [InlineData("<PrimitiveValue Type=\"System.Double\" Reference=\"0x0000000B\" Value=\"Unknown\" />", 11, null)]
        public void TestNull_DoubleParameter(string xml, int intRef, double value)
        {
            ParameterRef reference = intRef;
            
            IParameterContext context = new ParameterContext(DefinitionProvider);
            
            XmlDeserializer des = new XmlDeserializer(context);

            IParameterSet set = new ParameterSet(context);

            IDoubleParameter? p = des.ToParameter(XElement.Parse(xml), set) as IDoubleParameter;
            p.Should().NotBeNull();
            p.Reference.Should().Be(reference);
            p.Value.HasValue.Should().BeFalse();
        }

    }
}

