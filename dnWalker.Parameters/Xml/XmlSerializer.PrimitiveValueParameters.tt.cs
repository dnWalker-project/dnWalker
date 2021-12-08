using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dnWalker.Parameters.Xml
{

    public static partial class XmlSeserializer
    {
        public static XElement ToXml(this IBooleanParameter parameter)
        {
            return new XElement("PrimitiveValue", new XAttribute("Type", TypeNames.BooleanTypeName), new XAttribute("Id", parameter.Id), new XAttribute("Value", parameter.Value));
        }
        public static XElement ToXml(this IByteParameter parameter)
        {
            return new XElement("PrimitiveValue", new XAttribute("Type", TypeNames.ByteTypeName), new XAttribute("Id", parameter.Id), new XAttribute("Value", parameter.Value));
        }
        public static XElement ToXml(this ISByteParameter parameter)
        {
            return new XElement("PrimitiveValue", new XAttribute("Type", TypeNames.SByteTypeName), new XAttribute("Id", parameter.Id), new XAttribute("Value", parameter.Value));
        }
        public static XElement ToXml(this IInt16Parameter parameter)
        {
            return new XElement("PrimitiveValue", new XAttribute("Type", TypeNames.Int16TypeName), new XAttribute("Id", parameter.Id), new XAttribute("Value", parameter.Value));
        }
        public static XElement ToXml(this IInt32Parameter parameter)
        {
            return new XElement("PrimitiveValue", new XAttribute("Type", TypeNames.Int32TypeName), new XAttribute("Id", parameter.Id), new XAttribute("Value", parameter.Value));
        }
        public static XElement ToXml(this IInt64Parameter parameter)
        {
            return new XElement("PrimitiveValue", new XAttribute("Type", TypeNames.Int64TypeName), new XAttribute("Id", parameter.Id), new XAttribute("Value", parameter.Value));
        }
        public static XElement ToXml(this IUInt16Parameter parameter)
        {
            return new XElement("PrimitiveValue", new XAttribute("Type", TypeNames.UInt16TypeName), new XAttribute("Id", parameter.Id), new XAttribute("Value", parameter.Value));
        }
        public static XElement ToXml(this IUInt32Parameter parameter)
        {
            return new XElement("PrimitiveValue", new XAttribute("Type", TypeNames.UInt32TypeName), new XAttribute("Id", parameter.Id), new XAttribute("Value", parameter.Value));
        }
        public static XElement ToXml(this IUInt64Parameter parameter)
        {
            return new XElement("PrimitiveValue", new XAttribute("Type", TypeNames.UInt64TypeName), new XAttribute("Id", parameter.Id), new XAttribute("Value", parameter.Value));
        }
 
        public static XElement ToXml(this ICharParameter parameter)
        {
            char symbol = parameter.Value;

            string unicodeFormat = string.Format(@"U+{0:x4}", (int)symbol).ToUpper();

            return new XElement("PrimitiveValue", new XAttribute("Type", parameter.TypeName), new XAttribute("Id", parameter.Id), new XAttribute("Value", unicodeFormat));
        }

        public static XElement ToXml(this ISingleParameter parameter)
        {
            float number = parameter.Value;

            string numberRepr;
            if (float.IsNaN(number))
            {
                numberRepr = "NAN";
            }
            else if (float.IsPositiveInfinity(number))
            {
                numberRepr = "INF";
            }
            else if (float.IsNegativeInfinity(number))
            {
                numberRepr = "-INF";
            }
            else
            {
                numberRepr = number.ToString();
            }

            return new XElement("PrimitiveValue", new XAttribute("Type", parameter.TypeName), new XAttribute("Id", parameter.Id), new XAttribute("Value", numberRepr));
        }

        public static XElement ToXml(this IDoubleParameter parameter)
        {
            double number = parameter.Value;

            string numberRepr;
            if (double.IsNaN(number))
            {
                numberRepr = "NAN";
            }
            else if (double.IsPositiveInfinity(number))
            {
                numberRepr = "INF";
            }
            else if (double.IsNegativeInfinity(number))
            {
                numberRepr = "-INF";
            }
            else
            {
                numberRepr = number.ToString();
            }

            return new XElement("PrimitiveValue", new XAttribute("Type", parameter.TypeName), new XAttribute("Id", parameter.Id), new XAttribute("Value", numberRepr));
        }
    }
}
