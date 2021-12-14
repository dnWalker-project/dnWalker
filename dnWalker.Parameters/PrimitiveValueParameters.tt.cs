
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace dnWalker.Parameters
{

    public abstract class PrimitiveValueParameter<TValue> : Parameter, IPrimitiveValueParameter<TValue> where TValue : struct
    {
        protected PrimitiveValueParameter() : base(typeof(TValue).FullName!)
        { }

        protected PrimitiveValueParameter(TValue value) : base(typeof(TValue).FullName!)
        {
            Value = value;
        }
        protected PrimitiveValueParameter(TValue value, int id)  : base(typeof(TValue).FullName!, id)
        {
            Value = value;
        }

        public sealed override IEnumerable<IParameter> GetChildren()
        {
            return Enumerable.Empty<IParameter>();
        }

        public TValue Value
        {
            get;
            set;
        }
    }

    public class BooleanParameter : PrimitiveValueParameter<bool>, IBooleanParameter
    {
        public BooleanParameter() : base()
        { }
        public BooleanParameter(bool value) : base(value)
        { }
        public BooleanParameter(bool value, int id) : base(value, id)
        { }

        public override IParameter ShallowCopy(int id)
        {
            return new BooleanParameter(Value, id);
        }
    }
 
     public class CharParameter : PrimitiveValueParameter<char>, ICharParameter
    {
        public CharParameter() : base()
        { }
        public CharParameter(char value) : base(value)
        { }
        public CharParameter(char value, int id) : base(value, id)
        { }

        public override IParameter ShallowCopy(int id)
        {
            return new CharParameter(Value, id);
        }
    }
 
     public class ByteParameter : PrimitiveValueParameter<byte>, IByteParameter
    {
        public ByteParameter() : base()
        { }
        public ByteParameter(byte value) : base(value)
        { }
        public ByteParameter(byte value, int id) : base(value, id)
        { }

        public override IParameter ShallowCopy(int id)
        {
            return new ByteParameter(Value, id);
        }
    }
 
     public class SByteParameter : PrimitiveValueParameter<sbyte>, ISByteParameter
    {
        public SByteParameter() : base()
        { }
        public SByteParameter(sbyte value) : base(value)
        { }
        public SByteParameter(sbyte value, int id) : base(value, id)
        { }

        public override IParameter ShallowCopy(int id)
        {
            return new SByteParameter(Value, id);
        }
    }
 
     public class Int16Parameter : PrimitiveValueParameter<short>, IInt16Parameter
    {
        public Int16Parameter() : base()
        { }
        public Int16Parameter(short value) : base(value)
        { }
        public Int16Parameter(short value, int id) : base(value, id)
        { }

        public override IParameter ShallowCopy(int id)
        {
            return new Int16Parameter(Value, id);
        }
    }
 
     public class Int32Parameter : PrimitiveValueParameter<int>, IInt32Parameter
    {
        public Int32Parameter() : base()
        { }
        public Int32Parameter(int value) : base(value)
        { }
        public Int32Parameter(int value, int id) : base(value, id)
        { }

        public override IParameter ShallowCopy(int id)
        {
            return new Int32Parameter(Value, id);
        }
    }
 
     public class Int64Parameter : PrimitiveValueParameter<long>, IInt64Parameter
    {
        public Int64Parameter() : base()
        { }
        public Int64Parameter(long value) : base(value)
        { }
        public Int64Parameter(long value, int id) : base(value, id)
        { }

        public override IParameter ShallowCopy(int id)
        {
            return new Int64Parameter(Value, id);
        }
    }
 
     public class UInt16Parameter : PrimitiveValueParameter<ushort>, IUInt16Parameter
    {
        public UInt16Parameter() : base()
        { }
        public UInt16Parameter(ushort value) : base(value)
        { }
        public UInt16Parameter(ushort value, int id) : base(value, id)
        { }

        public override IParameter ShallowCopy(int id)
        {
            return new UInt16Parameter(Value, id);
        }
    }
 
     public class UInt32Parameter : PrimitiveValueParameter<uint>, IUInt32Parameter
    {
        public UInt32Parameter() : base()
        { }
        public UInt32Parameter(uint value) : base(value)
        { }
        public UInt32Parameter(uint value, int id) : base(value, id)
        { }

        public override IParameter ShallowCopy(int id)
        {
            return new UInt32Parameter(Value, id);
        }
    }
 
     public class UInt64Parameter : PrimitiveValueParameter<ulong>, IUInt64Parameter
    {
        public UInt64Parameter() : base()
        { }
        public UInt64Parameter(ulong value) : base(value)
        { }
        public UInt64Parameter(ulong value, int id) : base(value, id)
        { }

        public override IParameter ShallowCopy(int id)
        {
            return new UInt64Parameter(Value, id);
        }
    }
 
     public class SingleParameter : PrimitiveValueParameter<float>, ISingleParameter
    {
        public SingleParameter() : base()
        { }
        public SingleParameter(float value) : base(value)
        { }
        public SingleParameter(float value, int id) : base(value, id)
        { }

        public override IParameter ShallowCopy(int id)
        {
            return new SingleParameter(Value, id);
        }
    }
 
     public class DoubleParameter : PrimitiveValueParameter<double>, IDoubleParameter
    {
        public DoubleParameter() : base()
        { }
        public DoubleParameter(double value) : base(value)
        { }
        public DoubleParameter(double value, int id) : base(value, id)
        { }

        public override IParameter ShallowCopy(int id)
        {
            return new DoubleParameter(Value, id);
        }
    }
 
 }