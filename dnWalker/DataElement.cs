/*
 *   Copyright 2007 University of Twente, Formal Methods and Tools group
 *
 *   Licensed under the Apache License, Version 2.0 (the "License");
 *   you may not use this file except in compliance with the License.
 *   You may obtain a copy of the License at
 *
 *       http://www.apache.org/licenses/LICENSE-2.0
 *
 *   Unless required by applicable law or agreed to in writing, software
 *   distributed under the License is distributed on an "AS IS" BASIS,
 *   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *   See the License for the specific language governing permissions and
 *   limitations under the License.
 *
 */

namespace MMC.Data {
    using System; // For ObsoleteAttribute :-)

    using dnlib.DotNet.Emit;
    using MMC.State;
    using dnlib.DotNet;
    using MethodDefinition = dnlib.DotNet.MethodDef;
    using TypeDefinition = dnlib.DotNet.TypeDef;
    using FieldDefinition = dnlib.DotNet.FieldDef;
    using ParameterDefinition = dnlib.DotNet.Parameter;
    using dnWalker;

    public interface IDataElement : System.IComparable
    {
		bool Equals(IDataElement other);
		bool ToBool();
		string ToString();
		string WrapperName { get; }
    }

    /// The following two interfaces are used to abstract
    /// addition and substraction arithmetics, which can be 
    /// used on integers, floats, pointers etc.
    /// The checkoverflow boolean is used by instructions
    /// that check for overflow like add.ovf
    public interface IAddElement : IDataElement {
		IAddElement Add(INumericElement other, bool checkOverflow);
	}

    public interface ISubElement : IDataElement  {
		ISubElement Sub(INumericElement other, bool checkOverflow);
	}

    public interface IManagedPointer : IDataElement
    {
        IDataElement Value { get; set; }
		Int4 ToInt4();
	}

    public interface IRuntimeHandle : IDataElement {}

    public interface INumericElement : IDataElement, IAddElement, ISubElement
    {
		INumericElement Mul(INumericElement other, bool checkOverflow);
		INumericElement Div(INumericElement other);
		INumericElement Rem(INumericElement other);

        Int4 ToInt4(bool checkOverflow);
		UnsignedInt4 ToUnsignedInt4(bool checkOverflow);
		Int8 ToInt8(bool checkOverflow);
		UnsignedInt8 ToUnsignedInt8(bool checkOverflow);
		Float4 ToFloat4(bool checkOverflow);
		Float8 ToFloat8(bool checkOverflow);
	}

    public interface IRealElement : INumericElement {
		bool IsFinite();
	}

    public interface ISignedNumericElement : INumericElement {
		ISignedNumericElement Neg();		
	}

    public interface ISignedIntegerElement : INumericElement {
		INumericElement ToUnsigned();
	}

    public interface IIntegerElement : INumericElement
    {
        IIntegerElement And(IIntegerElement other);
        IIntegerElement Not();
        IIntegerElement Or(IIntegerElement other);
        IIntegerElement Xor(IIntegerElement other);
        IIntegerElement Shl(int x);
        IIntegerElement Shr(int x);
    }

    public interface IReferenceType : IDataElement {

		uint Location { get; }
	}

    /* --------------------------------------------------------------
	 * The following structs define objects that can be loaded onto
	 * the stack by load instructions, or be the result of the
	 * execution of some other (e.g. arithmetic) operation.
	 * -------------------------------------------------------------- */

    public struct Int4 : IIntegerElement, ISignedNumericElement, ISignedIntegerElement
    {
		int m_value;

		public static readonly Int4 Zero = new Int4(0);
		public string WrapperName { get { return "System.Int32"; } }
		public int Value { get { return m_value; } }
        // public DataElementKind Kind => DataElementKind.Int32;

		public IAddElement Add(INumericElement other, bool checkOverflow) {
			int op = other.ToInt4(checkOverflow).Value;

			if (checkOverflow)
				return new Int4(checked(m_value + op));
			else
				return new Int4(m_value + op);

		}

		public INumericElement ToUnsigned() {
			return ToUnsignedInt4(false);
		}

		public INumericElement Div(INumericElement other) {

			int op = other.ToInt4(false).Value;
			return new Int4(m_value / op);
		}

		public INumericElement Mul(INumericElement other, bool checkOverflow) {

			int op = other.ToInt4(checkOverflow).Value;

			if (checkOverflow)
				return new Int4(checked(m_value * op));
			else
				return new Int4(m_value * op);

		}

		public INumericElement Rem(INumericElement other) {

			int op = other.ToInt4(false).Value;
			return new Int4(m_value % op);
		}

		public ISignedNumericElement Neg() {

			return new Int4(-m_value);
		}

		public ISubElement Sub(INumericElement other, bool checkOverflow) {

			int op = other.ToInt4(checkOverflow).Value;

			if (checkOverflow)
				return new Int4(checked(m_value - op));
			else
				return new Int4(m_value - op);
		}

