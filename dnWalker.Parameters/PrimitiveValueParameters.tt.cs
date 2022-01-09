using System;
using System.Linq;

namespace dnWalker.Parameters
{

    public class BooleanParameter : Parameter, IBooleanParameter
    {
        internal BooleanParameter(IParameterContext context) : base(context)
        {
        }

        internal BooleanParameter(IParameterContext context, ParameterRef reference) : base(context, reference)
        {
        }
        
        internal BooleanParameter(IParameterContext context, ParameterRef reference, bool? value) : base(context, reference)
        {
            Value = value;
        }

        public bool? Value
        {
            get;
            set;
        }

        public override BooleanParameter CloneData(IParameterContext context)
        {
             BooleanParameter parameter = new BooleanParameter(context, Reference, Value);
             

            // foreach (var a in Accessors.Select(ac => ac.Clone()))
            // {
            //     parameter.Accessors.Add(a);
            // }

            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<Boolean>, Reference = {Reference}, Value = {Value}";
        }
    }

    public class CharParameter : Parameter, ICharParameter
    {
        internal CharParameter(IParameterContext context) : base(context)
        {
        }

        internal CharParameter(IParameterContext context, ParameterRef reference) : base(context, reference)
        {
        }
        
        internal CharParameter(IParameterContext context, ParameterRef reference, char? value) : base(context, reference)
        {
            Value = value;
        }

        public char? Value
        {
            get;
            set;
        }

        public override CharParameter CloneData(IParameterContext context)
        {
             CharParameter parameter = new CharParameter(context, Reference, Value);
             

            // foreach (var a in Accessors.Select(ac => ac.Clone()))
            // {
            //     parameter.Accessors.Add(a);
            // }

            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<Char>, Reference = {Reference}, Value = {Value}";
        }
    }

    public class ByteParameter : Parameter, IByteParameter
    {
        internal ByteParameter(IParameterContext context) : base(context)
        {
        }

        internal ByteParameter(IParameterContext context, ParameterRef reference) : base(context, reference)
        {
        }
        
        internal ByteParameter(IParameterContext context, ParameterRef reference, byte? value) : base(context, reference)
        {
            Value = value;
        }

        public byte? Value
        {
            get;
            set;
        }

        public override ByteParameter CloneData(IParameterContext context)
        {
             ByteParameter parameter = new ByteParameter(context, Reference, Value);
             

            // foreach (var a in Accessors.Select(ac => ac.Clone()))
            // {
            //     parameter.Accessors.Add(a);
            // }

            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<Byte>, Reference = {Reference}, Value = {Value}";
        }
    }

    public class SByteParameter : Parameter, ISByteParameter
    {
        internal SByteParameter(IParameterContext context) : base(context)
        {
        }

        internal SByteParameter(IParameterContext context, ParameterRef reference) : base(context, reference)
        {
        }
        
        internal SByteParameter(IParameterContext context, ParameterRef reference, sbyte? value) : base(context, reference)
        {
            Value = value;
        }

        public sbyte? Value
        {
            get;
            set;
        }

        public override SByteParameter CloneData(IParameterContext context)
        {
             SByteParameter parameter = new SByteParameter(context, Reference, Value);
             

            // foreach (var a in Accessors.Select(ac => ac.Clone()))
            // {
            //     parameter.Accessors.Add(a);
            // }

            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<SByte>, Reference = {Reference}, Value = {Value}";
        }
    }

    public class Int16Parameter : Parameter, IInt16Parameter
    {
        internal Int16Parameter(IParameterContext context) : base(context)
        {
        }

        internal Int16Parameter(IParameterContext context, ParameterRef reference) : base(context, reference)
        {
        }
        
        internal Int16Parameter(IParameterContext context, ParameterRef reference, short? value) : base(context, reference)
        {
            Value = value;
        }

        public short? Value
        {
            get;
            set;
        }

        public override Int16Parameter CloneData(IParameterContext context)
        {
             Int16Parameter parameter = new Int16Parameter(context, Reference, Value);
             

            // foreach (var a in Accessors.Select(ac => ac.Clone()))
            // {
            //     parameter.Accessors.Add(a);
            // }

            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<Int16>, Reference = {Reference}, Value = {Value}";
        }
    }

