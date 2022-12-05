using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    public interface IDomain
    {
        IReadOnlyCollection<ModuleDef> Modules { get; }
        //IReadOnlyCollection<AssemblyDef> Assemblies { get; }
        ModuleDef MainModule { get; }
        ModuleContext ModuleContext { get; }

        IResolver Resolver { get; }

        bool Load(AssemblyDef assembly);
    }

    public static class DomainExtensions
    {
        public static bool Load(this IDomain domain, string assemblyFile)
        {
            AssemblyDef assembly = AssemblyDef.Load(assemblyFile, domain.ModuleContext);
            return domain.Load(assembly);
        }
    }
}
