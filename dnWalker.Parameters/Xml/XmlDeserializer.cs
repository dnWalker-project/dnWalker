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
                store.AddRootParameter(rootParamterXml.ToParameter());
            }

            return store;
        }

        public static Parameter ToParameter(this XElement xml)
        {
            string parameteryKind = xml.Name.ToString();

            switch(parameteryKind)
            {
                case "PrimitiveValue": 
                    switch(xml.Attribute("Type")?.Value ?? throw new Exception("PrimitiveValueParameter XML must contain 'Type' attribute."))
                    {
                        case TypeNames.BooleanTypeName: return ToBooleanParameter(xml);
                        case TypeNames.CharTypeName: return ToCharParameter(xml);
                        case TypeNames.ByteTypeName: return ToByteParameter(xml);
                        case TypeNames.SByteTypeName: return ToSByteParameter(xml);
                        case TypeNames.Int16TypeName: return ToInt16Parameter(xml);
                        case TypeNames.Int32TypeName: return ToInt32Parameter(xml);
                        case TypeNames.Int64TypeName: return ToInt64Parameter(xml);
                        case TypeNames.UInt16TypeName: return ToUInt16Parameter(xml);
                        case TypeNames.UInt32TypeName: return ToUInt32Parameter(xml);
                        case TypeNames.UInt64TypeName: return ToUInt64Parameter(xml);
                        case TypeNames.SingleTypeName: return ToSingleParameter(xml);
                        case TypeNames.DoubleTypeName: return ToDoubleParameter(xml);
                        default:
                            throw new NotSupportedException("Unexpected primitive value parameter type: " + xml.Attribute("Type")?.Value);
                    }

                case "Object":
                    return ToObjectParameter(xml);

                case "Interface":
                    return ToInterfaceParameter(xml);

                case "Array":
                    return ToArrayParameter(xml);

                default:
                    throw new NotSupportedException("Unexpected parameter kind: " + parameteryKind);
            }
        }

        public static ObjectParameter ToObjectParameter(this XElement xml)
        {
            string typeName = xml.Attribute("Type")?.Value ?? throw new Exception("Object parameter XML must contain 'Type' attribute.");
            string name = xml.Attribute("Name")?.Value ?? throw new Exception("Parameter XML must contain 'Name' attribute.");
            bool isNull = bool.Parse(xml.Attribute("IsNull")?.Value ?? throw new Exception("Object parameter XML must contain 'IsNull' attribute."));

            ObjectParameter o = new ObjectParameter(typeName, name)
            {
                IsNull = isNull
            };

            if (!isNull)
            {
                foreach (XElement fieldElement in xml.Elements("Field"))
                {
                    string fieldName = fieldElement.Attribute("Name")?.Value ?? throw new Exception("Object field XMl must contain 'Name' attribute.");
                    Parameter fieldValue = fieldElement.Elements().First().ToParameter();

                    o.SetField(fieldName, fieldValue);
                }
            }

            return o;
        }

        public static InterfaceParameter ToInterfaceParameter(this XElement xml)
        {
            string typeName = xml.Attribute("Type")?.Value ?? throw new Exception("Interface parameter XML must contain 'Type' attribute.");
            string name = xml.Attribute("Name")?.Value ?? throw new Exception("Parameter XML must contain 'Name' attribute.");
            bool isNull = bool.Parse(xml.Attribute("IsNull")?.Value ?? throw new Exception("Interface parameter XML must contain 'IsNull' attribute."));

            InterfaceParameter i = new InterfaceParameter(typeName, name)
            {
                IsNull = isNull
            };

            if (!isNull)
            { 
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
            }
            return i;
        }

        public static ArrayParameter ToArrayParameter(this XElement xml)
        {
            string elementTypeName = xml.Attribute("ElementType")?.Value ?? throw new Exception("Array parameter XML must contain 'ElementType' attribute.");
            string name = xml.Attribute("Name")?.Value ?? throw new Exception("Parameter XML must contain 'Name' attribute.");
            bool isNull = bool.Parse(xml.Attribute("IsNull")?.Value ?? throw new Exception("Array parameter XML must contain 'IsNull' attribute."));
            int length = int.Parse(xml.Attribute("Length")?.Value ?? throw new Exception("Array parameter XML must contain 'Length' attribute."));

            ArrayParameter a = new ArrayParameter(elementTypeName, name)
            {
                IsNull = isNull,
                Length = length
            };

            if (!isNull)
            {
                foreach (XElement itemElement in xml.Elements("Item"))
                {
                    int itemIndex = int.Parse(itemElement.Attribute("Index")?.Value ?? throw new Exception("Array item XMl must contain 'Index' attribute."));
                    Parameter item = itemElement.Elements().First().ToParameter();

                    a.SetItem(itemIndex, item);
                }
            }
            return a;
        }

    }
}
