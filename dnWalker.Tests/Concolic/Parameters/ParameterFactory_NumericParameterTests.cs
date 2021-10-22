using dnWalker.Concolic.Parameters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Tests.Concolic.Parameters
{
    public class ParameterFactory_NumericParameterTests : dnlibTypeTestBase
    {
        [Fact]
        public void Test_ParameterFor_Boolean_Is_BooleanParameter()
        {
            var p = ParameterFactory.CreateParameter(GetType(typeof(bool)), "x");

            Assert.IsType<BooleanParameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_Char_Is_CharParameter()
        {
            var p = ParameterFactory.CreateParameter(GetType(typeof(char)), "x");

            Assert.IsType<CharParameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_Byte_Is_ByteParameter()
        {
            var p = ParameterFactory.CreateParameter(GetType(typeof(byte)), "x");

            Assert.IsType<ByteParameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_SByte_Is_SByteParameter()
        {
            var p = ParameterFactory.CreateParameter(GetType(typeof(sbyte)), "x");

            Assert.IsType<SByteParameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_Int16_Is_Int16Parameter()
        {
            var p = ParameterFactory.CreateParameter(GetType(typeof(short)), "x");

            Assert.IsType<Int16Parameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_Int32_Is_Int32Parameter()
        {
            var p = ParameterFactory.CreateParameter(GetType(typeof(int)), "x");

            Assert.IsType<Int32Parameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_Int64_Is_Int64Parameter()
        {
            var p = ParameterFactory.CreateParameter(GetType(typeof(long)), "x");

            Assert.IsType<Int64Parameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_UInt16_Is_UInt16Parameter()
        {
            var p = ParameterFactory.CreateParameter(GetType(typeof(ushort)), "x");

            Assert.IsType<UInt16Parameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_UInt32_Is_UInt32Parameter()
        {
            var p = ParameterFactory.CreateParameter(GetType(typeof(uint)), "x");

            Assert.IsType<UInt32Parameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_UInt64_Is_UInt64Parameter()
        {
            var p = ParameterFactory.CreateParameter(GetType(typeof(ulong)), "x");

            Assert.IsType<UInt64Parameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_Single_Is_SingleParameter()
        {
            var p = ParameterFactory.CreateParameter(GetType(typeof(float)), "x");

            Assert.IsType<SingleParameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_Double_Is_DoubleParameter()
        {
            var p = ParameterFactory.CreateParameter(GetType(typeof(double)), "x");

            Assert.IsType<DoubleParameter>(p);
        }
    }
}
