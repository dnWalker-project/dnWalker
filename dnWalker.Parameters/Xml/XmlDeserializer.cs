using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using static dnWalker.Parameters.Xml.XmlTokens;

namespace dnWalker.Parameters.Xml
{
    public class MissingElementException : Exception
    {
        public MissingElementException(string context, string elementName)
        {
            ElementName = elementName;
            Context = context;
        }

        protected MissingElementException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ElementName = info.GetString(nameof(ElementName)) ?? string.Empty;
            Context = info.GetString(nameof(Context)) ?? string.Empty;
        }

        public string ElementName { get; }

        public string Context { get; }

        public override string Message
        {
            get
            {
                return $"'{Context}' XML must contain an '{ElementName}' element.";
            }
        }
    }
    public class MissingAttributeException : Exception
    {
        public MissingAttributeException(string context, string attributeName)
        {
            AttributeName = attributeName;
            Context = context;
        }

        protected MissingAttributeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            AttributeName = info.GetString(nameof(AttributeName)) ?? string.Empty;
            Context = info.GetString(nameof(Context)) ?? string.Empty;
        }

        public string AttributeName { get; }

        public string Context { get; }

        public override string Message
        {
            get
            {
                return $"'{Context}' XML must contain an '{AttributeName}' attribute.";
            }
        }
    }

    public static partial class XmlDeserializer
    {
        private static ParameterRef String2Ref(string pref)
        {
            return Convert.ToInt32(pref.Substring(2));
        }

        private static bool? String2Bool(string value)
        {
            if (value == XmlUnknown) return null;
            return bool.Parse(value);
        }

        private static int? String2Int(string value)
        {
            if (value == XmlUnknown) return null;
            return int.Parse(value);
        }

        public static IParameter ToParameter(this XElement xml, IParameterContext context)
        {
            string parameterType = xml.Name.LocalName;

            IParameter? parameter;

            switch (parameterType)
            {
                case XmlObject: parameter = ToObjectParameter(xml, context); break;
                case XmlArray: parameter = ToArrayParameter(xml, context); break;
                case XmlPrimitiveValue: parameter = ToPrimitiveValueParameter(xml, context); break;
                case XmlStruct: parameter = ToStructParameter(xml, context); break;
                default: throw new NotSupportedException(xml.ToString());
            }

            parameter.Accessor = xml.Element(XmlAccessor)?.ToAccessor() ?? throw new MissingElementException(nameof(IParameter), XmlAccessor);

            return parameter;
        }

        public static IObjectParameter ToObjectParameter(this XElement xml, IParameterContext context)
        {
            string type = xml.Attribute(XmlType)?.Value ?? throw new MissingAttributeException(nameof(IObjectParameter), XmlType);
            bool? isNull = String2Bool(xml.Attribute(XmlIsNull)?.Value ?? throw new MissingAttributeException(nameof(IObjectParameter), XmlIsNull));
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(IObjectParameter), XmlReference));

            IObjectParameter objectParameter = context.CreateObjectParameter(reference, type, isNull);

            foreach (XElement fieldXml in xml.Elements(XmlField))
            {
                string fieldName = fieldXml.Attribute(XmlName)?.Value ?? throw new MissingAttributeException(nameof(IObjectParameter) + "/Field", XmlName);
                reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(IObjectParameter) + "/Field", XmlReference));

                objectParameter.SetField(fieldName, reference);
            }

            foreach (XElement methodResultXml in xml.Elements(XmlMethodResult))
            {
                MethodSignature methodSignature = methodResultXml.Attribute(XmlMethodSignature)?.Value ?? throw new MissingAttributeException(nameof(IObjectParameter) + "/MethodResult", XmlMethodSignature);
                int invocation = int.Parse(methodResultXml.Attribute(XmlInvocation)?.Value ?? throw new MissingAttributeException(nameof(IObjectParameter) + "/MethodResult", XmlInvocation));
                reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(IObjectParameter) + "/MethodResult", XmlReference));

                objectParameter.SetMethodResult(methodSignature, invocation, reference);
            }


            return objectParameter;
        }

        public static IStructParameter ToStructParameter(this XElement xml, IParameterContext context)
        {
            string type = xml.Attribute(XmlType)?.Value ?? throw new MissingAttributeException(nameof(IStructParameter), XmlType);
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(IStructParameter), XmlReference));

            IStructParameter structParameter = context.CreateStructParameter(reference, type);

            foreach (XElement fieldXml in xml.Elements(XmlField))
            {
                string fieldName = fieldXml.Attribute(XmlName)?.Value ?? throw new MissingAttributeException(nameof(IStructParameter) + "/Field", XmlName);
                reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(IStructParameter) + "/Field", XmlReference));

                structParameter.SetField(fieldName, reference);
            }

            return structParameter;
        }

        public static IArrayParameter ToArrayParameter(this XElement xml, IParameterContext context)
        {
            string type = xml.Attribute(XmlType)?.Value ?? throw new MissingAttributeException(nameof(IArrayParameter), XmlType);
            bool? isNull = String2Bool(xml.Attribute(XmlIsNull)?.Value ?? throw new MissingAttributeException(nameof(IArrayParameter), XmlIsNull));
            int? length = String2Int(xml.Attribute(XmlLength)?.Value ?? throw new MissingAttributeException(nameof(IArrayParameter), XmlLength));
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(IArrayParameter), XmlReference));

            IArrayParameter arrayParameter = context.CreateArrayParameter(reference, type, isNull, length);

            foreach (XElement itemXml in xml.Elements(XmlItem))
            {
                int index = int.Parse(itemXml.Attribute(XmlIndex)?.Value ?? throw new MissingAttributeException(nameof(IArrayParameter) + "/Item", XmlIndex));
                reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(IArrayParameter) + "/Item", XmlReference));

                arrayParameter.SetItem(index, reference);
            }



            return arrayParameter;
        }

        public static ParameterAccessor? ToAccessor(this XElement xml)
        {
            string type = xml.Attribute(XmlType)?.Value ?? throw new Exception("Parameter accessor XML must contain 'Type' attribute.");

            ParameterAccessor? accessor;

            switch (type)
            {
                case XmlField:
                    accessor = new FieldParameterAccessor
                        (
                            xml.Attribute(XmlField)?.Value ?? throw new MissingAttributeException(nameof(FieldParameterAccessor), XmlField),
                            String2Ref(xml.Attribute(XmlReference)?.Value.Substring(2) ?? throw new MissingAttributeException(nameof(FieldParameterAccessor), XmlReference))
                        );
                    break;

                case XmlMethodResult:
                    accessor = new MethodResultParameterAccessor
                        (
                            xml.Attribute(XmlMethodSignature)?.Value ?? throw new MissingAttributeException(nameof(MethodResultParameterAccessor), XmlMethodSignature),
                            int.Parse(xml.Attribute(XmlInvocation)?.Value ?? throw new MissingAttributeException(nameof(MethodResultParameterAccessor), XmlInvocation)),
                            String2Ref(xml.Attribute(XmlReference)?.Value.Substring(2) ?? throw new MissingAttributeException(nameof(MethodResultParameterAccessor), XmlReference))
                        );
                    break;

                case XmlIndex:
                    accessor = new ItemParameterAccessor
                        (
                            int.Parse(xml.Attribute(XmlIndex)?.Value ?? throw new MissingAttributeException(nameof(ItemParameterAccessor), XmlIndex)),
                            String2Ref(xml.Attribute(XmlReference)?.Value.Substring(2) ?? throw new MissingAttributeException(nameof(ItemParameterAccessor), XmlReference))
                        );
                    break;

                case XmlMethodArgumentRoot:
                    accessor = new MethodArgumentParameterAccessor
                        (
                            xml.Attribute(XmlName)?.Value ?? throw new MissingAttributeException(nameof(MethodArgumentParameterAccessor), XmlName)
                        );
                    break;

                case XmlStaticFieldRoot:
                    accessor = new StaticFieldParameterAccessor
                        (
                            xml.Attribute(XmlFullName)?.Value ?? throw new MissingAttributeException(nameof(StaticFieldParameterAccessor), XmlFullName),
                            xml.Attribute(XmlField)?.Value ?? throw new MissingAttributeException(nameof(StaticFieldParameterAccessor), XmlField)
                        );
                    break;

                case XmlNoAccessor:
                    accessor = null;
                    break;

                default:
                    throw new NotSupportedException();
            }

            return accessor;
        }

        public static IParameterContext ToParameterContext(this XElement xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException(nameof(xml));
            }

            if (xml.Name != XmlParameterContext)
            {
                throw new ArgumentException("Unexpected XML element.");
            }

            IParameterContext context = new ParameterContext();

            foreach (XElement parameterXml in xml.Elements())
            {
                parameterXml.ToParameter(context);
            }

            return context;
        }
    }
}
