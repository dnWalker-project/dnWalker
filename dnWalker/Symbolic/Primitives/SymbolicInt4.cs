using MMC.Data;
using System;
using System.Linq.Expressions;

namespace dnWalker.Symbolic.Primitives
{
    [System.Diagnostics.DebuggerDisplay("SymbolicInt4({Value})")]
	public struct SymbolicInt4 : IHasIntValue, IIntegerElement, ISignedNumericElement, ISignedIntegerElement, IConvertible, ISymbolic
	{
        public static readonly Int4 Zero = new Int4(0);
		
		public string WrapperName { get { return "System.Int32 (symbolic)"; } }

        public int Value { get; }

        Expression _expression;

		Expression ISymbolic.Expression => _expression;

        public IAddElement Add(INumericElement other, bool checkOverflow)
		{
			int op = other.ToInt4(checkOverflow).Value;

			if (checkOverflow)
				return new SymbolicInt4(checked(Value + op), null);
			else
				return new SymbolicInt4(Value + op, null);
		}

		public INumericElement ToUnsigned()
		{
			return ToUnsignedInt4(false);
		}

		public INumericElement Div(INumericElement other)
		{
			int op = other.ToInt4(false).Value;
			return new SymbolicInt4(Value / op, null);
		}

		public INumericElement Mul(INumericElement other, bool checkOverflow)
		{
			int op = other.ToInt4(checkOverflow).Value;

			if (checkOverflow)
			{
				return new SymbolicInt4(checked(Value * op), null);
			}
			return new SymbolicInt4(Value * op, null);
		}

		public INumericElement Rem(INumericElement other)
		{
			int op = other.ToInt4(false).Value;
			return new SymbolicInt4(Value % op, null);
		}

		public ISignedNumericElement Neg()
		{
			return new SymbolicInt4(-Value, Expression.MakeUnary(ExpressionType.Negate, _expression, typeof(int)));
		}

		public ISubElement Sub(INumericElement other, bool checkOverflow)
		{
			int op = other.ToInt4(checkOverflow).Value;

			if (checkOverflow)
				return new SymbolicInt4(checked(Value - op), null);
			else
				return new SymbolicInt4(Value - op, null);
		}

		public IIntegerElement And(IIntegerElement other)
		{
			int op = other.ToInt4(false).Value;
			return new SymbolicInt4(Value & op, null);
		}

		public IIntegerElement Not()
		{
			return new SymbolicInt4(~Value, null);
		}

		public IIntegerElement Or(IIntegerElement other)
		{
			int op = other.ToInt4(false).Value;
			return new SymbolicInt4(Value | op, null);
		}

		public IIntegerElement Xor(IIntegerElement other)
		{
			int op = other.ToInt4(false).Value;
			return new SymbolicInt4(Value ^ op, null);
		}

		public IIntegerElement Shl(int x)
		{
			return new SymbolicInt4(Value << x, null);
		}

		public IIntegerElement Shr(int x)
		{
			return new SymbolicInt4(Value >> x, null);
		}

		public Int4 ToInt4(bool checkOverflow) { throw new NotImplementedException(); }

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

		public Float8 ToFloat8(bool checkOverflow)
		{
			if (checkOverflow)
				return new Float8(checked((double)Value));
			else
				return new Float8((double)Value);
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

		public bool ToBool() { return Value != 0; }

		public bool Equals(IDataElement other)
		{
			return (other is Int4) && (((Int4)other).Value == Value);
		}

		public int CompareTo(object obj)
		{
			return Value.CompareTo(((IHasIntValue)obj).Value);
		}

		public override string ToString()
		{

			return Value.ToString();
		}

		public override int GetHashCode()
		{

			return (int)Value;
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

		public SymbolicInt4(int val, Expression expression)
		{
			Value = val;
			_expression = expression ?? throw new ArgumentNullException(nameof(expression));
		}

		public static explicit operator SymbolicInt4(int b) => new SymbolicInt4(b, null);
	}
}
