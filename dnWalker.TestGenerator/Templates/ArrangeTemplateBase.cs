using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic.Variables;

using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using dnlib.DotNet;
using dnWalker.TestGenerator.Symbolic.Heap;

namespace dnWalker.TestGenerator.Templates
{
    public abstract class ArrangeTemplateBase : IArrangeTemplate
    {
        private readonly Dictionary<TypeSig, int> _typeCounter = new Dictionary<TypeSig, int>();

        public abstract IEnumerable<string> Namespaces { get; }

        private int GetCounterValue(TypeSig type)
        {
            if (!_typeCounter.TryGetValue(type, out int value))
            {
                value = 1;
                _typeCounter[type] = value;
            }
            else
            {
                value++;
                _typeCounter[type] = value;
            }
            return value;
        }

        protected virtual string GenerateName(IReadOnlyHeapNode heapNode)
        {
            int counter = GetCounterValue(heapNode.Type);

            if (heapNode is IReadOnlyObjectHeapNode objectNode)
            {
                return FirstCharToLower($"{objectNode.Type.TypeName}{counter}");
            }
            else if (heapNode is IReadOnlyArrayHeapNode arrayNode)
            {
                return FirstCharToLower($"{arrayNode.ElementType.TypeName}Array{counter}");
            }

            throw new NotSupportedException("Unexpected heap node type.");
        }

        private static string FirstCharToLower(string str)
        {
            return string.Create(str.Length, str, static (span, str) =>
            {
                span[0] = char.ToLower(str[0]);
                str.AsSpan(1).CopyTo(span.Slice(1));
            });
        }

        protected string GetName(IReadOnlyHeapNode arrayNode, IDictionary<Location, string> locationNames)
        {
            Location location = arrayNode.Location;
            if (!locationNames.TryGetValue(location, out string? name))
            {
                name = GenerateName(arrayNode);
                locationNames.Add(location, name);
            }
            return name;
        }

        protected string GetLiteral(IValue value, IDictionary<Location, string> locationNames)
        {
            string result;
            if (value is Location location)
            {
                if (location == Location.Null)
                {
                    result = "null";
                }
                else if (!locationNames.TryGetValue(location, out result))
                {
                    result = $"LOCATION_{location.Value:X8}";
                }
            }
            else
            {
                result = value.ToString()!;
            }
            return result;
        }

        public IDictionary<Location, string> WriteArrange(IWriter output, IReadOnlyModel model, IMethod testedMethod)
        {
            IDictionary<Location, string> locationNames = new Dictionary<Location, string>();
            
            WriteArrangeHeap(output, model.HeapInfo, locationNames);

            ArrangeStaticFields(output, model, locationNames);
            ArrangeMethodArguments(output, model, locationNames, testedMethod);

            return locationNames;
        }

        #region Arrange Heap

        protected virtual void WriteArrangeHeap(IWriter output, IReadOnlyHeapInfo heap, IDictionary<Location, string> locationNames)
        {
            // create & sort heap node groups using dependency graph
            HeapGraph graph = heap.CreateGraph();

            IReadOnlyList<DependencyGroup> dependencyGroups = graph.GetDependencyGroups();

            // arrange each heap node group
            foreach (DependencyGroup dg in dependencyGroups)
            {
                WriteArrangeNodeGroup(output, dg, locationNames);
            }
        }

        /// <summary>
        /// Arranges a node group
        /// </summary>
        /// <param name="output"></param>
        /// <param name="nodes"></param>
        /// <param name="locationNames"></param>
        protected virtual void WriteArrangeNodeGroup(IWriter output, IReadOnlyCollection<IReadOnlyHeapNode> group, IDictionary<Location, string> locationNames)
        {
            // declare the variables & create the objects
            foreach (IReadOnlyHeapNode node in group)
            {
                WriteVariableDeclaration(output, node, locationNames);
                output.Write(TemplateUtils.AssignmentOperator);
                switch (node)
                {
                    case IReadOnlyArrayHeapNode arrayNode: WriteArrayCreation(output, arrayNode, locationNames); break;
                    case IReadOnlyObjectHeapNode objectNode: WriteObjectCreation(output, objectNode, locationNames); break;
                    default:
                        throw new NotSupportedException("Unexpected node type.");
                }
            }

            // set fields & elements & method results
            foreach (IReadOnlyHeapNode node in group)
            {
                switch (node)
                {
                    case IReadOnlyArrayHeapNode arrayNode: WriteArrayInitialization(output, arrayNode, locationNames); break;
                    case IReadOnlyObjectHeapNode objectNode: WriteObjectInitialization(output, objectNode, locationNames); break;
                    default:
                        throw new NotSupportedException("Unexpected node type.");
                }
            }
        }

        protected virtual void WriteVariableDeclaration(IWriter output, IReadOnlyHeapNode node, IDictionary<Location, string> locationNames, bool newLine = false)
        {
            output.Write(node.Type);
            output.Write(TemplateUtils.WhiteSpace);
            output.Write(GetName(node, locationNames));

            if (newLine)
            {
                output.WriteLine(";");
            }
        }

        protected virtual void WriteObjectCreation(IWriter output, IReadOnlyObjectHeapNode objectNode, IDictionary<Location, string> locationNames)
        {
            // TODO: how to handle non parameterless constructor? - from dependency graph the values should already be created...
            output.Write("new ");
            output.Write(objectNode.Type);
            output.WriteLine("();");
        }

