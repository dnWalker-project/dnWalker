using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;

using dnWalker.Symbolic.Variables;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using static dnWalker.Symbolic.Xml.XmlUtils;

namespace dnWalker.Symbolic.Xml
{
    public class XmlModelDeserializer
    {
        private readonly ITypeParser _typeParser;
        private readonly IMethodParser _methodParser;

        public XmlModelDeserializer(ITypeParser typeParser, IMethodParser methodParser)
        {
            _typeParser = typeParser ?? throw new ArgumentNullException(nameof(typeParser));
            _methodParser = methodParser ?? throw new ArgumentNullException(nameof(methodParser));
        }

        public IReadOnlyModel FromXml(XElement xml, IMethod method)
        {
            var constraint = new Constraint();//Constraint.Parse(GetAttribute(xml, "Constraint"));
            var model = new Model(constraint);

            {
                // variables
                var variablesXml = GetElement(xml, XmlTokens.Variables);
                foreach (var varXml in variablesXml.Elements())
                {
                    IRootVariable variable = varXml.Name.LocalName switch
                    {
                        XmlTokens.MethodArgument => MethodArgumentVariableFromXml(varXml, method),
                        XmlTokens.StaticField => StaticFieldVariableFromXml(varXml),
                        XmlTokens.ReturnValue => ReturnValueVariableFromXml(varXml, method),
                        _ => throw new NotSupportedException()
                    };

                    model.SetValue(variable, ValueFromXml(GetAttribute(varXml, XmlTokens.Value).Value, variable.Type));
                }

            }
            {
                // heap
                var heapXml = GetElement(xml, XmlTokens.Heap);
                var heapInfo = model.HeapInfo;
                foreach (var nodeXml in heapXml.Elements())
                {
                    var location = new Location((uint)GetAttribute(nodeXml, XmlTokens.Location));
                    var type = _typeParser.Parse((string)GetAttribute(nodeXml, XmlTokens.Type)).ToTypeDefOrRef().ToTypeSig();

                    var heapNode = nodeXml.Name.LocalName switch
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


        private static IValue ValueFromXml(string xml, TypeSig type)
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
            var objectNode = new ObjectHeapNode(location, type, (bool?)nodeXml.Attribute(XmlTokens.IsDirty) ?? false);

            foreach (var fieldXml in nodeXml.Elements(XmlTokens.InstanceField))
            {
                var declaringType = _typeParser.Parse((string)GetAttribute(fieldXml, XmlTokens.DeclaringType))
                    .ToTypeDefOrRef()
                    .ResolveTypeDefThrow();

                var fieldName = (string)GetAttribute(fieldXml, XmlTokens.FieldName);
                var valueXMl = (string)GetAttribute(fieldXml, XmlTokens.Value);

                var field = declaringType.FindField(fieldName);
                var value = ValueFromXml(valueXMl, field.FieldType);

                objectNode.SetField(field, value);
            }

            foreach (var methodXml in nodeXml.Elements(XmlTokens.MethodResult))
            {
                var method = _methodParser.Parse((string)GetAttribute(methodXml, XmlTokens.Method));
                var invocation = (int)GetAttribute(methodXml, XmlTokens.Invocation);

                var value = ValueFromXml((string)GetAttribute(methodXml, XmlTokens.Value), method.MethodSig.RetType);

                objectNode.SetMethodResult(method, invocation, value);
            }

            return objectNode;
        }

        private IHeapNode ArrayNodeFromXml(XElement nodeXml, Location location, TypeSig elementType)
        {
            var arrayNode = new ArrayHeapNode(location, elementType, (int)GetAttribute(nodeXml, XmlTokens.Length), (bool?)nodeXml.Attribute(XmlTokens.IsDirty) ?? false);

            foreach (var elementXml in nodeXml.Elements(XmlTokens.ArrayElement))
            {
                var index = (int)GetAttribute(elementXml, XmlTokens.Index);
                var value = ValueFromXml((string)GetAttribute(elementXml, XmlTokens.Value), elementType);
            }

            return arrayNode;
        }
        #endregion Deserialize Heap

        #region Deserialize Variables
        private MethodArgumentVariable MethodArgumentVariableFromXml(XElement varXml, IMethod method)
        {
            var name = GetAttribute(varXml, XmlTokens.Name).Value;
            var parameter = method.ResolveMethodDefThrow().Parameters.First(p => p.Name == name);
            return new MethodArgumentVariable(parameter);
        }

        private StaticFieldVariable StaticFieldVariableFromXml(XElement varXml)
        {
            var fieldName = GetAttribute(varXml, XmlTokens.FieldName).Value;
            var type = _typeParser.Parse(GetAttribute(varXml, XmlTokens.Type).Value).ToTypeDefOrRef();
            IField field = type.ResolveTypeDefThrow().Fields.First(fld => fld.Name == fieldName);
            return new StaticFieldVariable(field);
        }

        private ReturnValueVariable ReturnValueVariableFromXml(XElement varXml, IMethod method)
        {
            return new ReturnValueVariable(method);
        }
        #endregion Deserialize Variables
    }
}
