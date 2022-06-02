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

namespace dnWalker.Explorations.Xml
{
    public class XmlModelSerializer
    {
        private readonly ITypeTranslator _typeTranslator;
        private readonly IMethodTranslator _methodTranslator;

        public XmlModelSerializer(ITypeTranslator typeTranslator, IMethodTranslator methodTranslator)
        {
            _typeTranslator = typeTranslator ?? throw new ArgumentNullException(nameof(typeTranslator));
            _methodTranslator = methodTranslator ?? throw new ArgumentNullException(nameof(methodTranslator));
        }

        public XElement ToXml(IReadOnlyModel model, string? name = null)
        {
            if (name == null) name = XmlTokens.Model;

            XElement modelXml = new XElement(name);

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
            XElement variablesXml = new XElement(XmlTokens.Variables);

            foreach (IRootVariable variable in model.Variables)
            {
                variablesXml.Add(VariableToXml(variable, model));
            }

            return variablesXml;
        }

        private XElement VariableToXml(IRootVariable variable, IReadOnlyModel model)
        {
            XElement variableXml = variable switch
            {
                MethodArgumentVariable mav => MethodArgumentVariableToXml(mav),
                StaticFieldVariable sfv => StaticFieldVariableToXml(sfv),
                _ => throw new NotSupportedException()
            };

            variableXml.SetAttributeValue(XmlTokens.Value, ValueToXml(model.GetValueOrDefault(variable)));

            return variableXml;
        }

        private XElement MethodArgumentVariableToXml(MethodArgumentVariable mav)
        {
            XElement xml = new XElement(XmlTokens.MethodArgument);

            xml.SetAttributeValue(XmlTokens.Name, mav.Parameter.Name);

            return xml;
        }
        private XElement StaticFieldVariableToXml(StaticFieldVariable sfv)
        {
            XElement xml = new XElement(XmlTokens.StaticField);

            xml.SetAttributeValue(XmlTokens.Type, _typeTranslator.GetString(sfv.Field.DeclaringType));
            xml.SetAttributeValue(XmlTokens.FieldName, sfv.Field.Name);

            return xml;
        }
        #endregion Serialize Variables

        #region Serialize Heap
        private XElement HeapToXml(IReadOnlyHeapInfo heap)
        {
            XElement heapXml = new XElement(XmlTokens.Heap);

            foreach (HeapNode node in heap.Nodes)
            {
                XElement nodeXml = node switch
                {
                    IReadOnlyObjectHeapNode ohn => ObjectNodeToXml(ohn),
                    IReadOnlyArrayHeapNode ahn => ArrayNodeToXml(ahn),
                    _ => throw new NotSupportedException()
                };

                nodeXml.SetAttributeValue(XmlTokens.Location, node.Location.ToString());
                nodeXml.SetAttributeValue(XmlTokens.Type, _typeTranslator.GetString(node.Type));
            }

            return heapXml;
        }

        private XElement ObjectNodeToXml(IReadOnlyObjectHeapNode ohn)
        {
            XElement xml = new XElement(XmlTokens.ObjectNode);
            xml.SetAttributeValue(XmlTokens.IsDirty, ohn.IsDirty);

            foreach (IField fld in ohn.Fields)
            {
                XElement fieldXml = new XElement(XmlTokens.InstanceField);

                fieldXml.SetAttributeValue(XmlTokens.DeclaringType, _typeTranslator.GetString(fld.DeclaringType));
                fieldXml.SetAttributeValue(XmlTokens.FieldName, fld.Name);
                
                fieldXml.SetAttributeValue(XmlTokens.Value, ValueToXml(ohn.GetField(fld)));

                xml.Add(fieldXml);
            }

            foreach ((IMethod method, int invocation) in ohn.MethodInvocations)
            {
                XElement methodXml = new XElement(XmlTokens.MethodResult);

                methodXml.SetAttributeValue(XmlTokens.Method, _methodTranslator.GetString(method));
                methodXml.SetAttributeValue(XmlTokens.Invocation, invocation);

                methodXml.SetAttributeValue(XmlTokens.Value, ValueToXml(ohn.GetMethodResult(method, invocation)));

                xml.Add(methodXml);
            }
            return xml;
        }

        private XElement ArrayNodeToXml(IReadOnlyArrayHeapNode ahn)
        {
            XElement xml = new XElement(XmlTokens.ArrayNode);
            xml.SetAttributeValue(XmlTokens.IsDirty, ahn.IsDirty);
            xml.SetAttributeValue(XmlTokens.Length, ahn.Length);

            foreach (int index in ahn.Indeces)
            {
                XElement elementXml = new XElement(XmlTokens.ArrayElement);
                elementXml.SetAttributeValue(XmlTokens.Index, index);
                elementXml.SetAttributeValue(XmlTokens.Value, ValueToXml(ahn.GetElement(index)));

                xml.Add(elementXml);
            }

            return xml;
        }
        #endregion Serialize Heap
    }
}
