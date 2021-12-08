using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Parameters.Tests
{
    public abstract class PrimitiveValueParameterTests<TParameter, TValue> : ParameterTests<TParameter>
        where TValue : struct
        where TParameter : PrimitiveValueParameter<TValue>
    {
        [Fact]
        public void GetChildren_ShouldBe_Empty()
        {
            TParameter p = Create();
            p.GetChildren().Should().HaveCount(0);
        }

        [Fact]
        public void TypeName_ShouldBeEquiavalent_To_TValue()
        {
            TParameter p = Create();
            p.TypeName.Should().Be(typeof(TValue).FullName);
        }
    }

    public class BooleanParameterTests : PrimitiveValueParameterTests<BooleanParameter, bool>
    {
        protected override BooleanParameter Create(int id) => new BooleanParameter(default(bool), id);
    }

    public class CharParameterTests : PrimitiveValueParameterTests<CharParameter, char>
    {
        protected override CharParameter Create(int id) => new CharParameter(default(char), id);
    }

    public class ByteParameterTests : PrimitiveValueParameterTests<ByteParameter, byte>
    {
        protected override ByteParameter Create(int id) => new ByteParameter(default(byte), id);
    }

    public class SByteParameterTests : PrimitiveValueParameterTests<SByteParameter, sbyte>
    {
        protected override SByteParameter Create(int id) => new SByteParameter(default(sbyte), id);
    }

    public class Int16ParameterTests : PrimitiveValueParameterTests<Int16Parameter, short>
    {
        protected override Int16Parameter Create(int id) => new Int16Parameter(default(short), id);
    }

    public class Int32ParameterTests : PrimitiveValueParameterTests<Int32Parameter, int>
    {
        protected override Int32Parameter Create(int id) => new Int32Parameter(default(int), id);
    }

    public class Int64ParameterTests : PrimitiveValueParameterTests<Int64Parameter, long>
    {
        protected override Int64Parameter Create(int id) => new Int64Parameter(default(long), id);
    }

    public class UInt16ParameterTests : PrimitiveValueParameterTests<UInt16Parameter, ushort>
    {
        protected override UInt16Parameter Create(int id) => new UInt16Parameter(default(ushort), id);
    }

    public class UInt32ParameterTests : PrimitiveValueParameterTests<UInt32Parameter, uint>
    {
        protected override UInt32Parameter Create(int id) => new UInt32Parameter(default(uint), id);
    }

    public class UInt64ParameterTests : PrimitiveValueParameterTests<UInt64Parameter, ulong>
    {
        protected override UInt64Parameter Create(int id) => new UInt64Parameter(default(ulong), id);
    }

    public class SingleParameterTests : PrimitiveValueParameterTests<SingleParameter, float>
    {
        protected override SingleParameter Create(int id) => new SingleParameter(default(float), id);
    }

    public class DoubleParameterTests : PrimitiveValueParameterTests<DoubleParameter, double>
    {
        protected override DoubleParameter Create(int id) => new DoubleParameter(default(double), id);
    }

}