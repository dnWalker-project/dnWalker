using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public interface IParameterContext
    {
        IDefinitionProvider DefinitionProvider { get; }

    }
}
