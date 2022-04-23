using dnWalker.DataElements;
using dnWalker.TypeSystem;

using FluentAssertions;
using MMC;
using MMC.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

using Xunit.Abstractions;

namespace dnWalker.Tests
{
    public abstract class TestBase : IDisposable
    {
        //private class Converter : TextWriter
        //{
        //    private readonly ITestOutputHelper _output;

        //    public Converter(ITestOutputHelper output)
        //    {
        //        _output = output;
        //    }

        //    public override Encoding Encoding
        //    {
        //        get { return Encoding.UTF8; }
        //    }

        //    public override void WriteLine(string message)
        //    {
        //        _output.WriteLine(message);
        //    }

        //    public override void WriteLine(string format, params object[] args)
        //    {
        //        _output.WriteLine(format, args);
        //    }
        //}

        public ITestOutputHelper Output { get; }

        public IDefinitionProvider DefinitionProvider { get; }
        public Logger Logger { get; }

        //private TextWriter _originalConsoleOut;
        protected TestBase(ITestOutputHelper testOutputHelper, IDefinitionProvider definitionProvider)
        {
            //var converter = new Converter(testOutputHelper);
            //_originalConsoleOut = Console.Out;
            //Console.SetOut(converter);

            Logger = new Logger(Logger.Default | LogPriority.Trace);
            Logger.AddOutput(new TestLoggerOutput());
            Output = testOutputHelper;
            DefinitionProvider = definitionProvider;
        }

        public void Dispose()
        {
            //Console.SetOut(_originalConsoleOut);
        }

        //public static AssemblyLoader GetAssemblyLoader(string assemblyFilename)
        //{
        //    var assemblyLoader = new AssemblyLoader();

        //    var data = File.ReadAllBytes(assemblyFilename);

        //    var moduleDef = assemblyLoader.GetModuleDef(data);

        //    Assembly.LoadFrom(assemblyFilename);

        //    return assemblyLoader;
        //}

        public static IDomain GetDefinitionContext(string assemblyFileName)
        {
            IDomain definitionContext = Domain.LoadFromFile(assemblyFileName);

            Assembly.LoadFrom(assemblyFileName);

            return definitionContext;
        }

        private static readonly ISolver _solver = new Z3.Solver2();


        private readonly List<Action<IConcolicExplorerBuilder>> _initializeConcolicExplorerBuilder = new List<Action<IConcolicExplorerBuilder>>();
        private readonly List<Action<IModelCheckerExplorerBuilder>> _initializeModelCheckerExplorerBuilder = new List<Action<IModelCheckerExplorerBuilder>>();

        public void OverrideConcolicExplorerBuilderInitialization(Action<IConcolicExplorerBuilder> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _initializeConcolicExplorerBuilder.Add(action);
        }
        public void OverrideModelCheckerExplorerBuilderInitialization(Action<IModelCheckerExplorerBuilder> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _initializeModelCheckerExplorerBuilder.Add(action);
        }


        protected IModelCheckerExplorerBuilder GetModelCheckerBuilder(string methodName = null)
        {
            ModelCheckerExplorerBuilder builder = new ModelCheckerExplorerBuilder(() => Logger, () => DefinitionProvider, () => new SimpleStatistics(), methodName);

            _initializeModelCheckerExplorerBuilder.ForEach(a => a(builder));

            return builder;
        }

        protected IConcolicExplorerBuilder GetConcolicExplorerBuilder([CallerMemberName] string testMethod = null)
        {
            ConcolicExplorerBuilder builder = new ConcolicExplorerBuilder(() => Logger, () => DefinitionProvider, static () => _solver);

            _initializeConcolicExplorerBuilder.ForEach(a => a(builder));

            builder.ExportXmlData(testMethod);

            return builder;
        }

    }
}
