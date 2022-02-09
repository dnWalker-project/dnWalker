using dnWalker.Parameters.Serialization.Xml;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public class ParameterContext : IParameterContext
    {
        public IDefinitionProvider DefinitionProvider
        {
            get;
        }

        public ParameterContext(IDefinitionProvider definitionProvider)
        {
            DefinitionProvider = definitionProvider;
        }
    }
}
