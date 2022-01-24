using dnlib.DotNet;

using dnWalker.Instructions.Extensions;
using dnWalker.TypeSystem;

using MMC;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Parameters
{
    public abstract class dnlibTypeTestBase
    {
        private static readonly IConfig _config;
        private static readonly Logger _logger;

        private static readonly IInstructionExecProvider _instructionExecProvider;
        private static readonly IDefinitionProvider _definitionProvider;

        protected static ExplicitActiveState CreateState()
        {

            return new ExplicitActiveState(_config, _instructionExecProvider, _definitionProvider, _logger);
        }

        protected static IDefinitionProvider DefinitionProvider
        {
            get { return _definitionProvider; }
        }


        static dnlibTypeTestBase()
        {
            _config = new Config();
            _logger = new Logger();

            var f = new dnWalker.Instructions.ExtendableInstructionFactory().AddStandardExtensions();


            _instructionExecProvider = InstructionExecProvider.Get(_config, f);


            _definitionProvider = new DefinitionProvider(DefinitionContext.LoadFromAppDomain(typeof(dnlibTypeTestBase).Module));
        }

        public static TypeSig GetType(string typeName)
        {
            if (typeName.EndsWith("[]"))
            {
                return GetArrayType(typeName.Substring(0, typeName.Length - 2));
            }

            //TypeSig type = _modules
            //    .SelectMany(m => m.Types)
            //    .FirstOrDefault(t => t.ReflectionFullName == typeName)
            //    .ToTypeSig();

            var type = _definitionProvider.GetTypeDefinition(typeName).ToTypeSig();

            if (type == null) throw new Exception("Could not resolve type: " + typeName);

            return type;
        }

        public static TypeSig GetArrayType(string elementTypeName)
        {
            var elementType = GetType(elementTypeName);

            //ArraySig array = new ArraySig(elementType.ToTypeSig());
            var array = new SZArraySig(elementType);

            if (array == null)
            {
                throw new Exception("ArraySig is a null!");
            }

            return array;
        }

        public static TypeSig GetType(Type type)
        {
            if (type.IsArray)
            {
                return GetArrayType(type.GetElementType());
            }

            return GetType(type.FullName);
        }

        public static TypeSig GetArrayType(Type elementType)
        {
            return GetArrayType(elementType.FullName);
        }
    }
}
