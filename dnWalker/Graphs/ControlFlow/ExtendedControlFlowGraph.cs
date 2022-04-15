using dnlib.DotNet;
using dnlib.DotNet.Emit;

using Echo.ControlFlow;
using Echo.Platforms.Dnlib;

using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using QuikGraph;
using dnWalker.Concolic;

namespace dnWalker.Graphs.ControlFlow
{
    public abstract class CFGNode
    {
        public int Executions { get; set; }
    }

    public class InstructionCFGNode : CFGNode
    {
        public InstructionCFGNode(Instruction instruction)
        {
            Instruction = instruction ?? throw new ArgumentNullException(nameof(instruction));
        }

        public Instruction Instruction { get; }
    }

    public class ExceptionCFGNode : CFGNode
    {
        public ExceptionCFGNode(TypeDef exceptionType)
        {
            ExceptionType = exceptionType ?? throw new ArgumentNullException(nameof(exceptionType));
        }

        public TypeDef ExceptionType { get; }
    }

    public abstract class CFGEdge : QuikGraph.Edge<CFGNode>
    {
        protected CFGEdge([NotNull] CFGNode source, [NotNull] CFGNode target) : base(source, target)
        {
        }


        public int Executions { get; set; }
    }

    public class ConditionalCFGEdge : CFGEdge
    {
        public ConditionalCFGEdge([NotNull] CFGNode source, [NotNull] CFGNode target) : base(source, target)
        {
        }
    }

    public class UnconditionalCFGEdge : CFGEdge
    {
        public UnconditionalCFGEdge([NotNull] CFGNode source, [NotNull] CFGNode target) : base(source, target)
        {
        }
    }

    public class ExtendedControlFlowGraph
    {
        // exception names throwable by CIL
        private const string _arithmetic = "System.ArithmeticException";
        private const string _divideByZero = "System.DivideByZeroException";
        private const string _invalidAddress = "System.InvalidAddressException";
        private const string _overflow = "System.OverflowException";
        private const string _security = "System.SecurityException";
        private const string _stackOverflow = "System.StackOverflowException";

        private const string _tryLoad = "System.TryLoadException";
        private const string _indexOutOfRange = "System.IndexOutOfRangeException";
        private const string _invalidCast = "System.InvalidCastException";
        private const string _missingField = "System.MissingFieldException";
        private const string _missingMethod = "System.MissingMethodException";
        private const string _nullReference = "System.NullReferenceException";
        private const string _outOfMemory = "System.OutOfMemoryException";


        private static readonly TypeDef[] _noExceptions = Array.Empty<TypeDef>();


