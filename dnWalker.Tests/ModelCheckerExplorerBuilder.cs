using dnlib.DotNet;

using dnWalker.Concolic.Traversal;
using dnWalker.Instructions;
using dnWalker.Instructions.Extensions;
using dnWalker.Traversal;
using dnWalker.TypeSystem;

using MMC;
using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests
{
    public class ModelCheckerExplorerBuilder : ConfigBuilder<IModelCheckerExplorerBuilder>, IModelCheckerExplorerBuilder
    {
        private Func<Logger> _provideLogger;
        private Func<IDefinitionProvider> _provideDefinitionProvider;
        private Func<IStatistics> _provideStatistics;

        private string _methodName;
        private Func<ExplicitActiveState, IDataElement[]> _provideArgs = (cur) => Array.Empty<IDataElement>();

        private readonly List<IInstructionExecutor> _executors = new List<IInstructionExecutor>();
        private readonly List<(Type, object)> _services = new List<(Type, object)>();

        public ModelCheckerExplorerBuilder(Func<Logger> provideLogger, Func<IDefinitionProvider> provideDefinitionProvider, Func<IStatistics> provideStatistics, string methodName = null)
        {
            _provideLogger = provideLogger ?? throw new ArgumentNullException(nameof(provideLogger));
            _provideDefinitionProvider = provideDefinitionProvider ?? throw new ArgumentNullException(nameof(provideDefinitionProvider));
            _provideStatistics = provideStatistics ?? throw new ArgumentNullException(nameof(provideStatistics));

            _methodName = methodName;
        }

        public IModelCheckerExplorerBuilder OverrideLogger(Func<Logger> provideLogger)
        {
            _provideLogger = provideLogger ?? throw new ArgumentNullException(nameof(provideLogger));
            return this;
        }

        public IModelCheckerExplorerBuilder OverrideDefinitionProvider(Func<IDefinitionProvider> provideDefinitionProvider)
        {
            _provideDefinitionProvider = provideDefinitionProvider ?? throw new ArgumentNullException(nameof(_provideDefinitionProvider));
            return this;
        }

        public IModelCheckerExplorerBuilder OverrideStatistics(Func<IStatistics> provideStatistics)
        {
            _provideStatistics = provideStatistics ?? throw new ArgumentNullException(nameof(provideStatistics));
            return this;
        }

        public IModelCheckerExplorerBuilder AddService<TService>(TService service)
        {
            _services.Add((typeof(TService), service));
            return this;
        }

        public Explorer Build()
        {
            if (_methodName == null)
            {
                throw new InvalidOperationException("Cannot initialize the ModelChecker without method name!");
            }

            IDefinitionProvider definitionProvider = _provideDefinitionProvider();

            // curse of static data
            AllocatedDelegate.DelegateTypeDef = definitionProvider.BaseTypes.Delegate.ToTypeDefOrRef();

            Logger logger = _provideLogger();

            //StateSpaceSetup stateSpaaceSetup = new StateSpaceSetup(definitionProvider, Config, logger);
            MethodDef entryPoint = definitionProvider.GetMethodDefinition(_methodName) ?? throw new NullReferenceException($"Method {_methodName} not found");

            // a bit dirty...
            PathStore pathStore = new ConcolicPathStore(entryPoint);

            //ExplicitActiveState cur = stateSpaaceSetup.CreateInitialState(entryPoint, _provideArgs ?? throw new NullReferenceException("Args is null!"));

            var f = new dnWalker.Instructions.ExtendableInstructionFactory().AddStandardExtensions();
            IInstructionExecProvider instructionExecProvider = InstructionExecProvider.Get(Config, f);

            ExplicitActiveState cur = new ExplicitActiveState(Config, instructionExecProvider, definitionProvider, logger);
            cur.PathStore = pathStore;
            foreach ((Type type, object service) in _services)
            {
                cur.Services.RegisterService(type, type.FullName!, service);
            }

            IDataElement[] argsArray = _provideArgs(cur);
            DataElementList arguments = cur.StorageFactory.CreateList(argsArray.Length);
            for(int i = 0; i < argsArray.Length; i++) arguments[i] = argsArray[i];

            MethodState mainState = new MethodState(entryPoint, arguments, cur);

            cur.ThreadPool.CurrentThreadId = cur.ThreadPool.NewThread(cur, mainState, StateSpaceSetup.CreateMainThreadObject(cur, entryPoint, logger));
            

            IStatistics statistics = _provideStatistics();
            Explorer explorer = new Explorer(cur, statistics, logger, Config, pathStore);
            //explorer.Run();
            return explorer;
        }

        protected override IModelCheckerExplorerBuilder GetOutterBuilder()
        {
            return this;
        }

        public IModelCheckerExplorerBuilder SetMethod(string methodName)
        {
            _methodName = methodName;
            return this;
        }

        public IModelCheckerExplorerBuilder SetArgs(IDataElement[] args)
        {
            _provideArgs = state => args;
            return this;
        }

        public IModelCheckerExplorerBuilder SetArgs(Func<ExplicitActiveState, IDataElement[]> args)
        {
            _provideArgs = args;
            return this;
        }

        public IModelCheckerExplorerBuilder AddInstructionExecutor(IInstructionExecutor executor)
        {
            _executors.Add(executor);

            return this;
        }
    }
}
