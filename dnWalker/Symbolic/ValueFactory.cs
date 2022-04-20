using dnlib.DotNet;

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
                ICorLibTypes corTypes = type.Module.CorLibTypes;
                TypeEqualityComparer ec = TypeEqualityComparer.Instance;
                if (ec.Equals(type, corTypes.Boolean))
                {
                    return new PrimitiveValue<bool>(false);
                }
                else if (ec.Equals(type, corTypes.Char))
                {
                    return new PrimitiveValue<char>(char.MinValue);
                }
                else if (ec.Equals(type, corTypes.Byte))
                {
                    return new PrimitiveValue<byte>(0);
                }
                else if (ec.Equals(type, corTypes.UInt16))
                {
                    return new PrimitiveValue<ushort>(0);
                }
                else if (ec.Equals(type, corTypes.UInt32))
                {
                    return new PrimitiveValue<uint>(0);
                }
                else if (ec.Equals(type, corTypes.UInt64))
                {
                    return new PrimitiveValue<ulong>(0);
                }
                else if (ec.Equals(type, corTypes.SByte))
                {
                    return new PrimitiveValue<sbyte>(0);
                }
                else if (ec.Equals(type, corTypes.Int16))
                {
                    return new PrimitiveValue<short>(0);
                }
                else if (ec.Equals(type, corTypes.Int32))
                {
                    return new PrimitiveValue<int>(0);
                }
                else if (ec.Equals(type, corTypes.Int64))
                {
                    return new PrimitiveValue<long>(0);
                }
                else if (ec.Equals(type, corTypes.Single))
                {
                    return new PrimitiveValue<float>(0);
                }
                else if (ec.Equals(type, corTypes.Double))
                {
                    return new PrimitiveValue<double>(0);
                }

                throw new NotSupportedException("Unexpected primitive type.");
            }
            else
            {
                return Location.Null;
            }
        }
    }
}
