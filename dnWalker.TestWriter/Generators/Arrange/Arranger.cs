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
        private readonly IReadOnlyList<IArrangePrimitives> _arrangeWriters;
        private readonly ITestContext _context;
        private readonly IWriter _output;
        private readonly IReadOnlyModel _model;
        
        public Arranger(IReadOnlyList<IArrangePrimitives> arrangeWriters, ITestContext context, IWriter output)
        {
            _arrangeWriters = arrangeWriters;
            _context = context;
            _output = output;
            _model = context.Iteration.InputModel;
        }

        private void ArrangeOrThrow(Func<IArrangePrimitives, bool> arrangeAction, string? message = null)
        {
            if (!_arrangeWriters.Any(a => arrangeAction(a))) 
            {
                throw new InvalidOperationException(message);
            }
        }

        private void WriteArrangeHeap()
        {
            HeapGraph graph = _model.HeapInfo.CreateGraph();
            IReadOnlyList<DependencyGroup> dependencyGroups = graph.GetDependencyGroups();

            // generate the symbol contexts
            SetupSymbolContext(dependencyGroups);

            // write symbols arrange
            foreach (DependencyGroup dg in dependencyGroups)
            {
                // arrange each heap node group
                WriteArrangeNodeGroup(dg);
            }
        }

        private void SetupSymbolContext(IReadOnlyList<DependencyGroup> groups)
        {
            Dictionary<TypeSig, int> _classCounters = new Dictionary<TypeSig, int>();
            Dictionary<TypeSig, int> _arrayCounters = new Dictionary<TypeSig, int>();

            foreach (IReadOnlyHeapNode node in groups.SelectMany(dg => dg))
            {
                string symbol = GetSymbol(node);

                _ = _context.CreateSymbolContext(symbol, node);

            }

            string GetSymbol(IReadOnlyHeapNode node)
            {
                if (node is IReadOnlyHeapNode objNode)
                {
                    TypeSig type = objNode.Type;
                    int cnt = _classCounters[type]++;
                    return $"{type.GetNameOrAlias()}{cnt}";
                }
                else if (node is IReadOnlyArrayHeapNode arrNode)
                {
                    TypeSig elementType = arrNode.ElementType;
                    int cnt = _arrayCounters[elementType]++;
                    return $"{elementType.GetNameOrAlias()}Arr{cnt}";
                }

                throw new NotSupportedException("Unexpected heap node type.");
            }
        }

        private void WriteArrangeNodeGroup(IReadOnlyCollection<IReadOnlyHeapNode> group)
        {
            // declare the variables & create the objects
            foreach (IReadOnlyHeapNode node in group)
            {
                string symbol = _context.GetSymbolContext(node.Location)!.Symbol;

                ArrangeOrThrow(a => a.TryWriteArrangeCreateInstance(_context, _output, symbol), $"Failed to create instance for: '{symbol}'");
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
                            ArrangeOrThrow(a => a.TryWriteArrangeInitializeField(_context, _output, symbol, f, literal), $"Failed to arrange field '{f}' for '{symbol}'");
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

                            ArrangeOrThrow(a => a.TryWriteArrangeInitializeMethod(_context, _output, symbol, method, literals), $"Failed to arrange method '{md}' for '{symbol}'");
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

                            ArrangeOrThrow(a => a.TryWriteArrangeInitializeArrayElement(_context, _output, symbol, index, literal), $"Failed to arrange element '{index}' for '{symbol}'");
                        }
                    }
                }
            }
        }

        private void ArrangeStaticFields()
        {
            IReadOnlyModel model = _model;
            foreach (StaticFieldVariable staticFieldVar in model.Variables.OfType<StaticFieldVariable>())
            {
                string literal = _context.GetLiteral(model.GetValueOrDefault(staticFieldVar))!;

                ArrangeOrThrow(a => a.TryWriteArrangeInitializeStaticField(_context, _output, staticFieldVar.Field, literal));
            }
        }

        private void ArrangeMethodArguments()
        {
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
        }

        internal void Run()
        {
            IReadOnlyHeapInfo heap = _model.HeapInfo;
            if (!heap.IsEmpty())
            {
                _output.WriteLine("// Arrange input model heap");
                WriteArrangeHeap();
                _output.WriteLine(string.Empty);
            }
            if (_model.Variables.OfType<StaticFieldVariable>().Any())
            {
                _output.WriteLine("// Arrange static fields");
                ArrangeStaticFields();
                _output.WriteLine(string.Empty);
            }
            if (_context.Iteration.Exploration.MethodUnderTest.ResolveMethodDefThrow().Parameters.Count > 0)
            {
                _output.WriteLine("// Arrange method arguments");
                ArrangeMethodArguments();
                _output.WriteLine(string.Empty);
            }
        }
    }
}