		public IIntegerElement And(IIntegerElement other) {

			int op = other.ToInt4(false).Value;
			return new Int4(m_value & op);
		}

		public IIntegerElement Not() {

			return new Int4(~m_value);
		}

		public IIntegerElement Or(IIntegerElement other) {

			int op = other.ToInt4(false).Value;
			return new Int4(m_value | op);
		}

		public IIntegerElement Xor(IIntegerElement other) {

			int op = other.ToInt4(false).Value;
			return new Int4(m_value ^ op);
		}

		public IIntegerElement Shl(int x) {

			return new Int4(m_value << x);
		}

		public IIntegerElement Shr(int x) {

			return new Int4(m_value >> x);
		}

		public Int4 ToInt4(bool checkOverflow) { return this; }

		public UnsignedInt4 ToUnsignedInt4(bool checkOverflow) {
			if (checkOverflow)
				return new UnsignedInt4(checked((uint)m_value));
			else
				return new UnsignedInt4((uint)m_value);
		}

		public Int8 ToInt8(bool checkOverflow) {
			if (checkOverflow)
				return new Int8(checked((long)m_value));
			else
				return new Int8((long)m_value);
		}

		public UnsignedInt8 ToUnsignedInt8(bool checkOverflow) {
			if (checkOverflow)
				return new UnsignedInt8(checked((ulong)m_value));
			else
				return new UnsignedInt8((ulong)m_value);
		}

		public Float4 ToFloat4(bool checkOverflow) {
			if (checkOverflow)
				return new Float4(checked((float)m_value));
			else
				return new Float4((float)m_value);
		}

		public Float8 ToFloat8(bool checkOverflow) {
			if (checkOverflow)
				return new Float8(checked((double)m_value));
			else
				return new Float8((double)m_value);
		}

		public Int4 ToByte(bool checkOverflow) {

			if (checkOverflow)
				return new Int4(checked((sbyte)m_value));
			else
				return new Int4((sbyte)m_value);
		}

		public Int4 ToShort(bool checkOverflow) {
			if (checkOverflow)
				return new Int4(checked((short)m_value));
			else
				return new Int4((short)m_value);
		}

		public bool ToBool() { return m_value != 0; }

		public bool Equals(IDataElement other) {

			return (other is Int4) && (((Int4)other).Value == m_value);
		}

		public int CompareTo(object obj) {
			return m_value.CompareTo(((Int4)obj).Value);
		}

		public override string ToString() {

			return Value.ToString();
		}

		public override int GetHashCode() {

			return (int)m_value;
		}

		public Int4(int val) {

			m_value = val;
		}

        public static explicit operator Int4(int b) => new Int4(b);
    }

    public struct UnsignedInt4 : IIntegerElement {
		public static UnsignedInt4 Zero = new UnsignedInt4(0);
		uint m_value;

		public uint Value { get { return m_value; } }
		public string WrapperName { get { return "System.UInt32"; } }
        // public DataElementKind Kind => DataElementKind.UInt32;

        public IAddElement Add(INumericElement other, bool checkOverflow) {

			uint op = other.ToUnsignedInt4(checkOverflow).Value;

			if (checkOverflow)
				return new UnsignedInt4(checked(m_value + op));
			else
				return new UnsignedInt4(m_value + op);
		}

		public INumericElement Div(INumericElement other) {

			uint op = other.ToUnsignedInt4(false).Value;
			return new UnsignedInt4(m_value / op);
		}

		public INumericElement Mul(INumericElement other, bool checkOverflow) {

			uint op = other.ToUnsignedInt4(checkOverflow).Value;

			if (checkOverflow)
				return new UnsignedInt4(checked(m_value * op));
			else
				return new UnsignedInt4(m_value * op);
		}

		public INumericElement Rem(INumericElement other) {

			uint op = other.ToUnsignedInt4(false).Value;
			return new UnsignedInt4(m_value % op);
		}

		public ISubElement Sub(INumericElement other, bool checkOverflow) {

			uint op = other.ToUnsignedInt4(checkOverflow).Value;

			if (checkOverflow)
				return new UnsignedInt4(checked(m_value - op));
			else
				return new UnsignedInt4(m_value - op);
		}

		public IIntegerElement And(IIntegerElement other) {

			uint op = other.ToUnsignedInt4(false).Value;
			return new UnsignedInt4(m_value & op);
		}

		public IIntegerElement Not() {

			return new UnsignedInt4(~m_value);
		}

		public IIntegerElement Or(IIntegerElement other) {

			uint op = other.ToUnsignedInt4(false).Value;
			return new UnsignedInt4(m_value | op);
		}

		public IIntegerElement Xor(IIntegerElement other) {

			uint op = other.ToUnsignedInt4(false).Value;
			return new UnsignedInt4(m_value ^ op);
		}

		public IIntegerElement Shl(int x) {

			return new UnsignedInt4(m_value << x);
		}

		public IIntegerElement Shr(int x) {

			return new UnsignedInt4(m_value >> x);
		}

