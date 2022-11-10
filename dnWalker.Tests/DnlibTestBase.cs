using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.Tests
{
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
        protected DnlibTestBase(ITestOutputHelper textOutput)
        {
            _textOutput = textOutput;
            _definitionProvider = new DefinitionProvider(Domain.LoadFromAppDomain(System.Reflection.Assembly.GetCallingAssembly()));
        }

        protected TypeDef GetType(string ns, string typeName)
        {
            return DefinitionProvider.GetTypeDefinition(ns + "." + typeName);
        }

        protected IEnumerable<MethodDef> GetMethods(string ns, string typeName, string methodName)
        {
            return GetType(ns, typeName).FindMethods(methodName);
        }

        protected MethodDef GetMethod(string ns, string typeName, string methodName)
        {
            return GetMethods(ns, typeName, methodName).Single();
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
