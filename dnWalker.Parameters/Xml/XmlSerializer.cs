using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dnWalker.Parameters.Xml
{
    public static class XmlSerializer
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

        public static XElement ToXml(this CharParameter parameter)
        {
            char symbol = parameter.Value.HasValue ? parameter.Value.Value : default(char);

            string unicodeFormat = string.Format(@"U+{0:x4}", (int)symbol).ToUpper();

            return new XElement("PrimitiveValue", new XAttribute("Type", parameter.TypeName), new XAttribute("Name", parameter.Name), unicodeFormat);
        }

        public static XElement ToXml(this SingleParameter parameter)
        {
            float number = parameter.Value.HasValue ? parameter.Value.Value : default(float);

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

            return new XElement("PrimitiveValue", new XAttribute("Type", parameter.TypeName), new XAttribute("Name", parameter.Name), numberRepr);
        }

        public static XElement ToXml(this DoubleParameter parameter)
        {
            double number = parameter.Value.HasValue ? parameter.Value.Value : default(double);

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

            return new XElement("PrimitiveValue", new XAttribute("Type", parameter.TypeName), new XAttribute("Name", parameter.Name), numberRepr);
        }

        public static XElement ToXml<T>(this PrimitiveValueParameter<T> parameter) where T : struct
        {
            return new XElement("PrimitiveValue", new XAttribute("Type", parameter.TypeName), new XAttribute("Name", parameter.Name), parameter.Value.HasValue ? parameter.Value.Value : default(T));
        }

        public static XElement ToXml(this ObjectParameter parameter)
        {
            bool isNull = parameter.IsNull ?? true;
            XElement xml = new XElement("Object", new XAttribute("Type", parameter.TypeName), new XAttribute("Name", parameter.Name), new XAttribute("IsNull", isNull));
            
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
            bool isNull = parameter.IsNull ?? true;

            XElement xml = new XElement("Interface", new XAttribute("Type", parameter.TypeName), new XAttribute("Name", parameter.Name), new XAttribute("IsNull", isNull));

            if (!isNull)
            {
                xml.Add(parameter.GetKnownMethodResults()
                                 .Select(p =>
                                 {
                                     XElement methodResultXml = new XElement("MethodResult", new XAttribute("Name", p.Key));

                                     methodResultXml.Add(p.Value.Select(r =>
                                     {
                                         XElement callResultXml = new XElement("Call", new XAttribute("CallNumber", r.Key), ToXml(r.Value));
                                         return callResultXml;
                                     }));

                                     return methodResultXml;
                                 }));
            }
            return xml;
        }

        public static XElement ToXml(this ArrayParameter parameter)
        {
            bool isNull = parameter.IsNull ?? true;

            XElement xml = new XElement("Array", new XAttribute("ElementType", parameter.TypeName), new XAttribute("Name", parameter.Name), new XAttribute("IsNull", isNull), new XAttribute("Length", parameter.Length ?? 0));

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
