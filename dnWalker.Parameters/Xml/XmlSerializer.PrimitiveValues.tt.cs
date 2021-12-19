using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using static dnWalker.Parameters.Xml.XmlTokens;

namespace dnWalker.Parameters.Xml
{

    public static partial class XmlSerializer
    {
        public static XElement ToXml(this IBooleanParameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : parameter.Value.ToString()!;
            return new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
        }
        public static XElement ToXml(this IByteParameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : parameter.Value.ToString()!;
            return new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
        }
        public static XElement ToXml(this ISByteParameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : parameter.Value.ToString()!;
            return new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
        }
        public static XElement ToXml(this IInt16Parameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : parameter.Value.ToString()!;
            return new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
        }
        public static XElement ToXml(this IInt32Parameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : parameter.Value.ToString()!;
            return new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
        }
        public static XElement ToXml(this IInt64Parameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : parameter.Value.ToString()!;
            return new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
        }
        public static XElement ToXml(this IUInt16Parameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : parameter.Value.ToString()!;
            return new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
        }
        public static XElement ToXml(this IUInt32Parameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : parameter.Value.ToString()!;
            return new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
        }
        public static XElement ToXml(this IUInt64Parameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : parameter.Value.ToString()!;
            return new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
        }
        public static XElement ToXml(this ICharParameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : string.Format(@"U+{0:x4}", (int)parameter.Value.Value).ToUpper();
            return new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
        }
        public static XElement ToXml(this ISingleParameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : FloatToXml(parameter.Value.Value);
            return new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
        }
        public static XElement ToXml(this IDoubleParameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : DoubleToXml(parameter.Value.Value);
            return new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
        }
 

        private static string FloatToXml(float flt)
        {
            if (float.IsNaN(flt))
            {
                return "NAN";
            }
            else if (float.IsPositiveInfinity(flt))
            {
                return "INF";
            }
            else if (float.IsNegativeInfinity(flt))
            {
                return "-INF";
            }
            else
            {
                return flt.ToString()!;
            }
        }
        private static string DoubleToXml(double dbl)
        {
            if (double.IsNaN(dbl))
            {
                return "NAN";
            }
            else if (double.IsPositiveInfinity(dbl))
            {
                return "INF";
            }
            else if (double.IsNegativeInfinity(dbl))
            {
                return "-INF";
            }
            else
            {
                return dbl.ToString()!;
            }
        }
    }
}


