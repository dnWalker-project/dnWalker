using dnlib.DotNet;

using dnWalker.Symbolic.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public static class ValueFactory
    {
        public static IValue GetDefault(TypeSig type)
        {
            if (type.IsPrimitive)
            {
                if (type.IsBoolean())
                {
                    return new PrimitiveValue<bool>(false);
                }
                else if (type.IsChar())
                {
                    return new PrimitiveValue<char>(char.MinValue);
                }
                else if (type.IsByte())
                {
                    return new PrimitiveValue<byte>(0);
                }
                else if (type.IsUInt16())
                {
                    return new PrimitiveValue<ushort>(0);
                }
                else if (type.IsUInt32())
                {
                    return new PrimitiveValue<uint>(0);
                }
                else if (type.IsUInt64())
                {
                    return new PrimitiveValue<ulong>(0);
                }
                else if (type.IsSByte())
                {
                    return new PrimitiveValue<sbyte>(0);
                }
                else if (type.IsInt16())
                {
                    return new PrimitiveValue<short>(0);
                }
                else if (type.IsInt32())
                {
                    return new PrimitiveValue<int>(0);
                }
                else if (type.IsInt64())
                {
                    return new PrimitiveValue<long>(0);
                }
                else if (type.IsSingle())
                {
                    return new PrimitiveValue<float>(0);
                }
                else if (type.IsDouble())
                {
                    return new PrimitiveValue<double>(0);
                }

                throw new NotSupportedException("Unexpected primitive type.");
            }
            else if (type.IsString())
            {
                return StringValue.Null;
            }
            else
            {
                return Location.Null;
            }
        }
    }
}
