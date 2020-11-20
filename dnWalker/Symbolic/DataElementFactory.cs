using dnWalker.Symbolic.Primitives;
using MMC.Data;
using System;
using System.Linq.Expressions;

namespace dnWalker.Symbolic
{
    public class DataElementFactory
    {
        public static IDataElement CreateDataElement<T>(object value, Expression expression)
        {
            switch (Type.GetTypeCode(typeof(T)))
            {/*
                case TypeCode.Boolean: return new Int4((bool)value ? 1 : 0);
                case TypeCode.Char: return new Int4((char)value);
                case TypeCode.SByte: return new Int4((sbyte)value);
                case TypeCode.Byte: return new Int4((byte)value);
                case TypeCode.Int16: return new Int4((short)value);
                case TypeCode.UInt16: return new UnsignedInt4((ushort)value);*/
                case TypeCode.Int32: return new SymbolicInt4((int)value, expression);
                //case TypeCode.UInt32: return new UnsignedInt4((uint)value);
                //case TypeCode.Int64: return new Int8((long)value);
                //case TypeCode.UInt64: return new UnsignedInt8((ulong)value);
                //case TypeCode.Single: return new Float4((float)value);
                //case TypeCode.Double: return new Float8((double)value);
                //case TypeCode.String: return new ConstantString(value.ToString());*/
                default:/*
                    if (value is IntPtr ip)
                    {
                        return IntPtr.Size == 4 ? CreateDataElement(ip.ToInt32()) : CreateDataElement(ip.ToInt64());
                    }
                    if (value is UIntPtr up)
                    {
                        return IntPtr.Size == 4 ? CreateDataElement(up.ToUInt32()) : CreateDataElement(up.ToUInt64());
                    }*/
                    throw new NotSupportedException("CreateDataElement for " + typeof(T));
            }
        }
    }
}
