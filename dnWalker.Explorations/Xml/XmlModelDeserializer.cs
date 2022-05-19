using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic.Utils;
using dnWalker.Symbolic.Variables;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using static dnWalker.Explorations.Xml.XmlUtils;

namespace dnWalker.Explorations.Xml
{
    public class XmlModelDeserializer
    {
        private readonly ITypeTranslator _typeTranslator;
        private readonly IMethodTranslator _methodTranslator;

        public XmlModelDeserializer(ITypeTranslator typeTranslator, IMethodTranslator methodTranslator)
        {
            _typeTranslator = typeTranslator ?? throw new ArgumentNullException(nameof(typeTranslator));
            _methodTranslator = methodTranslator ?? throw new ArgumentNullException(nameof(methodTranslator));
        }

        public IReadOnlyModel FromXml(XElement xml, IMethod method)
        {
            Constraint constraint = new Constraint();//Constraint.Parse(GetAttribute(xml, "Constraint"));
            Model model = new Model(constraint);

            {
                // variables
                XElement variablesXml = GetElement(xml, XmlTokens.Variables);
                foreach (XElement varXml in variablesXml.Elements())
                {
                    IRootVariable variable = varXml.Name.LocalName switch
                    {
                        XmlTokens.MethodArgument => MethodArgumentVariableFromXml(varXml, method),
                        XmlTokens.StaticField => StaticFieldVariableFromXml(varXml),
                        _ => throw new NotSupportedException()
                    };

                    model.SetValue(variable, ValueFromXml(GetAttribute(varXml, XmlTokens.Value).Value, variable.Type));
                }

            }
            {
                // heap
                XElement heapXml = GetElement(xml, XmlTokens.Heap);
                IHeapInfo heapInfo = model.HeapInfo;
                foreach (XElement nodeXml in heapXml.Elements())
                {
                    Location location = new Location((uint)GetAttribute(nodeXml, XmlTokens.Location));
                    TypeSig type = _typeTranslator.FromString((string)GetAttribute(nodeXml, XmlTokens.Type)).ToTypeDefOrRef().ToTypeSig();

                    IHeapNode heapNode = nodeXml.Name.LocalName switch
                    {
                        XmlTokens.ObjectNode => ObjectNodeFromXml(nodeXml, location, type),
                        XmlTokens.ArrayNode => ArrayNodeFromXml(nodeXml, location, type),
                        _ => throw new NotSupportedException()
                    };
                    heapInfo.AddNode(heapNode);
                }
            }

            return model;
        }


        private IValue ValueFromXml(string xml, TypeSig type)
        {
            if (type.IsPrimitive)
            {
                if (type.IsBoolean())
                {
                    return new PrimitiveValue<bool>(bool.Parse(xml));
                }
                else if (type.IsChar())
                {
                    return new PrimitiveValue<char>(xml[0]);
                }
                else if (type.IsByte())
                {
                    return new PrimitiveValue<byte>(byte.Parse(xml));
                }
                else if (type.IsUInt16())
                {
                    return new PrimitiveValue<ushort>(ushort.Parse(xml));
                }
                else if (type.IsUInt32())
                {
                    return new PrimitiveValue<uint>(uint.Parse(xml));
                }
                else if (type.IsUInt64())
                {
                    return new PrimitiveValue<ulong>(ulong.Parse(xml));
                }
                else if (type.IsSByte())
                {
                    return new PrimitiveValue<sbyte>(sbyte.Parse(xml));
                }
                else if (type.IsInt16())
                {
                    return new PrimitiveValue<short>(short.Parse(xml));
                }
                else if (type.IsInt32())
                {
                    return new PrimitiveValue<int>(int.Parse(xml));
                }
                else if (type.IsInt64())
                {
                    return new PrimitiveValue<long>(long.Parse(xml));
                }
                else if (type.IsSingle())
                {
                    if (xml == "-INF") return new PrimitiveValue<float>(float.NegativeInfinity);
                    if (xml == "+INF") return new PrimitiveValue<float>(float.PositiveInfinity);
                    if (xml == "NAN") return new PrimitiveValue<float>(float.NaN);

                    return new PrimitiveValue<float>(float.Parse(xml));
                }
                else if (type.IsDouble())
                {
                    if (xml == "-INF") return new PrimitiveValue<double>(double.NegativeInfinity);
                    if (xml == "+INF") return new PrimitiveValue<double>(double.PositiveInfinity);
                    if (xml == "NAN") return new PrimitiveValue<double>(double.NaN);

                    return new PrimitiveValue<double>(double.Parse(xml));
                }

                throw new NotSupportedException("Unexpected primitive type.");
            }
            else if (type.IsString())
            {
                return new StringValue(xml);
            }
            else
            {
                return new Location(uint.Parse(xml.AsSpan(1), System.Globalization.NumberStyles.HexNumber));
            }
        }


        #region Deserialize Heap
        private IHeapNode ObjectNodeFromXml(XElement nodeXml, Location location, TypeSig type)
        {
            ObjectHeapNode objectNode = new ObjectHeapNode(location, type);

            Debug.Fail("Not yet implemented!!! TODO: set fields and method results");

            return objectNode;
        }

        private IHeapNode ArrayNodeFromXml(XElement nodeXml, Location location, TypeSig elementType)
        {
            ArrayHeapNode arrayNode = new ArrayHeapNode(location, elementType, (int)GetAttribute(nodeXml, XmlTokens.Length));

            Debug.Fail("Not yet implemented!!! TODO: set method results");

            return arrayNode;
        }
        #endregion Deserialize Heap

        #region Deserialize Variables
        private MethodArgumentVariable MethodArgumentVariableFromXml(XElement varXml, IMethod method)
        {
            string name = GetAttribute(varXml, XmlTokens.Name).Value;
            Parameter parameter = method.ResolveMethodDefThrow().Parameters.First(p => p.Name == name);
            return new MethodArgumentVariable(parameter);
        }

        private StaticFieldVariable StaticFieldVariableFromXml(XElement varXml)
        {
            string fieldName = GetAttribute(varXml, XmlTokens.FieldName).Value;
            ITypeDefOrRef type = _typeTranslator.FromString(GetAttribute(varXml, XmlTokens.Type).Value).ToTypeDefOrRef();
            IField field = type.ResolveTypeDefThrow().Fields.First(fld => fld.Name == fieldName);
            return new StaticFieldVariable(field);
        }
        #endregion Deserialize Variables
    }
}
