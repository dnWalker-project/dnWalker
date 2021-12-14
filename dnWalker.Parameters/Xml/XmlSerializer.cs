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
        public static XElement ToXml(this IParameter parameter)
        {
            switch (parameter)
            {
                case IBooleanParameter p: return p.ToXml();
                case ICharParameter p: return p.ToXml();
                case IByteParameter p: return p.ToXml();
                case ISByteParameter p: return p.ToXml();
                case IInt16Parameter p: return p.ToXml();
                case IInt32Parameter p: return p.ToXml();
                case IInt64Parameter p: return p.ToXml();
                case IUInt16Parameter p: return p.ToXml();
                case IUInt32Parameter p: return p.ToXml();
                case IUInt64Parameter p: return p.ToXml();
                case ISingleParameter p: return p.ToXml();
                case IDoubleParameter p: return p.ToXml();
                case IObjectParameter p: return p.ToXml();
                case IInterfaceParameter p: return p.ToXml();
                case IArrayParameter p: return p.ToXml();
                default:
                    throw new NotSupportedException();
            }

        }

        public static XElement ToXml(this IObjectParameter parameter)
        {
            bool? isNull = parameter.IsNull;
            XElement xml = new XElement("Object", new XAttribute("Type", parameter.TypeName), new XAttribute("Id", parameter.Id), new XAttribute("IsNull", isNull?.ToString() ?? "Unknown"));
            
            if (isNull == false)
            {
                xml.Add(parameter.GetFields()
                                 .Select(p =>
                                 {
                                     XElement fieldXml = new XElement("Field", new XAttribute("Name", p.Key), ToXml(p.Value));
                                     return fieldXml;
                                 }));
                xml.Add(parameter.GetMethodResults()
                                 .SelectMany(p => p.Value.Select((prm, i) => (methodSignature: p.Key, invocation: i, result: prm)))
                                 .Where(tpl => tpl.result != null)
                                 .Select(tpl => 
                                 {
                                     XElement methodXml = new XElement("MethodResult", new XAttribute("Signature", tpl.methodSignature.ToString()), new XAttribute("Invocation", tpl.invocation), ToXml(tpl.result!));
                                     return methodXml;
                                 }));
            }
            return xml;
        }

        public static XElement ToXml(this InterfaceParameter parameter)
        {
            bool? isNull = parameter.IsNull;

            XElement xml = new XElement("Interface", new XAttribute("Type", parameter.TypeName), new XAttribute("Id", parameter.Id), new XAttribute("IsNull", isNull?.ToString() ?? "Unknown"));

            if (isNull != false)
            {
                xml.Add(parameter.GetMethodResults()
                                 .SelectMany(p => p.Value.Select((prm, i) => (methodSignature: p.Key, invocation: i, result: prm)))
                                 .Where(tpl => tpl.result != null)
                                 .Select(tpl =>
                                 {
                                     XElement methodXml = new XElement("MethodResult", new XAttribute("Signature", tpl.methodSignature.ToString()), new XAttribute("Invocation", tpl.invocation), ToXml(tpl.result!));
                                     return methodXml;
                                 }));
            }
            return xml;
        }

        public static XElement ToXml(this ArrayParameter parameter)
        {
            bool? isNull = parameter.IsNull;

            XElement xml = new XElement("Array", new XAttribute("ElementType", parameter.TypeName), new XAttribute("Id", parameter.Id), new XAttribute("IsNull", isNull?.ToString() ?? "Unknown"), new XAttribute("Length", parameter.Length));

            if (isNull == false)
            {
                xml.Add(parameter.GetItems()
                                 .Select((p,i) => (index: i, item: p))
                                 .Where(tpl => tpl.item != null)
                                 .Select(tpl =>
                                 {
                                     XElement itemXml = new XElement("Item", new XAttribute("Index", tpl.index), ToXml(tpl.item!));
                                     return itemXml;
                                 }));
            }

            return xml;
        }
    
        public static XElement ToXml(this ParameterStore store)
        {
            XElement storeXml = new XElement("ParameterStore", store.GetRootParameters().Select(rp => new XElement("Root", new XAttribute("Name", rp.Key), ToXml(rp.Value))));
            return storeXml;
        }
    }
}
