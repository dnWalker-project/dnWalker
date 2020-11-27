using MMC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Primitives
{
    public struct Float8 : /*ISignedNumericElement, IRealElement, */IConvertible, ISymbolic
    {
        public static Float8 Zero = new Float8(0);

        public string WrapperName { get { return "System.Double"; } }

        public double Value { get; }

        Expression _expression;

        Expression ISymbolic.Expression => _expression;

        public bool IsFinite()
        {
            return !(double.IsInfinity(Value) || double.IsNaN(Value));
        }

        public IAddElement Add(INumericElement other, bool checkOverflow)
        {
            double op = other.ToFloat8(checkOverflow).Value;

            if (checkOverflow)
                return new Float8(checked(Value + op));
            else
                return new Float8(Value + op);
        }

        public INumericElement Div(INumericElement other)
        {
            double op = other.ToFloat8(false).Value;
            return new Float8(Value / op);
        }

        public INumericElement Mul(INumericElement other, bool checkOverflow)
        {
            double op = other.ToFloat8(checkOverflow).Value;

            if (checkOverflow)
                return new Float8(checked(Value * op));
            else
                return new Float8(Value * op);
        }

        public INumericElement Rem(INumericElement other)
        {
            double op = other.ToFloat8(false).Value;
            return new Float8(Value % op);
        }

        public ISubElement Sub(INumericElement other, bool checkOverflow)
        {
            double op = other.ToFloat8(checkOverflow).Value;

            if (checkOverflow)
                return new Float8(checked(Value - op));
            else
                return new Float8(Value - op);
        }

        public ISignedNumericElement Neg()
        {
            return new Float8(-Value);
        }

        public Int4 ToInt4(bool checkOverflow)
        {
            if (checkOverflow)
                return new Int4(checked((int)Value));
            else
                return new Int4((int)Value);
        }

        public UnsignedInt4 ToUnsignedInt4(bool checkOverflow)
        {
            if (checkOverflow)
                return new UnsignedInt4(checked((uint)Value));
            else
                return new UnsignedInt4((uint)Value);
        }

        public Int8 ToInt8(bool checkOverflow)
        {
            if (checkOverflow)
                return new Int8(checked((long)Value));
            else
                return new Int8((long)Value);
        }

        public UnsignedInt8 ToUnsignedInt8(bool checkOverflow)
        {
            if (checkOverflow)
                return new UnsignedInt8(checked((ulong)Value));
            else
                return new UnsignedInt8((ulong)Value);
        }

        public Float4 ToFloat4(bool checkOverflow)
        {
            if (checkOverflow)
                return new Float4(checked((float)Value));
            else
                return new Float4((float)Value);
        }

        public Float8 ToFloat8(bool checkOverflow) { return this; }

        public Float8(double val) { Value = val; }
        public bool ToBool() { return Value != 0; }

        public bool Equals(IDataElement other)
        {
            return other is Float8 float8 && float8.Value == Value;
        }

        public int CompareTo(object obj)
        {
            if (obj is IRealElement r)
            {
                return Value.CompareTo(r.ToFloat8(true).Value);
            }

            //return Value.CompareTo(((Float8)obj).Value);
            //return (int)(m_value - ((Float8)obj).Value);
            throw new NotSupportedException();
        }

        public override string ToString()
        {
            return Value.ToString() + "D";
        }

        public override int GetHashCode()
        {
            return (int)(Value * 191);
        }

        public TypeCode GetTypeCode()
        {
            return Value.GetTypeCode();
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToBoolean(provider);
        }

        public char ToChar(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToChar(provider);
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToSByte(provider);
        }

        public byte ToByte(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToByte(provider);
        }

        internal Int4 ToByte(bool checkOverflow)
        {
            if (checkOverflow)
                return new Int4(checked((byte)Value));
            else
                return new Int4((byte)Value);
        }

        public short ToInt16(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToInt16(provider);
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToUInt16(provider);
        }

        public int ToInt32(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToInt32(provider);
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToUInt32(provider);
        }

        public long ToInt64(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToInt64(provider);
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToUInt64(provider);
        }

        public float ToSingle(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToSingle(provider);
        }

        public double ToDouble(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToDouble(provider);
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToDecimal(provider);
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToDateTime(provider);
        }

        public string ToString(IFormatProvider provider)
        {
            return Value.ToString(provider);
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return ((IConvertible)Value).ToType(conversionType, provider);
        }
    }
}
