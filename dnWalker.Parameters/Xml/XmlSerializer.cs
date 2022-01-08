using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using static dnWalker.Parameters.Xml.XmlTokens;

namespace dnWalker.Parameters.Xml
{
    public static partial class XmlSerializer
    {
        public static XElement ToXml(this IParameter parameter)
        {
            XElement? xml;

            switch (parameter)
            {
                case IBooleanParameter p: xml = p.ToXml(); break;
                case ICharParameter p: xml = p.ToXml(); break;
                case IByteParameter p: xml = p.ToXml(); break;
                case ISByteParameter p: xml = p.ToXml(); break;
                case IInt16Parameter p: xml = p.ToXml(); break;
                case IInt32Parameter p: xml = p.ToXml(); break;
                case IInt64Parameter p: xml = p.ToXml(); break;
                case IUInt16Parameter p: xml = p.ToXml(); break;
                case IUInt32Parameter p: xml = p.ToXml(); break;
                case IUInt64Parameter p: xml = p.ToXml(); break;
                case ISingleParameter p: xml = p.ToXml(); break;
                case IDoubleParameter p: xml = p.ToXml(); break;
                case IObjectParameter p: xml = p.ToXml(); break;
                case IArrayParameter p: xml = p.ToXml(); break;
                case IStructParameter p: xml = p.ToXml(); break;
                default:
                    throw new NotSupportedException();
            }

            return xml;
        }

        public static XElement ToXml(this IObjectParameter parameter)
        {
            bool? isNull = parameter.IsNull;
            XElement xml = new XElement(XmlObject, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlIsNull, isNull?.ToString() ?? XmlUnknown));

            // add field references
            xml.Add(parameter.GetFields()
                                .Select(p => new XElement(XmlField, new XAttribute(XmlName, p.Key), new XAttribute(XmlReference, p.Value))));

            // add method result references
            xml.Add(parameter.GetMethodResults()
                                 .SelectMany(p => p.Value.Select((prm, i) => (methodSignature: p.Key, invocation: i, result: prm)))
                                 .Where(tpl => tpl.result != ParameterRef.Empty)
                                 .Select(tpl => new XElement(XmlMethodResult, new XAttribute(XmlMethodSignature, tpl.methodSignature), new XAttribute(XmlInvocation, tpl.invocation), new XAttribute(XmlReference, tpl.result))));

            xml.Add(parameter.Accessor.ToXml());

            return xml;
        }

        public static XElement ToXml(this IStructParameter parameter)
        {
            XElement xml = new XElement(XmlObject, new XAttribute(XmlType, parameter.Type), new XAttribute(XmlReference, parameter.Reference));

            // add field references
            xml.Add(parameter.GetFields()
                                .Select(p => new XElement(XmlField, new XAttribute(XmlName, p.Key), new XAttribute(XmlReference, p.Value))));

            xml.Add(parameter.Accessor.ToXml());

            return xml;
        }

        public static XElement ToXml(this IArrayParameter parameter)
        {
            bool? isNull = parameter.IsNull;
            int? length = parameter.Length;

            XElement xml = new XElement(XmlArray, new XAttribute(XmlElementType, parameter.ElementType), new XAttribute(XmlReference, parameter.Reference), new XAttribute(XmlIsNull, isNull?.ToString() ?? XmlUnknown), new XAttribute(XmlLength, length?.ToString() ?? XmlUnknown));

            // add items
            xml.Add(parameter.GetItems()
                             .Select((p, i) => (index: i, item: p))
                             .Where(tpl => tpl.item != ParameterRef.Empty)
                             .Select(tpl => new XElement(XmlItem, new XAttribute(XmlIndex, tpl.index), new XAttribute(XmlReference, tpl.item))));

            xml.Add(parameter.Accessor.ToXml());

            return xml;
        }

        public static XElement ToXml(this ParameterAccessor? accessor)
        {
            switch (accessor)
            {
                case FieldParameterAccessor field:
                    return new XElement(XmlAccessor, new XAttribute(XmlType, XmlField), new XAttribute(XmlReference, field.ParentRef), new XAttribute(XmlField, field.FieldName));

                case MethodResultParameterAccessor methodResult:
                    return new XElement(XmlAccessor, new XAttribute(XmlType, XmlMethodResult), new XAttribute(XmlReference, methodResult.ParentRef), new XAttribute(XmlMethodSignature, methodResult.MethodSignature), new XAttribute(XmlInvocation, methodResult.Invocation));

                case ItemParameterAccessor item:
                    return new XElement(XmlAccessor, new XAttribute(XmlType, XmlItem), new XAttribute(XmlReference, item.ParentRef), new XAttribute(XmlIndex, item.Index));

                case MethodArgumentParameterAccessor methodArgument:
                    return new XElement(XmlAccessor, new XAttribute(XmlType, XmlMethodArgumentRoot), new XAttribute(XmlName, methodArgument.Expression));

                case StaticFieldParameterAccessor staticField:
                    return new XElement(XmlAccessor, new XAttribute(XmlType, XmlStaticFieldRoot), new XAttribute(XmlFullName, staticField.ClassName), new XAttribute(XmlField, staticField.FieldName));

                case null:
                    return new XElement(XmlAccessor, new XAttribute(XmlType, XmlNoAccessor));

                case ReturnValueParameterAccessor returnValueParameterAccessor:
                    return new XElement(XmlAccessor, new XAttribute(XmlType, XmlReturnValue));

                default:
                    throw new NotSupportedException();

            }
        }

        public static XElement ToXml(this IParameterContext store)
        {
            XElement storeXml = new XElement(XmlParameterContext, store.Parameters.Values.Select(p => p.ToXml()));
            return storeXml;
        }
    }
}