        protected virtual void WriteArrayCreation(IWriter output, IReadOnlyArrayHeapNode arrayNode, IDictionary<Location, string> locationNames)
        {
            output.Write("new ");
            output.Write(arrayNode.ElementType);
            output.Write($"[{arrayNode.Length}];");
        }

        protected virtual void WriteObjectInitialization(IWriter output, IReadOnlyObjectHeapNode objectNode, IDictionary<Location, string> locationNames)
        {
            if (objectNode.HasFields)
            {
                WriteObjectFieldsInitialization(output, objectNode, locationNames);
            }
            if (objectNode.HasMethodInvocations)
            {
                WriteObjectMethodsInitialization(output, objectNode, locationNames);
            }
        }

        protected virtual void WriteObjectFieldsInitialization(IWriter output, IReadOnlyObjectHeapNode objectNode, IDictionary<Location, string> locationNames)
        {
            string varName = GetName(objectNode, locationNames);

            // this basic implementation doesn't know how to mock virtual/abstract/interface methods
            foreach (IField fld in objectNode.Fields)
            {
                string fieldValueLiteral = GetLiteral(objectNode.GetField(fld), locationNames);

                FieldDef fldDef = fld.ResolveFieldDefThrow();
                TypeSig declaringType = fld.DeclaringType.ToTypeSig();
                string fldName = fld.Name;

                if (fldDef.IsPublic)
                {
                    if (fldDef.IsInitOnly)
                    {
                        // must use reflection to set the field :(
                        output.Write("typeof(");
                        output.Write(declaringType);
                        output.WriteLine($").GetField(\"{fldName}\", Flags.Instance | Flags.Public).SetValue({varName}, {fieldValueLiteral});"); // calling Write with the format & args overload would be faster...
                    }
                    else
                    {
                        output.WriteLine($"{varName}.{fldName} = {fieldValueLiteral};"); // using output.WriteLine(format, args) would be faster...
                    }
                }
                else
                {
                    // must use reflection to set the field :(
                    output.Write("typeof(");
                    output.Write(declaringType);
                    output.WriteLine($").GetField(\"{fldName}\", Flags.Instance | Flags.NonPublic).SetValue({varName}, {fieldValueLiteral});"); // calling Write with the format & args overload would be faster...
                }
            }
        }

        protected abstract void WriteObjectMethodsInitialization(IWriter output, IReadOnlyObjectHeapNode objectNode, IDictionary<Location, string> locationNames);

        protected virtual void WriteArrayInitialization(IWriter output, IReadOnlyArrayHeapNode arrayNode, IDictionary<Location, string> locationNames)
        {
            string varName = GetName(arrayNode, locationNames);
            foreach (int index in arrayNode.Indeces)
            {
                string elementValueLiteral = GetLiteral(arrayNode.GetElement(index), locationNames);
                output.WriteLine($"{varName}[{index}] = {elementValueLiteral};");
            }
        }

        #endregion Arrange Heap

        #region Arrange Variables
        private void ArrangeStaticFields(IWriter output, IReadOnlyModel model, IDictionary<Location, string> locationNames)
        {
            foreach (StaticFieldVariable staticFieldVar in model.Variables.OfType<StaticFieldVariable>())
            {
                ArrangeStaticField(output, staticFieldVar, GetLiteral(model.GetValueOrDefault(staticFieldVar), locationNames));
            }
        }

        private void ArrangeStaticField(IWriter output, StaticFieldVariable variable, string literal)
        {
            IField fld = variable.Field;

            FieldDef fldDef = fld.ResolveFieldDefThrow();
            TypeSig declaringType = fld.DeclaringType.ToTypeSig();
            string fldName = fld.Name;

            if (fldDef.IsInitOnly)
            {
                // should not happen...
                Debug.Fail("Cannot set init only static field.");
            }

            if (fldDef.IsPublic)
            {
                // easily set
                output.Write(declaringType);
                output.Write(TemplateUtils.Coma);
                output.Write(fldName);
                output.Write(TemplateUtils.AssignmentOperator);
                output.WriteLine(literal);
            }
            else
            {
                // TODO: check for property setter...

                // fld def is not public => we need to use reflection to set it...
                output.Write("typeof(");
                output.Write(declaringType);
                output.WriteLine($").GetField(\"{fldName}\", Flags.Static | Flags.NonPublic).SetValue(null, {literal});"); // calling Write with the format & args overload would be faster...
            }
        }

        private void ArrangeMethodArguments(IWriter output, IReadOnlyModel model, IDictionary<Location, string> locationNames, IMethod method)
        {
            MethodDef md = method.ResolveMethodDefThrow();

            IList<Parameter> parameters = md.Parameters;
            
            //foreach (MethodArgumentVariable methodArgumentVar in model.Variables.OfType<MethodArgumentVariable>())
            foreach (Parameter p in parameters)
            {
                MethodArgumentVariable methodArgumentVar = new MethodArgumentVariable(p);

                ArrangeMethodArgument(output, methodArgumentVar, GetLiteral(model.GetValueOrDefault(methodArgumentVar), locationNames));
            }
        }

        private void ArrangeMethodArgument(IWriter output, MethodArgumentVariable methodArgumentVar, string literal)
        {
            output.Write(methodArgumentVar.Parameter.Type);
            output.Write(TemplateUtils.WhiteSpace);
            output.Write(methodArgumentVar.Parameter.Name);
            output.Write(TemplateUtils.AssignmentOperator);
            output.WriteLine(literal);
        }
        #endregion Arrange Variables
    }
}