    public class Int32Parameter : Parameter, IInt32Parameter
    {
        internal Int32Parameter(IParameterContext context) : base(context)
        {
        }

        internal Int32Parameter(IParameterContext context, ParameterRef reference) : base(context, reference)
        {
        }
        
        internal Int32Parameter(IParameterContext context, ParameterRef reference, int? value) : base(context, reference)
        {
            Value = value;
        }

        public int? Value
        {
            get;
            set;
        }

        public override Int32Parameter CloneData(IParameterContext context)
        {
             Int32Parameter parameter = new Int32Parameter(context, Reference, Value);
             

            // foreach (var a in Accessors.Select(ac => ac.Clone()))
            // {
            //     parameter.Accessors.Add(a);
            // }

            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<Int32>, Reference = {Reference}, Value = {Value}";
        }
    }

    public class Int64Parameter : Parameter, IInt64Parameter
    {
        internal Int64Parameter(IParameterContext context) : base(context)
        {
        }

        internal Int64Parameter(IParameterContext context, ParameterRef reference) : base(context, reference)
        {
        }
        
        internal Int64Parameter(IParameterContext context, ParameterRef reference, long? value) : base(context, reference)
        {
            Value = value;
        }

        public long? Value
        {
            get;
            set;
        }

        public override Int64Parameter CloneData(IParameterContext context)
        {
             Int64Parameter parameter = new Int64Parameter(context, Reference, Value);
             

            // foreach (var a in Accessors.Select(ac => ac.Clone()))
            // {
            //     parameter.Accessors.Add(a);
            // }

            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<Int64>, Reference = {Reference}, Value = {Value}";
        }
    }

    public class UInt16Parameter : Parameter, IUInt16Parameter
    {
        internal UInt16Parameter(IParameterContext context) : base(context)
        {
        }

        internal UInt16Parameter(IParameterContext context, ParameterRef reference) : base(context, reference)
        {
        }
        
        internal UInt16Parameter(IParameterContext context, ParameterRef reference, ushort? value) : base(context, reference)
        {
            Value = value;
        }

        public ushort? Value
        {
            get;
            set;
        }

        public override UInt16Parameter CloneData(IParameterContext context)
        {
             UInt16Parameter parameter = new UInt16Parameter(context, Reference, Value);
             

            // foreach (var a in Accessors.Select(ac => ac.Clone()))
            // {
            //     parameter.Accessors.Add(a);
            // }

            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<UInt16>, Reference = {Reference}, Value = {Value}";
        }
    }

    public class UInt32Parameter : Parameter, IUInt32Parameter
    {
        internal UInt32Parameter(IParameterContext context) : base(context)
        {
        }

        internal UInt32Parameter(IParameterContext context, ParameterRef reference) : base(context, reference)
        {
        }
        
        internal UInt32Parameter(IParameterContext context, ParameterRef reference, uint? value) : base(context, reference)
        {
            Value = value;
        }

        public uint? Value
        {
            get;
            set;
        }

        public override UInt32Parameter CloneData(IParameterContext context)
        {
             UInt32Parameter parameter = new UInt32Parameter(context, Reference, Value);
             

            // foreach (var a in Accessors.Select(ac => ac.Clone()))
            // {
            //     parameter.Accessors.Add(a);
            // }

            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<UInt32>, Reference = {Reference}, Value = {Value}";
        }
    }

    public class UInt64Parameter : Parameter, IUInt64Parameter
    {
        internal UInt64Parameter(IParameterContext context) : base(context)
        {
        }

        internal UInt64Parameter(IParameterContext context, ParameterRef reference) : base(context, reference)
        {
        }
        
        internal UInt64Parameter(IParameterContext context, ParameterRef reference, ulong? value) : base(context, reference)
        {
            Value = value;
        }

        public ulong? Value
        {
            get;
            set;
        }

        public override UInt64Parameter CloneData(IParameterContext context)
        {
             UInt64Parameter parameter = new UInt64Parameter(context, Reference, Value);
             

            // foreach (var a in Accessors.Select(ac => ac.Clone()))
            // {
            //     parameter.Accessors.Add(a);
            // }

            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<UInt64>, Reference = {Reference}, Value = {Value}";
        }
    }

