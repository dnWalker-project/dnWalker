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
        IReadOnlyCollection<AssemblyDef> Assemblies { get; }
        ModuleDef MainModule { get; }
        ModuleContext ModuleContext { get; }
    }
}
