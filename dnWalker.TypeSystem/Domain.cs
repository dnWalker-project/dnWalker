using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    public class Domain : IDomain
    {
        private readonly ModuleContext _moduleContext = ModuleDef.CreateModuleContext();

        private readonly Dictionary<string, ModuleDef> _loadedModules = new Dictionary<string, ModuleDef>();
        private readonly Dictionary<string, AssemblyDef> _loadedAssemblies = new Dictionary<string, AssemblyDef>();
        private ModuleDef _mainModule;

        private Domain()
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
            //foreach (AssemblyDef refAssembly in module.GetAssemblyRefs().Select(ar => _moduleContext.AssemblyResolver.Resolve(ar, module)))
            foreach (AssemblyRef ar in module.GetAssemblyRefs())
            {
                AssemblyDef refAssembly = _moduleContext.AssemblyResolver.Resolve(ar, module);

                if (refAssembly == null)
                {
                    System.Diagnostics.Debug.WriteLine($"WARNING: could not resolve the assembly: {ar.FullName}");
                    continue;
                }

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

        public static IDomain LoadFromFile(string file)
        {
            Domain definitionContext = new Domain();
            AssemblyDef mainAssembly = AssemblyDef.Load(file, definitionContext._moduleContext);
            definitionContext.SetupMain(mainAssembly);


            return definitionContext;
        }

        public static IDomain LoadFromAppDomain(Assembly loadedAssembly)
        {
            return LoadFromAppDomain(loadedAssembly.ManifestModule);
        }

        public static IDomain LoadFromAppDomain(Module loadedModule)
        {
            Domain definitionContext = new Domain();

            AssemblyDef mainAssembly = ModuleDefMD.Load(loadedModule, definitionContext._moduleContext).Assembly;
            definitionContext.SetupMain(mainAssembly);
            return definitionContext;
        }
    }
}