        private static TypeDef[] GetExceptions(Instruction instruction, IDefinitionProvider definitionProvider)
        {
            switch (instruction.OpCode.Code)
            {
                case Code.Add_Ovf:
                case Code.Add_Ovf_Un:
                    return new TypeDef[] { definitionProvider.GetTypeDefinition(_arithmetic) };

                case Code.Call:
                case Code.Callvirt:
                    {
                        // may invoke SecurityException      - way too complex => ignore
                        // may invoke MethodAccessException  - we expect well formed code => it should not happen => ignore
                        // may invoke MissingMethodException - we expect well formed code => it should not happen => ignore

                        if (instruction.Operand is IMethod m)
                        {
                            MethodDef md = m.ResolveMethodDefThrow();
                            if (md.IsStatic)
                            {
                                // static method => may throw nullReference
                                return new TypeDef[] { definitionProvider.GetTypeDefinition(_nullReference) };
                            }
                        }

                        return _noExceptions;
                    }

                case Code.Ckfinite:
                    return new TypeDef[] { definitionProvider.GetTypeDefinition(_arithmetic) }; // if the value is NaN or Infinity

                case Code.Conv_Ovf_I:
                case Code.Conv_Ovf_I1:
                case Code.Conv_Ovf_I2:
                case Code.Conv_Ovf_I4:
                case Code.Conv_Ovf_I8:
                case Code.Conv_Ovf_U:
                case Code.Conv_Ovf_U1:
                case Code.Conv_Ovf_U2:
                case Code.Conv_Ovf_U4:
                case Code.Conv_Ovf_U8:

                case Code.Conv_Ovf_I_Un:
                case Code.Conv_Ovf_I1_Un:
                case Code.Conv_Ovf_I2_Un:
                case Code.Conv_Ovf_I4_Un:
                case Code.Conv_Ovf_I8_Un:
                case Code.Conv_Ovf_U_Un:
                case Code.Conv_Ovf_U1_Un:
                case Code.Conv_Ovf_U2_Un:
                case Code.Conv_Ovf_U4_Un:
                case Code.Conv_Ovf_U8_Un:
                    return new TypeDef[] { definitionProvider.GetTypeDefinition(_overflow) };

                //case Code.Cpblk:
                //case Code.Initblk:
                //    return new TypeDef[] { definitionProvider.GetTypeDefinition(_nullReference) }; // if an invalid address is detected... => ignore?

                case Code.Rem:
                case Code.Rem_Un:
                case Code.Div:
                    return new TypeDef[] 
                    {
                        definitionProvider.GetTypeDefinition(_arithmetic), // if the result cannot be represented in the result type (e.g. value1 is the smallest integer, value2 is -1)
                        definitionProvider.GetTypeDefinition(_divideByZero) // if the value is zero, only for integer operands !!!
                    };
                case Code.Div_Un:
                    return new TypeDef[] { definitionProvider.GetTypeDefinition(_divideByZero) };

                case Code.Sub_Ovf:
                case Code.Mul_Ovf:
                case Code.Mul_Ovf_Un:
                    return new TypeDef[] { definitionProvider.GetTypeDefinition(_overflow) };

                case Code.Castclass:
                    return new TypeDef[] { definitionProvider.GetTypeDefinition(_invalidCast) };

                case Code.Ldelem:
                case Code.Ldelem_I:
                case Code.Ldelem_I1:
                case Code.Ldelem_I2:
                case Code.Ldelem_I4:
                case Code.Ldelem_I8:
                case Code.Ldelem_U1:
                case Code.Ldelem_U2:
                case Code.Ldelem_U4:
                case Code.Ldelem_R4:
                case Code.Ldelem_R8:
                case Code.Ldelem_Ref:
                case Code.Ldelema:

                case Code.Stelem:
                case Code.Stelem_I:
                case Code.Stelem_I1:
                case Code.Stelem_I2:
                case Code.Stelem_I4:
                case Code.Stelem_I8:
                case Code.Stelem_R4:
                case Code.Stelem_R8:
                case Code.Stelem_Ref:
                    return new TypeDef[] 
                    { 
                        definitionProvider.GetTypeDefinition(_nullReference), 
                        definitionProvider.GetTypeDefinition(_indexOutOfRange)
                    };

                case Code.Newarr:
                    return new TypeDef[] { definitionProvider.GetTypeDefinition(_overflow) }; // in case numElems < 0

                case Code.Ldvirtftn:
                case Code.Ldlen:
                case Code.Ldfld:
                case Code.Ldflda:
                case Code.Stfld:
                    return new TypeDef[] { definitionProvider.GetTypeDefinition(_nullReference) };

                default: 
                    return _noExceptions;
            }
        }

        /// <summary>
        /// Tries to resolve the handler should the specified exception be thrown by specified instruction.
        /// If no such handler exists within the provided method, returns null.
        /// </summary>
        /// <param name="srcInstruction"></param>
        /// <param name="exception"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private static Instruction GetHandler(Instruction instruction, TypeDef exception, MethodDef method, IDefinitionProvider definitionProvider)
        {
            if (!method.Body.HasExceptionHandlers)
            { 
                // triv case
                return null;
            }

            uint offset = instruction.Offset;
            IList<ExceptionHandler> handlers = method.Body.ExceptionHandlers;

            ExceptionHandler theHandler = null;

            foreach (ExceptionHandler handler in handlers)
            {
                // check handler type & whether catches this type
                if ((handler.HandlerType == ExceptionHandlerType.Filter ||
                     handler.HandlerType == ExceptionHandlerType.Catch && definitionProvider.IsSubtype(exception, handler.CatchType)))
                {
                    // check scope
                    if (theHandler == null ||
                        theHandler.TryStart.Offset < handler.TryStart.Offset ||
                        theHandler.TryEnd.Offset > handler.TryEnd.Offset)
                    {
                        theHandler = handler;
                    }
                }
            }

            return theHandler?.HandlerStart;
        }

