using dnWalker.TypeSystem;

using System;
using System.Linq;

namespace dnWalker.Parameters
{

    public class BooleanParameter : Parameter, IBooleanParameter
    {
        internal BooleanParameter(IParameterSet set) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.Boolean.TypeDefOrRef))
        {
        }

        internal BooleanParameter(IParameterSet set, ParameterRef reference) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.Boolean.TypeDefOrRef), reference)
        {
        }
        
        public bool? Value
        {
            get;
            set;
        }

        public override BooleanParameter CloneData(IParameterSet set)
        {
             BooleanParameter parameter = new BooleanParameter(set, Reference)
             {
                Value = this.Value
             };
             
            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<Boolean>, Reference = {Reference}, Value = {Value}";
        }
    }

    public class CharParameter : Parameter, ICharParameter
    {
        internal CharParameter(IParameterSet set) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.Char.TypeDefOrRef))
        {
        }

        internal CharParameter(IParameterSet set, ParameterRef reference) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.Char.TypeDefOrRef), reference)
        {
        }
        
        public char? Value
        {
            get;
            set;
        }

        public override CharParameter CloneData(IParameterSet set)
        {
             CharParameter parameter = new CharParameter(set, Reference)
             {
                Value = this.Value
             };
             
            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<Char>, Reference = {Reference}, Value = {Value}";
        }
    }

    public class ByteParameter : Parameter, IByteParameter
    {
        internal ByteParameter(IParameterSet set) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.Byte.TypeDefOrRef))
        {
        }

        internal ByteParameter(IParameterSet set, ParameterRef reference) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.Byte.TypeDefOrRef), reference)
        {
        }
        
        public byte? Value
        {
            get;
            set;
        }

        public override ByteParameter CloneData(IParameterSet set)
        {
             ByteParameter parameter = new ByteParameter(set, Reference)
             {
                Value = this.Value
             };
             
            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<Byte>, Reference = {Reference}, Value = {Value}";
        }
    }

    public class SByteParameter : Parameter, ISByteParameter
    {
        internal SByteParameter(IParameterSet set) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.SByte.TypeDefOrRef))
        {
        }

        internal SByteParameter(IParameterSet set, ParameterRef reference) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.SByte.TypeDefOrRef), reference)
        {
        }
        
        public sbyte? Value
        {
            get;
            set;
        }

        public override SByteParameter CloneData(IParameterSet set)
        {
             SByteParameter parameter = new SByteParameter(set, Reference)
             {
                Value = this.Value
             };
             
            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<SByte>, Reference = {Reference}, Value = {Value}";
        }
    }

    public class Int16Parameter : Parameter, IInt16Parameter
    {
        internal Int16Parameter(IParameterSet set) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.Int16.TypeDefOrRef))
        {
        }

        internal Int16Parameter(IParameterSet set, ParameterRef reference) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.Int16.TypeDefOrRef), reference)
        {
        }
        
        public short? Value
        {
            get;
            set;
        }

        public override Int16Parameter CloneData(IParameterSet set)
        {
             Int16Parameter parameter = new Int16Parameter(set, Reference)
             {
                Value = this.Value
             };
             
            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<Int16>, Reference = {Reference}, Value = {Value}";
        }
    }

    public class Int32Parameter : Parameter, IInt32Parameter
    {
        internal Int32Parameter(IParameterSet set) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.Int32.TypeDefOrRef))
        {
        }

        internal Int32Parameter(IParameterSet set, ParameterRef reference) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.Int32.TypeDefOrRef), reference)
        {
        }
        
        public int? Value
        {
            get;
            set;
        }

        public override Int32Parameter CloneData(IParameterSet set)
        {
             Int32Parameter parameter = new Int32Parameter(set, Reference)
             {
                Value = this.Value
             };
             
            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<Int32>, Reference = {Reference}, Value = {Value}";
        }
    }

    public class Int64Parameter : Parameter, IInt64Parameter
    {
        internal Int64Parameter(IParameterSet set) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.Int64.TypeDefOrRef))
        {
        }

        internal Int64Parameter(IParameterSet set, ParameterRef reference) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.Int64.TypeDefOrRef), reference)
        {
        }
        
        public long? Value
        {
            get;
            set;
        }

        public override Int64Parameter CloneData(IParameterSet set)
        {
             Int64Parameter parameter = new Int64Parameter(set, Reference)
             {
                Value = this.Value
             };
             
            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<Int64>, Reference = {Reference}, Value = {Value}";
        }
    }

    public class UInt16Parameter : Parameter, IUInt16Parameter
    {
        internal UInt16Parameter(IParameterSet set) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.UInt16.TypeDefOrRef))
        {
        }

        internal UInt16Parameter(IParameterSet set, ParameterRef reference) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.UInt16.TypeDefOrRef), reference)
        {
        }
        
        public ushort? Value
        {
            get;
            set;
        }

        public override UInt16Parameter CloneData(IParameterSet set)
        {
             UInt16Parameter parameter = new UInt16Parameter(set, Reference)
             {
                Value = this.Value
             };
             
            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<UInt16>, Reference = {Reference}, Value = {Value}";
        }
    }

    public class UInt32Parameter : Parameter, IUInt32Parameter
    {
        internal UInt32Parameter(IParameterSet set) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.UInt32.TypeDefOrRef))
        {
        }

        internal UInt32Parameter(IParameterSet set, ParameterRef reference) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.UInt32.TypeDefOrRef), reference)
        {
        }
        
        public uint? Value
        {
            get;
            set;
        }

        public override UInt32Parameter CloneData(IParameterSet set)
        {
             UInt32Parameter parameter = new UInt32Parameter(set, Reference)
             {
                Value = this.Value
             };
             
            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<UInt32>, Reference = {Reference}, Value = {Value}";
        }
    }

    public class UInt64Parameter : Parameter, IUInt64Parameter
    {
        internal UInt64Parameter(IParameterSet set) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.UInt64.TypeDefOrRef))
        {
        }

        internal UInt64Parameter(IParameterSet set, ParameterRef reference) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.UInt64.TypeDefOrRef), reference)
        {
        }
        
        public ulong? Value
        {
            get;
            set;
        }

        public override UInt64Parameter CloneData(IParameterSet set)
        {
             UInt64Parameter parameter = new UInt64Parameter(set, Reference)
             {
                Value = this.Value
             };
             
            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<UInt64>, Reference = {Reference}, Value = {Value}";
        }
    }

    public class SingleParameter : Parameter, ISingleParameter
    {
        internal SingleParameter(IParameterSet set) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.Single.TypeDefOrRef))
        {
        }

        internal SingleParameter(IParameterSet set, ParameterRef reference) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.Single.TypeDefOrRef), reference)
        {
        }
        
        public float? Value
        {
            get;
            set;
        }

        public override SingleParameter CloneData(IParameterSet set)
        {
             SingleParameter parameter = new SingleParameter(set, Reference)
             {
                Value = this.Value
             };
             
            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<Single>, Reference = {Reference}, Value = {Value}";
        }
    }

    public class DoubleParameter : Parameter, IDoubleParameter
    {
        internal DoubleParameter(IParameterSet set) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.Double.TypeDefOrRef))
        {
        }

        internal DoubleParameter(IParameterSet set, ParameterRef reference) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.Double.TypeDefOrRef), reference)
        {
        }
        
        public double? Value
        {
            get;
            set;
        }

        public override DoubleParameter CloneData(IParameterSet set)
        {
             DoubleParameter parameter = new DoubleParameter(set, Reference)
             {
                Value = this.Value
             };
             
            return parameter;
        }

        public override string ToString()
        {
            return $"Parameter<Double>, Reference = {Reference}, Value = {Value}";
        }
    }


    public static partial class ParameterContextExtensions
    {
        public static IBooleanParameter CreateBooleanParameter(this IParameterSet set)
        {
            return CreateBooleanParameter(set, set.GetParameterRef());
        }
        
        public static IBooleanParameter CreateBooleanParameter(this IParameterSet set, ParameterRef reference)
        {
            BooleanParameter parameter = new BooleanParameter(set, reference);
            set.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
        public static ICharParameter CreateCharParameter(this IParameterSet set)
        {
            return CreateCharParameter(set, set.GetParameterRef());
        }
        
        public static ICharParameter CreateCharParameter(this IParameterSet set, ParameterRef reference)
        {
            CharParameter parameter = new CharParameter(set, reference);
            set.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
        public static IByteParameter CreateByteParameter(this IParameterSet set)
        {
            return CreateByteParameter(set, set.GetParameterRef());
        }
        
        public static IByteParameter CreateByteParameter(this IParameterSet set, ParameterRef reference)
        {
            ByteParameter parameter = new ByteParameter(set, reference);
            set.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
        public static ISByteParameter CreateSByteParameter(this IParameterSet set)
        {
            return CreateSByteParameter(set, set.GetParameterRef());
        }
        
        public static ISByteParameter CreateSByteParameter(this IParameterSet set, ParameterRef reference)
        {
            SByteParameter parameter = new SByteParameter(set, reference);
            set.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
        public static IInt16Parameter CreateInt16Parameter(this IParameterSet set)
        {
            return CreateInt16Parameter(set, set.GetParameterRef());
        }
        
        public static IInt16Parameter CreateInt16Parameter(this IParameterSet set, ParameterRef reference)
        {
            Int16Parameter parameter = new Int16Parameter(set, reference);
            set.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
        public static IInt32Parameter CreateInt32Parameter(this IParameterSet set)
        {
            return CreateInt32Parameter(set, set.GetParameterRef());
        }
        
        public static IInt32Parameter CreateInt32Parameter(this IParameterSet set, ParameterRef reference)
        {
            Int32Parameter parameter = new Int32Parameter(set, reference);
            set.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
        public static IInt64Parameter CreateInt64Parameter(this IParameterSet set)
        {
            return CreateInt64Parameter(set, set.GetParameterRef());
        }
        
        public static IInt64Parameter CreateInt64Parameter(this IParameterSet set, ParameterRef reference)
        {
            Int64Parameter parameter = new Int64Parameter(set, reference);
            set.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
        public static IUInt16Parameter CreateUInt16Parameter(this IParameterSet set)
        {
            return CreateUInt16Parameter(set, set.GetParameterRef());
        }
        
        public static IUInt16Parameter CreateUInt16Parameter(this IParameterSet set, ParameterRef reference)
        {
            UInt16Parameter parameter = new UInt16Parameter(set, reference);
            set.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
        public static IUInt32Parameter CreateUInt32Parameter(this IParameterSet set)
        {
            return CreateUInt32Parameter(set, set.GetParameterRef());
        }
        
        public static IUInt32Parameter CreateUInt32Parameter(this IParameterSet set, ParameterRef reference)
        {
            UInt32Parameter parameter = new UInt32Parameter(set, reference);
            set.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
        public static IUInt64Parameter CreateUInt64Parameter(this IParameterSet set)
        {
            return CreateUInt64Parameter(set, set.GetParameterRef());
        }
        
        public static IUInt64Parameter CreateUInt64Parameter(this IParameterSet set, ParameterRef reference)
        {
            UInt64Parameter parameter = new UInt64Parameter(set, reference);
            set.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
        public static ISingleParameter CreateSingleParameter(this IParameterSet set)
        {
            return CreateSingleParameter(set, set.GetParameterRef());
        }
        
        public static ISingleParameter CreateSingleParameter(this IParameterSet set, ParameterRef reference)
        {
            SingleParameter parameter = new SingleParameter(set, reference);
            set.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
        public static IDoubleParameter CreateDoubleParameter(this IParameterSet set)
        {
            return CreateDoubleParameter(set, set.GetParameterRef());
        }
        
        public static IDoubleParameter CreateDoubleParameter(this IParameterSet set, ParameterRef reference)
        {
            DoubleParameter parameter = new DoubleParameter(set, reference);
            set.Parameters.Add(parameter.Reference, parameter);
            
            return parameter;
        }
    }
}