/*
    Copyright (C) 2014-2019 de4dot@gmail.com

    This file is part of dnSpy

    dnSpy is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    dnSpy is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with dnSpy.  If not, see <http://www.gnu.org/licenses/>.
*/

// The tests should be executed in 32-bit and 64-bit mode since IntPtr is either 4 or 8 bytes in size

// Test exceptions (slower)
//#define EXCEPTIONS

using dnWalker.Tests;
using MMC;
using System;
using Xunit;

namespace dnSpy.Debugger.DotNet.Interpreter.Tests
{
    [Trait("Category", "Interpreter")]
    public sealed class InterpreterTest : TestBase
    {
        private const string AssemblyFilename = @"..\..\..\extras\dnSpy.Debugger.DotNet.Interpreter.Tests.dll";

        protected static Lazy<DefinitionProvider> Lazy =
            new Lazy<DefinitionProvider>(() => DefinitionProvider.Create(GetAssemblyLoader(AssemblyFilename)));

        public InterpreterTest() : base(Lazy.Value)
        {
            _config.StateStorageSize = 5;
        }

        private new void Test(string methodName, params object[] args)
        {
            methodName = "dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass." + methodName;
            TestAndCompare(methodName, args);
        }

        [Theory]
        [InlineData(int.MinValue, int.MaxValue)] // [InlineData(int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(int.MaxValue, int.MinValue)] // [InlineData(int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(-5, 4)]
        [InlineData(4, -5)]
        [InlineData(int.MinValue, 0)]
        public void Test_ADD__Int32_IntPtr(object arg0, int arg1) { Test("Test_ADD__Int32_IntPtr", arg0, new IntPtr(arg1)); }

        [Theory]
        [InlineData(int.MinValue, int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue)]
        [InlineData(int.MaxValue, int.MinValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue)]
        [InlineData(-5, 4)]
        [InlineData(4, -5)]
        [InlineData(0, int.MinValue)]
        public void Test_ADD__IntPtr_Int32(int arg0, object arg1) { Test("Test_ADD__IntPtr_Int32", new IntPtr(arg0), arg1); }

        [Theory]
        [InlineData(int.MinValue, int.MaxValue)] // [InlineData(int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(int.MaxValue, int.MinValue)] // [InlineData(int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(-5, 4)]
        [InlineData(4, -5)]
        [InlineData(int.MinValue, 0)]
        public void Test_ADD_OVF__Int32_IntPtr(object arg0, int arg1) { Test("Test_ADD_OVF__Int32_IntPtr", arg0, new IntPtr(arg1)); }

        [Theory]
        [InlineData(int.MinValue, int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue)]
        [InlineData(int.MaxValue, int.MinValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue)]
        [InlineData(-5, 4)]
        [InlineData(4, -5)]
        [InlineData(0, int.MinValue)]
        public void Test_ADD_OVF__IntPtr_Int32(int arg0, object arg1) { Test("Test_ADD_OVF__IntPtr_Int32", new IntPtr(arg0), arg1); }

        [Theory]
        [InlineData(int.MinValue, int.MaxValue)] // [InlineData(int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(int.MaxValue, int.MinValue)] // [InlineData(int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(-5, 4)]
        [InlineData(4, -5)]
        [InlineData(int.MinValue, 0)]
        public void Test_ADD_OVF_UN__Int32_IntPtr(object arg0, int arg1) { Test("Test_ADD_OVF_UN__Int32_IntPtr", arg0, new IntPtr(arg1)); }

        [Theory]
        [InlineData(int.MinValue, int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue)]
        [InlineData(int.MaxValue, int.MinValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue)]
        [InlineData(-5, 4)]
        [InlineData(4, -5)]
        [InlineData(0, int.MinValue)]
        public void Test_ADD_OVF_UN__IntPtr_Int32(int arg0, object arg1) { Test("Test_ADD_OVF_UN__IntPtr_Int32", new IntPtr(arg0), arg1); }

        [Theory]
        [InlineData(int.MinValue, int.MaxValue)]
        [InlineData(int.MaxValue, int.MinValue)]
        [InlineData(-5, 4)]
        [InlineData(4, -5)]
        [InlineData(0, int.MinValue)]
        public void Test_ADD_OVF_UN__Int32(object arg0, object arg1) { Test("Test_ADD_OVF_UN__Int32", arg0, arg1); }

        [Theory]
        [InlineData(long.MinValue, long.MaxValue)]
        [InlineData(long.MaxValue, long.MinValue)]
        [InlineData(-5L, 4L)]
        [InlineData(4L, -5L)]
        [InlineData(0L, long.MinValue)]
        public void Test_ADD_OVF_UN__Int64(long arg0, long arg1) { Test("Test_ADD_OVF_UN__Int64", arg0, arg1); }

        [Theory]
        [InlineData(double.MinValue, double.MaxValue)]
        [InlineData(double.MaxValue, double.MinValue)]
        [InlineData(-5d, 4d)]
        [InlineData(4d, -5d)]
        [InlineData(42, -5d)]
        [InlineData(48, 55)]
        [InlineData(0d, double.MinValue)]
        [InlineData(0.0d, double.Epsilon)]
        [InlineData(0, double.PositiveInfinity)]
        [InlineData(double.PositiveInfinity, 0)]
        [InlineData(0, double.NegativeInfinity)]
        [InlineData(double.NegativeInfinity, 0)]
        [InlineData(double.NegativeInfinity, double.PositiveInfinity)]
        public void Test_ADD_OVF_UN__Double(object arg0, object arg1) { Test("Test_ADD_OVF_UN__Double", arg0, arg1); }

        [Fact]
        public void Test_AND__Int32() { Test("Test_AND__Int32", 0x5AA51234, 0x3FF37591); }

