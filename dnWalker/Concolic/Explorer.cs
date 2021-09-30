using dnlib.DotNet.Emit;
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

        public Traversal.PathStore PathStore { get; private set; }

        public void Run(string methodName, params IArg[] arguments)
        {
            var entryPoint = _definitionProvider.GetMethodDefinition(methodName)
                ?? throw new NullReferenceException($"Method {methodName} not found");

            var stateSpaceSetup = new StateSpaceSetup(_definitionProvider, _config, _logger);
            
            var pathStore = new Traversal.PathStore(entryPoint);
            PathStore = pathStore;            

            using (TextWriter writer = File.CreateText(@"c:\temp\dot.dot"))
            {
                var dotWriter = new Echo.Core.Graphing.Serialization.Dot.DotWriter(writer);
                dotWriter.SubGraphAdorner = new ExceptionHandlerAdorner<Instruction>();
                dotWriter.NodeAdorner = new ControlFlowNodeAdorner<Instruction>();
                dotWriter.EdgeAdorner = new ControlFlowEdgeAdorner<Instruction>();
                dotWriter.Write(entryPoint.ConstructStaticFlowGraph());
            }

            var args = arguments?.Select(a => a.AsDataElement(_definitionProvider)).ToArray() ?? new IDataElement[] { };

            // setup iteration management
            int maxIterations = _config.MaxIterations;
            int curIteration = 0;

            while (true)
            {
                SystemConsole.OutTextWriterRef = ObjectReference.Null;

                try
                {
                    // check iteration
                    if (maxIterations <= 0 || curIteration < maxIterations)
                    {
                        throw new Exception("Execution ran out of iterations without finishing " + maxIterations);
                    }

                    ++curIteration;

                    var parameters = new List<ParameterExpression>();
                    // initial state
                    // -------------
                    var instructionExecProvider = InstructionExecProvider.Get(_config, new Symbolic.Instructions.InstructionFactory());
                    var cur = new ExplicitActiveState(_config, instructionExecProvider, _definitionProvider, _logger);
                    cur.PathStore = pathStore;

                    DataElementList dataElementList;

                    /*foreach (var allocation in args.OfType<IReferenceType>())
                    {
                        //if (cur.
                    }*/

                    if (entryPoint.Parameters.Count == 1 && entryPoint.Parameters[0].Type.FullName == "System.String[]") // TODO
                    {
                        throw new NotImplementedException("Obsolete");/*
                        ObjectReference runArgsRef = cur.DynamicArea.AllocateArray(
                            cur.DynamicArea.DeterminePlacement(false),
                            cur.DefinitionProvider.GetTypeDefinition("System.String"),
                            arguments.Length);

                        if (_config.RunTimeParameters.Length > 0)
                        {
                            AllocatedArray runArgs = (AllocatedArray)cur.DynamicArea.Allocations[runArgsRef];
                            for (int i = 0; i < _config.RunTimeParameters.Length; ++i)
                            {
                                runArgs.Fields[i] = args[i];
                            }
                        }

                        dataElementList = cur.StorageFactory.CreateSingleton(runArgsRef);*/
                    }
                    else
                    {
                        dataElementList = cur.StorageFactory.CreateList(arguments.Length);
                        for (int i = 0; i < arguments.Length; i++)
                        {
                            var arg = args[i];
                            dataElementList[i] = arg;

                            Type paramType = typeof(int);

                            if (arg is ArrayOf arrayOf)
                            {
                                var placement = cur.DynamicArea.DeterminePlacement(false);
                                ObjectReference arrayRef = cur.DynamicArea.AllocateArray(
                                    placement,
                                    arrayOf.ElementType,
                                    arrayOf.Length);

                                AllocatedArray allocatedArray = (AllocatedArray)cur.DynamicArea.Allocations[arrayRef];

                                for (int j = 0; j < arrayOf.Inner.Length; j++)
                                {
                                    allocatedArray.Fields[j] = _definitionProvider.CreateDataElement(arrayOf.Inner.GetValue(j));
                                }

                                dataElementList[i] = arrayRef;
                                // todo
                                paramType = arrayOf.Inner.GetType();
                                //dataElementList = cur.StorageFactory.CreateSingleton(arrayRef);
                            }
                            else
                            {
                                // TODO
                                switch (entryPoint.Parameters[i].Type.FullName)
                                {
                                    case "System.Double":
                                        paramType = typeof(double);
                                        break;
                                }
                            }

                            var parameter = Expression.Parameter(
                                paramType,
                                entryPoint.Parameters[i].Name);
                            parameters.Add(parameter);

                            dataElementList[i].SetExpression(parameter, cur);
                        }

                        //dataElementList = cur.StorageFactory.CreateSingleton(args[0]);
                        if (arguments.Length != entryPoint.Parameters.Count)
                        {
                            throw new InvalidOperationException("Invalid number of arguments provided to method " + entryPoint.Name);
                        }
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

                    var statistics = new SimpleStatistics();

                    var explorer = new MMC.Explorer(cur, statistics, _logger, _config, PathStore);
                    explorer.InstructionExecuted += pathStore.OnInstructionExecuted;

                    explorer.Run();

                    var path = PathStore.CurrentPath;

                    System.Diagnostics.Debug.WriteLine($"Path explored {path.PathConstraintString}, input {string.Join(", ", args.Select(a => a.ToString()))}");

                    var next = PathStore.GetNextInputValues(_solver, parameters);

                    OnPathExplored?.Invoke(path);

                    if (next == null)
                    {
                        break;
                    }

                    args = entryPoint.Parameters.Select(p => new { p.Name, Value = next[p.Name] })
                        .Select(v =>
                        _definitionProvider.CreateDataElement(v.Value))
                        .ToArray();

                    PathStore.ResetPath();
                }
                catch (Exception e)
                {
                    _logger.Log(LogPriority.Fatal, e.Message);
                    throw;
                }
            }
        }
    }
}
