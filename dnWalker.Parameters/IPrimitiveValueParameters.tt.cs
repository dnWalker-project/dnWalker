using System;

namespace dnWalker.Parameters
{
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