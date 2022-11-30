using dnlib.DotNet;

using dnWalker.TypeSystem;

using Xunit.Abstractions;

namespace dnWalker.TestUtils
{
    public abstract class DnlibTestBase<TTests> : DnlibTestBase
    {
        protected DnlibTestBase(ITestOutputHelper textOutput) : base(textOutput, typeof(TTests))
        {
        }
    }

    public abstract class DnlibTestBase
    {
        private readonly ITestOutputHelper _textOutput;
        private readonly IDefinitionProvider _definitionProvider;

        /// <summary>
        /// Initializes the <see cref="DnlibTestBase.DefinitionProvider"/> using entry assembly specified by <paramref name="assemblyFileName"/>.
        /// </summary>
        /// <param name="textOutput"></param>
        /// <param name="assemblyFileName"></param>
        protected DnlibTestBase(ITestOutputHelper textOutput, string assemblyFileName)
        {
            _textOutput = textOutput;
            _definitionProvider = new DefinitionProvider(Domain.LoadFromFile(assemblyFileName));
        }

        /// <summary>
        /// Initializes the <see cref="DnlibTestBase.DefinitionProvider"/> using the calling assembly as the entry assembly.
        /// </summary>
        /// <param name="textOutput"></param>
        protected DnlibTestBase(ITestOutputHelper textOutput, System.Reflection.Assembly theAssembly)
        {
            _textOutput = textOutput;
            _definitionProvider = new DefinitionProvider(Domain.LoadFromAppDomain(theAssembly));
        }

        /// <summary>
        /// Initializes the <see cref="DnlibTestBase.DefinitionProvider"/> using the calling assembly as the entry assembly.
        /// </summary>
        /// <param name="textOutput"></param>
        protected DnlibTestBase(ITestOutputHelper textOutput, Type theType)
        {
            _textOutput = textOutput;
            _definitionProvider = new DefinitionProvider(Domain.LoadFromAppDomain(theType.Assembly));
        }

        protected TypeDef GetType(string ns, string typeName)
        {
            return DefinitionProvider.GetTypeDefinition(ns + "." + typeName);
        }

        protected TypeDef GetType(Type type)
        {
            return GetType(type.Namespace ?? string.Empty, type.Name);
        }

        protected IEnumerable<MethodDef> GetMethods(string ns, string typeName, string methodName)
        {
            return GetType(ns, typeName).FindMethods(methodName);
        }

        protected MethodDef GetMethod(string ns, string typeName, string methodName)
        {
            return GetMethods(ns, typeName, methodName).Single();
        }

        protected MethodDef GetMethod(Type type, string methodName)
        {
            return GetMethods(type.Namespace ?? string.Empty, type.Name, methodName).Single();
        }

        protected ITestOutputHelper TextOutput
        {
            get
            {
                return _textOutput;
            }
        }

        protected IDefinitionProvider DefinitionProvider
        {
            get
            {
                return _definitionProvider;
            }
        }
    }
}