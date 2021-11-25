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
                case BooleanParameter p: return p.ToXml();
                case CharParameter p: return p.ToXml();
                case ByteParameter p: return p.ToXml();
                case SByteParameter p: return p.ToXml();
                case Int16Parameter p: return p.ToXml();
                case Int32Parameter p: return p.ToXml();
                case Int64Parameter p: return p.ToXml();
                case UInt16Parameter p: return p.ToXml();
                case UInt32Parameter p: return p.ToXml();
                case UInt64Parameter p: return p.ToXml();
                case SingleParameter p: return p.ToXml();
                case DoubleParameter p: return p.ToXml();
                case ObjectParameter p: return p.ToXml();
                case InterfaceParameter p: return p.ToXml();
                case ArrayParameter p: return p.ToXml();
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
            XElement storeXml = new XElement("ParameterStore", store.RootParameters.Select(p => p.ToXml()));
            return storeXml;
        }
    }
}
