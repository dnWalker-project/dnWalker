using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.InterpreterTests.Symbolic
{
    public class ConversionTest : SymbolicInterpreterTestBase
    {
        public ConversionTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        // each conversion:
        // - bool
        // - other int number
        // - same int number - if to int
        // - other float number
        // - same float number - if to float

        #region CONV_I
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "Convert(x, Int32)")] // should be based on the architecture!!!
        public void Test_CONV_I__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new(object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")] // should be based on the architecture!!!
        public void Test_CONV_I__Int32(int arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "Convert(x, Int32)")] // should be based on the architecture!!!
        public void Test_CONV_I__Byte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "Convert(x, Int32)")] // should be based on the architecture!!!
        public void Test_CONV_I__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_I

        #region CONV_I1
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "Convert(x, SByte)")]
        public void Test_CONV_I1__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "Convert(x, SByte)")]
        public void Test_CONV_I1__Int32(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")] 
        public void Test_CONV_I1__SByte(sbyte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "Convert(x, SByte)")] 
        public void Test_CONV_I1__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_I1

        #region CONV_I2
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "Convert(x, Int16)")]
        public void Test_CONV_I2__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "Convert(x, Int16)")]
        public void Test_CONV_I2__Int32(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")]
        public void Test_CONV_I2__Int16(short arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "Convert(x, Int16)")]
        public void Test_CONV_I2__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_I2

        #region CONV_I4
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "Convert(x, Int32)")] 
        public void Test_CONV_I4__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")]
        public void Test_CONV_I4__Int32(int arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "Convert(x, Int32)")]
        public void Test_CONV_I4__SByte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "Convert(x, Int32)")] 
        public void Test_CONV_I4__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_I4

        #region CONV_I8
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "Convert(x, Int64)")]
        public void Test_CONV_I8__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")]
        public void Test_CONV_I8__Int64(long arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "Convert(x, Int64)")]
        public void Test_CONV_I8__SByte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "Convert(x, Int64)")]
        public void Test_CONV_I8__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_I8



        #region CONV_OVF_I
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "ConvertChecked(x, Int32)")] // should be based on the architecture!!!
        public void Test_CONV_OVF_I__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")] // should be based on the architecture!!!
        public void Test_CONV_OVF_I__Int32(int arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "ConvertChecked(x, Int32)")] // should be based on the architecture!!!
        public void Test_CONV_OVF_I__Byte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "ConvertChecked(x, Int32)")] // should be based on the architecture!!!
        public void Test_CONV_OVF_I__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_OVF_I

        #region CONV_OVF_I1
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "ConvertChecked(x, SByte)")]
        public void Test_CONV_OVF_I1__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "ConvertChecked(x, SByte)")]
        public void Test_CONV_OVF_I1__Int32(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")]
        public void Test_CONV_OVF_I1__SByte(sbyte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "ConvertChecked(x, SByte)")]
        public void Test_CONV_OVF_I1__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_OVF_I1

        #region CONV_OVF_I2
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "ConvertChecked(x, Int16)")]
        public void Test_CONV_OVF_I2__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "ConvertChecked(x, Int16)")]
        public void Test_CONV_OVF_I2__Int32(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")]
        public void Test_CONV_OVF_I2__Int16(short arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "ConvertChecked(x, Int16)")]
        public void Test_CONV_OVF_I2__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_OVF_I2

        #region CONV_OVF_I4
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "ConvertChecked(x, Int32)")]
        public void Test_CONV_OVF_I4__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")]
        public void Test_CONV_OVF_I4__Int32(int arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "ConvertChecked(x, Int32)")]
        public void Test_CONV_OVF_I4__SByte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "ConvertChecked(x, Int32)")]
        public void Test_CONV_OVF_I4__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_OVF_I4

        #region CONV_OVF_I8
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "ConvertChecked(x, Int64)")]
        public void Test_CONV_OVF_I8__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")]
        public void Test_CONV_OVF_I8__Int64(long arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "ConvertChecked(x, Int64)")]
        public void Test_CONV_OVF_I8__SByte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "ConvertChecked(x, Int64)")]
        public void Test_CONV_OVF_I8__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_OVF_I8



        #region CONV_OVF_I_UN
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "ConvertChecked(x, Int32)")] // should be based on the architecture!!!
        public void Test_CONV_OVF_I_UN__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")] // should be based on the architecture!!!
        public void Test_CONV_OVF_I_UN__Int32(int arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "ConvertChecked(x, Int32)")] // should be based on the architecture!!!
        public void Test_CONV_OVF_I_UN__Byte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "ConvertChecked(x, Int32)")] // should be based on the architecture!!!
        public void Test_CONV_OVF_I_UN__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_OVF_I_UN

        #region CONV_OVF_I1_UN
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "ConvertChecked(x, SByte)")]
        public void Test_CONV_OVF_I1_UN__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "ConvertChecked(x, SByte)")]
        public void Test_CONV_OVF_I1_UN__Int32(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")]
        public void Test_CONV_OVF_I1_UN__SByte(sbyte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "ConvertChecked(x, SByte)")]
        public void Test_CONV_OVF_I1_UN__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_OVF_I1_UN

        #region CONV_OVF_I2_UN
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "ConvertChecked(x, Int16)")]
        public void Test_CONV_OVF_I2_UN__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "ConvertChecked(x, Int16)")]
        public void Test_CONV_OVF_I2_UN__Int32(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")]
        public void Test_CONV_OVF_I2_UN__Int16(short arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "ConvertChecked(x, Int16)")]
        public void Test_CONV_OVF_I2_UN__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_OVF_I2_UN

        #region CONV_OVF_I4_UN
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "ConvertChecked(x, Int32)")]
        public void Test_CONV_OVF_I4_UN__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")]
        public void Test_CONV_OVF_I4_UN__Int32(int arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "ConvertChecked(x, Int32)")]
        public void Test_CONV_OVF_I4_UN__SByte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "ConvertChecked(x, Int32)")]
        public void Test_CONV_OVF_I4_UN__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_OVF_I4_UN

        #region CONV_OVF_I8_UN
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "ConvertChecked(x, Int64)")]
        public void Test_CONV_OVF_I8_UN__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")]
        public void Test_CONV_OVF_I8_UN__Int64(long arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "ConvertChecked(x, Int64)")]
        public void Test_CONV_OVF_I8_UN__SByte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "ConvertChecked(x, Int64)")]
        public void Test_CONV_OVF_I8_UN__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_OVF_I8_UN


        #region CONV_U
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "Convert(x, UInt32)")] // should be based on the architecture!!!
        public void Test_CONV_U__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")] // should be based on the architecture!!!
        public void Test_CONV_U__UInt32(uint arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "Convert(x, UInt32)")] // should be based on the architecture!!!
        public void Test_CONV_U__Byte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "Convert(x, UInt32)")] // should be based on the architecture!!!
        public void Test_CONV_U__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_U

        #region CONV_U1
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "Convert(x, Byte)")]
        public void Test_CONV_U1__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "Convert(x, Byte)")]
        public void Test_CONV_U1__Int32(int arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")]
        public void Test_CONV_U1__Byte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "Convert(x, Byte)")]
        public void Test_CONV_U1__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_U1

        #region CONV_U2
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "Convert(x, UInt16)")]
        public void Test_CONV_U2__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "Convert(x, UInt16)")]
        public void Test_CONV_U2__Int32(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")]
        public void Test_CONV_U2__UInt16(ushort arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "Convert(x, UInt16)")]
        public void Test_CONV_U2__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_U2

        #region CONV_U4
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "Convert(x, UInt32)")]
        public void Test_CONV_U4__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")]
        public void Test_CONV_U4__UInt32(uint arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "Convert(x, UInt32)")]
        public void Test_CONV_U4__SByte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "Convert(x, UInt32)")]
        public void Test_CONV_U4__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_U4

        #region CONV_U8
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "Convert(x, UInt64)")]
        public void Test_CONV_U8__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")]
        public void Test_CONV_U8__UInt64(ulong arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "Convert(x, UInt64)")]
        public void Test_CONV_U8__SByte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "Convert(x, UInt64)")]
        public void Test_CONV_U8__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_U8



        #region CONV_OVF_U
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "ConvertChecked(x, UInt32)")] // should be based on the architecture!!!
        public void Test_CONV_OVF_U__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")] // should be based on the architecture!!!
        public void Test_CONV_OVF_U__UInt32(uint arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "ConvertChecked(x, UInt32)")] // should be based on the architecture!!!
        public void Test_CONV_OVF_U__Byte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "ConvertChecked(x, UInt32)")] // should be based on the architecture!!!
        public void Test_CONV_OVF_U__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_OVF_U

        #region CONV_OVF_U1
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "ConvertChecked(x, Byte)")]
        public void Test_CONV_OVF_U1__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "ConvertChecked(x, Byte)")]
        public void Test_CONV_OVF_U1__Int32(int arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")]
        public void Test_CONV_OVF_U1__Byte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "ConvertChecked(x, Byte)")]
        public void Test_CONV_OVF_U1__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_OVF_U1

        #region CONV_OVF_U2
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "ConvertChecked(x, UInt16)")]
        public void Test_CONV_OVF_U2__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "ConvertChecked(x, UInt16)")]
        public void Test_CONV_OVF_U2__Int32(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")]
        public void Test_CONV_OVF_U2__UInt16(ushort arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "ConvertChecked(x, UInt16)")]
        public void Test_CONV_OVF_U2__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_OVF_U2

        #region CONV_OVF_U4
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "ConvertChecked(x, UInt32)")]
        public void Test_CONV_OVF_U4__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")]
        public void Test_CONV_OVF_U4__UInt32(uint arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "ConvertChecked(x, UInt32)")]
        public void Test_CONV_OVF_U4__SByte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "ConvertChecked(x, UInt32)")]
        public void Test_CONV_OVF_U4__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_OVF_U4

        #region CONV_OVF_U8
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "ConvertChecked(x, UInt64)")]
        public void Test_CONV_OVF_U8__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")]
        public void Test_CONV_OVF_U8__UInt64(ulong arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "ConvertChecked(x, UInt64)")]
        public void Test_CONV_OVF_U8__SByte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "ConvertChecked(x, UInt64)")]
        public void Test_CONV_OVF_U8__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_OVF_U8



        #region CONV_OVF_U_UN
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "ConvertChecked(x, UInt32)")] // should be based on the architecture!!!
        public void Test_CONV_OVF_U_UN__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")] // should be based on the architecture!!!
        public void Test_CONV_OVF_U_UN__UInt32(uint arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "ConvertChecked(x, UInt32)")] // should be based on the architecture!!!
        public void Test_CONV_OVF_U_UN__Byte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "ConvertChecked(x, UInt32)")] // should be based on the architecture!!!
        public void Test_CONV_OVF_U_UN__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_OVF_U_UN

        #region CONV_OVF_U1_UN
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "ConvertChecked(x, Byte)")]
        public void Test_CONV_OVF_U1_UN__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "ConvertChecked(x, Byte)")]
        public void Test_CONV_OVF_U1_UN__Int32(int arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")]
        public void Test_CONV_OVF_U1_UN__Byte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "ConvertChecked(x, Byte)")]
        public void Test_CONV_OVF_U1_UN__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_OVF_U1_UN

        #region CONV_OVF_U2_UN
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "ConvertChecked(x, UInt16)")]
        public void Test_CONV_OVF_U2_UN__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "ConvertChecked(x, UInt16)")]
        public void Test_CONV_OVF_U2_UN__Int32(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")]
        public void Test_CONV_OVF_U2_UN__UInt16(ushort arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "ConvertChecked(x, UInt16)")]
        public void Test_CONV_OVF_U2_UN__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_OVF_U2_UN

        #region CONV_OVF_U4_UN
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "ConvertChecked(x, UInt32)")]
        public void Test_CONV_OVF_U4_UN__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")]
        public void Test_CONV_OVF_U4_UN__UInt32(uint arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "ConvertChecked(x, UInt32)")]
        public void Test_CONV_OVF_U4_UN__SByte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "ConvertChecked(x, UInt32)")]
        public void Test_CONV_OVF_U4_UN__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_OVF_U4_UN

        #region CONV_OVF_U8_UN
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "ConvertChecked(x, UInt64)")]
        public void Test_CONV_OVF_U8_UN__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "x")]
        public void Test_CONV_OVF_U8_UN__UInt64(ulong arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "ConvertChecked(x, UInt64)")]
        public void Test_CONV_OVF_U8_UN__SByte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "ConvertChecked(x, UInt64)")]
        public void Test_CONV_OVF_U8_UN__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_OVF_U8_UN



        #region CONV_R_UN
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "Convert(x, Double)")] // should be based on the architecture!!!
        public void Test_CONV_R_UN__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "x")] // should be based on the architecture!!!
        public void Test_CONV_R_UN__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "Convert(x, Double)")] // should be based on the architecture!!!
        public void Test_CONV_R_UN__Byte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0f, null, null)]
        [InlineData(5.0f, "x", "Convert(x, Double)")] // should be based on the architecture!!!
        public void Test_CONV_R_UN__Single(float arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_R_UN

        #region CONV_R4
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "Convert(x, Single)")]
        public void Test_CONV_R4__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0f, null, null)]
        [InlineData(5.0f, "x", "x")]
        public void Test_CONV_R4__Single(float arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "Convert(x, Single)")]
        public void Test_CONV_R4__Byte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "Convert(x, Single)")]
        public void Test_CONV_R4__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_R4

        #region CONV_R8
        [Theory]
        [InlineData(true, null, null)]
        [InlineData(true, "x", "Convert(x, Double)")]
        public void Test_CONV_R8__Boolean(bool arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0, null, null)]
        [InlineData(5.0, "x", "x")] 
        public void Test_CONV_R8__Double(double arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5, null, null)]
        [InlineData(5, "x", "Convert(x, Double)")] 
        public void Test_CONV_R8__Byte(byte arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }

        [Theory]
        [InlineData(5.0f, null, null)]
        [InlineData(5.0f, "x", "Convert(x, Double)")]
        public void Test_CONV_R8__Single(float arg, string name, string resultExpr)
        {
            Test(Utils.ThisMethodName(), new (object, string)[] { (arg, name) }, null, resultExpr);
        }
        #endregion CONV_R8
    }
}