		public Int4 ToInt4(bool checkOverflow) {
			if (checkOverflow)
				return new Int4(checked((int)Value));
			else
				return new Int4((int)Value);
		}

		public UnsignedInt4 ToUnsignedInt4(bool checkOverflow) { return this; }

		public Int8 ToInt8(bool checkOverflow) {
			if (checkOverflow)
				return new Int8(checked((long)m_value));
			else
				return new Int8((long)m_value);
		}

		public UnsignedInt8 ToUnsignedInt8(bool checkOverflow) {
			if (checkOverflow)
				return new UnsignedInt8(checked((ulong)m_value));
			else
				return new UnsignedInt8((ulong)m_value);
		}

		public Float4 ToFloat4(bool checkOverflow) {
			if (checkOverflow)
				return new Float4(checked((float)m_value));
			else
				return new Float4((float)m_value);
		}

		public Float8 ToFloat8(bool checkOverflow) {
			if (checkOverflow)
				return new Float8(checked((double)m_value));
			else
				return new Float8((double)m_value);
		}

		public Int4 ToByte(bool checkOverflow) {
			if (checkOverflow)
				return new Int4(checked((byte)m_value));
			else
				return new Int4((byte)m_value);
		}

		public Int4 ToShort(bool checkOverflow) {
			ushort s;
			if (checkOverflow)
				s = checked((ushort)m_value);

			return new Int4((int)(m_value & 0x0000FFFF));
		}

		public bool ToBool() { return m_value != 0; }

		public bool Equals(IDataElement other) {

			return (other is UnsignedInt4) && ((UnsignedInt4)other).Value == m_value;
		}

		public int CompareTo(object obj) {
			return m_value.CompareTo(((UnsignedInt4)obj).Value);
		}

		public override string ToString() {

			return Value.ToString();
		}

		public override int GetHashCode() {

			return (int)m_value;
		}

		public UnsignedInt4(uint val) {

			m_value = val;
		}
	}

    public struct Int8 : IIntegerElement, ISignedNumericElement, ISignedIntegerElement {
		public static Int8 Zero = new Int8(0);
		long m_value;

		public string WrapperName { get { return "System.Int64"; } }
		public long Value { get { return m_value; } }
        // public DataElementKind Kind => DataElementKind.Int64;

        public IAddElement Add(INumericElement other, bool checkOverflow) {

			long op = other.ToInt8(checkOverflow).Value;

			if (checkOverflow)
				return new Int8(checked(m_value + op));
			else
				return new Int8(m_value + op);
		}

		public INumericElement Div(INumericElement other) {

			long op = other.ToInt8(false).Value;
			return new Int8(m_value / op);
		}

		public INumericElement Mul(INumericElement other, bool checkOverflow) {

			long op = other.ToInt8(checkOverflow).Value;
			if (checkOverflow)
				return new Int8(checked(m_value * op));
			else
				return new Int8(m_value * op);

		}

		public INumericElement Rem(INumericElement other) {

			long op = other.ToInt8(false).Value;
			return new Int8(m_value % op);
		}

		public ISubElement Sub(INumericElement other, bool checkOverflow) {

			long op = other.ToInt8(checkOverflow).Value;

			if (checkOverflow)
				return new Int8(checked(m_value - op));
			else
				return new Int8(m_value - op);
		}

		public ISignedNumericElement Neg() {

			return new Int8(-m_value);
		}

		public IIntegerElement And(IIntegerElement other) {

			long op = other.ToInt8(false).Value;
			return new Int8(m_value & op);
		}

		public IIntegerElement Not() {

			return new Int8(~m_value);
		}

		public IIntegerElement Or(IIntegerElement other) {

			long op = other.ToInt8(false).Value;
			return new Int8(m_value | op);
		}

		public IIntegerElement Xor(IIntegerElement other) {

			long op = other.ToInt8(false).Value;
			return new Int8(m_value ^ op);
		}

		public IIntegerElement Shl(int x) {

			return new Int8(m_value << x);
		}

		public IIntegerElement Shr(int x) {

			return new Int8(m_value >> x);
		}

		public Int4 ToInt4(bool checkOverflow) {
			if (checkOverflow)
				return new Int4(checked((int)m_value));
			else
				return new Int4((int)m_value);
		}

		public UnsignedInt4 ToUnsignedInt4(bool checkOverflow) {
			if (checkOverflow)
				return new UnsignedInt4(checked((uint)m_value));
			else
				return new UnsignedInt4((uint)m_value);
		}

		public Int8 ToInt8(bool checkOverflow) { return this; }

		public UnsignedInt8 ToUnsignedInt8(bool checkOverflow) {
			if (checkOverflow)
				return new UnsignedInt8(checked((ulong)m_value));
			else
				return new UnsignedInt8((ulong)m_value);
		}

		public Float4 ToFloat4(bool checkOverflow) {
			if (checkOverflow)
				return new Float4(checked((float)m_value));
			else
				return new Float4((float)m_value);
		}

