using dnWalker.Parameters;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Tests.Parameters
{
    public class ParameterFactory_NumericParameterTests : dnlibTypeTestBase
    {
        public void Test_ParameterFor_Boolean_Is_BooleanParameter()
        {
            IParameterContext ctx = new ParameterContext();
            IParameter p = ctx.CreateParameter(GetType("System.Boolean"));
            p.Should().BeOfType<BooleanParameter>();
        }
 
        public void Test_ParameterFor_Char_Is_CharParameter()
        {
            IParameterContext ctx = new ParameterContext();
            IParameter p = ctx.CreateParameter(GetType("System.Char"));
            p.Should().BeOfType<CharParameter>();
        }
 
        public void Test_ParameterFor_Byte_Is_ByteParameter()
        {
            IParameterContext ctx = new ParameterContext();
            IParameter p = ctx.CreateParameter(GetType("System.Byte"));
            p.Should().BeOfType<ByteParameter>();
        }
 
        public void Test_ParameterFor_SByte_Is_SByteParameter()
        {
            IParameterContext ctx = new ParameterContext();
            IParameter p = ctx.CreateParameter(GetType("System.SByte"));
            p.Should().BeOfType<SByteParameter>();
        }
 
        public void Test_ParameterFor_Int16_Is_Int16Parameter()
        {
            IParameterContext ctx = new ParameterContext();
            IParameter p = ctx.CreateParameter(GetType("System.Int16"));
            p.Should().BeOfType<Int16Parameter>();
        }
 
        public void Test_ParameterFor_Int32_Is_Int32Parameter()
        {
            IParameterContext ctx = new ParameterContext();
            IParameter p = ctx.CreateParameter(GetType("System.Int32"));
            p.Should().BeOfType<Int32Parameter>();
        }
 
        public void Test_ParameterFor_Int64_Is_Int64Parameter()
        {
            IParameterContext ctx = new ParameterContext();
            IParameter p = ctx.CreateParameter(GetType("System.Int64"));
            p.Should().BeOfType<Int64Parameter>();
        }
 
        public void Test_ParameterFor_UInt16_Is_UInt16Parameter()
        {
            IParameterContext ctx = new ParameterContext();
            IParameter p = ctx.CreateParameter(GetType("System.UInt16"));
            p.Should().BeOfType<UInt16Parameter>();
        }
 
        public void Test_ParameterFor_UInt32_Is_UInt32Parameter()
        {
            IParameterContext ctx = new ParameterContext();
            IParameter p = ctx.CreateParameter(GetType("System.UInt32"));
            p.Should().BeOfType<UInt32Parameter>();
        }
 
        public void Test_ParameterFor_UInt64_Is_UInt64Parameter()
        {
            IParameterContext ctx = new ParameterContext();
            IParameter p = ctx.CreateParameter(GetType("System.UInt64"));
            p.Should().BeOfType<UInt64Parameter>();
        }
 
        public void Test_ParameterFor_Single_Is_SingleParameter()
        {
            IParameterContext ctx = new ParameterContext();
            IParameter p = ctx.CreateParameter(GetType("System.Single"));
            p.Should().BeOfType<SingleParameter>();
        }
 
        public void Test_ParameterFor_Double_Is_DoubleParameter()
        {
            IParameterContext ctx = new ParameterContext();
            IParameter p = ctx.CreateParameter(GetType("System.Double"));
            p.Should().BeOfType<DoubleParameter>();
        }
 
    }
}
