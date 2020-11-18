using dnWalker.Symbolic.Expressions;
using MMC.Data;
using System;

namespace dnWalker.Symbolic.Primitives
{
    [System.Diagnostics.DebuggerDisplay("SymbolicInt4({Value})")]
	public struct SymbolicInt4 : IIntegerElement, ISignedNumericElement, ISignedIntegerElement, IConvertible, ISymbolic
	{
		int m_value;

		public static readonly Int4 Zero = new Int4(0);
		public string WrapperName { get { return "System.Int32 (symbolic)"; } }
		public int Value { get { return m_value; } }

        IExpression ISymbolic.Expression { get; set; }

        // public DataElementKind Kind => DataElementKind.Int32;

        public IAddElement Add(INumericElement other, bool checkOverflow)
		{
			int op = other.ToInt4(checkOverflow).Value;

			if (checkOverflow)
				return new SymbolicInt4(checked(m_value + op));
			else
				return new SymbolicInt4(m_value + op);
		}

		public INumericElement ToUnsigned()
		{
			return ToUnsignedInt4(false);
		}

		public INumericElement Div(INumericElement other)
		{
			int op = other.ToInt4(false).Value;
			return new SymbolicInt4(m_value / op);
		}

		public INumericElement Mul(INumericElement other, bool checkOverflow)
		{
			int op = other.ToInt4(checkOverflow).Value;

			if (checkOverflow)
			{
				return new SymbolicInt4(checked(m_value * op));
			}
			return new SymbolicInt4(m_value * op);
		}

		public INumericElement Rem(INumericElement other)
		{
			int op = other.ToInt4(false).Value;
			return new SymbolicInt4(m_value % op);
		}

		public ISignedNumericElement Neg()
		{
			return new SymbolicInt4(-m_value);
		}

		public ISubElement Sub(INumericElement other, bool checkOverflow)
		{
			int op = other.ToInt4(checkOverflow).Value;

			if (checkOverflow)
				return new SymbolicInt4(checked(m_value - op));
			else
				return new SymbolicInt4(m_value - op);
		}

		public IIntegerElement And(IIntegerElement other)
		{

			int op = other.ToInt4(false).Value;
			return new SymbolicInt4(m_value & op);
		}

		public IIntegerElement Not()
		{

			return new SymbolicInt4(~m_value);
		}

		public IIntegerElement Or(IIntegerElement other)
		{
			int op = other.ToInt4(false).Value;
			return new SymbolicInt4(m_value | op);
		}

		public IIntegerElement Xor(IIntegerElement other)
		{
			int op = other.ToInt4(false).Value;
			return new SymbolicInt4(m_value ^ op);
		}

		public IIntegerElement Shl(int x)
		{
			return new SymbolicInt4(m_value << x);
		}

		public IIntegerElement Shr(int x)
		{
			return new SymbolicInt4(m_value >> x);
		}

		public Int4 ToInt4(bool checkOverflow) { throw new NotImplementedException(); }

		public UnsignedInt4 ToUnsignedInt4(bool checkOverflow)
		{
			if (checkOverflow)
				return new UnsignedInt4(checked((uint)m_value));
			else
				return new UnsignedInt4((uint)m_value);
		}

		public Int8 ToInt8(bool checkOverflow)
		{
			if (checkOverflow)
				return new Int8(checked((long)m_value));
			else
				return new Int8((long)m_value);
		}

		public UnsignedInt8 ToUnsignedInt8(bool checkOverflow)
		{
			if (checkOverflow)
				return new UnsignedInt8(checked((ulong)m_value));
			else
				return new UnsignedInt8((ulong)m_value);
		}

		public Float4 ToFloat4(bool checkOverflow)
		{
			if (checkOverflow)
				return new Float4(checked((float)m_value));
			else
				return new Float4((float)m_value);
		}

		public Float8 ToFloat8(bool checkOverflow)
		{
			if (checkOverflow)
				return new Float8(checked((double)m_value));
			else
				return new Float8((double)m_value);
		}

		public Int4 ToByte(bool checkOverflow)
		{
			throw new NotImplementedException();
			/*if (checkOverflow)
				return new SymbolicInt4(checked((sbyte)m_value));
			else
				return new SymbolicInt4((sbyte)m_value);*/
		}

		public Int4 ToShort(bool checkOverflow)
		{
			throw new NotImplementedException();
			/*if (checkOverflow)
				return new SymbolicInt4(checked((short)m_value));
			else
				return new SymbolicInt4((short)m_value);*/
		}

		public bool ToBool() { return m_value != 0; }

		public bool Equals(IDataElement other)
		{

			return (other is Int4) && (((Int4)other).Value == m_value);
		}

		public int CompareTo(object obj)
		{
			return m_value.CompareTo(((Int4)obj).Value);
		}

		public override string ToString()
		{

			return Value.ToString();
		}

		public override int GetHashCode()
		{

			return (int)m_value;
		}

		public TypeCode GetTypeCode()
		{
			return m_value.GetTypeCode();
		}

		public bool ToBoolean(IFormatProvider provider)
		{
			return ((IConvertible)m_value).ToBoolean(provider);
		}

		public char ToChar(IFormatProvider provider)
		{
			return ((IConvertible)m_value).ToChar(provider);
		}

		public sbyte ToSByte(IFormatProvider provider)
		{
			return ((IConvertible)m_value).ToSByte(provider);
		}

		public byte ToByte(IFormatProvider provider)
		{
			return ((IConvertible)m_value).ToByte(provider);
		}

		public short ToInt16(IFormatProvider provider)
		{
			return ((IConvertible)m_value).ToInt16(provider);
		}

		public ushort ToUInt16(IFormatProvider provider)
		{
			return ((IConvertible)m_value).ToUInt16(provider);
		}

		public int ToInt32(IFormatProvider provider)
		{
			return ((IConvertible)m_value).ToInt32(provider);
		}

		public uint ToUInt32(IFormatProvider provider)
		{
			return ((IConvertible)m_value).ToUInt32(provider);
		}

		public long ToInt64(IFormatProvider provider)
		{
			return ((IConvertible)m_value).ToInt64(provider);
		}

		public ulong ToUInt64(IFormatProvider provider)
		{
			return ((IConvertible)m_value).ToUInt64(provider);
		}

		public float ToSingle(IFormatProvider provider)
		{
			return ((IConvertible)m_value).ToSingle(provider);
		}

		public double ToDouble(IFormatProvider provider)
		{
			return ((IConvertible)m_value).ToDouble(provider);
		}

		public decimal ToDecimal(IFormatProvider provider)
		{
			return ((IConvertible)m_value).ToDecimal(provider);
		}

		public DateTime ToDateTime(IFormatProvider provider)
		{
			return ((IConvertible)m_value).ToDateTime(provider);
		}

		public string ToString(IFormatProvider provider)
		{
			return m_value.ToString(provider);
		}

		public object ToType(Type conversionType, IFormatProvider provider)
		{
			return ((IConvertible)m_value).ToType(conversionType, provider);
		}

		public SymbolicInt4(int val)
		{
			m_value = val;
		}

		public static explicit operator SymbolicInt4(int b) => new SymbolicInt4(b);
	}
}