		public Float8 ToFloat8(bool checkOverflow) {
			if (checkOverflow)
				return new Float8(checked((double)m_value));
			else
				return new Float8((double)m_value);
		}

		public bool ToBool() { return m_value != 0; }

		public INumericElement ToUnsigned()
        {
			return ToUnsignedInt8(false);
		}

        public bool Equals(IDataElement other)
        {
            return (other is Int8) && ((Int8)other).Value == m_value;
        }

        public int CompareTo(object obj)
        {
            var n = (obj as INumericElement).ToInt8(true);
            return m_value.CompareTo(n.Value);
        }

        public override string ToString()
        {
            return Value.ToString() + "L";
        }

		public override int GetHashCode() {

			return (int)(m_value);
		}

		public Int8(long val) {

			m_value = val;
		}
	}

    public struct UnsignedInt8 : IIntegerElement {
		public static UnsignedInt8 Zero = new UnsignedInt8(0);
		ulong m_value;

		public string WrapperName { get { return "System.UInt64"; } }
		public ulong Value { get { return m_value; } }
        // public DataElementKind Kind => DataElementKind.UInt64;

        public IAddElement Add(INumericElement other, bool checkOverflow) {

			ulong op = other.ToUnsignedInt8(checkOverflow).Value;

			if (checkOverflow)
				return new UnsignedInt8(checked(m_value + op));
			else
				return new UnsignedInt8(m_value + op);

		}

		public INumericElement Div(INumericElement other) {

			ulong op = other.ToUnsignedInt8(false).Value;
			return new UnsignedInt8(m_value / op);
		}

		public INumericElement Mul(INumericElement other, bool checkOverflow) {

			ulong op = other.ToUnsignedInt8(checkOverflow).Value;

			if (checkOverflow)
				return new UnsignedInt8(checked(m_value * op));
			else
				return new UnsignedInt8(m_value * op);
		}

		public INumericElement Rem(INumericElement other) {

			ulong op = other.ToUnsignedInt8(false).Value;
			return new UnsignedInt8(m_value % op);
		}

		public ISubElement Sub(INumericElement other, bool checkOverflow) {

			ulong op = other.ToUnsignedInt8(checkOverflow).Value;

			if (checkOverflow)
				return new UnsignedInt8(checked(m_value - op));
			else
				return new UnsignedInt8(m_value - op);
		}

		public IIntegerElement And(IIntegerElement other) {

			ulong op = other.ToUnsignedInt8(false).Value;
			return new UnsignedInt8(m_value & op);
		}

		public IIntegerElement Not() {

			return new UnsignedInt8(~m_value);
		}

		public IIntegerElement Or(IIntegerElement other) {

			ulong op = other.ToUnsignedInt8(false).Value;
			return new UnsignedInt8(m_value | op);
		}

		public IIntegerElement Xor(IIntegerElement other) {

			ulong op = other.ToUnsignedInt8(false).Value;
			return new UnsignedInt8(m_value ^ op);
		}

		public IIntegerElement Shl(int x) {

			return new UnsignedInt8(m_value << x);
		}

		public IIntegerElement Shr(int x) {

			return new UnsignedInt8(m_value >> x);
		}

		public Int4 ToInt4(bool checkOverflow) {
			if (checkOverflow)
				return new Int4(checked((int)m_value));
			else
				return new Int4((int)m_value);
		}

		public UnsignedInt4 ToUnsignedInt4(bool checkOverflow) {
			if (checkOverflow)
				return new UnsignedInt4(checked((uint)m_value));
			else
				return new UnsignedInt4((uint)m_value);
		}

		public Int8 ToInt8(bool checkOverflow) {
			if (checkOverflow)
				return new Int8(checked((long)m_value));
			else
				return new Int8((long)m_value);
		}

		public UnsignedInt8 ToUnsignedInt8(bool checkOverflow) { return this; }

		public Float4 ToFloat4(bool checkOverflow) {
			if (checkOverflow)
				return new Float4(checked((float)m_value));
			else
				return new Float4((float)m_value);
		}

		public Float8 ToFloat8(bool checkOverflow) {
			if (checkOverflow)
				return new Float8(checked((double)m_value));
			else
				return new Float8((double)m_value);
		}

		public bool ToBool() { return m_value != 0; }

		public bool Equals(IDataElement other) {

			return (other is UnsignedInt8) && ((UnsignedInt8)other).Value == m_value;
		}

		public int CompareTo(object obj) {
			return m_value.CompareTo(((UnsignedInt8)obj).Value);
		}

		public override string ToString() {

			return Value.ToString() + "UL";
		}

		public override int GetHashCode() {

			return (int)(m_value * 101);
		}

		public UnsignedInt8(ulong val) {

			m_value = val;
		}
	}

    public struct Float4 : ISignedNumericElement, IRealElement {
		public static Float4 Zero = new Float4(0);

		float m_value;