    public class SingleParameter : Parameter, ISingleParameter
    {
        internal SingleParameter(IParameterContext context) : base(context)
        {
        }

        internal SingleParameter(IParameterContext context, ParameterRef reference) : base(context, reference)
        {
        }
        
        internal SingleParameter(IParameterContext context, ParameterRef reference, float? value) : base(context, reference)
        {
            Value = value;
        }

        public float? Value
        {
            get;
            set;
        }

        public override SingleParameter CloneData(IParameterContext context)
        {
             SingleParameter parameter = new SingleParameter(context, Reference, Value);
             

            // foreach (var a in Accessors.Select(ac => ac.Clone()))
            // {
            //     parameter.Accessors.Add(a);
            // }

            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<Single>, Reference = {Reference}, Value = {Value}";
        }
    }

    public class DoubleParameter : Parameter, IDoubleParameter
    {
        internal DoubleParameter(IParameterContext context) : base(context)
        {
        }

        internal DoubleParameter(IParameterContext context, ParameterRef reference) : base(context, reference)
        {
        }
        
        internal DoubleParameter(IParameterContext context, ParameterRef reference, double? value) : base(context, reference)
        {
            Value = value;
        }

        public double? Value
        {
            get;
            set;
        }

        public override DoubleParameter CloneData(IParameterContext context)
        {
             DoubleParameter parameter = new DoubleParameter(context, Reference, Value);
             

            // foreach (var a in Accessors.Select(ac => ac.Clone()))
            // {
            //     parameter.Accessors.Add(a);
            // }

            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<Double>, Reference = {Reference}, Value = {Value}";
        }
    }


    public static partial class ParameterContextExtensions
    {
        public static IBooleanParameter CreateBooleanParameter(this IParameterContext context)
        {
            return CreateBooleanParameter(context, ParameterRef.Any, null);
        }
        
        public static IBooleanParameter CreateBooleanParameter(this IParameterContext context, ParameterRef reference)
        {
            return CreateBooleanParameter(context, reference, null);
        }
        
