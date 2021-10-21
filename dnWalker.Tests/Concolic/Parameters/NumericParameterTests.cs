using dnWalker.Concolic.Parameters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Concolic.Parameters
{
    public class BooleanParameterTest : PrimitiveValueParameterTests<BooleanParameter, Boolean>
    {
        protected override BooleanParameter Create(String name = "p") => new BooleanParameter(name);
    }
    
    public class CharParameterTest : PrimitiveValueParameterTests<CharParameter, Char>
    {
        protected override CharParameter Create(String name = "p") => new CharParameter(name);
    }
    
    public class ByteParameterTest : PrimitiveValueParameterTests<ByteParameter, Byte>
    {
        protected override ByteParameter Create(String name = "p") => new ByteParameter(name);
    }
    
    public class SByteParameterTest : PrimitiveValueParameterTests<SByteParameter, SByte>
    {
        protected override SByteParameter Create(String name = "p") => new SByteParameter(name);
    }
    
    public class Int16ParameterTest : PrimitiveValueParameterTests<Int16Parameter, Int16>
    {
        protected override Int16Parameter Create(String name = "p") => new Int16Parameter(name);
    }
    
    public class Int32ParameterTest : PrimitiveValueParameterTests<Int32Parameter, Int32>
    {
        protected override Int32Parameter Create(String name = "p") => new Int32Parameter(name);
    }
    
    public class Int64ParameterTest : PrimitiveValueParameterTests<Int64Parameter, Int64>
    {
        protected override Int64Parameter Create(String name = "p") => new Int64Parameter(name);
    }
    
    public class UInt16ParameterTest : PrimitiveValueParameterTests<UInt16Parameter, UInt16>
    {
        protected override UInt16Parameter Create(String name = "p") => new UInt16Parameter(name);
    }
    
    public class UInt32ParameterTest : PrimitiveValueParameterTests<UInt32Parameter, UInt32>
    {
        protected override UInt32Parameter Create(String name = "p") => new UInt32Parameter(name);
    }
    
    public class UInt64ParameterTest : PrimitiveValueParameterTests<UInt64Parameter, UInt64>
    {
        protected override UInt64Parameter Create(String name = "p") => new UInt64Parameter(name);
    }

    public class SingleParameterTest : PrimitiveValueParameterTests<SingleParameter, Single>
    {
        protected override SingleParameter Create(String name = "p") => new SingleParameter(name);
    }
    public class DoubleParameterTest : PrimitiveValueParameterTests<DoubleParameter, Double>
    {
        protected override DoubleParameter Create(String name = "p") => new DoubleParameter(name);
    }
}