		public string WrapperName { get { return "System.Single"; } }
		public float Value { get { return m_value; } }
        // public DataElementKind Kind => DataElementKind.Single;

        public IAddElement Add(INumericElement other, bool checkOverflow) {

			float op = other.ToFloat4(checkOverflow).Value;

			if (checkOverflow)
				return new Float4(checked(m_value + op));
			else
				return new Float4(m_value + op);
		}

		public INumericElement Div(INumericElement other) {

			float op = other.ToFloat4(false).Value;
			return new Float4(m_value / op);
		}

		public bool IsFinite() {
			return !(float.IsInfinity(m_value) | float.IsNaN(m_value));
		}

		public INumericElement Mul(INumericElement other, bool checkOverflow) {

			float op = other.ToFloat4(checkOverflow).Value;

			if (checkOverflow)
				return new Float4(checked(m_value * op));
			else
				return new Float4(m_value * op);
		}

		public INumericElement Rem(INumericElement other) {

			float op = other.ToFloat4(false).Value;
			return new Float4(m_value % op);
		}

		public ISubElement Sub(INumericElement other, bool checkOverflow) {

			float op = other.ToFloat4(checkOverflow).Value;

			if (checkOverflow)
				return new Float4(checked(m_value - op));
			else
				return new Float4(m_value - op);
		}

		public ISignedNumericElement Neg() {

			return new Float4(-m_value);
		}

		public Int4 ToInt4(bool checkOverflow) {
			if (checkOverflow)
				return new Int4(checked((int)m_value));
			else
				return new Int4((int)m_value);
		}

		public UnsignedInt4 ToUnsignedInt4(bool checkOverflow) {
			if (checkOverflow)
				return new UnsignedInt4(checked((uint)m_value));
			else
				return new UnsignedInt4((uint)m_value);
		}

		public Int8 ToInt8(bool checkOverflow) {
			if (checkOverflow)
				return new Int8(checked((long)m_value));
			else
				return new Int8((long)m_value);
		}

		public UnsignedInt8 ToUnsignedInt8(bool checkOverflow) {
			if (checkOverflow)
				return new UnsignedInt8(checked((ulong)m_value));
			else
				return new UnsignedInt8((ulong)m_value);
		}

		public Float4 ToFloat4(bool checkOverflow) { return this; }

		public Float8 ToFloat8(bool checkOverflow) {
			if (checkOverflow)
				return new Float8(checked((double)m_value));
			else
				return new Float8((double)m_value);
		}

		public bool ToBool() { return m_value != 0; }

		public bool Equals(IDataElement other) {

			return (other is Float4) && ((Float4)other).Value == m_value;
		}

		public int CompareTo(object obj) {
			return m_value.CompareTo(((Float4)obj).Value);
		}

		public override string ToString() {

			return Value.ToString() + "F";
		}

		public override int GetHashCode() {

			return (int)(m_value * 577);
		}

		public Float4(float val) {

			m_value = val;
		}
	}

    public struct Float8 : ISignedNumericElement, IRealElement {
		public static Float8 Zero = new Float8(0);
		double m_value;

		public string WrapperName { get { return "System.Double"; } }
		public double Value { get { return m_value; } }
        // public DataElementKind Kind => DataElementKind.Double;

        public bool IsFinite() {
			return !(double.IsInfinity(m_value) | double.IsNaN(m_value));
		}

		public IAddElement Add(INumericElement other, bool checkOverflow) {

			double op = other.ToFloat8(checkOverflow).Value;

			if (checkOverflow)
				return new Float8(checked(m_value + op));
			else
				return new Float8(m_value + op);
		}

		public INumericElement Div(INumericElement other) {

			double op = other.ToFloat8(false).Value;
			return new Float8(m_value / op);
		}

		public INumericElement Mul(INumericElement other, bool checkOverflow) {

			double op = other.ToFloat8(checkOverflow).Value;

			if (checkOverflow)
				return new Float8(checked(m_value * op));
			else
				return new Float8(m_value * op);
		}

		public INumericElement Rem(INumericElement other) {

			double op = other.ToFloat8(false).Value;
			return new Float8(m_value % op);
		}

		public ISubElement Sub(INumericElement other, bool checkOverflow) {

			double op = other.ToFloat8(checkOverflow).Value;

			if (checkOverflow)
				return new Float8(checked(m_value - op));
			else
				return new Float8(m_value - op);
		}

		public ISignedNumericElement Neg() {

			return new Float8(-m_value);
		}

		public Int4 ToInt4(bool checkOverflow) {
			if (checkOverflow)
				return new Int4(checked((int)m_value));
			else
				return new Int4((int)m_value);
		}

		public UnsignedInt4 ToUnsignedInt4(bool checkOverflow) {
			if (checkOverflow)
				return new UnsignedInt4(checked((uint)m_value));
			else
				return new UnsignedInt4((uint)m_value);
		}

		public Int8 ToInt8(bool checkOverflow) {
			if (checkOverflow)
				return new Int8(checked((long)m_value));
			else
				return new Int8((long)m_value);
		}

