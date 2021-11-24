using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters.Tests
{
    public class BooleanParameterTest : PrimitiveValueParameterTests<BooleanParameter, bool>
    {
        protected override BooleanParameter Create(string name = "p") => new BooleanParameter(name);
    }

    public class CharParameterTest : PrimitiveValueParameterTests<CharParameter, char>
    {
        protected override CharParameter Create(string name = "p") => new CharParameter(name);
    }

    public class ByteParameterTest : PrimitiveValueParameterTests<ByteParameter, byte>
    {
        protected override ByteParameter Create(string name = "p") => new ByteParameter(name);
    }

    public class SByteParameterTest : PrimitiveValueParameterTests<SByteParameter, sbyte>
    {
        protected override SByteParameter Create(string name = "p") => new SByteParameter(name);
    }

    public class Int16ParameterTest : PrimitiveValueParameterTests<Int16Parameter, short>
    {
        protected override Int16Parameter Create(string name = "p") => new Int16Parameter(name);
    }

    public class Int32ParameterTest : PrimitiveValueParameterTests<Int32Parameter, int>
    {
        protected override Int32Parameter Create(string name = "p") => new Int32Parameter(name);
    }

    public class Int64ParameterTest : PrimitiveValueParameterTests<Int64Parameter, long>
    {
        protected override Int64Parameter Create(string name = "p") => new Int64Parameter(name);
    }

    public class UInt16ParameterTest : PrimitiveValueParameterTests<UInt16Parameter, ushort>
    {
        protected override UInt16Parameter Create(string name = "p") => new UInt16Parameter(name);
    }

    public class UInt32ParameterTest : PrimitiveValueParameterTests<UInt32Parameter, uint>
    {
        protected override UInt32Parameter Create(string name = "p") => new UInt32Parameter(name);
    }

    public class UInt64ParameterTest : PrimitiveValueParameterTests<UInt64Parameter, ulong>
    {
        protected override UInt64Parameter Create(string name = "p") => new UInt64Parameter(name);
    }

    public class SingleParameterTest : PrimitiveValueParameterTests<SingleParameter, float>
    {
        protected override SingleParameter Create(string name = "p") => new SingleParameter(name);
    }
    public class DoubleParameterTest : PrimitiveValueParameterTests<DoubleParameter, double>
    {
        protected override DoubleParameter Create(string name = "p") => new DoubleParameter(name);
    }
}
