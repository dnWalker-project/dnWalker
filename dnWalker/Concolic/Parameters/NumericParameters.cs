using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Expressions = System.Linq.Expressions;

namespace dnWalker.Concolic.Parameters
{

    public abstract class PrimitiveValueParameter : Parameter
    {
        protected PrimitiveValueParameter(String typeName) : base(typeName)
        {
        }

        protected PrimitiveValueParameter(String typeName, IEnumerable<ParameterTrait> traits) : base(typeName, traits)
        {
        }

    }

    public abstract class PrimitiveValueParameter<TValue> : PrimitiveValueParameter where TValue : struct
    {
        protected PrimitiveValueParameter(String typeName) : base(typeName)
        { }

        protected PrimitiveValueParameter(TValue value, String typeName) : base(typeName, new ParameterTrait[] { new ValueTrait<TValue>(value) })
        { }

        public TValue? Value
        {
            get
            {
                if (TryGetTrait<ValueTrait<TValue>>(out ValueTrait<TValue> trait))
                {
                    // value was set
                    return trait.Value;
                }
                else
                {
                    // dont care
                    return null;
                }
            }
            set
            {
                if (value.HasValue)
                {
                    // we want to explicitly set the value
                    if (TryGetTrait<ValueTrait<TValue>>(out ValueTrait<TValue> trait))
                    {
                        trait.Value = value.Value;
                    }
                    else
                    {
                        AddTrait<ValueTrait<TValue>>(new ValueTrait<TValue>(value.Value));
                    }
                }
                else
                {
                    // we want to clear the value
                    if (TryGetTrait<ValueTrait<TValue>>(out ValueTrait<TValue> trait))
                    {
                        Traits.Remove(trait);
                    }
                }
            }
        }

        protected override Type GetFrameworkType()
        {
            return typeof(TValue);
        }

        public override String ToString()
        {
            if (TryGetTrait<ValueTrait>(out ValueTrait valueTrait))
            {
                return $"Parameter: {Name}, Type: {TypeName}, Value: {valueTrait.Value}";
            }
            else
            {
                return $"Parameter: {Name}, Type: {TypeName}, Value: DEFAULT";
            }
            
        }
    }

    public class BooleanParameter : PrimitiveValueParameter<Boolean>
    {
        public BooleanParameter() : base(TypeNames.BooleanTypeName)
        {
        }

        public BooleanParameter(Boolean value) : base(value, TypeNames.BooleanTypeName)
        {
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            IDataElement element = new Int4((Value.HasValue && Value.Value) ? 1 : 0 );
            element.SetParameter(this, cur);
            return element;
        }
    }

    public class CharParameter : PrimitiveValueParameter<Char>
    {
        public CharParameter() : base(TypeNames.CharTypeName)
        {
        }

        public CharParameter(Char value) : base(value, TypeNames.CharTypeName)
        {
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            IDataElement element = new Int4(Value.HasValue ? Value.Value : 0);
            element.SetParameter(this, cur);
            return element;
        }
    }

    public class ByteParameter : PrimitiveValueParameter<Byte>
    {
        public ByteParameter() : base(TypeNames.ByteTypeName)
        {
        }

        public ByteParameter(Byte value) : base(value, TypeNames.ByteTypeName)
        {
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            IDataElement element = new Int4(Value.HasValue ? Value.Value : 0);
            element.SetParameter(this, cur);
            return element;
        }
    }

    public class SByteParameter : PrimitiveValueParameter<SByte>
    {
        public SByteParameter() : base(TypeNames.SByteTypeName)
        {
        }

        public SByteParameter(SByte value) : base(value, TypeNames.SByteTypeName)
        {
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            IDataElement element = new Int4(Value.HasValue ? Value.Value : 0);
            element.SetParameter(this, cur);
            return element;
        }
    }

    public class Int16Parameter : PrimitiveValueParameter<Int16>
    {
        public Int16Parameter() : base(TypeNames.Int16TypeName)
        {
        }

        public Int16Parameter(Int16 value) : base(value, TypeNames.Int16TypeName)
        {
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            IDataElement element = new Int4(Value.HasValue ? Value.Value : 0);
            element.SetParameter(this, cur);
            return element;
        }
    }

    public class Int32Parameter : PrimitiveValueParameter<Int32>
    {
        public Int32Parameter() : base(TypeNames.Int32TypeName)
        {
        }

        public Int32Parameter(Int32 value) : base(value, TypeNames.Int32TypeName)
        {
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            IDataElement element = new Int4(Value.HasValue ? Value.Value : 0);
            element.SetParameter(this, cur);
            return element;
        }
    }

    public class Int64Parameter : PrimitiveValueParameter<Int64>
    {
        public Int64Parameter() : base(TypeNames.Int64TypeName)
        {
        }

        public Int64Parameter(Int64 value) : base(value, TypeNames.Int64TypeName)
        {
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            IDataElement element = new Int8(Value.HasValue ? Value.Value : 0);
            element.SetParameter(this, cur);
            return element;
        }
    }

    public class UInt16Parameter : PrimitiveValueParameter<UInt16>
    {
        public UInt16Parameter() : base(TypeNames.UInt16TypeName)
        {
        }

        public UInt16Parameter(UInt16 value) : base(value, TypeNames.UInt16TypeName)
        {
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            IDataElement element = new UnsignedInt4(Value.HasValue ? Value.Value : 0U);
            element.SetParameter(this, cur);
            return element;
        }
    }

    public class UInt32Parameter : PrimitiveValueParameter<UInt32>
    {
        public UInt32Parameter() : base(TypeNames.UInt32TypeName)
        {
        }

        public UInt32Parameter(UInt32 value) : base(value, TypeNames.UInt32TypeName)
        {
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            IDataElement element = new UnsignedInt4(Value.HasValue ? Value.Value : 0U);
            element.SetParameter(this, cur);
            return element;
        }
    }

    public class UInt64Parameter : PrimitiveValueParameter<UInt64>
    {
        public UInt64Parameter() : base(TypeNames.UInt64TypeName)
        {
        }

        public UInt64Parameter(UInt64 value) : base(value, TypeNames.UInt64TypeName)
        {
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            IDataElement element = new UnsignedInt8(Value.HasValue ? Value.Value : 0);
            element.SetParameter(this, cur);
            return element;
        }
    }

    public class SingleParameter : PrimitiveValueParameter<Single>
    {
        public SingleParameter() : base(TypeNames.ByteTypeName)
        {
        }

        public SingleParameter(Single value) : base(value, TypeNames.SingleTypeName)
        {
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            IDataElement element = new Float4(Value.HasValue ? Value.Value : 0.0f);
            element.SetParameter(this, cur);
            return element;
        }
    }

    public class DoubleParameter : PrimitiveValueParameter<Double>
    {
        public DoubleParameter() : base(TypeNames.DoubleTypeName)
        {
        }

        public DoubleParameter(Double value) : base(value, TypeNames.DoubleTypeName)
        {
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            IDataElement element = new Float8(Value.HasValue ? Value.Value : 0.0d);
            element.SetParameter(this, cur);
            return element;
        }
    }
}