		public UnsignedInt8 ToUnsignedInt8(bool checkOverflow) {
			if (checkOverflow)
				return new UnsignedInt8(checked((ulong)m_value));
			else
				return new UnsignedInt8((ulong)m_value);
		}

		public Float4 ToFloat4(bool checkOverflow) {
			if (checkOverflow)
				return new Float4(checked((float)m_value));
			else
				return new Float4((float)m_value);
		}

		public Float8 ToFloat8(bool checkOverflow) { return this; }

		public Float8(double val) { m_value = val; }
		public bool ToBool() { return m_value != 0; }

		public bool Equals(IDataElement other) {

			return (other is Float8) && ((Float8)other).Value == m_value;
		}

		public int CompareTo(object obj) {
			return m_value.CompareTo(((Float8)obj).Value);
			//return (int)(m_value - ((Float8)obj).Value);
		}

		public override string ToString() {

			return Value.ToString() + "D";
		}

		public override int GetHashCode() {

			return (int)(m_value * 191);
		}
	}

	struct IntPointer : IDataElement {

		System.IntPtr m_value;

		public string WrapperName { get { return "System.IntPtr"; } }
		public System.IntPtr Value { get { return m_value; } }
        // public DataElementKind Kind => DataElementKind.NativeInt;

        public bool ToBool() { return ((uint)m_value) == 0; }

		public override string ToString() {

			return ((uint)m_value).ToString();
		}

		public bool Equals(IDataElement other) {

			return (other is IntPointer) && CompareTo(other) == 0;
		}

		public int CompareTo(object other) {

			// Special case: int32 and native int are the same in this
			// implementation (see ConvertInstructions.cs). Comparison
			// will only be signed with int32.
			if (other is Int4)
				return (int)((uint)m_value - ((Int4)other).ToUnsignedInt4(false).Value);
			return (int)((uint)m_value - (uint)((IntPointer)other).Value);
		}

		public override int GetHashCode() {

			return (int)m_value * 677;
		}

		public IntPointer(int i) {

			m_value = new System.IntPtr(i);
		}
	}

	struct ConstantString : IReferenceType {

		string m_value;

		public string WrapperName { get { return "System.String"; } }
		public string Value { get { return m_value; } }
        // public DataElementKind Kind => DataElementKind.String;

        // Should never be called.
        public uint Location
        {
            get
            {
                throw new Exception("query for location of constant string");
                //Logger.l.Warning("query for location of constant string");
                return 0;
            }
        }

		public bool ToBool() { return m_value != string.Empty; }

		public bool Equals(IDataElement other) {

			return (other is ConstantString) && ((ConstantString)other).Value == m_value;
		}

		public int CompareTo(object obj) {

			return m_value.CompareTo(((ConstantString)obj).Value);
		}

		public override string ToString() {

			return string.Format("\"{0}\"", m_value);
		}

		public override int GetHashCode() {

			return m_value.GetHashCode();
		}

		public ConstantString(string val) {

			m_value = val;
		}
	}

    public struct ObjectReference : IReferenceType {

		uint m_location;

		public static readonly ObjectReference Null = new ObjectReference(0);

        // public DataElementKind Kind => DataElementKind.Type;

        public string WrapperName { get { return ""; } }

		public uint Location {

			get { return m_location; }
		}

		public bool ToBool() { return m_location != 0; }

		public int CompareTo(object obj) {

			return (int)(m_location - ((ObjectReference)obj).Location);
		}

		public bool Equals(IDataElement other) {

			return (other is ObjectReference) &&
				((ObjectReference)other).Location == m_location;
		}

		public override string ToString() {
			if (this.Equals(ObjectReference.Null))
				return "null";
			else
				return "Alloc(" + Location + ")";
		}

		public override int GetHashCode() {

			return (int)(m_location);
		}

		public ObjectReference(uint loc) {

			m_location = loc;
		}
		public ObjectReference(int loc) : this((uint)loc) { }
	}

    abstract class MethodMemberPointer : IManagedPointer
    {
        protected MethodState m_method;
        protected int m_index;

        public string WrapperName { get { return ""; } }

        public int Index
        {
            get { return m_index; }
        }

        public MethodState MethodState
        {
            get { return m_method; }
        }

        public bool ToBool() { return Value != null; }

        public int CompareTo(object a)
        {
            return m_index - ((MethodMemberPointer)a).Index;
        }

        public Int4 ToInt4()
        {
            return new Int4(m_index);
        }

        public abstract override int GetHashCode();
        public abstract IDataElement Value { get; set; }
        public abstract bool Equals(IDataElement obj);

        public MethodMemberPointer(MethodState method, int index)
        {
            m_method = method;
            m_index = index;
        }
    }


	class LocalVariablePointer : MethodMemberPointer
    {
        public override IDataElement Value
        {
            get { return m_method.Locals[m_index]; }
            set { m_method.Locals[m_index] = value; }
        }

