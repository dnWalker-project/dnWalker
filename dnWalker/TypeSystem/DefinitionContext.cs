using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    public class DefinitionContext : IDefinitionContext
    {
        private readonly ModuleContext _moduleContext = ModuleDef.CreateModuleContext();

        private readonly Dictionary<string, ModuleDef> _loadedModules = new Dictionary<string, ModuleDef>();
        private readonly Dictionary<string, AssemblyDef> _loadedAssemblies = new Dictionary<string, AssemblyDef>();
        private ModuleDef _mainModule;

        private DefinitionContext()
        {

        }

        public ModuleDef MainModule
        {
            get { return _mainModule; }
        }

        public ModuleContext ModuleContext
        {
            get { return _moduleContext; }
        }

        public IReadOnlyCollection<ModuleDef> Modules
        {
            get
            {
                return _loadedModules.Values;
            }
        }

        public IReadOnlyCollection<AssemblyDef> Assemblies
        {
            get
            {
                return _loadedAssemblies.Values;
            }
        }

        private void Setup(ModuleDef module)
        {
            string moduleName = module.FullName;
            if (_loadedModules.ContainsKey(moduleName))
            {
                return;
            }

            _loadedModules.Add(moduleName, module);

            // load the referenced assemblies
            foreach (AssemblyDef refAssembly in module.GetAssemblyRefs().Select(ar => _moduleContext.AssemblyResolver.ResolveThrow(ar, module)))
            {
                Setup(refAssembly);
            }
        }

        private void Setup(AssemblyDef assembly)
        {
            string assemblyName = assembly.FullName;
            if (_loadedAssemblies.ContainsKey(assemblyName))
            {
                return;
            }

            _loadedAssemblies.Add(assemblyName, assembly);

            foreach (ModuleDef module in assembly.Modules)
            {
                Setup(module);
            }
        }

        private void SetupMain(AssemblyDef mainAssembly)
        {
            _mainModule = mainAssembly.ManifestModule;
            Setup(mainAssembly);
        }

        public static DefinitionContext LoadFromFile(string file)
        {
            DefinitionContext definitionContext = new DefinitionContext();
            AssemblyDef mainAssembly = AssemblyDef.Load(file, definitionContext._moduleContext);
            definitionContext.SetupMain(mainAssembly);


            return definitionContext;
        }

        public static DefinitionContext LoadFromAppDomain(Assembly loadedAssembly)
        {
            return LoadFromAppDomain(loadedAssembly.ManifestModule);
        }

        public static DefinitionContext LoadFromAppDomain(Module loadedModule)
        {
            DefinitionContext definitionContext = new DefinitionContext();

            AssemblyDef mainAssembly = ModuleDefMD.Load(loadedModule, definitionContext._moduleContext).Assembly;
            definitionContext.SetupMain(mainAssembly);
            return definitionContext;
        }
    }
}
