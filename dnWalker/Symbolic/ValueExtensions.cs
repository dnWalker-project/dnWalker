using MMC.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public static class ValueExtensions
    {
        public static IDataElement ToDataElement(this IValue value)
        {
            return value switch
            {
                StringValue str => new ConstantString(str.Content),

                PrimitiveValue<bool> v => new Int4(v.Value ? 1 : 0, IntKind.Bool),

                PrimitiveValue<byte> v => new Int4(v.Value),
                PrimitiveValue<ushort> v => new Int4(v.Value),
                PrimitiveValue<uint> v => new UnsignedInt4(v.Value),
                PrimitiveValue<ulong> v => new UnsignedInt8(v.Value),

                PrimitiveValue<sbyte> v => new Int4(v.Value),
                PrimitiveValue<short> v => new Int4(v.Value),
                PrimitiveValue<int> v => new Int4(v.Value),
                PrimitiveValue<long> v => new Int8(v.Value),

                PrimitiveValue<float> v => new Float4(v.Value),
                PrimitiveValue<double> v => new Float8(v.Value),
                Location l => l == Location.Null ? new ObjectReference(0) : throw new NotSupportedException("Cannot create not null object reference without active state."),

                _ => throw new NotSupportedException("Unexpected value type.")
            };
        }
    }
}
