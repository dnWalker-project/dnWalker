using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    public class Domain : IDomain
    {
        private readonly ModuleContext _moduleContext = ModuleDef.CreateModuleContext();

        private readonly Dictionary<string, ModuleDef> _loadedModules = new Dictionary<string, ModuleDef>();

        private ModuleDef? _mainModule;

        private readonly Resolver2 _typeResolver;

        private Domain()
        {
            _typeResolver = new Resolver2(_moduleContext.AssemblyResolver, _moduleContext.Resolver);
            _moduleContext.Resolver = _typeResolver;
        }

        public ModuleDef MainModule
        {
            get { return _mainModule ?? throw new InvalidOperationException("No module is loaded."); }
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

        public bool Load(AssemblyDef assemblyDef)
        {
            try
            {
                //Load(assemblyDef);
                _mainModule ??= assemblyDef.ManifestModule;

                foreach (ModuleDef module in assemblyDef.Modules)
                {
                    Load(module);
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to resolve or load assembly: '{assemblyDef}'. Error: '{ex}'");
                return false;
            }
        }

        private void Load(ModuleDef module)
        {
            string moduleName = module.FullName;
            if (_loadedModules.ContainsKey(moduleName))
            {
                return;
            }

            _loadedModules.Add(moduleName, module);

            // load the referenced assemblies
            AssemblyRef[] refs = module.GetAssemblyRefs().ToArray();
            foreach (AssemblyRef ar in refs)
            {
                AssemblyDef refAssembly = _moduleContext.AssemblyResolver.Resolve(ar, module);
                if (refAssembly != null)
                {
                    Load(refAssembly);
                }
            }
        }

        //private void Load(AssemblyDef assembly)
        //{
        //    foreach (ModuleDef module in assembly.Modules)
        //    {
        //        Load(module);
        //    }
        //}

        public static IDomain LoadFromFile(string file)
        {
            Domain domain = new Domain();
            AssemblyDef mainAssembly = AssemblyDef.Load(file, domain._moduleContext);
            domain._mainModule = mainAssembly.ManifestModule;
            domain.Load(mainAssembly);


            return domain;
        }

        public static IDomain LoadFromAppDomain(System.Reflection.Assembly loadedAssembly)
        {
            return LoadFromAppDomain(loadedAssembly.ManifestModule);
        }

        public static IDomain LoadFromAppDomain(System.Reflection.Module loadedModule)
        {
            Domain domain = new Domain();

            ModuleDef mainModule = ModuleDefMD.Load(loadedModule, domain._moduleContext);
            domain._mainModule = mainModule;
            domain.Load(mainModule.Assembly);
            return domain;
        }

        public static IDomain Create()
        {
            return new Domain();
        }

        public IResolver Resolver => _moduleContext.Resolver;
    }
}
