using dnlib.DotNet;
using System;
using System.Linq;

namespace dnWalker
{
    public class AssemblyLoader
    {
        private readonly ModuleContext _moduleContext;
        private ModuleDef _module;

        public AssemblyLoader()
        {
            _moduleContext = ModuleDef.CreateModuleContext();
        }

        public ModuleDef LoadAssembly(byte[] data)
        {
            AssemblyResolver asmResolver = (AssemblyResolver)_moduleContext.AssemblyResolver;
            asmResolver.EnableTypeDefCache = true;

            ModuleDefMD module = ModuleDefMD.Load(data, _moduleContext);
            module.Context = _moduleContext;

            ((AssemblyResolver)module.Context.AssemblyResolver).AddToCache(module);

            AssemblyDef asm = module.Assembly;

            /*var m = new ModuleDef();
            var asmDef = LoadAssemblies(config);
            // Load assembly, and initialize the definition lookup and active state.
            try
                {
                    Logger.l.Notice("loading main assembly...");
                    DefinitionProvider.LoadAssembly(config.AssemblyToCheckFileName);
                    Logger.l.Notice("loaded {0}", config.AssemblyToCheckFileName);
                }
                catch (System.Exception e)
                {
                    MonoModelChecker.Fatal("error loading assembly: " + config.AssemblyToCheckFileName + System.Environment.NewLine + e);
                }

                return DefinitionProvider.AssemblyDefinition;
            }*/
            _module = module;
            return module;
        }

        public ModuleDef GetModule() => _module ?? throw new Exception("Module was not loaded");

        public ModuleDef[] GetReferencedAssemblies(ModuleDef module)
        {
            return module.GetAssemblyRefs()
                .Select(ar => _moduleContext.AssemblyResolver.Resolve(ar.Name, module))
                .SelectMany(a => a.Modules)
                .ToArray();
        }
    }
}
