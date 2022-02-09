using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using static dnWalker.Parameters.Xml.XmlTokens;

namespace dnWalker.Parameters.Serialization.Xml
{
	internal partial class XmlDeserializer
	{
        public IPrimitiveValueParameter ToPrimitiveValueParameter(XElement xml, IParameterSet set)
        {
            string type = xml.Attribute(XmlType)?.Value ?? throw new MissingAttributeException(nameof(IPrimitiveValueParameter), XmlType);
            
            switch (type)
            {
                case "System.Boolean": return ToBooleanParameter(xml, set);
                case "System.Byte": return ToByteParameter(xml, set);
                case "System.SByte": return ToSByteParameter(xml, set);
                case "System.Int16": return ToInt16Parameter(xml, set);
                case "System.Int32": return ToInt32Parameter(xml, set);
                case "System.Int64": return ToInt64Parameter(xml, set);
                case "System.UInt16": return ToUInt16Parameter(xml, set);
                case "System.UInt32": return ToUInt32Parameter(xml, set);
                case "System.UInt64": return ToUInt64Parameter(xml, set);
                case "System.Char": return ToCharParameter(xml, set);
                case "System.Single": return ToSingleParameter(xml, set);
                case "System.Double": return ToDoubleParameter(xml, set);
                default: throw new NotSupportedException();
            }
        }

        private IBooleanParameter ToBooleanParameter(XElement xml, IParameterSet set)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(IBooleanParameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(BooleanParameter), XmlReference));

            IBooleanParameter parameter = set.CreateBooleanParameter(reference);
            parameter.Value = xmlValue == XmlUnknown ? null : bool.Parse(xmlValue);

            return parameter;
        }
        private IByteParameter ToByteParameter(XElement xml, IParameterSet set)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(IByteParameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(ByteParameter), XmlReference));

            IByteParameter parameter = set.CreateByteParameter(reference);
            parameter.Value = xmlValue == XmlUnknown ? null : byte.Parse(xmlValue);

            return parameter;
        }
        private ISByteParameter ToSByteParameter(XElement xml, IParameterSet set)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(ISByteParameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(SByteParameter), XmlReference));

            ISByteParameter parameter = set.CreateSByteParameter(reference);
            parameter.Value = xmlValue == XmlUnknown ? null : sbyte.Parse(xmlValue);

            return parameter;
        }
        private IInt16Parameter ToInt16Parameter(XElement xml, IParameterSet set)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(IInt16Parameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(Int16Parameter), XmlReference));

            IInt16Parameter parameter = set.CreateInt16Parameter(reference);
            parameter.Value = xmlValue == XmlUnknown ? null : short.Parse(xmlValue);

            return parameter;
        }
        private IInt32Parameter ToInt32Parameter(XElement xml, IParameterSet set)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(IInt32Parameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(Int32Parameter), XmlReference));

            IInt32Parameter parameter = set.CreateInt32Parameter(reference);
            parameter.Value = xmlValue == XmlUnknown ? null : int.Parse(xmlValue);

            return parameter;
        }
        private IInt64Parameter ToInt64Parameter(XElement xml, IParameterSet set)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(IInt64Parameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(Int64Parameter), XmlReference));

            IInt64Parameter parameter = set.CreateInt64Parameter(reference);
            parameter.Value = xmlValue == XmlUnknown ? null : long.Parse(xmlValue);

            return parameter;
        }
        private IUInt16Parameter ToUInt16Parameter(XElement xml, IParameterSet set)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(IUInt16Parameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(UInt16Parameter), XmlReference));

            IUInt16Parameter parameter = set.CreateUInt16Parameter(reference);
            parameter.Value = xmlValue == XmlUnknown ? null : ushort.Parse(xmlValue);

            return parameter;
        }
        private IUInt32Parameter ToUInt32Parameter(XElement xml, IParameterSet set)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(IUInt32Parameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(UInt32Parameter), XmlReference));

            IUInt32Parameter parameter = set.CreateUInt32Parameter(reference);
            parameter.Value = xmlValue == XmlUnknown ? null : uint.Parse(xmlValue);

            return parameter;
        }
        private IUInt64Parameter ToUInt64Parameter(XElement xml, IParameterSet set)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(IUInt64Parameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(UInt64Parameter), XmlReference));

            IUInt64Parameter parameter = set.CreateUInt64Parameter(reference);
            parameter.Value = xmlValue == XmlUnknown ? null : ulong.Parse(xmlValue);

            return parameter;
        }
        private ICharParameter ToCharParameter(XElement xml, IParameterSet set)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(ICharParameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(CharParameter), XmlReference));

            ICharParameter parameter = set.CreateCharParameter(reference);
            parameter.Value = xmlValue == XmlUnknown ? null : Convert.ToChar(Convert.ToInt32(xmlValue.Substring(2), 16));

            return parameter;
        }
        private ISingleParameter ToSingleParameter(XElement xml, IParameterSet set)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(ISingleParameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(SingleParameter), XmlReference));

            ISingleParameter parameter = set.CreateSingleParameter(reference);
            parameter.Value = xmlValue == XmlUnknown ? null : FloatFromXml(xmlValue);

            return parameter;
        }
        private IDoubleParameter ToDoubleParameter(XElement xml, IParameterSet set)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(IDoubleParameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(DoubleParameter), XmlReference));

            IDoubleParameter parameter = set.CreateDoubleParameter(reference);
            parameter.Value = xmlValue == XmlUnknown ? null : DoubleFromXml(xmlValue);

            return parameter;
        }

        private static float FloatFromXml(string xmlValue)
        {
            switch(xmlValue)
            {
                case "NAN": return float.NaN;
                case "INF": return float.PositiveInfinity;
                case "-INF": return float.NegativeInfinity;
                default: return float.Parse(xmlValue);
            }
        }

        private static double DoubleFromXml(string xmlValue)
        {
            switch(xmlValue)
            {
                case "NAN": return double.NaN;
                case "INF": return double.PositiveInfinity;
                case "-INF": return double.NegativeInfinity;
                default: return double.Parse(xmlValue);
            }
        }
    }
}
