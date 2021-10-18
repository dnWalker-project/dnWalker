using dnWalker.Concolic.Parameters;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Tests.Concolic.Parameters
{
    public class NumericParametersTests : dnlibTypeTestBase
    {
        [Fact]
        public void After_SettingValue_ValueTrait_IsAvailable_For_BooleanParameter()
        {
            const Boolean b = true;

            BooleanParameter p = new BooleanParameter();

            p.Traits.Count.Should().BeLessOrEqualTo(1);

            p.Value = b;

            p.Traits.Count.Should().BeLessOrEqualTo(1);
            p.TryGetTrait<ValueTrait<Boolean>>(out ValueTrait<Boolean> trait).Should().BeTrue();
            trait.Should().NotBeNull();
            trait.Value.Should().Be(b);
        }

        [Fact]
        public void After_SettingValue_ValueTrait_IsAvailable_For_CharParameter()
        {
            const Char c = 'c';

            CharParameter p = new CharParameter();

            p.Traits.Count.Should().BeLessOrEqualTo(1);

            p.Value = c;

            p.Traits.Count.Should().BeLessOrEqualTo(1);
            p.TryGetTrait<ValueTrait<Char>>(out ValueTrait<Char> trait).Should().BeTrue();
            trait.Should().NotBeNull();
            trait.Value.Should().Be(c);
        }

        [Fact]
        public void After_SettingValue_ValueTrait_IsAvailable_For_ByteParameter()
        {
            const Byte b = 128;

            ByteParameter p = new ByteParameter();

            p.Traits.Count.Should().BeLessOrEqualTo(1);

            p.Value = b;

            p.Traits.Count.Should().BeLessOrEqualTo(1);
            p.TryGetTrait<ValueTrait<Byte>>(out ValueTrait<Byte> trait).Should().BeTrue();
            trait.Should().NotBeNull();
            trait.Value.Should().Be(b);
        }

        [Fact]
        public void After_SettingValue_ValueTrait_IsAvailable_For_SByteParameter()
        {
            const SByte b = 127;

            SByteParameter p = new SByteParameter();

            p.Traits.Count.Should().BeLessOrEqualTo(1);

            p.Value = b;

            p.Traits.Count.Should().BeLessOrEqualTo(1);
            p.TryGetTrait<ValueTrait<SByte>>(out ValueTrait<SByte> trait).Should().BeTrue();
            trait.Should().NotBeNull();
            trait.Value.Should().Be(b);
        }

        [Fact]
        public void After_SettingValue_ValueTrait_IsAvailable_For_Int16Parameter()
        {
            const Int16 b = 128;

            Int16Parameter p = new Int16Parameter();

            p.Traits.Count.Should().BeLessOrEqualTo(1);

            p.Value = b;

            p.Traits.Count.Should().BeLessOrEqualTo(1);
            p.TryGetTrait<ValueTrait<Int16>>(out ValueTrait<Int16> trait).Should().BeTrue();
            trait.Should().NotBeNull();
            trait.Value.Should().Be(b);
        }

        [Fact]
        public void After_SettingValue_ValueTrait_IsAvailable_For_Int32Parameter()
        {
            const Int32 b = 128;

            Int32Parameter p = new Int32Parameter();

            p.Traits.Count.Should().BeLessOrEqualTo(1);

            p.Value = b;

            p.Traits.Count.Should().BeLessOrEqualTo(1);
            p.TryGetTrait<ValueTrait<Int32>>(out ValueTrait<Int32> trait).Should().BeTrue();
            trait.Should().NotBeNull();
            trait.Value.Should().Be(b);
        }

        [Fact]
        public void After_SettingValue_ValueTrait_IsAvailable_For_Int64Parameter()
        {
            const Int64 b = 128;

            Int64Parameter p = new Int64Parameter();

            p.Traits.Count.Should().BeLessOrEqualTo(1);

            p.Value = b;

            p.Traits.Count.Should().BeLessOrEqualTo(1);
            p.TryGetTrait<ValueTrait<Int64>>(out ValueTrait<Int64> trait).Should().BeTrue();
            trait.Should().NotBeNull();
            trait.Value.Should().Be(b);
        }

        [Fact]
        public void After_SettingValue_ValueTrait_IsAvailable_For_UInt16Parameter()
        {
            const UInt16 b = 128;

            UInt16Parameter p = new UInt16Parameter();

            p.Traits.Count.Should().BeLessOrEqualTo(1);

            p.Value = b;

            p.Traits.Count.Should().BeLessOrEqualTo(1);
            p.TryGetTrait<ValueTrait<UInt16>>(out ValueTrait<UInt16> trait).Should().BeTrue();
            trait.Should().NotBeNull();
            trait.Value.Should().Be(b);
        }

        [Fact]
        public void After_SettingValue_ValueTrait_IsAvailable_For_UInt32Parameter()
        {
            const UInt32 b = 128;

            UInt32Parameter p = new UInt32Parameter();

            p.Traits.Count.Should().BeLessOrEqualTo(1);

            p.Value = b;

            p.Traits.Count.Should().BeLessOrEqualTo(1);
            p.TryGetTrait<ValueTrait<UInt32>>(out ValueTrait<UInt32> trait).Should().BeTrue();
            trait.Should().NotBeNull();
            trait.Value.Should().Be(b);
        }

        [Fact]
        public void After_SettingValue_ValueTrait_IsAvailable_For_UInt64Parameter()
        {
            const UInt64 b = 128;

            UInt64Parameter p = new UInt64Parameter();

            p.Traits.Count.Should().BeLessOrEqualTo(1);

            p.Value = b;

            p.Traits.Count.Should().BeLessOrEqualTo(1);
            p.TryGetTrait<ValueTrait<UInt64>>(out ValueTrait<UInt64> trait).Should().BeTrue();
            trait.Should().NotBeNull();
            trait.Value.Should().Be(b);
        }

        [Fact]
        public void After_SettingValue_ValueTrait_IsAvailable_For_SingleParameter()
        {
            const Single b = 128;

            SingleParameter p = new SingleParameter();

            p.Traits.Count.Should().BeLessOrEqualTo(1);

            p.Value = b;

            p.Traits.Count.Should().BeLessOrEqualTo(1);
            p.TryGetTrait<ValueTrait<Single>>(out ValueTrait<Single> trait).Should().BeTrue();
            trait.Should().NotBeNull();
            trait.Value.Should().Be(b);
        }

        [Fact]
        public void After_SettingValue_ValueTrait_IsAvailable_For_DoubleParameter()
        {
            const Double b = 128;

            DoubleParameter p = new DoubleParameter();

            p.Traits.Count.Should().BeLessOrEqualTo(1);

            p.Value = b;

            p.Traits.Count.Should().BeLessOrEqualTo(1);
            p.TryGetTrait<ValueTrait<Double>>(out ValueTrait<Double> trait).Should().BeTrue();
            trait.Should().NotBeNull();
            trait.Value.Should().Be(b); 
        }
    }
}
