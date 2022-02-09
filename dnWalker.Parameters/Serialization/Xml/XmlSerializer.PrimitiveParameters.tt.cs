using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using static dnWalker.Parameters.Xml.XmlTokens;

namespace dnWalker.Parameters.Serialization.Xml
{

    public partial class XmlSerializer
    {
        private XElement ToXml(IBooleanParameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : parameter.Value.ToString()!;
            XElement xml = new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
            
            // xml.Add(parameter.Accessors.Select(pa => pa.ToXml()));

            return xml;
        }
        private XElement ToXml(IByteParameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : parameter.Value.ToString()!;
            XElement xml = new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
            
            // xml.Add(parameter.Accessors.Select(pa => pa.ToXml()));

            return xml;
        }
        private XElement ToXml(ISByteParameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : parameter.Value.ToString()!;
            XElement xml = new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
            
            // xml.Add(parameter.Accessors.Select(pa => pa.ToXml()));

            return xml;
        }
        private XElement ToXml(IInt16Parameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : parameter.Value.ToString()!;
            XElement xml = new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
            
            // xml.Add(parameter.Accessors.Select(pa => pa.ToXml()));

            return xml;
        }
        private XElement ToXml(IInt32Parameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : parameter.Value.ToString()!;
            XElement xml = new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
            
            // xml.Add(parameter.Accessors.Select(pa => pa.ToXml()));

            return xml;
        }
        private XElement ToXml(IInt64Parameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : parameter.Value.ToString()!;
            XElement xml = new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
            
            // xml.Add(parameter.Accessors.Select(pa => pa.ToXml()));

            return xml;
        }
        private XElement ToXml(IUInt16Parameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : parameter.Value.ToString()!;
            XElement xml = new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
            
            // xml.Add(parameter.Accessors.Select(pa => pa.ToXml()));

            return xml;
        }
        private XElement ToXml(IUInt32Parameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : parameter.Value.ToString()!;
            XElement xml = new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
            
            // xml.Add(parameter.Accessors.Select(pa => pa.ToXml()));

            return xml;
        }
        private XElement ToXml(IUInt64Parameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : parameter.Value.ToString()!;
            XElement xml = new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
            
            // xml.Add(parameter.Accessors.Select(pa => pa.ToXml()));

            return xml;
        }
        private XElement ToXml(ICharParameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : CharToString(parameter.Value.Value);
            XElement xml = new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
            
            // xml.Add(parameter.Accessors.Select(pa => pa.ToXml()));

            return xml;
        }
        private XElement ToXml(ISingleParameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : FloatToXml(parameter.Value.Value);
            XElement xml = new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
            
            // xml.Add(parameter.Accessors.Select(pa => pa.ToXml()));

            return xml;
        }
        private XElement ToXml(IDoubleParameter parameter)
        {
            string valueXmlString = parameter.Value is null ? XmlUnknown : DoubleToXml(parameter.Value.Value);
            XElement xml = new XElement(XmlPrimitiveValue, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlValue, valueXmlString));
            
            // xml.Add(parameter.Accessors.Select(pa => pa.ToXml()));

            return xml;
        }
 
        private static string CharToString(char c)
        {
            return string.Format(@"U+{0:x4}", (int)c).ToUpper();
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
