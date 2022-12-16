using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic.Variables;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dnWalker.Symbolic.Xml
{
    public class XmlModelSerializer
    {
        private readonly ITypeParser _typeParser;
        private readonly IMethodParser _methodParser;

        public XmlModelSerializer(ITypeParser typeParser, IMethodParser methodParser)
        {
            _typeParser = typeParser ?? throw new ArgumentNullException(nameof(typeParser));
            _methodParser = methodParser ?? throw new ArgumentNullException(nameof(methodParser));
        }

        public XElement ToXml(IReadOnlyModel model, string? name = null)
        {
            if (name == null) name = XmlTokens.Model;

            var modelXml = new XElement(name);

            modelXml.Add(VariablesToXml(model));
            modelXml.Add(HeapToXml(model.HeapInfo));

            return modelXml;
        }
        private string ValueToXml(IValue value)
        {
            // special cases
            if (value is PrimitiveValue<double> pDbl)
            {
                if (double.IsNegativeInfinity(pDbl.Value)) return XmlTokens.NegativeInfinity;
                if (double.IsPositiveInfinity(pDbl.Value)) return XmlTokens.PositiveInfinity;
                if (double.IsNaN(pDbl.Value)) return XmlTokens.NaN;
            }
            if (value is PrimitiveValue<float> pFlt)
            {
                if (float.IsNegativeInfinity(pFlt.Value)) return XmlTokens.NegativeInfinity;
                if (float.IsPositiveInfinity(pFlt.Value)) return XmlTokens.PositiveInfinity;
                if (float.IsNaN(pFlt.Value)) return XmlTokens.NaN;
            }

            return value.ToString()!;
        }

        #region Serialize Variables
        private XElement VariablesToXml(IReadOnlyModel model)
        {
            var variablesXml = new XElement(XmlTokens.Variables);

            foreach (var variable in model.Variables)
            {
                variablesXml.Add(VariableToXml(variable, model));
            }

            return variablesXml;
        }

        private XElement VariableToXml(IRootVariable variable, IReadOnlyModel model)
        {
            var variableXml = variable switch
            {
                MethodArgumentVariable mav => MethodArgumentVariableToXml(mav),
                StaticFieldVariable sfv => StaticFieldVariableToXml(sfv),
                ReturnValueVariable rvv => ReturnValueVariableToXml(rvv),
                _ => throw new NotSupportedException()
            };

            variableXml.SetAttributeValue(XmlTokens.Value, ValueToXml(model.GetValueOrDefault(variable)));

            return variableXml;
        }

        private XElement ReturnValueVariableToXml(ReturnValueVariable rvv)
        {
            var xml = new XElement(XmlTokens.ReturnValue);

            xml.SetAttributeValue(XmlTokens.MethodSignature, rvv.Method.FullName);

            return xml;
        }

        private XElement MethodArgumentVariableToXml(MethodArgumentVariable mav)
        {
            var xml = new XElement(XmlTokens.MethodArgument);

            xml.SetAttributeValue(XmlTokens.Name, mav.Parameter.Name);

            return xml;
        }
        private XElement StaticFieldVariableToXml(StaticFieldVariable sfv)
        {
            var xml = new XElement(XmlTokens.StaticField);

            xml.SetAttributeValue(XmlTokens.Type, sfv.Field.DeclaringType.ToString());
            xml.SetAttributeValue(XmlTokens.FieldName, sfv.Field.Name);

            return xml;
        }
        #endregion Serialize Variables

        #region Serialize Heap
        private XElement HeapToXml(IReadOnlyHeapInfo heap)
        {
            var heapXml = new XElement(XmlTokens.Heap);

            foreach (HeapNode node in heap.Nodes)
            {
                var nodeXml = node switch
                {
                    IReadOnlyObjectHeapNode ohn => ObjectNodeToXml(ohn),
                    IReadOnlyArrayHeapNode ahn => ArrayNodeToXml(ahn),
                    _ => throw new NotSupportedException()
                };

                nodeXml.SetAttributeValue(XmlTokens.Location, node.Location.ToString());
                nodeXml.SetAttributeValue(XmlTokens.Type, node.Type.ToString());

                heapXml.Add(nodeXml);
            }

            return heapXml;
        }

        private XElement ObjectNodeToXml(IReadOnlyObjectHeapNode ohn)
        {
            var xml = new XElement(XmlTokens.ObjectNode);
            xml.SetAttributeValue(XmlTokens.IsDirty, ohn.IsDirty);

            foreach (var fld in ohn.Fields)
            {
                var fieldXml = new XElement(XmlTokens.InstanceField);

                fieldXml.SetAttributeValue(XmlTokens.DeclaringType, fld.DeclaringType.ToString());
                fieldXml.SetAttributeValue(XmlTokens.FieldName, fld.Name);

                fieldXml.SetAttributeValue(XmlTokens.Value, ValueToXml(ohn.GetFieldOrDefault(fld)));

                xml.Add(fieldXml);
            }

            foreach ((var method, var invocation) in ohn.MethodInvocations)
            {
                var methodXml = new XElement(XmlTokens.MethodResult);

                methodXml.SetAttributeValue(XmlTokens.Method, method.ToString());
                methodXml.SetAttributeValue(XmlTokens.Invocation, invocation);

                methodXml.SetAttributeValue(XmlTokens.Value, ValueToXml(ohn.GetMethodResult(method, invocation)));

                xml.Add(methodXml);
            }
            return xml;
        }

        private XElement ArrayNodeToXml(IReadOnlyArrayHeapNode ahn)
        {
            var xml = new XElement(XmlTokens.ArrayNode);
            xml.SetAttributeValue(XmlTokens.IsDirty, ahn.IsDirty);
            xml.SetAttributeValue(XmlTokens.Length, ahn.Length);

            foreach (var index in ahn.Indeces)
            {
                var elementXml = new XElement(XmlTokens.ArrayElement);
                elementXml.SetAttributeValue(XmlTokens.Index, index);
                elementXml.SetAttributeValue(XmlTokens.Value, ValueToXml(ahn.GetElementOrDefault(index)));

                xml.Add(elementXml);
            }

            return xml;
        }
        #endregion Serialize Heap
    }
}
