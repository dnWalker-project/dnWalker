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
        protected PrimitiveValueParameter(String typeName, String name) : base(typeName, name)
        {
        }
    }

    public abstract class PrimitiveValueParameter<TValue> : PrimitiveValueParameter where TValue : struct
    {
        protected PrimitiveValueParameter(String typeName) : base(typeName)
        {
        }

        protected PrimitiveValueParameter(String typeName, String name) : base(typeName, name)
        { }

        protected PrimitiveValueParameter(String typeName, String name, TValue value) : base(typeName, name)
        {
            Value = value;
        }


        public TValue? Value
        {
            get;
            set;
            //get
            //{
            //    if (TryGetTrait<ValueTrait<TValue>>(out ValueTrait<TValue> trait))
            //    {
            //        // value was set
            //        return trait.Value;
            //    }
            //    else
            //    {
            //        // dont care
            //        return null;
            //    }
            //}
            //set
            //{
            //    if (value.HasValue)
            //    {
            //        // we want to explicitly set the value
            //        if (TryGetTrait<ValueTrait<TValue>>(out ValueTrait<TValue> trait))
            //        {
            //            trait.Value = value.Value;
            //        }
            //        else
            //        {
            //            AddTrait<ValueTrait<TValue>>(new ValueTrait<TValue>(value.Value));
            //        }
            //    }
            //    else
            //    {
            //        // we want to clear the value
            //        if (TryGetTrait<ValueTrait<TValue>>(out ValueTrait<TValue> trait))
            //        {
            //            Traits.Remove(trait);
            //        }
            //    }
            //}
        }

        public override String ToString()
        {
            if (Value.HasValue)
            {
                return $"Parameter: {Name}, Type: {TypeName}, Value: {Value.Value}";
            }
            else
            {
                return $"Parameter: {Name}, Type: {TypeName}, Value: DEFAULT";
            }
            
        }

        public override Boolean TryGetChildParameter(String name, out Parameter childParameter)
        {
            childParameter = null;
            return false;
        }

        private ParameterExpression _expression;

        public override IEnumerable<ParameterExpression> GetParameterExpressions()
        {
            yield return GetSingleParameterExpression();
        }

        public override Boolean HasSingleExpression => true;
        
        public override ParameterExpression GetSingleParameterExpression()
        {
            if (_expression == null)
            {
                _expression = Expression.Parameter(typeof(TValue), Name);
            }
            return _expression;
        }

        protected override void OnNameChanged(String newName)
        {
            base.OnNameChanged(newName);
            _expression = null;
        }
    }

    public class BooleanParameter : PrimitiveValueParameter<Boolean>
    {
        public BooleanParameter() : base(TypeNames.BooleanTypeName)
        {
        }

        public BooleanParameter(String name) : base(TypeNames.BooleanTypeName, name)
        {
        }

        public BooleanParameter(String name, Boolean value) : base(TypeNames.BooleanTypeName, name, value)
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

        public CharParameter(String name) : base(TypeNames.CharTypeName, name)
        {
        }

        public CharParameter(String name, Char value) : base(TypeNames.CharTypeName, name, value)
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

        public ByteParameter(String name) : base(TypeNames.ByteTypeName, name)
        {
        }

        public ByteParameter(String name, Byte value) : base(TypeNames.ByteTypeName, name, value)
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

        public SByteParameter(String name) : base(TypeNames.SByteTypeName, name)
        {
        }

        public SByteParameter(String name, SByte value) : base(TypeNames.SByteTypeName, name, value)
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

        public Int16Parameter(String name) : base(TypeNames.Int16TypeName, name)
        {
        }

        public Int16Parameter(String name, Int16 value) : base(TypeNames.Int16TypeName, name, value)
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

        public Int32Parameter(String name) : base(TypeNames.Int32TypeName, name)
        {
        }

        public Int32Parameter(String name, Int32 value) : base(TypeNames.Int32TypeName, name, value)
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

        public Int64Parameter(String name) : base(TypeNames.Int64TypeName, name)
        {
        }

        public Int64Parameter(String name, Int64 value) : base(TypeNames.Int64TypeName, name, value)
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

        public UInt16Parameter(String name) : base(TypeNames.UInt16TypeName, name)
        {
        }

        public UInt16Parameter(String name, UInt16 value) : base(TypeNames.UInt16TypeName, name, value)
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

        public UInt32Parameter(String name) : base(TypeNames.UInt32TypeName, name)
        {
        }

        public UInt32Parameter(String name, UInt32 value) : base(TypeNames.UInt32TypeName, name, value)
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

        public UInt64Parameter(String name) : base(TypeNames.UInt64TypeName, name)
        {
        }

        public UInt64Parameter(String name, UInt64 value) : base(TypeNames.UInt64TypeName, name, value)
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
        public SingleParameter() : base(TypeNames.SingleTypeName)
        {
        }

        public SingleParameter(String name) : base(TypeNames.SingleTypeName, name)
        {
        }

        public SingleParameter(String name, Single value) : base(TypeNames.SingleTypeName, name, value)
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

        public DoubleParameter(String name) : base(TypeNames.DoubleTypeName, name)
        {
        }

        public DoubleParameter(String name, Double value) : base(TypeNames.DoubleTypeName, name, value)
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
