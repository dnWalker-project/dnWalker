using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace dnWalker
{
    public class AssemblyLoader
    {
        private readonly ModuleContext _moduleContext;
        private ModuleDef _module;

        private HashSet<string> _loadedModules = new HashSet<string>();

        public AssemblyLoader()
        {
            _moduleContext = ModuleDef.CreateModuleContext();
        }

        public ModuleDef GetModuleDef(byte[] data)
        {
            var asmResolver = (AssemblyResolver)_moduleContext.AssemblyResolver;
            asmResolver.EnableTypeDefCache = true;

            var module = ModuleDefMD.Load(data, _moduleContext);
            module.Context = _moduleContext;

            ((AssemblyResolver)module.Context.AssemblyResolver).AddToCache(module);

            //AssemblyDef asm = module.Assembly;

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

            _loadedModules.Add(module.FullName);

            return module;
        }

        public ModuleDef GetModuleDef(Module module)
        {
            var asmResolver = (AssemblyResolver)_moduleContext.AssemblyResolver;
            asmResolver.EnableTypeDefCache = true;

            var moduleDef = ModuleDefMD.Load(module, _moduleContext);
            moduleDef.Context = _moduleContext;

            ((AssemblyResolver)moduleDef.Context.AssemblyResolver).AddToCache(moduleDef);

            _module = moduleDef;
            return moduleDef;
        }

        public ModuleDef GetModule() => _module ?? throw new Exception("Module was not loaded");

        private void LoadReferencedModulesRecursive(ModuleDef src, List<ModuleDef> modules)
        {
            var refs = src.GetAssemblyRefs();

            var refAssemblies = refs.Select(ar => _moduleContext.AssemblyResolver.Resolve(ar.Name, src));

            var refModules = refAssemblies.SelectMany(a => a.Modules);

            foreach (ModuleDef m in refModules)
            {
                if (_loadedModules.Contains(m.FullName)) continue;
                _loadedModules.Add(m.FullName);
                modules.Add(m);
                LoadReferencedModulesRecursive(m, modules);
            }

        }


        public ModuleDef[] GetReferencedModules(ModuleDef module)
        {
            List<ModuleDef> refs = new List<ModuleDef>();
            LoadReferencedModulesRecursive(module, refs);
            return refs.ToArray();

            //var refs = module.GetAssemblyRefs();

            //var refAssemblies = refs.Select(ar => _moduleContext.AssemblyResolver.Resolve(ar.Name, module));

            //var refModules = refAssemblies.SelectMany(a => a.Modules);


            //return refModules.ToArray();


            //return module.GetAssemblyRefs()
            //    .Select(ar => _moduleContext.AssemblyResolver.Resolve(ar.Name, module))
            //    .SelectMany(a => a.Modules)
            //    .ToArray();
        }

        /*public AppDomain CreateAppDomain(ModuleDef module, string path)
        {
            AppDomain domain = AppDomain.CreateDomain(module.FullName);
            domain.dir
            domain.ass
            var assembly = Assembly.LoadFile(path);

            return domain;
        }*/
    }
}
