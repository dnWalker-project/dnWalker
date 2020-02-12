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
        protected readonly Config _config;
        private readonly DefinitionProvider _definitionProvider;

        protected TestBase(DefinitionProvider definitionProvider)
        {
            _config = new Config();
            _logger = new Logger(Logger.Default | LogPriority.Trace);
            _logger.AddOutput(new TestLoggerOutput());
            _definitionProvider = definitionProvider;
        }

        protected static AssemblyLoader GetAssemblyLoader(string assemblyFilename)
        {
            var assemblyLoader = new AssemblyLoader();

            var data = File.ReadAllBytes(assemblyFilename);

            var moduleDef = assemblyLoader.GetModuleDef(data);

            Assembly.LoadFrom(assemblyFilename);

            return assemblyLoader;
        }

        public Logger _logger { get; }

        protected virtual object Test(string methodName, out Exception unhandledException, params object[] args)
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

            unhandledException = explorer.GetUnhandledException();

            return state.CurrentThread.RetValue;
        }

        protected virtual void TestAndCompare(string methodName, params object[] args)
        {
            object res2 = null;
            Exception modelCheckerException = null, ex2 = null;
            var modelCheckerResult = Test(methodName, out modelCheckerException, args);

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

            if (ex2 != null)
            {
                modelCheckerException?.GetType().Should().Be(ex2?.GetType(), modelCheckerException?.ToString());
                modelCheckerException?.Message.Should().BeEquivalentTo(ex2?.Message);
                return;
            }

            if (methodInfo.ReturnType != typeof(void))
            {
                if (modelCheckerResult != null && modelCheckerResult.Equals(ObjectReference.Null))
                {
                    modelCheckerResult = null;
                }

                if (modelCheckerResult == null && res2 == null)
                {
                    return;
                }

                if (modelCheckerException == null && modelCheckerResult == null && res2 != null)
                {
                    modelCheckerResult.Should().Be(res2);
                    return;
                }

                if (modelCheckerResult.GetType() == res2.GetType() && modelCheckerResult.GetType() == typeof(IntPtr))
                {
                    modelCheckerResult = ((IntPtr)modelCheckerResult).ToInt64();
                    res2 = ((IntPtr)res2).ToInt64();
                }

                if (modelCheckerResult.GetType() == res2.GetType() && modelCheckerResult.GetType() == typeof(UIntPtr))
                {
                    modelCheckerResult = ((UIntPtr)modelCheckerResult).ToUInt64();
                    res2 = ((UIntPtr)res2).ToUInt64();
                }

                if (methodInfo.ReturnType == typeof(IntPtr))
                {
                    var rr1 = (IConvertible)modelCheckerResult;
                    var rr2 = (IntPtr)res2;

                    Convert.ChangeType(rr1, typeof(long)).Should().BeEquivalentTo(rr2.ToInt64());
                    return;
                }

                if (methodInfo.ReturnType == typeof(UIntPtr))
                {
                    var rr1 = (IConvertible)modelCheckerResult;
                    var rr2 = (UIntPtr)res2;

                    Convert.ChangeType(rr1, typeof(ulong)).Should().BeEquivalentTo(rr2.ToUInt64());
                    return;
                }

                modelCheckerResult.Should().NotBeNull("value returned from model checker shoud not be null, but " + res2.ToString());
                res2.Should().NotBeNull();

                modelCheckerResult.Should().BeAssignableTo<IConvertible>();
                res2.Should().BeAssignableTo<IConvertible>();

                var modelCheckerReturnValue = Convert.ChangeType(modelCheckerResult, methodInfo.ReturnType);
                var r2 = Convert.ChangeType(res2, methodInfo.ReturnType);

                modelCheckerReturnValue.Should().BeEquivalentTo(r2);
            }
        }
    }
}