        [Fact]
        public void Test_AND__Int32_IntPtr() { Test("Test_AND__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); }

        [Fact]
        public void Test_AND__Int64() { Test("Test_AND__Int64", 0x5AA5123467306AB8L, 0x3FF375919AE00BB6L); }

        [Fact]
        public void Test_AND__IntPtr() { Test("Test_AND__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x5AA51234) : new IntPtr(0x5AA5123467306AB8L), IntPtr.Size == 4 ? new IntPtr(0x3FF37591) : new IntPtr(0x3FF375919AE00BB6L)); }

        [Fact]
        public void Test_AND__IntPtr_Int32() { Test("Test_AND__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); }

        [Fact]
        public void Test_BEQ() { Test("Test_BEQ"); }

        [Fact]
        public void Test_BEQ_S() { Test("Test_BEQ_S"); }

        [Fact]
        public void Test_BGE() { Test("Test_BGE"); }

        [Fact]
        public void Test_BGE_S() { Test("Test_BGE_S"); }

        [Fact]
        public void Test_BGE_UN() { Test("Test_BGE_UN"); }

        [Fact]
        public void Test_BGE_UN_S() { Test("Test_BGE_UN_S"); }

        [Fact]
        public void Test_BGT() { Test("Test_BGT"); }

        [Fact]
        public void Test_BGT_S() { Test("Test_BGT_S"); }

        [Fact]
        public void Test_BGT_UN() { Test("Test_BGT_UN"); }

        [Fact]
        public void Test_BGT_UN_S() { Test("Test_BGT_UN_S"); }

        [Fact]
        public void Test_BLE() { Test("Test_BLE"); }

        [Fact]
        public void Test_BLE_S() { Test("Test_BLE_S"); }

        [Fact]
        public void Test_BLE_UN() { Test("Test_BLE_UN"); }

        [Fact]
        public void Test_BLE_UN_S() { Test("Test_BLE_UN_S"); }

        [Fact]
        public void Test_BLT() { Test("Test_BLT"); }

        [Fact]
        public void Test_BLT_S() { Test("Test_BLT_S"); }

        [Fact]
        public void Test_BLT_UN() { Test("Test_BLT_UN"); }

        [Fact]
        public void Test_BLT_UN_S() { Test("Test_BLT_UN_S"); }

        [Fact]
        public void Test_BNE_UN() { Test("Test_BNE_UN"); }

        [Fact]
        public void Test_BNE_UN_S() { Test("Test_BNE_UN_S"); }

        [Fact]
        public void Test_BOX_UNBOX() { Test("Test_BOX_UNBOX"); }

        [Fact]
        public void Test_BR() { Test("Test_BR"); }

        [Fact]
        public void Test_BR_S() { Test("Test_BR_S"); }

        [Fact]
        public void Test_BRFALSE() { Test("Test_BRFALSE"); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(1.0d)]
        public void Test_BRFALSE__Double(object arg0) { Test("Test_BRFALSE__Double", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Test_BRFALSE__Int32(object arg0) { Test("Test_BRFALSE__Int32", arg0); }

        [Theory]
        [InlineData(0L)]
        [InlineData(1L)]
        public void Test_BRFALSE__Int64(object arg0) { Test("Test_BRFALSE__Int64", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Test_BRFALSE__IntPtr(int arg0) { Test("Test_BRFALSE__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(new object[] { null })]
        [InlineData("hello")]
        public void Test_BRFALSE__Object(object arg0) { Test("Test_BRFALSE__Object", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(1.0f)]
        public void Test_BRFALSE__Single(object arg0) { Test("Test_BRFALSE__Single", arg0); }

        [Fact]
        public void Test_BRFALSE_S() { Test("Test_BRFALSE_S"); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(1.0d)]
        public void Test_BRFALSE_S__Double(object arg0) { Test("Test_BRFALSE_S__Double", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Test_BRFALSE_S__Int32(object arg0) { Test("Test_BRFALSE_S__Int32", arg0); }

        [Theory]
        [InlineData(0L)]
        [InlineData(1L)]
        public void Test_BRFALSE_S__Int64(object arg0) { Test("Test_BRFALSE_S__Int64", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Test_BRFALSE_S__IntPtr(int arg0) { Test("Test_BRFALSE_S__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(new object[] { null })]
        [InlineData("hello")]
        public void Test_BRFALSE_S__Object(object arg0) { Test("Test_BRFALSE_S__Object", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(1.0f)]
        public void Test_BRFALSE_S__Single(object arg0) { Test("Test_BRFALSE_S__Single", arg0); }

        [Fact]
        public void Test_BRTRUE() { Test("Test_BRTRUE"); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(1.0d)]
        public void Test_BRTRUE__Double(object arg0) { Test("Test_BRTRUE__Double", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Test_BRTRUE__Int32(object arg0) { Test("Test_BRTRUE__Int32", arg0); }

        [Theory]
        [InlineData(0L)]
        [InlineData(1L)]
        public void Test_BRTRUE__Int64(object arg0) { Test("Test_BRTRUE__Int64", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Test_BRTRUE__IntPtr(int arg0) { Test("Test_BRTRUE__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(new object[] { null })]
        [InlineData("hello")]
        public void Test_BRTRUE__Object(object arg0) { Test("Test_BRTRUE__Object", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(1.0f)]
        public void Test_BRTRUE__Single(object arg0) { Test("Test_BRTRUE__Single", arg0); }

        [Fact]
        public void Test_BRTRUE_S() { Test("Test_BRTRUE_S"); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(1.0d)]
        public void Test_BRTRUE_S__Double(object arg0) { Test("Test_BRTRUE_S__Double", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Test_BRTRUE_S__Int32(object arg0) { Test("Test_BRTRUE_S__Int32", arg0); }

        [Theory]
        [InlineData(0L)]
        [InlineData(1L)]
        public void Test_BRTRUE_S__Int64(object arg0) { Test("Test_BRTRUE_S__Int64", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Test_BRTRUE_S__IntPtr(int arg0) { Test("Test_BRTRUE_S__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(new object[] { null })]
        [InlineData("hello")]
        public void Test_BRTRUE_S__Object(object arg0) { Test("Test_BRTRUE_S__Object", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(1.0f)]
        public void Test_BRTRUE_S__Single(object arg0) { Test("Test_BRTRUE_S__Single", arg0); }

        [Fact]
        public void Test_CALL__Instance_Class() { Test("Test_CALL__Instance_Class"); }

        [Fact]
        public void Test_CALL__Instance_Struct() { Test("Test_CALL__Instance_Struct"); }

        [Fact]
        public void Test_CALL__Static_Class() { Test("Test_CALL__Static_Class"); }

        [Fact]
        public void Test_CALL__Static_Struct() { Test("Test_CALL__Static_Struct"); }

        [Fact]
        public void Test_CASTCLASS() { Test("Test_CASTCLASS"); }

        [Theory]
        [InlineData(1.0d, 1.0d)]
        [InlineData(0.0d, 1.0d)]
        [InlineData(1.0d, 0.0d)]
        public void Test_CEQ__Double(object arg0, object arg1) { Test("Test_CEQ__Double", arg0, arg1); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, -1)]
        public void Test_CEQ__Int32(object arg0, object arg1) { Test("Test_CEQ__Int32", arg0, arg1); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void Test_CEQ__Int32_IntPtr(object arg0, int arg1) { Test("Test_CEQ__Int32_IntPtr", arg0, new IntPtr(arg1)); }

        [Theory]
        [InlineData(1L, 1L)]
        [InlineData(0L, 1L)]
        [InlineData(1L, -1L)]
        public void Test_CEQ__Int64(object arg0, object arg1) { Test("Test_CEQ__Int64", arg0, arg1); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void Test_CEQ__IntPtr(int arg0, int arg1) { Test("Test_CEQ__IntPtr", new IntPtr(arg0), new IntPtr(arg1)); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void Test_CEQ__IntPtr_Int32(int arg0, object arg1) { Test("Test_CEQ__IntPtr_Int32", new IntPtr(arg0), arg1); }

        [Theory]
        [InlineData(null, null)]
        [InlineData("1", "0")]
        public void Test_CEQ__Object(object arg0, object arg1) { Test("Test_CEQ__Object", arg0, arg1); }

        [Theory]
        [InlineData(1.0f, 1.0f)]
        [InlineData(0.0f, 1.0f)]
        [InlineData(1.0f, 0.0f)]
        public void Test_CEQ__Single(object arg0, object arg1) { Test("Test_CEQ__Single", arg0, arg1); }

        [Theory]
        [InlineData(1U, 1U)]
        [InlineData(0U, 1U)]
        [InlineData(1U, 0U)]
        public void Test_CEQ__UInt32(object arg0, object arg1) { Test("Test_CEQ__UInt32", arg0, arg1); }

        [Theory]
        [InlineData(1UL, 1UL)]
        [InlineData(0UL, 1UL)]
        [InlineData(1UL, 0UL)]
        public void Test_CEQ__UInt64(object arg0, object arg1) { Test("Test_CEQ__UInt64", arg0, arg1); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void Test_CEQ__UIntPtr(uint arg0, uint arg1) { Test("Test_CEQ__UIntPtr", new UIntPtr(arg0), new UIntPtr(arg1)); }

        [Theory]
        [InlineData(1.0d, 1.0d)]
        [InlineData(0.0d, 1.0d)]
        [InlineData(1.0d, 0.0d)]
        public void Test_CGT__Double(object arg0, object arg1) { Test("Test_CGT__Double", arg0, arg1); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, -1)]
        public void Test_CGT__Int32(object arg0, object arg1) { Test("Test_CGT__Int32", arg0, arg1); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void Test_CGT__Int32_IntPtr(object arg0, int arg1) { Test("Test_CGT__Int32_IntPtr", arg0, new IntPtr(arg1)); }

        [Theory]
        [InlineData(1L, 1L)]
        [InlineData(0L, 1L)]
        [InlineData(1L, -1L)]
        public void Test_CGT__Int64(object arg0, object arg1) { Test("Test_CGT__Int64", arg0, arg1); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void Test_CGT__IntPtr(int arg0, int arg1) { Test("Test_CGT__IntPtr", new IntPtr(arg0), new IntPtr(arg1)); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void Test_CGT__IntPtr_Int32(int arg0, object arg1) { Test("Test_CGT__IntPtr_Int32", new IntPtr(arg0), arg1); }

        [Theory]
        [InlineData(1.0f, 1.0f)]
        [InlineData(0.0f, 1.0f)]
        [InlineData(1.0f, 0.0f)]
        public void Test_CGT__Single(object arg0, object arg1) { Test("Test_CGT__Single", arg0, arg1); }

        [Theory]
        [InlineData(1U, 1U)]
        [InlineData(0U, 1U)]
        [InlineData(1U, 0U)]
        public void Test_CGT__UInt32(object arg0, object arg1) { Test("Test_CGT__UInt32", arg0, arg1); }

        [Theory]
        [InlineData(1UL, 1UL)]
        [InlineData(0UL, 1UL)]
        [InlineData(1UL, 0UL)]
        public void Test_CGT__UInt64(object arg0, object arg1) { Test("Test_CGT__UInt64", arg0, arg1); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void Test_CGT__UIntPtr(uint arg0, uint arg1) { Test("Test_CGT__UIntPtr", new UIntPtr(arg0), new UIntPtr(arg1)); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, -1)]
        public void Test_CGT_UN__Int32(object arg0, object arg1) { Test("Test_CGT_UN__Int32", arg0, arg1); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void Test_CGT_UN__Int32_IntPtr(object arg0, int arg1) { Test("Test_CGT_UN__Int32_IntPtr", arg0, new IntPtr(arg1)); }

        [Theory]
        [InlineData(1L, 1L)]
        [InlineData(0L, 1L)]
        [InlineData(1L, -1L)]
        public void Test_CGT_UN__Int64(object arg0, object arg1) { Test("Test_CGT_UN__Int64", arg0, arg1); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void Test_CGT_UN__IntPtr(int arg0, int arg1) { Test("Test_CGT_UN__IntPtr", new IntPtr(arg0), new IntPtr(arg1)); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void Test_CGT_UN__IntPtr_Int32(int arg0, object arg1) { Test("Test_CGT_UN__IntPtr_Int32", new IntPtr(arg0), arg1); }

        [Theory]
        [InlineData(1U, 1U)]
        [InlineData(0U, 1U)]
        [InlineData(1U, 0U)]
        public void Test_CGT_UN__UInt32(object arg0, object arg1) { Test("Test_CGT_UN__UInt32", arg0, arg1); }

        [Theory]
        [InlineData(1UL, 1UL)]
        [InlineData(0UL, 1UL)]
        [InlineData(1UL, 0UL)]
        public void Test_CGT_UN__UInt64(object arg0, object arg1) { Test("Test_CGT_UN__UInt64", arg0, arg1); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void Test_CGT_UN__UIntPtr(uint arg0, uint arg1) { Test("Test_CGT_UN__UIntPtr", new UIntPtr(arg0), new UIntPtr(arg1)); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(double.MaxValue)]
        [InlineData(double.Epsilon)]
        public void Test_CKFINITE__Double(object arg0) { Test("Test_CKFINITE__Double", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(float.MaxValue)]
        [InlineData(float.Epsilon)]
        public void Test_CKFINITE__Single(object arg0) { Test("Test_CKFINITE__Single", arg0); }

        [Theory]
        [InlineData(1.0d, 1.0d)]
        [InlineData(0.0d, 1.0d)]
        [InlineData(1.0d, 0.0d)]
        public void Test_CLT__Double(object arg0, object arg1) { Test("Test_CLT__Double", arg0, arg1); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, -1)]
        public void Test_CLT__Int32(object arg0, object arg1) { Test("Test_CLT__Int32", arg0, arg1); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void Test_CLT__Int32_IntPtr(object arg0, int arg1) { Test("Test_CLT__Int32_IntPtr", arg0, new IntPtr(arg1)); }

        [Theory]
        [InlineData(1L, 1L)]
        [InlineData(0L, 1L)]
        [InlineData(1L, -1L)]
        public void Test_CLT__Int64(object arg0, object arg1) { Test("Test_CLT__Int64", arg0, arg1); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void Test_CLT__IntPtr(int arg0, int arg1) { Test("Test_CLT__IntPtr", new IntPtr(arg0), new IntPtr(arg1)); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void Test_CLT__IntPtr_Int32(int arg0, object arg1) { Test("Test_CLT__IntPtr_Int32", new IntPtr(arg0), arg1); }

        [Theory]
        [InlineData(1.0f, 1.0f)]
        [InlineData(0.0f, 1.0f)]
        [InlineData(1.0f, 0.0f)]
        public void Test_CLT__Single(object arg0, object arg1) { Test("Test_CLT__Single", arg0, arg1); }

        [Theory]
        [InlineData(1U, 1U)]
        [InlineData(0U, 1U)]
        [InlineData(1U, 0U)]
        public void Test_CLT__UInt32(object arg0, object arg1) { Test("Test_CLT__UInt32", arg0, arg1); }

        [Theory]
        [InlineData(1UL, 1UL)]
        [InlineData(0UL, 1UL)]
        [InlineData(1UL, 0UL)]
        public void Test_CLT__UInt64(object arg0, object arg1) { Test("Test_CLT__UInt64", arg0, arg1); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void Test_CLT__UIntPtr(uint arg0, uint arg1) { Test("Test_CLT__UIntPtr", new UIntPtr(arg0), new UIntPtr(arg1)); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, -1)]
        public void Test_CLT_UN__Int32(object arg0, object arg1) { Test("Test_CLT_UN__Int32", arg0, arg1); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void Test_CLT_UN__Int32_IntPtr(object arg0, int arg1) { Test("Test_CLT_UN__Int32_IntPtr", arg0, new IntPtr(arg1)); }

        [Theory]
        [InlineData(1L, 1L)]
        [InlineData(0L, 1L)]
        [InlineData(1L, -1L)]
        public void Test_CLT_UN__Int64(object arg0, object arg1) { Test("Test_CLT_UN__Int64", arg0, arg1); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void Test_CLT_UN__IntPtr(int arg0, int arg1) { Test("Test_CLT_UN__IntPtr", new IntPtr(arg0), new IntPtr(arg1)); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void Test_CLT_UN__IntPtr_Int32(int arg0, object arg1) { Test("Test_CLT_UN__IntPtr_Int32", new IntPtr(arg0), arg1); }

        [Theory]
        [InlineData(1U, 1U)]
        [InlineData(0U, 1U)]
        [InlineData(1U, 0U)]
        public void Test_CLT_UN__UInt32(object arg0, object arg1) { Test("Test_CLT_UN__UInt32", arg0, arg1); }

        [Theory]
        [InlineData(1UL, 1UL)]
        [InlineData(0UL, 1UL)]
        [InlineData(1UL, 0UL)]
        public void Test_CLT_UN__UInt64(object arg0, object arg1) { Test("Test_CLT_UN__UInt64", arg0, arg1); }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void Test_CLT_UN__UIntPtr(uint arg0, uint arg1) { Test("Test_CLT_UN__UIntPtr", new UIntPtr(arg0), new UIntPtr(arg1)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_I__Boolean(object arg0) { Test("Test_CONV_I__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_I__Byte(object arg0) { Test("Test_CONV_I__Byte", arg0); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(-1.0d)]
        [InlineData(double.MinValue)]
        [InlineData(1.0d)]
        [InlineData(double.MaxValue)]
        [InlineData(double.NaN)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.Epsilon)]
        [InlineData(12345678910111213.1415d)]
        [InlineData(-12345678910111213.1415d)]
        public void Test_CONV_I__Double(object arg0) { Test("Test_CONV_I__Double", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_I__Char(object arg0) { Test("Test_CONV_I__Char", arg0); }

        [Theory]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        [InlineData((short)-0x1234)]
        public void Test_CONV_I__Int16(object arg0) { Test("Test_CONV_I__Int16", arg0); }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)-1)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        [InlineData((int)-0x12345678)]
        public void Test_CONV_I__Int32(object arg0) { Test("Test_CONV_I__Int32", arg0); }

        [Theory]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        [InlineData((long)0)]
        [InlineData((long)-1)]
        [InlineData((long)1)]
        [InlineData((long)0x123456789ABCDEF0)]
        [InlineData((long)-0x123456789ABCDEF0)]
        public void Test_CONV_I__Int64(object arg0) { Test("Test_CONV_I__Int64", arg0); }

        [Theory]
        [InlineData(int.MinValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L))]
        [InlineData(-0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L))]
        public void Test_CONV_I__IntPtr(int arg0) { Test("Test_CONV_I__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MinValue)]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)-1)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        [InlineData((sbyte)-0x12)]
        public void Test_CONV_I__SByte(object arg0) { Test("Test_CONV_I__SByte", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(-1.0f)]
        [InlineData(float.MinValue)]
        [InlineData(1.0f)]
        [InlineData(float.MaxValue)]
        [InlineData(float.NaN)]
        [InlineData(float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity)]
        [InlineData(float.Epsilon)]
        [InlineData(1234567891011.12f)]
        [InlineData(-1234567891011.12f)]
        public void Test_CONV_I__Single(object arg0) { Test("Test_CONV_I__Single", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_I__UInt16(object arg0) { Test("Test_CONV_I__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData((uint)0x12345678)]
        [InlineData((uint)0x9ABCDEF0)]
        public void Test_CONV_I__UInt32(object arg0) { Test("Test_CONV_I__UInt32", arg0); }

        [Theory]
        [InlineData(ulong.MinValue)]
        [InlineData(ulong.MaxValue)]
        [InlineData((ulong)0x123456789ABCDEF0)]
        [InlineData((ulong)0x9ABCDEF012345678)]
        public void Test_CONV_I__UInt64(object arg0) { Test("Test_CONV_I__UInt64", arg0); }

        [Theory]
        [InlineData(/*UIntPtr.Size == 4 ? new UIntPtr(*/uint.MinValue/*) : new UIntPtr(ulong.MinValue)*/)]
        [InlineData(/*UIntPtr.Size == 4 ? new UIntPtr(*/uint.MaxValue/*) : new UIntPtr(ulong.MaxValue)*/)]
        [InlineData(/*UIntPtr.Size == 4 ? new UIntPtr(*/0x12345678U/*) : new UIntPtr(0x123456789ABCDEF0UL)*/)]
        [InlineData(/*UIntPtr.Size == 4 ? new UIntPtr(*/0x9ABCDEF0U/*) : new UIntPtr(0x9ABCDEF012345678UL)*/)]
        public void Test_CONV_I__UIntPtr(uint arg0) { Test("Test_CONV_I__UIntPtr", new UIntPtr(arg0)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_I1__Boolean(object arg0) { Test("Test_CONV_I1__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_I1__Byte(object arg0) { Test("Test_CONV_I1__Byte", arg0); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(-1.0d)]
        [InlineData(double.MinValue)]
        [InlineData(1.0d)]
        [InlineData(double.MaxValue)]
        [InlineData(double.NaN)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.Epsilon)]
        [InlineData(12345678910111213.1415d)]
        [InlineData(-12345678910111213.1415d)]
        public void Test_CONV_I1__Double(object arg0) { Test("Test_CONV_I1__Double", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_I1__Char(object arg0) { Test("Test_CONV_I1__Char", arg0); }

        [Theory]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        [InlineData((short)-0x1234)]
        public void Test_CONV_I1__Int16(object arg0) { Test("Test_CONV_I1__Int16", arg0); }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)-1)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        [InlineData((int)-0x12345678)]
        public void Test_CONV_I1__Int32(object arg0) { Test("Test_CONV_I1__Int32", arg0); }

        [Theory]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        [InlineData((long)0)]
        [InlineData((long)-1)]
        [InlineData((long)1)]
        [InlineData((long)0x123456789ABCDEF0)]
        [InlineData((long)-0x123456789ABCDEF0)]
        public void Test_CONV_I1__Int64(object arg0) { Test("Test_CONV_I1__Int64", arg0); }

        [Theory]
        [InlineData(int.MinValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L))]
        [InlineData(-0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L))]
        public void Test_CONV_I1__IntPtr(int arg0) { Test("Test_CONV_I1__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MinValue)]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)-1)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        [InlineData((sbyte)-0x12)]
        public void Test_CONV_I1__SByte(object arg0) { Test("Test_CONV_I1__SByte", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(-1.0f)]
        [InlineData(float.MinValue)]
        [InlineData(1.0f)]
        [InlineData(float.MaxValue)]
        [InlineData(float.NaN)]
        [InlineData(float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity)]
        [InlineData(float.Epsilon)]
        [InlineData(1234567891011.12f)]
        [InlineData(-1234567891011.12f)]
        public void Test_CONV_I1__Single(object arg0) { Test("Test_CONV_I1__Single", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_I1__UInt16(object arg0) { Test("Test_CONV_I1__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData((uint)0x12345678)]
        [InlineData((uint)0x9ABCDEF0)]
        public void Test_CONV_I1__UInt32(object arg0) { Test("Test_CONV_I1__UInt32", arg0); }

        [Theory]
        [InlineData(ulong.MinValue)]
        [InlineData(ulong.MaxValue)]
        [InlineData((ulong)0x123456789ABCDEF0)]
        [InlineData((ulong)0x9ABCDEF012345678)]
        public void Test_CONV_I1__UInt64(object arg0) { Test("Test_CONV_I1__UInt64", arg0); }

        [Theory]
        [InlineData(uint.MinValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue))]
        [InlineData(uint.MaxValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue))]
        [InlineData(0x12345678U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL))]
        [InlineData(0x9ABCDEF0U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL))]
        public void Test_CONV_I1__UIntPtr(uint arg0) { Test("Test_CONV_I1__UIntPtr", new UIntPtr(arg0)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_I2__Boolean(object arg0) { Test("Test_CONV_I2__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_I2__Byte(object arg0) { Test("Test_CONV_I2__Byte", arg0); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(-1.0d)]
        [InlineData(double.MinValue)]
        [InlineData(1.0d)]
        [InlineData(double.MaxValue)]
        [InlineData(double.NaN)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.Epsilon)]
        [InlineData(12345678910111213.1415d)]
        [InlineData(-12345678910111213.1415d)]
        public void Test_CONV_I2__Double(object arg0) { Test("Test_CONV_I2__Double", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_I2__Char(object arg0) { Test("Test_CONV_I2__Char", arg0); }

        [Theory]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        [InlineData((short)-0x1234)]
        public void Test_CONV_I2__Int16(object arg0) { Test("Test_CONV_I2__Int16", arg0); }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)-1)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        [InlineData((int)-0x12345678)]
        public void Test_CONV_I2__Int32(object arg0) { Test("Test_CONV_I2__Int32", arg0); }

        [Theory]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        [InlineData((long)0)]
        [InlineData((long)-1)]
        [InlineData((long)1)]
        [InlineData((long)0x123456789ABCDEF0)]
        [InlineData((long)-0x123456789ABCDEF0)]
        public void Test_CONV_I2__Int64(object arg0) { Test("Test_CONV_I2__Int64", arg0); }

        [Theory]
        [InlineData(int.MinValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L))]
        [InlineData(-0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L))]
        public void Test_CONV_I2__IntPtr(int arg0) { Test("Test_CONV_I2__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MinValue)]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)-1)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        [InlineData((sbyte)-0x12)]
        public void Test_CONV_I2__SByte(object arg0) { Test("Test_CONV_I2__SByte", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(-1.0f)]
        [InlineData(float.MinValue)]
        [InlineData(1.0f)]
        [InlineData(float.MaxValue)]
        [InlineData(float.NaN)]
        [InlineData(float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity)]
        [InlineData(float.Epsilon)]
        [InlineData(1234567891011.12f)]
        [InlineData(-1234567891011.12f)]
        public void Test_CONV_I2__Single(object arg0) { Test("Test_CONV_I2__Single", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_I2__UInt16(object arg0) { Test("Test_CONV_I2__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData((uint)0x12345678)]
        [InlineData((uint)0x9ABCDEF0)]
        public void Test_CONV_I2__UInt32(object arg0) { Test("Test_CONV_I2__UInt32", arg0); }

        [Theory]
        [InlineData(ulong.MinValue)]
        [InlineData(ulong.MaxValue)]
        [InlineData((ulong)0x123456789ABCDEF0)]
        [InlineData((ulong)0x9ABCDEF012345678)]
        public void Test_CONV_I2__UInt64(object arg0) { Test("Test_CONV_I2__UInt64", arg0); }

        [Theory]
        [InlineData(uint.MinValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue))]
        [InlineData(uint.MaxValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue))]
        [InlineData(0x12345678U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL))]
        [InlineData(0x9ABCDEF0U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL))]
        public void Test_CONV_I2__UIntPtr(uint arg0) { Test("Test_CONV_I2__UIntPtr", new UIntPtr(arg0)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_I4__Boolean(object arg0) { Test("Test_CONV_I4__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_I4__Byte(object arg0) { Test("Test_CONV_I4__Byte", arg0); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(-1.0d)]
        [InlineData(double.MinValue)]
        [InlineData(1.0d)]
        [InlineData(double.MaxValue)]
        [InlineData(double.NaN)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.Epsilon)]
        [InlineData(12345678910111213.1415d)]
        [InlineData(-12345678910111213.1415d)]
        public void Test_CONV_I4__Double(object arg0) { Test("Test_CONV_I4__Double", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_I4__Char(object arg0) { Test("Test_CONV_I4__Char", arg0); }

        [Theory]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        [InlineData((short)-0x1234)]
        public void Test_CONV_I4__Int16(object arg0) { Test("Test_CONV_I4__Int16", arg0); }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)-1)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        [InlineData((int)-0x12345678)]
        public void Test_CONV_I4__Int32(object arg0) { Test("Test_CONV_I4__Int32", arg0); }

        [Theory]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        [InlineData((long)0)]
        [InlineData((long)-1)]
        [InlineData((long)1)]
        [InlineData((long)0x123456789ABCDEF0)]
        [InlineData((long)-0x123456789ABCDEF0)]
        public void Test_CONV_I4__Int64(object arg0) { Test("Test_CONV_I4__Int64", arg0); }

        [Theory]
        [InlineData(int.MinValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(0x12345678)]  // [InlineData(IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L))]
        [InlineData(-0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L))]
        public void Test_CONV_I4__IntPtr(int arg0) { Test("Test_CONV_I4__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MinValue)]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)-1)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        [InlineData((sbyte)-0x12)]
        public void Test_CONV_I4__SByte(object arg0) { Test("Test_CONV_I4__SByte", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(-1.0f)]
        [InlineData(float.MinValue)]
        [InlineData(1.0f)]
        [InlineData(float.MaxValue)]
        [InlineData(float.NaN)]
        [InlineData(float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity)]
        [InlineData(float.Epsilon)]
        [InlineData(1234567891011.12f)]
        [InlineData(-1234567891011.12f)]
        public void Test_CONV_I4__Single(object arg0) { Test("Test_CONV_I4__Single", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_I4__UInt16(object arg0) { Test("Test_CONV_I4__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData((uint)0x12345678)]
        [InlineData((uint)0x9ABCDEF0)]
        public void Test_CONV_I4__UInt32(object arg0) { Test("Test_CONV_I4__UInt32", arg0); }

        [Theory]
        [InlineData(ulong.MinValue)]
        [InlineData(ulong.MaxValue)]
        [InlineData((ulong)0x123456789ABCDEF0)]
        [InlineData((ulong)0x9ABCDEF012345678)]
        public void Test_CONV_I4__UInt64(object arg0) { Test("Test_CONV_I4__UInt64", arg0); }

        [Theory]
        [InlineData(uint.MinValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue))]
        [InlineData(uint.MaxValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue))]
        [InlineData(0x12345678U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL))]
        [InlineData(0x9ABCDEF0U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL))]
        public void Test_CONV_I4__UIntPtr(uint arg0) { Test("Test_CONV_I4__UIntPtr", new UIntPtr(arg0)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_I8__Boolean(object arg0) { Test("Test_CONV_I8__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_I8__Byte(object arg0) { Test("Test_CONV_I8__Byte", arg0); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(-1.0d)]
        [InlineData(double.MinValue)]
        [InlineData(1.0d)]
        [InlineData(double.MaxValue)]
        [InlineData(double.NaN)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.Epsilon)]
        [InlineData(12345678910111213.1415d)]
        [InlineData(-12345678910111213.1415d)]
        public void Test_CONV_I8__Double(object arg0) { Test("Test_CONV_I8__Double", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_I8__Char(object arg0) { Test("Test_CONV_I8__Char", arg0); }

        [Theory]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        [InlineData((short)-0x1234)]
        public void Test_CONV_I8__Int16(object arg0) { Test("Test_CONV_I8__Int16", arg0); }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)-1)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        [InlineData((int)-0x12345678)]
        public void Test_CONV_I8__Int32(object arg0) { Test("Test_CONV_I8__Int32", arg0); }

        [Theory]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        [InlineData((long)0)]
        [InlineData((long)-1)]
        [InlineData((long)1)]
        [InlineData((long)0x123456789ABCDEF0)]
        [InlineData((long)-0x123456789ABCDEF0)]
        public void Test_CONV_I8__Int64(object arg0) { Test("Test_CONV_I8__Int64", arg0); }

        [Theory]
        [InlineData(int.MinValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(0x12345678)]  // [InlineData(IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L))]
        [InlineData(-0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L))]
        public void Test_CONV_I8__IntPtr(int arg0) { Test("Test_CONV_I8__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MinValue)]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)-1)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        [InlineData((sbyte)-0x12)]
        public void Test_CONV_I8__SByte(object arg0) { Test("Test_CONV_I8__SByte", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(-1.0f)]
        [InlineData(float.MinValue)]
        [InlineData(1.0f)]
        [InlineData(float.MaxValue)]
        [InlineData(float.NaN)]
        [InlineData(float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity)]
        [InlineData(float.Epsilon)]
        [InlineData(1234567891011.12f)]
        [InlineData(-1234567891011.12f)]
        public void Test_CONV_I8__Single(object arg0) { Test("Test_CONV_I8__Single", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_I8__UInt16(object arg0) { Test("Test_CONV_I8__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData((uint)0x12345678)]
        [InlineData((uint)0x9ABCDEF0)]
        public void Test_CONV_I8__UInt32(object arg0) { Test("Test_CONV_I8__UInt32", arg0); }

        [Theory]
        [InlineData(ulong.MinValue)]
        [InlineData(ulong.MaxValue)]
        [InlineData((ulong)0x123456789ABCDEF0)]
        [InlineData((ulong)0x9ABCDEF012345678)]
        public void Test_CONV_I8__UInt64(object arg0) { Test("Test_CONV_I8__UInt64", arg0); }

        [Theory]
        [InlineData(uint.MinValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue))]
        [InlineData(uint.MaxValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue))]
        [InlineData(0x12345678U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL))]
        [InlineData(0x9ABCDEF0U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL))]
        public void Test_CONV_I8__UIntPtr(uint arg0) { Test("Test_CONV_I8__UIntPtr", new UIntPtr(arg0)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_OVF_I__Boolean(object arg0) { Test("Test_CONV_OVF_I__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_OVF_I__Byte(object arg0) { Test("Test_CONV_OVF_I__Byte", arg0); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(-1.0d)]
        [InlineData(1.0d)]
        [InlineData(double.Epsilon)]
        public void Test_CONV_OVF_I__Double(object arg0) { Test("Test_CONV_OVF_I__Double", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_OVF_I__Char(object arg0) { Test("Test_CONV_OVF_I__Char", arg0); }

        [Theory]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        [InlineData((short)-0x1234)]
        public void Test_CONV_OVF_I__Int16(object arg0) { Test("Test_CONV_OVF_I__Int16", arg0); }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)-1)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        [InlineData((int)-0x12345678)]
        public void Test_CONV_OVF_I__Int32(object arg0) { Test("Test_CONV_OVF_I__Int32", arg0); }

        [Theory]
        [InlineData((long)0)]
        [InlineData((long)1)]
        public void Test_CONV_OVF_I__Int64(object arg0) { Test("Test_CONV_OVF_I__Int64", arg0); }

        [Theory]
        [InlineData(int.MinValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(0x12345678)]  // [InlineData(IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L))]
        [InlineData(-0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L))]
        public void Test_CONV_OVF_I__IntPtr(int arg0) { Test("Test_CONV_OVF_I__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MinValue)]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)-1)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        [InlineData((sbyte)-0x12)]
        public void Test_CONV_OVF_I__SByte(object arg0) { Test("Test_CONV_OVF_I__SByte", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(-1.0f)]
        [InlineData(1.0f)]
        [InlineData(float.Epsilon)]
        public void Test_CONV_OVF_I__Single(object arg0) { Test("Test_CONV_OVF_I__Single", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_OVF_I__UInt16(object arg0) { Test("Test_CONV_OVF_I__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData((uint)0x12345678)]
        [InlineData((uint)0x9ABCDEF0)]
        public void Test_CONV_OVF_I__UInt32(object arg0) { Test("Test_CONV_OVF_I__UInt32", arg0); }

        [Theory]
        [InlineData(ulong.MinValue)]
        [InlineData(ulong.MaxValue)]
        public void Test_CONV_OVF_I__UInt64(object arg0) { Test("Test_CONV_OVF_I__UInt64", arg0); }

        [Theory]
        [InlineData(uint.MinValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue))]
        [InlineData(uint.MaxValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue))]
        [InlineData(0x12345678U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL))]
        [InlineData(0x9ABCDEF0U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL))]
        public void Test_CONV_OVF_I__UIntPtr(uint arg0) { Test("Test_CONV_OVF_I__UIntPtr", new UIntPtr(arg0)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_OVF_I_UN__Boolean(object arg0) { Test("Test_CONV_OVF_I_UN__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_OVF_I_UN__Byte(object arg0) { Test("Test_CONV_OVF_I_UN__Byte", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_OVF_I_UN__Char(object arg0) { Test("Test_CONV_OVF_I_UN__Char", arg0); }

        [Theory]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        public void Test_CONV_OVF_I_UN__Int16(object arg0) { Test("Test_CONV_OVF_I_UN__Int16", arg0); }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        public void Test_CONV_OVF_I_UN__Int32(object arg0) { Test("Test_CONV_OVF_I_UN__Int32", arg0); }

        [Theory]
        [InlineData((long)0)]
        [InlineData((long)1)]
        public void Test_CONV_OVF_I_UN__Int64(object arg0) { Test("Test_CONV_OVF_I_UN__Int64", arg0); }

        [Theory]
        [InlineData(int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L))]
        public void Test_CONV_OVF_I_UN__IntPtr(int arg0) { Test("Test_CONV_OVF_I_UN__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        public void Test_CONV_OVF_I_UN__SByte(object arg0) { Test("Test_CONV_OVF_I_UN__SByte", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_OVF_I_UN__UInt16(object arg0) { Test("Test_CONV_OVF_I_UN__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData((uint)0x12345678)]
        public void Test_CONV_OVF_I_UN__UInt32(object arg0) { Test("Test_CONV_OVF_I_UN__UInt32", arg0); }

        [Fact]
        public void Test_CONV_OVF_I_UN__UInt64() { Test("Test_CONV_OVF_I_UN__UInt64", ulong.MinValue); }

        [Theory]
        [InlineData(uint.MinValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue))]
        [InlineData(0x12345678U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL))]
        public void Test_CONV_OVF_I_UN__UIntPtr(uint arg0) { Test("Test_CONV_OVF_I_UN__UIntPtr", new UIntPtr(arg0)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_OVF_I1__Boolean(object arg0) { Test("Test_CONV_OVF_I1__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData((byte)0x12)]
        public void Test_CONV_OVF_I1__Byte(object arg0) { Test("Test_CONV_OVF_I1__Byte", arg0); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(-1.0d)]
        [InlineData(1.0d)]
        [InlineData(double.Epsilon)]
        public void Test_CONV_OVF_I1__Double(object arg0) { Test("Test_CONV_OVF_I1__Double", arg0); }

        [Fact]
        public void Test_CONV_OVF_I1__Char() { Test("Test_CONV_OVF_I1__Char", char.MinValue); }

        [Theory]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        public void Test_CONV_OVF_I1__Int16(object arg0) { Test("Test_CONV_OVF_I1__Int16", arg0); }

        [Theory]
        [InlineData((int)0)]
        [InlineData((int)-1)]
        [InlineData((int)1)]
        public void Test_CONV_OVF_I1__Int32(object arg0) { Test("Test_CONV_OVF_I1__Int32", arg0); }

        [Theory]
        [InlineData((long)0)]
        [InlineData((long)-1)]
        [InlineData((long)1)]
        public void Test_CONV_OVF_I1__Int64(object arg0) { Test("Test_CONV_OVF_I1__Int64", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        public void Test_CONV_OVF_I1__IntPtr(int arg0) { Test("Test_CONV_OVF_I1__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MinValue)]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)-1)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        [InlineData((sbyte)-0x12)]
        public void Test_CONV_OVF_I1__SByte(object arg0) { Test("Test_CONV_OVF_I1__SByte", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(-1.0f)]
        [InlineData(1.0f)]
        [InlineData(float.Epsilon)]
        public void Test_CONV_OVF_I1__Single(object arg0) { Test("Test_CONV_OVF_I1__Single", arg0); }

        [Fact]
        public void Test_CONV_OVF_I1__UInt16() { Test("Test_CONV_OVF_I1__UInt16", ushort.MinValue); }

        [Fact]
        public void Test_CONV_OVF_I1__UInt32() { Test("Test_CONV_OVF_I1__UInt32", uint.MinValue); }

        [Fact]
        public void Test_CONV_OVF_I1__UInt64() { Test("Test_CONV_OVF_I1__UInt64", ulong.MinValue); }

        [Theory]
        [InlineData(uint.MinValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue))]
        [InlineData(uint.MaxValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue))]
        public void Test_CONV_OVF_I1__UIntPtr(uint arg0) { Test("Test_CONV_OVF_I1__UIntPtr", new UIntPtr(arg0)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_OVF_I1_UN__Boolean(object arg0) { Test("Test_CONV_OVF_I1_UN__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData((byte)0x12)]
        public void Test_CONV_OVF_I1_UN__Byte(object arg0) { Test("Test_CONV_OVF_I1_UN__Byte", arg0); }

        [Fact]
        public void Test_CONV_OVF_I1_UN__Char() { Test("Test_CONV_OVF_I1_UN__Char", char.MinValue); }

        [Theory]
        [InlineData((short)0)]
        [InlineData((short)1)]
        public void Test_CONV_OVF_I1_UN__Int16(object arg0) { Test("Test_CONV_OVF_I1_UN__Int16", arg0); }

        [Theory]
        [InlineData((int)0)]
        [InlineData((int)1)]
        public void Test_CONV_OVF_I1_UN__Int32(object arg0) { Test("Test_CONV_OVF_I1_UN__Int32", arg0); }

        [Theory]
        [InlineData((long)0)]
        [InlineData((long)1)]
        public void Test_CONV_OVF_I1_UN__Int64(object arg0) { Test("Test_CONV_OVF_I1_UN__Int64", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Test_CONV_OVF_I1_UN__IntPtr(int arg0) { Test("Test_CONV_OVF_I1_UN__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        public void Test_CONV_OVF_I1_UN__SByte(object arg0) { Test("Test_CONV_OVF_I1_UN__SByte", arg0); }

        [Fact]
        public void Test_CONV_OVF_I1_UN__UInt16() { Test("Test_CONV_OVF_I1_UN__UInt16", ushort.MinValue); }

        [Fact]
        public void Test_CONV_OVF_I1_UN__UInt32() { Test("Test_CONV_OVF_I1_UN__UInt32", uint.MinValue); }

        [Fact]
        public void Test_CONV_OVF_I1_UN__UInt64() { Test("Test_CONV_OVF_I1_UN__UInt64", ulong.MinValue); }

        [Fact]
        public void Test_CONV_OVF_I1_UN__UIntPtr() { Test("Test_CONV_OVF_I1_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_OVF_I2__Boolean(object arg0) { Test("Test_CONV_OVF_I2__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_OVF_I2__Byte(object arg0) { Test("Test_CONV_OVF_I2__Byte", arg0); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(-1.0d)]
        [InlineData(1.0d)]
        [InlineData(double.Epsilon)]
        public void Test_CONV_OVF_I2__Double(object arg0) { Test("Test_CONV_OVF_I2__Double", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData((char)0x1234)]
        public void Test_CONV_OVF_I2__Char(object arg0) { Test("Test_CONV_OVF_I2__Char", arg0); }

        [Theory]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        [InlineData((short)-0x1234)]
        public void Test_CONV_OVF_I2__Int16(object arg0) { Test("Test_CONV_OVF_I2__Int16", arg0); }

        [Theory]
        [InlineData((int)0)]
        [InlineData((int)-1)]
        [InlineData((int)1)]
        public void Test_CONV_OVF_I2__Int32(object arg0) { Test("Test_CONV_OVF_I2__Int32", arg0); }

        [Theory]
        [InlineData((long)0)]
        [InlineData((long)-1)]
        [InlineData((long)1)]
        public void Test_CONV_OVF_I2__Int64(object arg0) { Test("Test_CONV_OVF_I2__Int64", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        public void Test_CONV_OVF_I2__IntPtr(int arg0) { Test("Test_CONV_OVF_I2__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MinValue)]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)-1)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        [InlineData((sbyte)-0x12)]
        public void Test_CONV_OVF_I2__SByte(object arg0) { Test("Test_CONV_OVF_I2__SByte", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(-1.0f)]
        [InlineData(1.0f)]
        [InlineData(float.Epsilon)]
        public void Test_CONV_OVF_I2__Single(object arg0) { Test("Test_CONV_OVF_I2__Single", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData((ushort)0x1234)]
        public void Test_CONV_OVF_I2__UInt16(object arg0) { Test("Test_CONV_OVF_I2__UInt16", arg0); }

        [Fact]
        public void Test_CONV_OVF_I2__UInt32() { Test("Test_CONV_OVF_I2__UInt32", uint.MinValue); }

        [Fact]
        public void Test_CONV_OVF_I2__UInt64() { Test("Test_CONV_OVF_I2__UInt64", ulong.MinValue); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_OVF_I2_UN__Boolean(object arg0) { Test("Test_CONV_OVF_I2_UN__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_OVF_I2_UN__Byte(object arg0) { Test("Test_CONV_OVF_I2_UN__Byte", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData((char)0x1234)]
        public void Test_CONV_OVF_I2_UN__Char(object arg0) { Test("Test_CONV_OVF_I2_UN__Char", arg0); }

        [Theory]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        public void Test_CONV_OVF_I2_UN__Int16(object arg0) { Test("Test_CONV_OVF_I2_UN__Int16", arg0); }

        [Theory]
        [InlineData((int)0)]
        [InlineData((int)1)]
        public void Test_CONV_OVF_I2_UN__Int32(object arg0) { Test("Test_CONV_OVF_I2_UN__Int32", arg0); }

        [Theory]
        [InlineData((long)0)]
        [InlineData((long)1)]
        public void Test_CONV_OVF_I2_UN__Int64(object arg0) { Test("Test_CONV_OVF_I2_UN__Int64", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Test_CONV_OVF_I2_UN__IntPtr(int arg0) { Test("Test_CONV_OVF_I2_UN__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        public void Test_CONV_OVF_I2_UN__SByte(object arg0) { Test("Test_CONV_OVF_I2_UN__SByte", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData((ushort)0x1234)]
        public void Test_CONV_OVF_I2_UN__UInt16(object arg0) { Test("Test_CONV_OVF_I2_UN__UInt16", arg0); }

        [Fact]
        public void Test_CONV_OVF_I2_UN__UInt32() { Test("Test_CONV_OVF_I2_UN__UInt32", uint.MinValue); }

        [Fact]
        public void Test_CONV_OVF_I2_UN__UInt64() { Test("Test_CONV_OVF_I2_UN__UInt64", ulong.MinValue); }

        [Fact]
        public void Test_CONV_OVF_I2_UN__UIntPtr() { Test("Test_CONV_OVF_I2_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_OVF_I4__Boolean(object arg0) { Test("Test_CONV_OVF_I4__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_OVF_I4__Byte(object arg0) { Test("Test_CONV_OVF_I4__Byte", arg0); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(-1.0d)]
        [InlineData(1.0d)]
        [InlineData(double.Epsilon)]
        public void Test_CONV_OVF_I4__Double(object arg0) { Test("Test_CONV_OVF_I4__Double", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_OVF_I4__Char(object arg0) { Test("Test_CONV_OVF_I4__Char", arg0); }

        [Theory]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        [InlineData((short)-0x1234)]
        public void Test_CONV_OVF_I4__Int16(object arg0) { Test("Test_CONV_OVF_I4__Int16", arg0); }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)-1)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        [InlineData((int)-0x12345678)]
        public void Test_CONV_OVF_I4__Int32(object arg0) { Test("Test_CONV_OVF_I4__Int32", arg0); }

        [Theory]
        [InlineData((long)0)]
        [InlineData((long)-1)]
        [InlineData((long)1)]
        public void Test_CONV_OVF_I4__Int64(object arg0) { Test("Test_CONV_OVF_I4__Int64", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        public void Test_CONV_OVF_I4__IntPtr(int arg0) { Test("Test_CONV_OVF_I4__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MinValue)]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)-1)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        [InlineData((sbyte)-0x12)]
        public void Test_CONV_OVF_I4__SByte(object arg0) { Test("Test_CONV_OVF_I4__SByte", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(-1.0f)]
        [InlineData(1.0f)]
        [InlineData(float.Epsilon)]
        public void Test_CONV_OVF_I4__Single(object arg0) { Test("Test_CONV_OVF_I4__Single", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_OVF_I4__UInt16(object arg0) { Test("Test_CONV_OVF_I4__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData((uint)0x12345678)]
        public void Test_CONV_OVF_I4__UInt32(object arg0) { Test("Test_CONV_OVF_I4__UInt32", arg0); }

        [Fact]
        public void Test_CONV_OVF_I4__UInt64() { Test("Test_CONV_OVF_I4__UInt64", ulong.MinValue); }

        [Theory]
        [InlineData(uint.MinValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue))]
        [InlineData(uint.MaxValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue))]
        public void Test_CONV_OVF_I4__UIntPtr(uint arg0) { Test("Test_CONV_OVF_I4__UIntPtr", new UIntPtr(arg0)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_OVF_I4_UN__Boolean(object arg0) { Test("Test_CONV_OVF_I4_UN__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_OVF_I4_UN__Byte(object arg0) { Test("Test_CONV_OVF_I4_UN__Byte", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_OVF_I4_UN__Char(object arg0) { Test("Test_CONV_OVF_I4_UN__Char", arg0); }

        [Theory]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        public void Test_CONV_OVF_I4_UN__Int16(object arg0) { Test("Test_CONV_OVF_I4_UN__Int16", arg0); }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        public void Test_CONV_OVF_I4_UN__Int32(object arg0) { Test("Test_CONV_OVF_I4_UN__Int32", arg0); }

        [Theory]
        [InlineData((long)0)]
        [InlineData((long)1)]
        public void Test_CONV_OVF_I4_UN__Int64(object arg0) { Test("Test_CONV_OVF_I4_UN__Int64", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Test_CONV_OVF_I4_UN__IntPtr(int arg0) { Test("Test_CONV_OVF_I4_UN__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        public void Test_CONV_OVF_I4_UN__SByte(object arg0) { Test("Test_CONV_OVF_I4_UN__SByte", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_OVF_I4_UN__UInt16(object arg0) { Test("Test_CONV_OVF_I4_UN__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData((uint)0x12345678)]
        public void Test_CONV_OVF_I4_UN__UInt32(object arg0) { Test("Test_CONV_OVF_I4_UN__UInt32", arg0); }

        [Fact]
        public void Test_CONV_OVF_I4_UN__UInt64() { Test("Test_CONV_OVF_I4_UN__UInt64", ulong.MinValue); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_OVF_I8__Boolean(object arg0) { Test("Test_CONV_OVF_I8__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_OVF_I8__Byte(object arg0) { Test("Test_CONV_OVF_I8__Byte", arg0); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(-1.0d)]
        [InlineData(1.0d)]
        [InlineData(double.Epsilon)]
        [InlineData(12345678910111213.1415d)]
        [InlineData(-12345678910111213.1415d)]
        public void Test_CONV_OVF_I8__Double(object arg0) { Test("Test_CONV_OVF_I8__Double", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_OVF_I8__Char(object arg0) { Test("Test_CONV_OVF_I8__Char", arg0); }

        [Theory]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        [InlineData((short)-0x1234)]
        public void Test_CONV_OVF_I8__Int16(object arg0) { Test("Test_CONV_OVF_I8__Int16", arg0); }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)-1)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        [InlineData((int)-0x12345678)]
        public void Test_CONV_OVF_I8__Int32(object arg0) { Test("Test_CONV_OVF_I8__Int32", arg0); }

        [Theory]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        [InlineData((long)0)]
        [InlineData((long)-1)]
        [InlineData((long)1)]
        [InlineData((long)0x123456789ABCDEF0)]
        [InlineData((long)-0x123456789ABCDEF0)]
        public void Test_CONV_OVF_I8__Int64(object arg0) { Test("Test_CONV_OVF_I8__Int64", arg0); }

        [Theory]
        [InlineData(int.MinValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L))]
        [InlineData(-0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L))]
        public void Test_CONV_OVF_I8__IntPtr(int arg0) { Test("Test_CONV_OVF_I8__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MinValue)]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)-1)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        [InlineData((sbyte)-0x12)]
        public void Test_CONV_OVF_I8__SByte(object arg0) { Test("Test_CONV_OVF_I8__SByte", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(-1.0f)]
        [InlineData(1.0f)]
        [InlineData(float.Epsilon)]
        [InlineData(1234567891011.12f)]
        [InlineData(-1234567891011.12f)]
        public void Test_CONV_OVF_I8__Single(object arg0) { Test("Test_CONV_OVF_I8__Single", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_OVF_I8__UInt16(object arg0) { Test("Test_CONV_OVF_I8__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData((uint)0x12345678)]
        [InlineData((uint)0x9ABCDEF0)]
        public void Test_CONV_OVF_I8__UInt32(object arg0) { Test("Test_CONV_OVF_I8__UInt32", arg0); }

        [Theory]
        [InlineData(ulong.MinValue)]
        [InlineData((ulong)0x123456789ABCDEF0)]
        public void Test_CONV_OVF_I8__UInt64(object arg0) { Test("Test_CONV_OVF_I8__UInt64", arg0); }

        [Theory]
        [InlineData(uint.MinValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue))]
        [InlineData(uint.MaxValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue))]
        [InlineData(0x12345678U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL))]
        [InlineData(0x9ABCDEF0U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL))]
        public void Test_CONV_OVF_I8__UIntPtr(uint arg0) { Test("Test_CONV_OVF_I8__UIntPtr", new UIntPtr(arg0)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_OVF_I8_UN__Boolean(object arg0) { Test("Test_CONV_OVF_I8_UN__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_OVF_I8_UN__Byte(object arg0) { Test("Test_CONV_OVF_I8_UN__Byte", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_OVF_I8_UN__Char(object arg0) { Test("Test_CONV_OVF_I8_UN__Char", arg0); }

        [Theory]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        [InlineData((short)-0x1234)]
        public void Test_CONV_OVF_I8_UN__Int16(object arg0) { Test("Test_CONV_OVF_I8_UN__Int16", arg0); }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)-1)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        [InlineData((int)-0x12345678)]
        public void Test_CONV_OVF_I8_UN__Int32(object arg0) { Test("Test_CONV_OVF_I8_UN__Int32", arg0); }

        [Theory]
        [InlineData((long)0)]
        [InlineData((long)1)]
        public void Test_CONV_OVF_I8_UN__Int64(object arg0) { Test("Test_CONV_OVF_I8_UN__Int64", arg0); }

        [Theory]
        [InlineData(int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L))]
        public void Test_CONV_OVF_I8_UN__IntPtr(int arg0) { Test("Test_CONV_OVF_I8_UN__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        public void Test_CONV_OVF_I8_UN__SByte(object arg0) { Test("Test_CONV_OVF_I8_UN__SByte", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_OVF_I8_UN__UInt16(object arg0) { Test("Test_CONV_OVF_I8_UN__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData((uint)0x12345678)]
        [InlineData((uint)0x9ABCDEF0)]
        public void Test_CONV_OVF_I8_UN__UInt32(object arg0) { Test("Test_CONV_OVF_I8_UN__UInt32", arg0); }

        [Theory]
        [InlineData(ulong.MinValue)]
        [InlineData((ulong)0x123456789ABCDEF0)]
        public void Test_CONV_OVF_I8_UN__UInt64(object arg0) { Test("Test_CONV_OVF_I8_UN__UInt64", arg0); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_OVF_U__Boolean(object arg0) { Test("Test_CONV_OVF_U__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_OVF_U__Byte(object arg0) { Test("Test_CONV_OVF_U__Byte", arg0); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(1.0d)]
        [InlineData(double.Epsilon)]
        public void Test_CONV_OVF_U__Double(object arg0) { Test("Test_CONV_OVF_U__Double", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_OVF_U__Char(object arg0) { Test("Test_CONV_OVF_U__Char", arg0); }

        [Theory]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        public void Test_CONV_OVF_U__Int16(object arg0) { Test("Test_CONV_OVF_U__Int16", arg0); }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        public void Test_CONV_OVF_U__Int32(object arg0) { Test("Test_CONV_OVF_U__Int32", arg0); }

        [Theory]
        [InlineData((long)0)]
        [InlineData((long)1)]
        public void Test_CONV_OVF_U__Int64(object arg0) { Test("Test_CONV_OVF_U__Int64", arg0); }

        [Theory]
        [InlineData(int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L))]
        public void Test_CONV_OVF_U__IntPtr(int arg0) { Test("Test_CONV_OVF_U__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        public void Test_CONV_OVF_U__SByte(object arg0) { Test("Test_CONV_OVF_U__SByte", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(1.0f)]
        [InlineData(float.Epsilon)]
        public void Test_CONV_OVF_U__Single(object arg0) { Test("Test_CONV_OVF_U__Single", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_OVF_U__UInt16(object arg0) { Test("Test_CONV_OVF_U__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData((uint)0x12345678)]
        public void Test_CONV_OVF_U__UInt32(object arg0) { Test("Test_CONV_OVF_U__UInt32", arg0); }

        [Fact]
        public void Test_CONV_OVF_U__UInt64() { Test("Test_CONV_OVF_U__UInt64", ulong.MinValue); }

        [Theory]
        [InlineData(uint.MinValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue))]
        [InlineData(0x12345678U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL))]
        public void Test_CONV_OVF_U__UIntPtr(uint arg0) { Test("Test_CONV_OVF_U__UIntPtr", new UIntPtr(arg0)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_OVF_U_UN__Boolean(object arg0) { Test("Test_CONV_OVF_U_UN__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_OVF_U_UN__Byte(object arg0) { Test("Test_CONV_OVF_U_UN__Byte", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_OVF_U_UN__Char(object arg0) { Test("Test_CONV_OVF_U_UN__Char", arg0); }

        [Theory]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        [InlineData((short)-0x1234)]
        public void Test_CONV_OVF_U_UN__Int16(object arg0) { Test("Test_CONV_OVF_U_UN__Int16", arg0); }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)-1)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        [InlineData((int)-0x12345678)]
        public void Test_CONV_OVF_U_UN__Int32(object arg0) { Test("Test_CONV_OVF_U_UN__Int32", arg0); }

        [Theory]
        [InlineData((long)0)]
        [InlineData((long)1)]
        public void Test_CONV_OVF_U_UN__Int64(object arg0) { Test("Test_CONV_OVF_U_UN__Int64", arg0); }

        [Theory]
        [InlineData(int.MinValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L))]
        [InlineData(0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L))]
        public void Test_CONV_OVF_U_UN__IntPtr(int arg0) { Test("Test_CONV_OVF_U_UN__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MinValue)]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)-1)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        [InlineData((sbyte)-0x12)]
        public void Test_CONV_OVF_U_UN__SByte(object arg0) { Test("Test_CONV_OVF_U_UN__SByte", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_OVF_U_UN__UInt16(object arg0) { Test("Test_CONV_OVF_U_UN__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData((uint)0x12345678)]
        [InlineData((uint)0x9ABCDEF0)]
        public void Test_CONV_OVF_U_UN__UInt32(object arg0) { Test("Test_CONV_OVF_U_UN__UInt32", arg0); }

        [Fact]
        public void Test_CONV_OVF_U_UN__UInt64() { Test("Test_CONV_OVF_U_UN__UInt64", ulong.MinValue); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_OVF_U1__Boolean(object arg0) { Test("Test_CONV_OVF_U1__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_OVF_U1__Byte(object arg0) { Test("Test_CONV_OVF_U1__Byte", arg0); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(1.0d)]
        [InlineData(double.Epsilon)]
        public void Test_CONV_OVF_U1__Double(object arg0) { Test("Test_CONV_OVF_U1__Double", arg0); }

        [Fact]
        public void Test_CONV_OVF_U1__Char() { Test("Test_CONV_OVF_U1__Char", char.MinValue); }

        [Theory]
        [InlineData((short)0)]
        [InlineData((short)1)]
        public void Test_CONV_OVF_U1__Int16(object arg0) { Test("Test_CONV_OVF_U1__Int16", arg0); }

        [Theory]
        [InlineData((int)0)]
        [InlineData((int)1)]
        public void Test_CONV_OVF_U1__Int32(object arg0) { Test("Test_CONV_OVF_U1__Int32", arg0); }

        [Theory]
        [InlineData((long)0)]
        [InlineData((long)1)]
        public void Test_CONV_OVF_U1__Int64(object arg0) { Test("Test_CONV_OVF_U1__Int64", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Test_CONV_OVF_U1__IntPtr(int arg0) { Test("Test_CONV_OVF_U1__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        public void Test_CONV_OVF_U1__SByte(object arg0) { Test("Test_CONV_OVF_U1__SByte", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(1.0f)]
        [InlineData(float.Epsilon)]
        public void Test_CONV_OVF_U1__Single(object arg0) { Test("Test_CONV_OVF_U1__Single", arg0); }

        [Fact]
        public void Test_CONV_OVF_U1__UInt16() { Test("Test_CONV_OVF_U1__UInt16", ushort.MinValue); }

        [Fact]
        public void Test_CONV_OVF_U1__UInt32() { Test("Test_CONV_OVF_U1__UInt32", uint.MinValue); }

        [Fact]
        public void Test_CONV_OVF_U1__UInt64() { Test("Test_CONV_OVF_U1__UInt64", ulong.MinValue); }

        [Fact]
        public void Test_CONV_OVF_U1__UIntPtr() { Test("Test_CONV_OVF_U1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_OVF_U1_UN__Boolean(object arg0) { Test("Test_CONV_OVF_U1_UN__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_OVF_U1_UN__Byte(object arg0) { Test("Test_CONV_OVF_U1_UN__Byte", arg0); }

        [Fact]
        public void Test_CONV_OVF_U1_UN__Char() { Test("Test_CONV_OVF_U1_UN__Char", char.MinValue); }

        [Theory]
        [InlineData((short)0)]
        [InlineData((short)1)]
        public void Test_CONV_OVF_U1_UN__Int16(object arg0) { Test("Test_CONV_OVF_U1_UN__Int16", arg0); }

        [Theory]
        [InlineData((int)0)]
        [InlineData((int)1)]
        public void Test_CONV_OVF_U1_UN__Int32(object arg0) { Test("Test_CONV_OVF_U1_UN__Int32", arg0); }

        [Theory]
        [InlineData((long)0)]
        [InlineData((long)1)]
        public void Test_CONV_OVF_U1_UN__Int64(object arg0) { Test("Test_CONV_OVF_U1_UN__Int64", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Test_CONV_OVF_U1_UN__IntPtr(int arg0) { Test("Test_CONV_OVF_U1_UN__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        public void Test_CONV_OVF_U1_UN__SByte(object arg0) { Test("Test_CONV_OVF_U1_UN__SByte", arg0); }

        [Fact]
        public void Test_CONV_OVF_U1_UN__UInt16() { Test("Test_CONV_OVF_U1_UN__UInt16", ushort.MinValue); }

        [Fact]
        public void Test_CONV_OVF_U1_UN__UInt32() { Test("Test_CONV_OVF_U1_UN__UInt32", uint.MinValue); }

        [Fact]
        public void Test_CONV_OVF_U1_UN__UInt64() { Test("Test_CONV_OVF_U1_UN__UInt64", ulong.MinValue); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_OVF_U2__Boolean(object arg0) { Test("Test_CONV_OVF_U2__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_OVF_U2__Byte(object arg0) { Test("Test_CONV_OVF_U2__Byte", arg0); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(1.0d)]
        [InlineData(double.Epsilon)]
        public void Test_CONV_OVF_U2__Double(object arg0) { Test("Test_CONV_OVF_U2__Double", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_OVF_U2__Char(object arg0) { Test("Test_CONV_OVF_U2__Char", arg0); }

        [Theory]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        public void Test_CONV_OVF_U2__Int16(object arg0) { Test("Test_CONV_OVF_U2__Int16", arg0); }

        [Theory]
        [InlineData((int)0)]
        [InlineData((int)1)]
        public void Test_CONV_OVF_U2__Int32(object arg0) { Test("Test_CONV_OVF_U2__Int32", arg0); }

        [Theory]
        [InlineData((long)0)]
        [InlineData((long)1)]
        public void Test_CONV_OVF_U2__Int64(object arg0) { Test("Test_CONV_OVF_U2__Int64", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Test_CONV_OVF_U2__IntPtr(int arg0) { Test("Test_CONV_OVF_U2__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        public void Test_CONV_OVF_U2__SByte(object arg0) { Test("Test_CONV_OVF_U2__SByte", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(1.0f)]
        [InlineData(float.Epsilon)]
        public void Test_CONV_OVF_U2__Single(object arg0) { Test("Test_CONV_OVF_U2__Single", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_OVF_U2__UInt16(object arg0) { Test("Test_CONV_OVF_U2__UInt16", arg0); }

        [Fact]
        public void Test_CONV_OVF_U2__UInt32() { Test("Test_CONV_OVF_U2__UInt32", uint.MinValue); }

        [Fact]
        public void Test_CONV_OVF_U2__UInt64() { Test("Test_CONV_OVF_U2__UInt64", ulong.MinValue); }

        [Fact]
        public void Test_CONV_OVF_U2__UIntPtr() { Test("Test_CONV_OVF_U2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_OVF_U2_UN__Boolean(object arg0) { Test("Test_CONV_OVF_U2_UN__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_OVF_U2_UN__Byte(object arg0) { Test("Test_CONV_OVF_U2_UN__Byte", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_OVF_U2_UN__Char(object arg0) { Test("Test_CONV_OVF_U2_UN__Char", arg0); }

        [Theory]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        public void Test_CONV_OVF_U2_UN__Int16(object arg0) { Test("Test_CONV_OVF_U2_UN__Int16", arg0); }

        [Theory]
        [InlineData((int)0)]
        [InlineData((int)1)]
        public void Test_CONV_OVF_U2_UN__Int32(object arg0) { Test("Test_CONV_OVF_U2_UN__Int32", arg0); }

        [Theory]
        [InlineData((long)0)]
        [InlineData((long)1)]
        public void Test_CONV_OVF_U2_UN__Int64(object arg0) { Test("Test_CONV_OVF_U2_UN__Int64", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Test_CONV_OVF_U2_UN__IntPtr(int arg0) { Test("Test_CONV_OVF_U2_UN__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        public void Test_CONV_OVF_U2_UN__SByte(object arg0) { Test("Test_CONV_OVF_U2_UN__SByte", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_OVF_U2_UN__UInt16(object arg0) { Test("Test_CONV_OVF_U2_UN__UInt16", arg0); }

        [Fact]
        public void Test_CONV_OVF_U2_UN__UInt32() { Test("Test_CONV_OVF_U2_UN__UInt32", uint.MinValue); }

        [Fact]
        public void Test_CONV_OVF_U2_UN__UInt64() { Test("Test_CONV_OVF_U2_UN__UInt64", ulong.MinValue); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_OVF_U4__Boolean(object arg0) { Test("Test_CONV_OVF_U4__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_OVF_U4__Byte(object arg0) { Test("Test_CONV_OVF_U4__Byte", arg0); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(1.0d)]
        [InlineData(double.Epsilon)]
        public void Test_CONV_OVF_U4__Double(object arg0) { Test("Test_CONV_OVF_U4__Double", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_OVF_U4__Char(object arg0) { Test("Test_CONV_OVF_U4__Char", arg0); }

        [Theory]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        public void Test_CONV_OVF_U4__Int16(object arg0) { Test("Test_CONV_OVF_U4__Int16", arg0); }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        public void Test_CONV_OVF_U4__Int32(object arg0) { Test("Test_CONV_OVF_U4__Int32", arg0); }

        [Theory]
        [InlineData((long)0)]
        [InlineData((long)1)]
        public void Test_CONV_OVF_U4__Int64(object arg0) { Test("Test_CONV_OVF_U4__Int64", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Test_CONV_OVF_U4__IntPtr(int arg0) { Test("Test_CONV_OVF_U4__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        public void Test_CONV_OVF_U4__SByte(object arg0) { Test("Test_CONV_OVF_U4__SByte", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(1.0f)]
        [InlineData(float.Epsilon)]
        public void Test_CONV_OVF_U4__Single(object arg0) { Test("Test_CONV_OVF_U4__Single", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_OVF_U4__UInt16(object arg0) { Test("Test_CONV_OVF_U4__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData((uint)0x12345678)]
        public void Test_CONV_OVF_U4__UInt32(object arg0) { Test("Test_CONV_OVF_U4__UInt32", arg0); }

        [Fact]
        public void Test_CONV_OVF_U4__UInt64() { Test("Test_CONV_OVF_U4__UInt64", ulong.MinValue); }

        [Fact]
        public void Test_CONV_OVF_U4__UIntPtr() { Test("Test_CONV_OVF_U4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_OVF_U4_UN__Boolean(object arg0) { Test("Test_CONV_OVF_U4_UN__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_OVF_U4_UN__Byte(object arg0) { Test("Test_CONV_OVF_U4_UN__Byte", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_OVF_U4_UN__Char(object arg0) { Test("Test_CONV_OVF_U4_UN__Char", arg0); }

        [Theory]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        [InlineData((short)-0x1234)]
        public void Test_CONV_OVF_U4_UN__Int16(object arg0) { Test("Test_CONV_OVF_U4_UN__Int16", arg0); }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)-1)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        [InlineData((int)-0x12345678)]
        public void Test_CONV_OVF_U4_UN__Int32(object arg0) { Test("Test_CONV_OVF_U4_UN__Int32", arg0); }

        [Theory]
        [InlineData((long)0)]
        [InlineData((long)1)]
        public void Test_CONV_OVF_U4_UN__Int64(object arg0) { Test("Test_CONV_OVF_U4_UN__Int64", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Test_CONV_OVF_U4_UN__IntPtr(int arg0) { Test("Test_CONV_OVF_U4_UN__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        public void Test_CONV_OVF_U4_UN__SByte(object arg0) { Test("Test_CONV_OVF_U4_UN__SByte", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_OVF_U4_UN__UInt16(object arg0) { Test("Test_CONV_OVF_U4_UN__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData((uint)0x12345678)]
        [InlineData((uint)0x9ABCDEF0)]
        public void Test_CONV_OVF_U4_UN__UInt32(object arg0) { Test("Test_CONV_OVF_U4_UN__UInt32", arg0); }

        [Fact]
        public void Test_CONV_OVF_U4_UN__UInt64() { Test("Test_CONV_OVF_U4_UN__UInt64", ulong.MinValue); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_OVF_U8__Boolean(object arg0) { Test("Test_CONV_OVF_U8__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_OVF_U8__Byte(object arg0) { Test("Test_CONV_OVF_U8__Byte", arg0); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(1.0d)]
        [InlineData(double.Epsilon)]
        [InlineData(12345678910111213.1415d)]
        public void Test_CONV_OVF_U8__Double(object arg0) { Test("Test_CONV_OVF_U8__Double", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_OVF_U8__Char(object arg0) { Test("Test_CONV_OVF_U8__Char", arg0); }

        [Theory]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        public void Test_CONV_OVF_U8__Int16(object arg0) { Test("Test_CONV_OVF_U8__Int16", arg0); }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        public void Test_CONV_OVF_U8__Int32(object arg0) { Test("Test_CONV_OVF_U8__Int32", arg0); }

        [Theory]
        [InlineData(long.MaxValue)]
        [InlineData((long)0)]
        [InlineData((long)1)]
        [InlineData((long)0x123456789ABCDEF0)]
        public void Test_CONV_OVF_U8__Int64(object arg0) { Test("Test_CONV_OVF_U8__Int64", arg0); }

        [Theory]
        [InlineData(int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L))]
        public void Test_CONV_OVF_U8__IntPtr(int arg0) { Test("Test_CONV_OVF_U8__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        public void Test_CONV_OVF_U8__SByte(object arg0) { Test("Test_CONV_OVF_U8__SByte", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(1.0f)]
        [InlineData(float.Epsilon)]
        public void Test_CONV_OVF_U8__Single(object arg0) { Test("Test_CONV_OVF_U8__Single", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_OVF_U8__UInt16(object arg0) { Test("Test_CONV_OVF_U8__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData((uint)0x12345678)]
        public void Test_CONV_OVF_U8__UInt32(object arg0) { Test("Test_CONV_OVF_U8__UInt32", arg0); }

        [Theory]
        [InlineData(ulong.MinValue)]
        [InlineData((ulong)0x123456789ABCDEF0)]
        public void Test_CONV_OVF_U8__UInt64(object arg0) { Test("Test_CONV_OVF_U8__UInt64", arg0); }

        [Theory]
        [InlineData(uint.MinValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue))]
        [InlineData(0x12345678U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL))]
        public void Test_CONV_OVF_U8__UIntPtr(uint arg0) { Test("Test_CONV_OVF_U8__UIntPtr", new UIntPtr(arg0)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_OVF_U8_UN__Boolean(object arg0) { Test("Test_CONV_OVF_U8_UN__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_OVF_U8_UN__Byte(object arg0) { Test("Test_CONV_OVF_U8_UN__Byte", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_OVF_U8_UN__Char(object arg0) { Test("Test_CONV_OVF_U8_UN__Char", arg0); }

        [Theory]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        [InlineData((short)-0x1234)]
        public void Test_CONV_OVF_U8_UN__Int16(object arg0) { Test("Test_CONV_OVF_U8_UN__Int16", arg0); }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)-1)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        [InlineData((int)-0x12345678)]
        public void Test_CONV_OVF_U8_UN__Int32(object arg0) { Test("Test_CONV_OVF_U8_UN__Int32", arg0); }

        [Theory]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        [InlineData((long)0)]
        [InlineData((long)-1)]
        [InlineData((long)1)]
        [InlineData((long)0x123456789ABCDEF0)]
        [InlineData((long)-0x123456789ABCDEF0)]
        public void Test_CONV_OVF_U8_UN__Int64(object arg0) { Test("Test_CONV_OVF_U8_UN__Int64", arg0); }

        [Theory]
        [InlineData(int.MinValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L))]
        [InlineData(-0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L))]
        public void Test_CONV_OVF_U8_UN__IntPtr(int arg0) { Test("Test_CONV_OVF_U8_UN__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        public void Test_CONV_OVF_U8_UN__SByte(object arg0) { Test("Test_CONV_OVF_U8_UN__SByte", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_OVF_U8_UN__UInt16(object arg0) { Test("Test_CONV_OVF_U8_UN__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData((uint)0x12345678)]
        [InlineData((uint)0x9ABCDEF0)]
        public void Test_CONV_OVF_U8_UN__UInt32(object arg0) { Test("Test_CONV_OVF_U8_UN__UInt32", arg0); }

        [Theory]
        [InlineData(ulong.MinValue)]
        [InlineData(ulong.MaxValue)]
        [InlineData((ulong)0x123456789ABCDEF0)]
        [InlineData((ulong)0x9ABCDEF012345678)]
        public void Test_CONV_OVF_U8_UN__UInt64(object arg0) { Test("Test_CONV_OVF_U8_UN__UInt64", arg0); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_R_UN__Boolean(object arg0) { Test("Test_CONV_R_UN__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_R_UN__Byte(object arg0) { Test("Test_CONV_R_UN__Byte", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_R_UN__Char(object arg0) { Test("Test_CONV_R_UN__Char", arg0); }

        [Theory]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        [InlineData((short)-0x1234)]
        public void Test_CONV_R_UN__Int16(object arg0) { Test("Test_CONV_R_UN__Int16", arg0); }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)-1)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        [InlineData((int)-0x12345678)]
        public void Test_CONV_R_UN__Int32(object arg0) { Test("Test_CONV_R_UN__Int32", arg0); }

        [Theory]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        [InlineData((long)0)]
        [InlineData((long)-1)]
        [InlineData((long)1)]
        [InlineData((long)0x123456789ABCDEF0)]
        [InlineData((long)-0x123456789ABCDEF0)]
        public void Test_CONV_R_UN__Int64(object arg0) { Test("Test_CONV_R_UN__Int64", arg0); }

        [Theory]
        [InlineData(int.MinValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L))]
        [InlineData(-0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L))]
        public void Test_CONV_R_UN__IntPtr(int arg0) { Test("Test_CONV_R_UN__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MinValue)]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)-1)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        [InlineData((sbyte)-0x12)]
        public void Test_CONV_R_UN__SByte(object arg0) { Test("Test_CONV_R_UN__SByte", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_R_UN__UInt16(object arg0) { Test("Test_CONV_R_UN__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData((uint)0x12345678)]
        [InlineData((uint)0x9ABCDEF0)]
        public void Test_CONV_R_UN__UInt32(object arg0) { Test("Test_CONV_R_UN__UInt32", arg0); }

        [Theory]
        [InlineData(ulong.MinValue)]
        [InlineData(ulong.MaxValue)]
        [InlineData((ulong)0x123456789ABCDEF0)]
        [InlineData((ulong)0x9ABCDEF012345678)]
        public void Test_CONV_R_UN__UInt64(object arg0) { Test("Test_CONV_R_UN__UInt64", arg0); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_R4__Boolean(object arg0) { Test("Test_CONV_R4__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_R4__Byte(object arg0) { Test("Test_CONV_R4__Byte", arg0); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(-1.0d)]
        [InlineData(double.MinValue)]
        [InlineData(1.0d)]
        [InlineData(double.MaxValue)]
        [InlineData(double.NaN)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.Epsilon)]
        [InlineData(12345678910111213.1415d)]
        [InlineData(-12345678910111213.1415d)]
        public void Test_CONV_R4__Double(object arg0) { Test("Test_CONV_R4__Double", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_R4__Char(object arg0) { Test("Test_CONV_R4__Char", arg0); }

        [Theory]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        [InlineData((short)-0x1234)]
        public void Test_CONV_R4__Int16(object arg0) { Test("Test_CONV_R4__Int16", arg0); }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)-1)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        [InlineData((int)-0x12345678)]
        public void Test_CONV_R4__Int32(object arg0) { Test("Test_CONV_R4__Int32", arg0); }

        [Theory]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        [InlineData((long)0)]
        [InlineData((long)-1)]
        [InlineData((long)1)]
        [InlineData((long)0x123456789ABCDEF0)]
        [InlineData((long)-0x123456789ABCDEF0)]
        public void Test_CONV_R4__Int64(object arg0) { Test("Test_CONV_R4__Int64", arg0); }

        [Theory]
        [InlineData(int.MinValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L))]
        [InlineData(0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L))]
        public void Test_CONV_R4__IntPtr(int arg0) { Test("Test_CONV_R4__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MinValue)]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)-1)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        [InlineData((sbyte)-0x12)]
        public void Test_CONV_R4__SByte(object arg0) { Test("Test_CONV_R4__SByte", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(-1.0f)]
        [InlineData(float.MinValue)]
        [InlineData(1.0f)]
        [InlineData(float.MaxValue)]
        [InlineData(float.NaN)]
        [InlineData(float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity)]
        [InlineData(float.Epsilon)]
        [InlineData(1234567891011.12f)]
        [InlineData(-1234567891011.12f)]
        public void Test_CONV_R4__Single(object arg0) { Test("Test_CONV_R4__Single", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_R4__UInt16(object arg0) { Test("Test_CONV_R4__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData((uint)0x12345678)]
        [InlineData((uint)0x9ABCDEF0)]
        public void Test_CONV_R4__UInt32(object arg0) { Test("Test_CONV_R4__UInt32", arg0); }

        [Theory]
        [InlineData(ulong.MinValue)]
        [InlineData(ulong.MaxValue)]
        [InlineData((ulong)0x123456789ABCDEF0)]
        [InlineData((ulong)0x9ABCDEF012345678)]
        public void Test_CONV_R4__UInt64(object arg0) { Test("Test_CONV_R4__UInt64", arg0); }

        [Theory]
        [InlineData(uint.MinValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue))]
        [InlineData(uint.MaxValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue))]
        [InlineData(0x12345678U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL))]
        [InlineData(0x9ABCDEF0U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL))]
        public void Test_CONV_R4__UIntPtr(uint arg0) { Test("Test_CONV_R4__UIntPtr", new UIntPtr(arg0)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_R8__Boolean(object arg0) { Test("Test_CONV_R8__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_R8__Byte(object arg0) { Test("Test_CONV_R8__Byte", arg0); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(-1.0d)]
        [InlineData(double.MinValue)]
        [InlineData(1.0d)]
        [InlineData(double.MaxValue)]
        [InlineData(double.NaN)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.Epsilon)]
        [InlineData(12345678910111213.1415d)]
        [InlineData(-12345678910111213.1415d)]
        public void Test_CONV_R8__Double(object arg0) { Test("Test_CONV_R8__Double", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_R8__Char(object arg0) { Test("Test_CONV_R8__Char", arg0); }

        [Theory]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        [InlineData((short)-0x1234)]
        public void Test_CONV_R8__Int16(object arg0) { Test("Test_CONV_R8__Int16", arg0); }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)-1)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        [InlineData((int)-0x12345678)]
        public void Test_CONV_R8__Int32(object arg0) { Test("Test_CONV_R8__Int32", arg0); }

        [Theory]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        [InlineData((long)0)]
        [InlineData((long)-1)]
        [InlineData((long)1)]
        [InlineData((long)0x123456789ABCDEF0)]
        [InlineData((long)-0x123456789ABCDEF0)]
        public void Test_CONV_R8__Int64(object arg0) { Test("Test_CONV_R8__Int64", arg0); }

        [Theory]
        [InlineData(int.MinValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L))]
        [InlineData(-0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L))]
        public void Test_CONV_R8__IntPtr(int arg0) { Test("Test_CONV_R8__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MinValue)]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)-1)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        [InlineData((sbyte)-0x12)]
        public void Test_CONV_R8__SByte(object arg0) { Test("Test_CONV_R8__SByte", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(-1.0f)]
        [InlineData(float.MinValue)]
        [InlineData(1.0f)]
        [InlineData(float.MaxValue)]
        [InlineData(float.NaN)]
        [InlineData(float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity)]
        [InlineData(float.Epsilon)]
        [InlineData(1234567891011.12f)]
        [InlineData(-1234567891011.12f)]
        public void Test_CONV_R8__Single(object arg0) { Test("Test_CONV_R8__Single", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_R8__UInt16(object arg0) { Test("Test_CONV_R8__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData((uint)0x12345678)]
        [InlineData((uint)0x9ABCDEF0)]
        public void Test_CONV_R8__UInt32(object arg0) { Test("Test_CONV_R8__UInt32", arg0); }

        [Theory]
        [InlineData(ulong.MinValue)]
        [InlineData(ulong.MaxValue)]
        [InlineData((ulong)0x123456789ABCDEF0)]
        [InlineData((ulong)0x9ABCDEF012345678)]
        public void Test_CONV_R8__UInt64(object arg0) { Test("Test_CONV_R8__UInt64", arg0); }

        [Theory]
        [InlineData(uint.MinValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue))]
        [InlineData(uint.MaxValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue))]
        [InlineData(0x12345678U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL))]
        [InlineData(0x9ABCDEF0U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL))]
        public void Test_CONV_R8__UIntPtr(uint arg0) { Test("Test_CONV_R8__UIntPtr", new UIntPtr(arg0)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_U__Boolean(object arg0) { Test("Test_CONV_U__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_U__Byte(object arg0) { Test("Test_CONV_U__Byte", arg0); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(-1.0d)]
        [InlineData(double.MinValue)]
        [InlineData(1.0d)]
        [InlineData(double.MaxValue)]
        [InlineData(double.NaN)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.Epsilon)]
        [InlineData(12345678910111213.1415d)]
        [InlineData(-12345678910111213.1415d)]
        public void Test_CONV_U__Double(object arg0) { Test("Test_CONV_U__Double", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_U__Char(object arg0) { Test("Test_CONV_U__Char", arg0); }

        [Theory]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        [InlineData((short)-0x1234)]
        public void Test_CONV_U__Int16(object arg0) { Test("Test_CONV_U__Int16", arg0); }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)-1)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        [InlineData((int)-0x12345678)]
        public void Test_CONV_U__Int32(object arg0) { Test("Test_CONV_U__Int32", arg0); }

        [Theory]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        [InlineData((long)0)]
        [InlineData((long)-1)]
        [InlineData((long)1)]
        [InlineData((long)0x123456789ABCDEF0)]
        [InlineData((long)-0x123456789ABCDEF0)]
        public void Test_CONV_U__Int64(object arg0) { Test("Test_CONV_U__Int64", arg0); }

        [Theory]
        [InlineData(int.MinValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L))]
        [InlineData(-0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L))]
        public void Test_CONV_U__IntPtr(int arg0) { Test("Test_CONV_U__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MinValue)]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)-1)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        [InlineData((sbyte)-0x12)]
        public void Test_CONV_U__SByte(object arg0) { Test("Test_CONV_U__SByte", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(-1.0f)]
        [InlineData(float.MinValue)]
        [InlineData(1.0f)]
        [InlineData(float.MaxValue)]
        [InlineData(float.NaN)]
        [InlineData(float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity)]
        [InlineData(float.Epsilon)]
        [InlineData(1234567891011.12f)]
        [InlineData(-1234567891011.12f)]
        public void Test_CONV_U__Single(object arg0) { Test("Test_CONV_U__Single", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_U__UInt16(object arg0) { Test("Test_CONV_U__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData((uint)0x12345678)]
        [InlineData((uint)0x9ABCDEF0)]
        public void Test_CONV_U__UInt32(object arg0) { Test("Test_CONV_U__UInt32", arg0); }

        [Theory]
        [InlineData(ulong.MinValue)]
        [InlineData(ulong.MaxValue)]
        [InlineData((ulong)0x123456789ABCDEF0)]
        [InlineData((ulong)0x9ABCDEF012345678)]
        public void Test_CONV_U__UInt64(object arg0) { Test("Test_CONV_U__UInt64", arg0); }

        [Theory]
        [InlineData(uint.MinValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue))]
        [InlineData(uint.MaxValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue))]
        [InlineData(0x12345678U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL))]
        [InlineData(0x9ABCDEF0U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL))]
        public void Test_CONV_U__UIntPtr(uint arg0) { Test("Test_CONV_U__UIntPtr", new UIntPtr(arg0)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_U1__Boolean(object arg0) { Test("Test_CONV_U1__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_U1__Byte(object arg0) { Test("Test_CONV_U1__Byte", arg0); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(-1.0d)]
        [InlineData(double.MinValue)]
        [InlineData(1.0d)]
        [InlineData(double.MaxValue)]
        [InlineData(double.NaN)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.Epsilon)]
        [InlineData(12345678910111213.1415d)]
        [InlineData(-12345678910111213.1415d)]
        public void Test_CONV_U1__Double(object arg0) { Test("Test_CONV_U1__Double", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_U1__Char(object arg0) { Test("Test_CONV_U1__Char", arg0); }

        [Theory]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        [InlineData((short)-0x1234)]
        public void Test_CONV_U1__Int16(object arg0) { Test("Test_CONV_U1__Int16", arg0); }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)-1)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        [InlineData((int)-0x12345678)]
        public void Test_CONV_U1__Int32(object arg0) { Test("Test_CONV_U1__Int32", arg0); }

        [Theory]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        [InlineData((long)0)]
        [InlineData((long)-1)]
        [InlineData((long)1)]
        [InlineData((long)0x123456789ABCDEF0)]
        [InlineData((long)-0x123456789ABCDEF0)]
        public void Test_CONV_U1__Int64(object arg0) { Test("Test_CONV_U1__Int64", arg0); }

        [Theory]
        [InlineData(int.MinValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L))]
        [InlineData(-0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L))]
        public void Test_CONV_U1__IntPtr(int arg0) { Test("Test_CONV_U1__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MinValue)]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)-1)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        [InlineData((sbyte)-0x12)]
        public void Test_CONV_U1__SByte(object arg0) { Test("Test_CONV_U1__SByte", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(-1.0f)]
        [InlineData(float.MinValue)]
        [InlineData(1.0f)]
        [InlineData(float.MaxValue)]
        [InlineData(float.NaN)]
        [InlineData(float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity)]
        [InlineData(float.Epsilon)]
        [InlineData(1234567891011.12f)]
        [InlineData(-1234567891011.12f)]
        public void Test_CONV_U1__Single(object arg0) { Test("Test_CONV_U1__Single", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_U1__UInt16(object arg0) { Test("Test_CONV_U1__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData((uint)0x12345678)]
        [InlineData((uint)0x9ABCDEF0)]
        public void Test_CONV_U1__UInt32(object arg0) { Test("Test_CONV_U1__UInt32", arg0); }

        [Theory]
        [InlineData(ulong.MinValue)]
        [InlineData(ulong.MaxValue)]
        [InlineData((ulong)0x123456789ABCDEF0)]
        [InlineData((ulong)0x9ABCDEF012345678)]
        public void Test_CONV_U1__UInt64(object arg0) { Test("Test_CONV_U1__UInt64", arg0); }

        [Theory]
        [InlineData(uint.MinValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue))]
        [InlineData(uint.MaxValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue))]
        [InlineData(0x12345678U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL))]
        [InlineData(0x9ABCDEF0U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL))]
        public void Test_CONV_U1__UIntPtr(uint arg0) { Test("Test_CONV_U1__UIntPtr", new UIntPtr(arg0)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_U2__Boolean(object arg0) { Test("Test_CONV_U2__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_U2__Byte(object arg0) { Test("Test_CONV_U2__Byte", arg0); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(-1.0d)]
        [InlineData(double.MinValue)]
        [InlineData(1.0d)]
        [InlineData(double.MaxValue)]
        [InlineData(double.NaN)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.Epsilon)]
        [InlineData(12345678910111213.1415d)]
        [InlineData(-12345678910111213.1415d)]
        public void Test_CONV_U2__Double(object arg0) { Test("Test_CONV_U2__Double", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_U2__Char(object arg0) { Test("Test_CONV_U2__Char", arg0); }

        [Theory]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        [InlineData((short)-0x1234)]
        public void Test_CONV_U2__Int16(object arg0) { Test("Test_CONV_U2__Int16", arg0); }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)-1)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        [InlineData((int)-0x12345678)]
        public void Test_CONV_U2__Int32(object arg0) { Test("Test_CONV_U2__Int32", arg0); }

        [Theory]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        [InlineData((long)0)]
        [InlineData((long)-1)]
        [InlineData((long)1)]
        [InlineData((long)0x123456789ABCDEF0)]
        [InlineData((long)-0x123456789ABCDEF0)]
        public void Test_CONV_U2__Int64(object arg0) { Test("Test_CONV_U2__Int64", arg0); }

        [Theory]
        [InlineData(int.MinValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L))]
        [InlineData(-0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L))]
        public void Test_CONV_U2__IntPtr(int arg0) { Test("Test_CONV_U2__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MinValue)]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)-1)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        [InlineData((sbyte)-0x12)]
        public void Test_CONV_U2__SByte(object arg0) { Test("Test_CONV_U2__SByte", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(-1.0f)]
        [InlineData(float.MinValue)]
        [InlineData(1.0f)]
        [InlineData(float.MaxValue)]
        [InlineData(float.NaN)]
        [InlineData(float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity)]
        [InlineData(float.Epsilon)]
        [InlineData(1234567891011.12f)]
        [InlineData(-1234567891011.12f)]
        public void Test_CONV_U2__Single(object arg0) { Test("Test_CONV_U2__Single", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_U2__UInt16(object arg0) { Test("Test_CONV_U2__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData((uint)0x12345678)]
        [InlineData((uint)0x9ABCDEF0)]
        public void Test_CONV_U2__UInt32(object arg0) { Test("Test_CONV_U2__UInt32", arg0); }

        [Theory]
        [InlineData(ulong.MinValue)]
        [InlineData(ulong.MaxValue)]
        [InlineData((ulong)0x123456789ABCDEF0)]
        [InlineData((ulong)0x9ABCDEF012345678)]
        public void Test_CONV_U2__UInt64(object arg0) { Test("Test_CONV_U2__UInt64", arg0); }

        [Theory]
        [InlineData(uint.MinValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue))]
        [InlineData(uint.MaxValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue))]
        [InlineData(0x12345678U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL))]
        [InlineData(0x9ABCDEF0U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL))]
        public void Test_CONV_U2__UIntPtr(uint arg0) { Test("Test_CONV_U2__UIntPtr", new UIntPtr(arg0)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_U4__Boolean(object arg0) { Test("Test_CONV_U4__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_U4__Byte(object arg0) { Test("Test_CONV_U4__Byte", arg0); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(-1.0d)]
        [InlineData(double.MinValue)]
        [InlineData(1.0d)]
        [InlineData(double.MaxValue)]
        [InlineData(double.NaN)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.Epsilon)]
        [InlineData(12345678910111213.1415d)]
        [InlineData(-12345678910111213.1415d)]
        public void Test_CONV_U4__Double(object arg0) { Test("Test_CONV_U4__Double", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_U4__Char(object arg0) { Test("Test_CONV_U4__Char", arg0); }

        [Theory]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        [InlineData((short)-0x1234)]
        public void Test_CONV_U4__Int16(object arg0) { Test("Test_CONV_U4__Int16", arg0); }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)-1)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        [InlineData((int)-0x12345678)]
        public void Test_CONV_U4__Int32(object arg0) { Test("Test_CONV_U4__Int32", arg0); }

        [Theory]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        [InlineData((long)0)]
        [InlineData((long)-1)]
        [InlineData((long)1)]
        [InlineData((long)0x123456789ABCDEF0)]
        [InlineData((long)-0x123456789ABCDEF0)]
        public void Test_CONV_U4__Int64(object arg0) { Test("Test_CONV_U4__Int64", arg0); }

        [Theory]
        [InlineData(int.MinValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L))]
        [InlineData(-0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L))]
        public void Test_CONV_U4__IntPtr(int arg0) { Test("Test_CONV_U4__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MinValue)]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)-1)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        [InlineData((sbyte)-0x12)]
        public void Test_CONV_U4__SByte(object arg0) { Test("Test_CONV_U4__SByte", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(-1.0f)]
        [InlineData(float.MinValue)]
        [InlineData(1.0f)]
        [InlineData(float.MaxValue)]
        [InlineData(float.NaN)]
        [InlineData(float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity)]
        [InlineData(float.Epsilon)]
        [InlineData(1234567891011.12f)]
        [InlineData(-1234567891011.12f)]
        public void Test_CONV_U4__Single(object arg0) { Test("Test_CONV_U4__Single", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_U4__UInt16(object arg0) { Test("Test_CONV_U4__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData((uint)0x12345678)]
        [InlineData((uint)0x9ABCDEF0)]
        public void Test_CONV_U4__UInt32(object arg0) { Test("Test_CONV_U4__UInt32", arg0); }

        [Theory]
        [InlineData(ulong.MinValue)]
        [InlineData(ulong.MaxValue)]
        [InlineData((ulong)0x123456789ABCDEF0)]
        [InlineData((ulong)0x9ABCDEF012345678)]
        public void Test_CONV_U4__UInt64(object arg0) { Test("Test_CONV_U4__UInt64", arg0); }

        [Theory]
        [InlineData(uint.MinValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue))]
        [InlineData(uint.MaxValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue))]
        [InlineData(0x12345678U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL))]
        [InlineData(0x9ABCDEF0U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL))]
        public void Test_CONV_U4__UIntPtr(uint arg0) { Test("Test_CONV_U4__UIntPtr", new UIntPtr(arg0)); }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_CONV_U8__Boolean(object arg0) { Test("Test_CONV_U8__Boolean", arg0); }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData((byte)0x12)]
        [InlineData((byte)0x9A)]
        public void Test_CONV_U8__Byte(object arg0) { Test("Test_CONV_U8__Byte", arg0); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(-1.0d)]
        [InlineData(double.MinValue)]
        [InlineData(1.0d)]
        [InlineData(double.MaxValue)]
        [InlineData(double.NaN)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.Epsilon)]
        [InlineData(12345678910111213.1415d)]
        [InlineData(-12345678910111213.1415d)]
        public void Test_CONV_U8__Double(object arg0) { Test("Test_CONV_U8__Double", arg0); }

        [Theory]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        public void Test_CONV_U8__Char(object arg0) { Test("Test_CONV_U8__Char", arg0); }

        [Theory]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        [InlineData((short)0x1234)]
        [InlineData((short)-0x1234)]
        public void Test_CONV_U8__Int16(object arg0) { Test("Test_CONV_U8__Int16", arg0); }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData((int)0)]
        [InlineData((int)-1)]
        [InlineData((int)1)]
        [InlineData((int)0x12345678)]
        [InlineData((int)-0x12345678)]
        public void Test_CONV_U8__Int32(object arg0) { Test("Test_CONV_U8__Int32", arg0); }

        [Theory]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        [InlineData((long)0)]
        [InlineData((long)-1)]
        [InlineData((long)1)]
        [InlineData((long)0x123456789ABCDEF0)]
        [InlineData((long)-0x123456789ABCDEF0)]
        public void Test_CONV_U8__Int64(object arg0) { Test("Test_CONV_U8__Int64", arg0); }

        [Theory]
        [InlineData(int.MinValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L))]
        [InlineData(-0x12345678)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L))]
        public void Test_CONV_U8__IntPtr(int arg0) { Test("Test_CONV_U8__IntPtr", new IntPtr(arg0)); }

        [Theory]
        [InlineData(sbyte.MinValue)]
        [InlineData(sbyte.MaxValue)]
        [InlineData((sbyte)0)]
        [InlineData((sbyte)-1)]
        [InlineData((sbyte)1)]
        [InlineData((sbyte)0x12)]
        [InlineData((sbyte)-0x12)]
        public void Test_CONV_U8__SByte(object arg0) { Test("Test_CONV_U8__SByte", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(-1.0f)]
        [InlineData(float.MinValue)]
        [InlineData(1.0f)]
        [InlineData(float.MaxValue)]
        [InlineData(float.NaN)]
        [InlineData(float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity)]
        [InlineData(float.Epsilon)]
        [InlineData(1234567891011.12f)]
        [InlineData(-1234567891011.12f)]
        public void Test_CONV_U8__Single(object arg0) { Test("Test_CONV_U8__Single", arg0); }

        [Theory]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData((ushort)0x1234)]
        [InlineData((ushort)0x9ABC)]
        public void Test_CONV_U8__UInt16(object arg0) { Test("Test_CONV_U8__UInt16", arg0); }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData((uint)0x12345678)]
        [InlineData((uint)0x9ABCDEF0)]
        public void Test_CONV_U8__UInt32(object arg0) { Test("Test_CONV_U8__UInt32", arg0); }

        [Theory]
        [InlineData(ulong.MinValue)]
        [InlineData(ulong.MaxValue)]
        [InlineData((ulong)0x123456789ABCDEF0)]
        [InlineData((ulong)0x9ABCDEF012345678)]
        public void Test_CONV_U8__UInt64(object arg0) { Test("Test_CONV_U8__UInt64", arg0); }

        [Theory]
        [InlineData(uint.MinValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue))]
        [InlineData(uint.MaxValue)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue))]
        [InlineData(0x12345678U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL))]
        [InlineData(0x9ABCDEF0U)] // [InlineData(UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL))]
        public void Test_CONV_U8__UIntPtr(uint arg0) { Test("Test_CONV_U8__UIntPtr", new UIntPtr(arg0)); }

        [Fact]
        public void Test_CPOBJ() { Test("Test_CPOBJ"); }

        [Theory]
        // TODO [InlineData(int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        // TODO [InlineData(int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(-1234, 4)]
        // TODO [InlineData(4, IntPtr.Size == 4 ? -1234 : -1234)]
        [InlineData(1, 0)]
        [InlineData(int.MinValue, 0)]
        public void Test_DIV__Int32_IntPtr(object arg0, int arg1) { Test("Test_DIV__Int32_IntPtr", arg0, new IntPtr(arg1)); }

        [Theory]
        // TODO [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue)]
        // TODO [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue)]
        // TODO [InlineData(IntPtr.Size == 4 ? -1234 : -1234, 4)]
        [InlineData(4, -1234)]
        [InlineData(4, 0)]
        [InlineData(0, int.MinValue)]
        public void Test_DIV__IntPtr_Int32(int arg0, object arg1) { Test("Test_DIV__IntPtr_Int32", new IntPtr(arg0), arg1); }

        [Theory]
        // TODO [InlineData(int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        // TODO [InlineData(int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(-1234, 4)]
        // TODO [InlineData(4, IntPtr.Size == 4 ? -1234 : -1234)]
        [InlineData(1, 0)]
        [InlineData(int.MinValue, 0)]
        public void Test_DIV_UN__Int32_IntPtr(object arg0, int arg1) { Test("Test_DIV_UN__Int32_IntPtr", arg0, new IntPtr(arg1)); }

        [Theory]
        // TODO [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue)]
        // TODO [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue)]
        // TODO [InlineData(IntPtr.Size == 4 ? -1234 : -1234, 4)]
        [InlineData(4, -1234)]
        [InlineData(1, 0)]
        [InlineData(0, int.MinValue)]
        public void Test_DIV_UN__IntPtr_Int32(int arg0, object arg1) { Test("Test_DIV_UN__IntPtr_Int32", new IntPtr(arg0), arg1); }

        [Fact]
        public void Test_DUP() { Test("Test_DUP", "hello", "bye"); }

        [Fact]
        public void Test_INITBLK() { Test("Test_INITBLK"); }

        [Fact]
        public void Test_INITOBJ() { Test("Test_INITOBJ"); }

        [Fact]
        public void Test_ISINST() { Test("Test_ISINST"); }

        [Fact]
        public void Test_LDARG__0() { Test("Test_LDARG__0", 123); }

        [Fact]
        public void Test_LDARG__128() { Test("Test_LDARG__128", "P1", "P2", "P3", "P4", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "P126", "P127", 123); }

        [Fact]
        public void Test_LDARG__4() { Test("Test_LDARG__4", "", "", "", "", 123); }

        [Fact]
        public void Test_LDARG_0() { Test("Test_LDARG_0", 123); }

        [Fact]
        public void Test_LDARG_1() { Test("Test_LDARG_1", "", 123); }

        [Fact]
        public void Test_LDARG_2() { Test("Test_LDARG_2", "", "", 123); }

        [Fact]
        public void Test_LDARG_3() { Test("Test_LDARG_3", "", "", "", 123); }

        [Fact]
        public void Test_LDARG_S__0() { Test("Test_LDARG_S__0", 123); }

        [Fact]
        public void Test_LDARG_S__128() { Test("Test_LDARG_S__128", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 123); }

        [Fact]
        public void Test_LDARG_S__4() { Test("Test_LDARG_S__4", "", "", "", "", 123); }

        [Fact]
        public void Test_LDARGA() { Test("Test_LDARGA", 123, "hello"); }

        [Fact]
        public void Test_LDARGA_S() { Test("Test_LDARGA_S", 123, "hello"); }

        [Fact]
        public void Test_LDC_I4__0() { Test("Test_LDC_I4__0"); }

        [Fact]
        public void Test_LDC_I4__1() { Test("Test_LDC_I4__1"); }

        [Fact]
        public void Test_LDC_I4__M1() { Test("Test_LDC_I4__M1"); }

        [Fact]
        public void Test_LDC_I4__MaxValue() { Test("Test_LDC_I4__MaxValue"); }

        [Fact]
        public void Test_LDC_I4__MinValue() { Test("Test_LDC_I4__MinValue"); }

        [Fact]
        public void Test_LDC_I4_0() { Test("Test_LDC_I4_0"); }

        [Fact]
        public void Test_LDC_I4_1() { Test("Test_LDC_I4_1"); }

        [Fact]
        public void Test_LDC_I4_2() { Test("Test_LDC_I4_2"); }

        [Fact]
        public void Test_LDC_I4_3() { Test("Test_LDC_I4_3"); }

        [Fact]
        public void Test_LDC_I4_4() { Test("Test_LDC_I4_4"); }

        [Fact]
        public void Test_LDC_I4_5() { Test("Test_LDC_I4_5"); }

        [Fact]
        public void Test_LDC_I4_6() { Test("Test_LDC_I4_6"); }

        [Fact]
        public void Test_LDC_I4_7() { Test("Test_LDC_I4_7"); }

        [Fact]
        public void Test_LDC_I4_8() { Test("Test_LDC_I4_8"); }

        [Fact]
        public void Test_LDC_I4_M1() { Test("Test_LDC_I4_M1"); }

        [Fact]
        public void Test_LDC_I4_S__0() { Test("Test_LDC_I4_S__0"); }

        [Fact]
        public void Test_LDC_I4_S__1() { Test("Test_LDC_I4_S__1"); }

        [Fact]
        public void Test_LDC_I4_S__M1() { Test("Test_LDC_I4_S__M1"); }

        [Fact]
        public void Test_LDC_I4_S__MaxValue() { Test("Test_LDC_I4_S__MaxValue"); }

        [Fact]
        public void Test_LDC_I4_S__MinValue() { Test("Test_LDC_I4_S__MinValue"); }

        [Fact]
        public void Test_LDC_I8__0() { Test("Test_LDC_I8__0"); }

        [Fact]
        public void Test_LDC_I8__1() { Test("Test_LDC_I8__1"); }

        [Fact]
        public void Test_LDC_I8__M1() { Test("Test_LDC_I8__M1"); }

        [Fact]
        public void Test_LDC_I8__MaxValue() { Test("Test_LDC_I8__MaxValue"); }

        [Fact]
        public void Test_LDC_I8__MinValue() { Test("Test_LDC_I8__MinValue"); }

        [Fact]
        public void Test_LDC_R4__1() { Test("Test_LDC_R4__1"); }

        [Fact]
        public void Test_LDC_R4__Epsilon() { Test("Test_LDC_R4__Epsilon"); }

        [Fact]
        public void Test_LDC_R4__M1() { Test("Test_LDC_R4__M1"); }

        [Fact]
        public void Test_LDC_R4__MaxValue() { Test("Test_LDC_R4__MaxValue"); }

        [Fact]
        public void Test_LDC_R4__MinValue() { Test("Test_LDC_R4__MinValue"); }

        [Fact]
        public void Test_LDC_R4__NaN() { Test("Test_LDC_R4__NaN"); }

        [Fact]
        public void Test_LDC_R4__NegativeInfinity() { Test("Test_LDC_R4__NegativeInfinity"); }

        [Fact]
        public void Test_LDC_R4__NegativeZero() { Test("Test_LDC_R4__NegativeZero"); }

        [Fact]
        public void Test_LDC_R4__PositiveInfinity() { Test("Test_LDC_R4__PositiveInfinity"); }

        [Fact]
        public void Test_LDC_R4__PositiveZero() { Test("Test_LDC_R4__PositiveZero"); }

        [Fact]
        public void Test_LDC_R8__1() { Test("Test_LDC_R8__1"); }

        [Fact]
        public void Test_LDC_R8__Epsilon() { Test("Test_LDC_R8__Epsilon"); }

        [Fact]
        public void Test_LDC_R8__M1() { Test("Test_LDC_R8__M1"); }

        [Fact]
        public void Test_LDC_R8__MaxValue() { Test("Test_LDC_R8__MaxValue"); }

        [Fact]
        public void Test_LDC_R8__MinValue() { Test("Test_LDC_R8__MinValue"); }

        [Fact]
        public void Test_LDC_R8__NaN() { Test("Test_LDC_R8__NaN"); }

        [Fact]
        public void Test_LDC_R8__NegativeInfinity() { Test("Test_LDC_R8__NegativeInfinity"); }

        [Fact]
        public void Test_LDC_R8__NegativeZero() { Test("Test_LDC_R8__NegativeZero"); }

        [Fact]
        public void Test_LDC_R8__PositiveInfinity() { Test("Test_LDC_R8__PositiveInfinity"); }

        [Fact]
        public void Test_LDC_R8__PositiveZero() { Test("Test_LDC_R8__PositiveZero"); }

        [Fact]
        public void Test_LDELEM__STELEM() { Test("Test_LDELEM__STELEM"); }

        [Fact]
        public void Test_LDELEM_I__STELEM_I() { Test("Test_LDELEM_I__STELEM_I"); }

        [Fact]
        public void Test_LDELEM_I1__STELEM_I1() { Test("Test_LDELEM_I1__STELEM_I1"); }

        [Fact]
        public void Test_LDELEM_I2__STELEM_I2() { Test("Test_LDELEM_I2__STELEM_I2"); }

        [Fact]
        public void Test_LDELEM_I4__STELEM_I4() { Test("Test_LDELEM_I4__STELEM_I4"); }

        [Fact]
        public void Test_LDELEM_I8__STELEM_I8() { Test("Test_LDELEM_I8__STELEM_I8"); }

        [Fact]
        public void Test_LDELEM_R4__STELEM_R4() { Test("Test_LDELEM_R4__STELEM_R4"); }

        [Fact]
        public void Test_LDELEM_R8__STELEM_R8() { Test("Test_LDELEM_R8__STELEM_R8"); }

        [Fact]
        public void Test_LDELEM_REF__STELEM_REF() { Test("Test_LDELEM_REF__STELEM_REF"); }

        [Fact]
        public void Test_LDELEM_U1__STELEM_I1() { Test("Test_LDELEM_U1__STELEM_I1"); }

        [Fact]
        public void Test_LDELEM_U2__STELEM_I2() { Test("Test_LDELEM_U2__STELEM_I2"); }

        [Fact]
        public void Test_LDELEM_U4__STELEM_I4() { Test("Test_LDELEM_U4__STELEM_I4"); }

        [Fact]
        public void Test_LDELEMA() { Test("Test_LDELEMA"); }

        [Fact]
        public void Test_LDFLD_STFLD_LDFLDA() { Test("Test_LDFLD_STFLD_LDFLDA"); }

        [Fact]
        public void Test_LDIND_STIND() { Test("Test_LDIND_STIND"); }

        [Theory]
        [InlineData(0)]
        [InlineData(123)]
        public void Test_LDLEN__Int32(object arg0) { Test("Test_LDLEN__Int32", arg0); }

        [Theory]
        [InlineData(0)]
        // TODO [InlineData(IntPtr.Size == 4 ? 123 : 123)]
        public void Test_LDLEN__IntPtr(int arg0) { Test("Test_LDLEN__IntPtr", new IntPtr(arg0)); }

        [Fact]
        public void Test_LDLOC__0() { Test("Test_LDLOC__0"); }

        [Fact]
        public void Test_LDLOC__128() { Test("Test_LDLOC__128"); }

        [Fact]
        public void Test_LDLOC__4() { Test("Test_LDLOC__4"); }

        [Fact]
        public void Test_LDLOC_0() { Test("Test_LDLOC_0"); }

        [Fact]
        public void Test_LDLOC_1() { Test("Test_LDLOC_1"); }

        [Fact]
        public void Test_LDLOC_2() { Test("Test_LDLOC_2"); }

        [Fact]
        public void Test_LDLOC_3() { Test("Test_LDLOC_3"); }

        [Fact]
        public void Test_LDLOC_S__0() { Test("Test_LDLOC_S__0"); }

        [Fact]
        public void Test_LDLOC_S__128() { Test("Test_LDLOC_S__128"); }

        [Fact]
        public void Test_LDLOC_S__4() { Test("Test_LDLOC_S__4"); }

        [Fact]
        public void Test_LDLOCA() { Test("Test_LDLOCA"); }

        [Fact]
        public void Test_LDLOCA_S() { Test("Test_LDLOCA_S"); }

        [Fact]
        public void Test_LDNULL() { Test("Test_LDNULL"); }

        [Fact]
        public void Test_LDOBJ_STOBJ() { Test("Test_LDOBJ_STOBJ"); }

        [Fact]
        public void Test_LDSFLD_STSFLD_LDSFLDA() { Test("Test_LDSFLD_STSFLD_LDSFLDA"); }

        [Fact]
        public void Test_LDSTR() { Test("Test_LDSTR"); }

        [Fact]
        public void Test_LDTOKEN__Field() { Test("Test_LDTOKEN__Field"); }

        [Fact]
        public void Test_LDTOKEN__Method() { Test("Test_LDTOKEN__Method"); }

        [Fact]
        public void Test_LDTOKEN__Type() { Test("Test_LDTOKEN__Type"); }

        [Fact]
        public void Test_LEAVE() { Test("Test_LEAVE"); }

        [Fact]
        public void Test_LEAVE__0() { Test("Test_LEAVE__0"); }

        [Fact]
        public void Test_LEAVE_S() { Test("Test_LEAVE_S"); }

        [Fact]
        public void Test_LEAVE_S__0() { Test("Test_LEAVE_S__0"); }

        [Fact]
        public void Test_LOCALLOC() { Test("Test_LOCALLOC"); }

        [Theory]
        // TODO [InlineData(int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        // TODO [InlineData(int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(-5, 4)]
        [InlineData(4, -5)]
        [InlineData(int.MinValue, 0)]
        public void Test_MUL__Int32_IntPtr(object arg0, int arg1) { Test("Test_MUL__Int32_IntPtr", arg0, new IntPtr(arg1)); }

        [Theory]
        // TODO [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue)]
        // TODO [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue)]
        [InlineData(-5, 4)]
        [InlineData(4, -5)]
        [InlineData(0, int.MinValue)]
        public void Test_MUL__IntPtr_Int32(int arg0, object arg1) { Test("Test_MUL__IntPtr_Int32", new IntPtr(arg0), arg1); }

        [Theory]
        [InlineData(-5, 4)]
        [InlineData(4, -5)]
        [InlineData(int.MinValue, 0)]
        public void Test_MUL_OVF__Int32_IntPtr(object arg0, int arg1) { Test("Test_MUL_OVF__Int32_IntPtr", arg0, new IntPtr(arg1)); }

        [Theory]
        [InlineData(-5, 4)]
        [InlineData(4, -5)]
        [InlineData(0, int.MinValue)]
        public void Test_MUL_OVF__IntPtr_Int32(int arg0, object arg1) { Test("Test_MUL_OVF__IntPtr_Int32", new IntPtr(arg0), arg1); }

        [Theory]
        [InlineData(5, 4)]
        [InlineData(4, 5)]
        [InlineData(int.MinValue, 0)]
        public void Test_MUL_OVF_UN__Int32_IntPtr(object arg0, int arg1) { Test("Test_MUL_OVF_UN__Int32_IntPtr", arg0, new IntPtr(arg1)); }

        [Theory]
        [InlineData(5, 4)]
        [InlineData(4, 5)]
        [InlineData(0, int.MinValue)]
        public void Test_MUL_OVF_UN__IntPtr_Int32(int arg0, object arg1) { Test("Test_MUL_OVF_UN__IntPtr_Int32", new IntPtr(arg0), arg1); }

        [Theory]
        [InlineData(12345678910111213.14151617d)]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        [InlineData(double.NaN)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.Epsilon)]
        public void Test_NEG__Double(object arg0) { Test("Test_NEG__Double", arg0); }

        [Fact]
        public void Test_NEG__Int32() { Test("Test_NEG__Int32", 0x12345678); }

        [Fact]
        public void Test_NEG__Int64() { Test("Test_NEG__Int64", 0x123456789ABCDEF0L); }

        [Fact]
        public void Test_NEG__IntPtr() { Test("Test_NEG__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); }

        [Theory]
        [InlineData(123456.789f)]
        [InlineData(float.MinValue)]
        [InlineData(float.MaxValue)]
        [InlineData(float.NaN)]
        [InlineData(float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity)]
        [InlineData(float.Epsilon)]
        public void Test_NEG__Single(object arg0) { Test("Test_NEG__Single", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(123)]
        public void Test_NEWARR__Int32(object arg0) { Test("Test_NEWARR__Int32", arg0); }

        [Theory]
        [InlineData(0)]
        // TODO [InlineData(IntPtr.Size == 4 ? 123 : 123)]
        public void Test_NEWARR__IntPtr(int arg0) { Test("Test_NEWARR__IntPtr", new IntPtr(arg0)); }

        [Fact]
        public void Test_NEWOBJ__Class() { Test("Test_NEWOBJ__Class"); }

        [Fact]
        public void Test_NEWOBJ__Struct() { Test("Test_NEWOBJ__Struct"); }

        [Fact]
        public void Test_NOP() { Test("Test_NOP"); }

        [Fact]
        public void Test_NOT__Int32() { Test("Test_NOT__Int32", 0x12345678); }

        [Fact]
        public void Test_NOT__Int64() { Test("Test_NOT__Int64", 0x123456789ABCDEF0L); }

        [Fact]
        public void Test_NOT__IntPtr() { Test("Test_NOT__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); }

        [Fact]
        public void Test_OR__Int32() { Test("Test_OR__Int32", 0x5AA51234, 0x3FF37591); }

        [Fact]
        public void Test_OR__Int32_IntPtr() { Test("Test_OR__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); }

        [Fact]
        public void Test_OR__Int64() { Test("Test_OR__Int64", 0x5AA5123467306AB8L, 0x3FF375919AE00BB6L); }

        [Fact]
        public void Test_OR__IntPtr() { Test("Test_OR__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x5AA51234) : new IntPtr(0x5AA5123467306AB8L), IntPtr.Size == 4 ? new IntPtr(0x3FF37591) : new IntPtr(0x3FF375919AE00BB6L)); }

        [Fact]
        public void Test_OR__IntPtr_Int32() { Test("Test_OR__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); }

        [Fact]
        public void Test_POP() { Test("Test_POP"); }

        [Fact]
        public void Test_READONLY() { Test("Test_READONLY"); }

        [Theory]
        // TODO [InlineData(int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        // TODO [InlineData(int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(-1234, 4)]
        // TODO [InlineData(4, IntPtr.Size == 4 ? -1234 : -1234)]
        [InlineData(4, 0)]
        [InlineData(int.MinValue, 0)]
        public void Test_REM__Int32_IntPtr(object arg0, int arg1) { Test("Test_REM__Int32_IntPtr", arg0, new IntPtr(arg1)); }

        [Theory]
        // TODO [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue)]
        // TODO [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue)]
        // TODO [InlineData(IntPtr.Size == 4 ? -1234 : -1234, 4)]
        [InlineData(4, -1234)]
        [InlineData(4, 0)]
        [InlineData(0, int.MinValue)]
        public void Test_REM__IntPtr_Int32(int arg0, object arg1) { Test("Test_REM__IntPtr_Int32", new IntPtr(arg0), arg1); }

        [Theory]
        // TODO [InlineData(int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        // TODO [InlineData(int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(-1234, 4)]
        // TODO [InlineData(4, IntPtr.Size == 4 ? -1234 : -1234)]
        [InlineData(1, 0)]
        [InlineData(int.MinValue, 0)]
        public void Test_REM_UN__Int32_IntPtr(object arg0, int arg1) { Test("Test_REM_UN__Int32_IntPtr", arg0, new IntPtr(arg1)); }

        [Theory]
        // TODO [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue)]
        // TODO [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue)]
        // TODO [InlineData(IntPtr.Size == 4 ? -1234 : -1234, 4)]
        [InlineData(4, -1234)]
        [InlineData(1, 0)]
        [InlineData(0, int.MinValue)]
        public void Test_REM_UN__IntPtr_Int32(int arg0, object arg1) { Test("Test_REM_UN__IntPtr_Int32", new IntPtr(arg0), arg1); }

        [Fact]
        public void Test_RET__Void() { Test("Test_RET__Void"); }

        [Fact]
        public void Test_SHL__Int32() { Test("Test_SHL__Int32", -0x5AA51234, 5); }

        [Fact]
        public void Test_SHL__Int64() { Test("Test_SHL__Int64", -0x5AA5123467306AB8L, 5); }

        [Fact]
        public void Test_SHL__IntPtr() { Test("Test_SHL__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x5AA51234) : new IntPtr(-0x5AA5123467306AB8L), 5); }

        [Fact]
        public void Test_SHR__Int32() { Test("Test_SHR__Int32", -0x5AA51234, 5); }

        [Fact]
        public void Test_SHR__Int64() { Test("Test_SHR__Int64", -0x5AA5123467306AB8L, 5); }

        [Fact]
        public void Test_SHR__IntPtr() { Test("Test_SHR__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x5AA51234) : new IntPtr(-0x5AA5123467306AB8L), 5); }

        [Fact]
        public void Test_SHR_UN__Int32() { Test("Test_SHR_UN__Int32", -0x5AA51234, 5); }

        [Fact]
        public void Test_SHR_UN__Int64() { Test("Test_SHR_UN__Int64", -0x5AA5123467306AB8L, 5); }

        [Fact]
        public void Test_SHR_UN__IntPtr() { Test("Test_SHR_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x5AA51234) : new IntPtr(-0x5AA5123467306AB8L), 5); }

        [Fact]
        public void Test_SIZEOF() { Test("Test_SIZEOF", IntPtr.Size); }

        [Fact]
        public void Test_STARG__0() { Test("Test_STARG__0", 123, 456); }

        [Fact]
        public void Test_STARG__128() { Test("Test_STARG__128", 456, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 123); }

        [Fact]
        public void Test_STARG_S__0() { Test("Test_STARG_S__0", 123, 456); }

        [Fact]
        public void Test_STARG_S__128() { Test("Test_STARG_S__128", 456, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 123); }

        [Fact]
        public void Test_STLOC__0() { Test("Test_STLOC__0", 123); }

        [Fact]
        public void Test_STLOC__128() { Test("Test_STLOC__128", 123); }

        [Fact]
        public void Test_STLOC_0() { Test("Test_STLOC_0", 123); }

        [Fact]
        public void Test_STLOC_1() { Test("Test_STLOC_1", 123); }

        [Fact]
        public void Test_STLOC_2() { Test("Test_STLOC_2", 123); }

        [Fact]
        public void Test_STLOC_3() { Test("Test_STLOC_3", 123); }

        [Fact]
        public void Test_STLOC_S__0() { Test("Test_STLOC_S__0", 123); }

        [Fact]
        public void Test_STLOC_S__128() { Test("Test_STLOC_S__128", 123); }

        [Theory]
        // TODO [InlineData(int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        // TODO [InlineData(int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(-5, 4)]
        [InlineData(4, -5)]
        [InlineData(int.MinValue, 0)]
        public void Test_SUB__Int32_IntPtr(object arg0, int arg1) { Test("Test_SUB__Int32_IntPtr", arg0, new IntPtr(arg1)); }

        [Theory]
        // TODO [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue)]
        // TODO [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue)]
        [InlineData(-5, 4)]
        [InlineData(4, -5)]
        [InlineData(0, int.MinValue)]
        public void Test_SUB__IntPtr_Int32(int arg0, object arg1) { Test("Test_SUB__IntPtr_Int32", new IntPtr(arg0), arg1); }

        [Theory]
        [InlineData(-5, 4)]
        [InlineData(4, -5)]
        [InlineData(int.MinValue, 0)]
        public void Test_SUB_OVF__Int32_IntPtr(object arg0, int arg1) { Test("Test_SUB_OVF__Int32_IntPtr", arg0, new IntPtr(arg1)); }

        [Theory]
        [InlineData(-5, 4)]
        [InlineData(4, -5)]
        [InlineData(0, int.MinValue)]
        public void Test_SUB_OVF__IntPtr_Int32(int arg0, object arg1) { Test("Test_SUB_OVF__IntPtr_Int32", new IntPtr(arg0), arg1); }

        [Theory]
        [InlineData(-5, 4)]
        [InlineData(int.MinValue, 0)]
        public void Test_SUB_OVF_UN__Int32_IntPtr(object arg0, int arg1) { Test("Test_SUB_OVF_UN__Int32_IntPtr", arg0, new IntPtr(arg1)); }

        [Theory]
        // TODO [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue)]
        [InlineData(-5, 4)]
        [InlineData(0, int.MinValue)]
        public void Test_SUB_OVF_UN__IntPtr_Int32(int arg0, object arg1) { Test("Test_SUB_OVF_UN__IntPtr_Int32", new IntPtr(arg0), arg1); }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void Test_SWITCH__0(object arg0) { Test("Test_SWITCH__0", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void Test_SWITCH__1(object arg0) { Test("Test_SWITCH__1", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData(2)]
        public void Test_SWITCH__2(object arg0) { Test("Test_SWITCH__2", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData(2)]
        [InlineData(3)]
        public void Test_SWITCH__3(object arg0) { Test("Test_SWITCH__3", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void Test_SWITCH__4(object arg0) { Test("Test_SWITCH__4", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Test_SWITCH__5(object arg0) { Test("Test_SWITCH__5", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public void Test_SWITCH__6(object arg0) { Test("Test_SWITCH__6", arg0); }

        /*Theory]
        [InlineData(0.0d)]
        [InlineData(-1.0d)]
        [InlineData(1.0d)]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        [InlineData(double.NaN)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.Epsilon)]
        [InlineData(2.0d)]
        [InlineData(3.0d)]
        [InlineData(4.0d)]
        [InlineData(5.0d)]
        [InlineData(6.0d)]
        public void Test_SWITCH__6_Double(object arg0) { Test("Test_SWITCH__6_Double", arg0); }*/

        [Theory]
        [InlineData(0L)]
        [InlineData(-1L)]
        [InlineData(1L)]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        [InlineData(2L)]
        [InlineData(3L)]
        [InlineData(4L)]
        [InlineData(5L)]
        [InlineData(6L)]
        public void Test_SWITCH__6_Int64(object arg0) { Test("Test_SWITCH__6_Int64", arg0); }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(int.MinValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue))]
        [InlineData(int.MaxValue)] // [InlineData(IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue))]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public void Test_SWITCH__6_IntPtr(int arg0) { Test("Test_SWITCH__6_IntPtr", new IntPtr(arg0)); }

        /*[Theory]
        [InlineData(0.0f)]
        [InlineData(-1.0f)]
        [InlineData(1.0f)]
        [InlineData(float.MinValue)]
        [InlineData(float.MaxValue)]
        [InlineData(float.NaN)]
        [InlineData(float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity)]
        [InlineData(float.Epsilon)]
        [InlineData(2.0f)]
        [InlineData(3.0f)]
        [InlineData(4.0f)]
        [InlineData(5.0f)]
        [InlineData(6.0f)]
        public void Test_SWITCH__6_Single(object arg0) { Test("Test_SWITCH__6_Single", arg0); }*/

        [Fact]
        public void Test_UNALIGNED() { Test("Test_UNALIGNED"); }

        [Fact]
        public void Test_UNALIGNED_VOLATILE() { Test("Test_UNALIGNED_VOLATILE"); }

        [Fact]
        public void Test_VOLATILE() { Test("Test_VOLATILE"); }

        [Fact]
        public void Test_XOR__Int32() { Test("Test_XOR__Int32", 0x5AA51234, 0x3FF37591); }

        [Fact]
        public void Test_XOR__Int32_IntPtr() { Test("Test_XOR__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); }

        [Fact]
        public void Test_XOR__Int64() { Test("Test_XOR__Int64", 0x5AA5123467306AB8L, 0x3FF375919AE00BB6L); }

        [Fact]
        public void Test_XOR__IntPtr() { Test("Test_XOR__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x5AA51234) : new IntPtr(0x5AA5123467306AB8L), IntPtr.Size == 4 ? new IntPtr(0x3FF37591) : new IntPtr(0x3FF375919AE00BB6L)); }

        [Fact]
        public void Test_XOR__IntPtr_Int32() { Test("Test_XOR__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); }



        /*

        void Verify(DmdType type, ILValue v1, object v2)
{
    var o1 = testRuntime.Convert(v1, type);
    VerifyValues(o1, v2);
}

void VerifyValues(object o1, object o2)
{
    bool b;
    if (o1 is float f1 && o2 is float f2)
        b = Equals(BitConverter.GetBytes(f1), BitConverter.GetBytes(f2));
    else if (o1 is double d1 && o2 is double d2)
        b = Equals(BitConverter.GetBytes(d1), BitConverter.GetBytes(d2));
    else
        b = Equals(o1, o2);
    if (!b)
        System.Diagnostics.Debugger.Break();
}

bool Verify(bool b)
{
    if (!b)
        System.Diagnostics.Debugger.Break();
    return b;
}

static bool Equals(byte[] a, byte[] b)
{
    if (a == b)
        return true;
    if (a is null || b is null)
        return false;
    if (a.Length != b.Length)
        return false;
    for (int i = 0; i < a.Length; i++)
    {
        if (a[i] != b[i])
            return false;
    }
    return true;
}

void TestMethodEX(string methodName, params object[] args)
{
#if EXCEPTIONS
            TestMethod(methodName, args);
#endif
}
void // TestMethodEX2(string methodName, params object[] args)
{
#if EXCEPTIONS
            TestMethod2(methodName, args);
#endif
}

void TestMethod(string methodName, params object[] args)
{
    var m1 = testType1.GetMethod(methodName) ?? throw new InvalidOperationException();
    var m2 = testType2.GetMethod(methodName) ?? throw new InvalidOperationException();
    if (args.Length != m1.GetMethodSignature().GetParameterTypes().Count)
        throw new InvalidOperationException();
    testRuntime.SetMethodExecState(CreateArguments(args), m1.GetMethodBody());
    var state = ilvm.CreateExecuteState(m1);
    ILValue res1;
    object res2;
    Exception ex1 = null, ex2 = null;
    try
    {
        res1 = ilvm.Execute(testRuntime.DebuggerRuntime, state);
    }
    catch (Exception ex)
    {
        ex1 = ex;
        res1 = null;
    }
    try
    {
        res2 = m2.Invoke(null, args);
    }
    catch (TargetInvocationException tie)
    {
        ex2 = tie.InnerException;
        res2 = null;
    }
    catch (Exception ex)
    {
        ex2 = ex;
        res2 = null;
    }
    if (!(ex1 is null) || !(ex2 is null))
        Verify(ex1?.GetType().FullName == ex2?.GetType().FullName);
    else
        Verify(m1.ReturnType, res1, res2);
}

void TestMethod2(string methodName, params object[] args)
{
    if (args.Length != 2)
        throw new InvalidOperationException();
    var args2 = new object[2] { args[1], args[0] };
    TestMethod(methodName, args);
    TestMethod(methodName, args2);
}

void TestMethod_BR(string methodName1, string methodName2, params object[] args)
{
    if (args.Length != 2)
        throw new InvalidOperationException();
    var args2 = new object[2] { args[1], args[0] };
    TestMethod(methodName1, args);
    TestMethod(methodName2, args2);
}

ILValue[] CreateArguments(object[] args)
{
    if (args.Length == 0)
        return Array.Empty<ILValue>();
    var res = new ILValue[args.Length];
    for (int i = 0; i < res.Length; i++)
        res[i] = CreateArgument(args[i]);
    return res;
}

ILValue CreateArgument(object value)
{
    if (value is null)
        return new NullObjectRefILValue();
    switch (Type.GetTypeCode(value.GetType()))
    {
        case TypeCode.Boolean: return new ConstantInt32ILValue(testAsm1.AppDomain, (bool)value ? 1 : 0);
        case TypeCode.Char: return new ConstantInt32ILValue(testAsm1.AppDomain, (char)value);
        case TypeCode.SByte: return new ConstantInt32ILValue(testAsm1.AppDomain, (sbyte)value);
        case TypeCode.Byte: return new ConstantInt32ILValue(testAsm1.AppDomain, (byte)value);
        case TypeCode.Int16: return new ConstantInt32ILValue(testAsm1.AppDomain, (short)value);
        case TypeCode.UInt16: return new ConstantInt32ILValue(testAsm1.AppDomain, (ushort)value);
        case TypeCode.Int32: return new ConstantInt32ILValue(testAsm1.AppDomain, (int)value);
        case TypeCode.UInt32: return new ConstantInt32ILValue(testAsm1.AppDomain, (int)(uint)value);
        case TypeCode.Int64: return new ConstantInt64ILValue(testAsm1.AppDomain, (long)value);
        case TypeCode.UInt64: return new ConstantInt64ILValue(testAsm1.AppDomain, (long)(ulong)value);
        case TypeCode.Single: return new ConstantFloatILValue(testAsm1.AppDomain, (float)value);
        case TypeCode.Double: return new ConstantFloatILValue(testAsm1.AppDomain, (double)value);
        case TypeCode.String: return new Fake.ConstantStringILValue(testAsm1.AppDomain.System_String, (string)value);
        default:
            if (value is IntPtr ip)
                return IntPtr.Size == 4 ? ConstantNativeIntILValue.Create32(testAsm1.AppDomain, ip.ToInt32()) : ConstantNativeIntILValue.Create64(testAsm1.AppDomain, ip.ToInt64());
            if (value is UIntPtr up)
                return IntPtr.Size == 4 ? ConstantNativeIntILValue.Create32(testAsm1.AppDomain, (int)up.ToUInt32()) : ConstantNativeIntILValue.Create64(testAsm1.AppDomain, (long)up.ToUInt64());
            throw new InvalidOperationException();
    }
}
    }
}
*/
    }
}