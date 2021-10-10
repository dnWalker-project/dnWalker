using dnlib.DotNet.Emit;

using dnWalker.DataElements;
using dnWalker.NativePeers;
using dnWalker.Symbolic;
using dnWalker.Traversal;
using Echo.ControlFlow.Serialization.Dot;
using Echo.Platforms.Dnlib;
using MMC;
using MMC.Data;
using MMC.InstructionExec;
using MMC.State;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace dnWalker.Concolic
{
    public delegate void PathExploredHandler(dnWalker.Traversal.Path path);

    public class Explorer
    {
        private readonly Config _config;
        private readonly Logger _logger;
        private readonly ISolver _solver;
        private readonly DefinitionProvider _definitionProvider;

        public Explorer(DefinitionProvider definitionProvider, Config config, Logger logger, ISolver solver)
        {
            _config = config;
            _logger = logger;
            _solver = solver;
            _definitionProvider = definitionProvider;            
        }

        public event PathExploredHandler OnPathExplored;

        private int _iterationCount;
        public int IterationCount
        {
            get
            {
                return _iterationCount;
            }
        }

        public Traversal.PathStore PathStore { get; private set; }

        private static void WriteFlowGraph(dnlib.DotNet.MethodDef method, string filename = @"c:\temp\dot.dot")
        {
            using (TextWriter writer = File.CreateText(filename))
            {
                Echo.Core.Graphing.Serialization.Dot.DotWriter dotWriter = new Echo.Core.Graphing.Serialization.Dot.DotWriter(writer);
                dotWriter.SubGraphAdorner = new ExceptionHandlerAdorner<Instruction>();
                dotWriter.NodeAdorner = new ControlFlowNodeAdorner<Instruction>();
                dotWriter.EdgeAdorner = new ControlFlowEdgeAdorner<Instruction>();
                dotWriter.Write(method.ConstructStaticFlowGraph());
            }
        }

        public void Run(string methodName, params IArg[] arguments)
        {
            dnlib.DotNet.MethodDef entryPoint = _definitionProvider.GetMethodDefinition(methodName)
                ?? throw new NullReferenceException($"Method {methodName} not found");

            WriteFlowGraph(entryPoint);

            StateSpaceSetup stateSpaceSetup = new StateSpaceSetup(_definitionProvider, _config, _logger);

            Traversal.PathStore pathStore = new Traversal.PathStore(entryPoint);
            PathStore = pathStore;

            //IDataElement[] args = arguments?.Select(a => a.AsDataElement(_definitionProvider)).ToArray() ?? new IDataElement[] { };

            // setup iteration management
            int maxIterations = _config.MaxIterations;
            _iterationCount = 0;

            IDictionary<String, Object> next = null;

            while (true)
            {
                SystemConsole.OutTextWriterRef = ObjectReference.Null;

                try
                {
                    // check iteration
                    if (maxIterations > 0 && _iterationCount >= maxIterations)
                    {
                        throw new MaxIterationsExceededException(maxIterations);
                    }

                    ++_iterationCount;

                    List<ParameterExpression> parameters = new List<ParameterExpression>();
                    // initial state
                    // -------------
                    IInstructionExecProvider instructionExecProvider = InstructionExecProvider.Get(_config, new Symbolic.Instructions.InstructionFactory());
                    ExplicitActiveState cur = new ExplicitActiveState(_config, instructionExecProvider, _definitionProvider, _logger);
                    cur.PathStore = pathStore;

                    DataElementList dataElementList = cur.StorageFactory.CreateList(arguments.Length);

                    for (int i = 0; i < arguments.Length; i++)
                    {
                        //IDataElement arg = args[i];

                        // either next is null => use arguments array
                        // OR next is not null => use entryPoint.Parameters array

                        String argName = entryPoint.Parameters[i].Name;
                        IDataElement arg = null;

                        if (next != null && next.TryGetValue(argName, out Object value))
                        {
                            // we have an explicit desired value from the solver
                            arg = _definitionProvider.CreateDataElement(value);
                        }
                        else
                        {
                            // we do not have an explicit desired value from the solver => use the value provided by outside
                            arg = arguments[i].AsDataElement(_definitionProvider);
                        }

                        // we use ArrayOf whenever we are working with an array of items
                        // we add parameter in the parameter list for every element
                        if (arg is ArrayOf arrayOf)
                        {
                            Type elementType = arrayOf.GetType().GetElementType();

                            // first we construct an array and initialize its items
                            // then we add the expressions for each item <PARAM_NAME>[i]...
                            Int32 placement = cur.DynamicArea.DeterminePlacement(false);
                            ObjectReference arrayRef = cur.DynamicArea.AllocateArray(placement, arrayOf.ElementType, arrayOf.Length);
                            dataElementList[i] = arrayRef;

                            // add array.Length parameter
                            ParameterExpression arrayLengthParameterExpression = Expression.Parameter(typeof(int), argName + "->Length");

                            AllocatedArray allocatedArray = (AllocatedArray)cur.DynamicArea.Allocations[arrayRef];

                            for (int j = 0; j < arrayOf.Inner.Length; j++)
                            {
                                String elementName = argName + "[" + j + "]";

                                IDataElement item = _definitionProvider.CreateDataElement(arrayOf.Inner.GetValue(j));

                                allocatedArray.Fields[j] = item;

                                ParameterExpression itemParameterExpression = Expression.Parameter(elementType, elementName);
                                parameters.Add(itemParameterExpression);

                                item.SetExpression(itemParameterExpression, cur);
                            }
                        }

                        // we use InterfaceProxy to inject custom values whenever its methods are requested
                        else if (arg is InterfaceProxy interfaceProxy)
                        {
                            dataElementList[i] = interfaceProxy;

                            IDictionary<String, Func<IDataElement>> methodsResolvers = new Dictionary<String, Func<IDataElement>>();

                            // for each method create a parameter expression <PARAM_NAME>.<METHOD_NAME>
                            // TODO: handle, should the result of the method be something different than a number or a string
                            foreach(dnlib.DotNet.MethodDef method in interfaceProxy.WrappedType.Methods)
                            {
                                String returnName = argName + "->" + method.FullName;

                                Type returnType = GetBaseTypeFromName(method.ReturnType.FullName);
                                if (returnType == null) throw new Exception("Not supported return type, only numbers and strings are supported: " + method.ReturnType.FullName);

                                ParameterExpression methodResultParameterExpression = Expression.Parameter(returnType, returnName);
                                parameters.Add(methodResultParameterExpression);

                                if (next != null && next.TryGetValue(returnName, out Object desiredResult))
                                {
                                    // we have a desired value specified => use it
                                    methodsResolvers[method.FullName] = () =>
                                    {
                                        IDataElement dataElement = _definitionProvider.CreateDataElement(next[returnName]);
                                        dataElement.SetExpression(methodResultParameterExpression, cur);
                                        return dataElement;
                                    };
                                }
                                else
                                {
                                    // no value is specified => use default
                                    methodsResolvers[method.FullName] = () =>
                                    {
                                        IDataElement dataElement = GetDefaultDataElement(returnType, _definitionProvider);
                                        dataElement.SetExpression(methodResultParameterExpression, cur);
                                        return dataElement;
                                    };
                                }
                            }

                            interfaceProxy.SetMethodResolvers(cur, methodsResolvers);

                            // cur.PathStore.CurrentPath.SetObjectAttribute<IDictionary<String, Func<IDataElement>>>(arg, "method_results", results);
                        }
                        // TODO: add ClassProxy?
                        // we use ClassProxy to inject custom values whenever its methods or fields are requested
                        // else if (arg is ClassProxy classProxy) { }

                        // now it should be base types only
                        else
                        {
                            Type paramType = GetBaseTypeFromName(entryPoint.Parameters[i].Type.FullName);
                            if (paramType == null) throw new Exception("Not supported param type, only arrays, interfaces, numbers and strings are supported: " + entryPoint.Parameters[i].Type.FullName);

                            dataElementList[i] = arg;

                            ParameterExpression argParameterExpression = Expression.Parameter(paramType, argName);
                            parameters.Add(argParameterExpression);

                            dataElementList[i].SetExpression(argParameterExpression, cur);
                        }

                        //ParameterExpression parameter = Expression.Parameter(
                        //    paramType,
                        //    entryPoint.Parameters[i].Name);
                        //parameters.Add(parameter);

                        //dataElementList[i].SetExpression(parameter, cur);
                    }

                    //dataElementList = cur.StorageFactory.CreateSingleton(args[0]);
                    if (arguments.Length != entryPoint.Parameters.Count)
                    {
                        throw new InvalidOperationException("Invalid number of arguments provided to method " + entryPoint.Name);
                    }
                    

                    MethodState mainState = new MethodState(
                        entryPoint,
                        dataElementList,
                        cur);

                    // Initialize main thread.
                    cur.ThreadPool.CurrentThreadId = cur.ThreadPool.NewThread(cur, mainState, StateSpaceSetup.CreateMainThreadObject(cur, entryPoint, _logger));                    
                    // -------------

                    //var cur = stateSpaceSetup.CreateInitialState(entryPoint, args);
                    cur.CurrentThread.InstructionExecuted += pathStore.OnInstructionExecuted;

                    SimpleStatistics statistics = new SimpleStatistics();

                    MMC.Explorer explorer = new MMC.Explorer(cur, statistics, _logger, _config, PathStore);
                    explorer.InstructionExecuted += pathStore.OnInstructionExecuted;

                    explorer.Run();

                    dnWalker.Traversal.Path path = PathStore.CurrentPath;

                    System.Diagnostics.Debug.WriteLine($"Path explored {path.PathConstraintString}, input {string.Join(", ", dataElementList.Select(a => a.ToString()))}");

                    next = PathStore.GetNextInputValues(_solver, parameters);

                    OnPathExplored?.Invoke(path);

                    if (next == null)
                    {
                        break;
                    }

                    // no need to do this
                    //args = entryPoint.Parameters
                    //    .Select(p => new { p.Name, Value = next[p.Name] })
                    //    .Select(v => _definitionProvider.CreateDataElement(v.Value))
                    //    .ToArray();

                    PathStore.ResetPath();
                }
                catch (Exception e)
                {
                    _logger.Log(LogPriority.Fatal, e.Message);
                    throw;
                }
            }
        }

        //private static Type GetFrameworkType(ArrayOf arrayOf)
        //{
        //    return arrayOf.Inner.GetType();
        //}

        private static Type GetBaseTypeFromName(String fullName)
        {
            switch (fullName)
            {
                case "System.Int32": return typeof(Int32);
                case "System.UInt16": return typeof(UInt16);
                case "System.UInt32": return typeof(UInt32);
                case "System.UInt64": return typeof(UInt64);
                case "System.Int16": return typeof(Int16);
                case "System.Int64": return typeof(Int64);
                case "System.SByte": return typeof(SByte);
                case "System.Byte": return typeof(Byte);
                case "System.Boolean": return typeof(Boolean);
                case "System.Char": return typeof(Char);
                case "System.Double": return typeof(Double);
                case "System.Decimal": return typeof(Decimal);
                case "System.Single": return typeof(Single);
                case "System.IntPtr": return typeof(IntPtr);
                case "System.UIntPtr": return typeof(UIntPtr);


                default:
                    return null;
                    // throw new ArgumentException("Unexpected type: " + fullName);
            }
        }

        private static IDataElement GetDefaultDataElement(Type type, DefinitionProvider definitionProvider)
        {
            Object defaultValue = type.IsValueType ? Activator.CreateInstance(type) : null;
            return definitionProvider.CreateDataElement(defaultValue);
        }
    }
}
