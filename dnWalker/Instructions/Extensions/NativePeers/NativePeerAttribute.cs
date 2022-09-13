using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.NativePeers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NativePeerAttribute : Attribute
    {
        public NativePeerAttribute(string typeName)
        {
            TypeName = typeName ?? throw new ArgumentNullException(nameof(typeName));
            Methods = null;
            MatchMethods = true;
        }
        public NativePeerAttribute(string typeName, params string[] methods)
        {
            TypeName = typeName ?? throw new ArgumentNullException(nameof(typeName));
            Methods = methods ?? throw new ArgumentNullException(nameof(methods));
        }

        public bool MatchMethods { get; set; }

        public string TypeName { get; }
        public string[] Methods { get; }
    }
}