		public override string ToString() {
            var lv = m_method.Locals[m_index];
            return "LV^" + lv != null ? lv.ToString() : "null";
		}

		public override int GetHashCode() {
			return MMC.Collections.SingleIntHasher.Hash(m_index) ^ HashMasks.MASK1;
		}

		public override bool Equals(IDataElement obj) {
			bool retval = false;

			if (obj is LocalVariablePointer) {
				LocalVariablePointer lv = obj as LocalVariablePointer;
				retval = lv.m_index == this.m_index && lv.m_method.Equals(this.m_method);
			}

			return retval;
		}

		public LocalVariablePointer(MethodState method, int index) : base(method, index) { }
	}

	class ArgumentPointer : MethodMemberPointer
    {
        public override IDataElement Value
        {
			get { return m_method.Arguments[m_index]; }
			set { m_method.Arguments[m_index] = value; }
		}

        public override string ToString()
        {
            var ap = m_method.Arguments[m_index];
            return "AP^" + ap != null ? ap.ToString() : "null";
        }

		public override bool Equals(IDataElement obj) {
			bool retval = false;

			if (obj is ArgumentPointer) {
				ArgumentPointer lv = obj as ArgumentPointer;
				retval = lv.m_index == this.m_index && lv.m_method.Equals(this.m_method);
			}

			return retval;
		}

		public override int GetHashCode() {
			return MMC.Collections.SingleIntHasher.Hash(m_index) ^ HashMasks.MASK2;
		}

		public ArgumentPointer(MethodState method, int index)
			: base(method, index) { }
	}

	class ObjectFieldPointer : IManagedPointer {

        private readonly ObjectReference m_objectRef;
		private readonly int m_index;
        private readonly ExplicitActiveState cur;

        public ObjectFieldPointer(ExplicitActiveState cur, ObjectReference or, int i) {
			m_index = i;
			m_objectRef = or;
            this.cur = cur;
            MemoryLocation = new MemoryLocation(m_index, m_objectRef, cur);
        }

		public string WrapperName { get { return ""; } }

		public Int4 ToInt4() {
			return new Int4(m_index);
		}

        public MemoryLocation MemoryLocation { get; }

        public IDataElement Value
        {
            get
            {
                AllocatedObject ao = cur.DynamicArea.Allocations[m_objectRef] as AllocatedObject;
                return ao.Fields[m_index];
            }
            set
            {
                AllocatedObject ao = cur.DynamicArea.Allocations[m_objectRef] as AllocatedObject;
                ObjectEscapePOR.UpdateReachability(true, ao.Fields[m_index], value, cur);
                ao.Fields[m_index] = value;
            }
        }

		public bool ToBool() { return !m_objectRef.Equals(ObjectReference.Null); }

		public int CompareTo(object obj) {
			return m_index - ((ObjectFieldPointer)obj).m_index;
		}

		public bool Equals(IDataElement other) {
			bool retval = false;

			if (other is ObjectFieldPointer) {
				ObjectFieldPointer otherP = other as ObjectFieldPointer;
				retval = m_objectRef.Equals(otherP.m_objectRef) && m_index == otherP.m_index;
			}

			return retval;
		}

		public override string ToString() {
			return "OFP^" + m_objectRef.ToString() + "." + m_index;
		}

		public override int GetHashCode() {
			return MMC.Collections.SingleIntHasher.Hash(m_index) ^ HashMasks.MASK3;
		}
	}
	
	/// ArrayElemPointers are now covered by object field pointers
	/*
	class ArrayElementPointer : IManagedPointer {

		ObjectReference m_objectRef;
		int m_index;

		public ArrayElementPointer(ObjectReference or, int i) {
			m_index = i;
			m_objectRef = or;
		}

		public Int4 ToInt4() {
			return new Int4(m_index);
		}

		public IDataElement Value {
			get {
				AllocatedArray ao = cur.DynamicArea.Allocations[m_objectRef] as AllocatedArray;
				return ao.Fields[m_index];
			}

			set {
				AllocatedArray ao = cur.DynamicArea.Allocations[m_objectRef] as AllocatedArray;
				ao.Fields[m_index] = value;
			}
		}

		public string WrapperName { get { return ""; } }

		public bool ToBool() { return !m_objectRef.Equals(ObjectReference.Null); }

		public int CompareTo(object obj) {
			return m_index - ((ArrayElementPointer)obj).m_index;
		}

		public bool Equals(IDataElement other) {
			bool retval = false;

			if (other is ArrayElementPointer) {
				ArrayElementPointer otherP = other as ArrayElementPointer;
				retval = m_objectRef.Equals(otherP.m_objectRef) && m_index == otherP.m_index;
			}

			return retval;
		}

		public override string ToString() {
			return "AEP^" + m_objectRef.ToString() + "." + m_index;
		}

		public override int GetHashCode() {
			return MMC.Collections.CyclicHasher.WangHash(m_index) ^ HashMasks.MASK4;
		}
	}*/


