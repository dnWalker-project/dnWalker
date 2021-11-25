using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dnWalker.Parameters.Xml
{
    public static partial class XmlSerializer
    {
        public static XElement ToXml(this Parameter parameter)
        {
            switch (parameter)
            {
                case BooleanParameter p: return ToXml(p);
                case CharParameter p: return ToXml(p);
                case ByteParameter p: return ToXml(p);
                case SByteParameter p: return ToXml(p);
                case Int16Parameter p: return ToXml(p);
                case Int32Parameter p: return ToXml(p);
                case Int64Parameter p: return ToXml(p);
                case UInt16Parameter p: return ToXml(p);
                case UInt32Parameter p: return ToXml(p);
                case UInt64Parameter p: return ToXml(p);
                case SingleParameter p: return ToXml(p);
                case DoubleParameter p: return ToXml(p);
                case ObjectParameter p: return ToXml(p);
                case InterfaceParameter p: return ToXml(p);
                case ArrayParameter p: return ToXml(p);
                default:
                    throw new NotSupportedException();
            }

        }

        public static XElement ToXml(this ObjectParameter parameter)
        {
            bool isNull = parameter.IsNull;
            XElement xml = new XElement("Object", new XAttribute("Type", parameter.TypeName), new XAttribute("Name", parameter.LocalName), new XAttribute("IsNull", isNull));
            
            if (!isNull)
            {
                xml.Add(parameter.GetKnownFields()
                                 .Select(p =>
                                 {
                                     XElement fieldXml = new XElement("Field", new XAttribute("Name", p.Key), ToXml(p.Value));
                                     return fieldXml;
                                 }));
            }
            return xml;
        }

        public static XElement ToXml(this InterfaceParameter parameter)
        {
            bool isNull = parameter.IsNull;

            XElement xml = new XElement("Interface", new XAttribute("Type", parameter.TypeName), new XAttribute("Name", parameter.LocalName), new XAttribute("IsNull", isNull));

            if (!isNull)
            {
                xml.Add(parameter.GetKnownMethodResults()
                            .GroupBy(p => p.Key.Item1)
                            .Select(g =>
                            {
                                XElement methodResultXml = new XElement("MethodResult", new XAttribute("Name", g.Key));
                                methodResultXml.Add(g.Select(r => new XElement("Call", new XAttribute("CallNumber", r.Key.Item2), ToXml(r.Value))));
                                return methodResultXml;
                            }));
            }
            return xml;
        }

        public static XElement ToXml(this ArrayParameter parameter)
        {
            bool isNull = parameter.IsNull;

            XElement xml = new XElement("Array", new XAttribute("ElementType", parameter.TypeName), new XAttribute("Name", parameter.LocalName), new XAttribute("IsNull", isNull), new XAttribute("Length", parameter.Length));

            if (!isNull)
            {
                xml.Add(parameter.GetKnownItems()
                                 .Select(p =>
                                 {
                                     XElement itemXml = new XElement("Item", new XAttribute("Index", p.Key), ToXml(p.Value));

                                     return itemXml;
                                 }));
            }

            return xml;
        }
    
        public static XElement ToXml(this ParameterStore store)
        {
            XElement storeXml = new XElement("ParameterStore", store.RootParameters.Select(p => ToXml(p)));
            return storeXml;
        }
    }
}
