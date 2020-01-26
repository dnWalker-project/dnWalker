using FluentAssertions;
using MMC;
using MMC.Data;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace dnWalker.Tests
{
    public abstract class TestBase
    {
        private readonly AssemblyLoader _assemblyLoader;
        private readonly Config _config;
        private readonly DefinitionProvider _definitionProvider;

        protected TestBase(string assemblyFilename)
        {
            _assemblyLoader = new AssemblyLoader();
            _config = new Config();
            var data = File.ReadAllBytes(assemblyFilename);
            var moduleDef = _assemblyLoader.GetModuleDef(data);
            _logger = new Logger(Logger.Default | LogPriority.Trace);
            _logger.AddOutput(new TestLoggerOutput());
            //_appDomain = _assemblyLoader.CreateAppDomain(moduleDef, data);
            Assembly.LoadFrom(assemblyFilename);
            _definitionProvider = DefinitionProvider.Create(_assemblyLoader, _logger);
        }

        public Logger _logger { get; }

        protected virtual object Test(string methodName, params object[] args)
        {
            var stateSpaceSetup = new StateSpaceSetup(_definitionProvider, _config, _logger);

            var entryPoint = _definitionProvider.GetMethodDefinition(methodName)
                ?? throw new NullReferenceException("Method not found");

            var state = stateSpaceSetup.CreateInitialState(
                entryPoint,
                args.Select(a => _definitionProvider.CreateDataElement(a)).ToArray());

            var statistics = new SimpleStatistics();

            var explorer = new Explorer(state, statistics, _logger, _config);
            explorer.Run();

            var ex = explorer.GetUnhandledException();
            if (ex != null)
            {
                throw ex;
            }

            return state.CurrentThread.RetValue;
        }

        protected virtual void TestAndCompare(string methodName, params object[] args)
        {
            object res1 = null, res2 = null;
            Exception ex1 = null, ex2 = null;
            try
            {
                res1 = Test(methodName, args);
            }
            catch (Exception ex)
            {
                ex1 = ex;
                res1 = null;
            }

            var methodInfo = Utils.GetMethodInfo(methodName);
            try
            {
                res2 = methodInfo.Invoke(null, args);
            }
            catch (TargetInvocationException tie)
            {
                ex2 = tie.InnerException;
                res2 = null;
            }
            catch (Exception ex)
            {
                ex2 = ex;
                res2 = null;
            }

            if (ex1 != null && ex2 == null)
            {
                System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex1).Throw();
            }

            if (methodInfo.ReturnType != typeof(void))
            {
                if (res1 == null && res2 == null)
                {
                    return;
                }

                res1.Should().NotBeNull("value returned from model checker shoud not be null, but " + res2.ToString());
                res2.Should().NotBeNull();

                res1.Should().BeAssignableTo<IConvertible>();
                res2.Should().BeAssignableTo<IConvertible>();

                var r1 = Convert.ChangeType(res1, methodInfo.ReturnType);
                var r2 = Convert.ChangeType(res2, methodInfo.ReturnType);

                r1.Should().BeEquivalentTo(r2);
            }
            /*if (!(ex1 is null) || !(ex2 is null))
				Verify(ex1?.GetType().FullName == ex2?.GetType().FullName);
			else
				Verify(m1.ReturnType, res1, res2);*/
        }
    }
}
