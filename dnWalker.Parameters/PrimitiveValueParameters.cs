using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace dnWalker.Parameters
{

    public abstract class PrimitiveValueParameter : Parameter
    {
        protected PrimitiveValueParameter(string typeName, string name) : base(typeName, name)
        {
        }
        protected PrimitiveValueParameter(string typeName, string name, Parameter? owner) : base(typeName, name, owner)
        {
        }

        public abstract object GetValue();

        public override IEnumerable<Parameter> GetOwnedParameters()
        {
            return Enumerable.Empty<Parameter>();
        }
    }

    public abstract class PrimitiveValueParameter<TValue> : PrimitiveValueParameter where TValue : struct
    {
        protected PrimitiveValueParameter(string typeName, string name) : base(typeName, name)
        {
        }

        protected PrimitiveValueParameter(string typeName, string name, Parameter? owner) : base(typeName, name, owner)
        { }

        protected PrimitiveValueParameter(string typeName, string name, TValue value) : base(typeName, name)
        {
            Value = value;
        }
        protected PrimitiveValueParameter(string typeName, string name, TValue value, Parameter? owner) : base(typeName, name, owner)
        {
            Value = value;
        }

        public override object GetValue()
        {
            return Value;
        }

        public TValue Value
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"Parameter: {LocalName}, Type: {TypeName}, Value: {Value}";
        }
    }

    public class BooleanParameter : PrimitiveValueParameter<bool>
    {
        public BooleanParameter(string name) : base(TypeNames.BooleanTypeName, name)
        {
        }

        public BooleanParameter(string name, Parameter? owner) : base(TypeNames.BooleanTypeName, name, owner)
        { }

        public BooleanParameter(string name, Boolean value) : base(TypeNames.BooleanTypeName, name)
        {
            Value = value;
        }
        public BooleanParameter(string name, Boolean value, Parameter? owner) : base(TypeNames.BooleanTypeName, name, owner)
        {
            Value = value;
        }
    }

    public class CharParameter : PrimitiveValueParameter<char>
    {
        public CharParameter(string name) : base(TypeNames.CharTypeName, name)
        {
        }

        public CharParameter(string name, Parameter? owner) : base(TypeNames.CharTypeName, name, owner)
        {
        }

        public CharParameter(string name, char value) : base(TypeNames.CharTypeName, name, value)
        {
        }

        public CharParameter(string name, char value, Parameter? owner) : base(TypeNames.CharTypeName, name, value, owner)
        {
        }
    }

    public class ByteParameter : PrimitiveValueParameter<byte>
    {
        public ByteParameter(string name) : base(TypeNames.ByteTypeName, name)
        {
        }

        public ByteParameter(string name, Parameter? owner) : base(TypeNames.ByteTypeName, name, owner)
        {
        }

        public ByteParameter(string name, Byte value) : base(TypeNames.ByteTypeName, name, value)
        {
        }

        public ByteParameter(string name, Byte value, Parameter? owner) : base(TypeNames.ByteTypeName, name, value, owner)
        {
        }
    }

    public class SByteParameter : PrimitiveValueParameter<sbyte>
    {
        public SByteParameter(string name) : base(TypeNames.SByteTypeName, name)
        {
        }

        public SByteParameter(string name, Parameter? owner) : base(TypeNames.SByteTypeName, name, owner)
        {
        }

        public SByteParameter(string name, sbyte value) : base(TypeNames.SByteTypeName, name, value)
        {
        }

        public SByteParameter(string name, sbyte value, Parameter? owner) : base(TypeNames.SByteTypeName, name, value, owner)
        {
        }
    }

    public class Int16Parameter : PrimitiveValueParameter<short>
    {
        public Int16Parameter(string name) : base(TypeNames.Int16TypeName, name)
        {
        }

        public Int16Parameter(string name, Parameter? owner) : base(TypeNames.Int16TypeName, name, owner)
        {
        }

        public Int16Parameter(string name, short value) : base(TypeNames.Int16TypeName, name, value)
        {
        }

        public Int16Parameter(string name, short value, Parameter? owner) : base(TypeNames.Int16TypeName, name, value, owner)
        {
        }
    }

    public class Int32Parameter : PrimitiveValueParameter<int>
    {
        public Int32Parameter(string name) : base(TypeNames.Int32TypeName, name)
        {
        }

        public Int32Parameter(string name, Parameter? owner) : base(TypeNames.Int32TypeName, name, owner)
        {
        }

        public Int32Parameter(string name, int value) : base(TypeNames.Int32TypeName, name, value)
        {
        }

        public Int32Parameter(string name, int value, Parameter? owner) : base(TypeNames.Int32TypeName, name, value, owner)
        {
        }
    }

    public class Int64Parameter : PrimitiveValueParameter<long>
    {
        public Int64Parameter(string name) : base(TypeNames.Int64TypeName, name)
        {
        }

        public Int64Parameter(string name, Parameter? owner) : base(TypeNames.Int64TypeName, name, owner)
        {
        }

        public Int64Parameter(string name, long value) : base(TypeNames.Int64TypeName, name, value)
        {
        }

        public Int64Parameter(string name, long value, Parameter? owner) : base(TypeNames.Int64TypeName, name, value, owner)
        {
        }
    }

    public class UInt16Parameter : PrimitiveValueParameter<ushort>
    {
        public UInt16Parameter(string name) : base(TypeNames.UInt16TypeName, name)
        {
        }

        public UInt16Parameter(string name, Parameter? owner) : base(TypeNames.UInt16TypeName, name, owner)
        {
        }

        public UInt16Parameter(string name, ushort value) : base(TypeNames.UInt16TypeName, name, value)
        {
        }

        public UInt16Parameter(string name, ushort value, Parameter? owner) : base(TypeNames.UInt16TypeName, name, value, owner)
        {
        }
    }

    public class UInt32Parameter : PrimitiveValueParameter<uint>
    {
        public UInt32Parameter(string name) : base(TypeNames.UInt32TypeName, name)
        {
        }

        public UInt32Parameter(string name, Parameter? owner) : base(TypeNames.UInt32TypeName, name, owner)
        {
        }

        public UInt32Parameter(string name, uint value) : base(TypeNames.UInt32TypeName, name, value)
        {
        }

        public UInt32Parameter(string name, uint value, Parameter? owner) : base(TypeNames.UInt32TypeName, name, value, owner)
        {
        }
    }

    public class UInt64Parameter : PrimitiveValueParameter<ulong>
    {
        public UInt64Parameter(string name) : base(TypeNames.UInt64TypeName, name)
        {
        }

        public UInt64Parameter(string name, Parameter? owner) : base(TypeNames.UInt64TypeName, name, owner)
        {
        }

        public UInt64Parameter(string name, ulong value) : base(TypeNames.UInt64TypeName, name, value)
        {
        }

        public UInt64Parameter(string name, ulong value, Parameter? owner) : base(TypeNames.UInt64TypeName, name, value, owner)
        {
        }
    }

    public class SingleParameter : PrimitiveValueParameter<float>
    {
        public SingleParameter(string name) : base(TypeNames.SingleTypeName, name)
        { 
        }

        public SingleParameter(string name, Parameter? owner) : base(TypeNames.SingleTypeName, name, owner)
        {
        }
        public SingleParameter(string name, float value) : base(TypeNames.SingleTypeName, name, value)
        {
        }

        public SingleParameter(string name, float value, Parameter? owner) : base(TypeNames.SingleTypeName, name, value, owner)
        {
        }
    }

    public class DoubleParameter : PrimitiveValueParameter<double>
    {
        public DoubleParameter(string name) : base(TypeNames.DoubleTypeName, name)
        {
        }

        public DoubleParameter(string name, Parameter? owner) : base(TypeNames.DoubleTypeName, name, owner)
        {
        }
        public DoubleParameter(string name, double value) : base(TypeNames.DoubleTypeName, name, value)
        {
        }

        public DoubleParameter(string name, double value, Parameter? owner) : base(TypeNames.DoubleTypeName, name, value, owner)
        {
        }
    }
}
