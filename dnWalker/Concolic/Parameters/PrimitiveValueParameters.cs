﻿using MMC.Data;
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
        protected PrimitiveValueParameter(string typeName) : base(typeName)
        {
        }
        protected PrimitiveValueParameter(string typeName, string name) : base(typeName, name)
        {
        }
    }

    public abstract class PrimitiveValueParameter<TValue> : PrimitiveValueParameter where TValue : struct
    {
        protected PrimitiveValueParameter(string typeName) : base(typeName)
        {
        }

        protected PrimitiveValueParameter(string typeName, string name) : base(typeName, name)
        { }

        protected PrimitiveValueParameter(string typeName, string name, TValue value) : base(typeName, name)
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

        public override string ToString()
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

        public override bool TryGetChildParameter(string name, out Parameter childParameter)
        {
            childParameter = null;
            return false;
        }

        private ParameterExpression _expression;

        public override IEnumerable<ParameterExpression> GetParameterExpressions()
        {
            yield return GetSingleParameterExpression();
        }

        public override bool HasSingleExpression => true;
        
        public override ParameterExpression GetSingleParameterExpression()
        {
            if (_expression == null)
            {
                _expression = Expression.Parameter(typeof(TValue), Name);
            }
            return _expression;
        }

        protected override void OnNameChanged(string newName)
        {
            base.OnNameChanged(newName);
            _expression = null;
        }
    }

    public class BooleanParameter : PrimitiveValueParameter<bool>
    {
        public BooleanParameter() : base(TypeNames.BooleanTypeName)
        {
        }

        public BooleanParameter(string name) : base(TypeNames.BooleanTypeName, name)
        {
        }

        public BooleanParameter(string name, bool value) : base(TypeNames.BooleanTypeName, name, value)
        {
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            IDataElement element = new Int4((Value.HasValue && Value.Value) ? 1 : 0 );
            element.SetParameter(this, cur);
            return element;
        }
    }

    public class CharParameter : PrimitiveValueParameter<char>
    {
        public CharParameter() : base(TypeNames.CharTypeName)
        {
        }

        public CharParameter(string name) : base(TypeNames.CharTypeName, name)
        {
        }

        public CharParameter(string name, char value) : base(TypeNames.CharTypeName, name, value)
        {
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            IDataElement element = new Int4(Value.HasValue ? Value.Value : 0);
            element.SetParameter(this, cur);
            return element;
        }
    }

    public class ByteParameter : PrimitiveValueParameter<byte>
    {
        public ByteParameter() : base(TypeNames.ByteTypeName)
        {
        }

        public ByteParameter(string name) : base(TypeNames.ByteTypeName, name)
        {
        }

        public ByteParameter(string name, byte value) : base(TypeNames.ByteTypeName, name, value)
        {
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            IDataElement element = new Int4(Value.HasValue ? Value.Value : 0);
            element.SetParameter(this, cur);
            return element;
        }
    }

    public class SByteParameter : PrimitiveValueParameter<sbyte>
    {
        public SByteParameter() : base(TypeNames.SByteTypeName)
        {
        }

        public SByteParameter(string name) : base(TypeNames.SByteTypeName, name)
        {
        }

        public SByteParameter(string name, sbyte value) : base(TypeNames.SByteTypeName, name, value)
        {
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            IDataElement element = new Int4(Value.HasValue ? Value.Value : 0);
            element.SetParameter(this, cur);
            return element;
        }
    }

    public class Int16Parameter : PrimitiveValueParameter<short>
    {
        public Int16Parameter() : base(TypeNames.Int16TypeName)
        {
        }

        public Int16Parameter(string name) : base(TypeNames.Int16TypeName, name)
        {
        }

        public Int16Parameter(string name, short value) : base(TypeNames.Int16TypeName, name, value)
        {
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            IDataElement element = new Int4(Value.HasValue ? Value.Value : 0);
            element.SetParameter(this, cur);
            return element;
        }
    }

    public class Int32Parameter : PrimitiveValueParameter<int>
    {
        public Int32Parameter() : base(TypeNames.Int32TypeName)
        {
        }

        public Int32Parameter(string name) : base(TypeNames.Int32TypeName, name)
        {
        }

        public Int32Parameter(string name, int value) : base(TypeNames.Int32TypeName, name, value)
        {
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            IDataElement element = new Int4(Value.HasValue ? Value.Value : 0);
            element.SetParameter(this, cur);
            return element;
        }
    }

    public class Int64Parameter : PrimitiveValueParameter<long>
    {
        public Int64Parameter() : base(TypeNames.Int64TypeName)
        {
        }

        public Int64Parameter(string name) : base(TypeNames.Int64TypeName, name)
        {
        }

        public Int64Parameter(string name, long value) : base(TypeNames.Int64TypeName, name, value)
        {
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            IDataElement element = new Int8(Value.HasValue ? Value.Value : 0);
            element.SetParameter(this, cur);
            return element;
        }
    }

    public class UInt16Parameter : PrimitiveValueParameter<ushort>
    {
        public UInt16Parameter() : base(TypeNames.UInt16TypeName)
        {
        }

        public UInt16Parameter(string name) : base(TypeNames.UInt16TypeName, name)
        {
        }

        public UInt16Parameter(string name, ushort value) : base(TypeNames.UInt16TypeName, name, value)
        {
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            IDataElement element = new UnsignedInt4(Value.HasValue ? Value.Value : 0U);
            element.SetParameter(this, cur);
            return element;
        }
    }

    public class UInt32Parameter : PrimitiveValueParameter<uint>
    {
        public UInt32Parameter() : base(TypeNames.UInt32TypeName)
        {
        }

        public UInt32Parameter(string name) : base(TypeNames.UInt32TypeName, name)
        {
        }

        public UInt32Parameter(string name, uint value) : base(TypeNames.UInt32TypeName, name, value)
        {
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            IDataElement element = new UnsignedInt4(Value.HasValue ? Value.Value : 0U);
            element.SetParameter(this, cur);
            return element;
        }
    }

    public class UInt64Parameter : PrimitiveValueParameter<ulong>
    {
        public UInt64Parameter() : base(TypeNames.UInt64TypeName)
        {
        }

        public UInt64Parameter(string name) : base(TypeNames.UInt64TypeName, name)
        {
        }

        public UInt64Parameter(string name, ulong value) : base(TypeNames.UInt64TypeName, name, value)
        {
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            IDataElement element = new UnsignedInt8(Value.HasValue ? Value.Value : 0);
            element.SetParameter(this, cur);
            return element;
        }
    }

    public class SingleParameter : PrimitiveValueParameter<float>
    {
        public SingleParameter() : base(TypeNames.SingleTypeName)
        {
        }

        public SingleParameter(string name) : base(TypeNames.SingleTypeName, name)
        {
        }

        public SingleParameter(string name, float value) : base(TypeNames.SingleTypeName, name, value)
        {
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            IDataElement element = new Float4(Value.HasValue ? Value.Value : 0.0f);
            element.SetParameter(this, cur);
            return element;
        }
    }

    public class DoubleParameter : PrimitiveValueParameter<double>
    {
        public DoubleParameter() : base(TypeNames.DoubleTypeName)
        {
        }

        public DoubleParameter(string name) : base(TypeNames.DoubleTypeName, name)
        {
        }

        public DoubleParameter(string name, double value) : base(TypeNames.DoubleTypeName, name, value)
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
