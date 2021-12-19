using dnlib.DotNet;

using dnWalker.Instructions.Extensions;

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
        private static readonly ModuleContext _context = ModuleDef.CreateModuleContext();
        //private static readonly List<ModuleDef> _modules = new List<ModuleDef>();

        private static readonly IConfig _config;
        private static readonly Logger _logger;

        private static readonly IInstructionExecProvider _instructionExecProvider;
        private static readonly DefinitionProvider _definitionProvider;

        protected static ExplicitActiveState CreateState()
        {

            return new ExplicitActiveState(_config, _instructionExecProvider, _definitionProvider, _logger);
        }

        protected static DefinitionProvider DefinitionProvider
        {
            get { return _definitionProvider; }
        }


        static dnlibTypeTestBase()
        {
            _config = new Config();
            _logger = new Logger();

            var f = new dnWalker.Instructions.ExtendableInstructionFactory();
            f.AddSymbolicExecution();
            f.AddPathConstraintProducers();
            //f.AddParameterHandlers();


            _instructionExecProvider = InstructionExecProvider.Get(_config, f);


            //AssemblyLoader assemblyLoader = new AssemblyLoader();
            //assemblyLoader.GetModuleDef(typeof(dnlibTypeTestBase).Module);




            ModuleDef mainModule = ModuleDefMD.Load(typeof(dnlibTypeTestBase).Module, _context);

            var refModules = mainModule
                .GetAssemblyRefs()
                .Select(ar => _context.AssemblyResolver.Resolve(ar.Name, mainModule))
                .Where(a => a != null)
                .SelectMany(a => a.Modules)
                .ToArray();

            _definitionProvider = new DefinitionProvider(mainModule, refModules);
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