        public static IBooleanParameter CreateBooleanParameter(this IParameterContext context, ParameterRef reference, bool? value)
        {
            BooleanParameter parameter = new BooleanParameter(context, reference, value);
            
            context.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
        public static ICharParameter CreateCharParameter(this IParameterContext context)
        {
            return CreateCharParameter(context, ParameterRef.Any, null);
        }
        
        public static ICharParameter CreateCharParameter(this IParameterContext context, ParameterRef reference)
        {
            return CreateCharParameter(context, reference, null);
        }
        
        public static ICharParameter CreateCharParameter(this IParameterContext context, ParameterRef reference, char? value)
        {
            CharParameter parameter = new CharParameter(context, reference, value);
            
            context.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
        public static IByteParameter CreateByteParameter(this IParameterContext context)
        {
            return CreateByteParameter(context, ParameterRef.Any, null);
        }
        
        public static IByteParameter CreateByteParameter(this IParameterContext context, ParameterRef reference)
        {
            return CreateByteParameter(context, reference, null);
        }
        
        public static IByteParameter CreateByteParameter(this IParameterContext context, ParameterRef reference, byte? value)
        {
            ByteParameter parameter = new ByteParameter(context, reference, value);
            
            context.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
        public static ISByteParameter CreateSByteParameter(this IParameterContext context)
        {
            return CreateSByteParameter(context, ParameterRef.Any, null);
        }
        
        public static ISByteParameter CreateSByteParameter(this IParameterContext context, ParameterRef reference)
        {
            return CreateSByteParameter(context, reference, null);
        }
        
        public static ISByteParameter CreateSByteParameter(this IParameterContext context, ParameterRef reference, sbyte? value)
        {
            SByteParameter parameter = new SByteParameter(context, reference, value);
            
            context.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
        public static IInt16Parameter CreateInt16Parameter(this IParameterContext context)
        {
            return CreateInt16Parameter(context, ParameterRef.Any, null);
        }
        
        public static IInt16Parameter CreateInt16Parameter(this IParameterContext context, ParameterRef reference)
        {
            return CreateInt16Parameter(context, reference, null);
        }
        
        public static IInt16Parameter CreateInt16Parameter(this IParameterContext context, ParameterRef reference, short? value)
        {
            Int16Parameter parameter = new Int16Parameter(context, reference, value);
            
            context.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
        public static IInt32Parameter CreateInt32Parameter(this IParameterContext context)
        {
            return CreateInt32Parameter(context, ParameterRef.Any, null);
        }
        
        public static IInt32Parameter CreateInt32Parameter(this IParameterContext context, ParameterRef reference)
        {
            return CreateInt32Parameter(context, reference, null);
        }
        
        public static IInt32Parameter CreateInt32Parameter(this IParameterContext context, ParameterRef reference, int? value)
        {
            Int32Parameter parameter = new Int32Parameter(context, reference, value);
            
            context.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
        public static IInt64Parameter CreateInt64Parameter(this IParameterContext context)
        {
            return CreateInt64Parameter(context, ParameterRef.Any, null);
        }
        
        public static IInt64Parameter CreateInt64Parameter(this IParameterContext context, ParameterRef reference)
        {
            return CreateInt64Parameter(context, reference, null);
        }
        
        public static IInt64Parameter CreateInt64Parameter(this IParameterContext context, ParameterRef reference, long? value)
        {
            Int64Parameter parameter = new Int64Parameter(context, reference, value);
            
            context.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
        public static IUInt16Parameter CreateUInt16Parameter(this IParameterContext context)
        {
            return CreateUInt16Parameter(context, ParameterRef.Any, null);
        }
        
        public static IUInt16Parameter CreateUInt16Parameter(this IParameterContext context, ParameterRef reference)
        {
            return CreateUInt16Parameter(context, reference, null);
        }
        
        public static IUInt16Parameter CreateUInt16Parameter(this IParameterContext context, ParameterRef reference, ushort? value)
        {
            UInt16Parameter parameter = new UInt16Parameter(context, reference, value);
            
            context.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
        public static IUInt32Parameter CreateUInt32Parameter(this IParameterContext context)
        {
            return CreateUInt32Parameter(context, ParameterRef.Any, null);
        }
        
        public static IUInt32Parameter CreateUInt32Parameter(this IParameterContext context, ParameterRef reference)
        {
            return CreateUInt32Parameter(context, reference, null);
        }
        
        public static IUInt32Parameter CreateUInt32Parameter(this IParameterContext context, ParameterRef reference, uint? value)
        {
            UInt32Parameter parameter = new UInt32Parameter(context, reference, value);
            
            context.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
        public static IUInt64Parameter CreateUInt64Parameter(this IParameterContext context)
        {
            return CreateUInt64Parameter(context, ParameterRef.Any, null);
        }
        
        public static IUInt64Parameter CreateUInt64Parameter(this IParameterContext context, ParameterRef reference)
        {
            return CreateUInt64Parameter(context, reference, null);
        }
        
        public static IUInt64Parameter CreateUInt64Parameter(this IParameterContext context, ParameterRef reference, ulong? value)
        {
            UInt64Parameter parameter = new UInt64Parameter(context, reference, value);
            
            context.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
        public static ISingleParameter CreateSingleParameter(this IParameterContext context)
        {
            return CreateSingleParameter(context, ParameterRef.Any, null);
        }
        
        public static ISingleParameter CreateSingleParameter(this IParameterContext context, ParameterRef reference)
        {
            return CreateSingleParameter(context, reference, null);
        }
        
        public static ISingleParameter CreateSingleParameter(this IParameterContext context, ParameterRef reference, float? value)
        {
            SingleParameter parameter = new SingleParameter(context, reference, value);
            
            context.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
        public static IDoubleParameter CreateDoubleParameter(this IParameterContext context)
        {
            return CreateDoubleParameter(context, ParameterRef.Any, null);
        }
        
        public static IDoubleParameter CreateDoubleParameter(this IParameterContext context, ParameterRef reference)
        {
            return CreateDoubleParameter(context, reference, null);
        }
        
        public static IDoubleParameter CreateDoubleParameter(this IParameterContext context, ParameterRef reference, double? value)
        {
            DoubleParameter parameter = new DoubleParameter(context, reference, value);
            
            context.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
    }
}