using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using static dnWalker.Parameters.Xml.XmlTokens;

namespace dnWalker.Parameters.Xml
{

    public static partial class XmlDeserializer
    {
        public static IPrimitiveValueParameter ToPrimitiveValueParameter(XElement xml, IParameterContext context)
        {
            string type = xml.Attribute(XmlType)?.Value ?? throw new MissingAttributeException(nameof(IPrimitiveValueParameter), XmlType);

            switch (type)
            {
                case "Boolean": return ToBooleanParameter(xml, context);
                case "Byte": return ToByteParameter(xml, context);
                case "SByte": return ToSByteParameter(xml, context);
                case "Int16": return ToInt16Parameter(xml, context);
                case "Int32": return ToInt32Parameter(xml, context);
                case "Int64": return ToInt64Parameter(xml, context);
                case "UInt16": return ToUInt16Parameter(xml, context);
                case "UInt32": return ToUInt32Parameter(xml, context);
                case "UInt64": return ToUInt64Parameter(xml, context);
                case "Char": return ToCharParameter(xml, context);
                case "Single": return ToSingleParameter(xml, context);
                case "Double": return ToDoubleParameter(xml, context);
                default: throw new NotSupportedException();
            }
        }
        public static IBooleanParameter ToBooleanParameter(this XElement xml, IParameterContext context)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(IBooleanParameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(BooleanParameter), XmlReference));

            bool? value = xmlValue == XmlUnknown ? null : bool.Parse(xmlValue);

            IBooleanParameter parameter = context.CreateBooleanParameter(reference);

            return parameter;
        }

        public static IByteParameter ToByteParameter(this XElement xml, IParameterContext context)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(IByteParameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(ByteParameter), XmlReference));

            byte? value = xmlValue == XmlUnknown ? null : byte.Parse(xmlValue);

            IByteParameter parameter = context.CreateByteParameter(reference);

            return parameter;
        }

        public static ISByteParameter ToSByteParameter(this XElement xml, IParameterContext context)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(ISByteParameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(SByteParameter), XmlReference));

            sbyte? value = xmlValue == XmlUnknown ? null : sbyte.Parse(xmlValue);

            ISByteParameter parameter = context.CreateSByteParameter(reference);

            return parameter;
        }

        public static IInt16Parameter ToInt16Parameter(this XElement xml, IParameterContext context)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(IInt16Parameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(Int16Parameter), XmlReference));

            short? value = xmlValue == XmlUnknown ? null : short.Parse(xmlValue);

            IInt16Parameter parameter = context.CreateInt16Parameter(reference);

            return parameter;
        }

        public static IInt32Parameter ToInt32Parameter(this XElement xml, IParameterContext context)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(IInt32Parameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(Int32Parameter), XmlReference));

            int? value = xmlValue == XmlUnknown ? null : int.Parse(xmlValue);

            IInt32Parameter parameter = context.CreateInt32Parameter(reference);

            return parameter;
        }

        public static IInt64Parameter ToInt64Parameter(this XElement xml, IParameterContext context)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(IInt64Parameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(Int64Parameter), XmlReference));

            long? value = xmlValue == XmlUnknown ? null : long.Parse(xmlValue);

            IInt64Parameter parameter = context.CreateInt64Parameter(reference);

            return parameter;
        }

        public static IUInt16Parameter ToUInt16Parameter(this XElement xml, IParameterContext context)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(IUInt16Parameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(UInt16Parameter), XmlReference));

            ushort? value = xmlValue == XmlUnknown ? null : ushort.Parse(xmlValue);

            IUInt16Parameter parameter = context.CreateUInt16Parameter(reference);

            return parameter;
        }

        public static IUInt32Parameter ToUInt32Parameter(this XElement xml, IParameterContext context)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(IUInt32Parameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(UInt32Parameter), XmlReference));

            uint? value = xmlValue == XmlUnknown ? null : uint.Parse(xmlValue);

            IUInt32Parameter parameter = context.CreateUInt32Parameter(reference);

            return parameter;
        }

        public static IUInt64Parameter ToUInt64Parameter(this XElement xml, IParameterContext context)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(IUInt64Parameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(UInt64Parameter), XmlReference));

            ulong? value = xmlValue == XmlUnknown ? null : ulong.Parse(xmlValue);

            IUInt64Parameter parameter = context.CreateUInt64Parameter(reference);

            return parameter;
        }

        public static ICharParameter ToCharParameter(this XElement xml, IParameterContext context)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(ICharParameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(CharParameter), XmlReference));

            char? value = xmlValue == XmlUnknown ? null : Convert.ToChar(Convert.ToInt32(xmlValue.Substring(2), 16));

            ICharParameter parameter = context.CreateCharParameter(reference);

            return parameter;
        }

        public static ISingleParameter ToSingleParameter(this XElement xml, IParameterContext context)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(ISingleParameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(SingleParameter), XmlReference));

            float? value = xmlValue == XmlUnknown ? null : FloatFromXml(xmlValue);

            ISingleParameter parameter = context.CreateSingleParameter(reference);

            return parameter;
        }

        public static IDoubleParameter ToDoubleParameter(this XElement xml, IParameterContext context)
        {
            string xmlValue = xml.Attribute(XmlValue)?.Value ?? throw new MissingAttributeException(nameof(IDoubleParameter), XmlValue);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(DoubleParameter), XmlReference));

            double? value = xmlValue == XmlUnknown ? null : DoubleFromXml(xmlValue);

            IDoubleParameter parameter = context.CreateDoubleParameter(reference);

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
