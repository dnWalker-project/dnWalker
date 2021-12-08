using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public interface IPrimitiveValueParameter : IParameter
    {
        object Value { get; set; }
    }

    public interface IPrimitiveValueParameter<T> : IPrimitiveValueParameter
        where T : struct
    {
        new T Value { get; set; }

        object IPrimitiveValueParameter.Value
        {
            get { return Value; }
            set { Value = (T)value; }
        }
    }
    public interface IBooleanParameter : IPrimitiveValueParameter<bool>
    {
    }
 
    public interface ICharParameter : IPrimitiveValueParameter<char>
    {
    }
 
    public interface IByteParameter : IPrimitiveValueParameter<byte>
    {
    }
 
    public interface ISByteParameter : IPrimitiveValueParameter<sbyte>
    {
    }
 
    public interface IInt16Parameter : IPrimitiveValueParameter<short>
    {
    }
 
    public interface IInt32Parameter : IPrimitiveValueParameter<int>
    {
    }
 
    public interface IInt64Parameter : IPrimitiveValueParameter<long>
    {
    }
 
    public interface IUInt16Parameter : IPrimitiveValueParameter<ushort>
    {
    }
 
    public interface IUInt32Parameter : IPrimitiveValueParameter<uint>
    {
    }
 
    public interface IUInt64Parameter : IPrimitiveValueParameter<ulong>
    {
    }
 
    public interface ISingleParameter : IPrimitiveValueParameter<float>
    {
    }
 
    public interface IDoubleParameter : IPrimitiveValueParameter<double>
    {
    }
 
}