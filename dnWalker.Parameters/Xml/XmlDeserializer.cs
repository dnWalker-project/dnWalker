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

            foreach(XElement rootXml in xml.Elements())
            {
                string name = rootXml.Attribute("Name")?.Value ?? throw new Exception("Root accessor must contain 'Name' attribute.");
                IParameter p = rootXml.Elements().First().ToParameter();
                store.AddRootParameter(name, p);
            }

            return store;
        }

        public static IParameter ToParameter(this XElement xml)
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
            int id = int.Parse(xml.Attribute("Id")?.Value ?? throw new Exception("Parameter XML must contain 'Id' attrubute."));
            bool isNull = bool.Parse(xml.Attribute("IsNull")?.Value ?? throw new Exception("Object parameter XML must contain 'IsNull' attribute."));

            ObjectParameter o = new ObjectParameter(typeName, id)
            {
                IsNull = isNull
            };

            if (!isNull)
            {
                foreach (XElement fieldElement in xml.Elements("Field"))
                {
                    string fieldName = fieldElement.Attribute("Name")?.Value ?? throw new Exception("Object field XMl must contain 'Name' attribute.");
                    IParameter fieldValue = fieldElement.Elements().First().ToParameter();

                    o.SetField(fieldName, fieldValue);
                }
                foreach (XElement methodResultElement in xml.Elements("MethodResult"))
                {
                    MethodSignature methodSignature = MethodSignature.Parse(methodResultElement.Attribute("Signature")?.Value ?? throw new Exception("MethodResult XML must contain 'Signature' attribute."));
                    int invocation = int.Parse(methodResultElement.Attribute("Invocation")?.Value ?? throw new Exception("MethodResult XML must contain 'Invocation' attribute."));
                    IParameter result = methodResultElement.Elements().First().ToParameter();

                    o.SetMethodResult(methodSignature, invocation, result);
                }
            }

            return o;
        }

        public static InterfaceParameter ToInterfaceParameter(this XElement xml)
        {
            string typeName = xml.Attribute("Type")?.Value ?? throw new Exception("Interface parameter XML must contain 'Type' attribute.");
            int id = int.Parse(xml.Attribute("Id")?.Value ?? throw new Exception("Parameter XML must contain 'Id' attrubute."));
            bool isNull = bool.Parse(xml.Attribute("IsNull")?.Value ?? throw new Exception("Interface parameter XML must contain 'IsNull' attribute."));

            InterfaceParameter i = new InterfaceParameter(typeName, id)
            {
                IsNull = isNull
            };

            if (!isNull)
            {

                foreach (XElement methodResultElement in xml.Elements("MethodResult"))
                {
                    MethodSignature methodSignature = MethodSignature.Parse(methodResultElement.Attribute("Signature")?.Value ?? throw new Exception("MethodResult XML must contain 'Signature' attribute."));
                    int invocation = int.Parse(methodResultElement.Attribute("Invocation")?.Value ?? throw new Exception("MethodResult XML must contain 'Invocation' attribute."));
                    IParameter result = methodResultElement.Elements().First().ToParameter();

                    i.SetMethodResult(methodSignature, invocation, result);
                }
            }
            return i;
        }

        public static ArrayParameter ToArrayParameter(this XElement xml)
        {
            string elementTypeName = xml.Attribute("ElementType")?.Value ?? throw new Exception("Array parameter XML must contain 'ElementType' attribute.");
            int id = int.Parse(xml.Attribute("Id")?.Value ?? throw new Exception("Parameter XML must contain 'Id' attrubute."));
            bool isNull = bool.Parse(xml.Attribute("IsNull")?.Value ?? throw new Exception("Array parameter XML must contain 'IsNull' attribute."));
            int length = int.Parse(xml.Attribute("Length")?.Value ?? throw new Exception("Array parameter XML must contain 'Length' attribute."));

            ArrayParameter a = new ArrayParameter(elementTypeName, id)
            {
                IsNull = isNull,
                Length = length
            };

            if (!isNull)
            {
                foreach (XElement itemElement in xml.Elements("Item"))
                {
                    int itemIndex = int.Parse(itemElement.Attribute("Index")?.Value ?? throw new Exception("Array item XMl must contain 'Index' attribute."));
                    IParameter item = itemElement.Elements().First().ToParameter();

                    a.SetItem(itemIndex, item);
                }
            }
            return a;
        }

    }
}
