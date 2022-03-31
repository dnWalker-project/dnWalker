using dnlib.DotNet;

using dnWalker.TypeSystem;

using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public partial class SystemString : SymbolicNativePeer
    {
        private static readonly Dictionary<string, IMethodHandler> _handlers = new Dictionary<string, IMethodHandler>()
        {
            ["op_Equality"] = new Equality(),
            ["get_Length"] = new Length(),
            ["Concat"] = new Concat(),
            ["StartsWith"] = new StartsWith(),
            ["EndsWith"] = new EndsWith(),
            ["Substring"] = new Substring(),
            ["Contains"] = new Contains(),
        };

        public SystemString()
        {
            SetHandlers(_handlers);
        }

        protected override ITypeDefOrRef GetPeerType(IDefinitionProvider definitionProvider)
        {
            return definitionProvider.BaseTypes.String.ToTypeDefOrRef();
        }
    }
}
