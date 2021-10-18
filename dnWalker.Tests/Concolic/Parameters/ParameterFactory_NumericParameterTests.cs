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
            Parameter p = ParameterFactory.CreateParameter("x", GetType(typeof(Boolean)));

            Assert.IsType<BooleanParameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_Char_Is_CharParameter()
        {
            Parameter p = ParameterFactory.CreateParameter("x", GetType(typeof(Char)));

            Assert.IsType<CharParameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_Byte_Is_ByteParameter()
        {
            Parameter p = ParameterFactory.CreateParameter("x", GetType(typeof(Byte)));

            Assert.IsType<ByteParameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_SByte_Is_SByteParameter()
        {
            Parameter p = ParameterFactory.CreateParameter("x", GetType(typeof(SByte)));

            Assert.IsType<SByteParameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_Int16_Is_Int16Parameter()
        {
            Parameter p = ParameterFactory.CreateParameter("x", GetType(typeof(Int16)));

            Assert.IsType<Int16Parameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_Int32_Is_Int32Parameter()
        {
            Parameter p = ParameterFactory.CreateParameter("x", GetType(typeof(Int32)));

            Assert.IsType<Int32Parameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_Int64_Is_Int64Parameter()
        {
            Parameter p = ParameterFactory.CreateParameter("x", GetType(typeof(Int64)));

            Assert.IsType<Int64Parameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_UInt16_Is_UInt16Parameter()
        {
            Parameter p = ParameterFactory.CreateParameter("x", GetType(typeof(UInt16)));

            Assert.IsType<UInt16Parameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_UInt32_Is_UInt32Parameter()
        {
            Parameter p = ParameterFactory.CreateParameter("x", GetType(typeof(UInt32)));

            Assert.IsType<UInt32Parameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_UInt64_Is_UInt64Parameter()
        {
            Parameter p = ParameterFactory.CreateParameter("x", GetType(typeof(UInt64)));

            Assert.IsType<UInt64Parameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_Single_Is_SingleParameter()
        {
            Parameter p = ParameterFactory.CreateParameter("x", GetType(typeof(Single)));

            Assert.IsType<SingleParameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_Double_Is_DoubleParameter()
        {
            Parameter p = ParameterFactory.CreateParameter("x", GetType(typeof(Double)));

            Assert.IsType<DoubleParameter>(p);
        }
    }
}
