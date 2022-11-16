using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators
{
    public class SymbolContext
    {
        private readonly string _literal;
        private readonly int _length;
        private readonly string _symbol;
        private TypeSig _type;
        private Location _location;

        internal SymbolContext(string literal, int length, string symbol, TypeSig type, Location location)
        {
            _literal = literal;
            _length = length;
            _symbol = symbol;
            _type = type;
            _location = location;
        }

        public string Symbol => _symbol;

        public Location Location => _location;

        public int Length => _length;

        public TypeSig Type => _type;

        public string Literal => _literal;

        public IList<IMemberRef> MembersToArrange { get; } = new List<IMemberRef>();


        internal static SymbolContext Create(string symbol, IReadOnlyHeapNode node)
        {
            Location l = node.Location;
            string literal = symbol;
            TypeSig type = node.Type;

            int length = node is IReadOnlyArrayHeapNode arr ? arr.Length : 0;

            SymbolContext ctx = new SymbolContext(literal, length, symbol, type, l);
            
            if (node is IReadOnlyObjectHeapNode objNode)
            {

                foreach (IField fld in objNode.Fields)
                {
                    ctx.MembersToArrange.Add(fld);
                }

                foreach (IMethod m in objNode.MethodInvocations
                    .Select(t => t.method)
                    //.Distinct(MethodEqualityComparer.DontCompareDeclaringTypes))
                    .Distinct(MethodEqualityComparer.CompareDeclaringTypes))
                {
                    ctx.MembersToArrange.Add(m);
                }
            }

            return ctx;
        }

        internal static SymbolContext Create(string symbol, TypeSig type)
        {
            return new SymbolContext(symbol, 0, symbol, type, Location.Null);
        }
    }
}
