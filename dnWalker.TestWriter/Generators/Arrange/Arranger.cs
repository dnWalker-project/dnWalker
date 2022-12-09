using dnlib.DotNet;
using dnlib.DotNet.Writer;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic.Heap.Graphs;
using dnWalker.Symbolic.Variables;
using dnWalker.TestWriter.TestModels;
using dnWalker.TestWriter.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators.Arrange
{
    internal class Arranger
    {
        private readonly ITestTemplate _template;
        private readonly ITestContext _context;
        private readonly IWriter _output;
        private readonly IReadOnlyModel _model;

        private readonly HeapGraph _heapGraph;

        public Arranger(ITestTemplate testTemplate, ITestContext context, IWriter output)
        {
            _template = testTemplate;
            _context = context;
            _output = output;
            _model = context.Iteration.InputModel;

            _heapGraph = _model.HeapInfo.CreateGraph();

            // generate the symbol contexts
            SetupSymbolContext(_heapGraph.GetDependencyGroups());
        }
        public void WriteArrangeHeap()
        {
            IReadOnlyHeapInfo heap = _model.HeapInfo;
            if (!heap.IsEmpty())
            {
                _output.WriteLine("// Arrange input model heap");

                // write symbols arrange
                foreach (DependencyGroup dg in _heapGraph.GetDependencyGroups())
                {
                    // arrange each heap node group
                    WriteArrangeNodeGroup(dg);
                }

                _output.WriteLine(string.Empty);
            }
        }

        private string TypeNameOrAliasToVarName(string nameOrAlias)
        {
            return nameOrAlias.Replace('.', '_')
                .FirstCharToLower();
        }

        private void SetupSymbolContext(IReadOnlyList<DependencyGroup> groups)
        {
            Dictionary<TypeSig, int> classCounters = new Dictionary<TypeSig, int>(TypeEqualityComparer.Instance);
            Dictionary<TypeSig, int> arrayCounters = new Dictionary<TypeSig, int>(TypeEqualityComparer.Instance);

            foreach (IReadOnlyHeapNode node in groups.SelectMany(dg => dg))
            {
                string symbol = GetSymbol(node);

                _ = _context.CreateSymbolContext(symbol, node);

            }

            string GetSymbol(IReadOnlyHeapNode node)
            {
                if (node is IReadOnlyObjectHeapNode objNode)
                {
                    TypeSig type = objNode.Type;
                    int cnt = IncreaseCounter(type, classCounters);
                    return $"{TypeNameOrAliasToVarName(type.GetNameOrAlias())}{cnt}";
                }
                else if (node is IReadOnlyArrayHeapNode arrNode)
                {
                    TypeSig elementType = arrNode.ElementType;
                    int cnt = IncreaseCounter(elementType, arrayCounters);
                    return $"{TypeNameOrAliasToVarName(elementType.GetNameOrAlias())}Arr{cnt}";
                }

                throw new NotSupportedException("Unexpected heap node type.");
            }

            static int IncreaseCounter<T>(T key, IDictionary<T, int> counters)
            {
                if (!counters.TryGetValue(key, out int value))
                {
                    counters[key] = 0;
                }
                return ++counters[key];
            }
        }

        private void WriteArrangeNodeGroup(IReadOnlyCollection<IReadOnlyHeapNode> group)
        {
            // declare the variables & create the objects
            foreach (IReadOnlyHeapNode node in group)
            {
                string symbol = _context.GetSymbolContext(node.Location)!.Symbol;

                _template.WriteArrangeCreateInstance(_context, _output, symbol);
            }

            // set fields & elements & method results
            foreach (IReadOnlyHeapNode node in group)
            {
                string symbol = _context.GetSymbolContext(node.Location)!.Symbol;

                if (node is IReadOnlyObjectHeapNode objNode)
                {
                    if (objNode.HasFields)
                    {
                        foreach (var f in objNode.Fields)
                        {
                            string literal = _context.GetLiteral(objNode.GetFieldOrDefault(f))!;
                            _template.WriteArrangeInitializeField(_context, _output, symbol, f, literal);
                        }
                    }

                    if (objNode.HasMethodInvocations)
                    {

                        foreach((IMethod method, IValue[] results) in objNode.GetMethodResults())
                        {
                            MethodDef md = method.ResolveMethodDefThrow();

                            string[] literals = new string[results.Length];
                            for (int i = 0; i < literals.Length; ++i)
                            {
                                if (results[i] != null)
                                {
                                    literals[i] = _context.GetLiteral(results[i])!;
                                }
                                else
                                {
                                    literals[i] = _context.GetDefaultLiteral(md.ReturnType);
                                }
                            }

                            _template.WriteArrangeInitializeMethod(_context, _output, symbol, method, literals);
                        }
                    }
                }

                else if (node is IReadOnlyArrayHeapNode arrNode)
                {
                    if (arrNode.HasElements)
                    {
                        foreach (int index in arrNode.Indeces) 
                        {
                            string literal = _context.GetLiteral(arrNode.GetElementOrDefault(index))!;

                            _template.WriteArrangeInitializeArrayElement(_context, _output, symbol, index, literal);
                        }
                    }
                }
            }
        }

        public void WriteArrangeStaticFields()
        {
            if (_model.Variables.OfType<StaticFieldVariable>().Any())
            {
                _output.WriteLine("// Arrange static fields");
                
                IReadOnlyModel model = _model;
                foreach (StaticFieldVariable staticFieldVar in model.Variables.OfType<StaticFieldVariable>())
                {
                    string literal = _context.GetLiteral(model.GetValueOrDefault(staticFieldVar))!;

                    _template.WriteArrangeInitializeStaticField(_context, _output, staticFieldVar.Field, literal);
                }

                _output.WriteLine(string.Empty);
            }
        }

        public void WriteArrangeMethodArguments()
        {
            if (_context.Iteration.Exploration.MethodUnderTest.ResolveMethodDefThrow().Parameters.Count > 0)
            {
                _output.WriteLine("// Arrange method arguments");
            
                IReadOnlyModel model = _model;
                MethodDef md = _context.Iteration.Exploration.MethodUnderTest.ResolveMethodDefThrow();

                IList<Parameter> parameters = md.Parameters;

                foreach (Parameter p in parameters)
                {
                    if (p.IsHiddenThisParameter)
                    {
                        WriteArgument(p.Type, "@this", GetArgLiteral(p));
                    }
                    else if (p.IsNormalMethodParameter)
                    {
                        WriteArgument(p.Type, p.Name, GetArgLiteral(p));
                    }
                }

                string GetArgLiteral(Parameter p)
                {
                    MethodArgumentVariable methodArgumentVar = new MethodArgumentVariable(p);
                    string literal = _context.GetLiteral(model.GetValueOrDefault(methodArgumentVar))!;
                    return literal;
                }

                void WriteArgument(TypeSig t, string symbol, string literal)
                {
                    string type = t.GetNameOrAlias();
                    _output.WriteLine($"{type} {symbol} = {literal};");
                }

                _output.WriteLine(string.Empty);
            }
        }
    }
}
