using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dnWalker.Parameters.Xml
{
    public static class XmlDeserializer
    {
        public static ParameterStore ToParameterStore(this XElement xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException(nameof(xml));
            }

            if (xml.Name != "ParameterStore")
            {
                throw new ArgumentException("Unexpected XML element.");
            }

            ParameterStore store = new ParameterStore();

            foreach(XElement rootParamterXml in xml.Elements())
            {
                store.AddParameter(rootParamterXml.ToParameter());
            }

            return store;
        }

        public static Parameter ToParameter(this XElement xml)
        {
            string parameteryKind = xml.Name.ToString();

            switch(parameteryKind)
            {
                case "PrimitiveValueParameter": 
                    switch(xml.Attribute("Type").Value)
                    {
                        case TypeNames.BooleanTypeName: return ToBooleanParameter(xml);
                        case TypeNames.CharTypeName: return ToCharParameter(xml);
                        case TypeNames.ByteTypeName: return ToByteParameter(xml);
                        case TypeNames.SByteTypeName: return ToSByteParameter(xml);
                        case TypeNames.Int16TypeName: return ToInt16Parameter(xml);
                        case TypeNames.Int32TypeName: return ToInt32Parameterer(xml);
                        case TypeNames.Int64TypeName: return ToInt64Parameter(xml);
                        case TypeNames.UInt16TypeName: return ToUInt16Parameter(xml);
                        case TypeNames.UInt32TypeName: return ToUInt32Parameter(xml);
                        case TypeNames.UInt64TypeName: return ToUInt64Parameter(xml);
                        case TypeNames.SingleTypeName: return ToSingleParameter(xml);
                        case TypeNames.DoubleTypeName: return ToDoubleParameter(xml);
                        default:
                            throw new NotSupportedException("Unexpected primitive value parameter type: " + xml.Attribute("Type").Value);
                    }

                case "ObjectParameter":
                    return ToObjectParameter(xml);

                case "InterfaceParameter":
                    return ToInterfaceParameter(xml);

                case "ArrayParameter":
                    return ToArrayParameter(xml);

                default:
                    throw new NotSupportedException("Unexpected parameter kind: " + parameteryKind);
            }
        }

        public static ObjectParameter ToObjectParameter(this XElement xml)
        {
            string typeName = xml.Attribute("Type")?.Value ?? throw new Exception("Object parameter XML must contain 'Type' attribute.");
            string name = xml.Attribute("Name")?.Value ?? throw new Exception("Parameter XML must contain 'Name' attribute.");
            bool isNull = bool.Parse(xml.Attribute("IsNunll")?.Value ?? throw new Exception("Object parameter XML must contain 'IsNull' attribute."));

            ObjectParameter o = new ObjectParameter(typeName, name)
            {
                IsNull = isNull
            };

            foreach(XElement fieldElement in xml.Elements("Field"))
            {
                string fieldName = fieldElement.Attribute("Name").Value ?? throw new Exception("Object field XMl must contain 'Name' attribute.");
                Parameter fieldValue = fieldElement.Elements().First().ToParameter();

                o.SetField(fieldName, fieldValue);
            }

            return o;
        }

        public static InterfaceParameter ToInterfaceParameter(this XElement xml)
        {
            string typeName = xml.Attribute("Type")?.Value ?? throw new Exception("Interface parameter XML must contain 'Type' attribute.");
            string name = xml.Attribute("Name")?.Value ?? throw new Exception("Parameter XML must contain 'Name' attribute.");
            bool isNull = bool.Parse(xml.Attribute("IsNunll")?.Value ?? throw new Exception("Interface parameter XML must contain 'IsNull' attribute."));

            InterfaceParameter i = new InterfaceParameter(typeName, name)
            {
                IsNull = isNull
            };

            foreach (XElement methodResultElement in xml.Elements("MethodResult"))
            {
                string methodName = methodResultElement.Attribute("Name")?.Value ?? throw new Exception("Interface MethodResult XML must contain 'Name' attribute.");
                foreach (XElement callResultEelement in methodResultElement.Elements("Call"))
                {
                    int callNumber = int.Parse(callResultEelement.Attribute("CallNumber")?.Value ?? throw new Exception("Interface CallResult XML must contain 'CallNumber' attribute."));
                    Parameter callResult = callResultEelement.Elements().First().ToParameter();

                    i.SetMethodResult(methodName, callNumber, callResult);
                }
            }

            return i;
        }

        public static ArrayParameter ToArrayParameter(this XElement xml)
        {
            string elementTypeName = xml.Attribute("ElementType")?.Value ?? throw new Exception("Array parameter XML must contain 'ElementType' attribute.");
            string name = xml.Attribute("Name")?.Value ?? throw new Exception("Parameter XML must contain 'Name' attribute.");
            bool isNull = bool.Parse(xml.Attribute("IsNunll")?.Value ?? throw new Exception("Array parameter XML must contain 'IsNull' attribute."));
            int length = int.Parse(xml.Attribute("Length")?.Value ?? throw new Exception("Array parameter XML must contain 'Length' attribute."));

            ArrayParameter a = new ArrayParameter(elementTypeName, name)
            {
                IsNull = isNull,
                Length = length
            };

            foreach (XElement itemElement in xml.Elements("Item"))
            {
                int itemIndex = int.Parse(itemElement.Attribute("Index").Value ?? throw new Exception("Array item XMl must contain 'Index' attribute."));
                Parameter item = itemElement.Elements().First().ToParameter();

                a.SetItemAt(itemIndex, item);
            }

            return a;
        }

        public static BooleanParameter ToBooleanParameter(this XElement xml)
        {
            string name = xml.Attribute("Name")?.Value ?? throw new Exception("Parameter XML must contain 'Name' attribute.");
            bool value = bool.Parse(xml.Value);
            return new BooleanParameter(name, value);
        }

        public static CharParameter ToCharParameter(this XElement xml)
        {
            string name = xml.Attribute("Name")?.Value ?? throw new Exception("Parameter XML must contain 'Name' attribute.");
            char value = char.Parse(xml.Value);
            return new CharParameter(name, value);
        }

        public static ByteParameter ToByteParameter(this XElement xml)
        {
            string name = xml.Attribute("Name")?.Value ?? throw new Exception("Parameter XML must contain 'Name' attribute.");
            byte value = byte.Parse(xml.Value);
            return new ByteParameter(name, value);
        }

        public static SByteParameter ToSByteParameter(this XElement xml)
        {
            string name = xml.Attribute("Name")?.Value ?? throw new Exception("Parameter XML must contain 'Name' attribute.");
            sbyte value = sbyte.Parse(xml.Value);
            return new SByteParameter(name, value);
        }

        public static Int16Parameter ToInt16Parameter(this XElement xml)
        {
            string name = xml.Attribute("Name")?.Value ?? throw new Exception("Parameter XML must contain 'Name' attribute.");
            short value = short.Parse(xml.Value);
            return new Int16Parameter(name, value);
        }

        public static Int32Parameter ToInt32Parameterer(this XElement xml)
        {
            string name = xml.Attribute("Name")?.Value ?? throw new Exception("Parameter XML must contain 'Name' attribute.");
            int value = int.Parse(xml.Value);
            return new Int32Parameter(name, value);
        }

        public static Int64Parameter ToInt64Parameter(this XElement xml)
        {
            string name = xml.Attribute("Name")?.Value ?? throw new Exception("Parameter XML must contain 'Name' attribute.");
            long value = long.Parse(xml.Value);
            return new Int64Parameter(name, value);
        }

        public static UInt16Parameter ToUInt16Parameter(this XElement xml)
        {
            string name = xml.Attribute("Name")?.Value ?? throw new Exception("Parameter XML must contain 'Name' attribute.");
            ushort value = ushort.Parse(xml.Value);
            return new UInt16Parameter(name, value);
        }

        public static UInt32Parameter ToUInt32Parameter(this XElement xml)
        {
            string name = xml.Attribute("Name")?.Value ?? throw new Exception("Parameter XML must contain 'Name' attribute.");
            uint value = uint.Parse(xml.Value);
            return new UInt32Parameter(name, value);
        }

        public static UInt64Parameter ToUInt64Parameter(this XElement xml)
        {
            string name = xml.Attribute("Name")?.Value ?? throw new Exception("Parameter XML must contain 'Name' attribute.");
            ulong value = ulong.Parse(xml.Value);
            return new UInt64Parameter(name, value);
        }

        public static SingleParameter ToSingleParameter(this XElement xml)
        {
            string name = xml.Attribute("Name")?.Value ?? throw new Exception("Parameter XML must contain 'Name' attribute.");
            float value = float.Parse(xml.Value);
            return new SingleParameter(name, value);
        }

        public static DoubleParameter ToDoubleParameter(this XElement xml)
        {
            string name = xml.Attribute("Name")?.Value ?? throw new Exception("Parameter XML must contain 'Name' attribute.");
            double value = double.Parse(xml.Value);
            return new DoubleParameter(name, value);
        }
    }
}