        public static ExtendedControlFlowGraph Build(MethodDef method, IDefinitionProvider definitionProvider)
        {
            Dictionary<long, Instruction> instructions = method
                .Body
                .Instructions
                .ToDictionary(static instr => (long)instr.Offset);

            Dictionary<long, InstructionCFGNode> cfgNodeLookup = instructions
                .ToDictionary(static p => p.Key, static p => new InstructionCFGNode(p.Value));

            Dictionary<TypeDef, ExceptionCFGNode> exceptionCFGNodeLookup = new Dictionary<TypeDef, ExceptionCFGNode>();

            ControlFlowGraph<Instruction> baseCFG = method.ConstructStaticFlowGraph();

            QuikGraph.AdjacencyGraph<CFGNode, CFGEdge> graph = new QuikGraph.AdjacencyGraph<CFGNode, CFGEdge>();
            graph.AddVertexRange(cfgNodeLookup.Values);


            foreach (ControlFlowNode<Instruction> cfgNode in baseCFG.Nodes)
            {
                InstructionCFGNode finalNode = cfgNodeLookup[cfgNode.Offset];

                // setup conventional edges
                foreach (ControlFlowEdge<Instruction> outEdge in cfgNode.GetOutgoingEdges())
                {
                    ControlFlowNode<Instruction> trgNode = outEdge.Target;
                    InstructionCFGNode finalTrgNode = cfgNodeLookup[trgNode.Offset];

                    graph.AddEdge(BuildEdge(finalNode, finalTrgNode, outEdge.Type));
                }

                // setup exceptions edges
                Instruction instruction = instructions[cfgNode.Offset];
                foreach (TypeDef exception in GetExceptions(instruction, definitionProvider))
                {
                    Instruction excHandler = GetHandler(instruction, exception, method, definitionProvider);
                    if (excHandler != null)
                    {
                        graph.AddEdge(BuildEdge(finalNode, cfgNodeLookup[excHandler.Offset], ControlFlowEdgeType.Conditional));
                    }
                    else
                    {
                        // no handler was found
                        // => ensure the virtual exception handler has its node & create the edge
                        if (!exceptionCFGNodeLookup.TryGetValue(exception, out ExceptionCFGNode virtualExceptionHandler))
                        {
                            virtualExceptionHandler = new ExceptionCFGNode(exception);
                            exceptionCFGNodeLookup.Add(exception, virtualExceptionHandler);
                            graph.AddVertex(virtualExceptionHandler);
                        }

                        graph.AddEdge(BuildEdge(finalNode, virtualExceptionHandler, ControlFlowEdgeType.Conditional));
                    }
                }
            }




            return new ExtendedControlFlowGraph(graph);


            static CFGEdge BuildEdge(CFGNode origin, CFGNode target, ControlFlowEdgeType edgeType)
            {
                switch (edgeType)
                {
                    case ControlFlowEdgeType.None:
                    case ControlFlowEdgeType.FallThrough:
                    case ControlFlowEdgeType.Unconditional:
                    case ControlFlowEdgeType.Abnormal:
                    default:
                        return new UnconditionalCFGEdge(origin, target);

                    case ControlFlowEdgeType.Conditional:
                        return new ConditionalCFGEdge(origin, target);
                }
            }
        }

        private AdjacencyGraph<CFGNode, CFGEdge> _graph;

        private ExtendedControlFlowGraph(AdjacencyGraph<CFGNode, CFGEdge> graph)
        {
            _graph = graph;
        }

        public Coverage GetCoverage()
        {
            int nodeCount = _graph.VertexCount;
            int edgeCount = _graph.EdgeCount;

            int coveredNodes = _graph.Vertices.Count(static v => v.Executions > 0);
            int coveredEdges = _graph.Edges.Count(static e => e.Executions > 0);

            return new Coverage((double)coveredNodes / nodeCount, (double)coveredEdges / edgeCount);
        }
    }
}
