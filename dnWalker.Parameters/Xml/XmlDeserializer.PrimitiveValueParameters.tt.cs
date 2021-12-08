using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dnWalker.Parameters.Xml
{

    public static partial class XmlDeserializer
    {
        public static IBooleanParameter ToBooleanParameter(this XElement xml)
        {
            int id = int.Parse(xml.Attribute("Id")?.Value ?? throw new Exception("Parameter XML must contain 'Id' attribute."));
            string valueStr = xml.Attribute("Value")?.Value ?? throw new Exception("PrimitiveValueParameter XML must contain 'Value' attribute.");
            bool value = bool.Parse(valueStr);
            return new BooleanParameter(value, id);
        }
        public static IByteParameter ToByteParameter(this XElement xml)
        {
            int id = int.Parse(xml.Attribute("Id")?.Value ?? throw new Exception("Parameter XML must contain 'Id' attribute."));
            string valueStr = xml.Attribute("Value")?.Value ?? throw new Exception("PrimitiveValueParameter XML must contain 'Value' attribute.");
            byte value = byte.Parse(valueStr);
            return new ByteParameter(value, id);
        }
        public static ISByteParameter ToSByteParameter(this XElement xml)
        {
            int id = int.Parse(xml.Attribute("Id")?.Value ?? throw new Exception("Parameter XML must contain 'Id' attribute."));
            string valueStr = xml.Attribute("Value")?.Value ?? throw new Exception("PrimitiveValueParameter XML must contain 'Value' attribute.");
            sbyte value = sbyte.Parse(valueStr);
            return new SByteParameter(value, id);
        }
        public static IInt16Parameter ToInt16Parameter(this XElement xml)
        {
            int id = int.Parse(xml.Attribute("Id")?.Value ?? throw new Exception("Parameter XML must contain 'Id' attribute."));
            string valueStr = xml.Attribute("Value")?.Value ?? throw new Exception("PrimitiveValueParameter XML must contain 'Value' attribute.");
            short value = short.Parse(valueStr);
            return new Int16Parameter(value, id);
        }
        public static IInt32Parameter ToInt32Parameter(this XElement xml)
        {
            int id = int.Parse(xml.Attribute("Id")?.Value ?? throw new Exception("Parameter XML must contain 'Id' attribute."));
            string valueStr = xml.Attribute("Value")?.Value ?? throw new Exception("PrimitiveValueParameter XML must contain 'Value' attribute.");
            int value = int.Parse(valueStr);
            return new Int32Parameter(value, id);
        }
        public static IInt64Parameter ToInt64Parameter(this XElement xml)
        {
            int id = int.Parse(xml.Attribute("Id")?.Value ?? throw new Exception("Parameter XML must contain 'Id' attribute."));
            string valueStr = xml.Attribute("Value")?.Value ?? throw new Exception("PrimitiveValueParameter XML must contain 'Value' attribute.");
            long value = long.Parse(valueStr);
            return new Int64Parameter(value, id);
        }
        public static IUInt16Parameter ToUInt16Parameter(this XElement xml)
        {
            int id = int.Parse(xml.Attribute("Id")?.Value ?? throw new Exception("Parameter XML must contain 'Id' attribute."));
            string valueStr = xml.Attribute("Value")?.Value ?? throw new Exception("PrimitiveValueParameter XML must contain 'Value' attribute.");
            ushort value = ushort.Parse(valueStr);
            return new UInt16Parameter(value, id);
        }
        public static IUInt32Parameter ToUInt32Parameter(this XElement xml)
        {
            int id = int.Parse(xml.Attribute("Id")?.Value ?? throw new Exception("Parameter XML must contain 'Id' attribute."));
            string valueStr = xml.Attribute("Value")?.Value ?? throw new Exception("PrimitiveValueParameter XML must contain 'Value' attribute.");
            uint value = uint.Parse(valueStr);
            return new UInt32Parameter(value, id);
        }
        public static IUInt64Parameter ToUInt64Parameter(this XElement xml)
        {
            int id = int.Parse(xml.Attribute("Id")?.Value ?? throw new Exception("Parameter XML must contain 'Id' attribute."));
            string valueStr = xml.Attribute("Value")?.Value ?? throw new Exception("PrimitiveValueParameter XML must contain 'Value' attribute.");
            ulong value = ulong.Parse(valueStr);
            return new UInt64Parameter(value, id);
        }
 
        public static CharParameter ToCharParameter(this XElement xml)
        {
            int id = int.Parse(xml.Attribute("Id")?.Value ?? throw new Exception("Parameter XML must contain 'Id' attribute."));
            string valueStr = xml.Attribute("Value")?.Value ?? throw new Exception("PrimitiveValueParameter XML must contain 'Value' attribute.");

            char value = Convert.ToChar(Convert.ToInt32(valueStr.Substring(2), 16));

            return new CharParameter(value, id);
        }

        public static SingleParameter ToSingleParameter(this XElement xml)
        {
            int id = int.Parse(xml.Attribute("Id")?.Value ?? throw new Exception("Parameter XML must contain 'Id' attribute."));
            string valueStr = xml.Attribute("Value")?.Value ?? throw new Exception("PrimitiveValueParameter XML must contain 'Value' attribute.");

            float value;
            switch (valueStr)
            {
                case "NAN": value = float.NaN; break;
                case "INF": value = float.PositiveInfinity; break;
                case "-INF": value = float.NegativeInfinity; break;


                default: value = float.Parse(valueStr); break;
            }

            return new SingleParameter(value, id);
        }

        public static DoubleParameter ToDoubleParameter(this XElement xml)
        {
            int id = int.Parse(xml.Attribute("Id")?.Value ?? throw new Exception("Parameter XML must contain 'Id' attribute."));
            string valueStr = xml.Attribute("Value")?.Value ?? throw new Exception("PrimitiveValueParameter XML must contain 'Value' attribute.");

            double value;
            switch (valueStr)
            {
                case "NAN": value = double.NaN; break;
                case "INF": value = double.PositiveInfinity; break;
                case "-INF": value = double.NegativeInfinity; break;


                default: value = double.Parse(valueStr); break;
            }

            return new DoubleParameter(value, id);
        }
    }
}
