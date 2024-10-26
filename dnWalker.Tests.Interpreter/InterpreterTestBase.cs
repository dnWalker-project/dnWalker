using dnWalker.TypeSystem;

using System.Reflection;

using Xunit.Abstractions;

namespace dnWalker.Tests.Interpreter
{
    public abstract class InterpreterTestBase
    {
#if Linux
        private const string AssemblyFileName = @"../../../../extras/dnSpy.Debugger.DotNet.Interpreter.Tests.dll"; 
#elif Windows
        private const string AssemblyFileName = @"..\..\..\..\extras\dnSpy.Debugger.DotNet.Interpreter.Tests.dll"; 
#endif

        private static readonly IDefinitionProvider _definitionProvider;
        private readonly ITestOutputHelper _output;

        static InterpreterTestBase()
        {
            Assembly.LoadFrom(AssemblyFileName);

            _definitionProvider = new DefinitionProvider(Domain.LoadFromFile(AssemblyFileName));
        }

        protected InterpreterTestBase(ITestOutputHelper output)
        {
            _output = output ?? throw new ArgumentNullException(nameof(output));
        }

        protected ITestOutputHelper Output => _output;
        protected IDefinitionProvider DefinitionProvider => _definitionProvider;
        protected static string GetFullMethodName(string methodName)
        {
            return $"dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass.{methodName}";
        }
    }
}
