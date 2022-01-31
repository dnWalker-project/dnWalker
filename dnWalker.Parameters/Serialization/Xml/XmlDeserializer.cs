using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using static dnWalker.Parameters.Xml.XmlTokens;

namespace dnWalker.Parameters.Serialization.Xml
{
    public partial class XmlDeserializer : IParameterDeserializer
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

        private readonly IParameterContext _context;
        private readonly ITypeTranslator _typeTranslator;
        private readonly IMethodTranslator _methodTranslator;

        private ParameterRef String2Ref(string pref)
        {
            return Convert.ToInt32(pref.Substring(2), 16);
        }

        private MethodSignature String2Method(string methodString)
        {
            return _methodTranslator.FromString(methodString);
        }

        private TypeSignature String2Type(string typeString)
        {
            return _typeTranslator.FromString(typeString);
        }

        private bool? String2Bool(string value)
        {
            if (value == XmlUnknown) return null;
            return bool.Parse(value);
        }

        private int? String2Int(string value)
        {
            if (value == XmlUnknown) return null;
            return int.Parse(value);
        }

        public XmlDeserializer(IParameterContext context) : this(context, new TypeTranslator(context.DefinitionProvider), new MethodTranslator(context.DefinitionProvider))
        { }

        public XmlDeserializer(IParameterContext context, ITypeTranslator typeTranslator, IMethodTranslator methodTranslator)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _typeTranslator = typeTranslator ?? throw new ArgumentNullException(nameof(typeTranslator));
            _methodTranslator = methodTranslator ?? throw new ArgumentNullException(nameof(methodTranslator));
        }

        public IParameterSet Deserialize(Stream input)
        {
            XElement setXml = XElement.Load(input);

            return ToParameterSet(setXml);
        }

        public IParameterSet ToParameterSet(XElement xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException(nameof(xml));
            }

            IParameterSet set = xml.Name.LocalName switch
            {
                XmlBaseParameterSet => new BaseParameterSet(_context),
                XmlExecutionParameterSet => new BaseParameterSet(_context),
                _ => throw new ArgumentException("Invalid XML element name.")
            };

            foreach (XElement parameterXml in xml.Elements())
            {
                IParameter parameter = ToParameter(parameterXml, set);
            }

            return set;
        }

        public ParameterAccessor ToAccessor(XElement xml)
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
                            String2Method(xml.Attribute(XmlMethodSignature)?.Value ?? throw new MissingAttributeException(nameof(MethodResultParameterAccessor), XmlMethodSignature)),
                            int.Parse(xml.Attribute(XmlInvocation)?.Value ?? throw new MissingAttributeException(nameof(MethodResultParameterAccessor), XmlInvocation)),
                            String2Ref(xml.Attribute(XmlReference)?.Value.Substring(2) ?? throw new MissingAttributeException(nameof(MethodResultParameterAccessor), XmlReference))
                        );
                    break;

                case XmlItem:
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

                case XmlReturnValue:
                    accessor = new ReturnValueParameterAccessor();
                    break;

                //case XmlNoAccessor:
                //    accessor = null;
                //    break;

                default:
                    throw new NotSupportedException();
            }

            return accessor;
        }

        public IParameter ToParameter(XElement xml, IParameterSet set)
        {
            string parameterType = xml.Name.LocalName;

            IParameter? parameter;

            switch (parameterType)
            {
                case XmlObject: parameter = ToObjectParameter(xml, set); break;
                case XmlArray: parameter = ToArrayParameter(xml, set); break;
                case XmlPrimitiveValue: parameter = ToPrimitiveValueParameter(xml, set); break;
                case XmlStruct: parameter = ToStructParameter(xml, set); break;
                default: throw new NotSupportedException(xml.ToString());
            }

            foreach (XElement accessorElement in xml.Elements(XmlAccessor))
            {
                parameter.Accessors.Add(ToAccessor(accessorElement));
            }

            return parameter;
        }

        private IObjectParameter ToObjectParameter(XElement xml, IParameterSet set)
        {
            TypeSignature type = String2Type(xml.Attribute(XmlType)?.Value ?? throw new MissingAttributeException(nameof(IObjectParameter), XmlType));
            bool? isNull = String2Bool(xml.Attribute(XmlIsNull)?.Value ?? throw new MissingAttributeException(nameof(IObjectParameter), XmlIsNull));
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(IObjectParameter), XmlReference));

            IObjectParameter objectParameter = set.CreateObjectParameter(type, reference, isNull);

            foreach (XElement fieldXml in xml.Elements(XmlField))
            {
                string fieldName = fieldXml.Attribute(XmlName)?.Value ?? throw new MissingAttributeException(nameof(IObjectParameter) + "/Field", XmlName);
                reference = String2Ref(fieldXml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(IObjectParameter) + "/Field", XmlReference));

                objectParameter.SetField(fieldName, reference);
            }

            foreach (XElement methodResultXml in xml.Elements(XmlMethodResult))
            {
                MethodSignature methodSignature = String2Method(methodResultXml.Attribute(XmlMethodSignature)?.Value ?? throw new MissingAttributeException(nameof(IObjectParameter) + "/MethodResult", XmlMethodSignature));
                int invocation = int.Parse(methodResultXml.Attribute(XmlInvocation)?.Value ?? throw new MissingAttributeException(nameof(IObjectParameter) + "/MethodResult", XmlInvocation));
                reference = String2Ref(methodResultXml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(IObjectParameter) + "/MethodResult", XmlReference));

                objectParameter.SetMethodResult(methodSignature, invocation, reference);
            }


            return objectParameter;
        }

        private IStructParameter ToStructParameter(XElement xml, IParameterSet set)
        {
            TypeSignature type = String2Type(xml.Attribute(XmlType)?.Value ?? throw new MissingAttributeException(nameof(IStructParameter), XmlType));
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(IStructParameter), XmlReference));

            IStructParameter structParameter = set.CreateStructParameter(type, reference);

            foreach (XElement fieldXml in xml.Elements(XmlField))
            {
                string fieldName = fieldXml.Attribute(XmlName)?.Value ?? throw new MissingAttributeException(nameof(IStructParameter) + "/Field", XmlName);
                reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(IStructParameter) + "/Field", XmlReference));

                structParameter.SetField(fieldName, reference);
            }

            return structParameter;
        }

        private IArrayParameter ToArrayParameter(XElement xml, IParameterSet set)
        {
            TypeSignature type = String2Type(xml.Attribute(XmlElementType)?.Value ?? throw new MissingAttributeException(nameof(IArrayParameter), XmlElementType));
            bool? isNull = String2Bool(xml.Attribute(XmlIsNull)?.Value ?? throw new MissingAttributeException(nameof(IArrayParameter), XmlIsNull));
            int? length = String2Int(xml.Attribute(XmlLength)?.Value ?? throw new MissingAttributeException(nameof(IArrayParameter), XmlLength));
            ParameterRef reference = String2Ref(xml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(IArrayParameter), XmlReference));

            IArrayParameter arrayParameter = set.CreateArrayParameter(type, reference, isNull, length);

            foreach (XElement itemXml in xml.Elements(XmlItem))
            {
                int index = int.Parse(itemXml.Attribute(XmlIndex)?.Value ?? throw new MissingAttributeException(nameof(IArrayParameter) + "/Item", XmlIndex));
                reference = String2Ref(itemXml.Attribute(XmlReference)?.Value ?? throw new MissingAttributeException(nameof(IArrayParameter) + "/Item", XmlReference));

                arrayParameter.SetItem(index, reference);
            }



            return arrayParameter;
        }
    }
}