	class StaticFieldPointer : IManagedPointer, IAddElement, ISubElement {

        TypeDefinition m_type;
		int m_index;
        private readonly ExplicitActiveState cur;

        public StaticFieldPointer(ExplicitActiveState cur, TypeDefinition or, int i)
        {
            m_index = i;
            m_type = or;
            this.cur = cur;
        }

		public Int4 ToInt4()
        {
			return new Int4(m_index);
		}

		public IAddElement Add(INumericElement a, bool checkOverflow)
        {
			AllocatedClass ac = cur.StaticArea.GetClass(m_type);
			/*
			 * Note: the offset added to this pointer is in the amount of bytes.
			 * The offset is dependent on the size of the datatype of the fields.
			 * If for example, there are three fields:
			 * int a;
			 * byte b;
			 * double f;
			 * 
			 * Then a has offset 0, b has offset 4, f has offset 5. In MMC however,
			 * a would be indexed to 0, b to 1 and f to 2. We have to recalculate the 
			 * index based from the byte offset
			 * 
			 * We have only made this to work for all types defined in System. This
			 * does not work on structs etc. It is useless to do this on structs anyway,
			 * because there is no sense of its internal structure.
			 * 
			 * BTW, we only do (and allow) this to pass a few more Microsoft's IL_BVT tests :)
			 */
			int i = 0;
			int byteOffset = a.ToInt4(false).Value;

			var typeDef = DefinitionProvider.GetTypeDefinition(ac.Type);
            foreach (var fld in typeDef.Fields)
            {
                if (i >= fld.FieldOffset)
                {
                    byteOffset -= cur.DefinitionProvider.SizeOf(fld.FieldType.FullName);
                }

                i++;

                if (byteOffset == 0)
                    break;
            }

			return new StaticFieldPointer(cur, m_type, i);
		}

		public MemoryLocation MemoryLocation
        {
            get { return new MemoryLocation(m_index, m_type, cur); }
        }

		public ISubElement Sub(INumericElement a, bool checkOverflow)
        {
			throw new System.InvalidOperationException("Sub ptr manipulation not implemented (yet)");
		}

		public IDataElement Value
        {
            get
            {
                AllocatedClass ac = cur.StaticArea.GetClass(m_type);
                return ac.Fields[m_index];
            }
            set
            {
                AllocatedClass ac = cur.StaticArea.GetClass(m_type);
                ObjectEscapePOR.UpdateReachability(true, ac.Fields[m_index], value, cur);
                ac.Fields[m_index] = value;
            }
        }

		public string WrapperName { get { return ""; } }

		public bool ToBool() { return m_type != null; }

		public int CompareTo(object obj) {
			return m_index - ((StaticFieldPointer)obj).m_index;
		}

		public bool Equals(IDataElement other) {
			bool retval = false;

			if (other is StaticFieldPointer) {
				StaticFieldPointer otherP = other as StaticFieldPointer;
				retval = m_type.FullName.Equals(otherP.m_type.FullName) && m_index == otherP.m_index;
			}

			return retval;
		}

		public override string ToString() {
			return "SFP^" + m_type.FullName + "." + m_index;
		}

		public override int GetHashCode() {
			return MMC.Collections.SingleIntHasher.Hash(m_index) ^ HashMasks.MASK5;
		}
	}



	struct TypePointer : IRuntimeHandle {

		ITypeDefOrRef m_typeDef;

		// maybe "System.Type"?
		public string WrapperName { get { return ""; } }

		public ITypeDefOrRef Type {

			get { return m_typeDef; }
		}

		public bool ToBool() { return false; }

		public bool Equals(IDataElement other) {

			return (other is TypePointer) && ((TypePointer)other).Type == m_typeDef;
		}

		public int CompareTo(object other) {

			TypePointer o = (TypePointer)other;
			return Type.Name.CompareTo(o.Type.Name);
		}

		public override string ToString() {

			return Type.Name;
		}

		public override int GetHashCode() {

			return (int)(m_typeDef.GetHashCode() * 2311);
		}

		public TypePointer(ITypeDefOrRef typeDef) {

			m_typeDef = typeDef;
		}
	}

    public struct MethodPointer : IRuntimeHandle {

		MethodDefinition m_method;

		public string WrapperName { get { return ""; } }

		public MethodDefinition Value {

			get { return m_method; }
			set { m_method = value; }
		}

		public bool ToBool() { return Value != null; }

		public bool Equals(IDataElement other) {

			return (other is MethodPointer) && ((MethodPointer)other).Value == Value;
		}

		public override string ToString() {

			return "MP^" + (m_method != null ? Value.Name.String : "null");
		}

		public int CompareTo(object other) {

			return Value.Name.CompareTo(((MethodPointer)other).Value.Name);
		}

		public override int GetHashCode() {

			return (int)(m_method.GetHashCode() * 3329);
		}

		public MethodPointer(MethodDefinition method) {

			m_method = method;
		}
	}
}
