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

        public static IValue GetValue<T>(T value)
            where T : struct
        {
            return new PrimitiveValue<T>(value);
        }


        public static IValue? ParseValue(string literal, TypeSig type)
        {
            if (string.IsNullOrEmpty(literal)) return GetDefault(type);

            IValue? value = null;

            if (type.IsString())
            {
                value = StringValue.Parse(literal);
            }
            else if (type.IsBoolean())
            {
                value = new PrimitiveValue<bool>(bool.Parse(literal));
            }
            else if (type.IsChar())
            {
                value = new PrimitiveValue<char>(literal[0]);
            }
            else if (type.IsByte())
            {
                value = new PrimitiveValue<byte>(byte.Parse(literal));
            }
            else if (type.IsUInt16())
            {
                value = new PrimitiveValue<ushort>(ushort.Parse(literal));
            }
            else if (type.IsUInt32())
            {
                value = new PrimitiveValue<uint>(uint.Parse(literal));
            }
            else if (type.IsUInt64())
            {
                value = new PrimitiveValue<ulong>(ulong.Parse(literal));
            }
            else if (type.IsSByte())
            {
                value = new PrimitiveValue<sbyte>(sbyte.Parse(literal));
            }
            else if (type.IsInt16())
            {
                value = new PrimitiveValue<short>(short.Parse(literal));
            }
            else if (type.IsInt32())
            {
                value = new PrimitiveValue<int>(int.Parse(literal));
            }
            else if (type.IsInt64())
            {
                value = new PrimitiveValue<long>(long.Parse(literal));
            }
            else if (type.IsSingle())
            {
                if (literal == "-INF") value = new PrimitiveValue<float>(float.NegativeInfinity);
                else if (literal == "+INF") value = new PrimitiveValue<float>(float.PositiveInfinity);
                else if (literal == "NAN") value = new PrimitiveValue<float>(float.NaN);
                else value = new PrimitiveValue<float>(float.Parse(literal));
            }
            else if (type.IsDouble())
            {
                if (literal == "-INF") value = new PrimitiveValue<double>(double.NegativeInfinity);
                else if (literal == "+INF") value = new PrimitiveValue<double>(double.PositiveInfinity);
                else if (literal == "NAN") value = new PrimitiveValue<double>(double.NaN);
                else value = new PrimitiveValue<double>(double.Parse(literal));
            }

            else if (!type.IsPrimitive)
            {
                if (literal == null || literal == "null") value = Location.Null;
                else
                {
                    throw new NotSupportedException("Non primitive non strings literals can only be null");
                }
            }

            return value;
        }

    }
}
