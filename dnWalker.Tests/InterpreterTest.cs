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

using dnWalker;
using dnWalker.Tests;
using MMC;
using MMC.Data;
using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace dnSpy.Debugger.DotNet.Interpreter.Tests
{
    /*abstract class TestRuntime
    {
        public abstract DmdRuntime Runtime { get; }
        public abstract DebuggerRuntime DebuggerRuntime { get; }
        public abstract void SetMethodExecState(ILValue[] arguments, DmdMethodBody body);

        public object Convert(ILValue v, DmdType type)
        {
            if (v.IsNull)
                return null;
            switch (v.Kind)
            {
                case ILValueKind.Int32:
                    var v32 = (ConstantInt32ILValue)v;
                    switch (DmdType.GetTypeCode(type))
                    {
                        case TypeCode.Boolean: return v32.Value != 0;
                        case TypeCode.Char: return (char)v32.Value;
                        case TypeCode.SByte: return (sbyte)v32.Value;
                        case TypeCode.Byte: return (byte)v32.Value;
                        case TypeCode.Int16: return (short)v32.Value;
                        case TypeCode.UInt16: return (ushort)v32.Value;
                        case TypeCode.Int32: return (int)v32.Value;
                        case TypeCode.UInt32: return (uint)v32.Value;
                        case TypeCode.Int64: return (long)v32.Value;
                        case TypeCode.UInt64: return (ulong)v32.Value;
                        default:
                            throw new InvalidOperationException();
                    }

                case ILValueKind.Int64:
                    var v64 = (ConstantInt64ILValue)v;
                    switch (DmdType.GetTypeCode(type))
                    {
                        case TypeCode.Int64: return (long)v64.Value;
                        case TypeCode.UInt64: return (ulong)v64.Value;
                        default:
                            throw new InvalidOperationException();
                    }

                case ILValueKind.Float:
                    var f = (ConstantFloatILValue)v;
                    switch (DmdType.GetTypeCode(type))
                    {
                        case TypeCode.Single: return (float)f.Value;
                        case TypeCode.Double: return (double)f.Value;
                        default:
                            throw new InvalidOperationException();
                    }

                case ILValueKind.NativeInt:
                    if (v is ConstantNativeIntILValue ni)
                    {
                        if (type == type.AppDomain.System_IntPtr)
                            return IntPtr.Size == 4 ? new IntPtr(ni.Value32) : new IntPtr(ni.Value64);
                        if (type == type.AppDomain.System_UIntPtr)
                            return UIntPtr.Size == 4 ? new UIntPtr(ni.UnsignedValue32) : new UIntPtr(ni.UnsignedValue64);
                    }
                    break;

                case ILValueKind.Type:
                    if (v is Fake.ConstantStringILValue s)
                        return s.Value;
                    break;

                case ILValueKind.ByRef:
                    break;
            }

            throw new InvalidOperationException();
        }
    }*/

    [Trait("Category", "Interpreter")]
    public sealed class InterpreterTest : TestBase
    {
        private const string AssemblyFilename = @"..\..\..\extras\dnSpy.Debugger.DotNet.Interpreter.Tests.dll";

        /*

sealed class DmdEvaluatorImpl : DmdEvaluator
{
public override object CreateInstance(object context, DmdConstructorInfo ctor, object[] arguments) => throw new NotImplementedException();
public override object? Invoke(object? context, DmdMethodBase method, object? obj, object?[] parameters) => throw new NotImplementedException();
public override object? LoadField(object? context, DmdFieldInfo field, object? obj) => throw new NotImplementedException();
public override void StoreField(object? context, DmdFieldInfo field, object? obj, object? value) => throw new NotImplementedException();
}

TestRuntime CreateTestRuntime()
{
var rt = DmdRuntimeFactory.CreateRuntime(new DmdEvaluatorImpl(), IntPtr.Size == 4 ? DmdImageFileMachine.I386 : DmdImageFileMachine.AMD64);
var ad = rt.CreateAppDomain(1);
ad.CreateAssembly(typeof(void).Assembly.Location);
ad.CreateAssembly(GetTestAssemblyFilename());
return new Fake.TestRuntimeImpl(rt);
}*/

        //public static void Test() => new InterpreterTest().TestCore();

        /*void TestCore()
        {
            try
            {
                TestCore2();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.ToString());
                System.Diagnostics.Debugger.Break();
            }
        }

        TestRuntime testRuntime;
        DmdAssembly testAsm1;
        Assembly testAsm2;
        DmdType testType1;
        Type testType2;
        ILVM ilvm;*/
        /*
        void TestCore2()
        {
            testRuntime = CreateTestRuntime();
            var aas = testRuntime.Runtime.GetAppDomains().First().GetAssemblies().Select(a => a.FullName).ToList();
            testAsm1 = testRuntime.Runtime.GetAppDomains().First().GetAssemblies()[1]/*(();
            testAsm2 = Assembly.LoadFile(testAsm1.Location);
            testType1 = testAsm1.GetType("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass") ?? throw new InvalidOperationException();
            testType2 = testAsm2.GetType(testType1.FullName) ?? throw new InvalidOperationException();
            ilvm = ILVMFactory.Create();
            */
        public InterpreterTest() : base(AssemblyFilename)
        {
        }

        protected override object Test(string methodName, params object[] args)
        {
            methodName = "dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass." + methodName;
                    
            object res1 = null, res2 = null;
            Exception ex1 = null, ex2 = null;
			try
            {
                res1 = base.Test(methodName, args);
            }
			catch (Exception ex)
            {
				ex1 = ex;
				res1 = null;
			}

            try
            {
                res2 = Utils.GetMethodInfo(methodName).Invoke(null, args);
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
            /*if (!(ex1 is null) || !(ex2 is null))
				Verify(ex1?.GetType().FullName == ex2?.GetType().FullName);
			else
				Verify(m1.ReturnType, res1, res2);*/
            return null;
        }

        [Fact] public void Test_RET__Void() { Test("Test_RET__Void"); } /* TestMethod("Test_RET__Void"); */
        [Fact] public void Test_LDC_I4_M1() { Test("Test_LDC_I4_M1"); } /* TestMethod("Test_LDC_I4_M1"); */
        [Fact] public void Test_LDC_I4_0() { Test("Test_LDC_I4_0"); } /* TestMethod("Test_LDC_I4_0"); */
        [Fact] public void Test_LDC_I4_1() { Test("Test_LDC_I4_1"); } /* TestMethod("Test_LDC_I4_1"); */
        [Fact] public void Test_LDC_I4_2() { Test("Test_LDC_I4_2"); } /* TestMethod("Test_LDC_I4_2"); */
        [Fact] public void Test_LDC_I4_3() { Test("Test_LDC_I4_3"); } /* TestMethod("Test_LDC_I4_3"); */
        [Fact] public void Test_LDC_I4_4() { Test("Test_LDC_I4_4"); } /* TestMethod("Test_LDC_I4_4"); */
        [Fact] public void Test_LDC_I4_5() { Test("Test_LDC_I4_5"); } /* TestMethod("Test_LDC_I4_5"); */
        [Fact] public void Test_LDC_I4_6() { Test("Test_LDC_I4_6"); } /* TestMethod("Test_LDC_I4_6"); */
        [Fact] public void Test_LDC_I4_7() { Test("Test_LDC_I4_7"); } /* TestMethod("Test_LDC_I4_7"); */
        [Fact] public void Test_LDC_I4_8() { Test("Test_LDC_I4_8"); } /* TestMethod("Test_LDC_I4_8"); */
        [Fact] public void Test_LDC_I4_S__MinValue() { Test("Test_LDC_I4_S__MinValue"); } /* TestMethod("Test_LDC_I4_S__MinValue"); */
        [Fact] public void Test_LDC_I4_S__MaxValue() { Test("Test_LDC_I4_S__MaxValue"); } /* TestMethod("Test_LDC_I4_S__MaxValue"); */
        [Fact] public void Test_LDC_I4_S__M1() { Test("Test_LDC_I4_S__M1"); } /* TestMethod("Test_LDC_I4_S__M1"); */
        [Fact] public void Test_LDC_I4_S__0() { Test("Test_LDC_I4_S__0"); } /* TestMethod("Test_LDC_I4_S__0"); */
        [Fact] public void Test_LDC_I4_S__1() { Test("Test_LDC_I4_S__1"); } /* TestMethod("Test_LDC_I4_S__1"); */
        [Fact] public void Test_LDC_I4__MinValue() { Test("Test_LDC_I4__MinValue"); } /* TestMethod("Test_LDC_I4__MinValue"); */
        [Fact] public void Test_LDC_I4__MaxValue() { Test("Test_LDC_I4__MaxValue"); } /* TestMethod("Test_LDC_I4__MaxValue"); */
        [Fact] public void Test_LDC_I4__M1() { Test("Test_LDC_I4__M1"); } /* TestMethod("Test_LDC_I4__M1"); */
        [Fact] public void Test_LDC_I4__0() { Test("Test_LDC_I4__0"); } /* TestMethod("Test_LDC_I4__0"); */
        [Fact] public void Test_LDC_I4__1() { Test("Test_LDC_I4__1"); } /* TestMethod("Test_LDC_I4__1"); */
        [Fact] public void Test_LDC_I8__MinValue() { Test("Test_LDC_I8__MinValue"); } /* TestMethod("Test_LDC_I8__MinValue"); */
        [Fact] public void Test_LDC_I8__MaxValue() { Test("Test_LDC_I8__MaxValue"); } /* TestMethod("Test_LDC_I8__MaxValue"); */
        [Fact] public void Test_LDC_I8__M1() { Test("Test_LDC_I8__M1"); } /* TestMethod("Test_LDC_I8__M1"); */
        [Fact] public void Test_LDC_I8__0() { Test("Test_LDC_I8__0"); } /* TestMethod("Test_LDC_I8__0"); */
        [Fact] public void Test_LDC_I8__1() { Test("Test_LDC_I8__1"); } /* TestMethod("Test_LDC_I8__1"); */
        [Fact] public void Test_LDC_R4__NaN() { Test("Test_LDC_R4__NaN"); } /* TestMethod("Test_LDC_R4__NaN"); */
        [Fact] public void Test_LDC_R4__Epsilon() { Test("Test_LDC_R4__Epsilon"); } /* TestMethod("Test_LDC_R4__Epsilon"); */
        [Fact] public void Test_LDC_R4__MinValue() { Test("Test_LDC_R4__MinValue"); } /* TestMethod("Test_LDC_R4__MinValue"); */
        [Fact] public void Test_LDC_R4__MaxValue() { Test("Test_LDC_R4__MaxValue"); } /* TestMethod("Test_LDC_R4__MaxValue"); */
        [Fact] public void Test_LDC_R4__NegativeInfinity() { Test("Test_LDC_R4__NegativeInfinity"); } /* TestMethod("Test_LDC_R4__NegativeInfinity"); */
        [Fact] public void Test_LDC_R4__PositiveInfinity() { Test("Test_LDC_R4__PositiveInfinity"); } /* TestMethod("Test_LDC_R4__PositiveInfinity"); */
        [Fact] public void Test_LDC_R4__PositiveZero() { Test("Test_LDC_R4__PositiveZero"); } /* TestMethod("Test_LDC_R4__PositiveZero"); */
        [Fact] public void Test_LDC_R4__NegativeZero() { Test("Test_LDC_R4__NegativeZero"); } /* TestMethod("Test_LDC_R4__NegativeZero"); */
        [Fact] public void Test_LDC_R4__M1() { Test("Test_LDC_R4__M1"); } /* TestMethod("Test_LDC_R4__M1"); */
        [Fact] public void Test_LDC_R4__1() { Test("Test_LDC_R4__1"); } /* TestMethod("Test_LDC_R4__1"); */
        [Fact] public void Test_LDC_R8__NaN() { Test("Test_LDC_R8__NaN"); } /* TestMethod("Test_LDC_R8__NaN"); */
        [Fact] public void Test_LDC_R8__Epsilon() { Test("Test_LDC_R8__Epsilon"); } /* TestMethod("Test_LDC_R8__Epsilon"); */
        [Fact] public void Test_LDC_R8__MinValue() { Test("Test_LDC_R8__MinValue"); } /* TestMethod("Test_LDC_R8__MinValue"); */
        [Fact] public void Test_LDC_R8__MaxValue() { Test("Test_LDC_R8__MaxValue"); } /* TestMethod("Test_LDC_R8__MaxValue"); */
        [Fact] public void Test_LDC_R8__NegativeInfinity() { Test("Test_LDC_R8__NegativeInfinity"); } /* TestMethod("Test_LDC_R8__NegativeInfinity"); */
        [Fact] public void Test_LDC_R8__PositiveInfinity() { Test("Test_LDC_R8__PositiveInfinity"); } /* TestMethod("Test_LDC_R8__PositiveInfinity"); */
        [Fact] public void Test_LDC_R8__PositiveZero() { Test("Test_LDC_R8__PositiveZero"); } /* TestMethod("Test_LDC_R8__PositiveZero"); */
        [Fact] public void Test_LDC_R8__NegativeZero() { Test("Test_LDC_R8__NegativeZero"); } /* TestMethod("Test_LDC_R8__NegativeZero"); */
        [Fact] public void Test_LDC_R8__M1() { Test("Test_LDC_R8__M1"); } /* TestMethod("Test_LDC_R8__M1"); */
        [Fact] public void Test_LDC_R8__1() { Test("Test_LDC_R8__1"); } /* TestMethod("Test_LDC_R8__1"); */
        [Fact] public void Test_LDSTR() { Test("Test_LDSTR"); } /* TestMethod("Test_LDSTR"); */
        [Fact] public void Test_LDNULL() { Test("Test_LDNULL"); } /* TestMethod("Test_LDNULL"); */
        [Fact] public void Test_LDARG_0() { Test("Test_LDARG_0", 123); } /* TestMethod("Test_LDARG_0", 123); */
        [Fact] public void Test_LDARG_1() { Test("Test_LDARG_1", "", 123); } /* TestMethod("Test_LDARG_1", "", 123); */
        [Fact] public void Test_LDARG_2() { Test("Test_LDARG_2", "", "", 123); } /* TestMethod("Test_LDARG_2", "", "", 123); */
        [Fact] public void Test_LDARG_3() { Test("Test_LDARG_3", "", "", "", 123); } /* TestMethod("Test_LDARG_3", "", "", "", 123); */
        [Fact] public void Test_LDARG_S__0() { Test("Test_LDARG_S__0", 123); } /* TestMethod("Test_LDARG_S__0", 123); */
        [Fact] public void Test_LDARG_S__4() { Test("Test_LDARG_S__4", "", "", "", "", 123); } /* TestMethod("Test_LDARG_S__4", "", "", "", "", 123); */
        [Fact] public void Test_LDARG_S__128() { Test("Test_LDARG_S__128", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 123); } /* TestMethod("Test_LDARG_S__128", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 123); */
        [Fact] public void Test_LDARG__0() { Test("Test_LDARG__0", 123); } /* TestMethod("Test_LDARG__0", 123); */
        [Fact] public void Test_LDARG__4() { Test("Test_LDARG__4", "", "", "", "", 123); } /* TestMethod("Test_LDARG__4", "", "", "", "", 123); */
        [Fact] public void Test_LDARG__128() { Test("Test_LDARG__128", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 123); } /* TestMethod("Test_LDARG__128", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 123); */
        [Fact] public void Test_LDLOC_0() { Test("Test_LDLOC_0"); } /* TestMethod("Test_LDLOC_0"); */
        [Fact] public void Test_LDLOC_1() { Test("Test_LDLOC_1"); } /* TestMethod("Test_LDLOC_1"); */
        [Fact] public void Test_LDLOC_2() { Test("Test_LDLOC_2"); } /* TestMethod("Test_LDLOC_2"); */
        [Fact] public void Test_LDLOC_3() { Test("Test_LDLOC_3"); } /* TestMethod("Test_LDLOC_3"); */
        [Fact] public void Test_LDLOC_S__0() { Test("Test_LDLOC_S__0"); } /* TestMethod("Test_LDLOC_S__0"); */
        [Fact] public void Test_LDLOC_S__4() { Test("Test_LDLOC_S__4"); } /* TestMethod("Test_LDLOC_S__4"); */
        [Fact] public void Test_LDLOC_S__128() { Test("Test_LDLOC_S__128"); } /* TestMethod("Test_LDLOC_S__128"); */
        [Fact] public void Test_LDLOC__0() { Test("Test_LDLOC__0"); } /* TestMethod("Test_LDLOC__0"); */
        [Fact] public void Test_LDLOC__4() { Test("Test_LDLOC__4"); } /* TestMethod("Test_LDLOC__4"); */
        [Fact] public void Test_LDLOC__128() { Test("Test_LDLOC__128"); } /* TestMethod("Test_LDLOC__128"); */
        [Fact] public void Test_STARG_S__0() { Test("Test_STARG_S__0", 123, 456); } /* TestMethod("Test_STARG_S__0", 123, 456); */
        [Fact] public void Test_STARG_S__128() { Test("Test_STARG_S__128", 456, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 123); } /* TestMethod("Test_STARG_S__128", 456, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 123); */
        [Fact] public void Test_STARG__0() { Test("Test_STARG__0", 123, 456); } /* TestMethod("Test_STARG__0", 123, 456); */
        [Fact] public void Test_STARG__128() { Test("Test_STARG__128", 456, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 123); } /* TestMethod("Test_STARG__128", 456, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 123); */
        [Fact] public void Test_STLOC_0() { Test("Test_STLOC_0", 123); } /* TestMethod("Test_STLOC_0", 123); */
        [Fact] public void Test_STLOC_1() { Test("Test_STLOC_1", 123); } /* TestMethod("Test_STLOC_1", 123); */
        [Fact] public void Test_STLOC_2() { Test("Test_STLOC_2", 123); } /* TestMethod("Test_STLOC_2", 123); */
        [Fact] public void Test_STLOC_3() { Test("Test_STLOC_3", 123); } /* TestMethod("Test_STLOC_3", 123); */
        [Fact] public void Test_STLOC_S__0() { Test("Test_STLOC_S__0", 123); } /* TestMethod("Test_STLOC_S__0", 123); */
        [Fact] public void Test_STLOC_S__128() { Test("Test_STLOC_S__128", 123); } /* TestMethod("Test_STLOC_S__128", 123); */
        [Fact] public void Test_STLOC__0() { Test("Test_STLOC__0", 123); } /* TestMethod("Test_STLOC__0", 123); */
        [Fact] public void Test_STLOC__128() { Test("Test_STLOC__128", 123); } /* TestMethod("Test_STLOC__128", 123); */
        [Fact] public void Test_NOP() { Test("Test_NOP"); } /* TestMethod("Test_NOP"); */

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Test_CONV_I__Boolean(bool value) { Test("Test_CONV_I__Boolean", value); }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Test_CONV_I1__Boolean(bool value) { Test("Test_CONV_I1__Boolean", value); }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Test_CONV_I2__Boolean(bool value) { Test("Test_CONV_I2__Boolean", value); } 

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Test_CONV_I4__Boolean(bool value) { Test("Test_CONV_I4__Boolean", value); }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Test_CONV_I8__Boolean(bool value) { Test("Test_CONV_I8__Boolean", value); }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Test_CONV_U__Boolean(bool value) { Test("Test_CONV_U__Boolean", value); }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Test_CONV_U1__Boolean(bool value) { Test("Test_CONV_U1__Boolean", value); }

        //[Fact] public void Test_CONV_U2__Boolean(bool value) { Test("Test_CONV_U2__Boolean", value); } /* TestMethod("Test_CONV_U2__Boolean", false); */
        ////[Fact] public void Test_CONV_U2__Boolean(bool value) { Test("Test_CONV_U2__Boolean", true); } /* TestMethod("Test_CONV_U2__Boolean", true); */
        //[Fact] public void Test_CONV_U4__Boolean(bool value) { Test("Test_CONV_U4__Boolean", false); } /* TestMethod("Test_CONV_U4__Boolean", false); */
        ////[Fact] public void Test_CONV_U4__Boolean(bool value) { Test("Test_CONV_U4__Boolean", true); } /* TestMethod("Test_CONV_U4__Boolean", true); */
        //[Fact] public void Test_CONV_U8__Boolean(bool value) { Test("Test_CONV_U8__Boolean", false); } /* TestMethod("Test_CONV_U8__Boolean", false); */
        ////[Fact] public void Test_CONV_U8__Boolean(bool value) { Test("Test_CONV_U8__Boolean", true); } /* TestMethod("Test_CONV_U8__Boolean", true); */
        //[Fact] public void Test_CONV_R4__Boolean(bool value) { Test("Test_CONV_R4__Boolean", false); } /* TestMethod("Test_CONV_R4__Boolean", false); */
        ////[Fact] public void Test_CONV_R4__Boolean(bool value) { Test("Test_CONV_R4__Boolean", true); } /* TestMethod("Test_CONV_R4__Boolean", true); */
        //[Fact] public void Test_CONV_R8__Boolean(bool value) { Test("Test_CONV_R8__Boolean", false); } /* TestMethod("Test_CONV_R8__Boolean", false); */
        ////[Fact] public void Test_CONV_R8__Boolean(bool value) { Test("Test_CONV_R8__Boolean", true); } /* TestMethod("Test_CONV_R8__Boolean", true); */
        //[Fact] public void Test_CONV_OVF_I__Boolean(bool value) { Test("Test_CONV_OVF_I__Boolean", false); } /* TestMethod("Test_CONV_OVF_I__Boolean", false); */
        ////[Fact] public void Test_CONV_OVF_I__Boolean(bool value) { Test("Test_CONV_OVF_I__Boolean", true); } /* TestMethod("Test_CONV_OVF_I__Boolean", true); */
        //[Fact] public void Test_CONV_OVF_I1__Boolean(bool value) { Test("Test_CONV_OVF_I1__Boolean", false); } /* TestMethod("Test_CONV_OVF_I1__Boolean", false); */
        ////[Fact] public void Test_CONV_OVF_I1__Boolean(bool value) { Test("Test_CONV_OVF_I1__Boolean", true); } /* TestMethod("Test_CONV_OVF_I1__Boolean", true); */
        //[Fact] public void Test_CONV_OVF_I2__Boolean(bool value) { Test("Test_CONV_OVF_I2__Boolean", false); } /* TestMethod("Test_CONV_OVF_I2__Boolean", false); */
        ////[Fact] public void Test_CONV_OVF_I2__Boolean(bool value) { Test("Test_CONV_OVF_I2__Boolean", true); } /* TestMethod("Test_CONV_OVF_I2__Boolean", true); */
        //[Fact] public void Test_CONV_OVF_I4__Boolean(bool value) { Test("Test_CONV_OVF_I4__Boolean", false); } /* TestMethod("Test_CONV_OVF_I4__Boolean", false); */
        ////[Fact] public void Test_CONV_OVF_I4__Boolean(bool value) { Test("Test_CONV_OVF_I4__Boolean", true); } /* TestMethod("Test_CONV_OVF_I4__Boolean", true); */
        //[Fact] public void Test_CONV_OVF_I8__Boolean(bool value) { Test("Test_CONV_OVF_I8__Boolean", false); } /* TestMethod("Test_CONV_OVF_I8__Boolean", false); */
        ////[Fact] public void Test_CONV_OVF_I8__Boolean(bool value) { Test("Test_CONV_OVF_I8__Boolean", true); } /* TestMethod("Test_CONV_OVF_I8__Boolean", true); */
        //[Fact] public void Test_CONV_OVF_U__Boolean(bool value) { Test("Test_CONV_OVF_U__Boolean", false); } /* TestMethod("Test_CONV_OVF_U__Boolean", false); */
        ////[Fact] public void Test_CONV_OVF_U__Boolean(bool value) { Test("Test_CONV_OVF_U__Boolean", true); } /* TestMethod("Test_CONV_OVF_U__Boolean", true); */
        //[Fact] public void Test_CONV_OVF_U1__Boolean(bool value) { Test("Test_CONV_OVF_U1__Boolean", false); } /* TestMethod("Test_CONV_OVF_U1__Boolean", false); */
        ////[Fact] public void Test_CONV_OVF_U1__Boolean(bool value) { Test("Test_CONV_OVF_U1__Boolean", true); } /* TestMethod("Test_CONV_OVF_U1__Boolean", true); */
        //[Fact] public void Test_CONV_OVF_U2__Boolean(bool value) { Test("Test_CONV_OVF_U2__Boolean", false); } /* TestMethod("Test_CONV_OVF_U2__Boolean", false); */
        //[Fact] public void Test_CONV_OVF_U2__Boolean(bool value) { Test("Test_CONV_OVF_U2__Boolean", true); } /* TestMethod("Test_CONV_OVF_U2__Boolean", true); */
        //[Fact] public void Test_CONV_OVF_U4__Boolean(bool value) { Test("Test_CONV_OVF_U4__Boolean", false); } /* TestMethod("Test_CONV_OVF_U4__Boolean", false); */
        //[Fact] public void Test_CONV_OVF_U4__Boolean(bool value) { Test("Test_CONV_OVF_U4__Boolean", true); } /* TestMethod("Test_CONV_OVF_U4__Boolean", true); */
        //[Fact] public void Test_CONV_OVF_U8__Boolean(bool value) { Test("Test_CONV_OVF_U8__Boolean", false); } /* TestMethod("Test_CONV_OVF_U8__Boolean", false); */
        //[Fact] public void Test_CONV_OVF_U8__Boolean(bool value) { Test("Test_CONV_OVF_U8__Boolean", true); } /* TestMethod("Test_CONV_OVF_U8__Boolean", true); */
        //[Fact] public void Test_CONV_OVF_I_UN__Boolean(bool value) { Test("Test_CONV_OVF_I_UN__Boolean", false); } /* TestMethod("Test_CONV_OVF_I_UN__Boolean", false); */
        //[Fact] public void Test_CONV_OVF_I_UN__Boolean(bool value) { Test("Test_CONV_OVF_I_UN__Boolean", true); } /* TestMethod("Test_CONV_OVF_I_UN__Boolean", true); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Boolean(bool value) { Test("Test_CONV_OVF_I1_UN__Boolean", false); } /* TestMethod("Test_CONV_OVF_I1_UN__Boolean", false); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Boolean(bool value) { Test("Test_CONV_OVF_I1_UN__Boolean", true); } /* TestMethod("Test_CONV_OVF_I1_UN__Boolean", true); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Boolean(bool value) { Test("Test_CONV_OVF_I2_UN__Boolean", false); } /* TestMethod("Test_CONV_OVF_I2_UN__Boolean", false); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Boolean(bool value) { Test("Test_CONV_OVF_I2_UN__Boolean", true); } /* TestMethod("Test_CONV_OVF_I2_UN__Boolean", true); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Boolean(bool value) { Test("Test_CONV_OVF_I4_UN__Boolean", false); } /* TestMethod("Test_CONV_OVF_I4_UN__Boolean", false); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Boolean(bool value) { Test("Test_CONV_OVF_I4_UN__Boolean", true); } /* TestMethod("Test_CONV_OVF_I4_UN__Boolean", true); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Boolean(bool value) { Test("Test_CONV_OVF_I8_UN__Boolean", false); } /* TestMethod("Test_CONV_OVF_I8_UN__Boolean", false); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Boolean(bool value) { Test("Test_CONV_OVF_I8_UN__Boolean", true); } /* TestMethod("Test_CONV_OVF_I8_UN__Boolean", true); */
        //[Fact] public void Test_CONV_OVF_U_UN__Boolean(bool value) { Test("Test_CONV_OVF_U_UN__Boolean", false); } /* TestMethod("Test_CONV_OVF_U_UN__Boolean", false); */
        //[Fact] public void Test_CONV_OVF_U_UN__Boolean(bool value) { Test("Test_CONV_OVF_U_UN__Boolean", true); } /* TestMethod("Test_CONV_OVF_U_UN__Boolean", true); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Boolean(bool value) { Test("Test_CONV_OVF_U1_UN__Boolean", false); } /* TestMethod("Test_CONV_OVF_U1_UN__Boolean", false); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Boolean(bool value) { Test("Test_CONV_OVF_U1_UN__Boolean", true); } /* TestMethod("Test_CONV_OVF_U1_UN__Boolean", true); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Boolean(bool value) { Test("Test_CONV_OVF_U2_UN__Boolean", false); } /* TestMethod("Test_CONV_OVF_U2_UN__Boolean", false); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Boolean(bool value) { Test("Test_CONV_OVF_U2_UN__Boolean", true); } /* TestMethod("Test_CONV_OVF_U2_UN__Boolean", true); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Boolean(bool value) { Test("Test_CONV_OVF_U4_UN__Boolean", false); } /* TestMethod("Test_CONV_OVF_U4_UN__Boolean", false); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Boolean(bool value) { Test("Test_CONV_OVF_U4_UN__Boolean", true); } /* TestMethod("Test_CONV_OVF_U4_UN__Boolean", true); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Boolean(bool value) { Test("Test_CONV_OVF_U8_UN__Boolean", false); } /* TestMethod("Test_CONV_OVF_U8_UN__Boolean", false); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Boolean(bool value) { Test("Test_CONV_OVF_U8_UN__Boolean", true); } /* TestMethod("Test_CONV_OVF_U8_UN__Boolean", true); */
        //[Fact] public void Test_CONV_R_UN__Boolean(bool value) { Test("Test_CONV_R_UN__Boolean", false); } /* TestMethod("Test_CONV_R_UN__Boolean", false); */
        //[Fact] public void Test_CONV_R_UN__Boolean(bool value) { Test("Test_CONV_R_UN__Boolean", true); } /* TestMethod("Test_CONV_R_UN__Boolean", true); */
        //[Fact] public void Test_CONV_I__Char() { Test("Test_CONV_I__Char", char.MinValue); } /* TestMethod("Test_CONV_I__Char", char.MinValue); */
        //[Fact] public void Test_CONV_I__Char() { Test("Test_CONV_I__Char", char.MaxValue); } /* TestMethod("Test_CONV_I__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_I__Char() { Test("Test_CONV_I__Char", (char)0x1234); } /* TestMethod("Test_CONV_I__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_I__Char() { Test("Test_CONV_I__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_I__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_I1__Char() { Test("Test_CONV_I1__Char", char.MinValue); } /* TestMethod("Test_CONV_I1__Char", char.MinValue); */
        //[Fact] public void Test_CONV_I1__Char() { Test("Test_CONV_I1__Char", char.MaxValue); } /* TestMethod("Test_CONV_I1__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_I1__Char() { Test("Test_CONV_I1__Char", (char)0x1234); } /* TestMethod("Test_CONV_I1__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_I1__Char() { Test("Test_CONV_I1__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_I1__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_I2__Char() { Test("Test_CONV_I2__Char", char.MinValue); } /* TestMethod("Test_CONV_I2__Char", char.MinValue); */
        //[Fact] public void Test_CONV_I2__Char() { Test("Test_CONV_I2__Char", char.MaxValue); } /* TestMethod("Test_CONV_I2__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_I2__Char() { Test("Test_CONV_I2__Char", (char)0x1234); } /* TestMethod("Test_CONV_I2__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_I2__Char() { Test("Test_CONV_I2__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_I2__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_I4__Char() { Test("Test_CONV_I4__Char", char.MinValue); } /* TestMethod("Test_CONV_I4__Char", char.MinValue); */
        //[Fact] public void Test_CONV_I4__Char() { Test("Test_CONV_I4__Char", char.MaxValue); } /* TestMethod("Test_CONV_I4__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_I4__Char() { Test("Test_CONV_I4__Char", (char)0x1234); } /* TestMethod("Test_CONV_I4__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_I4__Char() { Test("Test_CONV_I4__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_I4__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_I8__Char() { Test("Test_CONV_I8__Char", char.MinValue); } /* TestMethod("Test_CONV_I8__Char", char.MinValue); */
        //[Fact] public void Test_CONV_I8__Char() { Test("Test_CONV_I8__Char", char.MaxValue); } /* TestMethod("Test_CONV_I8__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_I8__Char() { Test("Test_CONV_I8__Char", (char)0x1234); } /* TestMethod("Test_CONV_I8__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_I8__Char() { Test("Test_CONV_I8__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_I8__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_U__Char() { Test("Test_CONV_U__Char", char.MinValue); } /* TestMethod("Test_CONV_U__Char", char.MinValue); */
        //[Fact] public void Test_CONV_U__Char() { Test("Test_CONV_U__Char", char.MaxValue); } /* TestMethod("Test_CONV_U__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_U__Char() { Test("Test_CONV_U__Char", (char)0x1234); } /* TestMethod("Test_CONV_U__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_U__Char() { Test("Test_CONV_U__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_U__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_U1__Char() { Test("Test_CONV_U1__Char", char.MinValue); } /* TestMethod("Test_CONV_U1__Char", char.MinValue); */
        //[Fact] public void Test_CONV_U1__Char() { Test("Test_CONV_U1__Char", char.MaxValue); } /* TestMethod("Test_CONV_U1__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_U1__Char() { Test("Test_CONV_U1__Char", (char)0x1234); } /* TestMethod("Test_CONV_U1__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_U1__Char() { Test("Test_CONV_U1__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_U1__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_U2__Char() { Test("Test_CONV_U2__Char", char.MinValue); } /* TestMethod("Test_CONV_U2__Char", char.MinValue); */
        //[Fact] public void Test_CONV_U2__Char() { Test("Test_CONV_U2__Char", char.MaxValue); } /* TestMethod("Test_CONV_U2__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_U2__Char() { Test("Test_CONV_U2__Char", (char)0x1234); } /* TestMethod("Test_CONV_U2__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_U2__Char() { Test("Test_CONV_U2__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_U2__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_U4__Char() { Test("Test_CONV_U4__Char", char.MinValue); } /* TestMethod("Test_CONV_U4__Char", char.MinValue); */
        //[Fact] public void Test_CONV_U4__Char() { Test("Test_CONV_U4__Char", char.MaxValue); } /* TestMethod("Test_CONV_U4__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_U4__Char() { Test("Test_CONV_U4__Char", (char)0x1234); } /* TestMethod("Test_CONV_U4__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_U4__Char() { Test("Test_CONV_U4__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_U4__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_U8__Char() { Test("Test_CONV_U8__Char", char.MinValue); } /* TestMethod("Test_CONV_U8__Char", char.MinValue); */
        //[Fact] public void Test_CONV_U8__Char() { Test("Test_CONV_U8__Char", char.MaxValue); } /* TestMethod("Test_CONV_U8__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_U8__Char() { Test("Test_CONV_U8__Char", (char)0x1234); } /* TestMethod("Test_CONV_U8__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_U8__Char() { Test("Test_CONV_U8__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_U8__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_R4__Char() { Test("Test_CONV_R4__Char", char.MinValue); } /* TestMethod("Test_CONV_R4__Char", char.MinValue); */
        //[Fact] public void Test_CONV_R4__Char() { Test("Test_CONV_R4__Char", char.MaxValue); } /* TestMethod("Test_CONV_R4__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_R4__Char() { Test("Test_CONV_R4__Char", (char)0x1234); } /* TestMethod("Test_CONV_R4__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_R4__Char() { Test("Test_CONV_R4__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_R4__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_R8__Char() { Test("Test_CONV_R8__Char", char.MinValue); } /* TestMethod("Test_CONV_R8__Char", char.MinValue); */
        //[Fact] public void Test_CONV_R8__Char() { Test("Test_CONV_R8__Char", char.MaxValue); } /* TestMethod("Test_CONV_R8__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_R8__Char() { Test("Test_CONV_R8__Char", (char)0x1234); } /* TestMethod("Test_CONV_R8__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_R8__Char() { Test("Test_CONV_R8__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_R8__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_I__Char() { Test("Test_CONV_OVF_I__Char", char.MinValue); } /* TestMethod("Test_CONV_OVF_I__Char", char.MinValue); */
        //[Fact] public void Test_CONV_OVF_I__Char() { Test("Test_CONV_OVF_I__Char", char.MaxValue); } /* TestMethod("Test_CONV_OVF_I__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I__Char() { Test("Test_CONV_OVF_I__Char", (char)0x1234); } /* TestMethod("Test_CONV_OVF_I__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_OVF_I__Char() { Test("Test_CONV_OVF_I__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_OVF_I__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_I1__Char() { Test("Test_CONV_OVF_I1__Char", char.MinValue); } /* TestMethod("Test_CONV_OVF_I1__Char", char.MinValue); */
        //[Fact] public void Test_CONV_OVF_I1__Char() { Test("Test_CONV_OVF_I1__Char", char.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I1__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I1__Char() { Test("Test_CONV_OVF_I1__Char", (char)0x1234); } /* TestMethodEX("Test_CONV_OVF_I1__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_OVF_I1__Char() { Test("Test_CONV_OVF_I1__Char", (char)0x9ABC); } /* TestMethodEX("Test_CONV_OVF_I1__Char", (char)0x9ABC); */

        [Theory]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        public void Test_CONV_OVF_I2__Char(char value) { Test("Test_CONV_OVF_I2__Char", value); }

        [Theory]
        [InlineData((char)0x1234)]
        [InlineData((char)0x9ABC)]
        [InlineData(char.MinValue)]
        [InlineData(char.MaxValue)]
        public void Test_CONV_OVF_I4__Char(char value) { Test("Test_CONV_OVF_I4__Char", value); } 

        //[Fact] public void Test_CONV_OVF_I8__Char() { Test("Test_CONV_OVF_I8__Char", char.MinValue); } /* TestMethod("Test_CONV_OVF_I8__Char", char.MinValue); */
        //[Fact] public void Test_CONV_OVF_I8__Char() { Test("Test_CONV_OVF_I8__Char", char.MaxValue); } /* TestMethod("Test_CONV_OVF_I8__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I8__Char() { Test("Test_CONV_OVF_I8__Char", (char)0x1234); } /* TestMethod("Test_CONV_OVF_I8__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_OVF_I8__Char() { Test("Test_CONV_OVF_I8__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_OVF_I8__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_U__Char() { Test("Test_CONV_OVF_U__Char", char.MinValue); } /* TestMethod("Test_CONV_OVF_U__Char", char.MinValue); */
        //[Fact] public void Test_CONV_OVF_U__Char() { Test("Test_CONV_OVF_U__Char", char.MaxValue); } /* TestMethod("Test_CONV_OVF_U__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U__Char() { Test("Test_CONV_OVF_U__Char", (char)0x1234); } /* TestMethod("Test_CONV_OVF_U__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_OVF_U__Char() { Test("Test_CONV_OVF_U__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_OVF_U__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_U1__Char() { Test("Test_CONV_OVF_U1__Char", char.MinValue); } /* TestMethod("Test_CONV_OVF_U1__Char", char.MinValue); */
        //[Fact] public void Test_CONV_OVF_U1__Char() { Test("Test_CONV_OVF_U1__Char", char.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U1__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U1__Char() { Test("Test_CONV_OVF_U1__Char", (char)0x1234); } /* TestMethodEX("Test_CONV_OVF_U1__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_OVF_U1__Char() { Test("Test_CONV_OVF_U1__Char", (char)0x9ABC); } /* TestMethodEX("Test_CONV_OVF_U1__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_U2__Char() { Test("Test_CONV_OVF_U2__Char", char.MinValue); } /* TestMethod("Test_CONV_OVF_U2__Char", char.MinValue); */
        //[Fact] public void Test_CONV_OVF_U2__Char() { Test("Test_CONV_OVF_U2__Char", char.MaxValue); } /* TestMethod("Test_CONV_OVF_U2__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U2__Char() { Test("Test_CONV_OVF_U2__Char", (char)0x1234); } /* TestMethod("Test_CONV_OVF_U2__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_OVF_U2__Char() { Test("Test_CONV_OVF_U2__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_OVF_U2__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_U4__Char() { Test("Test_CONV_OVF_U4__Char", char.MinValue); } /* TestMethod("Test_CONV_OVF_U4__Char", char.MinValue); */
        //[Fact] public void Test_CONV_OVF_U4__Char() { Test("Test_CONV_OVF_U4__Char", char.MaxValue); } /* TestMethod("Test_CONV_OVF_U4__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U4__Char() { Test("Test_CONV_OVF_U4__Char", (char)0x1234); } /* TestMethod("Test_CONV_OVF_U4__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_OVF_U4__Char() { Test("Test_CONV_OVF_U4__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_OVF_U4__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_U8__Char() { Test("Test_CONV_OVF_U8__Char", char.MinValue); } /* TestMethod("Test_CONV_OVF_U8__Char", char.MinValue); */
        //[Fact] public void Test_CONV_OVF_U8__Char() { Test("Test_CONV_OVF_U8__Char", char.MaxValue); } /* TestMethod("Test_CONV_OVF_U8__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U8__Char() { Test("Test_CONV_OVF_U8__Char", (char)0x1234); } /* TestMethod("Test_CONV_OVF_U8__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_OVF_U8__Char() { Test("Test_CONV_OVF_U8__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_OVF_U8__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_I_UN__Char() { Test("Test_CONV_OVF_I_UN__Char", char.MinValue); } /* TestMethod("Test_CONV_OVF_I_UN__Char", char.MinValue); */
        //[Fact] public void Test_CONV_OVF_I_UN__Char() { Test("Test_CONV_OVF_I_UN__Char", char.MaxValue); } /* TestMethod("Test_CONV_OVF_I_UN__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I_UN__Char() { Test("Test_CONV_OVF_I_UN__Char", (char)0x1234); } /* TestMethod("Test_CONV_OVF_I_UN__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_OVF_I_UN__Char() { Test("Test_CONV_OVF_I_UN__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_OVF_I_UN__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Char() { Test("Test_CONV_OVF_I1_UN__Char", char.MinValue); } /* TestMethod("Test_CONV_OVF_I1_UN__Char", char.MinValue); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Char() { Test("Test_CONV_OVF_I1_UN__Char", char.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I1_UN__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Char() { Test("Test_CONV_OVF_I1_UN__Char", (char)0x1234); } /* TestMethodEX("Test_CONV_OVF_I1_UN__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Char() { Test("Test_CONV_OVF_I1_UN__Char", (char)0x9ABC); } /* TestMethodEX("Test_CONV_OVF_I1_UN__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Char() { Test("Test_CONV_OVF_I2_UN__Char", char.MinValue); } /* TestMethod("Test_CONV_OVF_I2_UN__Char", char.MinValue); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Char() { Test("Test_CONV_OVF_I2_UN__Char", char.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I2_UN__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Char() { Test("Test_CONV_OVF_I2_UN__Char", (char)0x1234); } /* TestMethod("Test_CONV_OVF_I2_UN__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Char() { Test("Test_CONV_OVF_I2_UN__Char", (char)0x9ABC); } /* TestMethodEX("Test_CONV_OVF_I2_UN__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Char() { Test("Test_CONV_OVF_I4_UN__Char", char.MinValue); } /* TestMethod("Test_CONV_OVF_I4_UN__Char", char.MinValue); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Char() { Test("Test_CONV_OVF_I4_UN__Char", char.MaxValue); } /* TestMethod("Test_CONV_OVF_I4_UN__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Char() { Test("Test_CONV_OVF_I4_UN__Char", (char)0x1234); } /* TestMethod("Test_CONV_OVF_I4_UN__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Char() { Test("Test_CONV_OVF_I4_UN__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_OVF_I4_UN__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Char() { Test("Test_CONV_OVF_I8_UN__Char", char.MinValue); } /* TestMethod("Test_CONV_OVF_I8_UN__Char", char.MinValue); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Char() { Test("Test_CONV_OVF_I8_UN__Char", char.MaxValue); } /* TestMethod("Test_CONV_OVF_I8_UN__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Char() { Test("Test_CONV_OVF_I8_UN__Char", (char)0x1234); } /* TestMethod("Test_CONV_OVF_I8_UN__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Char() { Test("Test_CONV_OVF_I8_UN__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_OVF_I8_UN__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_U_UN__Char() { Test("Test_CONV_OVF_U_UN__Char", char.MinValue); } /* TestMethod("Test_CONV_OVF_U_UN__Char", char.MinValue); */
        //[Fact] public void Test_CONV_OVF_U_UN__Char() { Test("Test_CONV_OVF_U_UN__Char", char.MaxValue); } /* TestMethod("Test_CONV_OVF_U_UN__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U_UN__Char() { Test("Test_CONV_OVF_U_UN__Char", (char)0x1234); } /* TestMethod("Test_CONV_OVF_U_UN__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_OVF_U_UN__Char() { Test("Test_CONV_OVF_U_UN__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_OVF_U_UN__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Char() { Test("Test_CONV_OVF_U1_UN__Char", char.MinValue); } /* TestMethod("Test_CONV_OVF_U1_UN__Char", char.MinValue); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Char() { Test("Test_CONV_OVF_U1_UN__Char", char.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U1_UN__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Char() { Test("Test_CONV_OVF_U1_UN__Char", (char)0x1234); } /* TestMethodEX("Test_CONV_OVF_U1_UN__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Char() { Test("Test_CONV_OVF_U1_UN__Char", (char)0x9ABC); } /* TestMethodEX("Test_CONV_OVF_U1_UN__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Char() { Test("Test_CONV_OVF_U2_UN__Char", char.MinValue); } /* TestMethod("Test_CONV_OVF_U2_UN__Char", char.MinValue); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Char() { Test("Test_CONV_OVF_U2_UN__Char", char.MaxValue); } /* TestMethod("Test_CONV_OVF_U2_UN__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Char() { Test("Test_CONV_OVF_U2_UN__Char", (char)0x1234); } /* TestMethod("Test_CONV_OVF_U2_UN__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Char() { Test("Test_CONV_OVF_U2_UN__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_OVF_U2_UN__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Char() { Test("Test_CONV_OVF_U4_UN__Char", char.MinValue); } /* TestMethod("Test_CONV_OVF_U4_UN__Char", char.MinValue); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Char() { Test("Test_CONV_OVF_U4_UN__Char", char.MaxValue); } /* TestMethod("Test_CONV_OVF_U4_UN__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Char() { Test("Test_CONV_OVF_U4_UN__Char", (char)0x1234); } /* TestMethod("Test_CONV_OVF_U4_UN__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Char() { Test("Test_CONV_OVF_U4_UN__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_OVF_U4_UN__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Char() { Test("Test_CONV_OVF_U8_UN__Char", char.MinValue); } /* TestMethod("Test_CONV_OVF_U8_UN__Char", char.MinValue); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Char() { Test("Test_CONV_OVF_U8_UN__Char", char.MaxValue); } /* TestMethod("Test_CONV_OVF_U8_UN__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Char() { Test("Test_CONV_OVF_U8_UN__Char", (char)0x1234); } /* TestMethod("Test_CONV_OVF_U8_UN__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Char() { Test("Test_CONV_OVF_U8_UN__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_OVF_U8_UN__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_R_UN__Char() { Test("Test_CONV_R_UN__Char", char.MinValue); } /* TestMethod("Test_CONV_R_UN__Char", char.MinValue); */
        //[Fact] public void Test_CONV_R_UN__Char() { Test("Test_CONV_R_UN__Char", char.MaxValue); } /* TestMethod("Test_CONV_R_UN__Char", char.MaxValue); */
        //[Fact] public void Test_CONV_R_UN__Char() { Test("Test_CONV_R_UN__Char", (char)0x1234); } /* TestMethod("Test_CONV_R_UN__Char", (char)0x1234); */
        //[Fact] public void Test_CONV_R_UN__Char() { Test("Test_CONV_R_UN__Char", (char)0x9ABC); } /* TestMethod("Test_CONV_R_UN__Char", (char)0x9ABC); */
        //[Fact] public void Test_CONV_I__IntPtr() { Test("Test_CONV_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_CONV_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_I__IntPtr() { Test("Test_CONV_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_CONV_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_I__IntPtr() { Test("Test_CONV_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_I__IntPtr() { Test("Test_CONV_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethod("Test_CONV_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_I__IntPtr() { Test("Test_CONV_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_I__IntPtr() { Test("Test_CONV_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_I__IntPtr() { Test("Test_CONV_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_I1__IntPtr() { Test("Test_CONV_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_CONV_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_I1__IntPtr() { Test("Test_CONV_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_CONV_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_I1__IntPtr() { Test("Test_CONV_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_I1__IntPtr() { Test("Test_CONV_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethod("Test_CONV_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_I1__IntPtr() { Test("Test_CONV_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_I1__IntPtr() { Test("Test_CONV_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_I1__IntPtr() { Test("Test_CONV_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_I2__IntPtr() { Test("Test_CONV_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_CONV_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_I2__IntPtr() { Test("Test_CONV_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_CONV_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_I2__IntPtr() { Test("Test_CONV_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_I2__IntPtr() { Test("Test_CONV_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethod("Test_CONV_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_I2__IntPtr() { Test("Test_CONV_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_I2__IntPtr() { Test("Test_CONV_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_I2__IntPtr() { Test("Test_CONV_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_I4__IntPtr() { Test("Test_CONV_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_CONV_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_I4__IntPtr() { Test("Test_CONV_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_CONV_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_I4__IntPtr() { Test("Test_CONV_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_I4__IntPtr() { Test("Test_CONV_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethod("Test_CONV_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_I4__IntPtr() { Test("Test_CONV_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_I4__IntPtr() { Test("Test_CONV_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_I4__IntPtr() { Test("Test_CONV_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_I8__IntPtr() { Test("Test_CONV_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_CONV_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_I8__IntPtr() { Test("Test_CONV_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_CONV_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_I8__IntPtr() { Test("Test_CONV_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_I8__IntPtr() { Test("Test_CONV_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethod("Test_CONV_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_I8__IntPtr() { Test("Test_CONV_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_I8__IntPtr() { Test("Test_CONV_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_I8__IntPtr() { Test("Test_CONV_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_U__IntPtr() { Test("Test_CONV_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_CONV_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_U__IntPtr() { Test("Test_CONV_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_CONV_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_U__IntPtr() { Test("Test_CONV_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_U__IntPtr() { Test("Test_CONV_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethod("Test_CONV_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_U__IntPtr() { Test("Test_CONV_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_U__IntPtr() { Test("Test_CONV_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_U__IntPtr() { Test("Test_CONV_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_U1__IntPtr() { Test("Test_CONV_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_CONV_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_U1__IntPtr() { Test("Test_CONV_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_CONV_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_U1__IntPtr() { Test("Test_CONV_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_U1__IntPtr() { Test("Test_CONV_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethod("Test_CONV_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_U1__IntPtr() { Test("Test_CONV_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_U1__IntPtr() { Test("Test_CONV_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_U1__IntPtr() { Test("Test_CONV_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_U2__IntPtr() { Test("Test_CONV_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_CONV_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_U2__IntPtr() { Test("Test_CONV_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_CONV_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_U2__IntPtr() { Test("Test_CONV_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_U2__IntPtr() { Test("Test_CONV_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethod("Test_CONV_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_U2__IntPtr() { Test("Test_CONV_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_U2__IntPtr() { Test("Test_CONV_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_U2__IntPtr() { Test("Test_CONV_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_U4__IntPtr() { Test("Test_CONV_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_CONV_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_U4__IntPtr() { Test("Test_CONV_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_CONV_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_U4__IntPtr() { Test("Test_CONV_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_U4__IntPtr() { Test("Test_CONV_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethod("Test_CONV_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_U4__IntPtr() { Test("Test_CONV_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_U4__IntPtr() { Test("Test_CONV_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_U4__IntPtr() { Test("Test_CONV_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_U8__IntPtr() { Test("Test_CONV_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_CONV_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_U8__IntPtr() { Test("Test_CONV_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_CONV_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_U8__IntPtr() { Test("Test_CONV_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_U8__IntPtr() { Test("Test_CONV_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethod("Test_CONV_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_U8__IntPtr() { Test("Test_CONV_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_U8__IntPtr() { Test("Test_CONV_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_U8__IntPtr() { Test("Test_CONV_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_R4__IntPtr() { Test("Test_CONV_R4__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_CONV_R4__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_R4__IntPtr() { Test("Test_CONV_R4__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_CONV_R4__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_R4__IntPtr() { Test("Test_CONV_R4__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_R4__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_R4__IntPtr() { Test("Test_CONV_R4__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethod("Test_CONV_R4__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_R4__IntPtr() { Test("Test_CONV_R4__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_R4__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_R4__IntPtr() { Test("Test_CONV_R4__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_R4__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_R4__IntPtr() { Test("Test_CONV_R4__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_R4__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_R8__IntPtr() { Test("Test_CONV_R8__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_CONV_R8__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_R8__IntPtr() { Test("Test_CONV_R8__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_CONV_R8__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_R8__IntPtr() { Test("Test_CONV_R8__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_R8__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_R8__IntPtr() { Test("Test_CONV_R8__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethod("Test_CONV_R8__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_R8__IntPtr() { Test("Test_CONV_R8__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_R8__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_R8__IntPtr() { Test("Test_CONV_R8__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_R8__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_R8__IntPtr() { Test("Test_CONV_R8__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_R8__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_I__IntPtr() { Test("Test_CONV_OVF_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_CONV_OVF_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_OVF_I__IntPtr() { Test("Test_CONV_OVF_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_CONV_OVF_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_I__IntPtr() { Test("Test_CONV_OVF_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_OVF_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_OVF_I__IntPtr() { Test("Test_CONV_OVF_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethod("Test_CONV_OVF_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_OVF_I__IntPtr() { Test("Test_CONV_OVF_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_OVF_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_OVF_I__IntPtr() { Test("Test_CONV_OVF_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_OVF_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_I__IntPtr() { Test("Test_CONV_OVF_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_OVF_I__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_I1__IntPtr() { Test("Test_CONV_OVF_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethodEX("Test_CONV_OVF_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_OVF_I1__IntPtr() { Test("Test_CONV_OVF_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_I1__IntPtr() { Test("Test_CONV_OVF_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_OVF_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_OVF_I1__IntPtr() { Test("Test_CONV_OVF_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethod("Test_CONV_OVF_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_OVF_I1__IntPtr() { Test("Test_CONV_OVF_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_OVF_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_OVF_I1__IntPtr() { Test("Test_CONV_OVF_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_I1__IntPtr() { Test("Test_CONV_OVF_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_I1__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_I2__IntPtr() { Test("Test_CONV_OVF_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethodEX("Test_CONV_OVF_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_OVF_I2__IntPtr() { Test("Test_CONV_OVF_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_I2__IntPtr() { Test("Test_CONV_OVF_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_OVF_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_OVF_I2__IntPtr() { Test("Test_CONV_OVF_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethod("Test_CONV_OVF_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_OVF_I2__IntPtr() { Test("Test_CONV_OVF_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_OVF_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_OVF_I2__IntPtr() { Test("Test_CONV_OVF_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_I2__IntPtr() { Test("Test_CONV_OVF_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_I2__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_I4__IntPtr() { Test("Test_CONV_OVF_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethodEX("Test_CONV_OVF_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_OVF_I4__IntPtr() { Test("Test_CONV_OVF_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_I4__IntPtr() { Test("Test_CONV_OVF_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_OVF_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_OVF_I4__IntPtr() { Test("Test_CONV_OVF_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethod("Test_CONV_OVF_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_OVF_I4__IntPtr() { Test("Test_CONV_OVF_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_OVF_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_OVF_I4__IntPtr() { Test("Test_CONV_OVF_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_I4__IntPtr() { Test("Test_CONV_OVF_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_I4__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_I8__IntPtr() { Test("Test_CONV_OVF_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_CONV_OVF_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_OVF_I8__IntPtr() { Test("Test_CONV_OVF_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_CONV_OVF_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_I8__IntPtr() { Test("Test_CONV_OVF_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_OVF_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_OVF_I8__IntPtr() { Test("Test_CONV_OVF_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethod("Test_CONV_OVF_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_OVF_I8__IntPtr() { Test("Test_CONV_OVF_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_OVF_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_OVF_I8__IntPtr() { Test("Test_CONV_OVF_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_OVF_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_I8__IntPtr() { Test("Test_CONV_OVF_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_OVF_I8__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_U__IntPtr() { Test("Test_CONV_OVF_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethodEX("Test_CONV_OVF_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_OVF_U__IntPtr() { Test("Test_CONV_OVF_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_CONV_OVF_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_U__IntPtr() { Test("Test_CONV_OVF_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_OVF_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_OVF_U__IntPtr() { Test("Test_CONV_OVF_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethodEX("Test_CONV_OVF_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_OVF_U__IntPtr() { Test("Test_CONV_OVF_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_OVF_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_OVF_U__IntPtr() { Test("Test_CONV_OVF_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_OVF_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_U__IntPtr() { Test("Test_CONV_OVF_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_U__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_U1__IntPtr() { Test("Test_CONV_OVF_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethodEX("Test_CONV_OVF_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_OVF_U1__IntPtr() { Test("Test_CONV_OVF_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_U1__IntPtr() { Test("Test_CONV_OVF_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_OVF_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_OVF_U1__IntPtr() { Test("Test_CONV_OVF_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethodEX("Test_CONV_OVF_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_OVF_U1__IntPtr() { Test("Test_CONV_OVF_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_OVF_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_OVF_U1__IntPtr() { Test("Test_CONV_OVF_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_U1__IntPtr() { Test("Test_CONV_OVF_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_U1__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_U2__IntPtr() { Test("Test_CONV_OVF_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethodEX("Test_CONV_OVF_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_OVF_U2__IntPtr() { Test("Test_CONV_OVF_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_U2__IntPtr() { Test("Test_CONV_OVF_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_OVF_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_OVF_U2__IntPtr() { Test("Test_CONV_OVF_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethodEX("Test_CONV_OVF_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_OVF_U2__IntPtr() { Test("Test_CONV_OVF_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_OVF_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_OVF_U2__IntPtr() { Test("Test_CONV_OVF_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_U2__IntPtr() { Test("Test_CONV_OVF_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_U2__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_U4__IntPtr() { Test("Test_CONV_OVF_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethodEX("Test_CONV_OVF_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_OVF_U4__IntPtr() { Test("Test_CONV_OVF_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_U4__IntPtr() { Test("Test_CONV_OVF_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_OVF_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_OVF_U4__IntPtr() { Test("Test_CONV_OVF_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethodEX("Test_CONV_OVF_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_OVF_U4__IntPtr() { Test("Test_CONV_OVF_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_OVF_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_OVF_U4__IntPtr() { Test("Test_CONV_OVF_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_U4__IntPtr() { Test("Test_CONV_OVF_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_U4__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_U8__IntPtr() { Test("Test_CONV_OVF_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethodEX("Test_CONV_OVF_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_OVF_U8__IntPtr() { Test("Test_CONV_OVF_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_CONV_OVF_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_U8__IntPtr() { Test("Test_CONV_OVF_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_OVF_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_OVF_U8__IntPtr() { Test("Test_CONV_OVF_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethodEX("Test_CONV_OVF_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_OVF_U8__IntPtr() { Test("Test_CONV_OVF_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_OVF_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_OVF_U8__IntPtr() { Test("Test_CONV_OVF_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_OVF_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_U8__IntPtr() { Test("Test_CONV_OVF_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_U8__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_I_UN__IntPtr() { Test("Test_CONV_OVF_I_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethodEX("Test_CONV_OVF_I_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_OVF_I_UN__IntPtr() { Test("Test_CONV_OVF_I_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_CONV_OVF_I_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_I_UN__IntPtr() { Test("Test_CONV_OVF_I_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_OVF_I_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_OVF_I_UN__IntPtr() { Test("Test_CONV_OVF_I_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethodEX("Test_CONV_OVF_I_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_OVF_I_UN__IntPtr() { Test("Test_CONV_OVF_I_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_OVF_I_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_OVF_I_UN__IntPtr() { Test("Test_CONV_OVF_I_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_OVF_I_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_I_UN__IntPtr() { Test("Test_CONV_OVF_I_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_I_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_I1_UN__IntPtr() { Test("Test_CONV_OVF_I1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethodEX("Test_CONV_OVF_I1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_OVF_I1_UN__IntPtr() { Test("Test_CONV_OVF_I1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_I1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_I1_UN__IntPtr() { Test("Test_CONV_OVF_I1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_OVF_I1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_OVF_I1_UN__IntPtr() { Test("Test_CONV_OVF_I1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethodEX("Test_CONV_OVF_I1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_OVF_I1_UN__IntPtr() { Test("Test_CONV_OVF_I1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_OVF_I1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_OVF_I1_UN__IntPtr() { Test("Test_CONV_OVF_I1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_I1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_I1_UN__IntPtr() { Test("Test_CONV_OVF_I1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_I1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_I2_UN__IntPtr() { Test("Test_CONV_OVF_I2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethodEX("Test_CONV_OVF_I2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_OVF_I2_UN__IntPtr() { Test("Test_CONV_OVF_I2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_I2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_I2_UN__IntPtr() { Test("Test_CONV_OVF_I2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_OVF_I2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_OVF_I2_UN__IntPtr() { Test("Test_CONV_OVF_I2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethodEX("Test_CONV_OVF_I2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_OVF_I2_UN__IntPtr() { Test("Test_CONV_OVF_I2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_OVF_I2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_OVF_I2_UN__IntPtr() { Test("Test_CONV_OVF_I2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_I2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_I2_UN__IntPtr() { Test("Test_CONV_OVF_I2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_I2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_I4_UN__IntPtr() { Test("Test_CONV_OVF_I4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethodEX("Test_CONV_OVF_I4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_OVF_I4_UN__IntPtr() { Test("Test_CONV_OVF_I4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_I4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_I4_UN__IntPtr() { Test("Test_CONV_OVF_I4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_OVF_I4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_OVF_I4_UN__IntPtr() { Test("Test_CONV_OVF_I4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethodEX("Test_CONV_OVF_I4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_OVF_I4_UN__IntPtr() { Test("Test_CONV_OVF_I4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_OVF_I4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_OVF_I4_UN__IntPtr() { Test("Test_CONV_OVF_I4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_I4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_I4_UN__IntPtr() { Test("Test_CONV_OVF_I4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_I4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_I8_UN__IntPtr() { Test("Test_CONV_OVF_I8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethodEX("Test_CONV_OVF_I8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_OVF_I8_UN__IntPtr() { Test("Test_CONV_OVF_I8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_CONV_OVF_I8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_I8_UN__IntPtr() { Test("Test_CONV_OVF_I8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_OVF_I8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_OVF_I8_UN__IntPtr() { Test("Test_CONV_OVF_I8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethodEX("Test_CONV_OVF_I8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_OVF_I8_UN__IntPtr() { Test("Test_CONV_OVF_I8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_OVF_I8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_OVF_I8_UN__IntPtr() { Test("Test_CONV_OVF_I8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_OVF_I8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_I8_UN__IntPtr() { Test("Test_CONV_OVF_I8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_I8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_U_UN__IntPtr() { Test("Test_CONV_OVF_U_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_CONV_OVF_U_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_OVF_U_UN__IntPtr() { Test("Test_CONV_OVF_U_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_CONV_OVF_U_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_U_UN__IntPtr() { Test("Test_CONV_OVF_U_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_OVF_U_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_OVF_U_UN__IntPtr() { Test("Test_CONV_OVF_U_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethod("Test_CONV_OVF_U_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_OVF_U_UN__IntPtr() { Test("Test_CONV_OVF_U_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_OVF_U_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_OVF_U_UN__IntPtr() { Test("Test_CONV_OVF_U_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_OVF_U_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_U_UN__IntPtr() { Test("Test_CONV_OVF_U_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_OVF_U_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_U1_UN__IntPtr() { Test("Test_CONV_OVF_U1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethodEX("Test_CONV_OVF_U1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_OVF_U1_UN__IntPtr() { Test("Test_CONV_OVF_U1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_U1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_U1_UN__IntPtr() { Test("Test_CONV_OVF_U1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_OVF_U1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_OVF_U1_UN__IntPtr() { Test("Test_CONV_OVF_U1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethodEX("Test_CONV_OVF_U1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_OVF_U1_UN__IntPtr() { Test("Test_CONV_OVF_U1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_OVF_U1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_OVF_U1_UN__IntPtr() { Test("Test_CONV_OVF_U1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_U1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_U1_UN__IntPtr() { Test("Test_CONV_OVF_U1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_U1_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_U2_UN__IntPtr() { Test("Test_CONV_OVF_U2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethodEX("Test_CONV_OVF_U2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_OVF_U2_UN__IntPtr() { Test("Test_CONV_OVF_U2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_U2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_U2_UN__IntPtr() { Test("Test_CONV_OVF_U2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_OVF_U2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_OVF_U2_UN__IntPtr() { Test("Test_CONV_OVF_U2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethodEX("Test_CONV_OVF_U2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_OVF_U2_UN__IntPtr() { Test("Test_CONV_OVF_U2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_OVF_U2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_OVF_U2_UN__IntPtr() { Test("Test_CONV_OVF_U2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_U2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_U2_UN__IntPtr() { Test("Test_CONV_OVF_U2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_U2_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_U4_UN__IntPtr() { Test("Test_CONV_OVF_U4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethodEX("Test_CONV_OVF_U4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_OVF_U4_UN__IntPtr() { Test("Test_CONV_OVF_U4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_U4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_U4_UN__IntPtr() { Test("Test_CONV_OVF_U4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_OVF_U4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_OVF_U4_UN__IntPtr() { Test("Test_CONV_OVF_U4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethodEX("Test_CONV_OVF_U4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_OVF_U4_UN__IntPtr() { Test("Test_CONV_OVF_U4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_OVF_U4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_OVF_U4_UN__IntPtr() { Test("Test_CONV_OVF_U4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_U4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_U4_UN__IntPtr() { Test("Test_CONV_OVF_U4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethodEX("Test_CONV_OVF_U4_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_U8_UN__IntPtr() { Test("Test_CONV_OVF_U8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_CONV_OVF_U8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_OVF_U8_UN__IntPtr() { Test("Test_CONV_OVF_U8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_CONV_OVF_U8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_U8_UN__IntPtr() { Test("Test_CONV_OVF_U8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_OVF_U8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_OVF_U8_UN__IntPtr() { Test("Test_CONV_OVF_U8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethod("Test_CONV_OVF_U8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_OVF_U8_UN__IntPtr() { Test("Test_CONV_OVF_U8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_OVF_U8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_OVF_U8_UN__IntPtr() { Test("Test_CONV_OVF_U8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_OVF_U8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_OVF_U8_UN__IntPtr() { Test("Test_CONV_OVF_U8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_OVF_U8_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_R_UN__IntPtr() { Test("Test_CONV_R_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_CONV_R_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_CONV_R_UN__IntPtr() { Test("Test_CONV_R_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_CONV_R_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_CONV_R_UN__IntPtr() { Test("Test_CONV_R_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CONV_R_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CONV_R_UN__IntPtr() { Test("Test_CONV_R_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethod("Test_CONV_R_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_CONV_R_UN__IntPtr() { Test("Test_CONV_R_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CONV_R_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CONV_R_UN__IntPtr() { Test("Test_CONV_R_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_R_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_R_UN__IntPtr() { Test("Test_CONV_R_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); } /* TestMethod("Test_CONV_R_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x12345678) : new IntPtr(-0x123456789ABCDEF0L)); */
        //[Fact] public void Test_CONV_I__SByte() { Test("Test_CONV_I__SByte", sbyte.MinValue); } /* TestMethod("Test_CONV_I__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_I__SByte() { Test("Test_CONV_I__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_I__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_I__SByte() { Test("Test_CONV_I__SByte", (sbyte)0); } /* TestMethod("Test_CONV_I__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_I__SByte() { Test("Test_CONV_I__SByte", (sbyte)-1); } /* TestMethod("Test_CONV_I__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_I__SByte() { Test("Test_CONV_I__SByte", (sbyte)1); } /* TestMethod("Test_CONV_I__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_I__SByte() { Test("Test_CONV_I__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_I__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_I__SByte() { Test("Test_CONV_I__SByte", (sbyte)-0x12); } /* TestMethod("Test_CONV_I__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_I1__SByte() { Test("Test_CONV_I1__SByte", sbyte.MinValue); } /* TestMethod("Test_CONV_I1__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_I1__SByte() { Test("Test_CONV_I1__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_I1__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_I1__SByte() { Test("Test_CONV_I1__SByte", (sbyte)0); } /* TestMethod("Test_CONV_I1__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_I1__SByte() { Test("Test_CONV_I1__SByte", (sbyte)-1); } /* TestMethod("Test_CONV_I1__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_I1__SByte() { Test("Test_CONV_I1__SByte", (sbyte)1); } /* TestMethod("Test_CONV_I1__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_I1__SByte() { Test("Test_CONV_I1__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_I1__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_I1__SByte() { Test("Test_CONV_I1__SByte", (sbyte)-0x12); } /* TestMethod("Test_CONV_I1__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_I2__SByte() { Test("Test_CONV_I2__SByte", sbyte.MinValue); } /* TestMethod("Test_CONV_I2__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_I2__SByte() { Test("Test_CONV_I2__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_I2__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_I2__SByte() { Test("Test_CONV_I2__SByte", (sbyte)0); } /* TestMethod("Test_CONV_I2__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_I2__SByte() { Test("Test_CONV_I2__SByte", (sbyte)-1); } /* TestMethod("Test_CONV_I2__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_I2__SByte() { Test("Test_CONV_I2__SByte", (sbyte)1); } /* TestMethod("Test_CONV_I2__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_I2__SByte() { Test("Test_CONV_I2__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_I2__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_I2__SByte() { Test("Test_CONV_I2__SByte", (sbyte)-0x12); } /* TestMethod("Test_CONV_I2__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_I4__SByte() { Test("Test_CONV_I4__SByte", sbyte.MinValue); } /* TestMethod("Test_CONV_I4__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_I4__SByte() { Test("Test_CONV_I4__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_I4__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_I4__SByte() { Test("Test_CONV_I4__SByte", (sbyte)0); } /* TestMethod("Test_CONV_I4__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_I4__SByte() { Test("Test_CONV_I4__SByte", (sbyte)-1); } /* TestMethod("Test_CONV_I4__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_I4__SByte() { Test("Test_CONV_I4__SByte", (sbyte)1); } /* TestMethod("Test_CONV_I4__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_I4__SByte() { Test("Test_CONV_I4__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_I4__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_I4__SByte() { Test("Test_CONV_I4__SByte", (sbyte)-0x12); } /* TestMethod("Test_CONV_I4__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_I8__SByte() { Test("Test_CONV_I8__SByte", sbyte.MinValue); } /* TestMethod("Test_CONV_I8__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_I8__SByte() { Test("Test_CONV_I8__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_I8__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_I8__SByte() { Test("Test_CONV_I8__SByte", (sbyte)0); } /* TestMethod("Test_CONV_I8__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_I8__SByte() { Test("Test_CONV_I8__SByte", (sbyte)-1); } /* TestMethod("Test_CONV_I8__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_I8__SByte() { Test("Test_CONV_I8__SByte", (sbyte)1); } /* TestMethod("Test_CONV_I8__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_I8__SByte() { Test("Test_CONV_I8__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_I8__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_I8__SByte() { Test("Test_CONV_I8__SByte", (sbyte)-0x12); } /* TestMethod("Test_CONV_I8__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_U__SByte() { Test("Test_CONV_U__SByte", sbyte.MinValue); } /* TestMethod("Test_CONV_U__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_U__SByte() { Test("Test_CONV_U__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_U__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_U__SByte() { Test("Test_CONV_U__SByte", (sbyte)0); } /* TestMethod("Test_CONV_U__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_U__SByte() { Test("Test_CONV_U__SByte", (sbyte)-1); } /* TestMethod("Test_CONV_U__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_U__SByte() { Test("Test_CONV_U__SByte", (sbyte)1); } /* TestMethod("Test_CONV_U__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_U__SByte() { Test("Test_CONV_U__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_U__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_U__SByte() { Test("Test_CONV_U__SByte", (sbyte)-0x12); } /* TestMethod("Test_CONV_U__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_U1__SByte() { Test("Test_CONV_U1__SByte", sbyte.MinValue); } /* TestMethod("Test_CONV_U1__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_U1__SByte() { Test("Test_CONV_U1__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_U1__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_U1__SByte() { Test("Test_CONV_U1__SByte", (sbyte)0); } /* TestMethod("Test_CONV_U1__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_U1__SByte() { Test("Test_CONV_U1__SByte", (sbyte)-1); } /* TestMethod("Test_CONV_U1__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_U1__SByte() { Test("Test_CONV_U1__SByte", (sbyte)1); } /* TestMethod("Test_CONV_U1__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_U1__SByte() { Test("Test_CONV_U1__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_U1__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_U1__SByte() { Test("Test_CONV_U1__SByte", (sbyte)-0x12); } /* TestMethod("Test_CONV_U1__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_U2__SByte() { Test("Test_CONV_U2__SByte", sbyte.MinValue); } /* TestMethod("Test_CONV_U2__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_U2__SByte() { Test("Test_CONV_U2__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_U2__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_U2__SByte() { Test("Test_CONV_U2__SByte", (sbyte)0); } /* TestMethod("Test_CONV_U2__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_U2__SByte() { Test("Test_CONV_U2__SByte", (sbyte)-1); } /* TestMethod("Test_CONV_U2__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_U2__SByte() { Test("Test_CONV_U2__SByte", (sbyte)1); } /* TestMethod("Test_CONV_U2__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_U2__SByte() { Test("Test_CONV_U2__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_U2__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_U2__SByte() { Test("Test_CONV_U2__SByte", (sbyte)-0x12); } /* TestMethod("Test_CONV_U2__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_U4__SByte() { Test("Test_CONV_U4__SByte", sbyte.MinValue); } /* TestMethod("Test_CONV_U4__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_U4__SByte() { Test("Test_CONV_U4__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_U4__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_U4__SByte() { Test("Test_CONV_U4__SByte", (sbyte)0); } /* TestMethod("Test_CONV_U4__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_U4__SByte() { Test("Test_CONV_U4__SByte", (sbyte)-1); } /* TestMethod("Test_CONV_U4__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_U4__SByte() { Test("Test_CONV_U4__SByte", (sbyte)1); } /* TestMethod("Test_CONV_U4__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_U4__SByte() { Test("Test_CONV_U4__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_U4__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_U4__SByte() { Test("Test_CONV_U4__SByte", (sbyte)-0x12); } /* TestMethod("Test_CONV_U4__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_U8__SByte() { Test("Test_CONV_U8__SByte", sbyte.MinValue); } /* TestMethod("Test_CONV_U8__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_U8__SByte() { Test("Test_CONV_U8__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_U8__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_U8__SByte() { Test("Test_CONV_U8__SByte", (sbyte)0); } /* TestMethod("Test_CONV_U8__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_U8__SByte() { Test("Test_CONV_U8__SByte", (sbyte)-1); } /* TestMethod("Test_CONV_U8__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_U8__SByte() { Test("Test_CONV_U8__SByte", (sbyte)1); } /* TestMethod("Test_CONV_U8__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_U8__SByte() { Test("Test_CONV_U8__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_U8__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_U8__SByte() { Test("Test_CONV_U8__SByte", (sbyte)-0x12); } /* TestMethod("Test_CONV_U8__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_R4__SByte() { Test("Test_CONV_R4__SByte", sbyte.MinValue); } /* TestMethod("Test_CONV_R4__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_R4__SByte() { Test("Test_CONV_R4__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_R4__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_R4__SByte() { Test("Test_CONV_R4__SByte", (sbyte)0); } /* TestMethod("Test_CONV_R4__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_R4__SByte() { Test("Test_CONV_R4__SByte", (sbyte)-1); } /* TestMethod("Test_CONV_R4__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_R4__SByte() { Test("Test_CONV_R4__SByte", (sbyte)1); } /* TestMethod("Test_CONV_R4__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_R4__SByte() { Test("Test_CONV_R4__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_R4__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_R4__SByte() { Test("Test_CONV_R4__SByte", (sbyte)-0x12); } /* TestMethod("Test_CONV_R4__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_R8__SByte() { Test("Test_CONV_R8__SByte", sbyte.MinValue); } /* TestMethod("Test_CONV_R8__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_R8__SByte() { Test("Test_CONV_R8__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_R8__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_R8__SByte() { Test("Test_CONV_R8__SByte", (sbyte)0); } /* TestMethod("Test_CONV_R8__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_R8__SByte() { Test("Test_CONV_R8__SByte", (sbyte)-1); } /* TestMethod("Test_CONV_R8__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_R8__SByte() { Test("Test_CONV_R8__SByte", (sbyte)1); } /* TestMethod("Test_CONV_R8__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_R8__SByte() { Test("Test_CONV_R8__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_R8__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_R8__SByte() { Test("Test_CONV_R8__SByte", (sbyte)-0x12); } /* TestMethod("Test_CONV_R8__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_OVF_I__SByte() { Test("Test_CONV_OVF_I__SByte", sbyte.MinValue); } /* TestMethod("Test_CONV_OVF_I__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_OVF_I__SByte() { Test("Test_CONV_OVF_I__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_OVF_I__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I__SByte() { Test("Test_CONV_OVF_I__SByte", (sbyte)0); } /* TestMethod("Test_CONV_OVF_I__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_OVF_I__SByte() { Test("Test_CONV_OVF_I__SByte", (sbyte)-1); } /* TestMethod("Test_CONV_OVF_I__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_OVF_I__SByte() { Test("Test_CONV_OVF_I__SByte", (sbyte)1); } /* TestMethod("Test_CONV_OVF_I__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_OVF_I__SByte() { Test("Test_CONV_OVF_I__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_OVF_I__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_OVF_I__SByte() { Test("Test_CONV_OVF_I__SByte", (sbyte)-0x12); } /* TestMethod("Test_CONV_OVF_I__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_OVF_I1__SByte() { Test("Test_CONV_OVF_I1__SByte", sbyte.MinValue); } /* TestMethod("Test_CONV_OVF_I1__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_OVF_I1__SByte() { Test("Test_CONV_OVF_I1__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_OVF_I1__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I1__SByte() { Test("Test_CONV_OVF_I1__SByte", (sbyte)0); } /* TestMethod("Test_CONV_OVF_I1__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_OVF_I1__SByte() { Test("Test_CONV_OVF_I1__SByte", (sbyte)-1); } /* TestMethod("Test_CONV_OVF_I1__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_OVF_I1__SByte() { Test("Test_CONV_OVF_I1__SByte", (sbyte)1); } /* TestMethod("Test_CONV_OVF_I1__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_OVF_I1__SByte() { Test("Test_CONV_OVF_I1__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_OVF_I1__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_OVF_I1__SByte() { Test("Test_CONV_OVF_I1__SByte", (sbyte)-0x12); } /* TestMethod("Test_CONV_OVF_I1__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_OVF_I2__SByte() { Test("Test_CONV_OVF_I2__SByte", sbyte.MinValue); } /* TestMethod("Test_CONV_OVF_I2__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_OVF_I2__SByte() { Test("Test_CONV_OVF_I2__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_OVF_I2__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I2__SByte() { Test("Test_CONV_OVF_I2__SByte", (sbyte)0); } /* TestMethod("Test_CONV_OVF_I2__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_OVF_I2__SByte() { Test("Test_CONV_OVF_I2__SByte", (sbyte)-1); } /* TestMethod("Test_CONV_OVF_I2__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_OVF_I2__SByte() { Test("Test_CONV_OVF_I2__SByte", (sbyte)1); } /* TestMethod("Test_CONV_OVF_I2__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_OVF_I2__SByte() { Test("Test_CONV_OVF_I2__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_OVF_I2__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_OVF_I2__SByte() { Test("Test_CONV_OVF_I2__SByte", (sbyte)-0x12); } /* TestMethod("Test_CONV_OVF_I2__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_OVF_I4__SByte() { Test("Test_CONV_OVF_I4__SByte", sbyte.MinValue); } /* TestMethod("Test_CONV_OVF_I4__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_OVF_I4__SByte() { Test("Test_CONV_OVF_I4__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_OVF_I4__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I4__SByte() { Test("Test_CONV_OVF_I4__SByte", (sbyte)0); } /* TestMethod("Test_CONV_OVF_I4__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_OVF_I4__SByte() { Test("Test_CONV_OVF_I4__SByte", (sbyte)-1); } /* TestMethod("Test_CONV_OVF_I4__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_OVF_I4__SByte() { Test("Test_CONV_OVF_I4__SByte", (sbyte)1); } /* TestMethod("Test_CONV_OVF_I4__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_OVF_I4__SByte() { Test("Test_CONV_OVF_I4__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_OVF_I4__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_OVF_I4__SByte() { Test("Test_CONV_OVF_I4__SByte", (sbyte)-0x12); } /* TestMethod("Test_CONV_OVF_I4__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_OVF_I8__SByte() { Test("Test_CONV_OVF_I8__SByte", sbyte.MinValue); } /* TestMethod("Test_CONV_OVF_I8__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_OVF_I8__SByte() { Test("Test_CONV_OVF_I8__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_OVF_I8__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I8__SByte() { Test("Test_CONV_OVF_I8__SByte", (sbyte)0); } /* TestMethod("Test_CONV_OVF_I8__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_OVF_I8__SByte() { Test("Test_CONV_OVF_I8__SByte", (sbyte)-1); } /* TestMethod("Test_CONV_OVF_I8__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_OVF_I8__SByte() { Test("Test_CONV_OVF_I8__SByte", (sbyte)1); } /* TestMethod("Test_CONV_OVF_I8__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_OVF_I8__SByte() { Test("Test_CONV_OVF_I8__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_OVF_I8__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_OVF_I8__SByte() { Test("Test_CONV_OVF_I8__SByte", (sbyte)-0x12); } /* TestMethod("Test_CONV_OVF_I8__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_OVF_U__SByte() { Test("Test_CONV_OVF_U__SByte", sbyte.MinValue); } /* TestMethodEX("Test_CONV_OVF_U__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_OVF_U__SByte() { Test("Test_CONV_OVF_U__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_OVF_U__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U__SByte() { Test("Test_CONV_OVF_U__SByte", (sbyte)0); } /* TestMethod("Test_CONV_OVF_U__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_OVF_U__SByte() { Test("Test_CONV_OVF_U__SByte", (sbyte)-1); } /* TestMethodEX("Test_CONV_OVF_U__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_OVF_U__SByte() { Test("Test_CONV_OVF_U__SByte", (sbyte)1); } /* TestMethod("Test_CONV_OVF_U__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_OVF_U__SByte() { Test("Test_CONV_OVF_U__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_OVF_U__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_OVF_U__SByte() { Test("Test_CONV_OVF_U__SByte", (sbyte)-0x12); } /* TestMethodEX("Test_CONV_OVF_U__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_OVF_U1__SByte() { Test("Test_CONV_OVF_U1__SByte", sbyte.MinValue); } /* TestMethodEX("Test_CONV_OVF_U1__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_OVF_U1__SByte() { Test("Test_CONV_OVF_U1__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_OVF_U1__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U1__SByte() { Test("Test_CONV_OVF_U1__SByte", (sbyte)0); } /* TestMethod("Test_CONV_OVF_U1__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_OVF_U1__SByte() { Test("Test_CONV_OVF_U1__SByte", (sbyte)-1); } /* TestMethodEX("Test_CONV_OVF_U1__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_OVF_U1__SByte() { Test("Test_CONV_OVF_U1__SByte", (sbyte)1); } /* TestMethod("Test_CONV_OVF_U1__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_OVF_U1__SByte() { Test("Test_CONV_OVF_U1__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_OVF_U1__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_OVF_U1__SByte() { Test("Test_CONV_OVF_U1__SByte", (sbyte)-0x12); } /* TestMethodEX("Test_CONV_OVF_U1__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_OVF_U2__SByte() { Test("Test_CONV_OVF_U2__SByte", sbyte.MinValue); } /* TestMethodEX("Test_CONV_OVF_U2__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_OVF_U2__SByte() { Test("Test_CONV_OVF_U2__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_OVF_U2__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U2__SByte() { Test("Test_CONV_OVF_U2__SByte", (sbyte)0); } /* TestMethod("Test_CONV_OVF_U2__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_OVF_U2__SByte() { Test("Test_CONV_OVF_U2__SByte", (sbyte)-1); } /* TestMethodEX("Test_CONV_OVF_U2__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_OVF_U2__SByte() { Test("Test_CONV_OVF_U2__SByte", (sbyte)1); } /* TestMethod("Test_CONV_OVF_U2__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_OVF_U2__SByte() { Test("Test_CONV_OVF_U2__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_OVF_U2__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_OVF_U2__SByte() { Test("Test_CONV_OVF_U2__SByte", (sbyte)-0x12); } /* TestMethodEX("Test_CONV_OVF_U2__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_OVF_U4__SByte() { Test("Test_CONV_OVF_U4__SByte", sbyte.MinValue); } /* TestMethodEX("Test_CONV_OVF_U4__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_OVF_U4__SByte() { Test("Test_CONV_OVF_U4__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_OVF_U4__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U4__SByte() { Test("Test_CONV_OVF_U4__SByte", (sbyte)0); } /* TestMethod("Test_CONV_OVF_U4__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_OVF_U4__SByte() { Test("Test_CONV_OVF_U4__SByte", (sbyte)-1); } /* TestMethodEX("Test_CONV_OVF_U4__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_OVF_U4__SByte() { Test("Test_CONV_OVF_U4__SByte", (sbyte)1); } /* TestMethod("Test_CONV_OVF_U4__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_OVF_U4__SByte() { Test("Test_CONV_OVF_U4__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_OVF_U4__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_OVF_U4__SByte() { Test("Test_CONV_OVF_U4__SByte", (sbyte)-0x12); } /* TestMethodEX("Test_CONV_OVF_U4__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_OVF_U8__SByte() { Test("Test_CONV_OVF_U8__SByte", sbyte.MinValue); } /* TestMethodEX("Test_CONV_OVF_U8__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_OVF_U8__SByte() { Test("Test_CONV_OVF_U8__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_OVF_U8__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U8__SByte() { Test("Test_CONV_OVF_U8__SByte", (sbyte)0); } /* TestMethod("Test_CONV_OVF_U8__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_OVF_U8__SByte() { Test("Test_CONV_OVF_U8__SByte", (sbyte)-1); } /* TestMethodEX("Test_CONV_OVF_U8__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_OVF_U8__SByte() { Test("Test_CONV_OVF_U8__SByte", (sbyte)1); } /* TestMethod("Test_CONV_OVF_U8__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_OVF_U8__SByte() { Test("Test_CONV_OVF_U8__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_OVF_U8__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_OVF_U8__SByte() { Test("Test_CONV_OVF_U8__SByte", (sbyte)-0x12); } /* TestMethodEX("Test_CONV_OVF_U8__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_OVF_I_UN__SByte() { Test("Test_CONV_OVF_I_UN__SByte", sbyte.MinValue); } /* TestMethodEX("Test_CONV_OVF_I_UN__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_OVF_I_UN__SByte() { Test("Test_CONV_OVF_I_UN__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_OVF_I_UN__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I_UN__SByte() { Test("Test_CONV_OVF_I_UN__SByte", (sbyte)0); } /* TestMethod("Test_CONV_OVF_I_UN__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_OVF_I_UN__SByte() { Test("Test_CONV_OVF_I_UN__SByte", (sbyte)-1); } /* TestMethodEX("Test_CONV_OVF_I_UN__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_OVF_I_UN__SByte() { Test("Test_CONV_OVF_I_UN__SByte", (sbyte)1); } /* TestMethod("Test_CONV_OVF_I_UN__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_OVF_I_UN__SByte() { Test("Test_CONV_OVF_I_UN__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_OVF_I_UN__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_OVF_I_UN__SByte() { Test("Test_CONV_OVF_I_UN__SByte", (sbyte)-0x12); } /* TestMethodEX("Test_CONV_OVF_I_UN__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_OVF_I1_UN__SByte() { Test("Test_CONV_OVF_I1_UN__SByte", sbyte.MinValue); } /* TestMethodEX("Test_CONV_OVF_I1_UN__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_OVF_I1_UN__SByte() { Test("Test_CONV_OVF_I1_UN__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_OVF_I1_UN__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I1_UN__SByte() { Test("Test_CONV_OVF_I1_UN__SByte", (sbyte)0); } /* TestMethod("Test_CONV_OVF_I1_UN__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_OVF_I1_UN__SByte() { Test("Test_CONV_OVF_I1_UN__SByte", (sbyte)-1); } /* TestMethodEX("Test_CONV_OVF_I1_UN__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_OVF_I1_UN__SByte() { Test("Test_CONV_OVF_I1_UN__SByte", (sbyte)1); } /* TestMethod("Test_CONV_OVF_I1_UN__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_OVF_I1_UN__SByte() { Test("Test_CONV_OVF_I1_UN__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_OVF_I1_UN__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_OVF_I1_UN__SByte() { Test("Test_CONV_OVF_I1_UN__SByte", (sbyte)-0x12); } /* TestMethodEX("Test_CONV_OVF_I1_UN__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_OVF_I2_UN__SByte() { Test("Test_CONV_OVF_I2_UN__SByte", sbyte.MinValue); } /* TestMethodEX("Test_CONV_OVF_I2_UN__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_OVF_I2_UN__SByte() { Test("Test_CONV_OVF_I2_UN__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_OVF_I2_UN__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I2_UN__SByte() { Test("Test_CONV_OVF_I2_UN__SByte", (sbyte)0); } /* TestMethod("Test_CONV_OVF_I2_UN__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_OVF_I2_UN__SByte() { Test("Test_CONV_OVF_I2_UN__SByte", (sbyte)-1); } /* TestMethodEX("Test_CONV_OVF_I2_UN__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_OVF_I2_UN__SByte() { Test("Test_CONV_OVF_I2_UN__SByte", (sbyte)1); } /* TestMethod("Test_CONV_OVF_I2_UN__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_OVF_I2_UN__SByte() { Test("Test_CONV_OVF_I2_UN__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_OVF_I2_UN__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_OVF_I2_UN__SByte() { Test("Test_CONV_OVF_I2_UN__SByte", (sbyte)-0x12); } /* TestMethodEX("Test_CONV_OVF_I2_UN__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_OVF_I4_UN__SByte() { Test("Test_CONV_OVF_I4_UN__SByte", sbyte.MinValue); } /* TestMethodEX("Test_CONV_OVF_I4_UN__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_OVF_I4_UN__SByte() { Test("Test_CONV_OVF_I4_UN__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_OVF_I4_UN__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I4_UN__SByte() { Test("Test_CONV_OVF_I4_UN__SByte", (sbyte)0); } /* TestMethod("Test_CONV_OVF_I4_UN__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_OVF_I4_UN__SByte() { Test("Test_CONV_OVF_I4_UN__SByte", (sbyte)-1); } /* TestMethodEX("Test_CONV_OVF_I4_UN__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_OVF_I4_UN__SByte() { Test("Test_CONV_OVF_I4_UN__SByte", (sbyte)1); } /* TestMethod("Test_CONV_OVF_I4_UN__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_OVF_I4_UN__SByte() { Test("Test_CONV_OVF_I4_UN__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_OVF_I4_UN__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_OVF_I4_UN__SByte() { Test("Test_CONV_OVF_I4_UN__SByte", (sbyte)-0x12); } /* TestMethodEX("Test_CONV_OVF_I4_UN__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_OVF_I8_UN__SByte() { Test("Test_CONV_OVF_I8_UN__SByte", sbyte.MinValue); } /* TestMethodEX("Test_CONV_OVF_I8_UN__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_OVF_I8_UN__SByte() { Test("Test_CONV_OVF_I8_UN__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_OVF_I8_UN__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I8_UN__SByte() { Test("Test_CONV_OVF_I8_UN__SByte", (sbyte)0); } /* TestMethod("Test_CONV_OVF_I8_UN__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_OVF_I8_UN__SByte() { Test("Test_CONV_OVF_I8_UN__SByte", (sbyte)-1); } /* TestMethodEX("Test_CONV_OVF_I8_UN__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_OVF_I8_UN__SByte() { Test("Test_CONV_OVF_I8_UN__SByte", (sbyte)1); } /* TestMethod("Test_CONV_OVF_I8_UN__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_OVF_I8_UN__SByte() { Test("Test_CONV_OVF_I8_UN__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_OVF_I8_UN__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_OVF_I8_UN__SByte() { Test("Test_CONV_OVF_I8_UN__SByte", (sbyte)-0x12); } /* TestMethodEX("Test_CONV_OVF_I8_UN__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_OVF_U_UN__SByte() { Test("Test_CONV_OVF_U_UN__SByte", sbyte.MinValue); } /* TestMethod("Test_CONV_OVF_U_UN__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_OVF_U_UN__SByte() { Test("Test_CONV_OVF_U_UN__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_OVF_U_UN__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U_UN__SByte() { Test("Test_CONV_OVF_U_UN__SByte", (sbyte)0); } /* TestMethod("Test_CONV_OVF_U_UN__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_OVF_U_UN__SByte() { Test("Test_CONV_OVF_U_UN__SByte", (sbyte)-1); } /* TestMethod("Test_CONV_OVF_U_UN__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_OVF_U_UN__SByte() { Test("Test_CONV_OVF_U_UN__SByte", (sbyte)1); } /* TestMethod("Test_CONV_OVF_U_UN__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_OVF_U_UN__SByte() { Test("Test_CONV_OVF_U_UN__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_OVF_U_UN__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_OVF_U_UN__SByte() { Test("Test_CONV_OVF_U_UN__SByte", (sbyte)-0x12); } /* TestMethod("Test_CONV_OVF_U_UN__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_OVF_U1_UN__SByte() { Test("Test_CONV_OVF_U1_UN__SByte", sbyte.MinValue); } /* TestMethodEX("Test_CONV_OVF_U1_UN__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_OVF_U1_UN__SByte() { Test("Test_CONV_OVF_U1_UN__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_OVF_U1_UN__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U1_UN__SByte() { Test("Test_CONV_OVF_U1_UN__SByte", (sbyte)0); } /* TestMethod("Test_CONV_OVF_U1_UN__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_OVF_U1_UN__SByte() { Test("Test_CONV_OVF_U1_UN__SByte", (sbyte)-1); } /* TestMethodEX("Test_CONV_OVF_U1_UN__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_OVF_U1_UN__SByte() { Test("Test_CONV_OVF_U1_UN__SByte", (sbyte)1); } /* TestMethod("Test_CONV_OVF_U1_UN__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_OVF_U1_UN__SByte() { Test("Test_CONV_OVF_U1_UN__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_OVF_U1_UN__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_OVF_U1_UN__SByte() { Test("Test_CONV_OVF_U1_UN__SByte", (sbyte)-0x12); } /* TestMethodEX("Test_CONV_OVF_U1_UN__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_OVF_U2_UN__SByte() { Test("Test_CONV_OVF_U2_UN__SByte", sbyte.MinValue); } /* TestMethodEX("Test_CONV_OVF_U2_UN__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_OVF_U2_UN__SByte() { Test("Test_CONV_OVF_U2_UN__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_OVF_U2_UN__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U2_UN__SByte() { Test("Test_CONV_OVF_U2_UN__SByte", (sbyte)0); } /* TestMethod("Test_CONV_OVF_U2_UN__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_OVF_U2_UN__SByte() { Test("Test_CONV_OVF_U2_UN__SByte", (sbyte)-1); } /* TestMethodEX("Test_CONV_OVF_U2_UN__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_OVF_U2_UN__SByte() { Test("Test_CONV_OVF_U2_UN__SByte", (sbyte)1); } /* TestMethod("Test_CONV_OVF_U2_UN__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_OVF_U2_UN__SByte() { Test("Test_CONV_OVF_U2_UN__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_OVF_U2_UN__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_OVF_U2_UN__SByte() { Test("Test_CONV_OVF_U2_UN__SByte", (sbyte)-0x12); } /* TestMethodEX("Test_CONV_OVF_U2_UN__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_OVF_U4_UN__SByte() { Test("Test_CONV_OVF_U4_UN__SByte", sbyte.MinValue); } /* TestMethodEX("Test_CONV_OVF_U4_UN__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_OVF_U4_UN__SByte() { Test("Test_CONV_OVF_U4_UN__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_OVF_U4_UN__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U4_UN__SByte() { Test("Test_CONV_OVF_U4_UN__SByte", (sbyte)0); } /* TestMethod("Test_CONV_OVF_U4_UN__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_OVF_U4_UN__SByte() { Test("Test_CONV_OVF_U4_UN__SByte", (sbyte)-1); } /* TestMethodEX("Test_CONV_OVF_U4_UN__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_OVF_U4_UN__SByte() { Test("Test_CONV_OVF_U4_UN__SByte", (sbyte)1); } /* TestMethod("Test_CONV_OVF_U4_UN__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_OVF_U4_UN__SByte() { Test("Test_CONV_OVF_U4_UN__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_OVF_U4_UN__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_OVF_U4_UN__SByte() { Test("Test_CONV_OVF_U4_UN__SByte", (sbyte)-0x12); } /* TestMethodEX("Test_CONV_OVF_U4_UN__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_OVF_U8_UN__SByte() { Test("Test_CONV_OVF_U8_UN__SByte", sbyte.MinValue); } /* TestMethodEX("Test_CONV_OVF_U8_UN__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_OVF_U8_UN__SByte() { Test("Test_CONV_OVF_U8_UN__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_OVF_U8_UN__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U8_UN__SByte() { Test("Test_CONV_OVF_U8_UN__SByte", (sbyte)0); } /* TestMethod("Test_CONV_OVF_U8_UN__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_OVF_U8_UN__SByte() { Test("Test_CONV_OVF_U8_UN__SByte", (sbyte)-1); } /* TestMethodEX("Test_CONV_OVF_U8_UN__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_OVF_U8_UN__SByte() { Test("Test_CONV_OVF_U8_UN__SByte", (sbyte)1); } /* TestMethod("Test_CONV_OVF_U8_UN__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_OVF_U8_UN__SByte() { Test("Test_CONV_OVF_U8_UN__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_OVF_U8_UN__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_OVF_U8_UN__SByte() { Test("Test_CONV_OVF_U8_UN__SByte", (sbyte)-0x12); } /* TestMethodEX("Test_CONV_OVF_U8_UN__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_R_UN__SByte() { Test("Test_CONV_R_UN__SByte", sbyte.MinValue); } /* TestMethod("Test_CONV_R_UN__SByte", sbyte.MinValue); */
        //[Fact] public void Test_CONV_R_UN__SByte() { Test("Test_CONV_R_UN__SByte", sbyte.MaxValue); } /* TestMethod("Test_CONV_R_UN__SByte", sbyte.MaxValue); */
        //[Fact] public void Test_CONV_R_UN__SByte() { Test("Test_CONV_R_UN__SByte", (sbyte)0); } /* TestMethod("Test_CONV_R_UN__SByte", (sbyte)0); */
        //[Fact] public void Test_CONV_R_UN__SByte() { Test("Test_CONV_R_UN__SByte", (sbyte)-1); } /* TestMethod("Test_CONV_R_UN__SByte", (sbyte)-1); */
        //[Fact] public void Test_CONV_R_UN__SByte() { Test("Test_CONV_R_UN__SByte", (sbyte)1); } /* TestMethod("Test_CONV_R_UN__SByte", (sbyte)1); */
        //[Fact] public void Test_CONV_R_UN__SByte() { Test("Test_CONV_R_UN__SByte", (sbyte)0x12); } /* TestMethod("Test_CONV_R_UN__SByte", (sbyte)0x12); */
        //[Fact] public void Test_CONV_R_UN__SByte() { Test("Test_CONV_R_UN__SByte", (sbyte)-0x12); } /* TestMethod("Test_CONV_R_UN__SByte", (sbyte)-0x12); */
        //[Fact] public void Test_CONV_I__Int16() { Test("Test_CONV_I__Int16", short.MinValue); } /* TestMethod("Test_CONV_I__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_I__Int16() { Test("Test_CONV_I__Int16", short.MaxValue); } /* TestMethod("Test_CONV_I__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_I__Int16() { Test("Test_CONV_I__Int16", (short)0); } /* TestMethod("Test_CONV_I__Int16", (short)0); */
        //[Fact] public void Test_CONV_I__Int16() { Test("Test_CONV_I__Int16", (short)-1); } /* TestMethod("Test_CONV_I__Int16", (short)-1); */
        //[Fact] public void Test_CONV_I__Int16() { Test("Test_CONV_I__Int16", (short)1); } /* TestMethod("Test_CONV_I__Int16", (short)1); */
        //[Fact] public void Test_CONV_I__Int16() { Test("Test_CONV_I__Int16", (short)0x1234); } /* TestMethod("Test_CONV_I__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_I__Int16() { Test("Test_CONV_I__Int16", (short)-0x1234); } /* TestMethod("Test_CONV_I__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_I1__Int16() { Test("Test_CONV_I1__Int16", short.MinValue); } /* TestMethod("Test_CONV_I1__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_I1__Int16() { Test("Test_CONV_I1__Int16", short.MaxValue); } /* TestMethod("Test_CONV_I1__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_I1__Int16() { Test("Test_CONV_I1__Int16", (short)0); } /* TestMethod("Test_CONV_I1__Int16", (short)0); */
        //[Fact] public void Test_CONV_I1__Int16() { Test("Test_CONV_I1__Int16", (short)-1); } /* TestMethod("Test_CONV_I1__Int16", (short)-1); */
        //[Fact] public void Test_CONV_I1__Int16() { Test("Test_CONV_I1__Int16", (short)1); } /* TestMethod("Test_CONV_I1__Int16", (short)1); */
        //[Fact] public void Test_CONV_I1__Int16() { Test("Test_CONV_I1__Int16", (short)0x1234); } /* TestMethod("Test_CONV_I1__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_I1__Int16() { Test("Test_CONV_I1__Int16", (short)-0x1234); } /* TestMethod("Test_CONV_I1__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_I2__Int16() { Test("Test_CONV_I2__Int16", short.MinValue); } /* TestMethod("Test_CONV_I2__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_I2__Int16() { Test("Test_CONV_I2__Int16", short.MaxValue); } /* TestMethod("Test_CONV_I2__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_I2__Int16() { Test("Test_CONV_I2__Int16", (short)0); } /* TestMethod("Test_CONV_I2__Int16", (short)0); */
        //[Fact] public void Test_CONV_I2__Int16() { Test("Test_CONV_I2__Int16", (short)-1); } /* TestMethod("Test_CONV_I2__Int16", (short)-1); */
        //[Fact] public void Test_CONV_I2__Int16() { Test("Test_CONV_I2__Int16", (short)1); } /* TestMethod("Test_CONV_I2__Int16", (short)1); */
        //[Fact] public void Test_CONV_I2__Int16() { Test("Test_CONV_I2__Int16", (short)0x1234); } /* TestMethod("Test_CONV_I2__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_I2__Int16() { Test("Test_CONV_I2__Int16", (short)-0x1234); } /* TestMethod("Test_CONV_I2__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_I4__Int16() { Test("Test_CONV_I4__Int16", short.MinValue); } /* TestMethod("Test_CONV_I4__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_I4__Int16() { Test("Test_CONV_I4__Int16", short.MaxValue); } /* TestMethod("Test_CONV_I4__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_I4__Int16() { Test("Test_CONV_I4__Int16", (short)0); } /* TestMethod("Test_CONV_I4__Int16", (short)0); */
        //[Fact] public void Test_CONV_I4__Int16() { Test("Test_CONV_I4__Int16", (short)-1); } /* TestMethod("Test_CONV_I4__Int16", (short)-1); */
        //[Fact] public void Test_CONV_I4__Int16() { Test("Test_CONV_I4__Int16", (short)1); } /* TestMethod("Test_CONV_I4__Int16", (short)1); */
        //[Fact] public void Test_CONV_I4__Int16() { Test("Test_CONV_I4__Int16", (short)0x1234); } /* TestMethod("Test_CONV_I4__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_I4__Int16() { Test("Test_CONV_I4__Int16", (short)-0x1234); } /* TestMethod("Test_CONV_I4__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_I8__Int16() { Test("Test_CONV_I8__Int16", short.MinValue); } /* TestMethod("Test_CONV_I8__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_I8__Int16() { Test("Test_CONV_I8__Int16", short.MaxValue); } /* TestMethod("Test_CONV_I8__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_I8__Int16() { Test("Test_CONV_I8__Int16", (short)0); } /* TestMethod("Test_CONV_I8__Int16", (short)0); */
        //[Fact] public void Test_CONV_I8__Int16() { Test("Test_CONV_I8__Int16", (short)-1); } /* TestMethod("Test_CONV_I8__Int16", (short)-1); */
        //[Fact] public void Test_CONV_I8__Int16() { Test("Test_CONV_I8__Int16", (short)1); } /* TestMethod("Test_CONV_I8__Int16", (short)1); */
        //[Fact] public void Test_CONV_I8__Int16() { Test("Test_CONV_I8__Int16", (short)0x1234); } /* TestMethod("Test_CONV_I8__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_I8__Int16() { Test("Test_CONV_I8__Int16", (short)-0x1234); } /* TestMethod("Test_CONV_I8__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_U__Int16() { Test("Test_CONV_U__Int16", short.MinValue); } /* TestMethod("Test_CONV_U__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_U__Int16() { Test("Test_CONV_U__Int16", short.MaxValue); } /* TestMethod("Test_CONV_U__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_U__Int16() { Test("Test_CONV_U__Int16", (short)0); } /* TestMethod("Test_CONV_U__Int16", (short)0); */
        //[Fact] public void Test_CONV_U__Int16() { Test("Test_CONV_U__Int16", (short)-1); } /* TestMethod("Test_CONV_U__Int16", (short)-1); */
        //[Fact] public void Test_CONV_U__Int16() { Test("Test_CONV_U__Int16", (short)1); } /* TestMethod("Test_CONV_U__Int16", (short)1); */
        //[Fact] public void Test_CONV_U__Int16() { Test("Test_CONV_U__Int16", (short)0x1234); } /* TestMethod("Test_CONV_U__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_U__Int16() { Test("Test_CONV_U__Int16", (short)-0x1234); } /* TestMethod("Test_CONV_U__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_U1__Int16() { Test("Test_CONV_U1__Int16", short.MinValue); } /* TestMethod("Test_CONV_U1__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_U1__Int16() { Test("Test_CONV_U1__Int16", short.MaxValue); } /* TestMethod("Test_CONV_U1__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_U1__Int16() { Test("Test_CONV_U1__Int16", (short)0); } /* TestMethod("Test_CONV_U1__Int16", (short)0); */
        //[Fact] public void Test_CONV_U1__Int16() { Test("Test_CONV_U1__Int16", (short)-1); } /* TestMethod("Test_CONV_U1__Int16", (short)-1); */
        //[Fact] public void Test_CONV_U1__Int16() { Test("Test_CONV_U1__Int16", (short)1); } /* TestMethod("Test_CONV_U1__Int16", (short)1); */
        //[Fact] public void Test_CONV_U1__Int16() { Test("Test_CONV_U1__Int16", (short)0x1234); } /* TestMethod("Test_CONV_U1__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_U1__Int16() { Test("Test_CONV_U1__Int16", (short)-0x1234); } /* TestMethod("Test_CONV_U1__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_U2__Int16() { Test("Test_CONV_U2__Int16", short.MinValue); } /* TestMethod("Test_CONV_U2__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_U2__Int16() { Test("Test_CONV_U2__Int16", short.MaxValue); } /* TestMethod("Test_CONV_U2__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_U2__Int16() { Test("Test_CONV_U2__Int16", (short)0); } /* TestMethod("Test_CONV_U2__Int16", (short)0); */
        //[Fact] public void Test_CONV_U2__Int16() { Test("Test_CONV_U2__Int16", (short)-1); } /* TestMethod("Test_CONV_U2__Int16", (short)-1); */
        //[Fact] public void Test_CONV_U2__Int16() { Test("Test_CONV_U2__Int16", (short)1); } /* TestMethod("Test_CONV_U2__Int16", (short)1); */
        //[Fact] public void Test_CONV_U2__Int16() { Test("Test_CONV_U2__Int16", (short)0x1234); } /* TestMethod("Test_CONV_U2__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_U2__Int16() { Test("Test_CONV_U2__Int16", (short)-0x1234); } /* TestMethod("Test_CONV_U2__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_U4__Int16() { Test("Test_CONV_U4__Int16", short.MinValue); } /* TestMethod("Test_CONV_U4__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_U4__Int16() { Test("Test_CONV_U4__Int16", short.MaxValue); } /* TestMethod("Test_CONV_U4__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_U4__Int16() { Test("Test_CONV_U4__Int16", (short)0); } /* TestMethod("Test_CONV_U4__Int16", (short)0); */
        //[Fact] public void Test_CONV_U4__Int16() { Test("Test_CONV_U4__Int16", (short)-1); } /* TestMethod("Test_CONV_U4__Int16", (short)-1); */
        //[Fact] public void Test_CONV_U4__Int16() { Test("Test_CONV_U4__Int16", (short)1); } /* TestMethod("Test_CONV_U4__Int16", (short)1); */
        //[Fact] public void Test_CONV_U4__Int16() { Test("Test_CONV_U4__Int16", (short)0x1234); } /* TestMethod("Test_CONV_U4__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_U4__Int16() { Test("Test_CONV_U4__Int16", (short)-0x1234); } /* TestMethod("Test_CONV_U4__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_U8__Int16() { Test("Test_CONV_U8__Int16", short.MinValue); } /* TestMethod("Test_CONV_U8__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_U8__Int16() { Test("Test_CONV_U8__Int16", short.MaxValue); } /* TestMethod("Test_CONV_U8__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_U8__Int16() { Test("Test_CONV_U8__Int16", (short)0); } /* TestMethod("Test_CONV_U8__Int16", (short)0); */
        //[Fact] public void Test_CONV_U8__Int16() { Test("Test_CONV_U8__Int16", (short)-1); } /* TestMethod("Test_CONV_U8__Int16", (short)-1); */
        //[Fact] public void Test_CONV_U8__Int16() { Test("Test_CONV_U8__Int16", (short)1); } /* TestMethod("Test_CONV_U8__Int16", (short)1); */
        //[Fact] public void Test_CONV_U8__Int16() { Test("Test_CONV_U8__Int16", (short)0x1234); } /* TestMethod("Test_CONV_U8__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_U8__Int16() { Test("Test_CONV_U8__Int16", (short)-0x1234); } /* TestMethod("Test_CONV_U8__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_R4__Int16() { Test("Test_CONV_R4__Int16", short.MinValue); } /* TestMethod("Test_CONV_R4__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_R4__Int16() { Test("Test_CONV_R4__Int16", short.MaxValue); } /* TestMethod("Test_CONV_R4__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_R4__Int16() { Test("Test_CONV_R4__Int16", (short)0); } /* TestMethod("Test_CONV_R4__Int16", (short)0); */
        //[Fact] public void Test_CONV_R4__Int16() { Test("Test_CONV_R4__Int16", (short)-1); } /* TestMethod("Test_CONV_R4__Int16", (short)-1); */
        //[Fact] public void Test_CONV_R4__Int16() { Test("Test_CONV_R4__Int16", (short)1); } /* TestMethod("Test_CONV_R4__Int16", (short)1); */
        //[Fact] public void Test_CONV_R4__Int16() { Test("Test_CONV_R4__Int16", (short)0x1234); } /* TestMethod("Test_CONV_R4__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_R4__Int16() { Test("Test_CONV_R4__Int16", (short)-0x1234); } /* TestMethod("Test_CONV_R4__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_R8__Int16() { Test("Test_CONV_R8__Int16", short.MinValue); } /* TestMethod("Test_CONV_R8__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_R8__Int16() { Test("Test_CONV_R8__Int16", short.MaxValue); } /* TestMethod("Test_CONV_R8__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_R8__Int16() { Test("Test_CONV_R8__Int16", (short)0); } /* TestMethod("Test_CONV_R8__Int16", (short)0); */
        //[Fact] public void Test_CONV_R8__Int16() { Test("Test_CONV_R8__Int16", (short)-1); } /* TestMethod("Test_CONV_R8__Int16", (short)-1); */
        //[Fact] public void Test_CONV_R8__Int16() { Test("Test_CONV_R8__Int16", (short)1); } /* TestMethod("Test_CONV_R8__Int16", (short)1); */
        //[Fact] public void Test_CONV_R8__Int16() { Test("Test_CONV_R8__Int16", (short)0x1234); } /* TestMethod("Test_CONV_R8__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_R8__Int16() { Test("Test_CONV_R8__Int16", (short)-0x1234); } /* TestMethod("Test_CONV_R8__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_OVF_I__Int16() { Test("Test_CONV_OVF_I__Int16", short.MinValue); } /* TestMethod("Test_CONV_OVF_I__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_OVF_I__Int16() { Test("Test_CONV_OVF_I__Int16", short.MaxValue); } /* TestMethod("Test_CONV_OVF_I__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I__Int16() { Test("Test_CONV_OVF_I__Int16", (short)0); } /* TestMethod("Test_CONV_OVF_I__Int16", (short)0); */
        //[Fact] public void Test_CONV_OVF_I__Int16() { Test("Test_CONV_OVF_I__Int16", (short)-1); } /* TestMethod("Test_CONV_OVF_I__Int16", (short)-1); */
        //[Fact] public void Test_CONV_OVF_I__Int16() { Test("Test_CONV_OVF_I__Int16", (short)1); } /* TestMethod("Test_CONV_OVF_I__Int16", (short)1); */
        //[Fact] public void Test_CONV_OVF_I__Int16() { Test("Test_CONV_OVF_I__Int16", (short)0x1234); } /* TestMethod("Test_CONV_OVF_I__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_OVF_I__Int16() { Test("Test_CONV_OVF_I__Int16", (short)-0x1234); } /* TestMethod("Test_CONV_OVF_I__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_OVF_I1__Int16() { Test("Test_CONV_OVF_I1__Int16", short.MinValue); } /* TestMethodEX("Test_CONV_OVF_I1__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_OVF_I1__Int16() { Test("Test_CONV_OVF_I1__Int16", short.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I1__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I1__Int16() { Test("Test_CONV_OVF_I1__Int16", (short)0); } /* TestMethod("Test_CONV_OVF_I1__Int16", (short)0); */
        //[Fact] public void Test_CONV_OVF_I1__Int16() { Test("Test_CONV_OVF_I1__Int16", (short)-1); } /* TestMethod("Test_CONV_OVF_I1__Int16", (short)-1); */
        //[Fact] public void Test_CONV_OVF_I1__Int16() { Test("Test_CONV_OVF_I1__Int16", (short)1); } /* TestMethod("Test_CONV_OVF_I1__Int16", (short)1); */
        //[Fact] public void Test_CONV_OVF_I1__Int16() { Test("Test_CONV_OVF_I1__Int16", (short)0x1234); } /* TestMethodEX("Test_CONV_OVF_I1__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_OVF_I1__Int16() { Test("Test_CONV_OVF_I1__Int16", (short)-0x1234); } /* TestMethodEX("Test_CONV_OVF_I1__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_OVF_I2__Int16() { Test("Test_CONV_OVF_I2__Int16", short.MinValue); } /* TestMethod("Test_CONV_OVF_I2__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_OVF_I2__Int16() { Test("Test_CONV_OVF_I2__Int16", short.MaxValue); } /* TestMethod("Test_CONV_OVF_I2__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I2__Int16() { Test("Test_CONV_OVF_I2__Int16", (short)0); } /* TestMethod("Test_CONV_OVF_I2__Int16", (short)0); */
        //[Fact] public void Test_CONV_OVF_I2__Int16() { Test("Test_CONV_OVF_I2__Int16", (short)-1); } /* TestMethod("Test_CONV_OVF_I2__Int16", (short)-1); */
        //[Fact] public void Test_CONV_OVF_I2__Int16() { Test("Test_CONV_OVF_I2__Int16", (short)1); } /* TestMethod("Test_CONV_OVF_I2__Int16", (short)1); */
        //[Fact] public void Test_CONV_OVF_I2__Int16() { Test("Test_CONV_OVF_I2__Int16", (short)0x1234); } /* TestMethod("Test_CONV_OVF_I2__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_OVF_I2__Int16() { Test("Test_CONV_OVF_I2__Int16", (short)-0x1234); } /* TestMethod("Test_CONV_OVF_I2__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_OVF_I4__Int16() { Test("Test_CONV_OVF_I4__Int16", short.MinValue); } /* TestMethod("Test_CONV_OVF_I4__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_OVF_I4__Int16() { Test("Test_CONV_OVF_I4__Int16", short.MaxValue); } /* TestMethod("Test_CONV_OVF_I4__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I4__Int16() { Test("Test_CONV_OVF_I4__Int16", (short)0); } /* TestMethod("Test_CONV_OVF_I4__Int16", (short)0); */
        //[Fact] public void Test_CONV_OVF_I4__Int16() { Test("Test_CONV_OVF_I4__Int16", (short)-1); } /* TestMethod("Test_CONV_OVF_I4__Int16", (short)-1); */
        //[Fact] public void Test_CONV_OVF_I4__Int16() { Test("Test_CONV_OVF_I4__Int16", (short)1); } /* TestMethod("Test_CONV_OVF_I4__Int16", (short)1); */
        //[Fact] public void Test_CONV_OVF_I4__Int16() { Test("Test_CONV_OVF_I4__Int16", (short)0x1234); } /* TestMethod("Test_CONV_OVF_I4__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_OVF_I4__Int16() { Test("Test_CONV_OVF_I4__Int16", (short)-0x1234); } /* TestMethod("Test_CONV_OVF_I4__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_OVF_I8__Int16() { Test("Test_CONV_OVF_I8__Int16", short.MinValue); } /* TestMethod("Test_CONV_OVF_I8__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_OVF_I8__Int16() { Test("Test_CONV_OVF_I8__Int16", short.MaxValue); } /* TestMethod("Test_CONV_OVF_I8__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I8__Int16() { Test("Test_CONV_OVF_I8__Int16", (short)0); } /* TestMethod("Test_CONV_OVF_I8__Int16", (short)0); */
        //[Fact] public void Test_CONV_OVF_I8__Int16() { Test("Test_CONV_OVF_I8__Int16", (short)-1); } /* TestMethod("Test_CONV_OVF_I8__Int16", (short)-1); */
        //[Fact] public void Test_CONV_OVF_I8__Int16() { Test("Test_CONV_OVF_I8__Int16", (short)1); } /* TestMethod("Test_CONV_OVF_I8__Int16", (short)1); */
        //[Fact] public void Test_CONV_OVF_I8__Int16() { Test("Test_CONV_OVF_I8__Int16", (short)0x1234); } /* TestMethod("Test_CONV_OVF_I8__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_OVF_I8__Int16() { Test("Test_CONV_OVF_I8__Int16", (short)-0x1234); } /* TestMethod("Test_CONV_OVF_I8__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_OVF_U__Int16() { Test("Test_CONV_OVF_U__Int16", short.MinValue); } /* TestMethodEX("Test_CONV_OVF_U__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_OVF_U__Int16() { Test("Test_CONV_OVF_U__Int16", short.MaxValue); } /* TestMethod("Test_CONV_OVF_U__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U__Int16() { Test("Test_CONV_OVF_U__Int16", (short)0); } /* TestMethod("Test_CONV_OVF_U__Int16", (short)0); */
        //[Fact] public void Test_CONV_OVF_U__Int16() { Test("Test_CONV_OVF_U__Int16", (short)-1); } /* TestMethodEX("Test_CONV_OVF_U__Int16", (short)-1); */
        //[Fact] public void Test_CONV_OVF_U__Int16() { Test("Test_CONV_OVF_U__Int16", (short)1); } /* TestMethod("Test_CONV_OVF_U__Int16", (short)1); */
        //[Fact] public void Test_CONV_OVF_U__Int16() { Test("Test_CONV_OVF_U__Int16", (short)0x1234); } /* TestMethod("Test_CONV_OVF_U__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_OVF_U__Int16() { Test("Test_CONV_OVF_U__Int16", (short)-0x1234); } /* TestMethodEX("Test_CONV_OVF_U__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_OVF_U1__Int16() { Test("Test_CONV_OVF_U1__Int16", short.MinValue); } /* TestMethodEX("Test_CONV_OVF_U1__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_OVF_U1__Int16() { Test("Test_CONV_OVF_U1__Int16", short.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U1__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U1__Int16() { Test("Test_CONV_OVF_U1__Int16", (short)0); } /* TestMethod("Test_CONV_OVF_U1__Int16", (short)0); */
        //[Fact] public void Test_CONV_OVF_U1__Int16() { Test("Test_CONV_OVF_U1__Int16", (short)-1); } /* TestMethodEX("Test_CONV_OVF_U1__Int16", (short)-1); */
        //[Fact] public void Test_CONV_OVF_U1__Int16() { Test("Test_CONV_OVF_U1__Int16", (short)1); } /* TestMethod("Test_CONV_OVF_U1__Int16", (short)1); */
        //[Fact] public void Test_CONV_OVF_U1__Int16() { Test("Test_CONV_OVF_U1__Int16", (short)0x1234); } /* TestMethodEX("Test_CONV_OVF_U1__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_OVF_U1__Int16() { Test("Test_CONV_OVF_U1__Int16", (short)-0x1234); } /* TestMethodEX("Test_CONV_OVF_U1__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_OVF_U2__Int16() { Test("Test_CONV_OVF_U2__Int16", short.MinValue); } /* TestMethodEX("Test_CONV_OVF_U2__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_OVF_U2__Int16() { Test("Test_CONV_OVF_U2__Int16", short.MaxValue); } /* TestMethod("Test_CONV_OVF_U2__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U2__Int16() { Test("Test_CONV_OVF_U2__Int16", (short)0); } /* TestMethod("Test_CONV_OVF_U2__Int16", (short)0); */
        //[Fact] public void Test_CONV_OVF_U2__Int16() { Test("Test_CONV_OVF_U2__Int16", (short)-1); } /* TestMethodEX("Test_CONV_OVF_U2__Int16", (short)-1); */
        //[Fact] public void Test_CONV_OVF_U2__Int16() { Test("Test_CONV_OVF_U2__Int16", (short)1); } /* TestMethod("Test_CONV_OVF_U2__Int16", (short)1); */
        //[Fact] public void Test_CONV_OVF_U2__Int16() { Test("Test_CONV_OVF_U2__Int16", (short)0x1234); } /* TestMethod("Test_CONV_OVF_U2__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_OVF_U2__Int16() { Test("Test_CONV_OVF_U2__Int16", (short)-0x1234); } /* TestMethodEX("Test_CONV_OVF_U2__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_OVF_U4__Int16() { Test("Test_CONV_OVF_U4__Int16", short.MinValue); } /* TestMethodEX("Test_CONV_OVF_U4__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_OVF_U4__Int16() { Test("Test_CONV_OVF_U4__Int16", short.MaxValue); } /* TestMethod("Test_CONV_OVF_U4__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U4__Int16() { Test("Test_CONV_OVF_U4__Int16", (short)0); } /* TestMethod("Test_CONV_OVF_U4__Int16", (short)0); */
        //[Fact] public void Test_CONV_OVF_U4__Int16() { Test("Test_CONV_OVF_U4__Int16", (short)-1); } /* TestMethodEX("Test_CONV_OVF_U4__Int16", (short)-1); */
        //[Fact] public void Test_CONV_OVF_U4__Int16() { Test("Test_CONV_OVF_U4__Int16", (short)1); } /* TestMethod("Test_CONV_OVF_U4__Int16", (short)1); */
        //[Fact] public void Test_CONV_OVF_U4__Int16() { Test("Test_CONV_OVF_U4__Int16", (short)0x1234); } /* TestMethod("Test_CONV_OVF_U4__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_OVF_U4__Int16() { Test("Test_CONV_OVF_U4__Int16", (short)-0x1234); } /* TestMethodEX("Test_CONV_OVF_U4__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_OVF_U8__Int16() { Test("Test_CONV_OVF_U8__Int16", short.MinValue); } /* TestMethodEX("Test_CONV_OVF_U8__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_OVF_U8__Int16() { Test("Test_CONV_OVF_U8__Int16", short.MaxValue); } /* TestMethod("Test_CONV_OVF_U8__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U8__Int16() { Test("Test_CONV_OVF_U8__Int16", (short)0); } /* TestMethod("Test_CONV_OVF_U8__Int16", (short)0); */
        //[Fact] public void Test_CONV_OVF_U8__Int16() { Test("Test_CONV_OVF_U8__Int16", (short)-1); } /* TestMethodEX("Test_CONV_OVF_U8__Int16", (short)-1); */
        //[Fact] public void Test_CONV_OVF_U8__Int16() { Test("Test_CONV_OVF_U8__Int16", (short)1); } /* TestMethod("Test_CONV_OVF_U8__Int16", (short)1); */
        //[Fact] public void Test_CONV_OVF_U8__Int16() { Test("Test_CONV_OVF_U8__Int16", (short)0x1234); } /* TestMethod("Test_CONV_OVF_U8__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_OVF_U8__Int16() { Test("Test_CONV_OVF_U8__Int16", (short)-0x1234); } /* TestMethodEX("Test_CONV_OVF_U8__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_OVF_I_UN__Int16() { Test("Test_CONV_OVF_I_UN__Int16", short.MinValue); } /* TestMethodEX("Test_CONV_OVF_I_UN__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_OVF_I_UN__Int16() { Test("Test_CONV_OVF_I_UN__Int16", short.MaxValue); } /* TestMethod("Test_CONV_OVF_I_UN__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I_UN__Int16() { Test("Test_CONV_OVF_I_UN__Int16", (short)0); } /* TestMethod("Test_CONV_OVF_I_UN__Int16", (short)0); */
        //[Fact] public void Test_CONV_OVF_I_UN__Int16() { Test("Test_CONV_OVF_I_UN__Int16", (short)-1); } /* TestMethodEX("Test_CONV_OVF_I_UN__Int16", (short)-1); */
        //[Fact] public void Test_CONV_OVF_I_UN__Int16() { Test("Test_CONV_OVF_I_UN__Int16", (short)1); } /* TestMethod("Test_CONV_OVF_I_UN__Int16", (short)1); */
        //[Fact] public void Test_CONV_OVF_I_UN__Int16() { Test("Test_CONV_OVF_I_UN__Int16", (short)0x1234); } /* TestMethod("Test_CONV_OVF_I_UN__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_OVF_I_UN__Int16() { Test("Test_CONV_OVF_I_UN__Int16", (short)-0x1234); } /* TestMethodEX("Test_CONV_OVF_I_UN__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Int16() { Test("Test_CONV_OVF_I1_UN__Int16", short.MinValue); } /* TestMethodEX("Test_CONV_OVF_I1_UN__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Int16() { Test("Test_CONV_OVF_I1_UN__Int16", short.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I1_UN__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Int16() { Test("Test_CONV_OVF_I1_UN__Int16", (short)0); } /* TestMethod("Test_CONV_OVF_I1_UN__Int16", (short)0); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Int16() { Test("Test_CONV_OVF_I1_UN__Int16", (short)-1); } /* TestMethodEX("Test_CONV_OVF_I1_UN__Int16", (short)-1); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Int16() { Test("Test_CONV_OVF_I1_UN__Int16", (short)1); } /* TestMethod("Test_CONV_OVF_I1_UN__Int16", (short)1); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Int16() { Test("Test_CONV_OVF_I1_UN__Int16", (short)0x1234); } /* TestMethodEX("Test_CONV_OVF_I1_UN__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Int16() { Test("Test_CONV_OVF_I1_UN__Int16", (short)-0x1234); } /* TestMethodEX("Test_CONV_OVF_I1_UN__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Int16() { Test("Test_CONV_OVF_I2_UN__Int16", short.MinValue); } /* TestMethodEX("Test_CONV_OVF_I2_UN__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Int16() { Test("Test_CONV_OVF_I2_UN__Int16", short.MaxValue); } /* TestMethod("Test_CONV_OVF_I2_UN__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Int16() { Test("Test_CONV_OVF_I2_UN__Int16", (short)0); } /* TestMethod("Test_CONV_OVF_I2_UN__Int16", (short)0); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Int16() { Test("Test_CONV_OVF_I2_UN__Int16", (short)-1); } /* TestMethodEX("Test_CONV_OVF_I2_UN__Int16", (short)-1); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Int16() { Test("Test_CONV_OVF_I2_UN__Int16", (short)1); } /* TestMethod("Test_CONV_OVF_I2_UN__Int16", (short)1); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Int16() { Test("Test_CONV_OVF_I2_UN__Int16", (short)0x1234); } /* TestMethod("Test_CONV_OVF_I2_UN__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Int16() { Test("Test_CONV_OVF_I2_UN__Int16", (short)-0x1234); } /* TestMethodEX("Test_CONV_OVF_I2_UN__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Int16() { Test("Test_CONV_OVF_I4_UN__Int16", short.MinValue); } /* TestMethodEX("Test_CONV_OVF_I4_UN__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Int16() { Test("Test_CONV_OVF_I4_UN__Int16", short.MaxValue); } /* TestMethod("Test_CONV_OVF_I4_UN__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Int16() { Test("Test_CONV_OVF_I4_UN__Int16", (short)0); } /* TestMethod("Test_CONV_OVF_I4_UN__Int16", (short)0); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Int16() { Test("Test_CONV_OVF_I4_UN__Int16", (short)-1); } /* TestMethodEX("Test_CONV_OVF_I4_UN__Int16", (short)-1); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Int16() { Test("Test_CONV_OVF_I4_UN__Int16", (short)1); } /* TestMethod("Test_CONV_OVF_I4_UN__Int16", (short)1); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Int16() { Test("Test_CONV_OVF_I4_UN__Int16", (short)0x1234); } /* TestMethod("Test_CONV_OVF_I4_UN__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Int16() { Test("Test_CONV_OVF_I4_UN__Int16", (short)-0x1234); } /* TestMethodEX("Test_CONV_OVF_I4_UN__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Int16() { Test("Test_CONV_OVF_I8_UN__Int16", short.MinValue); } /* TestMethod("Test_CONV_OVF_I8_UN__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Int16() { Test("Test_CONV_OVF_I8_UN__Int16", short.MaxValue); } /* TestMethod("Test_CONV_OVF_I8_UN__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Int16() { Test("Test_CONV_OVF_I8_UN__Int16", (short)0); } /* TestMethod("Test_CONV_OVF_I8_UN__Int16", (short)0); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Int16() { Test("Test_CONV_OVF_I8_UN__Int16", (short)-1); } /* TestMethod("Test_CONV_OVF_I8_UN__Int16", (short)-1); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Int16() { Test("Test_CONV_OVF_I8_UN__Int16", (short)1); } /* TestMethod("Test_CONV_OVF_I8_UN__Int16", (short)1); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Int16() { Test("Test_CONV_OVF_I8_UN__Int16", (short)0x1234); } /* TestMethod("Test_CONV_OVF_I8_UN__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Int16() { Test("Test_CONV_OVF_I8_UN__Int16", (short)-0x1234); } /* TestMethod("Test_CONV_OVF_I8_UN__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_OVF_U_UN__Int16() { Test("Test_CONV_OVF_U_UN__Int16", short.MinValue); } /* TestMethod("Test_CONV_OVF_U_UN__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_OVF_U_UN__Int16() { Test("Test_CONV_OVF_U_UN__Int16", short.MaxValue); } /* TestMethod("Test_CONV_OVF_U_UN__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U_UN__Int16() { Test("Test_CONV_OVF_U_UN__Int16", (short)0); } /* TestMethod("Test_CONV_OVF_U_UN__Int16", (short)0); */
        //[Fact] public void Test_CONV_OVF_U_UN__Int16() { Test("Test_CONV_OVF_U_UN__Int16", (short)-1); } /* TestMethod("Test_CONV_OVF_U_UN__Int16", (short)-1); */
        //[Fact] public void Test_CONV_OVF_U_UN__Int16() { Test("Test_CONV_OVF_U_UN__Int16", (short)1); } /* TestMethod("Test_CONV_OVF_U_UN__Int16", (short)1); */
        //[Fact] public void Test_CONV_OVF_U_UN__Int16() { Test("Test_CONV_OVF_U_UN__Int16", (short)0x1234); } /* TestMethod("Test_CONV_OVF_U_UN__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_OVF_U_UN__Int16() { Test("Test_CONV_OVF_U_UN__Int16", (short)-0x1234); } /* TestMethod("Test_CONV_OVF_U_UN__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Int16() { Test("Test_CONV_OVF_U1_UN__Int16", short.MinValue); } /* TestMethodEX("Test_CONV_OVF_U1_UN__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Int16() { Test("Test_CONV_OVF_U1_UN__Int16", short.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U1_UN__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Int16() { Test("Test_CONV_OVF_U1_UN__Int16", (short)0); } /* TestMethod("Test_CONV_OVF_U1_UN__Int16", (short)0); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Int16() { Test("Test_CONV_OVF_U1_UN__Int16", (short)-1); } /* TestMethodEX("Test_CONV_OVF_U1_UN__Int16", (short)-1); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Int16() { Test("Test_CONV_OVF_U1_UN__Int16", (short)1); } /* TestMethod("Test_CONV_OVF_U1_UN__Int16", (short)1); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Int16() { Test("Test_CONV_OVF_U1_UN__Int16", (short)0x1234); } /* TestMethodEX("Test_CONV_OVF_U1_UN__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Int16() { Test("Test_CONV_OVF_U1_UN__Int16", (short)-0x1234); } /* TestMethodEX("Test_CONV_OVF_U1_UN__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Int16() { Test("Test_CONV_OVF_U2_UN__Int16", short.MinValue); } /* TestMethodEX("Test_CONV_OVF_U2_UN__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Int16() { Test("Test_CONV_OVF_U2_UN__Int16", short.MaxValue); } /* TestMethod("Test_CONV_OVF_U2_UN__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Int16() { Test("Test_CONV_OVF_U2_UN__Int16", (short)0); } /* TestMethod("Test_CONV_OVF_U2_UN__Int16", (short)0); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Int16() { Test("Test_CONV_OVF_U2_UN__Int16", (short)-1); } /* TestMethodEX("Test_CONV_OVF_U2_UN__Int16", (short)-1); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Int16() { Test("Test_CONV_OVF_U2_UN__Int16", (short)1); } /* TestMethod("Test_CONV_OVF_U2_UN__Int16", (short)1); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Int16() { Test("Test_CONV_OVF_U2_UN__Int16", (short)0x1234); } /* TestMethod("Test_CONV_OVF_U2_UN__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Int16() { Test("Test_CONV_OVF_U2_UN__Int16", (short)-0x1234); } /* TestMethodEX("Test_CONV_OVF_U2_UN__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Int16() { Test("Test_CONV_OVF_U4_UN__Int16", short.MinValue); } /* TestMethod("Test_CONV_OVF_U4_UN__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Int16() { Test("Test_CONV_OVF_U4_UN__Int16", short.MaxValue); } /* TestMethod("Test_CONV_OVF_U4_UN__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Int16() { Test("Test_CONV_OVF_U4_UN__Int16", (short)0); } /* TestMethod("Test_CONV_OVF_U4_UN__Int16", (short)0); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Int16() { Test("Test_CONV_OVF_U4_UN__Int16", (short)-1); } /* TestMethod("Test_CONV_OVF_U4_UN__Int16", (short)-1); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Int16() { Test("Test_CONV_OVF_U4_UN__Int16", (short)1); } /* TestMethod("Test_CONV_OVF_U4_UN__Int16", (short)1); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Int16() { Test("Test_CONV_OVF_U4_UN__Int16", (short)0x1234); } /* TestMethod("Test_CONV_OVF_U4_UN__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Int16() { Test("Test_CONV_OVF_U4_UN__Int16", (short)-0x1234); } /* TestMethod("Test_CONV_OVF_U4_UN__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Int16() { Test("Test_CONV_OVF_U8_UN__Int16", short.MinValue); } /* TestMethod("Test_CONV_OVF_U8_UN__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Int16() { Test("Test_CONV_OVF_U8_UN__Int16", short.MaxValue); } /* TestMethod("Test_CONV_OVF_U8_UN__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Int16() { Test("Test_CONV_OVF_U8_UN__Int16", (short)0); } /* TestMethod("Test_CONV_OVF_U8_UN__Int16", (short)0); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Int16() { Test("Test_CONV_OVF_U8_UN__Int16", (short)-1); } /* TestMethod("Test_CONV_OVF_U8_UN__Int16", (short)-1); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Int16() { Test("Test_CONV_OVF_U8_UN__Int16", (short)1); } /* TestMethod("Test_CONV_OVF_U8_UN__Int16", (short)1); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Int16() { Test("Test_CONV_OVF_U8_UN__Int16", (short)0x1234); } /* TestMethod("Test_CONV_OVF_U8_UN__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Int16() { Test("Test_CONV_OVF_U8_UN__Int16", (short)-0x1234); } /* TestMethod("Test_CONV_OVF_U8_UN__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_R_UN__Int16() { Test("Test_CONV_R_UN__Int16", short.MinValue); } /* TestMethod("Test_CONV_R_UN__Int16", short.MinValue); */
        //[Fact] public void Test_CONV_R_UN__Int16() { Test("Test_CONV_R_UN__Int16", short.MaxValue); } /* TestMethod("Test_CONV_R_UN__Int16", short.MaxValue); */
        //[Fact] public void Test_CONV_R_UN__Int16() { Test("Test_CONV_R_UN__Int16", (short)0); } /* TestMethod("Test_CONV_R_UN__Int16", (short)0); */
        //[Fact] public void Test_CONV_R_UN__Int16() { Test("Test_CONV_R_UN__Int16", (short)-1); } /* TestMethod("Test_CONV_R_UN__Int16", (short)-1); */
        //[Fact] public void Test_CONV_R_UN__Int16() { Test("Test_CONV_R_UN__Int16", (short)1); } /* TestMethod("Test_CONV_R_UN__Int16", (short)1); */
        //[Fact] public void Test_CONV_R_UN__Int16() { Test("Test_CONV_R_UN__Int16", (short)0x1234); } /* TestMethod("Test_CONV_R_UN__Int16", (short)0x1234); */
        //[Fact] public void Test_CONV_R_UN__Int16() { Test("Test_CONV_R_UN__Int16", (short)-0x1234); } /* TestMethod("Test_CONV_R_UN__Int16", (short)-0x1234); */
        //[Fact] public void Test_CONV_I__Int32() { Test("Test_CONV_I__Int32", int.MinValue); } /* TestMethod("Test_CONV_I__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_I__Int32() { Test("Test_CONV_I__Int32", int.MaxValue); } /* TestMethod("Test_CONV_I__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_I__Int32() { Test("Test_CONV_I__Int32", (int)0); } /* TestMethod("Test_CONV_I__Int32", (int)0); */
        //[Fact] public void Test_CONV_I__Int32() { Test("Test_CONV_I__Int32", (int)-1); } /* TestMethod("Test_CONV_I__Int32", (int)-1); */
        //[Fact] public void Test_CONV_I__Int32() { Test("Test_CONV_I__Int32", (int)1); } /* TestMethod("Test_CONV_I__Int32", (int)1); */
        //[Fact] public void Test_CONV_I__Int32() { Test("Test_CONV_I__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_I__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_I__Int32() { Test("Test_CONV_I__Int32", (int)-0x12345678); } /* TestMethod("Test_CONV_I__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_I1__Int32() { Test("Test_CONV_I1__Int32", int.MinValue); } /* TestMethod("Test_CONV_I1__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_I1__Int32() { Test("Test_CONV_I1__Int32", int.MaxValue); } /* TestMethod("Test_CONV_I1__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_I1__Int32() { Test("Test_CONV_I1__Int32", (int)0); } /* TestMethod("Test_CONV_I1__Int32", (int)0); */
        //[Fact] public void Test_CONV_I1__Int32() { Test("Test_CONV_I1__Int32", (int)-1); } /* TestMethod("Test_CONV_I1__Int32", (int)-1); */
        //[Fact] public void Test_CONV_I1__Int32() { Test("Test_CONV_I1__Int32", (int)1); } /* TestMethod("Test_CONV_I1__Int32", (int)1); */
        //[Fact] public void Test_CONV_I1__Int32() { Test("Test_CONV_I1__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_I1__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_I1__Int32() { Test("Test_CONV_I1__Int32", (int)-0x12345678); } /* TestMethod("Test_CONV_I1__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_I2__Int32() { Test("Test_CONV_I2__Int32", int.MinValue); } /* TestMethod("Test_CONV_I2__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_I2__Int32() { Test("Test_CONV_I2__Int32", int.MaxValue); } /* TestMethod("Test_CONV_I2__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_I2__Int32() { Test("Test_CONV_I2__Int32", (int)0); } /* TestMethod("Test_CONV_I2__Int32", (int)0); */
        //[Fact] public void Test_CONV_I2__Int32() { Test("Test_CONV_I2__Int32", (int)-1); } /* TestMethod("Test_CONV_I2__Int32", (int)-1); */
        //[Fact] public void Test_CONV_I2__Int32() { Test("Test_CONV_I2__Int32", (int)1); } /* TestMethod("Test_CONV_I2__Int32", (int)1); */
        //[Fact] public void Test_CONV_I2__Int32() { Test("Test_CONV_I2__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_I2__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_I2__Int32() { Test("Test_CONV_I2__Int32", (int)-0x12345678); } /* TestMethod("Test_CONV_I2__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_I4__Int32() { Test("Test_CONV_I4__Int32", int.MinValue); } /* TestMethod("Test_CONV_I4__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_I4__Int32() { Test("Test_CONV_I4__Int32", int.MaxValue); } /* TestMethod("Test_CONV_I4__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_I4__Int32() { Test("Test_CONV_I4__Int32", (int)0); } /* TestMethod("Test_CONV_I4__Int32", (int)0); */
        //[Fact] public void Test_CONV_I4__Int32() { Test("Test_CONV_I4__Int32", (int)-1); } /* TestMethod("Test_CONV_I4__Int32", (int)-1); */
        //[Fact] public void Test_CONV_I4__Int32() { Test("Test_CONV_I4__Int32", (int)1); } /* TestMethod("Test_CONV_I4__Int32", (int)1); */
        //[Fact] public void Test_CONV_I4__Int32() { Test("Test_CONV_I4__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_I4__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_I4__Int32() { Test("Test_CONV_I4__Int32", (int)-0x12345678); } /* TestMethod("Test_CONV_I4__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_I8__Int32() { Test("Test_CONV_I8__Int32", int.MinValue); } /* TestMethod("Test_CONV_I8__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_I8__Int32() { Test("Test_CONV_I8__Int32", int.MaxValue); } /* TestMethod("Test_CONV_I8__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_I8__Int32() { Test("Test_CONV_I8__Int32", (int)0); } /* TestMethod("Test_CONV_I8__Int32", (int)0); */
        //[Fact] public void Test_CONV_I8__Int32() { Test("Test_CONV_I8__Int32", (int)-1); } /* TestMethod("Test_CONV_I8__Int32", (int)-1); */
        //[Fact] public void Test_CONV_I8__Int32() { Test("Test_CONV_I8__Int32", (int)1); } /* TestMethod("Test_CONV_I8__Int32", (int)1); */
        //[Fact] public void Test_CONV_I8__Int32() { Test("Test_CONV_I8__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_I8__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_I8__Int32() { Test("Test_CONV_I8__Int32", (int)-0x12345678); } /* TestMethod("Test_CONV_I8__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_U__Int32() { Test("Test_CONV_U__Int32", int.MinValue); } /* TestMethod("Test_CONV_U__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_U__Int32() { Test("Test_CONV_U__Int32", int.MaxValue); } /* TestMethod("Test_CONV_U__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_U__Int32() { Test("Test_CONV_U__Int32", (int)0); } /* TestMethod("Test_CONV_U__Int32", (int)0); */
        //[Fact] public void Test_CONV_U__Int32() { Test("Test_CONV_U__Int32", (int)-1); } /* TestMethod("Test_CONV_U__Int32", (int)-1); */
        //[Fact] public void Test_CONV_U__Int32() { Test("Test_CONV_U__Int32", (int)1); } /* TestMethod("Test_CONV_U__Int32", (int)1); */
        //[Fact] public void Test_CONV_U__Int32() { Test("Test_CONV_U__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_U__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_U__Int32() { Test("Test_CONV_U__Int32", (int)-0x12345678); } /* TestMethod("Test_CONV_U__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_U1__Int32() { Test("Test_CONV_U1__Int32", int.MinValue); } /* TestMethod("Test_CONV_U1__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_U1__Int32() { Test("Test_CONV_U1__Int32", int.MaxValue); } /* TestMethod("Test_CONV_U1__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_U1__Int32() { Test("Test_CONV_U1__Int32", (int)0); } /* TestMethod("Test_CONV_U1__Int32", (int)0); */
        //[Fact] public void Test_CONV_U1__Int32() { Test("Test_CONV_U1__Int32", (int)-1); } /* TestMethod("Test_CONV_U1__Int32", (int)-1); */
        //[Fact] public void Test_CONV_U1__Int32() { Test("Test_CONV_U1__Int32", (int)1); } /* TestMethod("Test_CONV_U1__Int32", (int)1); */
        //[Fact] public void Test_CONV_U1__Int32() { Test("Test_CONV_U1__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_U1__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_U1__Int32() { Test("Test_CONV_U1__Int32", (int)-0x12345678); } /* TestMethod("Test_CONV_U1__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_U2__Int32() { Test("Test_CONV_U2__Int32", int.MinValue); } /* TestMethod("Test_CONV_U2__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_U2__Int32() { Test("Test_CONV_U2__Int32", int.MaxValue); } /* TestMethod("Test_CONV_U2__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_U2__Int32() { Test("Test_CONV_U2__Int32", (int)0); } /* TestMethod("Test_CONV_U2__Int32", (int)0); */
        //[Fact] public void Test_CONV_U2__Int32() { Test("Test_CONV_U2__Int32", (int)-1); } /* TestMethod("Test_CONV_U2__Int32", (int)-1); */
        //[Fact] public void Test_CONV_U2__Int32() { Test("Test_CONV_U2__Int32", (int)1); } /* TestMethod("Test_CONV_U2__Int32", (int)1); */
        //[Fact] public void Test_CONV_U2__Int32() { Test("Test_CONV_U2__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_U2__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_U2__Int32() { Test("Test_CONV_U2__Int32", (int)-0x12345678); } /* TestMethod("Test_CONV_U2__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_U4__Int32() { Test("Test_CONV_U4__Int32", int.MinValue); } /* TestMethod("Test_CONV_U4__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_U4__Int32() { Test("Test_CONV_U4__Int32", int.MaxValue); } /* TestMethod("Test_CONV_U4__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_U4__Int32() { Test("Test_CONV_U4__Int32", (int)0); } /* TestMethod("Test_CONV_U4__Int32", (int)0); */
        //[Fact] public void Test_CONV_U4__Int32() { Test("Test_CONV_U4__Int32", (int)-1); } /* TestMethod("Test_CONV_U4__Int32", (int)-1); */
        //[Fact] public void Test_CONV_U4__Int32() { Test("Test_CONV_U4__Int32", (int)1); } /* TestMethod("Test_CONV_U4__Int32", (int)1); */
        //[Fact] public void Test_CONV_U4__Int32() { Test("Test_CONV_U4__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_U4__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_U4__Int32() { Test("Test_CONV_U4__Int32", (int)-0x12345678); } /* TestMethod("Test_CONV_U4__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_U8__Int32() { Test("Test_CONV_U8__Int32", int.MinValue); } /* TestMethod("Test_CONV_U8__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_U8__Int32() { Test("Test_CONV_U8__Int32", int.MaxValue); } /* TestMethod("Test_CONV_U8__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_U8__Int32() { Test("Test_CONV_U8__Int32", (int)0); } /* TestMethod("Test_CONV_U8__Int32", (int)0); */
        //[Fact] public void Test_CONV_U8__Int32() { Test("Test_CONV_U8__Int32", (int)-1); } /* TestMethod("Test_CONV_U8__Int32", (int)-1); */
        //[Fact] public void Test_CONV_U8__Int32() { Test("Test_CONV_U8__Int32", (int)1); } /* TestMethod("Test_CONV_U8__Int32", (int)1); */
        //[Fact] public void Test_CONV_U8__Int32() { Test("Test_CONV_U8__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_U8__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_U8__Int32() { Test("Test_CONV_U8__Int32", (int)-0x12345678); } /* TestMethod("Test_CONV_U8__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_R4__Int32() { Test("Test_CONV_R4__Int32", int.MinValue); } /* TestMethod("Test_CONV_R4__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_R4__Int32() { Test("Test_CONV_R4__Int32", int.MaxValue); } /* TestMethod("Test_CONV_R4__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_R4__Int32() { Test("Test_CONV_R4__Int32", (int)0); } /* TestMethod("Test_CONV_R4__Int32", (int)0); */
        //[Fact] public void Test_CONV_R4__Int32() { Test("Test_CONV_R4__Int32", (int)-1); } /* TestMethod("Test_CONV_R4__Int32", (int)-1); */
        //[Fact] public void Test_CONV_R4__Int32() { Test("Test_CONV_R4__Int32", (int)1); } /* TestMethod("Test_CONV_R4__Int32", (int)1); */
        //[Fact] public void Test_CONV_R4__Int32() { Test("Test_CONV_R4__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_R4__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_R4__Int32() { Test("Test_CONV_R4__Int32", (int)-0x12345678); } /* TestMethod("Test_CONV_R4__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_R8__Int32() { Test("Test_CONV_R8__Int32", int.MinValue); } /* TestMethod("Test_CONV_R8__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_R8__Int32() { Test("Test_CONV_R8__Int32", int.MaxValue); } /* TestMethod("Test_CONV_R8__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_R8__Int32() { Test("Test_CONV_R8__Int32", (int)0); } /* TestMethod("Test_CONV_R8__Int32", (int)0); */
        //[Fact] public void Test_CONV_R8__Int32() { Test("Test_CONV_R8__Int32", (int)-1); } /* TestMethod("Test_CONV_R8__Int32", (int)-1); */
        //[Fact] public void Test_CONV_R8__Int32() { Test("Test_CONV_R8__Int32", (int)1); } /* TestMethod("Test_CONV_R8__Int32", (int)1); */
        //[Fact] public void Test_CONV_R8__Int32() { Test("Test_CONV_R8__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_R8__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_R8__Int32() { Test("Test_CONV_R8__Int32", (int)-0x12345678); } /* TestMethod("Test_CONV_R8__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_OVF_I__Int32() { Test("Test_CONV_OVF_I__Int32", int.MinValue); } /* TestMethod("Test_CONV_OVF_I__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_OVF_I__Int32() { Test("Test_CONV_OVF_I__Int32", int.MaxValue); } /* TestMethod("Test_CONV_OVF_I__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I__Int32() { Test("Test_CONV_OVF_I__Int32", (int)0); } /* TestMethod("Test_CONV_OVF_I__Int32", (int)0); */
        //[Fact] public void Test_CONV_OVF_I__Int32() { Test("Test_CONV_OVF_I__Int32", (int)-1); } /* TestMethod("Test_CONV_OVF_I__Int32", (int)-1); */
        //[Fact] public void Test_CONV_OVF_I__Int32() { Test("Test_CONV_OVF_I__Int32", (int)1); } /* TestMethod("Test_CONV_OVF_I__Int32", (int)1); */
        //[Fact] public void Test_CONV_OVF_I__Int32() { Test("Test_CONV_OVF_I__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_OVF_I__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_OVF_I__Int32() { Test("Test_CONV_OVF_I__Int32", (int)-0x12345678); } /* TestMethod("Test_CONV_OVF_I__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_OVF_I1__Int32() { Test("Test_CONV_OVF_I1__Int32", int.MinValue); } /* TestMethodEX("Test_CONV_OVF_I1__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_OVF_I1__Int32() { Test("Test_CONV_OVF_I1__Int32", int.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I1__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I1__Int32() { Test("Test_CONV_OVF_I1__Int32", (int)0); } /* TestMethod("Test_CONV_OVF_I1__Int32", (int)0); */
        //[Fact] public void Test_CONV_OVF_I1__Int32() { Test("Test_CONV_OVF_I1__Int32", (int)-1); } /* TestMethod("Test_CONV_OVF_I1__Int32", (int)-1); */
        //[Fact] public void Test_CONV_OVF_I1__Int32() { Test("Test_CONV_OVF_I1__Int32", (int)1); } /* TestMethod("Test_CONV_OVF_I1__Int32", (int)1); */
        //[Fact] public void Test_CONV_OVF_I1__Int32() { Test("Test_CONV_OVF_I1__Int32", (int)0x12345678); } /* TestMethodEX("Test_CONV_OVF_I1__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_OVF_I1__Int32() { Test("Test_CONV_OVF_I1__Int32", (int)-0x12345678); } /* TestMethodEX("Test_CONV_OVF_I1__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_OVF_I2__Int32() { Test("Test_CONV_OVF_I2__Int32", int.MinValue); } /* TestMethodEX("Test_CONV_OVF_I2__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_OVF_I2__Int32() { Test("Test_CONV_OVF_I2__Int32", int.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I2__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I2__Int32() { Test("Test_CONV_OVF_I2__Int32", (int)0); } /* TestMethod("Test_CONV_OVF_I2__Int32", (int)0); */
        //[Fact] public void Test_CONV_OVF_I2__Int32() { Test("Test_CONV_OVF_I2__Int32", (int)-1); } /* TestMethod("Test_CONV_OVF_I2__Int32", (int)-1); */
        //[Fact] public void Test_CONV_OVF_I2__Int32() { Test("Test_CONV_OVF_I2__Int32", (int)1); } /* TestMethod("Test_CONV_OVF_I2__Int32", (int)1); */
        //[Fact] public void Test_CONV_OVF_I2__Int32() { Test("Test_CONV_OVF_I2__Int32", (int)0x12345678); } /* TestMethodEX("Test_CONV_OVF_I2__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_OVF_I2__Int32() { Test("Test_CONV_OVF_I2__Int32", (int)-0x12345678); } /* TestMethodEX("Test_CONV_OVF_I2__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_OVF_I4__Int32() { Test("Test_CONV_OVF_I4__Int32", int.MinValue); } /* TestMethod("Test_CONV_OVF_I4__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_OVF_I4__Int32() { Test("Test_CONV_OVF_I4__Int32", int.MaxValue); } /* TestMethod("Test_CONV_OVF_I4__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I4__Int32() { Test("Test_CONV_OVF_I4__Int32", (int)0); } /* TestMethod("Test_CONV_OVF_I4__Int32", (int)0); */
        //[Fact] public void Test_CONV_OVF_I4__Int32() { Test("Test_CONV_OVF_I4__Int32", (int)-1); } /* TestMethod("Test_CONV_OVF_I4__Int32", (int)-1); */
        //[Fact] public void Test_CONV_OVF_I4__Int32() { Test("Test_CONV_OVF_I4__Int32", (int)1); } /* TestMethod("Test_CONV_OVF_I4__Int32", (int)1); */
        //[Fact] public void Test_CONV_OVF_I4__Int32() { Test("Test_CONV_OVF_I4__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_OVF_I4__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_OVF_I4__Int32() { Test("Test_CONV_OVF_I4__Int32", (int)-0x12345678); } /* TestMethod("Test_CONV_OVF_I4__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_OVF_I8__Int32() { Test("Test_CONV_OVF_I8__Int32", int.MinValue); } /* TestMethod("Test_CONV_OVF_I8__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_OVF_I8__Int32() { Test("Test_CONV_OVF_I8__Int32", int.MaxValue); } /* TestMethod("Test_CONV_OVF_I8__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I8__Int32() { Test("Test_CONV_OVF_I8__Int32", (int)0); } /* TestMethod("Test_CONV_OVF_I8__Int32", (int)0); */
        //[Fact] public void Test_CONV_OVF_I8__Int32() { Test("Test_CONV_OVF_I8__Int32", (int)-1); } /* TestMethod("Test_CONV_OVF_I8__Int32", (int)-1); */
        //[Fact] public void Test_CONV_OVF_I8__Int32() { Test("Test_CONV_OVF_I8__Int32", (int)1); } /* TestMethod("Test_CONV_OVF_I8__Int32", (int)1); */
        //[Fact] public void Test_CONV_OVF_I8__Int32() { Test("Test_CONV_OVF_I8__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_OVF_I8__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_OVF_I8__Int32() { Test("Test_CONV_OVF_I8__Int32", (int)-0x12345678); } /* TestMethod("Test_CONV_OVF_I8__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_OVF_U__Int32() { Test("Test_CONV_OVF_U__Int32", int.MinValue); } /* TestMethodEX("Test_CONV_OVF_U__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_OVF_U__Int32() { Test("Test_CONV_OVF_U__Int32", int.MaxValue); } /* TestMethod("Test_CONV_OVF_U__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U__Int32() { Test("Test_CONV_OVF_U__Int32", (int)0); } /* TestMethod("Test_CONV_OVF_U__Int32", (int)0); */
        //[Fact] public void Test_CONV_OVF_U__Int32() { Test("Test_CONV_OVF_U__Int32", (int)-1); } /* TestMethodEX("Test_CONV_OVF_U__Int32", (int)-1); */
        //[Fact] public void Test_CONV_OVF_U__Int32() { Test("Test_CONV_OVF_U__Int32", (int)1); } /* TestMethod("Test_CONV_OVF_U__Int32", (int)1); */
        //[Fact] public void Test_CONV_OVF_U__Int32() { Test("Test_CONV_OVF_U__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_OVF_U__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_OVF_U__Int32() { Test("Test_CONV_OVF_U__Int32", (int)-0x12345678); } /* TestMethodEX("Test_CONV_OVF_U__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_OVF_U1__Int32() { Test("Test_CONV_OVF_U1__Int32", int.MinValue); } /* TestMethodEX("Test_CONV_OVF_U1__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_OVF_U1__Int32() { Test("Test_CONV_OVF_U1__Int32", int.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U1__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U1__Int32() { Test("Test_CONV_OVF_U1__Int32", (int)0); } /* TestMethod("Test_CONV_OVF_U1__Int32", (int)0); */
        //[Fact] public void Test_CONV_OVF_U1__Int32() { Test("Test_CONV_OVF_U1__Int32", (int)-1); } /* TestMethodEX("Test_CONV_OVF_U1__Int32", (int)-1); */
        //[Fact] public void Test_CONV_OVF_U1__Int32() { Test("Test_CONV_OVF_U1__Int32", (int)1); } /* TestMethod("Test_CONV_OVF_U1__Int32", (int)1); */
        //[Fact] public void Test_CONV_OVF_U1__Int32() { Test("Test_CONV_OVF_U1__Int32", (int)0x12345678); } /* TestMethodEX("Test_CONV_OVF_U1__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_OVF_U1__Int32() { Test("Test_CONV_OVF_U1__Int32", (int)-0x12345678); } /* TestMethodEX("Test_CONV_OVF_U1__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_OVF_U2__Int32() { Test("Test_CONV_OVF_U2__Int32", int.MinValue); } /* TestMethodEX("Test_CONV_OVF_U2__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_OVF_U2__Int32() { Test("Test_CONV_OVF_U2__Int32", int.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U2__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U2__Int32() { Test("Test_CONV_OVF_U2__Int32", (int)0); } /* TestMethod("Test_CONV_OVF_U2__Int32", (int)0); */
        //[Fact] public void Test_CONV_OVF_U2__Int32() { Test("Test_CONV_OVF_U2__Int32", (int)-1); } /* TestMethodEX("Test_CONV_OVF_U2__Int32", (int)-1); */
        //[Fact] public void Test_CONV_OVF_U2__Int32() { Test("Test_CONV_OVF_U2__Int32", (int)1); } /* TestMethod("Test_CONV_OVF_U2__Int32", (int)1); */
        //[Fact] public void Test_CONV_OVF_U2__Int32() { Test("Test_CONV_OVF_U2__Int32", (int)0x12345678); } /* TestMethodEX("Test_CONV_OVF_U2__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_OVF_U2__Int32() { Test("Test_CONV_OVF_U2__Int32", (int)-0x12345678); } /* TestMethodEX("Test_CONV_OVF_U2__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_OVF_U4__Int32() { Test("Test_CONV_OVF_U4__Int32", int.MinValue); } /* TestMethodEX("Test_CONV_OVF_U4__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_OVF_U4__Int32() { Test("Test_CONV_OVF_U4__Int32", int.MaxValue); } /* TestMethod("Test_CONV_OVF_U4__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U4__Int32() { Test("Test_CONV_OVF_U4__Int32", (int)0); } /* TestMethod("Test_CONV_OVF_U4__Int32", (int)0); */
        //[Fact] public void Test_CONV_OVF_U4__Int32() { Test("Test_CONV_OVF_U4__Int32", (int)-1); } /* TestMethodEX("Test_CONV_OVF_U4__Int32", (int)-1); */
        //[Fact] public void Test_CONV_OVF_U4__Int32() { Test("Test_CONV_OVF_U4__Int32", (int)1); } /* TestMethod("Test_CONV_OVF_U4__Int32", (int)1); */
        //[Fact] public void Test_CONV_OVF_U4__Int32() { Test("Test_CONV_OVF_U4__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_OVF_U4__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_OVF_U4__Int32() { Test("Test_CONV_OVF_U4__Int32", (int)-0x12345678); } /* TestMethodEX("Test_CONV_OVF_U4__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_OVF_U8__Int32() { Test("Test_CONV_OVF_U8__Int32", int.MinValue); } /* TestMethodEX("Test_CONV_OVF_U8__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_OVF_U8__Int32() { Test("Test_CONV_OVF_U8__Int32", int.MaxValue); } /* TestMethod("Test_CONV_OVF_U8__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U8__Int32() { Test("Test_CONV_OVF_U8__Int32", (int)0); } /* TestMethod("Test_CONV_OVF_U8__Int32", (int)0); */
        //[Fact] public void Test_CONV_OVF_U8__Int32() { Test("Test_CONV_OVF_U8__Int32", (int)-1); } /* TestMethodEX("Test_CONV_OVF_U8__Int32", (int)-1); */
        //[Fact] public void Test_CONV_OVF_U8__Int32() { Test("Test_CONV_OVF_U8__Int32", (int)1); } /* TestMethod("Test_CONV_OVF_U8__Int32", (int)1); */
        //[Fact] public void Test_CONV_OVF_U8__Int32() { Test("Test_CONV_OVF_U8__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_OVF_U8__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_OVF_U8__Int32() { Test("Test_CONV_OVF_U8__Int32", (int)-0x12345678); } /* TestMethodEX("Test_CONV_OVF_U8__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_OVF_I_UN__Int32() { Test("Test_CONV_OVF_I_UN__Int32", int.MinValue); } /* TestMethodEX("Test_CONV_OVF_I_UN__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_OVF_I_UN__Int32() { Test("Test_CONV_OVF_I_UN__Int32", int.MaxValue); } /* TestMethod("Test_CONV_OVF_I_UN__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I_UN__Int32() { Test("Test_CONV_OVF_I_UN__Int32", (int)0); } /* TestMethod("Test_CONV_OVF_I_UN__Int32", (int)0); */
        //[Fact] public void Test_CONV_OVF_I_UN__Int32() { Test("Test_CONV_OVF_I_UN__Int32", (int)-1); } /* TestMethodEX("Test_CONV_OVF_I_UN__Int32", (int)-1); */
        //[Fact] public void Test_CONV_OVF_I_UN__Int32() { Test("Test_CONV_OVF_I_UN__Int32", (int)1); } /* TestMethod("Test_CONV_OVF_I_UN__Int32", (int)1); */
        //[Fact] public void Test_CONV_OVF_I_UN__Int32() { Test("Test_CONV_OVF_I_UN__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_OVF_I_UN__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_OVF_I_UN__Int32() { Test("Test_CONV_OVF_I_UN__Int32", (int)-0x12345678); } /* TestMethodEX("Test_CONV_OVF_I_UN__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Int32() { Test("Test_CONV_OVF_I1_UN__Int32", int.MinValue); } /* TestMethodEX("Test_CONV_OVF_I1_UN__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Int32() { Test("Test_CONV_OVF_I1_UN__Int32", int.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I1_UN__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Int32() { Test("Test_CONV_OVF_I1_UN__Int32", (int)0); } /* TestMethod("Test_CONV_OVF_I1_UN__Int32", (int)0); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Int32() { Test("Test_CONV_OVF_I1_UN__Int32", (int)-1); } /* TestMethodEX("Test_CONV_OVF_I1_UN__Int32", (int)-1); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Int32() { Test("Test_CONV_OVF_I1_UN__Int32", (int)1); } /* TestMethod("Test_CONV_OVF_I1_UN__Int32", (int)1); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Int32() { Test("Test_CONV_OVF_I1_UN__Int32", (int)0x12345678); } /* TestMethodEX("Test_CONV_OVF_I1_UN__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Int32() { Test("Test_CONV_OVF_I1_UN__Int32", (int)-0x12345678); } /* TestMethodEX("Test_CONV_OVF_I1_UN__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Int32() { Test("Test_CONV_OVF_I2_UN__Int32", int.MinValue); } /* TestMethodEX("Test_CONV_OVF_I2_UN__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Int32() { Test("Test_CONV_OVF_I2_UN__Int32", int.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I2_UN__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Int32() { Test("Test_CONV_OVF_I2_UN__Int32", (int)0); } /* TestMethod("Test_CONV_OVF_I2_UN__Int32", (int)0); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Int32() { Test("Test_CONV_OVF_I2_UN__Int32", (int)-1); } /* TestMethodEX("Test_CONV_OVF_I2_UN__Int32", (int)-1); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Int32() { Test("Test_CONV_OVF_I2_UN__Int32", (int)1); } /* TestMethod("Test_CONV_OVF_I2_UN__Int32", (int)1); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Int32() { Test("Test_CONV_OVF_I2_UN__Int32", (int)0x12345678); } /* TestMethodEX("Test_CONV_OVF_I2_UN__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Int32() { Test("Test_CONV_OVF_I2_UN__Int32", (int)-0x12345678); } /* TestMethodEX("Test_CONV_OVF_I2_UN__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Int32() { Test("Test_CONV_OVF_I4_UN__Int32", int.MinValue); } /* TestMethodEX("Test_CONV_OVF_I4_UN__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Int32() { Test("Test_CONV_OVF_I4_UN__Int32", int.MaxValue); } /* TestMethod("Test_CONV_OVF_I4_UN__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Int32() { Test("Test_CONV_OVF_I4_UN__Int32", (int)0); } /* TestMethod("Test_CONV_OVF_I4_UN__Int32", (int)0); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Int32() { Test("Test_CONV_OVF_I4_UN__Int32", (int)-1); } /* TestMethodEX("Test_CONV_OVF_I4_UN__Int32", (int)-1); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Int32() { Test("Test_CONV_OVF_I4_UN__Int32", (int)1); } /* TestMethod("Test_CONV_OVF_I4_UN__Int32", (int)1); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Int32() { Test("Test_CONV_OVF_I4_UN__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_OVF_I4_UN__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Int32() { Test("Test_CONV_OVF_I4_UN__Int32", (int)-0x12345678); } /* TestMethodEX("Test_CONV_OVF_I4_UN__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Int32() { Test("Test_CONV_OVF_I8_UN__Int32", int.MinValue); } /* TestMethod("Test_CONV_OVF_I8_UN__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Int32() { Test("Test_CONV_OVF_I8_UN__Int32", int.MaxValue); } /* TestMethod("Test_CONV_OVF_I8_UN__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Int32() { Test("Test_CONV_OVF_I8_UN__Int32", (int)0); } /* TestMethod("Test_CONV_OVF_I8_UN__Int32", (int)0); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Int32() { Test("Test_CONV_OVF_I8_UN__Int32", (int)-1); } /* TestMethod("Test_CONV_OVF_I8_UN__Int32", (int)-1); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Int32() { Test("Test_CONV_OVF_I8_UN__Int32", (int)1); } /* TestMethod("Test_CONV_OVF_I8_UN__Int32", (int)1); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Int32() { Test("Test_CONV_OVF_I8_UN__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_OVF_I8_UN__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Int32() { Test("Test_CONV_OVF_I8_UN__Int32", (int)-0x12345678); } /* TestMethod("Test_CONV_OVF_I8_UN__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_OVF_U_UN__Int32() { Test("Test_CONV_OVF_U_UN__Int32", int.MinValue); } /* TestMethod("Test_CONV_OVF_U_UN__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_OVF_U_UN__Int32() { Test("Test_CONV_OVF_U_UN__Int32", int.MaxValue); } /* TestMethod("Test_CONV_OVF_U_UN__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U_UN__Int32() { Test("Test_CONV_OVF_U_UN__Int32", (int)0); } /* TestMethod("Test_CONV_OVF_U_UN__Int32", (int)0); */
        //[Fact] public void Test_CONV_OVF_U_UN__Int32() { Test("Test_CONV_OVF_U_UN__Int32", (int)-1); } /* TestMethod("Test_CONV_OVF_U_UN__Int32", (int)-1); */
        //[Fact] public void Test_CONV_OVF_U_UN__Int32() { Test("Test_CONV_OVF_U_UN__Int32", (int)1); } /* TestMethod("Test_CONV_OVF_U_UN__Int32", (int)1); */
        //[Fact] public void Test_CONV_OVF_U_UN__Int32() { Test("Test_CONV_OVF_U_UN__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_OVF_U_UN__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_OVF_U_UN__Int32() { Test("Test_CONV_OVF_U_UN__Int32", (int)-0x12345678); } /* TestMethod("Test_CONV_OVF_U_UN__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Int32() { Test("Test_CONV_OVF_U1_UN__Int32", int.MinValue); } /* TestMethodEX("Test_CONV_OVF_U1_UN__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Int32() { Test("Test_CONV_OVF_U1_UN__Int32", int.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U1_UN__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Int32() { Test("Test_CONV_OVF_U1_UN__Int32", (int)0); } /* TestMethod("Test_CONV_OVF_U1_UN__Int32", (int)0); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Int32() { Test("Test_CONV_OVF_U1_UN__Int32", (int)-1); } /* TestMethodEX("Test_CONV_OVF_U1_UN__Int32", (int)-1); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Int32() { Test("Test_CONV_OVF_U1_UN__Int32", (int)1); } /* TestMethod("Test_CONV_OVF_U1_UN__Int32", (int)1); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Int32() { Test("Test_CONV_OVF_U1_UN__Int32", (int)0x12345678); } /* TestMethodEX("Test_CONV_OVF_U1_UN__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Int32() { Test("Test_CONV_OVF_U1_UN__Int32", (int)-0x12345678); } /* TestMethodEX("Test_CONV_OVF_U1_UN__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Int32() { Test("Test_CONV_OVF_U2_UN__Int32", int.MinValue); } /* TestMethodEX("Test_CONV_OVF_U2_UN__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Int32() { Test("Test_CONV_OVF_U2_UN__Int32", int.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U2_UN__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Int32() { Test("Test_CONV_OVF_U2_UN__Int32", (int)0); } /* TestMethod("Test_CONV_OVF_U2_UN__Int32", (int)0); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Int32() { Test("Test_CONV_OVF_U2_UN__Int32", (int)-1); } /* TestMethodEX("Test_CONV_OVF_U2_UN__Int32", (int)-1); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Int32() { Test("Test_CONV_OVF_U2_UN__Int32", (int)1); } /* TestMethod("Test_CONV_OVF_U2_UN__Int32", (int)1); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Int32() { Test("Test_CONV_OVF_U2_UN__Int32", (int)0x12345678); } /* TestMethodEX("Test_CONV_OVF_U2_UN__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Int32() { Test("Test_CONV_OVF_U2_UN__Int32", (int)-0x12345678); } /* TestMethodEX("Test_CONV_OVF_U2_UN__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Int32() { Test("Test_CONV_OVF_U4_UN__Int32", int.MinValue); } /* TestMethod("Test_CONV_OVF_U4_UN__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Int32() { Test("Test_CONV_OVF_U4_UN__Int32", int.MaxValue); } /* TestMethod("Test_CONV_OVF_U4_UN__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Int32() { Test("Test_CONV_OVF_U4_UN__Int32", (int)0); } /* TestMethod("Test_CONV_OVF_U4_UN__Int32", (int)0); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Int32() { Test("Test_CONV_OVF_U4_UN__Int32", (int)-1); } /* TestMethod("Test_CONV_OVF_U4_UN__Int32", (int)-1); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Int32() { Test("Test_CONV_OVF_U4_UN__Int32", (int)1); } /* TestMethod("Test_CONV_OVF_U4_UN__Int32", (int)1); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Int32() { Test("Test_CONV_OVF_U4_UN__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_OVF_U4_UN__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Int32() { Test("Test_CONV_OVF_U4_UN__Int32", (int)-0x12345678); } /* TestMethod("Test_CONV_OVF_U4_UN__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Int32() { Test("Test_CONV_OVF_U8_UN__Int32", int.MinValue); } /* TestMethod("Test_CONV_OVF_U8_UN__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Int32() { Test("Test_CONV_OVF_U8_UN__Int32", int.MaxValue); } /* TestMethod("Test_CONV_OVF_U8_UN__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Int32() { Test("Test_CONV_OVF_U8_UN__Int32", (int)0); } /* TestMethod("Test_CONV_OVF_U8_UN__Int32", (int)0); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Int32() { Test("Test_CONV_OVF_U8_UN__Int32", (int)-1); } /* TestMethod("Test_CONV_OVF_U8_UN__Int32", (int)-1); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Int32() { Test("Test_CONV_OVF_U8_UN__Int32", (int)1); } /* TestMethod("Test_CONV_OVF_U8_UN__Int32", (int)1); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Int32() { Test("Test_CONV_OVF_U8_UN__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_OVF_U8_UN__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Int32() { Test("Test_CONV_OVF_U8_UN__Int32", (int)-0x12345678); } /* TestMethod("Test_CONV_OVF_U8_UN__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_R_UN__Int32() { Test("Test_CONV_R_UN__Int32", int.MinValue); } /* TestMethod("Test_CONV_R_UN__Int32", int.MinValue); */
        //[Fact] public void Test_CONV_R_UN__Int32() { Test("Test_CONV_R_UN__Int32", int.MaxValue); } /* TestMethod("Test_CONV_R_UN__Int32", int.MaxValue); */
        //[Fact] public void Test_CONV_R_UN__Int32() { Test("Test_CONV_R_UN__Int32", (int)0); } /* TestMethod("Test_CONV_R_UN__Int32", (int)0); */
        //[Fact] public void Test_CONV_R_UN__Int32() { Test("Test_CONV_R_UN__Int32", (int)-1); } /* TestMethod("Test_CONV_R_UN__Int32", (int)-1); */
        //[Fact] public void Test_CONV_R_UN__Int32() { Test("Test_CONV_R_UN__Int32", (int)1); } /* TestMethod("Test_CONV_R_UN__Int32", (int)1); */
        //[Fact] public void Test_CONV_R_UN__Int32() { Test("Test_CONV_R_UN__Int32", (int)0x12345678); } /* TestMethod("Test_CONV_R_UN__Int32", (int)0x12345678); */
        //[Fact] public void Test_CONV_R_UN__Int32() { Test("Test_CONV_R_UN__Int32", (int)-0x12345678); } /* TestMethod("Test_CONV_R_UN__Int32", (int)-0x12345678); */
        //[Fact] public void Test_CONV_I__Int64() { Test("Test_CONV_I__Int64", long.MinValue); } /* TestMethod("Test_CONV_I__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_I__Int64() { Test("Test_CONV_I__Int64", long.MaxValue); } /* TestMethod("Test_CONV_I__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_I__Int64() { Test("Test_CONV_I__Int64", (long)0); } /* TestMethod("Test_CONV_I__Int64", (long)0); */
        //[Fact] public void Test_CONV_I__Int64() { Test("Test_CONV_I__Int64", (long)-1); } /* TestMethod("Test_CONV_I__Int64", (long)-1); */
        //[Fact] public void Test_CONV_I__Int64() { Test("Test_CONV_I__Int64", (long)1); } /* TestMethod("Test_CONV_I__Int64", (long)1); */
        //[Fact] public void Test_CONV_I__Int64() { Test("Test_CONV_I__Int64", (long)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_I__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_I__Int64() { Test("Test_CONV_I__Int64", (long)-0x123456789ABCDEF0); } /* TestMethod("Test_CONV_I__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_I1__Int64() { Test("Test_CONV_I1__Int64", long.MinValue); } /* TestMethod("Test_CONV_I1__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_I1__Int64() { Test("Test_CONV_I1__Int64", long.MaxValue); } /* TestMethod("Test_CONV_I1__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_I1__Int64() { Test("Test_CONV_I1__Int64", (long)0); } /* TestMethod("Test_CONV_I1__Int64", (long)0); */
        //[Fact] public void Test_CONV_I1__Int64() { Test("Test_CONV_I1__Int64", (long)-1); } /* TestMethod("Test_CONV_I1__Int64", (long)-1); */
        //[Fact] public void Test_CONV_I1__Int64() { Test("Test_CONV_I1__Int64", (long)1); } /* TestMethod("Test_CONV_I1__Int64", (long)1); */
        //[Fact] public void Test_CONV_I1__Int64() { Test("Test_CONV_I1__Int64", (long)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_I1__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_I1__Int64() { Test("Test_CONV_I1__Int64", (long)-0x123456789ABCDEF0); } /* TestMethod("Test_CONV_I1__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_I2__Int64() { Test("Test_CONV_I2__Int64", long.MinValue); } /* TestMethod("Test_CONV_I2__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_I2__Int64() { Test("Test_CONV_I2__Int64", long.MaxValue); } /* TestMethod("Test_CONV_I2__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_I2__Int64() { Test("Test_CONV_I2__Int64", (long)0); } /* TestMethod("Test_CONV_I2__Int64", (long)0); */
        //[Fact] public void Test_CONV_I2__Int64() { Test("Test_CONV_I2__Int64", (long)-1); } /* TestMethod("Test_CONV_I2__Int64", (long)-1); */
        //[Fact] public void Test_CONV_I2__Int64() { Test("Test_CONV_I2__Int64", (long)1); } /* TestMethod("Test_CONV_I2__Int64", (long)1); */
        //[Fact] public void Test_CONV_I2__Int64() { Test("Test_CONV_I2__Int64", (long)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_I2__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_I2__Int64() { Test("Test_CONV_I2__Int64", (long)-0x123456789ABCDEF0); } /* TestMethod("Test_CONV_I2__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_I4__Int64() { Test("Test_CONV_I4__Int64", long.MinValue); } /* TestMethod("Test_CONV_I4__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_I4__Int64() { Test("Test_CONV_I4__Int64", long.MaxValue); } /* TestMethod("Test_CONV_I4__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_I4__Int64() { Test("Test_CONV_I4__Int64", (long)0); } /* TestMethod("Test_CONV_I4__Int64", (long)0); */
        //[Fact] public void Test_CONV_I4__Int64() { Test("Test_CONV_I4__Int64", (long)-1); } /* TestMethod("Test_CONV_I4__Int64", (long)-1); */
        //[Fact] public void Test_CONV_I4__Int64() { Test("Test_CONV_I4__Int64", (long)1); } /* TestMethod("Test_CONV_I4__Int64", (long)1); */
        //[Fact] public void Test_CONV_I4__Int64() { Test("Test_CONV_I4__Int64", (long)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_I4__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_I4__Int64() { Test("Test_CONV_I4__Int64", (long)-0x123456789ABCDEF0); } /* TestMethod("Test_CONV_I4__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_I8__Int64() { Test("Test_CONV_I8__Int64", long.MinValue); } /* TestMethod("Test_CONV_I8__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_I8__Int64() { Test("Test_CONV_I8__Int64", long.MaxValue); } /* TestMethod("Test_CONV_I8__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_I8__Int64() { Test("Test_CONV_I8__Int64", (long)0); } /* TestMethod("Test_CONV_I8__Int64", (long)0); */
        //[Fact] public void Test_CONV_I8__Int64() { Test("Test_CONV_I8__Int64", (long)-1); } /* TestMethod("Test_CONV_I8__Int64", (long)-1); */
        //[Fact] public void Test_CONV_I8__Int64() { Test("Test_CONV_I8__Int64", (long)1); } /* TestMethod("Test_CONV_I8__Int64", (long)1); */
        //[Fact] public void Test_CONV_I8__Int64() { Test("Test_CONV_I8__Int64", (long)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_I8__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_I8__Int64() { Test("Test_CONV_I8__Int64", (long)-0x123456789ABCDEF0); } /* TestMethod("Test_CONV_I8__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_U__Int64() { Test("Test_CONV_U__Int64", long.MinValue); } /* TestMethod("Test_CONV_U__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_U__Int64() { Test("Test_CONV_U__Int64", long.MaxValue); } /* TestMethod("Test_CONV_U__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_U__Int64() { Test("Test_CONV_U__Int64", (long)0); } /* TestMethod("Test_CONV_U__Int64", (long)0); */
        //[Fact] public void Test_CONV_U__Int64() { Test("Test_CONV_U__Int64", (long)-1); } /* TestMethod("Test_CONV_U__Int64", (long)-1); */
        //[Fact] public void Test_CONV_U__Int64() { Test("Test_CONV_U__Int64", (long)1); } /* TestMethod("Test_CONV_U__Int64", (long)1); */
        //[Fact] public void Test_CONV_U__Int64() { Test("Test_CONV_U__Int64", (long)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_U__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_U__Int64() { Test("Test_CONV_U__Int64", (long)-0x123456789ABCDEF0); } /* TestMethod("Test_CONV_U__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_U1__Int64() { Test("Test_CONV_U1__Int64", long.MinValue); } /* TestMethod("Test_CONV_U1__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_U1__Int64() { Test("Test_CONV_U1__Int64", long.MaxValue); } /* TestMethod("Test_CONV_U1__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_U1__Int64() { Test("Test_CONV_U1__Int64", (long)0); } /* TestMethod("Test_CONV_U1__Int64", (long)0); */
        //[Fact] public void Test_CONV_U1__Int64() { Test("Test_CONV_U1__Int64", (long)-1); } /* TestMethod("Test_CONV_U1__Int64", (long)-1); */
        //[Fact] public void Test_CONV_U1__Int64() { Test("Test_CONV_U1__Int64", (long)1); } /* TestMethod("Test_CONV_U1__Int64", (long)1); */
        //[Fact] public void Test_CONV_U1__Int64() { Test("Test_CONV_U1__Int64", (long)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_U1__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_U1__Int64() { Test("Test_CONV_U1__Int64", (long)-0x123456789ABCDEF0); } /* TestMethod("Test_CONV_U1__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_U2__Int64() { Test("Test_CONV_U2__Int64", long.MinValue); } /* TestMethod("Test_CONV_U2__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_U2__Int64() { Test("Test_CONV_U2__Int64", long.MaxValue); } /* TestMethod("Test_CONV_U2__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_U2__Int64() { Test("Test_CONV_U2__Int64", (long)0); } /* TestMethod("Test_CONV_U2__Int64", (long)0); */
        //[Fact] public void Test_CONV_U2__Int64() { Test("Test_CONV_U2__Int64", (long)-1); } /* TestMethod("Test_CONV_U2__Int64", (long)-1); */
        //[Fact] public void Test_CONV_U2__Int64() { Test("Test_CONV_U2__Int64", (long)1); } /* TestMethod("Test_CONV_U2__Int64", (long)1); */
        //[Fact] public void Test_CONV_U2__Int64() { Test("Test_CONV_U2__Int64", (long)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_U2__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_U2__Int64() { Test("Test_CONV_U2__Int64", (long)-0x123456789ABCDEF0); } /* TestMethod("Test_CONV_U2__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_U4__Int64() { Test("Test_CONV_U4__Int64", long.MinValue); } /* TestMethod("Test_CONV_U4__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_U4__Int64() { Test("Test_CONV_U4__Int64", long.MaxValue); } /* TestMethod("Test_CONV_U4__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_U4__Int64() { Test("Test_CONV_U4__Int64", (long)0); } /* TestMethod("Test_CONV_U4__Int64", (long)0); */
        //[Fact] public void Test_CONV_U4__Int64() { Test("Test_CONV_U4__Int64", (long)-1); } /* TestMethod("Test_CONV_U4__Int64", (long)-1); */
        //[Fact] public void Test_CONV_U4__Int64() { Test("Test_CONV_U4__Int64", (long)1); } /* TestMethod("Test_CONV_U4__Int64", (long)1); */
        //[Fact] public void Test_CONV_U4__Int64() { Test("Test_CONV_U4__Int64", (long)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_U4__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_U4__Int64() { Test("Test_CONV_U4__Int64", (long)-0x123456789ABCDEF0); } /* TestMethod("Test_CONV_U4__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_U8__Int64() { Test("Test_CONV_U8__Int64", long.MinValue); } /* TestMethod("Test_CONV_U8__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_U8__Int64() { Test("Test_CONV_U8__Int64", long.MaxValue); } /* TestMethod("Test_CONV_U8__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_U8__Int64() { Test("Test_CONV_U8__Int64", (long)0); } /* TestMethod("Test_CONV_U8__Int64", (long)0); */
        //[Fact] public void Test_CONV_U8__Int64() { Test("Test_CONV_U8__Int64", (long)-1); } /* TestMethod("Test_CONV_U8__Int64", (long)-1); */
        //[Fact] public void Test_CONV_U8__Int64() { Test("Test_CONV_U8__Int64", (long)1); } /* TestMethod("Test_CONV_U8__Int64", (long)1); */
        //[Fact] public void Test_CONV_U8__Int64() { Test("Test_CONV_U8__Int64", (long)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_U8__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_U8__Int64() { Test("Test_CONV_U8__Int64", (long)-0x123456789ABCDEF0); } /* TestMethod("Test_CONV_U8__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_R4__Int64() { Test("Test_CONV_R4__Int64", long.MinValue); } /* TestMethod("Test_CONV_R4__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_R4__Int64() { Test("Test_CONV_R4__Int64", long.MaxValue); } /* TestMethod("Test_CONV_R4__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_R4__Int64() { Test("Test_CONV_R4__Int64", (long)0); } /* TestMethod("Test_CONV_R4__Int64", (long)0); */
        //[Fact] public void Test_CONV_R4__Int64() { Test("Test_CONV_R4__Int64", (long)-1); } /* TestMethod("Test_CONV_R4__Int64", (long)-1); */
        //[Fact] public void Test_CONV_R4__Int64() { Test("Test_CONV_R4__Int64", (long)1); } /* TestMethod("Test_CONV_R4__Int64", (long)1); */
        //[Fact] public void Test_CONV_R4__Int64() { Test("Test_CONV_R4__Int64", (long)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_R4__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_R4__Int64() { Test("Test_CONV_R4__Int64", (long)-0x123456789ABCDEF0); } /* TestMethod("Test_CONV_R4__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_R8__Int64() { Test("Test_CONV_R8__Int64", long.MinValue); } /* TestMethod("Test_CONV_R8__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_R8__Int64() { Test("Test_CONV_R8__Int64", long.MaxValue); } /* TestMethod("Test_CONV_R8__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_R8__Int64() { Test("Test_CONV_R8__Int64", (long)0); } /* TestMethod("Test_CONV_R8__Int64", (long)0); */
        //[Fact] public void Test_CONV_R8__Int64() { Test("Test_CONV_R8__Int64", (long)-1); } /* TestMethod("Test_CONV_R8__Int64", (long)-1); */
        //[Fact] public void Test_CONV_R8__Int64() { Test("Test_CONV_R8__Int64", (long)1); } /* TestMethod("Test_CONV_R8__Int64", (long)1); */
        //[Fact] public void Test_CONV_R8__Int64() { Test("Test_CONV_R8__Int64", (long)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_R8__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_R8__Int64() { Test("Test_CONV_R8__Int64", (long)-0x123456789ABCDEF0); } /* TestMethod("Test_CONV_R8__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I__Int64() { Test("Test_CONV_OVF_I__Int64", long.MinValue); } /* TestMethodEX("Test_CONV_OVF_I__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_OVF_I__Int64() { Test("Test_CONV_OVF_I__Int64", long.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I__Int64() { Test("Test_CONV_OVF_I__Int64", (long)0); } /* TestMethod("Test_CONV_OVF_I__Int64", (long)0); */
        //[Fact] public void Test_CONV_OVF_I__Int64() { Test("Test_CONV_OVF_I__Int64", (long)-1); } /* TestMethodEX("Test_CONV_OVF_I__Int64", (long)-1); */
        //[Fact] public void Test_CONV_OVF_I__Int64() { Test("Test_CONV_OVF_I__Int64", (long)1); } /* TestMethod("Test_CONV_OVF_I__Int64", (long)1); */
        //[Fact] public void Test_CONV_OVF_I__Int64() { Test("Test_CONV_OVF_I__Int64", (long)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I__Int64() { Test("Test_CONV_OVF_I__Int64", (long)-0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I1__Int64() { Test("Test_CONV_OVF_I1__Int64", long.MinValue); } /* TestMethodEX("Test_CONV_OVF_I1__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_OVF_I1__Int64() { Test("Test_CONV_OVF_I1__Int64", long.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I1__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I1__Int64() { Test("Test_CONV_OVF_I1__Int64", (long)0); } /* TestMethod("Test_CONV_OVF_I1__Int64", (long)0); */
        //[Fact] public void Test_CONV_OVF_I1__Int64() { Test("Test_CONV_OVF_I1__Int64", (long)-1); } /* TestMethod("Test_CONV_OVF_I1__Int64", (long)-1); */
        //[Fact] public void Test_CONV_OVF_I1__Int64() { Test("Test_CONV_OVF_I1__Int64", (long)1); } /* TestMethod("Test_CONV_OVF_I1__Int64", (long)1); */
        //[Fact] public void Test_CONV_OVF_I1__Int64() { Test("Test_CONV_OVF_I1__Int64", (long)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I1__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I1__Int64() { Test("Test_CONV_OVF_I1__Int64", (long)-0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I1__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I2__Int64() { Test("Test_CONV_OVF_I2__Int64", long.MinValue); } /* TestMethodEX("Test_CONV_OVF_I2__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_OVF_I2__Int64() { Test("Test_CONV_OVF_I2__Int64", long.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I2__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I2__Int64() { Test("Test_CONV_OVF_I2__Int64", (long)0); } /* TestMethod("Test_CONV_OVF_I2__Int64", (long)0); */
        //[Fact] public void Test_CONV_OVF_I2__Int64() { Test("Test_CONV_OVF_I2__Int64", (long)-1); } /* TestMethod("Test_CONV_OVF_I2__Int64", (long)-1); */
        //[Fact] public void Test_CONV_OVF_I2__Int64() { Test("Test_CONV_OVF_I2__Int64", (long)1); } /* TestMethod("Test_CONV_OVF_I2__Int64", (long)1); */
        //[Fact] public void Test_CONV_OVF_I2__Int64() { Test("Test_CONV_OVF_I2__Int64", (long)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I2__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I2__Int64() { Test("Test_CONV_OVF_I2__Int64", (long)-0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I2__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I4__Int64() { Test("Test_CONV_OVF_I4__Int64", long.MinValue); } /* TestMethodEX("Test_CONV_OVF_I4__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_OVF_I4__Int64() { Test("Test_CONV_OVF_I4__Int64", long.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I4__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I4__Int64() { Test("Test_CONV_OVF_I4__Int64", (long)0); } /* TestMethod("Test_CONV_OVF_I4__Int64", (long)0); */
        //[Fact] public void Test_CONV_OVF_I4__Int64() { Test("Test_CONV_OVF_I4__Int64", (long)-1); } /* TestMethod("Test_CONV_OVF_I4__Int64", (long)-1); */
        //[Fact] public void Test_CONV_OVF_I4__Int64() { Test("Test_CONV_OVF_I4__Int64", (long)1); } /* TestMethod("Test_CONV_OVF_I4__Int64", (long)1); */
        //[Fact] public void Test_CONV_OVF_I4__Int64() { Test("Test_CONV_OVF_I4__Int64", (long)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I4__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I4__Int64() { Test("Test_CONV_OVF_I4__Int64", (long)-0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I4__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I8__Int64() { Test("Test_CONV_OVF_I8__Int64", long.MinValue); } /* TestMethod("Test_CONV_OVF_I8__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_OVF_I8__Int64() { Test("Test_CONV_OVF_I8__Int64", long.MaxValue); } /* TestMethod("Test_CONV_OVF_I8__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I8__Int64() { Test("Test_CONV_OVF_I8__Int64", (long)0); } /* TestMethod("Test_CONV_OVF_I8__Int64", (long)0); */
        //[Fact] public void Test_CONV_OVF_I8__Int64() { Test("Test_CONV_OVF_I8__Int64", (long)-1); } /* TestMethod("Test_CONV_OVF_I8__Int64", (long)-1); */
        //[Fact] public void Test_CONV_OVF_I8__Int64() { Test("Test_CONV_OVF_I8__Int64", (long)1); } /* TestMethod("Test_CONV_OVF_I8__Int64", (long)1); */
        //[Fact] public void Test_CONV_OVF_I8__Int64() { Test("Test_CONV_OVF_I8__Int64", (long)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_OVF_I8__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I8__Int64() { Test("Test_CONV_OVF_I8__Int64", (long)-0x123456789ABCDEF0); } /* TestMethod("Test_CONV_OVF_I8__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U__Int64() { Test("Test_CONV_OVF_U__Int64", long.MinValue); } /* TestMethodEX("Test_CONV_OVF_U__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_OVF_U__Int64() { Test("Test_CONV_OVF_U__Int64", long.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U__Int64() { Test("Test_CONV_OVF_U__Int64", (long)0); } /* TestMethod("Test_CONV_OVF_U__Int64", (long)0); */
        //[Fact] public void Test_CONV_OVF_U__Int64() { Test("Test_CONV_OVF_U__Int64", (long)-1); } /* TestMethodEX("Test_CONV_OVF_U__Int64", (long)-1); */
        //[Fact] public void Test_CONV_OVF_U__Int64() { Test("Test_CONV_OVF_U__Int64", (long)1); } /* TestMethod("Test_CONV_OVF_U__Int64", (long)1); */
        //[Fact] public void Test_CONV_OVF_U__Int64() { Test("Test_CONV_OVF_U__Int64", (long)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U__Int64() { Test("Test_CONV_OVF_U__Int64", (long)-0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U1__Int64() { Test("Test_CONV_OVF_U1__Int64", long.MinValue); } /* TestMethodEX("Test_CONV_OVF_U1__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_OVF_U1__Int64() { Test("Test_CONV_OVF_U1__Int64", long.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U1__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U1__Int64() { Test("Test_CONV_OVF_U1__Int64", (long)0); } /* TestMethod("Test_CONV_OVF_U1__Int64", (long)0); */
        //[Fact] public void Test_CONV_OVF_U1__Int64() { Test("Test_CONV_OVF_U1__Int64", (long)-1); } /* TestMethodEX("Test_CONV_OVF_U1__Int64", (long)-1); */
        //[Fact] public void Test_CONV_OVF_U1__Int64() { Test("Test_CONV_OVF_U1__Int64", (long)1); } /* TestMethod("Test_CONV_OVF_U1__Int64", (long)1); */
        //[Fact] public void Test_CONV_OVF_U1__Int64() { Test("Test_CONV_OVF_U1__Int64", (long)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U1__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U1__Int64() { Test("Test_CONV_OVF_U1__Int64", (long)-0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U1__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U2__Int64() { Test("Test_CONV_OVF_U2__Int64", long.MinValue); } /* TestMethodEX("Test_CONV_OVF_U2__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_OVF_U2__Int64() { Test("Test_CONV_OVF_U2__Int64", long.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U2__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U2__Int64() { Test("Test_CONV_OVF_U2__Int64", (long)0); } /* TestMethod("Test_CONV_OVF_U2__Int64", (long)0); */
        //[Fact] public void Test_CONV_OVF_U2__Int64() { Test("Test_CONV_OVF_U2__Int64", (long)-1); } /* TestMethodEX("Test_CONV_OVF_U2__Int64", (long)-1); */
        //[Fact] public void Test_CONV_OVF_U2__Int64() { Test("Test_CONV_OVF_U2__Int64", (long)1); } /* TestMethod("Test_CONV_OVF_U2__Int64", (long)1); */
        //[Fact] public void Test_CONV_OVF_U2__Int64() { Test("Test_CONV_OVF_U2__Int64", (long)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U2__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U2__Int64() { Test("Test_CONV_OVF_U2__Int64", (long)-0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U2__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U4__Int64() { Test("Test_CONV_OVF_U4__Int64", long.MinValue); } /* TestMethodEX("Test_CONV_OVF_U4__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_OVF_U4__Int64() { Test("Test_CONV_OVF_U4__Int64", long.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U4__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U4__Int64() { Test("Test_CONV_OVF_U4__Int64", (long)0); } /* TestMethod("Test_CONV_OVF_U4__Int64", (long)0); */
        //[Fact] public void Test_CONV_OVF_U4__Int64() { Test("Test_CONV_OVF_U4__Int64", (long)-1); } /* TestMethodEX("Test_CONV_OVF_U4__Int64", (long)-1); */
        //[Fact] public void Test_CONV_OVF_U4__Int64() { Test("Test_CONV_OVF_U4__Int64", (long)1); } /* TestMethod("Test_CONV_OVF_U4__Int64", (long)1); */
        //[Fact] public void Test_CONV_OVF_U4__Int64() { Test("Test_CONV_OVF_U4__Int64", (long)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U4__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U4__Int64() { Test("Test_CONV_OVF_U4__Int64", (long)-0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U4__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U8__Int64() { Test("Test_CONV_OVF_U8__Int64", long.MinValue); } /* TestMethodEX("Test_CONV_OVF_U8__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_OVF_U8__Int64() { Test("Test_CONV_OVF_U8__Int64", long.MaxValue); } /* TestMethod("Test_CONV_OVF_U8__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U8__Int64() { Test("Test_CONV_OVF_U8__Int64", (long)0); } /* TestMethod("Test_CONV_OVF_U8__Int64", (long)0); */
        //[Fact] public void Test_CONV_OVF_U8__Int64() { Test("Test_CONV_OVF_U8__Int64", (long)-1); } /* TestMethodEX("Test_CONV_OVF_U8__Int64", (long)-1); */
        //[Fact] public void Test_CONV_OVF_U8__Int64() { Test("Test_CONV_OVF_U8__Int64", (long)1); } /* TestMethod("Test_CONV_OVF_U8__Int64", (long)1); */
        //[Fact] public void Test_CONV_OVF_U8__Int64() { Test("Test_CONV_OVF_U8__Int64", (long)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_OVF_U8__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U8__Int64() { Test("Test_CONV_OVF_U8__Int64", (long)-0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U8__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I_UN__Int64() { Test("Test_CONV_OVF_I_UN__Int64", long.MinValue); } /* TestMethodEX("Test_CONV_OVF_I_UN__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_OVF_I_UN__Int64() { Test("Test_CONV_OVF_I_UN__Int64", long.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I_UN__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I_UN__Int64() { Test("Test_CONV_OVF_I_UN__Int64", (long)0); } /* TestMethod("Test_CONV_OVF_I_UN__Int64", (long)0); */
        //[Fact] public void Test_CONV_OVF_I_UN__Int64() { Test("Test_CONV_OVF_I_UN__Int64", (long)-1); } /* TestMethodEX("Test_CONV_OVF_I_UN__Int64", (long)-1); */
        //[Fact] public void Test_CONV_OVF_I_UN__Int64() { Test("Test_CONV_OVF_I_UN__Int64", (long)1); } /* TestMethod("Test_CONV_OVF_I_UN__Int64", (long)1); */
        //[Fact] public void Test_CONV_OVF_I_UN__Int64() { Test("Test_CONV_OVF_I_UN__Int64", (long)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I_UN__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I_UN__Int64() { Test("Test_CONV_OVF_I_UN__Int64", (long)-0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I_UN__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Int64() { Test("Test_CONV_OVF_I1_UN__Int64", long.MinValue); } /* TestMethodEX("Test_CONV_OVF_I1_UN__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Int64() { Test("Test_CONV_OVF_I1_UN__Int64", long.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I1_UN__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Int64() { Test("Test_CONV_OVF_I1_UN__Int64", (long)0); } /* TestMethod("Test_CONV_OVF_I1_UN__Int64", (long)0); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Int64() { Test("Test_CONV_OVF_I1_UN__Int64", (long)-1); } /* TestMethodEX("Test_CONV_OVF_I1_UN__Int64", (long)-1); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Int64() { Test("Test_CONV_OVF_I1_UN__Int64", (long)1); } /* TestMethod("Test_CONV_OVF_I1_UN__Int64", (long)1); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Int64() { Test("Test_CONV_OVF_I1_UN__Int64", (long)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I1_UN__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Int64() { Test("Test_CONV_OVF_I1_UN__Int64", (long)-0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I1_UN__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Int64() { Test("Test_CONV_OVF_I2_UN__Int64", long.MinValue); } /* TestMethodEX("Test_CONV_OVF_I2_UN__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Int64() { Test("Test_CONV_OVF_I2_UN__Int64", long.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I2_UN__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Int64() { Test("Test_CONV_OVF_I2_UN__Int64", (long)0); } /* TestMethod("Test_CONV_OVF_I2_UN__Int64", (long)0); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Int64() { Test("Test_CONV_OVF_I2_UN__Int64", (long)-1); } /* TestMethodEX("Test_CONV_OVF_I2_UN__Int64", (long)-1); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Int64() { Test("Test_CONV_OVF_I2_UN__Int64", (long)1); } /* TestMethod("Test_CONV_OVF_I2_UN__Int64", (long)1); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Int64() { Test("Test_CONV_OVF_I2_UN__Int64", (long)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I2_UN__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Int64() { Test("Test_CONV_OVF_I2_UN__Int64", (long)-0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I2_UN__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Int64() { Test("Test_CONV_OVF_I4_UN__Int64", long.MinValue); } /* TestMethodEX("Test_CONV_OVF_I4_UN__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Int64() { Test("Test_CONV_OVF_I4_UN__Int64", long.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I4_UN__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Int64() { Test("Test_CONV_OVF_I4_UN__Int64", (long)0); } /* TestMethod("Test_CONV_OVF_I4_UN__Int64", (long)0); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Int64() { Test("Test_CONV_OVF_I4_UN__Int64", (long)-1); } /* TestMethodEX("Test_CONV_OVF_I4_UN__Int64", (long)-1); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Int64() { Test("Test_CONV_OVF_I4_UN__Int64", (long)1); } /* TestMethod("Test_CONV_OVF_I4_UN__Int64", (long)1); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Int64() { Test("Test_CONV_OVF_I4_UN__Int64", (long)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I4_UN__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Int64() { Test("Test_CONV_OVF_I4_UN__Int64", (long)-0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I4_UN__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Int64() { Test("Test_CONV_OVF_I8_UN__Int64", long.MinValue); } /* TestMethodEX("Test_CONV_OVF_I8_UN__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Int64() { Test("Test_CONV_OVF_I8_UN__Int64", long.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I8_UN__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Int64() { Test("Test_CONV_OVF_I8_UN__Int64", (long)0); } /* TestMethod("Test_CONV_OVF_I8_UN__Int64", (long)0); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Int64() { Test("Test_CONV_OVF_I8_UN__Int64", (long)-1); } /* TestMethodEX("Test_CONV_OVF_I8_UN__Int64", (long)-1); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Int64() { Test("Test_CONV_OVF_I8_UN__Int64", (long)1); } /* TestMethod("Test_CONV_OVF_I8_UN__Int64", (long)1); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Int64() { Test("Test_CONV_OVF_I8_UN__Int64", (long)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I8_UN__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Int64() { Test("Test_CONV_OVF_I8_UN__Int64", (long)-0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I8_UN__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U_UN__Int64() { Test("Test_CONV_OVF_U_UN__Int64", long.MinValue); } /* TestMethodEX("Test_CONV_OVF_U_UN__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_OVF_U_UN__Int64() { Test("Test_CONV_OVF_U_UN__Int64", long.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U_UN__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U_UN__Int64() { Test("Test_CONV_OVF_U_UN__Int64", (long)0); } /* TestMethod("Test_CONV_OVF_U_UN__Int64", (long)0); */
        //[Fact] public void Test_CONV_OVF_U_UN__Int64() { Test("Test_CONV_OVF_U_UN__Int64", (long)-1); } /* TestMethodEX("Test_CONV_OVF_U_UN__Int64", (long)-1); */
        //[Fact] public void Test_CONV_OVF_U_UN__Int64() { Test("Test_CONV_OVF_U_UN__Int64", (long)1); } /* TestMethod("Test_CONV_OVF_U_UN__Int64", (long)1); */
        //[Fact] public void Test_CONV_OVF_U_UN__Int64() { Test("Test_CONV_OVF_U_UN__Int64", (long)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U_UN__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U_UN__Int64() { Test("Test_CONV_OVF_U_UN__Int64", (long)-0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U_UN__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Int64() { Test("Test_CONV_OVF_U1_UN__Int64", long.MinValue); } /* TestMethodEX("Test_CONV_OVF_U1_UN__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Int64() { Test("Test_CONV_OVF_U1_UN__Int64", long.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U1_UN__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Int64() { Test("Test_CONV_OVF_U1_UN__Int64", (long)0); } /* TestMethod("Test_CONV_OVF_U1_UN__Int64", (long)0); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Int64() { Test("Test_CONV_OVF_U1_UN__Int64", (long)-1); } /* TestMethodEX("Test_CONV_OVF_U1_UN__Int64", (long)-1); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Int64() { Test("Test_CONV_OVF_U1_UN__Int64", (long)1); } /* TestMethod("Test_CONV_OVF_U1_UN__Int64", (long)1); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Int64() { Test("Test_CONV_OVF_U1_UN__Int64", (long)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U1_UN__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Int64() { Test("Test_CONV_OVF_U1_UN__Int64", (long)-0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U1_UN__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Int64() { Test("Test_CONV_OVF_U2_UN__Int64", long.MinValue); } /* TestMethodEX("Test_CONV_OVF_U2_UN__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Int64() { Test("Test_CONV_OVF_U2_UN__Int64", long.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U2_UN__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Int64() { Test("Test_CONV_OVF_U2_UN__Int64", (long)0); } /* TestMethod("Test_CONV_OVF_U2_UN__Int64", (long)0); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Int64() { Test("Test_CONV_OVF_U2_UN__Int64", (long)-1); } /* TestMethodEX("Test_CONV_OVF_U2_UN__Int64", (long)-1); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Int64() { Test("Test_CONV_OVF_U2_UN__Int64", (long)1); } /* TestMethod("Test_CONV_OVF_U2_UN__Int64", (long)1); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Int64() { Test("Test_CONV_OVF_U2_UN__Int64", (long)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U2_UN__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Int64() { Test("Test_CONV_OVF_U2_UN__Int64", (long)-0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U2_UN__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Int64() { Test("Test_CONV_OVF_U4_UN__Int64", long.MinValue); } /* TestMethodEX("Test_CONV_OVF_U4_UN__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Int64() { Test("Test_CONV_OVF_U4_UN__Int64", long.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U4_UN__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Int64() { Test("Test_CONV_OVF_U4_UN__Int64", (long)0); } /* TestMethod("Test_CONV_OVF_U4_UN__Int64", (long)0); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Int64() { Test("Test_CONV_OVF_U4_UN__Int64", (long)-1); } /* TestMethodEX("Test_CONV_OVF_U4_UN__Int64", (long)-1); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Int64() { Test("Test_CONV_OVF_U4_UN__Int64", (long)1); } /* TestMethod("Test_CONV_OVF_U4_UN__Int64", (long)1); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Int64() { Test("Test_CONV_OVF_U4_UN__Int64", (long)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U4_UN__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Int64() { Test("Test_CONV_OVF_U4_UN__Int64", (long)-0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U4_UN__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Int64() { Test("Test_CONV_OVF_U8_UN__Int64", long.MinValue); } /* TestMethod("Test_CONV_OVF_U8_UN__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Int64() { Test("Test_CONV_OVF_U8_UN__Int64", long.MaxValue); } /* TestMethod("Test_CONV_OVF_U8_UN__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Int64() { Test("Test_CONV_OVF_U8_UN__Int64", (long)0); } /* TestMethod("Test_CONV_OVF_U8_UN__Int64", (long)0); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Int64() { Test("Test_CONV_OVF_U8_UN__Int64", (long)-1); } /* TestMethod("Test_CONV_OVF_U8_UN__Int64", (long)-1); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Int64() { Test("Test_CONV_OVF_U8_UN__Int64", (long)1); } /* TestMethod("Test_CONV_OVF_U8_UN__Int64", (long)1); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Int64() { Test("Test_CONV_OVF_U8_UN__Int64", (long)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_OVF_U8_UN__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Int64() { Test("Test_CONV_OVF_U8_UN__Int64", (long)-0x123456789ABCDEF0); } /* TestMethod("Test_CONV_OVF_U8_UN__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_R_UN__Int64() { Test("Test_CONV_R_UN__Int64", long.MinValue); } /* TestMethod("Test_CONV_R_UN__Int64", long.MinValue); */
        //[Fact] public void Test_CONV_R_UN__Int64() { Test("Test_CONV_R_UN__Int64", long.MaxValue); } /* TestMethod("Test_CONV_R_UN__Int64", long.MaxValue); */
        //[Fact] public void Test_CONV_R_UN__Int64() { Test("Test_CONV_R_UN__Int64", (long)0); } /* TestMethod("Test_CONV_R_UN__Int64", (long)0); */
        //[Fact] public void Test_CONV_R_UN__Int64() { Test("Test_CONV_R_UN__Int64", (long)-1); } /* TestMethod("Test_CONV_R_UN__Int64", (long)-1); */
        //[Fact] public void Test_CONV_R_UN__Int64() { Test("Test_CONV_R_UN__Int64", (long)1); } /* TestMethod("Test_CONV_R_UN__Int64", (long)1); */
        //[Fact] public void Test_CONV_R_UN__Int64() { Test("Test_CONV_R_UN__Int64", (long)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_R_UN__Int64", (long)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_R_UN__Int64() { Test("Test_CONV_R_UN__Int64", (long)-0x123456789ABCDEF0); } /* TestMethod("Test_CONV_R_UN__Int64", (long)-0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_I__UIntPtr() { Test("Test_CONV_I__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_I__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_I__UIntPtr() { Test("Test_CONV_I__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethod("Test_CONV_I__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_I__UIntPtr() { Test("Test_CONV_I__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethod("Test_CONV_I__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_I__UIntPtr() { Test("Test_CONV_I__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethod("Test_CONV_I__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_I1__UIntPtr() { Test("Test_CONV_I1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_I1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_I1__UIntPtr() { Test("Test_CONV_I1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethod("Test_CONV_I1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_I1__UIntPtr() { Test("Test_CONV_I1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethod("Test_CONV_I1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_I1__UIntPtr() { Test("Test_CONV_I1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethod("Test_CONV_I1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_I2__UIntPtr() { Test("Test_CONV_I2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_I2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_I2__UIntPtr() { Test("Test_CONV_I2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethod("Test_CONV_I2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_I2__UIntPtr() { Test("Test_CONV_I2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethod("Test_CONV_I2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_I2__UIntPtr() { Test("Test_CONV_I2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethod("Test_CONV_I2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_I4__UIntPtr() { Test("Test_CONV_I4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_I4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_I4__UIntPtr() { Test("Test_CONV_I4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethod("Test_CONV_I4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_I4__UIntPtr() { Test("Test_CONV_I4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethod("Test_CONV_I4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_I4__UIntPtr() { Test("Test_CONV_I4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethod("Test_CONV_I4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_I8__UIntPtr() { Test("Test_CONV_I8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_I8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_I8__UIntPtr() { Test("Test_CONV_I8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethod("Test_CONV_I8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_I8__UIntPtr() { Test("Test_CONV_I8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethod("Test_CONV_I8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_I8__UIntPtr() { Test("Test_CONV_I8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethod("Test_CONV_I8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_U__UIntPtr() { Test("Test_CONV_U__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_U__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_U__UIntPtr() { Test("Test_CONV_U__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethod("Test_CONV_U__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_U__UIntPtr() { Test("Test_CONV_U__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethod("Test_CONV_U__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_U__UIntPtr() { Test("Test_CONV_U__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethod("Test_CONV_U__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_U1__UIntPtr() { Test("Test_CONV_U1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_U1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_U1__UIntPtr() { Test("Test_CONV_U1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethod("Test_CONV_U1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_U1__UIntPtr() { Test("Test_CONV_U1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethod("Test_CONV_U1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_U1__UIntPtr() { Test("Test_CONV_U1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethod("Test_CONV_U1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_U2__UIntPtr() { Test("Test_CONV_U2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_U2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_U2__UIntPtr() { Test("Test_CONV_U2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethod("Test_CONV_U2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_U2__UIntPtr() { Test("Test_CONV_U2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethod("Test_CONV_U2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_U2__UIntPtr() { Test("Test_CONV_U2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethod("Test_CONV_U2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_U4__UIntPtr() { Test("Test_CONV_U4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_U4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_U4__UIntPtr() { Test("Test_CONV_U4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethod("Test_CONV_U4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_U4__UIntPtr() { Test("Test_CONV_U4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethod("Test_CONV_U4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_U4__UIntPtr() { Test("Test_CONV_U4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethod("Test_CONV_U4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_U8__UIntPtr() { Test("Test_CONV_U8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_U8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_U8__UIntPtr() { Test("Test_CONV_U8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethod("Test_CONV_U8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_U8__UIntPtr() { Test("Test_CONV_U8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethod("Test_CONV_U8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_U8__UIntPtr() { Test("Test_CONV_U8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethod("Test_CONV_U8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_R4__UIntPtr() { Test("Test_CONV_R4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_R4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_R4__UIntPtr() { Test("Test_CONV_R4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethod("Test_CONV_R4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_R4__UIntPtr() { Test("Test_CONV_R4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethod("Test_CONV_R4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_R4__UIntPtr() { Test("Test_CONV_R4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethod("Test_CONV_R4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_R8__UIntPtr() { Test("Test_CONV_R8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_R8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_R8__UIntPtr() { Test("Test_CONV_R8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethod("Test_CONV_R8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_R8__UIntPtr() { Test("Test_CONV_R8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethod("Test_CONV_R8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_R8__UIntPtr() { Test("Test_CONV_R8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethod("Test_CONV_R8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_OVF_I__UIntPtr() { Test("Test_CONV_OVF_I__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_OVF_I__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_OVF_I__UIntPtr() { Test("Test_CONV_OVF_I__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethod("Test_CONV_OVF_I__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_I__UIntPtr() { Test("Test_CONV_OVF_I__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethod("Test_CONV_OVF_I__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_OVF_I__UIntPtr() { Test("Test_CONV_OVF_I__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethod("Test_CONV_OVF_I__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_OVF_I1__UIntPtr() { Test("Test_CONV_OVF_I1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_OVF_I1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_OVF_I1__UIntPtr() { Test("Test_CONV_OVF_I1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethod("Test_CONV_OVF_I1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_I1__UIntPtr() { Test("Test_CONV_OVF_I1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethodEX("Test_CONV_OVF_I1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_OVF_I1__UIntPtr() { Test("Test_CONV_OVF_I1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethodEX("Test_CONV_OVF_I1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_OVF_I2__UIntPtr() { Test("Test_CONV_OVF_I2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethodEX("Test_CONV_OVF_I2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_OVF_I2__UIntPtr() { Test("Test_CONV_OVF_I2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_I2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_I2__UIntPtr() { Test("Test_CONV_OVF_I2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethodEX("Test_CONV_OVF_I2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_OVF_I2__UIntPtr() { Test("Test_CONV_OVF_I2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethodEX("Test_CONV_OVF_I2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_OVF_I4__UIntPtr() { Test("Test_CONV_OVF_I4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_OVF_I4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_OVF_I4__UIntPtr() { Test("Test_CONV_OVF_I4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethod("Test_CONV_OVF_I4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_I4__UIntPtr() { Test("Test_CONV_OVF_I4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethodEX("Test_CONV_OVF_I4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_OVF_I4__UIntPtr() { Test("Test_CONV_OVF_I4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethodEX("Test_CONV_OVF_I4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_OVF_I8__UIntPtr() { Test("Test_CONV_OVF_I8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_OVF_I8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_OVF_I8__UIntPtr() { Test("Test_CONV_OVF_I8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethod("Test_CONV_OVF_I8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_I8__UIntPtr() { Test("Test_CONV_OVF_I8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethod("Test_CONV_OVF_I8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_OVF_I8__UIntPtr() { Test("Test_CONV_OVF_I8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethod("Test_CONV_OVF_I8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_OVF_U__UIntPtr() { Test("Test_CONV_OVF_U__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_OVF_U__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_OVF_U__UIntPtr() { Test("Test_CONV_OVF_U__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_U__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_U__UIntPtr() { Test("Test_CONV_OVF_U__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethod("Test_CONV_OVF_U__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_OVF_U__UIntPtr() { Test("Test_CONV_OVF_U__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethodEX("Test_CONV_OVF_U__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_OVF_U1__UIntPtr() { Test("Test_CONV_OVF_U1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_OVF_U1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_OVF_U1__UIntPtr() { Test("Test_CONV_OVF_U1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_U1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_U1__UIntPtr() { Test("Test_CONV_OVF_U1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethodEX("Test_CONV_OVF_U1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_OVF_U1__UIntPtr() { Test("Test_CONV_OVF_U1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethodEX("Test_CONV_OVF_U1__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_OVF_U2__UIntPtr() { Test("Test_CONV_OVF_U2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_OVF_U2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_OVF_U2__UIntPtr() { Test("Test_CONV_OVF_U2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_U2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_U2__UIntPtr() { Test("Test_CONV_OVF_U2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethodEX("Test_CONV_OVF_U2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_OVF_U2__UIntPtr() { Test("Test_CONV_OVF_U2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethodEX("Test_CONV_OVF_U2__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_OVF_U4__UIntPtr() { Test("Test_CONV_OVF_U4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_OVF_U4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_OVF_U4__UIntPtr() { Test("Test_CONV_OVF_U4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_U4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_U4__UIntPtr() { Test("Test_CONV_OVF_U4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethodEX("Test_CONV_OVF_U4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_OVF_U4__UIntPtr() { Test("Test_CONV_OVF_U4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethodEX("Test_CONV_OVF_U4__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_OVF_U8__UIntPtr() { Test("Test_CONV_OVF_U8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_OVF_U8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_OVF_U8__UIntPtr() { Test("Test_CONV_OVF_U8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_U8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_U8__UIntPtr() { Test("Test_CONV_OVF_U8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethod("Test_CONV_OVF_U8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_OVF_U8__UIntPtr() { Test("Test_CONV_OVF_U8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethodEX("Test_CONV_OVF_U8__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_OVF_I_UN__UIntPtr() { Test("Test_CONV_OVF_I_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_OVF_I_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_OVF_I_UN__UIntPtr() { Test("Test_CONV_OVF_I_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_I_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_I_UN__UIntPtr() { Test("Test_CONV_OVF_I_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethod("Test_CONV_OVF_I_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_OVF_I_UN__UIntPtr() { Test("Test_CONV_OVF_I_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethodEX("Test_CONV_OVF_I_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_OVF_I1_UN__UIntPtr() { Test("Test_CONV_OVF_I1_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_OVF_I1_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_OVF_I1_UN__UIntPtr() { Test("Test_CONV_OVF_I1_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_I1_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_I1_UN__UIntPtr() { Test("Test_CONV_OVF_I1_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethodEX("Test_CONV_OVF_I1_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_OVF_I1_UN__UIntPtr() { Test("Test_CONV_OVF_I1_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethodEX("Test_CONV_OVF_I1_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_OVF_I2_UN__UIntPtr() { Test("Test_CONV_OVF_I2_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethod("Test_CONV_OVF_I2_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_OVF_I2_UN__UIntPtr() { Test("Test_CONV_OVF_I2_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_I2_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_I2_UN__UIntPtr() { Test("Test_CONV_OVF_I2_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethodEX("Test_CONV_OVF_I2_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_OVF_I2_UN__UIntPtr() { Test("Test_CONV_OVF_I2_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethodEX("Test_CONV_OVF_I2_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_OVF_I4_UN__UIntPtr() { Test("Test_CONV_OVF_I4_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethodEX("Test_CONV_OVF_I4_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_OVF_I4_UN__UIntPtr() { Test("Test_CONV_OVF_I4_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_I4_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_I4_UN__UIntPtr() { Test("Test_CONV_OVF_I4_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethodEX("Test_CONV_OVF_I4_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_OVF_I4_UN__UIntPtr() { Test("Test_CONV_OVF_I4_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethodEX("Test_CONV_OVF_I4_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_OVF_I8_UN__UIntPtr() { Test("Test_CONV_OVF_I8_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethodEX("Test_CONV_OVF_I8_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_OVF_I8_UN__UIntPtr() { Test("Test_CONV_OVF_I8_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_I8_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_I8_UN__UIntPtr() { Test("Test_CONV_OVF_I8_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethodEX("Test_CONV_OVF_I8_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_OVF_I8_UN__UIntPtr() { Test("Test_CONV_OVF_I8_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethodEX("Test_CONV_OVF_I8_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_OVF_U_UN__UIntPtr() { Test("Test_CONV_OVF_U_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethodEX("Test_CONV_OVF_U_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_OVF_U_UN__UIntPtr() { Test("Test_CONV_OVF_U_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_U_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_U_UN__UIntPtr() { Test("Test_CONV_OVF_U_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethodEX("Test_CONV_OVF_U_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_OVF_U_UN__UIntPtr() { Test("Test_CONV_OVF_U_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethodEX("Test_CONV_OVF_U_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_OVF_U1_UN__UIntPtr() { Test("Test_CONV_OVF_U1_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethodEX("Test_CONV_OVF_U1_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_OVF_U1_UN__UIntPtr() { Test("Test_CONV_OVF_U1_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_U1_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_U1_UN__UIntPtr() { Test("Test_CONV_OVF_U1_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethodEX("Test_CONV_OVF_U1_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_OVF_U1_UN__UIntPtr() { Test("Test_CONV_OVF_U1_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethodEX("Test_CONV_OVF_U1_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_OVF_U2_UN__UIntPtr() { Test("Test_CONV_OVF_U2_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethodEX("Test_CONV_OVF_U2_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_OVF_U2_UN__UIntPtr() { Test("Test_CONV_OVF_U2_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_U2_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_U2_UN__UIntPtr() { Test("Test_CONV_OVF_U2_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethodEX("Test_CONV_OVF_U2_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_OVF_U2_UN__UIntPtr() { Test("Test_CONV_OVF_U2_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethodEX("Test_CONV_OVF_U2_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_OVF_U4_UN__UIntPtr() { Test("Test_CONV_OVF_U4_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethodEX("Test_CONV_OVF_U4_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_OVF_U4_UN__UIntPtr() { Test("Test_CONV_OVF_U4_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_U4_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_U4_UN__UIntPtr() { Test("Test_CONV_OVF_U4_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethodEX("Test_CONV_OVF_U4_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_OVF_U4_UN__UIntPtr() { Test("Test_CONV_OVF_U4_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethodEX("Test_CONV_OVF_U4_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_OVF_U8_UN__UIntPtr() { Test("Test_CONV_OVF_U8_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethodEX("Test_CONV_OVF_U8_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_OVF_U8_UN__UIntPtr() { Test("Test_CONV_OVF_U8_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethodEX("Test_CONV_OVF_U8_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_OVF_U8_UN__UIntPtr() { Test("Test_CONV_OVF_U8_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethodEX("Test_CONV_OVF_U8_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_OVF_U8_UN__UIntPtr() { Test("Test_CONV_OVF_U8_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethodEX("Test_CONV_OVF_U8_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_R_UN__UIntPtr() { Test("Test_CONV_R_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); } /* TestMethodEX("Test_CONV_R_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MinValue) : new UIntPtr(ulong.MinValue)); */
        //[Fact] public void Test_CONV_R_UN__UIntPtr() { Test("Test_CONV_R_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); } /* TestMethodEX("Test_CONV_R_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(uint.MaxValue) : new UIntPtr(ulong.MaxValue)); */
        //[Fact] public void Test_CONV_R_UN__UIntPtr() { Test("Test_CONV_R_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); } /* TestMethodEX("Test_CONV_R_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x12345678U) : new UIntPtr(0x123456789ABCDEF0UL)); */
        //[Fact] public void Test_CONV_R_UN__UIntPtr() { Test("Test_CONV_R_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); } /* TestMethodEX("Test_CONV_R_UN__UIntPtr", UIntPtr.Size == 4 ? new UIntPtr(0x9ABCDEF0U) : new UIntPtr(0x9ABCDEF012345678UL)); */
        //[Fact] public void Test_CONV_I__Byte() { Test("Test_CONV_I__Byte", byte.MinValue); } /* TestMethod("Test_CONV_I__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_I__Byte() { Test("Test_CONV_I__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_I__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_I__Byte() { Test("Test_CONV_I__Byte", (byte)0x12); } /* TestMethod("Test_CONV_I__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_I__Byte() { Test("Test_CONV_I__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_I__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_I1__Byte() { Test("Test_CONV_I1__Byte", byte.MinValue); } /* TestMethod("Test_CONV_I1__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_I1__Byte() { Test("Test_CONV_I1__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_I1__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_I1__Byte() { Test("Test_CONV_I1__Byte", (byte)0x12); } /* TestMethod("Test_CONV_I1__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_I1__Byte() { Test("Test_CONV_I1__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_I1__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_I2__Byte() { Test("Test_CONV_I2__Byte", byte.MinValue); } /* TestMethod("Test_CONV_I2__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_I2__Byte() { Test("Test_CONV_I2__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_I2__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_I2__Byte() { Test("Test_CONV_I2__Byte", (byte)0x12); } /* TestMethod("Test_CONV_I2__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_I2__Byte() { Test("Test_CONV_I2__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_I2__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_I4__Byte() { Test("Test_CONV_I4__Byte", byte.MinValue); } /* TestMethod("Test_CONV_I4__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_I4__Byte() { Test("Test_CONV_I4__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_I4__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_I4__Byte() { Test("Test_CONV_I4__Byte", (byte)0x12); } /* TestMethod("Test_CONV_I4__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_I4__Byte() { Test("Test_CONV_I4__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_I4__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_I8__Byte() { Test("Test_CONV_I8__Byte", byte.MinValue); } /* TestMethod("Test_CONV_I8__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_I8__Byte() { Test("Test_CONV_I8__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_I8__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_I8__Byte() { Test("Test_CONV_I8__Byte", (byte)0x12); } /* TestMethod("Test_CONV_I8__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_I8__Byte() { Test("Test_CONV_I8__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_I8__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_U__Byte() { Test("Test_CONV_U__Byte", byte.MinValue); } /* TestMethod("Test_CONV_U__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_U__Byte() { Test("Test_CONV_U__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_U__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_U__Byte() { Test("Test_CONV_U__Byte", (byte)0x12); } /* TestMethod("Test_CONV_U__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_U__Byte() { Test("Test_CONV_U__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_U__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_U1__Byte() { Test("Test_CONV_U1__Byte", byte.MinValue); } /* TestMethod("Test_CONV_U1__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_U1__Byte() { Test("Test_CONV_U1__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_U1__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_U1__Byte() { Test("Test_CONV_U1__Byte", (byte)0x12); } /* TestMethod("Test_CONV_U1__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_U1__Byte() { Test("Test_CONV_U1__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_U1__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_U2__Byte() { Test("Test_CONV_U2__Byte", byte.MinValue); } /* TestMethod("Test_CONV_U2__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_U2__Byte() { Test("Test_CONV_U2__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_U2__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_U2__Byte() { Test("Test_CONV_U2__Byte", (byte)0x12); } /* TestMethod("Test_CONV_U2__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_U2__Byte() { Test("Test_CONV_U2__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_U2__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_U4__Byte() { Test("Test_CONV_U4__Byte", byte.MinValue); } /* TestMethod("Test_CONV_U4__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_U4__Byte() { Test("Test_CONV_U4__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_U4__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_U4__Byte() { Test("Test_CONV_U4__Byte", (byte)0x12); } /* TestMethod("Test_CONV_U4__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_U4__Byte() { Test("Test_CONV_U4__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_U4__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_U8__Byte() { Test("Test_CONV_U8__Byte", byte.MinValue); } /* TestMethod("Test_CONV_U8__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_U8__Byte() { Test("Test_CONV_U8__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_U8__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_U8__Byte() { Test("Test_CONV_U8__Byte", (byte)0x12); } /* TestMethod("Test_CONV_U8__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_U8__Byte() { Test("Test_CONV_U8__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_U8__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_R4__Byte() { Test("Test_CONV_R4__Byte", byte.MinValue); } /* TestMethod("Test_CONV_R4__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_R4__Byte() { Test("Test_CONV_R4__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_R4__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_R4__Byte() { Test("Test_CONV_R4__Byte", (byte)0x12); } /* TestMethod("Test_CONV_R4__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_R4__Byte() { Test("Test_CONV_R4__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_R4__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_R8__Byte() { Test("Test_CONV_R8__Byte", byte.MinValue); } /* TestMethod("Test_CONV_R8__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_R8__Byte() { Test("Test_CONV_R8__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_R8__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_R8__Byte() { Test("Test_CONV_R8__Byte", (byte)0x12); } /* TestMethod("Test_CONV_R8__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_R8__Byte() { Test("Test_CONV_R8__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_R8__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_OVF_I__Byte() { Test("Test_CONV_OVF_I__Byte", byte.MinValue); } /* TestMethod("Test_CONV_OVF_I__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_OVF_I__Byte() { Test("Test_CONV_OVF_I__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_OVF_I__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I__Byte() { Test("Test_CONV_OVF_I__Byte", (byte)0x12); } /* TestMethod("Test_CONV_OVF_I__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_OVF_I__Byte() { Test("Test_CONV_OVF_I__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_OVF_I__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_OVF_I1__Byte() { Test("Test_CONV_OVF_I1__Byte", byte.MinValue); } /* TestMethod("Test_CONV_OVF_I1__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_OVF_I1__Byte() { Test("Test_CONV_OVF_I1__Byte", byte.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I1__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I1__Byte() { Test("Test_CONV_OVF_I1__Byte", (byte)0x12); } /* TestMethod("Test_CONV_OVF_I1__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_OVF_I1__Byte() { Test("Test_CONV_OVF_I1__Byte", (byte)0x9A); } /* TestMethodEX("Test_CONV_OVF_I1__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_OVF_I2__Byte() { Test("Test_CONV_OVF_I2__Byte", byte.MinValue); } /* TestMethod("Test_CONV_OVF_I2__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_OVF_I2__Byte() { Test("Test_CONV_OVF_I2__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_OVF_I2__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I2__Byte() { Test("Test_CONV_OVF_I2__Byte", (byte)0x12); } /* TestMethod("Test_CONV_OVF_I2__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_OVF_I2__Byte() { Test("Test_CONV_OVF_I2__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_OVF_I2__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_OVF_I4__Byte() { Test("Test_CONV_OVF_I4__Byte", byte.MinValue); } /* TestMethod("Test_CONV_OVF_I4__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_OVF_I4__Byte() { Test("Test_CONV_OVF_I4__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_OVF_I4__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I4__Byte() { Test("Test_CONV_OVF_I4__Byte", (byte)0x12); } /* TestMethod("Test_CONV_OVF_I4__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_OVF_I4__Byte() { Test("Test_CONV_OVF_I4__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_OVF_I4__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_OVF_I8__Byte() { Test("Test_CONV_OVF_I8__Byte", byte.MinValue); } /* TestMethod("Test_CONV_OVF_I8__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_OVF_I8__Byte() { Test("Test_CONV_OVF_I8__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_OVF_I8__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I8__Byte() { Test("Test_CONV_OVF_I8__Byte", (byte)0x12); } /* TestMethod("Test_CONV_OVF_I8__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_OVF_I8__Byte() { Test("Test_CONV_OVF_I8__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_OVF_I8__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_OVF_U__Byte() { Test("Test_CONV_OVF_U__Byte", byte.MinValue); } /* TestMethod("Test_CONV_OVF_U__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_OVF_U__Byte() { Test("Test_CONV_OVF_U__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_OVF_U__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U__Byte() { Test("Test_CONV_OVF_U__Byte", (byte)0x12); } /* TestMethod("Test_CONV_OVF_U__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_OVF_U__Byte() { Test("Test_CONV_OVF_U__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_OVF_U__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_OVF_U1__Byte() { Test("Test_CONV_OVF_U1__Byte", byte.MinValue); } /* TestMethod("Test_CONV_OVF_U1__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_OVF_U1__Byte() { Test("Test_CONV_OVF_U1__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_OVF_U1__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U1__Byte() { Test("Test_CONV_OVF_U1__Byte", (byte)0x12); } /* TestMethod("Test_CONV_OVF_U1__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_OVF_U1__Byte() { Test("Test_CONV_OVF_U1__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_OVF_U1__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_OVF_U2__Byte() { Test("Test_CONV_OVF_U2__Byte", byte.MinValue); } /* TestMethod("Test_CONV_OVF_U2__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_OVF_U2__Byte() { Test("Test_CONV_OVF_U2__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_OVF_U2__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U2__Byte() { Test("Test_CONV_OVF_U2__Byte", (byte)0x12); } /* TestMethod("Test_CONV_OVF_U2__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_OVF_U2__Byte() { Test("Test_CONV_OVF_U2__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_OVF_U2__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_OVF_U4__Byte() { Test("Test_CONV_OVF_U4__Byte", byte.MinValue); } /* TestMethod("Test_CONV_OVF_U4__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_OVF_U4__Byte() { Test("Test_CONV_OVF_U4__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_OVF_U4__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U4__Byte() { Test("Test_CONV_OVF_U4__Byte", (byte)0x12); } /* TestMethod("Test_CONV_OVF_U4__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_OVF_U4__Byte() { Test("Test_CONV_OVF_U4__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_OVF_U4__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_OVF_U8__Byte() { Test("Test_CONV_OVF_U8__Byte", byte.MinValue); } /* TestMethod("Test_CONV_OVF_U8__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_OVF_U8__Byte() { Test("Test_CONV_OVF_U8__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_OVF_U8__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U8__Byte() { Test("Test_CONV_OVF_U8__Byte", (byte)0x12); } /* TestMethod("Test_CONV_OVF_U8__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_OVF_U8__Byte() { Test("Test_CONV_OVF_U8__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_OVF_U8__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_OVF_I_UN__Byte() { Test("Test_CONV_OVF_I_UN__Byte", byte.MinValue); } /* TestMethod("Test_CONV_OVF_I_UN__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_OVF_I_UN__Byte() { Test("Test_CONV_OVF_I_UN__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_OVF_I_UN__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I_UN__Byte() { Test("Test_CONV_OVF_I_UN__Byte", (byte)0x12); } /* TestMethod("Test_CONV_OVF_I_UN__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_OVF_I_UN__Byte() { Test("Test_CONV_OVF_I_UN__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_OVF_I_UN__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Byte() { Test("Test_CONV_OVF_I1_UN__Byte", byte.MinValue); } /* TestMethod("Test_CONV_OVF_I1_UN__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Byte() { Test("Test_CONV_OVF_I1_UN__Byte", byte.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I1_UN__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Byte() { Test("Test_CONV_OVF_I1_UN__Byte", (byte)0x12); } /* TestMethod("Test_CONV_OVF_I1_UN__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_OVF_I1_UN__Byte() { Test("Test_CONV_OVF_I1_UN__Byte", (byte)0x9A); } /* TestMethodEX("Test_CONV_OVF_I1_UN__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Byte() { Test("Test_CONV_OVF_I2_UN__Byte", byte.MinValue); } /* TestMethod("Test_CONV_OVF_I2_UN__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Byte() { Test("Test_CONV_OVF_I2_UN__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_OVF_I2_UN__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Byte() { Test("Test_CONV_OVF_I2_UN__Byte", (byte)0x12); } /* TestMethod("Test_CONV_OVF_I2_UN__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_OVF_I2_UN__Byte() { Test("Test_CONV_OVF_I2_UN__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_OVF_I2_UN__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Byte() { Test("Test_CONV_OVF_I4_UN__Byte", byte.MinValue); } /* TestMethod("Test_CONV_OVF_I4_UN__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Byte() { Test("Test_CONV_OVF_I4_UN__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_OVF_I4_UN__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Byte() { Test("Test_CONV_OVF_I4_UN__Byte", (byte)0x12); } /* TestMethod("Test_CONV_OVF_I4_UN__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_OVF_I4_UN__Byte() { Test("Test_CONV_OVF_I4_UN__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_OVF_I4_UN__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Byte() { Test("Test_CONV_OVF_I8_UN__Byte", byte.MinValue); } /* TestMethod("Test_CONV_OVF_I8_UN__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Byte() { Test("Test_CONV_OVF_I8_UN__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_OVF_I8_UN__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Byte() { Test("Test_CONV_OVF_I8_UN__Byte", (byte)0x12); } /* TestMethod("Test_CONV_OVF_I8_UN__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_OVF_I8_UN__Byte() { Test("Test_CONV_OVF_I8_UN__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_OVF_I8_UN__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_OVF_U_UN__Byte() { Test("Test_CONV_OVF_U_UN__Byte", byte.MinValue); } /* TestMethod("Test_CONV_OVF_U_UN__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_OVF_U_UN__Byte() { Test("Test_CONV_OVF_U_UN__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_OVF_U_UN__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U_UN__Byte() { Test("Test_CONV_OVF_U_UN__Byte", (byte)0x12); } /* TestMethod("Test_CONV_OVF_U_UN__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_OVF_U_UN__Byte() { Test("Test_CONV_OVF_U_UN__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_OVF_U_UN__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Byte() { Test("Test_CONV_OVF_U1_UN__Byte", byte.MinValue); } /* TestMethod("Test_CONV_OVF_U1_UN__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Byte() { Test("Test_CONV_OVF_U1_UN__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_OVF_U1_UN__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Byte() { Test("Test_CONV_OVF_U1_UN__Byte", (byte)0x12); } /* TestMethod("Test_CONV_OVF_U1_UN__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_OVF_U1_UN__Byte() { Test("Test_CONV_OVF_U1_UN__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_OVF_U1_UN__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Byte() { Test("Test_CONV_OVF_U2_UN__Byte", byte.MinValue); } /* TestMethod("Test_CONV_OVF_U2_UN__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Byte() { Test("Test_CONV_OVF_U2_UN__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_OVF_U2_UN__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Byte() { Test("Test_CONV_OVF_U2_UN__Byte", (byte)0x12); } /* TestMethod("Test_CONV_OVF_U2_UN__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_OVF_U2_UN__Byte() { Test("Test_CONV_OVF_U2_UN__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_OVF_U2_UN__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Byte() { Test("Test_CONV_OVF_U4_UN__Byte", byte.MinValue); } /* TestMethod("Test_CONV_OVF_U4_UN__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Byte() { Test("Test_CONV_OVF_U4_UN__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_OVF_U4_UN__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Byte() { Test("Test_CONV_OVF_U4_UN__Byte", (byte)0x12); } /* TestMethod("Test_CONV_OVF_U4_UN__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_OVF_U4_UN__Byte() { Test("Test_CONV_OVF_U4_UN__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_OVF_U4_UN__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Byte() { Test("Test_CONV_OVF_U8_UN__Byte", byte.MinValue); } /* TestMethod("Test_CONV_OVF_U8_UN__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Byte() { Test("Test_CONV_OVF_U8_UN__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_OVF_U8_UN__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Byte() { Test("Test_CONV_OVF_U8_UN__Byte", (byte)0x12); } /* TestMethod("Test_CONV_OVF_U8_UN__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_OVF_U8_UN__Byte() { Test("Test_CONV_OVF_U8_UN__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_OVF_U8_UN__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_R_UN__Byte() { Test("Test_CONV_R_UN__Byte", byte.MinValue); } /* TestMethod("Test_CONV_R_UN__Byte", byte.MinValue); */
        //[Fact] public void Test_CONV_R_UN__Byte() { Test("Test_CONV_R_UN__Byte", byte.MaxValue); } /* TestMethod("Test_CONV_R_UN__Byte", byte.MaxValue); */
        //[Fact] public void Test_CONV_R_UN__Byte() { Test("Test_CONV_R_UN__Byte", (byte)0x12); } /* TestMethod("Test_CONV_R_UN__Byte", (byte)0x12); */
        //[Fact] public void Test_CONV_R_UN__Byte() { Test("Test_CONV_R_UN__Byte", (byte)0x9A); } /* TestMethod("Test_CONV_R_UN__Byte", (byte)0x9A); */
        //[Fact] public void Test_CONV_I__UInt16() { Test("Test_CONV_I__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_I__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_I__UInt16() { Test("Test_CONV_I__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_I__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_I__UInt16() { Test("Test_CONV_I__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_I__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_I__UInt16() { Test("Test_CONV_I__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_I__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_I1__UInt16() { Test("Test_CONV_I1__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_I1__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_I1__UInt16() { Test("Test_CONV_I1__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_I1__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_I1__UInt16() { Test("Test_CONV_I1__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_I1__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_I1__UInt16() { Test("Test_CONV_I1__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_I1__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_I2__UInt16() { Test("Test_CONV_I2__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_I2__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_I2__UInt16() { Test("Test_CONV_I2__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_I2__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_I2__UInt16() { Test("Test_CONV_I2__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_I2__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_I2__UInt16() { Test("Test_CONV_I2__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_I2__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_I4__UInt16() { Test("Test_CONV_I4__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_I4__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_I4__UInt16() { Test("Test_CONV_I4__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_I4__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_I4__UInt16() { Test("Test_CONV_I4__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_I4__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_I4__UInt16() { Test("Test_CONV_I4__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_I4__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_I8__UInt16() { Test("Test_CONV_I8__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_I8__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_I8__UInt16() { Test("Test_CONV_I8__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_I8__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_I8__UInt16() { Test("Test_CONV_I8__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_I8__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_I8__UInt16() { Test("Test_CONV_I8__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_I8__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_U__UInt16() { Test("Test_CONV_U__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_U__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_U__UInt16() { Test("Test_CONV_U__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_U__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_U__UInt16() { Test("Test_CONV_U__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_U__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_U__UInt16() { Test("Test_CONV_U__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_U__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_U1__UInt16() { Test("Test_CONV_U1__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_U1__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_U1__UInt16() { Test("Test_CONV_U1__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_U1__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_U1__UInt16() { Test("Test_CONV_U1__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_U1__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_U1__UInt16() { Test("Test_CONV_U1__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_U1__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_U2__UInt16() { Test("Test_CONV_U2__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_U2__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_U2__UInt16() { Test("Test_CONV_U2__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_U2__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_U2__UInt16() { Test("Test_CONV_U2__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_U2__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_U2__UInt16() { Test("Test_CONV_U2__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_U2__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_U4__UInt16() { Test("Test_CONV_U4__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_U4__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_U4__UInt16() { Test("Test_CONV_U4__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_U4__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_U4__UInt16() { Test("Test_CONV_U4__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_U4__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_U4__UInt16() { Test("Test_CONV_U4__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_U4__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_U8__UInt16() { Test("Test_CONV_U8__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_U8__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_U8__UInt16() { Test("Test_CONV_U8__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_U8__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_U8__UInt16() { Test("Test_CONV_U8__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_U8__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_U8__UInt16() { Test("Test_CONV_U8__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_U8__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_R4__UInt16() { Test("Test_CONV_R4__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_R4__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_R4__UInt16() { Test("Test_CONV_R4__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_R4__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_R4__UInt16() { Test("Test_CONV_R4__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_R4__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_R4__UInt16() { Test("Test_CONV_R4__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_R4__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_R8__UInt16() { Test("Test_CONV_R8__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_R8__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_R8__UInt16() { Test("Test_CONV_R8__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_R8__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_R8__UInt16() { Test("Test_CONV_R8__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_R8__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_R8__UInt16() { Test("Test_CONV_R8__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_R8__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_I__UInt16() { Test("Test_CONV_OVF_I__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_OVF_I__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_OVF_I__UInt16() { Test("Test_CONV_OVF_I__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_OVF_I__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I__UInt16() { Test("Test_CONV_OVF_I__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_OVF_I__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_OVF_I__UInt16() { Test("Test_CONV_OVF_I__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_OVF_I__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_I1__UInt16() { Test("Test_CONV_OVF_I1__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_OVF_I1__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_OVF_I1__UInt16() { Test("Test_CONV_OVF_I1__UInt16", ushort.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I1__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I1__UInt16() { Test("Test_CONV_OVF_I1__UInt16", (ushort)0x1234); } /* TestMethodEX("Test_CONV_OVF_I1__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_OVF_I1__UInt16() { Test("Test_CONV_OVF_I1__UInt16", (ushort)0x9ABC); } /* TestMethodEX("Test_CONV_OVF_I1__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_I2__UInt16() { Test("Test_CONV_OVF_I2__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_OVF_I2__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_OVF_I2__UInt16() { Test("Test_CONV_OVF_I2__UInt16", ushort.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I2__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I2__UInt16() { Test("Test_CONV_OVF_I2__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_OVF_I2__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_OVF_I2__UInt16() { Test("Test_CONV_OVF_I2__UInt16", (ushort)0x9ABC); } /* TestMethodEX("Test_CONV_OVF_I2__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_I4__UInt16() { Test("Test_CONV_OVF_I4__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_OVF_I4__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_OVF_I4__UInt16() { Test("Test_CONV_OVF_I4__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_OVF_I4__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I4__UInt16() { Test("Test_CONV_OVF_I4__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_OVF_I4__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_OVF_I4__UInt16() { Test("Test_CONV_OVF_I4__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_OVF_I4__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_I8__UInt16() { Test("Test_CONV_OVF_I8__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_OVF_I8__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_OVF_I8__UInt16() { Test("Test_CONV_OVF_I8__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_OVF_I8__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I8__UInt16() { Test("Test_CONV_OVF_I8__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_OVF_I8__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_OVF_I8__UInt16() { Test("Test_CONV_OVF_I8__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_OVF_I8__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_U__UInt16() { Test("Test_CONV_OVF_U__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_OVF_U__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_OVF_U__UInt16() { Test("Test_CONV_OVF_U__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_OVF_U__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U__UInt16() { Test("Test_CONV_OVF_U__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_OVF_U__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_OVF_U__UInt16() { Test("Test_CONV_OVF_U__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_OVF_U__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_U1__UInt16() { Test("Test_CONV_OVF_U1__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_OVF_U1__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_OVF_U1__UInt16() { Test("Test_CONV_OVF_U1__UInt16", ushort.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U1__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U1__UInt16() { Test("Test_CONV_OVF_U1__UInt16", (ushort)0x1234); } /* TestMethodEX("Test_CONV_OVF_U1__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_OVF_U1__UInt16() { Test("Test_CONV_OVF_U1__UInt16", (ushort)0x9ABC); } /* TestMethodEX("Test_CONV_OVF_U1__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_U2__UInt16() { Test("Test_CONV_OVF_U2__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_OVF_U2__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_OVF_U2__UInt16() { Test("Test_CONV_OVF_U2__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_OVF_U2__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U2__UInt16() { Test("Test_CONV_OVF_U2__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_OVF_U2__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_OVF_U2__UInt16() { Test("Test_CONV_OVF_U2__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_OVF_U2__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_U4__UInt16() { Test("Test_CONV_OVF_U4__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_OVF_U4__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_OVF_U4__UInt16() { Test("Test_CONV_OVF_U4__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_OVF_U4__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U4__UInt16() { Test("Test_CONV_OVF_U4__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_OVF_U4__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_OVF_U4__UInt16() { Test("Test_CONV_OVF_U4__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_OVF_U4__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_U8__UInt16() { Test("Test_CONV_OVF_U8__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_OVF_U8__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_OVF_U8__UInt16() { Test("Test_CONV_OVF_U8__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_OVF_U8__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U8__UInt16() { Test("Test_CONV_OVF_U8__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_OVF_U8__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_OVF_U8__UInt16() { Test("Test_CONV_OVF_U8__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_OVF_U8__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_I_UN__UInt16() { Test("Test_CONV_OVF_I_UN__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_OVF_I_UN__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_OVF_I_UN__UInt16() { Test("Test_CONV_OVF_I_UN__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_OVF_I_UN__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I_UN__UInt16() { Test("Test_CONV_OVF_I_UN__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_OVF_I_UN__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_OVF_I_UN__UInt16() { Test("Test_CONV_OVF_I_UN__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_OVF_I_UN__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_I1_UN__UInt16() { Test("Test_CONV_OVF_I1_UN__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_OVF_I1_UN__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_OVF_I1_UN__UInt16() { Test("Test_CONV_OVF_I1_UN__UInt16", ushort.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I1_UN__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I1_UN__UInt16() { Test("Test_CONV_OVF_I1_UN__UInt16", (ushort)0x1234); } /* TestMethodEX("Test_CONV_OVF_I1_UN__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_OVF_I1_UN__UInt16() { Test("Test_CONV_OVF_I1_UN__UInt16", (ushort)0x9ABC); } /* TestMethodEX("Test_CONV_OVF_I1_UN__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_I2_UN__UInt16() { Test("Test_CONV_OVF_I2_UN__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_OVF_I2_UN__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_OVF_I2_UN__UInt16() { Test("Test_CONV_OVF_I2_UN__UInt16", ushort.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I2_UN__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I2_UN__UInt16() { Test("Test_CONV_OVF_I2_UN__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_OVF_I2_UN__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_OVF_I2_UN__UInt16() { Test("Test_CONV_OVF_I2_UN__UInt16", (ushort)0x9ABC); } /* TestMethodEX("Test_CONV_OVF_I2_UN__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_I4_UN__UInt16() { Test("Test_CONV_OVF_I4_UN__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_OVF_I4_UN__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_OVF_I4_UN__UInt16() { Test("Test_CONV_OVF_I4_UN__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_OVF_I4_UN__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I4_UN__UInt16() { Test("Test_CONV_OVF_I4_UN__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_OVF_I4_UN__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_OVF_I4_UN__UInt16() { Test("Test_CONV_OVF_I4_UN__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_OVF_I4_UN__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_I8_UN__UInt16() { Test("Test_CONV_OVF_I8_UN__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_OVF_I8_UN__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_OVF_I8_UN__UInt16() { Test("Test_CONV_OVF_I8_UN__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_OVF_I8_UN__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I8_UN__UInt16() { Test("Test_CONV_OVF_I8_UN__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_OVF_I8_UN__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_OVF_I8_UN__UInt16() { Test("Test_CONV_OVF_I8_UN__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_OVF_I8_UN__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_U_UN__UInt16() { Test("Test_CONV_OVF_U_UN__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_OVF_U_UN__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_OVF_U_UN__UInt16() { Test("Test_CONV_OVF_U_UN__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_OVF_U_UN__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U_UN__UInt16() { Test("Test_CONV_OVF_U_UN__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_OVF_U_UN__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_OVF_U_UN__UInt16() { Test("Test_CONV_OVF_U_UN__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_OVF_U_UN__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_U1_UN__UInt16() { Test("Test_CONV_OVF_U1_UN__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_OVF_U1_UN__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_OVF_U1_UN__UInt16() { Test("Test_CONV_OVF_U1_UN__UInt16", ushort.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U1_UN__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U1_UN__UInt16() { Test("Test_CONV_OVF_U1_UN__UInt16", (ushort)0x1234); } /* TestMethodEX("Test_CONV_OVF_U1_UN__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_OVF_U1_UN__UInt16() { Test("Test_CONV_OVF_U1_UN__UInt16", (ushort)0x9ABC); } /* TestMethodEX("Test_CONV_OVF_U1_UN__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_U2_UN__UInt16() { Test("Test_CONV_OVF_U2_UN__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_OVF_U2_UN__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_OVF_U2_UN__UInt16() { Test("Test_CONV_OVF_U2_UN__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_OVF_U2_UN__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U2_UN__UInt16() { Test("Test_CONV_OVF_U2_UN__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_OVF_U2_UN__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_OVF_U2_UN__UInt16() { Test("Test_CONV_OVF_U2_UN__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_OVF_U2_UN__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_U4_UN__UInt16() { Test("Test_CONV_OVF_U4_UN__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_OVF_U4_UN__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_OVF_U4_UN__UInt16() { Test("Test_CONV_OVF_U4_UN__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_OVF_U4_UN__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U4_UN__UInt16() { Test("Test_CONV_OVF_U4_UN__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_OVF_U4_UN__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_OVF_U4_UN__UInt16() { Test("Test_CONV_OVF_U4_UN__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_OVF_U4_UN__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_OVF_U8_UN__UInt16() { Test("Test_CONV_OVF_U8_UN__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_OVF_U8_UN__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_OVF_U8_UN__UInt16() { Test("Test_CONV_OVF_U8_UN__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_OVF_U8_UN__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U8_UN__UInt16() { Test("Test_CONV_OVF_U8_UN__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_OVF_U8_UN__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_OVF_U8_UN__UInt16() { Test("Test_CONV_OVF_U8_UN__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_OVF_U8_UN__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_R_UN__UInt16() { Test("Test_CONV_R_UN__UInt16", ushort.MinValue); } /* TestMethod("Test_CONV_R_UN__UInt16", ushort.MinValue); */
        //[Fact] public void Test_CONV_R_UN__UInt16() { Test("Test_CONV_R_UN__UInt16", ushort.MaxValue); } /* TestMethod("Test_CONV_R_UN__UInt16", ushort.MaxValue); */
        //[Fact] public void Test_CONV_R_UN__UInt16() { Test("Test_CONV_R_UN__UInt16", (ushort)0x1234); } /* TestMethod("Test_CONV_R_UN__UInt16", (ushort)0x1234); */
        //[Fact] public void Test_CONV_R_UN__UInt16() { Test("Test_CONV_R_UN__UInt16", (ushort)0x9ABC); } /* TestMethod("Test_CONV_R_UN__UInt16", (ushort)0x9ABC); */
        //[Fact] public void Test_CONV_I__UInt32() { Test("Test_CONV_I__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_I__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_I__UInt32() { Test("Test_CONV_I__UInt32", uint.MaxValue); } /* TestMethod("Test_CONV_I__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_I__UInt32() { Test("Test_CONV_I__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_I__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_I__UInt32() { Test("Test_CONV_I__UInt32", (uint)0x9ABCDEF0); } /* TestMethod("Test_CONV_I__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_I1__UInt32() { Test("Test_CONV_I1__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_I1__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_I1__UInt32() { Test("Test_CONV_I1__UInt32", uint.MaxValue); } /* TestMethod("Test_CONV_I1__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_I1__UInt32() { Test("Test_CONV_I1__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_I1__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_I1__UInt32() { Test("Test_CONV_I1__UInt32", (uint)0x9ABCDEF0); } /* TestMethod("Test_CONV_I1__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_I2__UInt32() { Test("Test_CONV_I2__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_I2__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_I2__UInt32() { Test("Test_CONV_I2__UInt32", uint.MaxValue); } /* TestMethod("Test_CONV_I2__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_I2__UInt32() { Test("Test_CONV_I2__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_I2__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_I2__UInt32() { Test("Test_CONV_I2__UInt32", (uint)0x9ABCDEF0); } /* TestMethod("Test_CONV_I2__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_I4__UInt32() { Test("Test_CONV_I4__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_I4__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_I4__UInt32() { Test("Test_CONV_I4__UInt32", uint.MaxValue); } /* TestMethod("Test_CONV_I4__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_I4__UInt32() { Test("Test_CONV_I4__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_I4__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_I4__UInt32() { Test("Test_CONV_I4__UInt32", (uint)0x9ABCDEF0); } /* TestMethod("Test_CONV_I4__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_I8__UInt32() { Test("Test_CONV_I8__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_I8__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_I8__UInt32() { Test("Test_CONV_I8__UInt32", uint.MaxValue); } /* TestMethod("Test_CONV_I8__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_I8__UInt32() { Test("Test_CONV_I8__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_I8__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_I8__UInt32() { Test("Test_CONV_I8__UInt32", (uint)0x9ABCDEF0); } /* TestMethod("Test_CONV_I8__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_U__UInt32() { Test("Test_CONV_U__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_U__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_U__UInt32() { Test("Test_CONV_U__UInt32", uint.MaxValue); } /* TestMethod("Test_CONV_U__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_U__UInt32() { Test("Test_CONV_U__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_U__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_U__UInt32() { Test("Test_CONV_U__UInt32", (uint)0x9ABCDEF0); } /* TestMethod("Test_CONV_U__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_U1__UInt32() { Test("Test_CONV_U1__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_U1__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_U1__UInt32() { Test("Test_CONV_U1__UInt32", uint.MaxValue); } /* TestMethod("Test_CONV_U1__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_U1__UInt32() { Test("Test_CONV_U1__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_U1__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_U1__UInt32() { Test("Test_CONV_U1__UInt32", (uint)0x9ABCDEF0); } /* TestMethod("Test_CONV_U1__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_U2__UInt32() { Test("Test_CONV_U2__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_U2__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_U2__UInt32() { Test("Test_CONV_U2__UInt32", uint.MaxValue); } /* TestMethod("Test_CONV_U2__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_U2__UInt32() { Test("Test_CONV_U2__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_U2__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_U2__UInt32() { Test("Test_CONV_U2__UInt32", (uint)0x9ABCDEF0); } /* TestMethod("Test_CONV_U2__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_U4__UInt32() { Test("Test_CONV_U4__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_U4__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_U4__UInt32() { Test("Test_CONV_U4__UInt32", uint.MaxValue); } /* TestMethod("Test_CONV_U4__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_U4__UInt32() { Test("Test_CONV_U4__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_U4__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_U4__UInt32() { Test("Test_CONV_U4__UInt32", (uint)0x9ABCDEF0); } /* TestMethod("Test_CONV_U4__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_U8__UInt32() { Test("Test_CONV_U8__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_U8__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_U8__UInt32() { Test("Test_CONV_U8__UInt32", uint.MaxValue); } /* TestMethod("Test_CONV_U8__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_U8__UInt32() { Test("Test_CONV_U8__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_U8__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_U8__UInt32() { Test("Test_CONV_U8__UInt32", (uint)0x9ABCDEF0); } /* TestMethod("Test_CONV_U8__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_R4__UInt32() { Test("Test_CONV_R4__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_R4__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_R4__UInt32() { Test("Test_CONV_R4__UInt32", uint.MaxValue); } /* TestMethod("Test_CONV_R4__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_R4__UInt32() { Test("Test_CONV_R4__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_R4__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_R4__UInt32() { Test("Test_CONV_R4__UInt32", (uint)0x9ABCDEF0); } /* TestMethod("Test_CONV_R4__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_R8__UInt32() { Test("Test_CONV_R8__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_R8__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_R8__UInt32() { Test("Test_CONV_R8__UInt32", uint.MaxValue); } /* TestMethod("Test_CONV_R8__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_R8__UInt32() { Test("Test_CONV_R8__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_R8__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_R8__UInt32() { Test("Test_CONV_R8__UInt32", (uint)0x9ABCDEF0); } /* TestMethod("Test_CONV_R8__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I__UInt32() { Test("Test_CONV_OVF_I__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_OVF_I__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_OVF_I__UInt32() { Test("Test_CONV_OVF_I__UInt32", uint.MaxValue); } /* TestMethod("Test_CONV_OVF_I__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I__UInt32() { Test("Test_CONV_OVF_I__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_OVF_I__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_OVF_I__UInt32() { Test("Test_CONV_OVF_I__UInt32", (uint)0x9ABCDEF0); } /* TestMethod("Test_CONV_OVF_I__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I1__UInt32() { Test("Test_CONV_OVF_I1__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_OVF_I1__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_OVF_I1__UInt32() { Test("Test_CONV_OVF_I1__UInt32", uint.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I1__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I1__UInt32() { Test("Test_CONV_OVF_I1__UInt32", (uint)0x12345678); } /* TestMethodEX("Test_CONV_OVF_I1__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_OVF_I1__UInt32() { Test("Test_CONV_OVF_I1__UInt32", (uint)0x9ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I1__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I2__UInt32() { Test("Test_CONV_OVF_I2__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_OVF_I2__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_OVF_I2__UInt32() { Test("Test_CONV_OVF_I2__UInt32", uint.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I2__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I2__UInt32() { Test("Test_CONV_OVF_I2__UInt32", (uint)0x12345678); } /* TestMethodEX("Test_CONV_OVF_I2__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_OVF_I2__UInt32() { Test("Test_CONV_OVF_I2__UInt32", (uint)0x9ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I2__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I4__UInt32() { Test("Test_CONV_OVF_I4__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_OVF_I4__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_OVF_I4__UInt32() { Test("Test_CONV_OVF_I4__UInt32", uint.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I4__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I4__UInt32() { Test("Test_CONV_OVF_I4__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_OVF_I4__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_OVF_I4__UInt32() { Test("Test_CONV_OVF_I4__UInt32", (uint)0x9ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I4__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I8__UInt32() { Test("Test_CONV_OVF_I8__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_OVF_I8__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_OVF_I8__UInt32() { Test("Test_CONV_OVF_I8__UInt32", uint.MaxValue); } /* TestMethod("Test_CONV_OVF_I8__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I8__UInt32() { Test("Test_CONV_OVF_I8__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_OVF_I8__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_OVF_I8__UInt32() { Test("Test_CONV_OVF_I8__UInt32", (uint)0x9ABCDEF0); } /* TestMethod("Test_CONV_OVF_I8__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U__UInt32() { Test("Test_CONV_OVF_U__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_OVF_U__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_OVF_U__UInt32() { Test("Test_CONV_OVF_U__UInt32", uint.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U__UInt32() { Test("Test_CONV_OVF_U__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_OVF_U__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_OVF_U__UInt32() { Test("Test_CONV_OVF_U__UInt32", (uint)0x9ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U1__UInt32() { Test("Test_CONV_OVF_U1__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_OVF_U1__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_OVF_U1__UInt32() { Test("Test_CONV_OVF_U1__UInt32", uint.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U1__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U1__UInt32() { Test("Test_CONV_OVF_U1__UInt32", (uint)0x12345678); } /* TestMethodEX("Test_CONV_OVF_U1__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_OVF_U1__UInt32() { Test("Test_CONV_OVF_U1__UInt32", (uint)0x9ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U1__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U2__UInt32() { Test("Test_CONV_OVF_U2__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_OVF_U2__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_OVF_U2__UInt32() { Test("Test_CONV_OVF_U2__UInt32", uint.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U2__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U2__UInt32() { Test("Test_CONV_OVF_U2__UInt32", (uint)0x12345678); } /* TestMethodEX("Test_CONV_OVF_U2__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_OVF_U2__UInt32() { Test("Test_CONV_OVF_U2__UInt32", (uint)0x9ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U2__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U4__UInt32() { Test("Test_CONV_OVF_U4__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_OVF_U4__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_OVF_U4__UInt32() { Test("Test_CONV_OVF_U4__UInt32", uint.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U4__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U4__UInt32() { Test("Test_CONV_OVF_U4__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_OVF_U4__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_OVF_U4__UInt32() { Test("Test_CONV_OVF_U4__UInt32", (uint)0x9ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U4__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U8__UInt32() { Test("Test_CONV_OVF_U8__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_OVF_U8__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_OVF_U8__UInt32() { Test("Test_CONV_OVF_U8__UInt32", uint.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U8__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U8__UInt32() { Test("Test_CONV_OVF_U8__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_OVF_U8__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_OVF_U8__UInt32() { Test("Test_CONV_OVF_U8__UInt32", (uint)0x9ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U8__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I_UN__UInt32() { Test("Test_CONV_OVF_I_UN__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_OVF_I_UN__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_OVF_I_UN__UInt32() { Test("Test_CONV_OVF_I_UN__UInt32", uint.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I_UN__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I_UN__UInt32() { Test("Test_CONV_OVF_I_UN__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_OVF_I_UN__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_OVF_I_UN__UInt32() { Test("Test_CONV_OVF_I_UN__UInt32", (uint)0x9ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I_UN__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I1_UN__UInt32() { Test("Test_CONV_OVF_I1_UN__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_OVF_I1_UN__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_OVF_I1_UN__UInt32() { Test("Test_CONV_OVF_I1_UN__UInt32", uint.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I1_UN__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I1_UN__UInt32() { Test("Test_CONV_OVF_I1_UN__UInt32", (uint)0x12345678); } /* TestMethodEX("Test_CONV_OVF_I1_UN__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_OVF_I1_UN__UInt32() { Test("Test_CONV_OVF_I1_UN__UInt32", (uint)0x9ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I1_UN__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I2_UN__UInt32() { Test("Test_CONV_OVF_I2_UN__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_OVF_I2_UN__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_OVF_I2_UN__UInt32() { Test("Test_CONV_OVF_I2_UN__UInt32", uint.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I2_UN__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I2_UN__UInt32() { Test("Test_CONV_OVF_I2_UN__UInt32", (uint)0x12345678); } /* TestMethodEX("Test_CONV_OVF_I2_UN__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_OVF_I2_UN__UInt32() { Test("Test_CONV_OVF_I2_UN__UInt32", (uint)0x9ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I2_UN__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I4_UN__UInt32() { Test("Test_CONV_OVF_I4_UN__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_OVF_I4_UN__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_OVF_I4_UN__UInt32() { Test("Test_CONV_OVF_I4_UN__UInt32", uint.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I4_UN__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I4_UN__UInt32() { Test("Test_CONV_OVF_I4_UN__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_OVF_I4_UN__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_OVF_I4_UN__UInt32() { Test("Test_CONV_OVF_I4_UN__UInt32", (uint)0x9ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I4_UN__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I8_UN__UInt32() { Test("Test_CONV_OVF_I8_UN__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_OVF_I8_UN__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_OVF_I8_UN__UInt32() { Test("Test_CONV_OVF_I8_UN__UInt32", uint.MaxValue); } /* TestMethod("Test_CONV_OVF_I8_UN__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I8_UN__UInt32() { Test("Test_CONV_OVF_I8_UN__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_OVF_I8_UN__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_OVF_I8_UN__UInt32() { Test("Test_CONV_OVF_I8_UN__UInt32", (uint)0x9ABCDEF0); } /* TestMethod("Test_CONV_OVF_I8_UN__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U_UN__UInt32() { Test("Test_CONV_OVF_U_UN__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_OVF_U_UN__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_OVF_U_UN__UInt32() { Test("Test_CONV_OVF_U_UN__UInt32", uint.MaxValue); } /* TestMethod("Test_CONV_OVF_U_UN__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U_UN__UInt32() { Test("Test_CONV_OVF_U_UN__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_OVF_U_UN__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_OVF_U_UN__UInt32() { Test("Test_CONV_OVF_U_UN__UInt32", (uint)0x9ABCDEF0); } /* TestMethod("Test_CONV_OVF_U_UN__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U1_UN__UInt32() { Test("Test_CONV_OVF_U1_UN__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_OVF_U1_UN__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_OVF_U1_UN__UInt32() { Test("Test_CONV_OVF_U1_UN__UInt32", uint.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U1_UN__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U1_UN__UInt32() { Test("Test_CONV_OVF_U1_UN__UInt32", (uint)0x12345678); } /* TestMethodEX("Test_CONV_OVF_U1_UN__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_OVF_U1_UN__UInt32() { Test("Test_CONV_OVF_U1_UN__UInt32", (uint)0x9ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U1_UN__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U2_UN__UInt32() { Test("Test_CONV_OVF_U2_UN__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_OVF_U2_UN__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_OVF_U2_UN__UInt32() { Test("Test_CONV_OVF_U2_UN__UInt32", uint.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U2_UN__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U2_UN__UInt32() { Test("Test_CONV_OVF_U2_UN__UInt32", (uint)0x12345678); } /* TestMethodEX("Test_CONV_OVF_U2_UN__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_OVF_U2_UN__UInt32() { Test("Test_CONV_OVF_U2_UN__UInt32", (uint)0x9ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U2_UN__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U4_UN__UInt32() { Test("Test_CONV_OVF_U4_UN__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_OVF_U4_UN__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_OVF_U4_UN__UInt32() { Test("Test_CONV_OVF_U4_UN__UInt32", uint.MaxValue); } /* TestMethod("Test_CONV_OVF_U4_UN__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U4_UN__UInt32() { Test("Test_CONV_OVF_U4_UN__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_OVF_U4_UN__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_OVF_U4_UN__UInt32() { Test("Test_CONV_OVF_U4_UN__UInt32", (uint)0x9ABCDEF0); } /* TestMethod("Test_CONV_OVF_U4_UN__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U8_UN__UInt32() { Test("Test_CONV_OVF_U8_UN__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_OVF_U8_UN__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_OVF_U8_UN__UInt32() { Test("Test_CONV_OVF_U8_UN__UInt32", uint.MaxValue); } /* TestMethod("Test_CONV_OVF_U8_UN__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U8_UN__UInt32() { Test("Test_CONV_OVF_U8_UN__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_OVF_U8_UN__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_OVF_U8_UN__UInt32() { Test("Test_CONV_OVF_U8_UN__UInt32", (uint)0x9ABCDEF0); } /* TestMethod("Test_CONV_OVF_U8_UN__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_R_UN__UInt32() { Test("Test_CONV_R_UN__UInt32", uint.MinValue); } /* TestMethod("Test_CONV_R_UN__UInt32", uint.MinValue); */
        //[Fact] public void Test_CONV_R_UN__UInt32() { Test("Test_CONV_R_UN__UInt32", uint.MaxValue); } /* TestMethod("Test_CONV_R_UN__UInt32", uint.MaxValue); */
        //[Fact] public void Test_CONV_R_UN__UInt32() { Test("Test_CONV_R_UN__UInt32", (uint)0x12345678); } /* TestMethod("Test_CONV_R_UN__UInt32", (uint)0x12345678); */
        //[Fact] public void Test_CONV_R_UN__UInt32() { Test("Test_CONV_R_UN__UInt32", (uint)0x9ABCDEF0); } /* TestMethod("Test_CONV_R_UN__UInt32", (uint)0x9ABCDEF0); */
        //[Fact] public void Test_CONV_I__UInt64() { Test("Test_CONV_I__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_I__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_I__UInt64() { Test("Test_CONV_I__UInt64", ulong.MaxValue); } /* TestMethod("Test_CONV_I__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_I__UInt64() { Test("Test_CONV_I__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_I__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_I__UInt64() { Test("Test_CONV_I__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethod("Test_CONV_I__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_I1__UInt64() { Test("Test_CONV_I1__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_I1__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_I1__UInt64() { Test("Test_CONV_I1__UInt64", ulong.MaxValue); } /* TestMethod("Test_CONV_I1__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_I1__UInt64() { Test("Test_CONV_I1__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_I1__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_I1__UInt64() { Test("Test_CONV_I1__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethod("Test_CONV_I1__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_I2__UInt64() { Test("Test_CONV_I2__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_I2__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_I2__UInt64() { Test("Test_CONV_I2__UInt64", ulong.MaxValue); } /* TestMethod("Test_CONV_I2__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_I2__UInt64() { Test("Test_CONV_I2__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_I2__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_I2__UInt64() { Test("Test_CONV_I2__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethod("Test_CONV_I2__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_I4__UInt64() { Test("Test_CONV_I4__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_I4__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_I4__UInt64() { Test("Test_CONV_I4__UInt64", ulong.MaxValue); } /* TestMethod("Test_CONV_I4__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_I4__UInt64() { Test("Test_CONV_I4__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_I4__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_I4__UInt64() { Test("Test_CONV_I4__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethod("Test_CONV_I4__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_I8__UInt64() { Test("Test_CONV_I8__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_I8__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_I8__UInt64() { Test("Test_CONV_I8__UInt64", ulong.MaxValue); } /* TestMethod("Test_CONV_I8__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_I8__UInt64() { Test("Test_CONV_I8__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_I8__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_I8__UInt64() { Test("Test_CONV_I8__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethod("Test_CONV_I8__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_U__UInt64() { Test("Test_CONV_U__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_U__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_U__UInt64() { Test("Test_CONV_U__UInt64", ulong.MaxValue); } /* TestMethod("Test_CONV_U__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_U__UInt64() { Test("Test_CONV_U__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_U__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_U__UInt64() { Test("Test_CONV_U__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethod("Test_CONV_U__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_U1__UInt64() { Test("Test_CONV_U1__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_U1__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_U1__UInt64() { Test("Test_CONV_U1__UInt64", ulong.MaxValue); } /* TestMethod("Test_CONV_U1__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_U1__UInt64() { Test("Test_CONV_U1__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_U1__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_U1__UInt64() { Test("Test_CONV_U1__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethod("Test_CONV_U1__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_U2__UInt64() { Test("Test_CONV_U2__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_U2__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_U2__UInt64() { Test("Test_CONV_U2__UInt64", ulong.MaxValue); } /* TestMethod("Test_CONV_U2__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_U2__UInt64() { Test("Test_CONV_U2__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_U2__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_U2__UInt64() { Test("Test_CONV_U2__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethod("Test_CONV_U2__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_U4__UInt64() { Test("Test_CONV_U4__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_U4__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_U4__UInt64() { Test("Test_CONV_U4__UInt64", ulong.MaxValue); } /* TestMethod("Test_CONV_U4__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_U4__UInt64() { Test("Test_CONV_U4__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_U4__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_U4__UInt64() { Test("Test_CONV_U4__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethod("Test_CONV_U4__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_U8__UInt64() { Test("Test_CONV_U8__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_U8__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_U8__UInt64() { Test("Test_CONV_U8__UInt64", ulong.MaxValue); } /* TestMethod("Test_CONV_U8__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_U8__UInt64() { Test("Test_CONV_U8__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_U8__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_U8__UInt64() { Test("Test_CONV_U8__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethod("Test_CONV_U8__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_R4__UInt64() { Test("Test_CONV_R4__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_R4__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_R4__UInt64() { Test("Test_CONV_R4__UInt64", ulong.MaxValue); } /* TestMethod("Test_CONV_R4__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_R4__UInt64() { Test("Test_CONV_R4__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_R4__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_R4__UInt64() { Test("Test_CONV_R4__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethod("Test_CONV_R4__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_R8__UInt64() { Test("Test_CONV_R8__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_R8__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_R8__UInt64() { Test("Test_CONV_R8__UInt64", ulong.MaxValue); } /* TestMethod("Test_CONV_R8__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_R8__UInt64() { Test("Test_CONV_R8__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_R8__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_R8__UInt64() { Test("Test_CONV_R8__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethod("Test_CONV_R8__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_OVF_I__UInt64() { Test("Test_CONV_OVF_I__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_OVF_I__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_OVF_I__UInt64() { Test("Test_CONV_OVF_I__UInt64", ulong.MaxValue); } /* TestMethod("Test_CONV_OVF_I__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I__UInt64() { Test("Test_CONV_OVF_I__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I__UInt64() { Test("Test_CONV_OVF_I__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethodEX("Test_CONV_OVF_I__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_OVF_I1__UInt64() { Test("Test_CONV_OVF_I1__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_OVF_I1__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_OVF_I1__UInt64() { Test("Test_CONV_OVF_I1__UInt64", ulong.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I1__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I1__UInt64() { Test("Test_CONV_OVF_I1__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I1__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I1__UInt64() { Test("Test_CONV_OVF_I1__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethodEX("Test_CONV_OVF_I1__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_OVF_I2__UInt64() { Test("Test_CONV_OVF_I2__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_OVF_I2__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_OVF_I2__UInt64() { Test("Test_CONV_OVF_I2__UInt64", ulong.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I2__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I2__UInt64() { Test("Test_CONV_OVF_I2__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I2__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I2__UInt64() { Test("Test_CONV_OVF_I2__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethodEX("Test_CONV_OVF_I2__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_OVF_I4__UInt64() { Test("Test_CONV_OVF_I4__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_OVF_I4__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_OVF_I4__UInt64() { Test("Test_CONV_OVF_I4__UInt64", ulong.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I4__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I4__UInt64() { Test("Test_CONV_OVF_I4__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I4__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I4__UInt64() { Test("Test_CONV_OVF_I4__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethodEX("Test_CONV_OVF_I4__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_OVF_I8__UInt64() { Test("Test_CONV_OVF_I8__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_OVF_I8__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_OVF_I8__UInt64() { Test("Test_CONV_OVF_I8__UInt64", ulong.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I8__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I8__UInt64() { Test("Test_CONV_OVF_I8__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_OVF_I8__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I8__UInt64() { Test("Test_CONV_OVF_I8__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethodEX("Test_CONV_OVF_I8__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_OVF_U__UInt64() { Test("Test_CONV_OVF_U__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_OVF_U__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_OVF_U__UInt64() { Test("Test_CONV_OVF_U__UInt64", ulong.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U__UInt64() { Test("Test_CONV_OVF_U__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U__UInt64() { Test("Test_CONV_OVF_U__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethodEX("Test_CONV_OVF_U__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_OVF_U1__UInt64() { Test("Test_CONV_OVF_U1__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_OVF_U1__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_OVF_U1__UInt64() { Test("Test_CONV_OVF_U1__UInt64", ulong.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U1__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U1__UInt64() { Test("Test_CONV_OVF_U1__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U1__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U1__UInt64() { Test("Test_CONV_OVF_U1__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethodEX("Test_CONV_OVF_U1__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_OVF_U2__UInt64() { Test("Test_CONV_OVF_U2__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_OVF_U2__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_OVF_U2__UInt64() { Test("Test_CONV_OVF_U2__UInt64", ulong.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U2__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U2__UInt64() { Test("Test_CONV_OVF_U2__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U2__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U2__UInt64() { Test("Test_CONV_OVF_U2__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethodEX("Test_CONV_OVF_U2__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_OVF_U4__UInt64() { Test("Test_CONV_OVF_U4__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_OVF_U4__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_OVF_U4__UInt64() { Test("Test_CONV_OVF_U4__UInt64", ulong.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U4__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U4__UInt64() { Test("Test_CONV_OVF_U4__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U4__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U4__UInt64() { Test("Test_CONV_OVF_U4__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethodEX("Test_CONV_OVF_U4__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_OVF_U8__UInt64() { Test("Test_CONV_OVF_U8__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_OVF_U8__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_OVF_U8__UInt64() { Test("Test_CONV_OVF_U8__UInt64", ulong.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U8__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U8__UInt64() { Test("Test_CONV_OVF_U8__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_OVF_U8__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U8__UInt64() { Test("Test_CONV_OVF_U8__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethodEX("Test_CONV_OVF_U8__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_OVF_I_UN__UInt64() { Test("Test_CONV_OVF_I_UN__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_OVF_I_UN__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_OVF_I_UN__UInt64() { Test("Test_CONV_OVF_I_UN__UInt64", ulong.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I_UN__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I_UN__UInt64() { Test("Test_CONV_OVF_I_UN__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I_UN__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I_UN__UInt64() { Test("Test_CONV_OVF_I_UN__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethodEX("Test_CONV_OVF_I_UN__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_OVF_I1_UN__UInt64() { Test("Test_CONV_OVF_I1_UN__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_OVF_I1_UN__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_OVF_I1_UN__UInt64() { Test("Test_CONV_OVF_I1_UN__UInt64", ulong.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I1_UN__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I1_UN__UInt64() { Test("Test_CONV_OVF_I1_UN__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I1_UN__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I1_UN__UInt64() { Test("Test_CONV_OVF_I1_UN__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethodEX("Test_CONV_OVF_I1_UN__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_OVF_I2_UN__UInt64() { Test("Test_CONV_OVF_I2_UN__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_OVF_I2_UN__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_OVF_I2_UN__UInt64() { Test("Test_CONV_OVF_I2_UN__UInt64", ulong.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I2_UN__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I2_UN__UInt64() { Test("Test_CONV_OVF_I2_UN__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I2_UN__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I2_UN__UInt64() { Test("Test_CONV_OVF_I2_UN__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethodEX("Test_CONV_OVF_I2_UN__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_OVF_I4_UN__UInt64() { Test("Test_CONV_OVF_I4_UN__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_OVF_I4_UN__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_OVF_I4_UN__UInt64() { Test("Test_CONV_OVF_I4_UN__UInt64", ulong.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I4_UN__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I4_UN__UInt64() { Test("Test_CONV_OVF_I4_UN__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_I4_UN__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I4_UN__UInt64() { Test("Test_CONV_OVF_I4_UN__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethodEX("Test_CONV_OVF_I4_UN__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_OVF_I8_UN__UInt64() { Test("Test_CONV_OVF_I8_UN__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_OVF_I8_UN__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_OVF_I8_UN__UInt64() { Test("Test_CONV_OVF_I8_UN__UInt64", ulong.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I8_UN__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I8_UN__UInt64() { Test("Test_CONV_OVF_I8_UN__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_OVF_I8_UN__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_I8_UN__UInt64() { Test("Test_CONV_OVF_I8_UN__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethodEX("Test_CONV_OVF_I8_UN__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_OVF_U_UN__UInt64() { Test("Test_CONV_OVF_U_UN__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_OVF_U_UN__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_OVF_U_UN__UInt64() { Test("Test_CONV_OVF_U_UN__UInt64", ulong.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U_UN__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U_UN__UInt64() { Test("Test_CONV_OVF_U_UN__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U_UN__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U_UN__UInt64() { Test("Test_CONV_OVF_U_UN__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethodEX("Test_CONV_OVF_U_UN__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_OVF_U1_UN__UInt64() { Test("Test_CONV_OVF_U1_UN__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_OVF_U1_UN__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_OVF_U1_UN__UInt64() { Test("Test_CONV_OVF_U1_UN__UInt64", ulong.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U1_UN__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U1_UN__UInt64() { Test("Test_CONV_OVF_U1_UN__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U1_UN__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U1_UN__UInt64() { Test("Test_CONV_OVF_U1_UN__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethodEX("Test_CONV_OVF_U1_UN__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_OVF_U2_UN__UInt64() { Test("Test_CONV_OVF_U2_UN__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_OVF_U2_UN__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_OVF_U2_UN__UInt64() { Test("Test_CONV_OVF_U2_UN__UInt64", ulong.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U2_UN__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U2_UN__UInt64() { Test("Test_CONV_OVF_U2_UN__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U2_UN__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U2_UN__UInt64() { Test("Test_CONV_OVF_U2_UN__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethodEX("Test_CONV_OVF_U2_UN__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_OVF_U4_UN__UInt64() { Test("Test_CONV_OVF_U4_UN__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_OVF_U4_UN__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_OVF_U4_UN__UInt64() { Test("Test_CONV_OVF_U4_UN__UInt64", ulong.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U4_UN__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U4_UN__UInt64() { Test("Test_CONV_OVF_U4_UN__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethodEX("Test_CONV_OVF_U4_UN__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U4_UN__UInt64() { Test("Test_CONV_OVF_U4_UN__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethodEX("Test_CONV_OVF_U4_UN__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_OVF_U8_UN__UInt64() { Test("Test_CONV_OVF_U8_UN__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_OVF_U8_UN__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_OVF_U8_UN__UInt64() { Test("Test_CONV_OVF_U8_UN__UInt64", ulong.MaxValue); } /* TestMethod("Test_CONV_OVF_U8_UN__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U8_UN__UInt64() { Test("Test_CONV_OVF_U8_UN__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_OVF_U8_UN__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_OVF_U8_UN__UInt64() { Test("Test_CONV_OVF_U8_UN__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethod("Test_CONV_OVF_U8_UN__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_R_UN__UInt64() { Test("Test_CONV_R_UN__UInt64", ulong.MinValue); } /* TestMethod("Test_CONV_R_UN__UInt64", ulong.MinValue); */
        //[Fact] public void Test_CONV_R_UN__UInt64() { Test("Test_CONV_R_UN__UInt64", ulong.MaxValue); } /* TestMethod("Test_CONV_R_UN__UInt64", ulong.MaxValue); */
        //[Fact] public void Test_CONV_R_UN__UInt64() { Test("Test_CONV_R_UN__UInt64", (ulong)0x123456789ABCDEF0); } /* TestMethod("Test_CONV_R_UN__UInt64", (ulong)0x123456789ABCDEF0); */
        //[Fact] public void Test_CONV_R_UN__UInt64() { Test("Test_CONV_R_UN__UInt64", (ulong)0x9ABCDEF012345678); } /* TestMethod("Test_CONV_R_UN__UInt64", (ulong)0x9ABCDEF012345678); */
        //[Fact] public void Test_CONV_I__Single() { Test("Test_CONV_I__Single", 0.0f); } /* TestMethod("Test_CONV_I__Single", 0.0f); */
        //[Fact] public void Test_CONV_I__Single() { Test("Test_CONV_I__Single", -0.0f); } /* TestMethod("Test_CONV_I__Single", -0.0f); */
        //[Fact] public void Test_CONV_I__Single() { Test("Test_CONV_I__Single", -1.0f); } /* TestMethod("Test_CONV_I__Single", -1.0f); */
        //[Fact] public void Test_CONV_I__Single() { Test("Test_CONV_I__Single", float.MinValue); } /* TestMethod("Test_CONV_I__Single", float.MinValue); */
        //[Fact] public void Test_CONV_I__Single() { Test("Test_CONV_I__Single", 1.0f); } /* TestMethod("Test_CONV_I__Single", 1.0f); */
        //[Fact] public void Test_CONV_I__Single() { Test("Test_CONV_I__Single", float.MaxValue); } /* TestMethod("Test_CONV_I__Single", float.MaxValue); */
        //[Fact] public void Test_CONV_I__Single() { Test("Test_CONV_I__Single", float.NaN); } /* TestMethod("Test_CONV_I__Single", float.NaN); */
        //[Fact] public void Test_CONV_I__Single() { Test("Test_CONV_I__Single", float.NegativeInfinity); } /* TestMethod("Test_CONV_I__Single", float.NegativeInfinity); */
        //[Fact] public void Test_CONV_I__Single() { Test("Test_CONV_I__Single", float.PositiveInfinity); } /* TestMethod("Test_CONV_I__Single", float.PositiveInfinity); */
        //[Fact] public void Test_CONV_I__Single() { Test("Test_CONV_I__Single", float.Epsilon); } /* TestMethod("Test_CONV_I__Single", float.Epsilon); */
        //[Fact] public void Test_CONV_I__Single() { Test("Test_CONV_I__Single", 1234567891011.12f); } /* TestMethod("Test_CONV_I__Single", 1234567891011.12f); */
        //[Fact] public void Test_CONV_I__Single() { Test("Test_CONV_I__Single", -1234567891011.12f); } /* TestMethod("Test_CONV_I__Single", -1234567891011.12f); */
        //[Fact] public void Test_CONV_I1__Single() { Test("Test_CONV_I1__Single", 0.0f); } /* TestMethod("Test_CONV_I1__Single", 0.0f); */
        //[Fact] public void Test_CONV_I1__Single() { Test("Test_CONV_I1__Single", -0.0f); } /* TestMethod("Test_CONV_I1__Single", -0.0f); */
        //[Fact] public void Test_CONV_I1__Single() { Test("Test_CONV_I1__Single", -1.0f); } /* TestMethod("Test_CONV_I1__Single", -1.0f); */
        //[Fact] public void Test_CONV_I1__Single() { Test("Test_CONV_I1__Single", float.MinValue); } /* TestMethod("Test_CONV_I1__Single", float.MinValue); */
        //[Fact] public void Test_CONV_I1__Single() { Test("Test_CONV_I1__Single", 1.0f); } /* TestMethod("Test_CONV_I1__Single", 1.0f); */
        //[Fact] public void Test_CONV_I1__Single() { Test("Test_CONV_I1__Single", float.MaxValue); } /* TestMethod("Test_CONV_I1__Single", float.MaxValue); */
        //[Fact] public void Test_CONV_I1__Single() { Test("Test_CONV_I1__Single", float.NaN); } /* TestMethod("Test_CONV_I1__Single", float.NaN); */
        //[Fact] public void Test_CONV_I1__Single() { Test("Test_CONV_I1__Single", float.NegativeInfinity); } /* TestMethod("Test_CONV_I1__Single", float.NegativeInfinity); */
        //[Fact] public void Test_CONV_I1__Single() { Test("Test_CONV_I1__Single", float.PositiveInfinity); } /* TestMethod("Test_CONV_I1__Single", float.PositiveInfinity); */
        //[Fact] public void Test_CONV_I1__Single() { Test("Test_CONV_I1__Single", float.Epsilon); } /* TestMethod("Test_CONV_I1__Single", float.Epsilon); */
        //[Fact] public void Test_CONV_I1__Single() { Test("Test_CONV_I1__Single", 1234567891011.12f); } /* TestMethod("Test_CONV_I1__Single", 1234567891011.12f); */
        //[Fact] public void Test_CONV_I1__Single() { Test("Test_CONV_I1__Single", -1234567891011.12f); } /* TestMethod("Test_CONV_I1__Single", -1234567891011.12f); */
        //[Fact] public void Test_CONV_I2__Single() { Test("Test_CONV_I2__Single", 0.0f); } /* TestMethod("Test_CONV_I2__Single", 0.0f); */
        //[Fact] public void Test_CONV_I2__Single() { Test("Test_CONV_I2__Single", -0.0f); } /* TestMethod("Test_CONV_I2__Single", -0.0f); */
        //[Fact] public void Test_CONV_I2__Single() { Test("Test_CONV_I2__Single", -1.0f); } /* TestMethod("Test_CONV_I2__Single", -1.0f); */
        //[Fact] public void Test_CONV_I2__Single() { Test("Test_CONV_I2__Single", float.MinValue); } /* TestMethod("Test_CONV_I2__Single", float.MinValue); */
        //[Fact] public void Test_CONV_I2__Single() { Test("Test_CONV_I2__Single", 1.0f); } /* TestMethod("Test_CONV_I2__Single", 1.0f); */
        //[Fact] public void Test_CONV_I2__Single() { Test("Test_CONV_I2__Single", float.MaxValue); } /* TestMethod("Test_CONV_I2__Single", float.MaxValue); */
        //[Fact] public void Test_CONV_I2__Single() { Test("Test_CONV_I2__Single", float.NaN); } /* TestMethod("Test_CONV_I2__Single", float.NaN); */
        //[Fact] public void Test_CONV_I2__Single() { Test("Test_CONV_I2__Single", float.NegativeInfinity); } /* TestMethod("Test_CONV_I2__Single", float.NegativeInfinity); */
        //[Fact] public void Test_CONV_I2__Single() { Test("Test_CONV_I2__Single", float.PositiveInfinity); } /* TestMethod("Test_CONV_I2__Single", float.PositiveInfinity); */
        //[Fact] public void Test_CONV_I2__Single() { Test("Test_CONV_I2__Single", float.Epsilon); } /* TestMethod("Test_CONV_I2__Single", float.Epsilon); */
        //[Fact] public void Test_CONV_I2__Single() { Test("Test_CONV_I2__Single", 1234567891011.12f); } /* TestMethod("Test_CONV_I2__Single", 1234567891011.12f); */
        //[Fact] public void Test_CONV_I2__Single() { Test("Test_CONV_I2__Single", -1234567891011.12f); } /* TestMethod("Test_CONV_I2__Single", -1234567891011.12f); */
        //[Fact] public void Test_CONV_I4__Single() { Test("Test_CONV_I4__Single", 0.0f); } /* TestMethod("Test_CONV_I4__Single", 0.0f); */
        //[Fact] public void Test_CONV_I4__Single() { Test("Test_CONV_I4__Single", -0.0f); } /* TestMethod("Test_CONV_I4__Single", -0.0f); */
        //[Fact] public void Test_CONV_I4__Single() { Test("Test_CONV_I4__Single", -1.0f); } /* TestMethod("Test_CONV_I4__Single", -1.0f); */
        //[Fact] public void Test_CONV_I4__Single() { Test("Test_CONV_I4__Single", float.MinValue); } /* TestMethod("Test_CONV_I4__Single", float.MinValue); */
        //[Fact] public void Test_CONV_I4__Single() { Test("Test_CONV_I4__Single", 1.0f); } /* TestMethod("Test_CONV_I4__Single", 1.0f); */
        //[Fact] public void Test_CONV_I4__Single() { Test("Test_CONV_I4__Single", float.MaxValue); } /* TestMethod("Test_CONV_I4__Single", float.MaxValue); */
        //[Fact] public void Test_CONV_I4__Single() { Test("Test_CONV_I4__Single", float.NaN); } /* TestMethod("Test_CONV_I4__Single", float.NaN); */
        //[Fact] public void Test_CONV_I4__Single() { Test("Test_CONV_I4__Single", float.NegativeInfinity); } /* TestMethod("Test_CONV_I4__Single", float.NegativeInfinity); */
        //[Fact] public void Test_CONV_I4__Single() { Test("Test_CONV_I4__Single", float.PositiveInfinity); } /* TestMethod("Test_CONV_I4__Single", float.PositiveInfinity); */
        //[Fact] public void Test_CONV_I4__Single() { Test("Test_CONV_I4__Single", float.Epsilon); } /* TestMethod("Test_CONV_I4__Single", float.Epsilon); */
        //[Fact] public void Test_CONV_I4__Single() { Test("Test_CONV_I4__Single", 1234567891011.12f); } /* TestMethod("Test_CONV_I4__Single", 1234567891011.12f); */
        //[Fact] public void Test_CONV_I4__Single() { Test("Test_CONV_I4__Single", -1234567891011.12f); } /* TestMethod("Test_CONV_I4__Single", -1234567891011.12f); */
        //[Fact] public void Test_CONV_I8__Single() { Test("Test_CONV_I8__Single", 0.0f); } /* TestMethod("Test_CONV_I8__Single", 0.0f); */
        //[Fact] public void Test_CONV_I8__Single() { Test("Test_CONV_I8__Single", -0.0f); } /* TestMethod("Test_CONV_I8__Single", -0.0f); */
        //[Fact] public void Test_CONV_I8__Single() { Test("Test_CONV_I8__Single", -1.0f); } /* TestMethod("Test_CONV_I8__Single", -1.0f); */
        //[Fact] public void Test_CONV_I8__Single() { Test("Test_CONV_I8__Single", float.MinValue); } /* TestMethod("Test_CONV_I8__Single", float.MinValue); */
        //[Fact] public void Test_CONV_I8__Single() { Test("Test_CONV_I8__Single", 1.0f); } /* TestMethod("Test_CONV_I8__Single", 1.0f); */
        //[Fact] public void Test_CONV_I8__Single() { Test("Test_CONV_I8__Single", float.MaxValue); } /* TestMethod("Test_CONV_I8__Single", float.MaxValue); */
        //[Fact] public void Test_CONV_I8__Single() { Test("Test_CONV_I8__Single", float.NaN); } /* TestMethod("Test_CONV_I8__Single", float.NaN); */
        //[Fact] public void Test_CONV_I8__Single() { Test("Test_CONV_I8__Single", float.NegativeInfinity); } /* TestMethod("Test_CONV_I8__Single", float.NegativeInfinity); */
        //[Fact] public void Test_CONV_I8__Single() { Test("Test_CONV_I8__Single", float.PositiveInfinity); } /* TestMethod("Test_CONV_I8__Single", float.PositiveInfinity); */
        //[Fact] public void Test_CONV_I8__Single() { Test("Test_CONV_I8__Single", float.Epsilon); } /* TestMethod("Test_CONV_I8__Single", float.Epsilon); */
        //[Fact] public void Test_CONV_I8__Single() { Test("Test_CONV_I8__Single", 1234567891011.12f); } /* TestMethod("Test_CONV_I8__Single", 1234567891011.12f); */
        //[Fact] public void Test_CONV_I8__Single() { Test("Test_CONV_I8__Single", -1234567891011.12f); } /* TestMethod("Test_CONV_I8__Single", -1234567891011.12f); */
        //[Fact] public void Test_CONV_U__Single() { Test("Test_CONV_U__Single", 0.0f); } /* TestMethod("Test_CONV_U__Single", 0.0f); */
        //[Fact] public void Test_CONV_U__Single() { Test("Test_CONV_U__Single", -0.0f); } /* TestMethod("Test_CONV_U__Single", -0.0f); */
        //[Fact] public void Test_CONV_U__Single() { Test("Test_CONV_U__Single", -1.0f); } /* TestMethod("Test_CONV_U__Single", -1.0f); */
        //[Fact] public void Test_CONV_U__Single() { Test("Test_CONV_U__Single", float.MinValue); } /* TestMethod("Test_CONV_U__Single", float.MinValue); */
        //[Fact] public void Test_CONV_U__Single() { Test("Test_CONV_U__Single", 1.0f); } /* TestMethod("Test_CONV_U__Single", 1.0f); */
        //[Fact] public void Test_CONV_U__Single() { Test("Test_CONV_U__Single", float.MaxValue); } /* TestMethod("Test_CONV_U__Single", float.MaxValue); */
        //[Fact] public void Test_CONV_U__Single() { Test("Test_CONV_U__Single", float.NaN); } /* TestMethod("Test_CONV_U__Single", float.NaN); */
        //[Fact] public void Test_CONV_U__Single() { Test("Test_CONV_U__Single", float.NegativeInfinity); } /* TestMethod("Test_CONV_U__Single", float.NegativeInfinity); */
        //[Fact] public void Test_CONV_U__Single() { Test("Test_CONV_U__Single", float.PositiveInfinity); } /* TestMethod("Test_CONV_U__Single", float.PositiveInfinity); */
        //[Fact] public void Test_CONV_U__Single() { Test("Test_CONV_U__Single", float.Epsilon); } /* TestMethod("Test_CONV_U__Single", float.Epsilon); */
        //[Fact] public void Test_CONV_U__Single() { Test("Test_CONV_U__Single", 1234567891011.12f); } /* TestMethod("Test_CONV_U__Single", 1234567891011.12f); */
        //[Fact] public void Test_CONV_U__Single() { Test("Test_CONV_U__Single", -1234567891011.12f); } /* TestMethod("Test_CONV_U__Single", -1234567891011.12f); */
        //[Fact] public void Test_CONV_U1__Single() { Test("Test_CONV_U1__Single", 0.0f); } /* TestMethod("Test_CONV_U1__Single", 0.0f); */
        //[Fact] public void Test_CONV_U1__Single() { Test("Test_CONV_U1__Single", -0.0f); } /* TestMethod("Test_CONV_U1__Single", -0.0f); */
        //[Fact] public void Test_CONV_U1__Single() { Test("Test_CONV_U1__Single", -1.0f); } /* TestMethod("Test_CONV_U1__Single", -1.0f); */
        //[Fact] public void Test_CONV_U1__Single() { Test("Test_CONV_U1__Single", float.MinValue); } /* TestMethod("Test_CONV_U1__Single", float.MinValue); */
        //[Fact] public void Test_CONV_U1__Single() { Test("Test_CONV_U1__Single", 1.0f); } /* TestMethod("Test_CONV_U1__Single", 1.0f); */
        //[Fact] public void Test_CONV_U1__Single() { Test("Test_CONV_U1__Single", float.MaxValue); } /* TestMethod("Test_CONV_U1__Single", float.MaxValue); */
        //[Fact] public void Test_CONV_U1__Single() { Test("Test_CONV_U1__Single", float.NaN); } /* TestMethod("Test_CONV_U1__Single", float.NaN); */
        //[Fact] public void Test_CONV_U1__Single() { Test("Test_CONV_U1__Single", float.NegativeInfinity); } /* TestMethod("Test_CONV_U1__Single", float.NegativeInfinity); */
        //[Fact] public void Test_CONV_U1__Single() { Test("Test_CONV_U1__Single", float.PositiveInfinity); } /* TestMethod("Test_CONV_U1__Single", float.PositiveInfinity); */
        //[Fact] public void Test_CONV_U1__Single() { Test("Test_CONV_U1__Single", float.Epsilon); } /* TestMethod("Test_CONV_U1__Single", float.Epsilon); */
        //[Fact] public void Test_CONV_U1__Single() { Test("Test_CONV_U1__Single", 1234567891011.12f); } /* TestMethod("Test_CONV_U1__Single", 1234567891011.12f); */
        //[Fact] public void Test_CONV_U1__Single() { Test("Test_CONV_U1__Single", -1234567891011.12f); } /* TestMethod("Test_CONV_U1__Single", -1234567891011.12f); */
        //[Fact] public void Test_CONV_U2__Single() { Test("Test_CONV_U2__Single", 0.0f); } /* TestMethod("Test_CONV_U2__Single", 0.0f); */
        //[Fact] public void Test_CONV_U2__Single() { Test("Test_CONV_U2__Single", -0.0f); } /* TestMethod("Test_CONV_U2__Single", -0.0f); */
        //[Fact] public void Test_CONV_U2__Single() { Test("Test_CONV_U2__Single", -1.0f); } /* TestMethod("Test_CONV_U2__Single", -1.0f); */
        //[Fact] public void Test_CONV_U2__Single() { Test("Test_CONV_U2__Single", float.MinValue); } /* TestMethod("Test_CONV_U2__Single", float.MinValue); */
        //[Fact] public void Test_CONV_U2__Single() { Test("Test_CONV_U2__Single", 1.0f); } /* TestMethod("Test_CONV_U2__Single", 1.0f); */
        //[Fact] public void Test_CONV_U2__Single() { Test("Test_CONV_U2__Single", float.MaxValue); } /* TestMethod("Test_CONV_U2__Single", float.MaxValue); */
        //[Fact] public void Test_CONV_U2__Single() { Test("Test_CONV_U2__Single", float.NaN); } /* TestMethod("Test_CONV_U2__Single", float.NaN); */
        //[Fact] public void Test_CONV_U2__Single() { Test("Test_CONV_U2__Single", float.NegativeInfinity); } /* TestMethod("Test_CONV_U2__Single", float.NegativeInfinity); */
        //[Fact] public void Test_CONV_U2__Single() { Test("Test_CONV_U2__Single", float.PositiveInfinity); } /* TestMethod("Test_CONV_U2__Single", float.PositiveInfinity); */
        //[Fact] public void Test_CONV_U2__Single() { Test("Test_CONV_U2__Single", float.Epsilon); } /* TestMethod("Test_CONV_U2__Single", float.Epsilon); */
        //[Fact] public void Test_CONV_U2__Single() { Test("Test_CONV_U2__Single", 1234567891011.12f); } /* TestMethod("Test_CONV_U2__Single", 1234567891011.12f); */
        //[Fact] public void Test_CONV_U2__Single() { Test("Test_CONV_U2__Single", -1234567891011.12f); } /* TestMethod("Test_CONV_U2__Single", -1234567891011.12f); */
        //[Fact] public void Test_CONV_U4__Single() { Test("Test_CONV_U4__Single", 0.0f); } /* TestMethod("Test_CONV_U4__Single", 0.0f); */
        //[Fact] public void Test_CONV_U4__Single() { Test("Test_CONV_U4__Single", -0.0f); } /* TestMethod("Test_CONV_U4__Single", -0.0f); */
        //[Fact] public void Test_CONV_U4__Single() { Test("Test_CONV_U4__Single", -1.0f); } /* TestMethod("Test_CONV_U4__Single", -1.0f); */
        //[Fact] public void Test_CONV_U4__Single() { Test("Test_CONV_U4__Single", float.MinValue); } /* TestMethod("Test_CONV_U4__Single", float.MinValue); */
        //[Fact] public void Test_CONV_U4__Single() { Test("Test_CONV_U4__Single", 1.0f); } /* TestMethod("Test_CONV_U4__Single", 1.0f); */
        //[Fact] public void Test_CONV_U4__Single() { Test("Test_CONV_U4__Single", float.MaxValue); } /* TestMethod("Test_CONV_U4__Single", float.MaxValue); */
        //[Fact] public void Test_CONV_U4__Single() { Test("Test_CONV_U4__Single", float.NaN); } /* TestMethod("Test_CONV_U4__Single", float.NaN); */
        //[Fact] public void Test_CONV_U4__Single() { Test("Test_CONV_U4__Single", float.NegativeInfinity); } /* TestMethod("Test_CONV_U4__Single", float.NegativeInfinity); */
        //[Fact] public void Test_CONV_U4__Single() { Test("Test_CONV_U4__Single", float.PositiveInfinity); } /* TestMethod("Test_CONV_U4__Single", float.PositiveInfinity); */
        //[Fact] public void Test_CONV_U4__Single() { Test("Test_CONV_U4__Single", float.Epsilon); } /* TestMethod("Test_CONV_U4__Single", float.Epsilon); */
        //[Fact] public void Test_CONV_U4__Single() { Test("Test_CONV_U4__Single", 1234567891011.12f); } /* TestMethod("Test_CONV_U4__Single", 1234567891011.12f); */
        //[Fact] public void Test_CONV_U4__Single() { Test("Test_CONV_U4__Single", -1234567891011.12f); } /* TestMethod("Test_CONV_U4__Single", -1234567891011.12f); */
        //[Fact] public void Test_CONV_U8__Single() { Test("Test_CONV_U8__Single", 0.0f); } /* TestMethod("Test_CONV_U8__Single", 0.0f); */
        //[Fact] public void Test_CONV_U8__Single() { Test("Test_CONV_U8__Single", -0.0f); } /* TestMethod("Test_CONV_U8__Single", -0.0f); */
        //[Fact] public void Test_CONV_U8__Single() { Test("Test_CONV_U8__Single", -1.0f); } /* TestMethod("Test_CONV_U8__Single", -1.0f); */
        //[Fact] public void Test_CONV_U8__Single() { Test("Test_CONV_U8__Single", float.MinValue); } /* TestMethod("Test_CONV_U8__Single", float.MinValue); */
        //[Fact] public void Test_CONV_U8__Single() { Test("Test_CONV_U8__Single", 1.0f); } /* TestMethod("Test_CONV_U8__Single", 1.0f); */
        //[Fact] public void Test_CONV_U8__Single() { Test("Test_CONV_U8__Single", float.MaxValue); } /* TestMethod("Test_CONV_U8__Single", float.MaxValue); */
        //[Fact] public void Test_CONV_U8__Single() { Test("Test_CONV_U8__Single", float.NaN); } /* TestMethod("Test_CONV_U8__Single", float.NaN); */
        //[Fact] public void Test_CONV_U8__Single() { Test("Test_CONV_U8__Single", float.NegativeInfinity); } /* TestMethod("Test_CONV_U8__Single", float.NegativeInfinity); */
        //[Fact] public void Test_CONV_U8__Single() { Test("Test_CONV_U8__Single", float.PositiveInfinity); } /* TestMethod("Test_CONV_U8__Single", float.PositiveInfinity); */
        //[Fact] public void Test_CONV_U8__Single() { Test("Test_CONV_U8__Single", float.Epsilon); } /* TestMethod("Test_CONV_U8__Single", float.Epsilon); */
        //[Fact] public void Test_CONV_U8__Single() { Test("Test_CONV_U8__Single", 1234567891011.12f); } /* TestMethod("Test_CONV_U8__Single", 1234567891011.12f); */
        //[Fact] public void Test_CONV_U8__Single() { Test("Test_CONV_U8__Single", -1234567891011.12f); } /* TestMethod("Test_CONV_U8__Single", -1234567891011.12f); */
        //[Fact] public void Test_CONV_R4__Single() { Test("Test_CONV_R4__Single", 0.0f); } /* TestMethod("Test_CONV_R4__Single", 0.0f); */
        //[Fact] public void Test_CONV_R4__Single() { Test("Test_CONV_R4__Single", -0.0f); } /* TestMethod("Test_CONV_R4__Single", -0.0f); */
        //[Fact] public void Test_CONV_R4__Single() { Test("Test_CONV_R4__Single", -1.0f); } /* TestMethod("Test_CONV_R4__Single", -1.0f); */
        //[Fact] public void Test_CONV_R4__Single() { Test("Test_CONV_R4__Single", float.MinValue); } /* TestMethod("Test_CONV_R4__Single", float.MinValue); */
        //[Fact] public void Test_CONV_R4__Single() { Test("Test_CONV_R4__Single", 1.0f); } /* TestMethod("Test_CONV_R4__Single", 1.0f); */
        //[Fact] public void Test_CONV_R4__Single() { Test("Test_CONV_R4__Single", float.MaxValue); } /* TestMethod("Test_CONV_R4__Single", float.MaxValue); */
        //[Fact] public void Test_CONV_R4__Single() { Test("Test_CONV_R4__Single", float.NaN); } /* TestMethod("Test_CONV_R4__Single", float.NaN); */
        //[Fact] public void Test_CONV_R4__Single() { Test("Test_CONV_R4__Single", float.NegativeInfinity); } /* TestMethod("Test_CONV_R4__Single", float.NegativeInfinity); */
        //[Fact] public void Test_CONV_R4__Single() { Test("Test_CONV_R4__Single", float.PositiveInfinity); } /* TestMethod("Test_CONV_R4__Single", float.PositiveInfinity); */
        //[Fact] public void Test_CONV_R4__Single() { Test("Test_CONV_R4__Single", float.Epsilon); } /* TestMethod("Test_CONV_R4__Single", float.Epsilon); */
        //[Fact] public void Test_CONV_R4__Single() { Test("Test_CONV_R4__Single", 1234567891011.12f); } /* TestMethod("Test_CONV_R4__Single", 1234567891011.12f); */
        //[Fact] public void Test_CONV_R4__Single() { Test("Test_CONV_R4__Single", -1234567891011.12f); } /* TestMethod("Test_CONV_R4__Single", -1234567891011.12f); */
        //[Fact] public void Test_CONV_R8__Single() { Test("Test_CONV_R8__Single", 0.0f); } /* TestMethod("Test_CONV_R8__Single", 0.0f); */
        //[Fact] public void Test_CONV_R8__Single() { Test("Test_CONV_R8__Single", -0.0f); } /* TestMethod("Test_CONV_R8__Single", -0.0f); */
        //[Fact] public void Test_CONV_R8__Single() { Test("Test_CONV_R8__Single", -1.0f); } /* TestMethod("Test_CONV_R8__Single", -1.0f); */
        //[Fact] public void Test_CONV_R8__Single() { Test("Test_CONV_R8__Single", float.MinValue); } /* TestMethod("Test_CONV_R8__Single", float.MinValue); */
        //[Fact] public void Test_CONV_R8__Single() { Test("Test_CONV_R8__Single", 1.0f); } /* TestMethod("Test_CONV_R8__Single", 1.0f); */
        //[Fact] public void Test_CONV_R8__Single() { Test("Test_CONV_R8__Single", float.MaxValue); } /* TestMethod("Test_CONV_R8__Single", float.MaxValue); */
        //[Fact] public void Test_CONV_R8__Single() { Test("Test_CONV_R8__Single", float.NaN); } /* TestMethod("Test_CONV_R8__Single", float.NaN); */
        //[Fact] public void Test_CONV_R8__Single() { Test("Test_CONV_R8__Single", float.NegativeInfinity); } /* TestMethod("Test_CONV_R8__Single", float.NegativeInfinity); */
        //[Fact] public void Test_CONV_R8__Single() { Test("Test_CONV_R8__Single", float.PositiveInfinity); } /* TestMethod("Test_CONV_R8__Single", float.PositiveInfinity); */
        //[Fact] public void Test_CONV_R8__Single() { Test("Test_CONV_R8__Single", float.Epsilon); } /* TestMethod("Test_CONV_R8__Single", float.Epsilon); */
        //[Fact] public void Test_CONV_R8__Single() { Test("Test_CONV_R8__Single", 1234567891011.12f); } /* TestMethod("Test_CONV_R8__Single", 1234567891011.12f); */
        //[Fact] public void Test_CONV_R8__Single() { Test("Test_CONV_R8__Single", -1234567891011.12f); } /* TestMethod("Test_CONV_R8__Single", -1234567891011.12f); */
        //[Fact] public void Test_CONV_OVF_I__Single() { Test("Test_CONV_OVF_I__Single", 0.0f); } /* TestMethod("Test_CONV_OVF_I__Single", 0.0f); */
        //[Fact] public void Test_CONV_OVF_I__Single() { Test("Test_CONV_OVF_I__Single", -0.0f); } /* TestMethod("Test_CONV_OVF_I__Single", -0.0f); */
        //[Fact] public void Test_CONV_OVF_I__Single() { Test("Test_CONV_OVF_I__Single", -1.0f); } /* TestMethod("Test_CONV_OVF_I__Single", -1.0f); */
        //[Fact] public void Test_CONV_OVF_I__Single() { Test("Test_CONV_OVF_I__Single", float.MinValue); } /* TestMethodEX("Test_CONV_OVF_I__Single", float.MinValue); */
        //[Fact] public void Test_CONV_OVF_I__Single() { Test("Test_CONV_OVF_I__Single", 1.0f); } /* TestMethod("Test_CONV_OVF_I__Single", 1.0f); */
        //[Fact] public void Test_CONV_OVF_I__Single() { Test("Test_CONV_OVF_I__Single", float.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I__Single", float.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I__Single() { Test("Test_CONV_OVF_I__Single", float.NaN); } /* TestMethodEX("Test_CONV_OVF_I__Single", float.NaN); */
        //[Fact] public void Test_CONV_OVF_I__Single() { Test("Test_CONV_OVF_I__Single", float.NegativeInfinity); } /* TestMethodEX("Test_CONV_OVF_I__Single", float.NegativeInfinity); */
        //[Fact] public void Test_CONV_OVF_I__Single() { Test("Test_CONV_OVF_I__Single", float.PositiveInfinity); } /* TestMethodEX("Test_CONV_OVF_I__Single", float.PositiveInfinity); */
        //[Fact] public void Test_CONV_OVF_I__Single() { Test("Test_CONV_OVF_I__Single", float.Epsilon); } /* TestMethod("Test_CONV_OVF_I__Single", float.Epsilon); */
        //[Fact] public void Test_CONV_OVF_I__Single() { Test("Test_CONV_OVF_I__Single", 1234567891011.12f); } /* TestMethodEX("Test_CONV_OVF_I__Single", 1234567891011.12f); */
        //[Fact] public void Test_CONV_OVF_I__Single() { Test("Test_CONV_OVF_I__Single", -1234567891011.12f); } /* TestMethodEX("Test_CONV_OVF_I__Single", -1234567891011.12f); */
        //[Fact] public void Test_CONV_OVF_I1__Single() { Test("Test_CONV_OVF_I1__Single", 0.0f); } /* TestMethod("Test_CONV_OVF_I1__Single", 0.0f); */
        //[Fact] public void Test_CONV_OVF_I1__Single() { Test("Test_CONV_OVF_I1__Single", -0.0f); } /* TestMethod("Test_CONV_OVF_I1__Single", -0.0f); */
        //[Fact] public void Test_CONV_OVF_I1__Single() { Test("Test_CONV_OVF_I1__Single", -1.0f); } /* TestMethod("Test_CONV_OVF_I1__Single", -1.0f); */
        //[Fact] public void Test_CONV_OVF_I1__Single() { Test("Test_CONV_OVF_I1__Single", float.MinValue); } /* TestMethodEX("Test_CONV_OVF_I1__Single", float.MinValue); */
        //[Fact] public void Test_CONV_OVF_I1__Single() { Test("Test_CONV_OVF_I1__Single", 1.0f); } /* TestMethod("Test_CONV_OVF_I1__Single", 1.0f); */
        //[Fact] public void Test_CONV_OVF_I1__Single() { Test("Test_CONV_OVF_I1__Single", float.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I1__Single", float.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I1__Single() { Test("Test_CONV_OVF_I1__Single", float.NaN); } /* TestMethodEX("Test_CONV_OVF_I1__Single", float.NaN); */
        //[Fact] public void Test_CONV_OVF_I1__Single() { Test("Test_CONV_OVF_I1__Single", float.NegativeInfinity); } /* TestMethodEX("Test_CONV_OVF_I1__Single", float.NegativeInfinity); */
        //[Fact] public void Test_CONV_OVF_I1__Single() { Test("Test_CONV_OVF_I1__Single", float.PositiveInfinity); } /* TestMethodEX("Test_CONV_OVF_I1__Single", float.PositiveInfinity); */
        //[Fact] public void Test_CONV_OVF_I1__Single() { Test("Test_CONV_OVF_I1__Single", float.Epsilon); } /* TestMethod("Test_CONV_OVF_I1__Single", float.Epsilon); */
        //[Fact] public void Test_CONV_OVF_I1__Single() { Test("Test_CONV_OVF_I1__Single", 1234567891011.12f); } /* TestMethodEX("Test_CONV_OVF_I1__Single", 1234567891011.12f); */
        //[Fact] public void Test_CONV_OVF_I1__Single() { Test("Test_CONV_OVF_I1__Single", -1234567891011.12f); } /* TestMethodEX("Test_CONV_OVF_I1__Single", -1234567891011.12f); */
        //[Fact] public void Test_CONV_OVF_I2__Single() { Test("Test_CONV_OVF_I2__Single", 0.0f); } /* TestMethod("Test_CONV_OVF_I2__Single", 0.0f); */
        //[Fact] public void Test_CONV_OVF_I2__Single() { Test("Test_CONV_OVF_I2__Single", -0.0f); } /* TestMethod("Test_CONV_OVF_I2__Single", -0.0f); */
        //[Fact] public void Test_CONV_OVF_I2__Single() { Test("Test_CONV_OVF_I2__Single", -1.0f); } /* TestMethod("Test_CONV_OVF_I2__Single", -1.0f); */
        //[Fact] public void Test_CONV_OVF_I2__Single() { Test("Test_CONV_OVF_I2__Single", float.MinValue); } /* TestMethodEX("Test_CONV_OVF_I2__Single", float.MinValue); */
        //[Fact] public void Test_CONV_OVF_I2__Single() { Test("Test_CONV_OVF_I2__Single", 1.0f); } /* TestMethod("Test_CONV_OVF_I2__Single", 1.0f); */
        //[Fact] public void Test_CONV_OVF_I2__Single() { Test("Test_CONV_OVF_I2__Single", float.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I2__Single", float.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I2__Single() { Test("Test_CONV_OVF_I2__Single", float.NaN); } /* TestMethodEX("Test_CONV_OVF_I2__Single", float.NaN); */
        //[Fact] public void Test_CONV_OVF_I2__Single() { Test("Test_CONV_OVF_I2__Single", float.NegativeInfinity); } /* TestMethodEX("Test_CONV_OVF_I2__Single", float.NegativeInfinity); */
        //[Fact] public void Test_CONV_OVF_I2__Single() { Test("Test_CONV_OVF_I2__Single", float.PositiveInfinity); } /* TestMethodEX("Test_CONV_OVF_I2__Single", float.PositiveInfinity); */
        //[Fact] public void Test_CONV_OVF_I2__Single() { Test("Test_CONV_OVF_I2__Single", float.Epsilon); } /* TestMethod("Test_CONV_OVF_I2__Single", float.Epsilon); */
        //[Fact] public void Test_CONV_OVF_I2__Single() { Test("Test_CONV_OVF_I2__Single", 1234567891011.12f); } /* TestMethodEX("Test_CONV_OVF_I2__Single", 1234567891011.12f); */
        //[Fact] public void Test_CONV_OVF_I2__Single() { Test("Test_CONV_OVF_I2__Single", -1234567891011.12f); } /* TestMethodEX("Test_CONV_OVF_I2__Single", -1234567891011.12f); */
        //[Fact] public void Test_CONV_OVF_I4__Single() { Test("Test_CONV_OVF_I4__Single", 0.0f); } /* TestMethod("Test_CONV_OVF_I4__Single", 0.0f); */
        //[Fact] public void Test_CONV_OVF_I4__Single() { Test("Test_CONV_OVF_I4__Single", -0.0f); } /* TestMethod("Test_CONV_OVF_I4__Single", -0.0f); */
        //[Fact] public void Test_CONV_OVF_I4__Single() { Test("Test_CONV_OVF_I4__Single", -1.0f); } /* TestMethod("Test_CONV_OVF_I4__Single", -1.0f); */
        //[Fact] public void Test_CONV_OVF_I4__Single() { Test("Test_CONV_OVF_I4__Single", float.MinValue); } /* TestMethodEX("Test_CONV_OVF_I4__Single", float.MinValue); */
        //[Fact] public void Test_CONV_OVF_I4__Single() { Test("Test_CONV_OVF_I4__Single", 1.0f); } /* TestMethod("Test_CONV_OVF_I4__Single", 1.0f); */
        //[Fact] public void Test_CONV_OVF_I4__Single() { Test("Test_CONV_OVF_I4__Single", float.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I4__Single", float.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I4__Single() { Test("Test_CONV_OVF_I4__Single", float.NaN); } /* TestMethodEX("Test_CONV_OVF_I4__Single", float.NaN); */
        //[Fact] public void Test_CONV_OVF_I4__Single() { Test("Test_CONV_OVF_I4__Single", float.NegativeInfinity); } /* TestMethodEX("Test_CONV_OVF_I4__Single", float.NegativeInfinity); */
        //[Fact] public void Test_CONV_OVF_I4__Single() { Test("Test_CONV_OVF_I4__Single", float.PositiveInfinity); } /* TestMethodEX("Test_CONV_OVF_I4__Single", float.PositiveInfinity); */
        //[Fact] public void Test_CONV_OVF_I4__Single() { Test("Test_CONV_OVF_I4__Single", float.Epsilon); } /* TestMethod("Test_CONV_OVF_I4__Single", float.Epsilon); */
        //[Fact] public void Test_CONV_OVF_I4__Single() { Test("Test_CONV_OVF_I4__Single", 1234567891011.12f); } /* TestMethodEX("Test_CONV_OVF_I4__Single", 1234567891011.12f); */
        //[Fact] public void Test_CONV_OVF_I4__Single() { Test("Test_CONV_OVF_I4__Single", -1234567891011.12f); } /* TestMethodEX("Test_CONV_OVF_I4__Single", -1234567891011.12f); */
        //[Fact] public void Test_CONV_OVF_I8__Single() { Test("Test_CONV_OVF_I8__Single", 0.0f); } /* TestMethod("Test_CONV_OVF_I8__Single", 0.0f); */
        //[Fact] public void Test_CONV_OVF_I8__Single() { Test("Test_CONV_OVF_I8__Single", -0.0f); } /* TestMethod("Test_CONV_OVF_I8__Single", -0.0f); */
        //[Fact] public void Test_CONV_OVF_I8__Single() { Test("Test_CONV_OVF_I8__Single", -1.0f); } /* TestMethod("Test_CONV_OVF_I8__Single", -1.0f); */
        //[Fact] public void Test_CONV_OVF_I8__Single() { Test("Test_CONV_OVF_I8__Single", float.MinValue); } /* TestMethodEX("Test_CONV_OVF_I8__Single", float.MinValue); */
        //[Fact] public void Test_CONV_OVF_I8__Single() { Test("Test_CONV_OVF_I8__Single", 1.0f); } /* TestMethod("Test_CONV_OVF_I8__Single", 1.0f); */
        //[Fact] public void Test_CONV_OVF_I8__Single() { Test("Test_CONV_OVF_I8__Single", float.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I8__Single", float.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I8__Single() { Test("Test_CONV_OVF_I8__Single", float.NaN); } /* TestMethodEX("Test_CONV_OVF_I8__Single", float.NaN); */
        //[Fact] public void Test_CONV_OVF_I8__Single() { Test("Test_CONV_OVF_I8__Single", float.NegativeInfinity); } /* TestMethodEX("Test_CONV_OVF_I8__Single", float.NegativeInfinity); */
        //[Fact] public void Test_CONV_OVF_I8__Single() { Test("Test_CONV_OVF_I8__Single", float.PositiveInfinity); } /* TestMethodEX("Test_CONV_OVF_I8__Single", float.PositiveInfinity); */
        //[Fact] public void Test_CONV_OVF_I8__Single() { Test("Test_CONV_OVF_I8__Single", float.Epsilon); } /* TestMethod("Test_CONV_OVF_I8__Single", float.Epsilon); */
        //[Fact] public void Test_CONV_OVF_I8__Single() { Test("Test_CONV_OVF_I8__Single", 1234567891011.12f); } /* TestMethod("Test_CONV_OVF_I8__Single", 1234567891011.12f); */
        //[Fact] public void Test_CONV_OVF_I8__Single() { Test("Test_CONV_OVF_I8__Single", -1234567891011.12f); } /* TestMethod("Test_CONV_OVF_I8__Single", -1234567891011.12f); */
        //[Fact] public void Test_CONV_OVF_U__Single() { Test("Test_CONV_OVF_U__Single", 0.0f); } /* TestMethod("Test_CONV_OVF_U__Single", 0.0f); */
        //[Fact] public void Test_CONV_OVF_U__Single() { Test("Test_CONV_OVF_U__Single", -0.0f); } /* TestMethod("Test_CONV_OVF_U__Single", -0.0f); */
        //[Fact] public void Test_CONV_OVF_U__Single() { Test("Test_CONV_OVF_U__Single", -1.0f); } /* TestMethodEX("Test_CONV_OVF_U__Single", -1.0f); */
        //[Fact] public void Test_CONV_OVF_U__Single() { Test("Test_CONV_OVF_U__Single", float.MinValue); } /* TestMethodEX("Test_CONV_OVF_U__Single", float.MinValue); */
        //[Fact] public void Test_CONV_OVF_U__Single() { Test("Test_CONV_OVF_U__Single", 1.0f); } /* TestMethod("Test_CONV_OVF_U__Single", 1.0f); */
        //[Fact] public void Test_CONV_OVF_U__Single() { Test("Test_CONV_OVF_U__Single", float.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U__Single", float.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U__Single() { Test("Test_CONV_OVF_U__Single", float.NaN); } /* TestMethodEX("Test_CONV_OVF_U__Single", float.NaN); */
        //[Fact] public void Test_CONV_OVF_U__Single() { Test("Test_CONV_OVF_U__Single", float.NegativeInfinity); } /* TestMethodEX("Test_CONV_OVF_U__Single", float.NegativeInfinity); */
        //[Fact] public void Test_CONV_OVF_U__Single() { Test("Test_CONV_OVF_U__Single", float.PositiveInfinity); } /* TestMethodEX("Test_CONV_OVF_U__Single", float.PositiveInfinity); */
        //[Fact] public void Test_CONV_OVF_U__Single() { Test("Test_CONV_OVF_U__Single", float.Epsilon); } /* TestMethod("Test_CONV_OVF_U__Single", float.Epsilon); */
        //[Fact] public void Test_CONV_OVF_U__Single() { Test("Test_CONV_OVF_U__Single", 1234567891011.12f); } /* TestMethodEX("Test_CONV_OVF_U__Single", 1234567891011.12f); */
        //[Fact] public void Test_CONV_OVF_U__Single() { Test("Test_CONV_OVF_U__Single", -1234567891011.12f); } /* TestMethodEX("Test_CONV_OVF_U__Single", -1234567891011.12f); */
        //[Fact] public void Test_CONV_OVF_U1__Single() { Test("Test_CONV_OVF_U1__Single", 0.0f); } /* TestMethod("Test_CONV_OVF_U1__Single", 0.0f); */
        //[Fact] public void Test_CONV_OVF_U1__Single() { Test("Test_CONV_OVF_U1__Single", -0.0f); } /* TestMethod("Test_CONV_OVF_U1__Single", -0.0f); */
        //[Fact] public void Test_CONV_OVF_U1__Single() { Test("Test_CONV_OVF_U1__Single", -1.0f); } /* TestMethodEX("Test_CONV_OVF_U1__Single", -1.0f); */
        //[Fact] public void Test_CONV_OVF_U1__Single() { Test("Test_CONV_OVF_U1__Single", float.MinValue); } /* TestMethodEX("Test_CONV_OVF_U1__Single", float.MinValue); */
        //[Fact] public void Test_CONV_OVF_U1__Single() { Test("Test_CONV_OVF_U1__Single", 1.0f); } /* TestMethod("Test_CONV_OVF_U1__Single", 1.0f); */
        //[Fact] public void Test_CONV_OVF_U1__Single() { Test("Test_CONV_OVF_U1__Single", float.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U1__Single", float.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U1__Single() { Test("Test_CONV_OVF_U1__Single", float.NaN); } /* TestMethodEX("Test_CONV_OVF_U1__Single", float.NaN); */
        //[Fact] public void Test_CONV_OVF_U1__Single() { Test("Test_CONV_OVF_U1__Single", float.NegativeInfinity); } /* TestMethodEX("Test_CONV_OVF_U1__Single", float.NegativeInfinity); */
        //[Fact] public void Test_CONV_OVF_U1__Single() { Test("Test_CONV_OVF_U1__Single", float.PositiveInfinity); } /* TestMethodEX("Test_CONV_OVF_U1__Single", float.PositiveInfinity); */
        //[Fact] public void Test_CONV_OVF_U1__Single() { Test("Test_CONV_OVF_U1__Single", float.Epsilon); } /* TestMethod("Test_CONV_OVF_U1__Single", float.Epsilon); */
        //[Fact] public void Test_CONV_OVF_U1__Single() { Test("Test_CONV_OVF_U1__Single", 1234567891011.12f); } /* TestMethodEX("Test_CONV_OVF_U1__Single", 1234567891011.12f); */
        //[Fact] public void Test_CONV_OVF_U1__Single() { Test("Test_CONV_OVF_U1__Single", -1234567891011.12f); } /* TestMethodEX("Test_CONV_OVF_U1__Single", -1234567891011.12f); */
        //[Fact] public void Test_CONV_OVF_U2__Single() { Test("Test_CONV_OVF_U2__Single", 0.0f); } /* TestMethod("Test_CONV_OVF_U2__Single", 0.0f); */
        //[Fact] public void Test_CONV_OVF_U2__Single() { Test("Test_CONV_OVF_U2__Single", -0.0f); } /* TestMethod("Test_CONV_OVF_U2__Single", -0.0f); */
        //[Fact] public void Test_CONV_OVF_U2__Single() { Test("Test_CONV_OVF_U2__Single", -1.0f); } /* TestMethodEX("Test_CONV_OVF_U2__Single", -1.0f); */
        //[Fact] public void Test_CONV_OVF_U2__Single() { Test("Test_CONV_OVF_U2__Single", float.MinValue); } /* TestMethodEX("Test_CONV_OVF_U2__Single", float.MinValue); */
        //[Fact] public void Test_CONV_OVF_U2__Single() { Test("Test_CONV_OVF_U2__Single", 1.0f); } /* TestMethod("Test_CONV_OVF_U2__Single", 1.0f); */
        //[Fact] public void Test_CONV_OVF_U2__Single() { Test("Test_CONV_OVF_U2__Single", float.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U2__Single", float.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U2__Single() { Test("Test_CONV_OVF_U2__Single", float.NaN); } /* TestMethodEX("Test_CONV_OVF_U2__Single", float.NaN); */
        //[Fact] public void Test_CONV_OVF_U2__Single() { Test("Test_CONV_OVF_U2__Single", float.NegativeInfinity); } /* TestMethodEX("Test_CONV_OVF_U2__Single", float.NegativeInfinity); */
        //[Fact] public void Test_CONV_OVF_U2__Single() { Test("Test_CONV_OVF_U2__Single", float.PositiveInfinity); } /* TestMethodEX("Test_CONV_OVF_U2__Single", float.PositiveInfinity); */
        //[Fact] public void Test_CONV_OVF_U2__Single() { Test("Test_CONV_OVF_U2__Single", float.Epsilon); } /* TestMethod("Test_CONV_OVF_U2__Single", float.Epsilon); */
        //[Fact] public void Test_CONV_OVF_U2__Single() { Test("Test_CONV_OVF_U2__Single", 1234567891011.12f); } /* TestMethodEX("Test_CONV_OVF_U2__Single", 1234567891011.12f); */
        //[Fact] public void Test_CONV_OVF_U2__Single() { Test("Test_CONV_OVF_U2__Single", -1234567891011.12f); } /* TestMethodEX("Test_CONV_OVF_U2__Single", -1234567891011.12f); */
        //[Fact] public void Test_CONV_OVF_U4__Single() { Test("Test_CONV_OVF_U4__Single", 0.0f); } /* TestMethod("Test_CONV_OVF_U4__Single", 0.0f); */
        //[Fact] public void Test_CONV_OVF_U4__Single() { Test("Test_CONV_OVF_U4__Single", -0.0f); } /* TestMethod("Test_CONV_OVF_U4__Single", -0.0f); */
        //[Fact] public void Test_CONV_OVF_U4__Single() { Test("Test_CONV_OVF_U4__Single", -1.0f); } /* TestMethodEX("Test_CONV_OVF_U4__Single", -1.0f); */
        //[Fact] public void Test_CONV_OVF_U4__Single() { Test("Test_CONV_OVF_U4__Single", float.MinValue); } /* TestMethodEX("Test_CONV_OVF_U4__Single", float.MinValue); */
        //[Fact] public void Test_CONV_OVF_U4__Single() { Test("Test_CONV_OVF_U4__Single", 1.0f); } /* TestMethod("Test_CONV_OVF_U4__Single", 1.0f); */
        //[Fact] public void Test_CONV_OVF_U4__Single() { Test("Test_CONV_OVF_U4__Single", float.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U4__Single", float.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U4__Single() { Test("Test_CONV_OVF_U4__Single", float.NaN); } /* TestMethodEX("Test_CONV_OVF_U4__Single", float.NaN); */
        //[Fact] public void Test_CONV_OVF_U4__Single() { Test("Test_CONV_OVF_U4__Single", float.NegativeInfinity); } /* TestMethodEX("Test_CONV_OVF_U4__Single", float.NegativeInfinity); */
        //[Fact] public void Test_CONV_OVF_U4__Single() { Test("Test_CONV_OVF_U4__Single", float.PositiveInfinity); } /* TestMethodEX("Test_CONV_OVF_U4__Single", float.PositiveInfinity); */
        //[Fact] public void Test_CONV_OVF_U4__Single() { Test("Test_CONV_OVF_U4__Single", float.Epsilon); } /* TestMethod("Test_CONV_OVF_U4__Single", float.Epsilon); */
        //[Fact] public void Test_CONV_OVF_U4__Single() { Test("Test_CONV_OVF_U4__Single", 1234567891011.12f); } /* TestMethodEX("Test_CONV_OVF_U4__Single", 1234567891011.12f); */
        //[Fact] public void Test_CONV_OVF_U4__Single() { Test("Test_CONV_OVF_U4__Single", -1234567891011.12f); } /* TestMethodEX("Test_CONV_OVF_U4__Single", -1234567891011.12f); */
        //[Fact] public void Test_CONV_OVF_U8__Single() { Test("Test_CONV_OVF_U8__Single", 0.0f); } /* TestMethod("Test_CONV_OVF_U8__Single", 0.0f); */
        //[Fact] public void Test_CONV_OVF_U8__Single() { Test("Test_CONV_OVF_U8__Single", -0.0f); } /* TestMethod("Test_CONV_OVF_U8__Single", -0.0f); */
        //[Fact] public void Test_CONV_OVF_U8__Single() { Test("Test_CONV_OVF_U8__Single", -1.0f); } /* TestMethodEX("Test_CONV_OVF_U8__Single", -1.0f); */
        //[Fact] public void Test_CONV_OVF_U8__Single() { Test("Test_CONV_OVF_U8__Single", float.MinValue); } /* TestMethodEX("Test_CONV_OVF_U8__Single", float.MinValue); */
        //[Fact] public void Test_CONV_OVF_U8__Single() { Test("Test_CONV_OVF_U8__Single", 1.0f); } /* TestMethod("Test_CONV_OVF_U8__Single", 1.0f); */
        //[Fact] public void Test_CONV_OVF_U8__Single() { Test("Test_CONV_OVF_U8__Single", float.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U8__Single", float.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U8__Single() { Test("Test_CONV_OVF_U8__Single", float.NaN); } /* TestMethodEX("Test_CONV_OVF_U8__Single", float.NaN); */
        //[Fact] public void Test_CONV_OVF_U8__Single() { Test("Test_CONV_OVF_U8__Single", float.NegativeInfinity); } /* TestMethodEX("Test_CONV_OVF_U8__Single", float.NegativeInfinity); */
        //[Fact] public void Test_CONV_OVF_U8__Single() { Test("Test_CONV_OVF_U8__Single", float.PositiveInfinity); } /* TestMethodEX("Test_CONV_OVF_U8__Single", float.PositiveInfinity); */
        //[Fact] public void Test_CONV_OVF_U8__Single() { Test("Test_CONV_OVF_U8__Single", float.Epsilon); } /* TestMethod("Test_CONV_OVF_U8__Single", float.Epsilon); */
        //[Fact] public void Test_CONV_OVF_U8__Single() { Test("Test_CONV_OVF_U8__Single", 1234567891011.12f); } /* TestMethodEX("Test_CONV_OVF_U8__Single", 1234567891011.12f); */
        //[Fact] public void Test_CONV_OVF_U8__Single() { Test("Test_CONV_OVF_U8__Single", -1234567891011.12f); } /* TestMethodEX("Test_CONV_OVF_U8__Single", -1234567891011.12f); */
        //[Fact] public void Test_CONV_I__Double() { Test("Test_CONV_I__Double", 0.0d); } /* TestMethod("Test_CONV_I__Double", 0.0d); */
        //[Fact] public void Test_CONV_I__Double() { Test("Test_CONV_I__Double", -0.0d); } /* TestMethod("Test_CONV_I__Double", -0.0d); */
        //[Fact] public void Test_CONV_I__Double() { Test("Test_CONV_I__Double", -1.0d); } /* TestMethod("Test_CONV_I__Double", -1.0d); */
        //[Fact] public void Test_CONV_I__Double() { Test("Test_CONV_I__Double", double.MinValue); } /* TestMethod("Test_CONV_I__Double", double.MinValue); */
        //[Fact] public void Test_CONV_I__Double() { Test("Test_CONV_I__Double", 1.0d); } /* TestMethod("Test_CONV_I__Double", 1.0d); */
        //[Fact] public void Test_CONV_I__Double() { Test("Test_CONV_I__Double", double.MaxValue); } /* TestMethod("Test_CONV_I__Double", double.MaxValue); */
        //[Fact] public void Test_CONV_I__Double() { Test("Test_CONV_I__Double", double.NaN); } /* TestMethod("Test_CONV_I__Double", double.NaN); */
        //[Fact] public void Test_CONV_I__Double() { Test("Test_CONV_I__Double", double.NegativeInfinity); } /* TestMethod("Test_CONV_I__Double", double.NegativeInfinity); */
        //[Fact] public void Test_CONV_I__Double() { Test("Test_CONV_I__Double", double.PositiveInfinity); } /* TestMethod("Test_CONV_I__Double", double.PositiveInfinity); */
        //[Fact] public void Test_CONV_I__Double() { Test("Test_CONV_I__Double", double.Epsilon); } /* TestMethod("Test_CONV_I__Double", double.Epsilon); */
        //[Fact] public void Test_CONV_I__Double() { Test("Test_CONV_I__Double", 12345678910111213.1415d); } /* TestMethod("Test_CONV_I__Double", 12345678910111213.1415d); */
        //[Fact] public void Test_CONV_I__Double() { Test("Test_CONV_I__Double", -12345678910111213.1415d); } /* TestMethod("Test_CONV_I__Double", -12345678910111213.1415d); */
        //[Fact] public void Test_CONV_I1__Double() { Test("Test_CONV_I1__Double", 0.0d); } /* TestMethod("Test_CONV_I1__Double", 0.0d); */
        //[Fact] public void Test_CONV_I1__Double() { Test("Test_CONV_I1__Double", -0.0d); } /* TestMethod("Test_CONV_I1__Double", -0.0d); */
        //[Fact] public void Test_CONV_I1__Double() { Test("Test_CONV_I1__Double", -1.0d); } /* TestMethod("Test_CONV_I1__Double", -1.0d); */
        //[Fact] public void Test_CONV_I1__Double() { Test("Test_CONV_I1__Double", double.MinValue); } /* TestMethod("Test_CONV_I1__Double", double.MinValue); */
        //[Fact] public void Test_CONV_I1__Double() { Test("Test_CONV_I1__Double", 1.0d); } /* TestMethod("Test_CONV_I1__Double", 1.0d); */
        //[Fact] public void Test_CONV_I1__Double() { Test("Test_CONV_I1__Double", double.MaxValue); } /* TestMethod("Test_CONV_I1__Double", double.MaxValue); */
        //[Fact] public void Test_CONV_I1__Double() { Test("Test_CONV_I1__Double", double.NaN); } /* TestMethod("Test_CONV_I1__Double", double.NaN); */
        //[Fact] public void Test_CONV_I1__Double() { Test("Test_CONV_I1__Double", double.NegativeInfinity); } /* TestMethod("Test_CONV_I1__Double", double.NegativeInfinity); */
        //[Fact] public void Test_CONV_I1__Double() { Test("Test_CONV_I1__Double", double.PositiveInfinity); } /* TestMethod("Test_CONV_I1__Double", double.PositiveInfinity); */
        //[Fact] public void Test_CONV_I1__Double() { Test("Test_CONV_I1__Double", double.Epsilon); } /* TestMethod("Test_CONV_I1__Double", double.Epsilon); */
        //[Fact] public void Test_CONV_I1__Double() { Test("Test_CONV_I1__Double", 12345678910111213.1415d); } /* TestMethod("Test_CONV_I1__Double", 12345678910111213.1415d); */
        //[Fact] public void Test_CONV_I1__Double() { Test("Test_CONV_I1__Double", -12345678910111213.1415d); } /* TestMethod("Test_CONV_I1__Double", -12345678910111213.1415d); */
        //[Fact] public void Test_CONV_I2__Double() { Test("Test_CONV_I2__Double", 0.0d); } /* TestMethod("Test_CONV_I2__Double", 0.0d); */
        //[Fact] public void Test_CONV_I2__Double() { Test("Test_CONV_I2__Double", -0.0d); } /* TestMethod("Test_CONV_I2__Double", -0.0d); */
        //[Fact] public void Test_CONV_I2__Double() { Test("Test_CONV_I2__Double", -1.0d); } /* TestMethod("Test_CONV_I2__Double", -1.0d); */
        //[Fact] public void Test_CONV_I2__Double() { Test("Test_CONV_I2__Double", double.MinValue); } /* TestMethod("Test_CONV_I2__Double", double.MinValue); */
        //[Fact] public void Test_CONV_I2__Double() { Test("Test_CONV_I2__Double", 1.0d); } /* TestMethod("Test_CONV_I2__Double", 1.0d); */
        //[Fact] public void Test_CONV_I2__Double() { Test("Test_CONV_I2__Double", double.MaxValue); } /* TestMethod("Test_CONV_I2__Double", double.MaxValue); */
        //[Fact] public void Test_CONV_I2__Double() { Test("Test_CONV_I2__Double", double.NaN); } /* TestMethod("Test_CONV_I2__Double", double.NaN); */
        //[Fact] public void Test_CONV_I2__Double() { Test("Test_CONV_I2__Double", double.NegativeInfinity); } /* TestMethod("Test_CONV_I2__Double", double.NegativeInfinity); */
        //[Fact] public void Test_CONV_I2__Double() { Test("Test_CONV_I2__Double", double.PositiveInfinity); } /* TestMethod("Test_CONV_I2__Double", double.PositiveInfinity); */
        //[Fact] public void Test_CONV_I2__Double() { Test("Test_CONV_I2__Double", double.Epsilon); } /* TestMethod("Test_CONV_I2__Double", double.Epsilon); */
        //[Fact] public void Test_CONV_I2__Double() { Test("Test_CONV_I2__Double", 12345678910111213.1415d); } /* TestMethod("Test_CONV_I2__Double", 12345678910111213.1415d); */
        //[Fact] public void Test_CONV_I2__Double() { Test("Test_CONV_I2__Double", -12345678910111213.1415d); } /* TestMethod("Test_CONV_I2__Double", -12345678910111213.1415d); */
        //[Fact] public void Test_CONV_I4__Double() { Test("Test_CONV_I4__Double", 0.0d); } /* TestMethod("Test_CONV_I4__Double", 0.0d); */
        //[Fact] public void Test_CONV_I4__Double() { Test("Test_CONV_I4__Double", -0.0d); } /* TestMethod("Test_CONV_I4__Double", -0.0d); */
        //[Fact] public void Test_CONV_I4__Double() { Test("Test_CONV_I4__Double", -1.0d); } /* TestMethod("Test_CONV_I4__Double", -1.0d); */
        //[Fact] public void Test_CONV_I4__Double() { Test("Test_CONV_I4__Double", double.MinValue); } /* TestMethod("Test_CONV_I4__Double", double.MinValue); */
        //[Fact] public void Test_CONV_I4__Double() { Test("Test_CONV_I4__Double", 1.0d); } /* TestMethod("Test_CONV_I4__Double", 1.0d); */
        //[Fact] public void Test_CONV_I4__Double() { Test("Test_CONV_I4__Double", double.MaxValue); } /* TestMethod("Test_CONV_I4__Double", double.MaxValue); */
        //[Fact] public void Test_CONV_I4__Double() { Test("Test_CONV_I4__Double", double.NaN); } /* TestMethod("Test_CONV_I4__Double", double.NaN); */
        //[Fact] public void Test_CONV_I4__Double() { Test("Test_CONV_I4__Double", double.NegativeInfinity); } /* TestMethod("Test_CONV_I4__Double", double.NegativeInfinity); */
        //[Fact] public void Test_CONV_I4__Double() { Test("Test_CONV_I4__Double", double.PositiveInfinity); } /* TestMethod("Test_CONV_I4__Double", double.PositiveInfinity); */
        //[Fact] public void Test_CONV_I4__Double() { Test("Test_CONV_I4__Double", double.Epsilon); } /* TestMethod("Test_CONV_I4__Double", double.Epsilon); */
        //[Fact] public void Test_CONV_I4__Double() { Test("Test_CONV_I4__Double", 12345678910111213.1415d); } /* TestMethod("Test_CONV_I4__Double", 12345678910111213.1415d); */
        //[Fact] public void Test_CONV_I4__Double() { Test("Test_CONV_I4__Double", -12345678910111213.1415d); } /* TestMethod("Test_CONV_I4__Double", -12345678910111213.1415d); */
        //[Fact] public void Test_CONV_I8__Double() { Test("Test_CONV_I8__Double", 0.0d); } /* TestMethod("Test_CONV_I8__Double", 0.0d); */
        //[Fact] public void Test_CONV_I8__Double() { Test("Test_CONV_I8__Double", -0.0d); } /* TestMethod("Test_CONV_I8__Double", -0.0d); */
        //[Fact] public void Test_CONV_I8__Double() { Test("Test_CONV_I8__Double", -1.0d); } /* TestMethod("Test_CONV_I8__Double", -1.0d); */
        //[Fact] public void Test_CONV_I8__Double() { Test("Test_CONV_I8__Double", double.MinValue); } /* TestMethod("Test_CONV_I8__Double", double.MinValue); */
        //[Fact] public void Test_CONV_I8__Double() { Test("Test_CONV_I8__Double", 1.0d); } /* TestMethod("Test_CONV_I8__Double", 1.0d); */
        //[Fact] public void Test_CONV_I8__Double() { Test("Test_CONV_I8__Double", double.MaxValue); } /* TestMethod("Test_CONV_I8__Double", double.MaxValue); */
        //[Fact] public void Test_CONV_I8__Double() { Test("Test_CONV_I8__Double", double.NaN); } /* TestMethod("Test_CONV_I8__Double", double.NaN); */
        //[Fact] public void Test_CONV_I8__Double() { Test("Test_CONV_I8__Double", double.NegativeInfinity); } /* TestMethod("Test_CONV_I8__Double", double.NegativeInfinity); */
        //[Fact] public void Test_CONV_I8__Double() { Test("Test_CONV_I8__Double", double.PositiveInfinity); } /* TestMethod("Test_CONV_I8__Double", double.PositiveInfinity); */
        //[Fact] public void Test_CONV_I8__Double() { Test("Test_CONV_I8__Double", double.Epsilon); } /* TestMethod("Test_CONV_I8__Double", double.Epsilon); */
        //[Fact] public void Test_CONV_I8__Double() { Test("Test_CONV_I8__Double", 12345678910111213.1415d); } /* TestMethod("Test_CONV_I8__Double", 12345678910111213.1415d); */
        //[Fact] public void Test_CONV_I8__Double() { Test("Test_CONV_I8__Double", -12345678910111213.1415d); } /* TestMethod("Test_CONV_I8__Double", -12345678910111213.1415d); */
        //[Fact] public void Test_CONV_U__Double() { Test("Test_CONV_U__Double", 0.0d); } /* TestMethod("Test_CONV_U__Double", 0.0d); */
        //[Fact] public void Test_CONV_U__Double() { Test("Test_CONV_U__Double", -0.0d); } /* TestMethod("Test_CONV_U__Double", -0.0d); */
        //[Fact] public void Test_CONV_U__Double() { Test("Test_CONV_U__Double", -1.0d); } /* TestMethod("Test_CONV_U__Double", -1.0d); */
        //[Fact] public void Test_CONV_U__Double() { Test("Test_CONV_U__Double", double.MinValue); } /* TestMethod("Test_CONV_U__Double", double.MinValue); */
        //[Fact] public void Test_CONV_U__Double() { Test("Test_CONV_U__Double", 1.0d); } /* TestMethod("Test_CONV_U__Double", 1.0d); */
        //[Fact] public void Test_CONV_U__Double() { Test("Test_CONV_U__Double", double.MaxValue); } /* TestMethod("Test_CONV_U__Double", double.MaxValue); */
        //[Fact] public void Test_CONV_U__Double() { Test("Test_CONV_U__Double", double.NaN); } /* TestMethod("Test_CONV_U__Double", double.NaN); */
        //[Fact] public void Test_CONV_U__Double() { Test("Test_CONV_U__Double", double.NegativeInfinity); } /* TestMethod("Test_CONV_U__Double", double.NegativeInfinity); */
        //[Fact] public void Test_CONV_U__Double() { Test("Test_CONV_U__Double", double.PositiveInfinity); } /* TestMethod("Test_CONV_U__Double", double.PositiveInfinity); */
        //[Fact] public void Test_CONV_U__Double() { Test("Test_CONV_U__Double", double.Epsilon); } /* TestMethod("Test_CONV_U__Double", double.Epsilon); */
        //[Fact] public void Test_CONV_U__Double() { Test("Test_CONV_U__Double", 12345678910111213.1415d); } /* TestMethod("Test_CONV_U__Double", 12345678910111213.1415d); */
        //[Fact] public void Test_CONV_U__Double() { Test("Test_CONV_U__Double", -12345678910111213.1415d); } /* TestMethod("Test_CONV_U__Double", -12345678910111213.1415d); */
        //[Fact] public void Test_CONV_U1__Double() { Test("Test_CONV_U1__Double", 0.0d); } /* TestMethod("Test_CONV_U1__Double", 0.0d); */
        //[Fact] public void Test_CONV_U1__Double() { Test("Test_CONV_U1__Double", -0.0d); } /* TestMethod("Test_CONV_U1__Double", -0.0d); */
        //[Fact] public void Test_CONV_U1__Double() { Test("Test_CONV_U1__Double", -1.0d); } /* TestMethod("Test_CONV_U1__Double", -1.0d); */
        //[Fact] public void Test_CONV_U1__Double() { Test("Test_CONV_U1__Double", double.MinValue); } /* TestMethod("Test_CONV_U1__Double", double.MinValue); */
        //[Fact] public void Test_CONV_U1__Double() { Test("Test_CONV_U1__Double", 1.0d); } /* TestMethod("Test_CONV_U1__Double", 1.0d); */
        //[Fact] public void Test_CONV_U1__Double() { Test("Test_CONV_U1__Double", double.MaxValue); } /* TestMethod("Test_CONV_U1__Double", double.MaxValue); */
        //[Fact] public void Test_CONV_U1__Double() { Test("Test_CONV_U1__Double", double.NaN); } /* TestMethod("Test_CONV_U1__Double", double.NaN); */
        //[Fact] public void Test_CONV_U1__Double() { Test("Test_CONV_U1__Double", double.NegativeInfinity); } /* TestMethod("Test_CONV_U1__Double", double.NegativeInfinity); */
        //[Fact] public void Test_CONV_U1__Double() { Test("Test_CONV_U1__Double", double.PositiveInfinity); } /* TestMethod("Test_CONV_U1__Double", double.PositiveInfinity); */
        //[Fact] public void Test_CONV_U1__Double() { Test("Test_CONV_U1__Double", double.Epsilon); } /* TestMethod("Test_CONV_U1__Double", double.Epsilon); */
        //[Fact] public void Test_CONV_U1__Double() { Test("Test_CONV_U1__Double", 12345678910111213.1415d); } /* TestMethod("Test_CONV_U1__Double", 12345678910111213.1415d); */
        //[Fact] public void Test_CONV_U1__Double() { Test("Test_CONV_U1__Double", -12345678910111213.1415d); } /* TestMethod("Test_CONV_U1__Double", -12345678910111213.1415d); */
        //[Fact] public void Test_CONV_U2__Double() { Test("Test_CONV_U2__Double", 0.0d); } /* TestMethod("Test_CONV_U2__Double", 0.0d); */
        //[Fact] public void Test_CONV_U2__Double() { Test("Test_CONV_U2__Double", -0.0d); } /* TestMethod("Test_CONV_U2__Double", -0.0d); */
        //[Fact] public void Test_CONV_U2__Double() { Test("Test_CONV_U2__Double", -1.0d); } /* TestMethod("Test_CONV_U2__Double", -1.0d); */
        //[Fact] public void Test_CONV_U2__Double() { Test("Test_CONV_U2__Double", double.MinValue); } /* TestMethod("Test_CONV_U2__Double", double.MinValue); */
        //[Fact] public void Test_CONV_U2__Double() { Test("Test_CONV_U2__Double", 1.0d); } /* TestMethod("Test_CONV_U2__Double", 1.0d); */
        //[Fact] public void Test_CONV_U2__Double() { Test("Test_CONV_U2__Double", double.MaxValue); } /* TestMethod("Test_CONV_U2__Double", double.MaxValue); */
        //[Fact] public void Test_CONV_U2__Double() { Test("Test_CONV_U2__Double", double.NaN); } /* TestMethod("Test_CONV_U2__Double", double.NaN); */
        //[Fact] public void Test_CONV_U2__Double() { Test("Test_CONV_U2__Double", double.NegativeInfinity); } /* TestMethod("Test_CONV_U2__Double", double.NegativeInfinity); */
        //[Fact] public void Test_CONV_U2__Double() { Test("Test_CONV_U2__Double", double.PositiveInfinity); } /* TestMethod("Test_CONV_U2__Double", double.PositiveInfinity); */
        //[Fact] public void Test_CONV_U2__Double() { Test("Test_CONV_U2__Double", double.Epsilon); } /* TestMethod("Test_CONV_U2__Double", double.Epsilon); */
        //[Fact] public void Test_CONV_U2__Double() { Test("Test_CONV_U2__Double", 12345678910111213.1415d); } /* TestMethod("Test_CONV_U2__Double", 12345678910111213.1415d); */
        //[Fact] public void Test_CONV_U2__Double() { Test("Test_CONV_U2__Double", -12345678910111213.1415d); } /* TestMethod("Test_CONV_U2__Double", -12345678910111213.1415d); */
        //[Fact] public void Test_CONV_U4__Double() { Test("Test_CONV_U4__Double", 0.0d); } /* TestMethod("Test_CONV_U4__Double", 0.0d); */
        //[Fact] public void Test_CONV_U4__Double() { Test("Test_CONV_U4__Double", -0.0d); } /* TestMethod("Test_CONV_U4__Double", -0.0d); */
        //[Fact] public void Test_CONV_U4__Double() { Test("Test_CONV_U4__Double", -1.0d); } /* TestMethod("Test_CONV_U4__Double", -1.0d); */
        //[Fact] public void Test_CONV_U4__Double() { Test("Test_CONV_U4__Double", double.MinValue); } /* TestMethod("Test_CONV_U4__Double", double.MinValue); */
        //[Fact] public void Test_CONV_U4__Double() { Test("Test_CONV_U4__Double", 1.0d); } /* TestMethod("Test_CONV_U4__Double", 1.0d); */
        //[Fact] public void Test_CONV_U4__Double() { Test("Test_CONV_U4__Double", double.MaxValue); } /* TestMethod("Test_CONV_U4__Double", double.MaxValue); */
        //[Fact] public void Test_CONV_U4__Double() { Test("Test_CONV_U4__Double", double.NaN); } /* TestMethod("Test_CONV_U4__Double", double.NaN); */
        //[Fact] public void Test_CONV_U4__Double() { Test("Test_CONV_U4__Double", double.NegativeInfinity); } /* TestMethod("Test_CONV_U4__Double", double.NegativeInfinity); */
        //[Fact] public void Test_CONV_U4__Double() { Test("Test_CONV_U4__Double", double.PositiveInfinity); } /* TestMethod("Test_CONV_U4__Double", double.PositiveInfinity); */
        //[Fact] public void Test_CONV_U4__Double() { Test("Test_CONV_U4__Double", double.Epsilon); } /* TestMethod("Test_CONV_U4__Double", double.Epsilon); */
        //[Fact] public void Test_CONV_U4__Double() { Test("Test_CONV_U4__Double", 12345678910111213.1415d); } /* TestMethod("Test_CONV_U4__Double", 12345678910111213.1415d); */
        //[Fact] public void Test_CONV_U4__Double() { Test("Test_CONV_U4__Double", -12345678910111213.1415d); } /* TestMethod("Test_CONV_U4__Double", -12345678910111213.1415d); */
        //[Fact] public void Test_CONV_U8__Double() { Test("Test_CONV_U8__Double", 0.0d); } /* TestMethod("Test_CONV_U8__Double", 0.0d); */
        //[Fact] public void Test_CONV_U8__Double() { Test("Test_CONV_U8__Double", -0.0d); } /* TestMethod("Test_CONV_U8__Double", -0.0d); */
        //[Fact] public void Test_CONV_U8__Double() { Test("Test_CONV_U8__Double", -1.0d); } /* TestMethod("Test_CONV_U8__Double", -1.0d); */
        //[Fact] public void Test_CONV_U8__Double() { Test("Test_CONV_U8__Double", double.MinValue); } /* TestMethod("Test_CONV_U8__Double", double.MinValue); */
        //[Fact] public void Test_CONV_U8__Double() { Test("Test_CONV_U8__Double", 1.0d); } /* TestMethod("Test_CONV_U8__Double", 1.0d); */
        //[Fact] public void Test_CONV_U8__Double() { Test("Test_CONV_U8__Double", double.MaxValue); } /* TestMethod("Test_CONV_U8__Double", double.MaxValue); */
        //[Fact] public void Test_CONV_U8__Double() { Test("Test_CONV_U8__Double", double.NaN); } /* TestMethod("Test_CONV_U8__Double", double.NaN); */
        //[Fact] public void Test_CONV_U8__Double() { Test("Test_CONV_U8__Double", double.NegativeInfinity); } /* TestMethod("Test_CONV_U8__Double", double.NegativeInfinity); */
        //[Fact] public void Test_CONV_U8__Double() { Test("Test_CONV_U8__Double", double.PositiveInfinity); } /* TestMethod("Test_CONV_U8__Double", double.PositiveInfinity); */
        //[Fact] public void Test_CONV_U8__Double() { Test("Test_CONV_U8__Double", double.Epsilon); } /* TestMethod("Test_CONV_U8__Double", double.Epsilon); */
        //[Fact] public void Test_CONV_U8__Double() { Test("Test_CONV_U8__Double", 12345678910111213.1415d); } /* TestMethod("Test_CONV_U8__Double", 12345678910111213.1415d); */
        //[Fact] public void Test_CONV_U8__Double() { Test("Test_CONV_U8__Double", -12345678910111213.1415d); } /* TestMethod("Test_CONV_U8__Double", -12345678910111213.1415d); */
        //[Fact] public void Test_CONV_R4__Double() { Test("Test_CONV_R4__Double", 0.0d); } /* TestMethod("Test_CONV_R4__Double", 0.0d); */
        //[Fact] public void Test_CONV_R4__Double() { Test("Test_CONV_R4__Double", -0.0d); } /* TestMethod("Test_CONV_R4__Double", -0.0d); */
        //[Fact] public void Test_CONV_R4__Double() { Test("Test_CONV_R4__Double", -1.0d); } /* TestMethod("Test_CONV_R4__Double", -1.0d); */
        //[Fact] public void Test_CONV_R4__Double() { Test("Test_CONV_R4__Double", double.MinValue); } /* TestMethod("Test_CONV_R4__Double", double.MinValue); */
        //[Fact] public void Test_CONV_R4__Double() { Test("Test_CONV_R4__Double", 1.0d); } /* TestMethod("Test_CONV_R4__Double", 1.0d); */
        //[Fact] public void Test_CONV_R4__Double() { Test("Test_CONV_R4__Double", double.MaxValue); } /* TestMethod("Test_CONV_R4__Double", double.MaxValue); */
        //[Fact] public void Test_CONV_R4__Double() { Test("Test_CONV_R4__Double", double.NaN); } /* TestMethod("Test_CONV_R4__Double", double.NaN); */
        //[Fact] public void Test_CONV_R4__Double() { Test("Test_CONV_R4__Double", double.NegativeInfinity); } /* TestMethod("Test_CONV_R4__Double", double.NegativeInfinity); */
        //[Fact] public void Test_CONV_R4__Double() { Test("Test_CONV_R4__Double", double.PositiveInfinity); } /* TestMethod("Test_CONV_R4__Double", double.PositiveInfinity); */
        //[Fact] public void Test_CONV_R4__Double() { Test("Test_CONV_R4__Double", double.Epsilon); } /* TestMethod("Test_CONV_R4__Double", double.Epsilon); */
        //[Fact] public void Test_CONV_R4__Double() { Test("Test_CONV_R4__Double", 12345678910111213.1415d); } /* TestMethod("Test_CONV_R4__Double", 12345678910111213.1415d); */
        //[Fact] public void Test_CONV_R4__Double() { Test("Test_CONV_R4__Double", -12345678910111213.1415d); } /* TestMethod("Test_CONV_R4__Double", -12345678910111213.1415d); */
        //[Fact] public void Test_CONV_R8__Double() { Test("Test_CONV_R8__Double", 0.0d); } /* TestMethod("Test_CONV_R8__Double", 0.0d); */
        //[Fact] public void Test_CONV_R8__Double() { Test("Test_CONV_R8__Double", -0.0d); } /* TestMethod("Test_CONV_R8__Double", -0.0d); */
        //[Fact] public void Test_CONV_R8__Double() { Test("Test_CONV_R8__Double", -1.0d); } /* TestMethod("Test_CONV_R8__Double", -1.0d); */
        //[Fact] public void Test_CONV_R8__Double() { Test("Test_CONV_R8__Double", double.MinValue); } /* TestMethod("Test_CONV_R8__Double", double.MinValue); */
        //[Fact] public void Test_CONV_R8__Double() { Test("Test_CONV_R8__Double", 1.0d); } /* TestMethod("Test_CONV_R8__Double", 1.0d); */
        //[Fact] public void Test_CONV_R8__Double() { Test("Test_CONV_R8__Double", double.MaxValue); } /* TestMethod("Test_CONV_R8__Double", double.MaxValue); */
        //[Fact] public void Test_CONV_R8__Double() { Test("Test_CONV_R8__Double", double.NaN); } /* TestMethod("Test_CONV_R8__Double", double.NaN); */
        //[Fact] public void Test_CONV_R8__Double() { Test("Test_CONV_R8__Double", double.NegativeInfinity); } /* TestMethod("Test_CONV_R8__Double", double.NegativeInfinity); */
        //[Fact] public void Test_CONV_R8__Double() { Test("Test_CONV_R8__Double", double.PositiveInfinity); } /* TestMethod("Test_CONV_R8__Double", double.PositiveInfinity); */
        //[Fact] public void Test_CONV_R8__Double() { Test("Test_CONV_R8__Double", double.Epsilon); } /* TestMethod("Test_CONV_R8__Double", double.Epsilon); */
        //[Fact] public void Test_CONV_R8__Double() { Test("Test_CONV_R8__Double", 12345678910111213.1415d); } /* TestMethod("Test_CONV_R8__Double", 12345678910111213.1415d); */
        //[Fact] public void Test_CONV_R8__Double() { Test("Test_CONV_R8__Double", -12345678910111213.1415d); } /* TestMethod("Test_CONV_R8__Double", -12345678910111213.1415d); */
        //[Fact] public void Test_CONV_OVF_I__Double() { Test("Test_CONV_OVF_I__Double", 0.0d); } /* TestMethod("Test_CONV_OVF_I__Double", 0.0d); */
        //[Fact] public void Test_CONV_OVF_I__Double() { Test("Test_CONV_OVF_I__Double", -0.0d); } /* TestMethod("Test_CONV_OVF_I__Double", -0.0d); */
        //[Fact] public void Test_CONV_OVF_I__Double() { Test("Test_CONV_OVF_I__Double", -1.0d); } /* TestMethod("Test_CONV_OVF_I__Double", -1.0d); */
        //[Fact] public void Test_CONV_OVF_I__Double() { Test("Test_CONV_OVF_I__Double", double.MinValue); } /* TestMethodEX("Test_CONV_OVF_I__Double", double.MinValue); */
        //[Fact] public void Test_CONV_OVF_I__Double() { Test("Test_CONV_OVF_I__Double", 1.0d); } /* TestMethod("Test_CONV_OVF_I__Double", 1.0d); */
        //[Fact] public void Test_CONV_OVF_I__Double() { Test("Test_CONV_OVF_I__Double", double.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I__Double", double.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I__Double() { Test("Test_CONV_OVF_I__Double", double.NaN); } /* TestMethodEX("Test_CONV_OVF_I__Double", double.NaN); */
        //[Fact] public void Test_CONV_OVF_I__Double() { Test("Test_CONV_OVF_I__Double", double.NegativeInfinity); } /* TestMethodEX("Test_CONV_OVF_I__Double", double.NegativeInfinity); */
        //[Fact] public void Test_CONV_OVF_I__Double() { Test("Test_CONV_OVF_I__Double", double.PositiveInfinity); } /* TestMethodEX("Test_CONV_OVF_I__Double", double.PositiveInfinity); */
        //[Fact] public void Test_CONV_OVF_I__Double() { Test("Test_CONV_OVF_I__Double", double.Epsilon); } /* TestMethod("Test_CONV_OVF_I__Double", double.Epsilon); */
        //[Fact] public void Test_CONV_OVF_I__Double() { Test("Test_CONV_OVF_I__Double", 12345678910111213.1415d); } /* TestMethodEX("Test_CONV_OVF_I__Double", 12345678910111213.1415d); */
        //[Fact] public void Test_CONV_OVF_I__Double() { Test("Test_CONV_OVF_I__Double", -12345678910111213.1415d); } /* TestMethodEX("Test_CONV_OVF_I__Double", -12345678910111213.1415d); */
        //[Fact] public void Test_CONV_OVF_I1__Double() { Test("Test_CONV_OVF_I1__Double", 0.0d); } /* TestMethod("Test_CONV_OVF_I1__Double", 0.0d); */
        //[Fact] public void Test_CONV_OVF_I1__Double() { Test("Test_CONV_OVF_I1__Double", -0.0d); } /* TestMethod("Test_CONV_OVF_I1__Double", -0.0d); */
        //[Fact] public void Test_CONV_OVF_I1__Double() { Test("Test_CONV_OVF_I1__Double", -1.0d); } /* TestMethod("Test_CONV_OVF_I1__Double", -1.0d); */
        //[Fact] public void Test_CONV_OVF_I1__Double() { Test("Test_CONV_OVF_I1__Double", double.MinValue); } /* TestMethodEX("Test_CONV_OVF_I1__Double", double.MinValue); */
        //[Fact] public void Test_CONV_OVF_I1__Double() { Test("Test_CONV_OVF_I1__Double", 1.0d); } /* TestMethod("Test_CONV_OVF_I1__Double", 1.0d); */
        //[Fact] public void Test_CONV_OVF_I1__Double() { Test("Test_CONV_OVF_I1__Double", double.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I1__Double", double.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I1__Double() { Test("Test_CONV_OVF_I1__Double", double.NaN); } /* TestMethodEX("Test_CONV_OVF_I1__Double", double.NaN); */
        //[Fact] public void Test_CONV_OVF_I1__Double() { Test("Test_CONV_OVF_I1__Double", double.NegativeInfinity); } /* TestMethodEX("Test_CONV_OVF_I1__Double", double.NegativeInfinity); */
        //[Fact] public void Test_CONV_OVF_I1__Double() { Test("Test_CONV_OVF_I1__Double", double.PositiveInfinity); } /* TestMethodEX("Test_CONV_OVF_I1__Double", double.PositiveInfinity); */
        //[Fact] public void Test_CONV_OVF_I1__Double() { Test("Test_CONV_OVF_I1__Double", double.Epsilon); } /* TestMethod("Test_CONV_OVF_I1__Double", double.Epsilon); */
        //[Fact] public void Test_CONV_OVF_I1__Double() { Test("Test_CONV_OVF_I1__Double", 12345678910111213.1415d); } /* TestMethodEX("Test_CONV_OVF_I1__Double", 12345678910111213.1415d); */
        //[Fact] public void Test_CONV_OVF_I1__Double() { Test("Test_CONV_OVF_I1__Double", -12345678910111213.1415d); } /* TestMethodEX("Test_CONV_OVF_I1__Double", -12345678910111213.1415d); */
        //[Fact] public void Test_CONV_OVF_I2__Double() { Test("Test_CONV_OVF_I2__Double", 0.0d); } /* TestMethod("Test_CONV_OVF_I2__Double", 0.0d); */
        //[Fact] public void Test_CONV_OVF_I2__Double() { Test("Test_CONV_OVF_I2__Double", -0.0d); } /* TestMethod("Test_CONV_OVF_I2__Double", -0.0d); */
        //[Fact] public void Test_CONV_OVF_I2__Double() { Test("Test_CONV_OVF_I2__Double", -1.0d); } /* TestMethod("Test_CONV_OVF_I2__Double", -1.0d); */
        //[Fact] public void Test_CONV_OVF_I2__Double() { Test("Test_CONV_OVF_I2__Double", double.MinValue); } /* TestMethodEX("Test_CONV_OVF_I2__Double", double.MinValue); */
        //[Fact] public void Test_CONV_OVF_I2__Double() { Test("Test_CONV_OVF_I2__Double", 1.0d); } /* TestMethod("Test_CONV_OVF_I2__Double", 1.0d); */
        //[Fact] public void Test_CONV_OVF_I2__Double() { Test("Test_CONV_OVF_I2__Double", double.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I2__Double", double.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I2__Double() { Test("Test_CONV_OVF_I2__Double", double.NaN); } /* TestMethodEX("Test_CONV_OVF_I2__Double", double.NaN); */
        //[Fact] public void Test_CONV_OVF_I2__Double() { Test("Test_CONV_OVF_I2__Double", double.NegativeInfinity); } /* TestMethodEX("Test_CONV_OVF_I2__Double", double.NegativeInfinity); */
        //[Fact] public void Test_CONV_OVF_I2__Double() { Test("Test_CONV_OVF_I2__Double", double.PositiveInfinity); } /* TestMethodEX("Test_CONV_OVF_I2__Double", double.PositiveInfinity); */
        //[Fact] public void Test_CONV_OVF_I2__Double() { Test("Test_CONV_OVF_I2__Double", double.Epsilon); } /* TestMethod("Test_CONV_OVF_I2__Double", double.Epsilon); */
        //[Fact] public void Test_CONV_OVF_I2__Double() { Test("Test_CONV_OVF_I2__Double", 12345678910111213.1415d); } /* TestMethodEX("Test_CONV_OVF_I2__Double", 12345678910111213.1415d); */
        //[Fact] public void Test_CONV_OVF_I2__Double() { Test("Test_CONV_OVF_I2__Double", -12345678910111213.1415d); } /* TestMethodEX("Test_CONV_OVF_I2__Double", -12345678910111213.1415d); */
        //[Fact] public void Test_CONV_OVF_I4__Double() { Test("Test_CONV_OVF_I4__Double", 0.0d); } /* TestMethod("Test_CONV_OVF_I4__Double", 0.0d); */
        //[Fact] public void Test_CONV_OVF_I4__Double() { Test("Test_CONV_OVF_I4__Double", -0.0d); } /* TestMethod("Test_CONV_OVF_I4__Double", -0.0d); */
        //[Fact] public void Test_CONV_OVF_I4__Double() { Test("Test_CONV_OVF_I4__Double", -1.0d); } /* TestMethod("Test_CONV_OVF_I4__Double", -1.0d); */
        //[Fact] public void Test_CONV_OVF_I4__Double() { Test("Test_CONV_OVF_I4__Double", double.MinValue); } /* TestMethodEX("Test_CONV_OVF_I4__Double", double.MinValue); */
        //[Fact] public void Test_CONV_OVF_I4__Double() { Test("Test_CONV_OVF_I4__Double", 1.0d); } /* TestMethod("Test_CONV_OVF_I4__Double", 1.0d); */
        //[Fact] public void Test_CONV_OVF_I4__Double() { Test("Test_CONV_OVF_I4__Double", double.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I4__Double", double.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I4__Double() { Test("Test_CONV_OVF_I4__Double", double.NaN); } /* TestMethodEX("Test_CONV_OVF_I4__Double", double.NaN); */
        //[Fact] public void Test_CONV_OVF_I4__Double() { Test("Test_CONV_OVF_I4__Double", double.NegativeInfinity); } /* TestMethodEX("Test_CONV_OVF_I4__Double", double.NegativeInfinity); */
        //[Fact] public void Test_CONV_OVF_I4__Double() { Test("Test_CONV_OVF_I4__Double", double.PositiveInfinity); } /* TestMethodEX("Test_CONV_OVF_I4__Double", double.PositiveInfinity); */
        //[Fact] public void Test_CONV_OVF_I4__Double() { Test("Test_CONV_OVF_I4__Double", double.Epsilon); } /* TestMethod("Test_CONV_OVF_I4__Double", double.Epsilon); */
        //[Fact] public void Test_CONV_OVF_I4__Double() { Test("Test_CONV_OVF_I4__Double", 12345678910111213.1415d); } /* TestMethodEX("Test_CONV_OVF_I4__Double", 12345678910111213.1415d); */
        //[Fact] public void Test_CONV_OVF_I4__Double() { Test("Test_CONV_OVF_I4__Double", -12345678910111213.1415d); } /* TestMethodEX("Test_CONV_OVF_I4__Double", -12345678910111213.1415d); */
        //[Fact] public void Test_CONV_OVF_I8__Double() { Test("Test_CONV_OVF_I8__Double", 0.0d); } /* TestMethod("Test_CONV_OVF_I8__Double", 0.0d); */
        //[Fact] public void Test_CONV_OVF_I8__Double() { Test("Test_CONV_OVF_I8__Double", -0.0d); } /* TestMethod("Test_CONV_OVF_I8__Double", -0.0d); */
        //[Fact] public void Test_CONV_OVF_I8__Double() { Test("Test_CONV_OVF_I8__Double", -1.0d); } /* TestMethod("Test_CONV_OVF_I8__Double", -1.0d); */
        //[Fact] public void Test_CONV_OVF_I8__Double() { Test("Test_CONV_OVF_I8__Double", double.MinValue); } /* TestMethodEX("Test_CONV_OVF_I8__Double", double.MinValue); */
        //[Fact] public void Test_CONV_OVF_I8__Double() { Test("Test_CONV_OVF_I8__Double", 1.0d); } /* TestMethod("Test_CONV_OVF_I8__Double", 1.0d); */
        //[Fact] public void Test_CONV_OVF_I8__Double() { Test("Test_CONV_OVF_I8__Double", double.MaxValue); } /* TestMethodEX("Test_CONV_OVF_I8__Double", double.MaxValue); */
        //[Fact] public void Test_CONV_OVF_I8__Double() { Test("Test_CONV_OVF_I8__Double", double.NaN); } /* TestMethodEX("Test_CONV_OVF_I8__Double", double.NaN); */
        //[Fact] public void Test_CONV_OVF_I8__Double() { Test("Test_CONV_OVF_I8__Double", double.NegativeInfinity); } /* TestMethodEX("Test_CONV_OVF_I8__Double", double.NegativeInfinity); */
        //[Fact] public void Test_CONV_OVF_I8__Double() { Test("Test_CONV_OVF_I8__Double", double.PositiveInfinity); } /* TestMethodEX("Test_CONV_OVF_I8__Double", double.PositiveInfinity); */
        //[Fact] public void Test_CONV_OVF_I8__Double() { Test("Test_CONV_OVF_I8__Double", double.Epsilon); } /* TestMethod("Test_CONV_OVF_I8__Double", double.Epsilon); */
        //[Fact] public void Test_CONV_OVF_I8__Double() { Test("Test_CONV_OVF_I8__Double", 12345678910111213.1415d); } /* TestMethod("Test_CONV_OVF_I8__Double", 12345678910111213.1415d); */
        //[Fact] public void Test_CONV_OVF_I8__Double() { Test("Test_CONV_OVF_I8__Double", -12345678910111213.1415d); } /* TestMethod("Test_CONV_OVF_I8__Double", -12345678910111213.1415d); */
        //[Fact] public void Test_CONV_OVF_U__Double() { Test("Test_CONV_OVF_U__Double", 0.0d); } /* TestMethod("Test_CONV_OVF_U__Double", 0.0d); */
        //[Fact] public void Test_CONV_OVF_U__Double() { Test("Test_CONV_OVF_U__Double", -0.0d); } /* TestMethod("Test_CONV_OVF_U__Double", -0.0d); */
        //[Fact] public void Test_CONV_OVF_U__Double() { Test("Test_CONV_OVF_U__Double", -1.0d); } /* TestMethodEX("Test_CONV_OVF_U__Double", -1.0d); */
        //[Fact] public void Test_CONV_OVF_U__Double() { Test("Test_CONV_OVF_U__Double", double.MinValue); } /* TestMethodEX("Test_CONV_OVF_U__Double", double.MinValue); */
        //[Fact] public void Test_CONV_OVF_U__Double() { Test("Test_CONV_OVF_U__Double", 1.0d); } /* TestMethod("Test_CONV_OVF_U__Double", 1.0d); */
        //[Fact] public void Test_CONV_OVF_U__Double() { Test("Test_CONV_OVF_U__Double", double.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U__Double", double.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U__Double() { Test("Test_CONV_OVF_U__Double", double.NaN); } /* TestMethodEX("Test_CONV_OVF_U__Double", double.NaN); */
        //[Fact] public void Test_CONV_OVF_U__Double() { Test("Test_CONV_OVF_U__Double", double.NegativeInfinity); } /* TestMethodEX("Test_CONV_OVF_U__Double", double.NegativeInfinity); */
        //[Fact] public void Test_CONV_OVF_U__Double() { Test("Test_CONV_OVF_U__Double", double.PositiveInfinity); } /* TestMethodEX("Test_CONV_OVF_U__Double", double.PositiveInfinity); */
        //[Fact] public void Test_CONV_OVF_U__Double() { Test("Test_CONV_OVF_U__Double", double.Epsilon); } /* TestMethod("Test_CONV_OVF_U__Double", double.Epsilon); */
        //[Fact] public void Test_CONV_OVF_U__Double() { Test("Test_CONV_OVF_U__Double", 12345678910111213.1415d); } /* TestMethodEX("Test_CONV_OVF_U__Double", 12345678910111213.1415d); */
        //[Fact] public void Test_CONV_OVF_U__Double() { Test("Test_CONV_OVF_U__Double", -12345678910111213.1415d); } /* TestMethodEX("Test_CONV_OVF_U__Double", -12345678910111213.1415d); */
        //[Fact] public void Test_CONV_OVF_U1__Double() { Test("Test_CONV_OVF_U1__Double", 0.0d); } /* TestMethod("Test_CONV_OVF_U1__Double", 0.0d); */
        //[Fact] public void Test_CONV_OVF_U1__Double() { Test("Test_CONV_OVF_U1__Double", -0.0d); } /* TestMethod("Test_CONV_OVF_U1__Double", -0.0d); */
        //[Fact] public void Test_CONV_OVF_U1__Double() { Test("Test_CONV_OVF_U1__Double", -1.0d); } /* TestMethodEX("Test_CONV_OVF_U1__Double", -1.0d); */
        //[Fact] public void Test_CONV_OVF_U1__Double() { Test("Test_CONV_OVF_U1__Double", double.MinValue); } /* TestMethodEX("Test_CONV_OVF_U1__Double", double.MinValue); */
        //[Fact] public void Test_CONV_OVF_U1__Double() { Test("Test_CONV_OVF_U1__Double", 1.0d); } /* TestMethod("Test_CONV_OVF_U1__Double", 1.0d); */
        //[Fact] public void Test_CONV_OVF_U1__Double() { Test("Test_CONV_OVF_U1__Double", double.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U1__Double", double.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U1__Double() { Test("Test_CONV_OVF_U1__Double", double.NaN); } /* TestMethodEX("Test_CONV_OVF_U1__Double", double.NaN); */
        //[Fact] public void Test_CONV_OVF_U1__Double() { Test("Test_CONV_OVF_U1__Double", double.NegativeInfinity); } /* TestMethodEX("Test_CONV_OVF_U1__Double", double.NegativeInfinity); */
        //[Fact] public void Test_CONV_OVF_U1__Double() { Test("Test_CONV_OVF_U1__Double", double.PositiveInfinity); } /* TestMethodEX("Test_CONV_OVF_U1__Double", double.PositiveInfinity); */
        //[Fact] public void Test_CONV_OVF_U1__Double() { Test("Test_CONV_OVF_U1__Double", double.Epsilon); } /* TestMethod("Test_CONV_OVF_U1__Double", double.Epsilon); */
        //[Fact] public void Test_CONV_OVF_U1__Double() { Test("Test_CONV_OVF_U1__Double", 12345678910111213.1415d); } /* TestMethodEX("Test_CONV_OVF_U1__Double", 12345678910111213.1415d); */
        //[Fact] public void Test_CONV_OVF_U1__Double() { Test("Test_CONV_OVF_U1__Double", -12345678910111213.1415d); } /* TestMethodEX("Test_CONV_OVF_U1__Double", -12345678910111213.1415d); */
        //[Fact] public void Test_CONV_OVF_U2__Double() { Test("Test_CONV_OVF_U2__Double", 0.0d); } /* TestMethod("Test_CONV_OVF_U2__Double", 0.0d); */
        //[Fact] public void Test_CONV_OVF_U2__Double() { Test("Test_CONV_OVF_U2__Double", -0.0d); } /* TestMethod("Test_CONV_OVF_U2__Double", -0.0d); */
        //[Fact] public void Test_CONV_OVF_U2__Double() { Test("Test_CONV_OVF_U2__Double", -1.0d); } /* TestMethodEX("Test_CONV_OVF_U2__Double", -1.0d); */
        //[Fact] public void Test_CONV_OVF_U2__Double() { Test("Test_CONV_OVF_U2__Double", double.MinValue); } /* TestMethodEX("Test_CONV_OVF_U2__Double", double.MinValue); */
        //[Fact] public void Test_CONV_OVF_U2__Double() { Test("Test_CONV_OVF_U2__Double", 1.0d); } /* TestMethod("Test_CONV_OVF_U2__Double", 1.0d); */
        //[Fact] public void Test_CONV_OVF_U2__Double() { Test("Test_CONV_OVF_U2__Double", double.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U2__Double", double.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U2__Double() { Test("Test_CONV_OVF_U2__Double", double.NaN); } /* TestMethodEX("Test_CONV_OVF_U2__Double", double.NaN); */
        //[Fact] public void Test_CONV_OVF_U2__Double() { Test("Test_CONV_OVF_U2__Double", double.NegativeInfinity); } /* TestMethodEX("Test_CONV_OVF_U2__Double", double.NegativeInfinity); */
        //[Fact] public void Test_CONV_OVF_U2__Double() { Test("Test_CONV_OVF_U2__Double", double.PositiveInfinity); } /* TestMethodEX("Test_CONV_OVF_U2__Double", double.PositiveInfinity); */
        //[Fact] public void Test_CONV_OVF_U2__Double() { Test("Test_CONV_OVF_U2__Double", double.Epsilon); } /* TestMethod("Test_CONV_OVF_U2__Double", double.Epsilon); */
        //[Fact] public void Test_CONV_OVF_U2__Double() { Test("Test_CONV_OVF_U2__Double", 12345678910111213.1415d); } /* TestMethodEX("Test_CONV_OVF_U2__Double", 12345678910111213.1415d); */
        //[Fact] public void Test_CONV_OVF_U2__Double() { Test("Test_CONV_OVF_U2__Double", -12345678910111213.1415d); } /* TestMethodEX("Test_CONV_OVF_U2__Double", -12345678910111213.1415d); */
        //[Fact] public void Test_CONV_OVF_U4__Double() { Test("Test_CONV_OVF_U4__Double", 0.0d); } /* TestMethod("Test_CONV_OVF_U4__Double", 0.0d); */
        //[Fact] public void Test_CONV_OVF_U4__Double() { Test("Test_CONV_OVF_U4__Double", -0.0d); } /* TestMethod("Test_CONV_OVF_U4__Double", -0.0d); */
        //[Fact] public void Test_CONV_OVF_U4__Double() { Test("Test_CONV_OVF_U4__Double", -1.0d); } /* TestMethodEX("Test_CONV_OVF_U4__Double", -1.0d); */
        //[Fact] public void Test_CONV_OVF_U4__Double() { Test("Test_CONV_OVF_U4__Double", double.MinValue); } /* TestMethodEX("Test_CONV_OVF_U4__Double", double.MinValue); */
        //[Fact] public void Test_CONV_OVF_U4__Double() { Test("Test_CONV_OVF_U4__Double", 1.0d); } /* TestMethod("Test_CONV_OVF_U4__Double", 1.0d); */
        //[Fact] public void Test_CONV_OVF_U4__Double() { Test("Test_CONV_OVF_U4__Double", double.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U4__Double", double.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U4__Double() { Test("Test_CONV_OVF_U4__Double", double.NaN); } /* TestMethodEX("Test_CONV_OVF_U4__Double", double.NaN); */
        //[Fact] public void Test_CONV_OVF_U4__Double() { Test("Test_CONV_OVF_U4__Double", double.NegativeInfinity); } /* TestMethodEX("Test_CONV_OVF_U4__Double", double.NegativeInfinity); */
        //[Fact] public void Test_CONV_OVF_U4__Double() { Test("Test_CONV_OVF_U4__Double", double.PositiveInfinity); } /* TestMethodEX("Test_CONV_OVF_U4__Double", double.PositiveInfinity); */
        //[Fact] public void Test_CONV_OVF_U4__Double() { Test("Test_CONV_OVF_U4__Double", double.Epsilon); } /* TestMethod("Test_CONV_OVF_U4__Double", double.Epsilon); */
        //[Fact] public void Test_CONV_OVF_U4__Double() { Test("Test_CONV_OVF_U4__Double", 12345678910111213.1415d); } /* TestMethodEX("Test_CONV_OVF_U4__Double", 12345678910111213.1415d); */
        //[Fact] public void Test_CONV_OVF_U4__Double() { Test("Test_CONV_OVF_U4__Double", -12345678910111213.1415d); } /* TestMethodEX("Test_CONV_OVF_U4__Double", -12345678910111213.1415d); */
        //[Fact] public void Test_CONV_OVF_U8__Double() { Test("Test_CONV_OVF_U8__Double", 0.0d); } /* TestMethod("Test_CONV_OVF_U8__Double", 0.0d); */
        //[Fact] public void Test_CONV_OVF_U8__Double() { Test("Test_CONV_OVF_U8__Double", -0.0d); } /* TestMethod("Test_CONV_OVF_U8__Double", -0.0d); */
        //[Fact] public void Test_CONV_OVF_U8__Double() { Test("Test_CONV_OVF_U8__Double", -1.0d); } /* TestMethodEX("Test_CONV_OVF_U8__Double", -1.0d); */
        //[Fact] public void Test_CONV_OVF_U8__Double() { Test("Test_CONV_OVF_U8__Double", double.MinValue); } /* TestMethodEX("Test_CONV_OVF_U8__Double", double.MinValue); */
        //[Fact] public void Test_CONV_OVF_U8__Double() { Test("Test_CONV_OVF_U8__Double", 1.0d); } /* TestMethod("Test_CONV_OVF_U8__Double", 1.0d); */
        //[Fact] public void Test_CONV_OVF_U8__Double() { Test("Test_CONV_OVF_U8__Double", double.MaxValue); } /* TestMethodEX("Test_CONV_OVF_U8__Double", double.MaxValue); */
        //[Fact] public void Test_CONV_OVF_U8__Double() { Test("Test_CONV_OVF_U8__Double", double.NaN); } /* TestMethodEX("Test_CONV_OVF_U8__Double", double.NaN); */
        //[Fact] public void Test_CONV_OVF_U8__Double() { Test("Test_CONV_OVF_U8__Double", double.NegativeInfinity); } /* TestMethodEX("Test_CONV_OVF_U8__Double", double.NegativeInfinity); */
        //[Fact] public void Test_CONV_OVF_U8__Double() { Test("Test_CONV_OVF_U8__Double", double.PositiveInfinity); } /* TestMethodEX("Test_CONV_OVF_U8__Double", double.PositiveInfinity); */
        //[Fact] public void Test_CONV_OVF_U8__Double() { Test("Test_CONV_OVF_U8__Double", double.Epsilon); } /* TestMethod("Test_CONV_OVF_U8__Double", double.Epsilon); */
        //[Fact] public void Test_CONV_OVF_U8__Double() { Test("Test_CONV_OVF_U8__Double", 12345678910111213.1415d); } /* TestMethod("Test_CONV_OVF_U8__Double", 12345678910111213.1415d); */
        //[Fact] public void Test_CONV_OVF_U8__Double() { Test("Test_CONV_OVF_U8__Double", -12345678910111213.1415d); } /* TestMethodEX("Test_CONV_OVF_U8__Double", -12345678910111213.1415d); */

        //[Fact] public void Test_CEQ__Int32() { Test("Test_CEQ__Int32", 1, 1); } /* TestMethod("Test_CEQ__Int32", 1, 1); */
        //[Fact] public void Test_CEQ__Int32() { Test("Test_CEQ__Int32", 0, 1); } /* TestMethod("Test_CEQ__Int32", 0, 1); */
        //[Fact] public void Test_CEQ__Int32() { Test("Test_CEQ__Int32", 1, -1); } /* TestMethod("Test_CEQ__Int32", 1, -1); */
        //[Fact] public void Test_CEQ__UInt32() { Test("Test_CEQ__UInt32", 1U, 1U); } /* TestMethod("Test_CEQ__UInt32", 1U, 1U); */
        //[Fact] public void Test_CEQ__UInt32() { Test("Test_CEQ__UInt32", 0U, 1U); } /* TestMethod("Test_CEQ__UInt32", 0U, 1U); */
        //[Fact] public void Test_CEQ__UInt32() { Test("Test_CEQ__UInt32", 1U, 0U); } /* TestMethod("Test_CEQ__UInt32", 1U, 0U); */
        //[Fact] public void Test_CEQ__Int64() { Test("Test_CEQ__Int64", 1L, 1L); } /* TestMethod("Test_CEQ__Int64", 1L, 1L); */
        //[Fact] public void Test_CEQ__Int64() { Test("Test_CEQ__Int64", 0L, 1L); } /* TestMethod("Test_CEQ__Int64", 0L, 1L); */
        //[Fact] public void Test_CEQ__Int64() { Test("Test_CEQ__Int64", 1L, -1L); } /* TestMethod("Test_CEQ__Int64", 1L, -1L); */
        //[Fact] public void Test_CEQ__UInt64() { Test("Test_CEQ__UInt64", 1UL, 1UL); } /* TestMethod("Test_CEQ__UInt64", 1UL, 1UL); */
        //[Fact] public void Test_CEQ__UInt64() { Test("Test_CEQ__UInt64", 0UL, 1UL); } /* TestMethod("Test_CEQ__UInt64", 0UL, 1UL); */
        //[Fact] public void Test_CEQ__UInt64() { Test("Test_CEQ__UInt64", 1UL, 0UL); } /* TestMethod("Test_CEQ__UInt64", 1UL, 0UL); */
        //[Fact] public void Test_CEQ__IntPtr() { Test("Test_CEQ__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CEQ__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CEQ__IntPtr() { Test("Test_CEQ__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CEQ__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CEQ__IntPtr() { Test("Test_CEQ__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CEQ__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CEQ__UIntPtr() { Test("Test_CEQ__UIntPtr", new UIntPtr(1), new UIntPtr(1)); } /* TestMethod("Test_CEQ__UIntPtr", new UIntPtr(1), new UIntPtr(1)); */
        //[Fact] public void Test_CEQ__UIntPtr() { Test("Test_CEQ__UIntPtr", new UIntPtr(0), new UIntPtr(1)); } /* TestMethod("Test_CEQ__UIntPtr", new UIntPtr(0), new UIntPtr(1)); */
        //[Fact] public void Test_CEQ__UIntPtr() { Test("Test_CEQ__UIntPtr", new UIntPtr(1), new UIntPtr(0)); } /* TestMethod("Test_CEQ__UIntPtr", new UIntPtr(1), new UIntPtr(0)); */
        //[Fact] public void Test_CEQ__IntPtr_Int32() { Test("Test_CEQ__IntPtr_Int32", new IntPtr(1), 1); } /* TestMethod("Test_CEQ__IntPtr_Int32", new IntPtr(1), 1); */
        //[Fact] public void Test_CEQ__IntPtr_Int32() { Test("Test_CEQ__IntPtr_Int32", new IntPtr(0), 1); } /* TestMethod("Test_CEQ__IntPtr_Int32", new IntPtr(0), 1); */
        //[Fact] public void Test_CEQ__IntPtr_Int32() { Test("Test_CEQ__IntPtr_Int32", new IntPtr(1), 0); } /* TestMethod("Test_CEQ__IntPtr_Int32", new IntPtr(1), 0); */
        //[Fact] public void Test_CEQ__Int32_IntPtr() { Test("Test_CEQ__Int32_IntPtr", 1, new IntPtr(1)); } /* TestMethod("Test_CEQ__Int32_IntPtr", 1, new IntPtr(1)); */
        //[Fact] public void Test_CEQ__Int32_IntPtr() { Test("Test_CEQ__Int32_IntPtr", 0, new IntPtr(1)); } /* TestMethod("Test_CEQ__Int32_IntPtr", 0, new IntPtr(1)); */
        //[Fact] public void Test_CEQ__Int32_IntPtr() { Test("Test_CEQ__Int32_IntPtr", 1, new IntPtr(0)); } /* TestMethod("Test_CEQ__Int32_IntPtr", 1, new IntPtr(0)); */
        //[Fact] public void Test_CEQ__Single() { Test("Test_CEQ__Single", 1.0f, 1.0f); } /* TestMethod("Test_CEQ__Single", 1.0f, 1.0f); */
        //[Fact] public void Test_CEQ__Single() { Test("Test_CEQ__Single", 0.0f, 1.0f); } /* TestMethod("Test_CEQ__Single", 0.0f, 1.0f); */
        //[Fact] public void Test_CEQ__Single() { Test("Test_CEQ__Single", 1.0f, 0.0f); } /* TestMethod("Test_CEQ__Single", 1.0f, 0.0f); */
        //[Fact] public void Test_CEQ__Double() { Test("Test_CEQ__Double", 1.0d, 1.0d); } /* TestMethod("Test_CEQ__Double", 1.0d, 1.0d); */
        //[Fact] public void Test_CEQ__Double() { Test("Test_CEQ__Double", 0.0d, 1.0d); } /* TestMethod("Test_CEQ__Double", 0.0d, 1.0d); */
        //[Fact] public void Test_CEQ__Double() { Test("Test_CEQ__Double", 1.0d, 0.0d); } /* TestMethod("Test_CEQ__Double", 1.0d, 0.0d); */
        //[Fact] public void Test_CEQ__Object() { Test("Test_CEQ__Object", null, null); } /* TestMethod("Test_CEQ__Object", null, null); */
        //[Fact] public void Test_CEQ__Object() { Test("Test_CEQ__Object", "1", "0"); } /* TestMethod("Test_CEQ__Object", "1", "0"); */

        //[Fact] public void Test_CGT__Int32() { Test("Test_CGT__Int32", 1, 1); } /* TestMethod("Test_CGT__Int32", 1, 1); */
        //[Fact] public void Test_CGT__Int32() { Test("Test_CGT__Int32", 0, 1); } /* TestMethod("Test_CGT__Int32", 0, 1); */
        //[Fact] public void Test_CGT__Int32() { Test("Test_CGT__Int32", 1, -1); } /* TestMethod("Test_CGT__Int32", 1, -1); */
        //[Fact] public void Test_CGT__UInt32() { Test("Test_CGT__UInt32", 1U, 1U); } /* TestMethod("Test_CGT__UInt32", 1U, 1U); */
        //[Fact] public void Test_CGT__UInt32() { Test("Test_CGT__UInt32", 0U, 1U); } /* TestMethod("Test_CGT__UInt32", 0U, 1U); */
        //[Fact] public void Test_CGT__UInt32() { Test("Test_CGT__UInt32", 1U, 0U); } /* TestMethod("Test_CGT__UInt32", 1U, 0U); */
        //[Fact] public void Test_CGT__Int64() { Test("Test_CGT__Int64", 1L, 1L); } /* TestMethod("Test_CGT__Int64", 1L, 1L); */
        //[Fact] public void Test_CGT__Int64() { Test("Test_CGT__Int64", 0L, 1L); } /* TestMethod("Test_CGT__Int64", 0L, 1L); */
        //[Fact] public void Test_CGT__Int64() { Test("Test_CGT__Int64", 1L, -1L); } /* TestMethod("Test_CGT__Int64", 1L, -1L); */
        //[Fact] public void Test_CGT__UInt64() { Test("Test_CGT__UInt64", 1UL, 1UL); } /* TestMethod("Test_CGT__UInt64", 1UL, 1UL); */
        //[Fact] public void Test_CGT__UInt64() { Test("Test_CGT__UInt64", 0UL, 1UL); } /* TestMethod("Test_CGT__UInt64", 0UL, 1UL); */
        //[Fact] public void Test_CGT__UInt64() { Test("Test_CGT__UInt64", 1UL, 0UL); } /* TestMethod("Test_CGT__UInt64", 1UL, 0UL); */
        //[Fact] public void Test_CGT__IntPtr() { Test("Test_CGT__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CGT__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CGT__IntPtr() { Test("Test_CGT__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CGT__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CGT__IntPtr() { Test("Test_CGT__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CGT__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CGT__UIntPtr() { Test("Test_CGT__UIntPtr", new UIntPtr(1), new UIntPtr(1)); } /* TestMethod("Test_CGT__UIntPtr", new UIntPtr(1), new UIntPtr(1)); */
        //[Fact] public void Test_CGT__UIntPtr() { Test("Test_CGT__UIntPtr", new UIntPtr(0), new UIntPtr(1)); } /* TestMethod("Test_CGT__UIntPtr", new UIntPtr(0), new UIntPtr(1)); */
        //[Fact] public void Test_CGT__UIntPtr() { Test("Test_CGT__UIntPtr", new UIntPtr(1), new UIntPtr(0)); } /* TestMethod("Test_CGT__UIntPtr", new UIntPtr(1), new UIntPtr(0)); */
        //[Fact] public void Test_CGT__IntPtr_Int32() { Test("Test_CGT__IntPtr_Int32", new IntPtr(1), 1); } /* TestMethod("Test_CGT__IntPtr_Int32", new IntPtr(1), 1); */
        //[Fact] public void Test_CGT__IntPtr_Int32() { Test("Test_CGT__IntPtr_Int32", new IntPtr(0), 1); } /* TestMethod("Test_CGT__IntPtr_Int32", new IntPtr(0), 1); */
        //[Fact] public void Test_CGT__IntPtr_Int32() { Test("Test_CGT__IntPtr_Int32", new IntPtr(1), 0); } /* TestMethod("Test_CGT__IntPtr_Int32", new IntPtr(1), 0); */
        //[Fact] public void Test_CGT__Int32_IntPtr() { Test("Test_CGT__Int32_IntPtr", 1, new IntPtr(1)); } /* TestMethod("Test_CGT__Int32_IntPtr", 1, new IntPtr(1)); */
        //[Fact] public void Test_CGT__Int32_IntPtr() { Test("Test_CGT__Int32_IntPtr", 0, new IntPtr(1)); } /* TestMethod("Test_CGT__Int32_IntPtr", 0, new IntPtr(1)); */
        //[Fact] public void Test_CGT__Int32_IntPtr() { Test("Test_CGT__Int32_IntPtr", 1, new IntPtr(0)); } /* TestMethod("Test_CGT__Int32_IntPtr", 1, new IntPtr(0)); */
        //[Fact] public void Test_CGT__Single() { Test("Test_CGT__Single", 1.0f, 1.0f); } /* TestMethod("Test_CGT__Single", 1.0f, 1.0f); */
        //[Fact] public void Test_CGT__Single() { Test("Test_CGT__Single", 0.0f, 1.0f); } /* TestMethod("Test_CGT__Single", 0.0f, 1.0f); */
        //[Fact] public void Test_CGT__Single() { Test("Test_CGT__Single", 1.0f, 0.0f); } /* TestMethod("Test_CGT__Single", 1.0f, 0.0f); */
        //[Fact] public void Test_CGT__Double() { Test("Test_CGT__Double", 1.0d, 1.0d); } /* TestMethod("Test_CGT__Double", 1.0d, 1.0d); */
        //[Fact] public void Test_CGT__Double() { Test("Test_CGT__Double", 0.0d, 1.0d); } /* TestMethod("Test_CGT__Double", 0.0d, 1.0d); */
        //[Fact] public void Test_CGT__Double() { Test("Test_CGT__Double", 1.0d, 0.0d); } /* TestMethod("Test_CGT__Double", 1.0d, 0.0d); */

        //[Fact] public void Test_CLT__Int32() { Test("Test_CLT__Int32", 1, 1); } /* TestMethod("Test_CLT__Int32", 1, 1); */
        //[Fact] public void Test_CLT__Int32() { Test("Test_CLT__Int32", 0, 1); } /* TestMethod("Test_CLT__Int32", 0, 1); */
        //[Fact] public void Test_CLT__Int32() { Test("Test_CLT__Int32", 1, -1); } /* TestMethod("Test_CLT__Int32", 1, -1); */
        //[Fact] public void Test_CLT__UInt32() { Test("Test_CLT__UInt32", 1U, 1U); } /* TestMethod("Test_CLT__UInt32", 1U, 1U); */
        //[Fact] public void Test_CLT__UInt32() { Test("Test_CLT__UInt32", 0U, 1U); } /* TestMethod("Test_CLT__UInt32", 0U, 1U); */
        //[Fact] public void Test_CLT__UInt32() { Test("Test_CLT__UInt32", 1U, 0U); } /* TestMethod("Test_CLT__UInt32", 1U, 0U); */
        //[Fact] public void Test_CLT__Int64() { Test("Test_CLT__Int64", 1L, 1L); } /* TestMethod("Test_CLT__Int64", 1L, 1L); */
        //[Fact] public void Test_CLT__Int64() { Test("Test_CLT__Int64", 0L, 1L); } /* TestMethod("Test_CLT__Int64", 0L, 1L); */
        //[Fact] public void Test_CLT__Int64() { Test("Test_CLT__Int64", 1L, -1L); } /* TestMethod("Test_CLT__Int64", 1L, -1L); */
        //[Fact] public void Test_CLT__UInt64() { Test("Test_CLT__UInt64", 1UL, 1UL); } /* TestMethod("Test_CLT__UInt64", 1UL, 1UL); */
        //[Fact] public void Test_CLT__UInt64() { Test("Test_CLT__UInt64", 0UL, 1UL); } /* TestMethod("Test_CLT__UInt64", 0UL, 1UL); */
        //[Fact] public void Test_CLT__UInt64() { Test("Test_CLT__UInt64", 1UL, 0UL); } /* TestMethod("Test_CLT__UInt64", 1UL, 0UL); */
        //[Fact] public void Test_CLT__IntPtr() { Test("Test_CLT__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CLT__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CLT__IntPtr() { Test("Test_CLT__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CLT__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CLT__IntPtr() { Test("Test_CLT__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CLT__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CLT__UIntPtr() { Test("Test_CLT__UIntPtr", new UIntPtr(1), new UIntPtr(1)); } /* TestMethod("Test_CLT__UIntPtr", new UIntPtr(1), new UIntPtr(1)); */
        //[Fact] public void Test_CLT__UIntPtr() { Test("Test_CLT__UIntPtr", new UIntPtr(0), new UIntPtr(1)); } /* TestMethod("Test_CLT__UIntPtr", new UIntPtr(0), new UIntPtr(1)); */
        //[Fact] public void Test_CLT__UIntPtr() { Test("Test_CLT__UIntPtr", new UIntPtr(1), new UIntPtr(0)); } /* TestMethod("Test_CLT__UIntPtr", new UIntPtr(1), new UIntPtr(0)); */
        //[Fact] public void Test_CLT__IntPtr_Int32() { Test("Test_CLT__IntPtr_Int32", new IntPtr(1), 1); } /* TestMethod("Test_CLT__IntPtr_Int32", new IntPtr(1), 1); */
        //[Fact] public void Test_CLT__IntPtr_Int32() { Test("Test_CLT__IntPtr_Int32", new IntPtr(0), 1); } /* TestMethod("Test_CLT__IntPtr_Int32", new IntPtr(0), 1); */
        //[Fact] public void Test_CLT__IntPtr_Int32() { Test("Test_CLT__IntPtr_Int32", new IntPtr(1), 0); } /* TestMethod("Test_CLT__IntPtr_Int32", new IntPtr(1), 0); */
        //[Fact] public void Test_CLT__Int32_IntPtr() { Test("Test_CLT__Int32_IntPtr", 1, new IntPtr(1)); } /* TestMethod("Test_CLT__Int32_IntPtr", 1, new IntPtr(1)); */
        //[Fact] public void Test_CLT__Int32_IntPtr() { Test("Test_CLT__Int32_IntPtr", 0, new IntPtr(1)); } /* TestMethod("Test_CLT__Int32_IntPtr", 0, new IntPtr(1)); */
        //[Fact] public void Test_CLT__Int32_IntPtr() { Test("Test_CLT__Int32_IntPtr", 1, new IntPtr(0)); } /* TestMethod("Test_CLT__Int32_IntPtr", 1, new IntPtr(0)); */
        //[Fact] public void Test_CLT__Single() { Test("Test_CLT__Single", 1.0f, 1.0f); } /* TestMethod("Test_CLT__Single", 1.0f, 1.0f); */
        //[Fact] public void Test_CLT__Single() { Test("Test_CLT__Single", 0.0f, 1.0f); } /* TestMethod("Test_CLT__Single", 0.0f, 1.0f); */
        //[Fact] public void Test_CLT__Single() { Test("Test_CLT__Single", 1.0f, 0.0f); } /* TestMethod("Test_CLT__Single", 1.0f, 0.0f); */
        //[Fact] public void Test_CLT__Double() { Test("Test_CLT__Double", 1.0d, 1.0d); } /* TestMethod("Test_CLT__Double", 1.0d, 1.0d); */
        //[Fact] public void Test_CLT__Double() { Test("Test_CLT__Double", 0.0d, 1.0d); } /* TestMethod("Test_CLT__Double", 0.0d, 1.0d); */
        //[Fact] public void Test_CLT__Double() { Test("Test_CLT__Double", 1.0d, 0.0d); } /* TestMethod("Test_CLT__Double", 1.0d, 0.0d); */

        //[Fact] public void Test_CGT_UN__Int32() { Test("Test_CGT_UN__Int32", 1, 1); } /* TestMethod("Test_CGT_UN__Int32", 1, 1); */
        //[Fact] public void Test_CGT_UN__Int32() { Test("Test_CGT_UN__Int32", 0, 1); } /* TestMethod("Test_CGT_UN__Int32", 0, 1); */
        //[Fact] public void Test_CGT_UN__Int32() { Test("Test_CGT_UN__Int32", 1, -1); } /* TestMethod("Test_CGT_UN__Int32", 1, -1); */
        //[Fact] public void Test_CGT_UN__UInt32() { Test("Test_CGT_UN__UInt32", 1U, 1U); } /* TestMethod("Test_CGT_UN__UInt32", 1U, 1U); */
        //[Fact] public void Test_CGT_UN__UInt32() { Test("Test_CGT_UN__UInt32", 0U, 1U); } /* TestMethod("Test_CGT_UN__UInt32", 0U, 1U); */
        //[Fact] public void Test_CGT_UN__UInt32() { Test("Test_CGT_UN__UInt32", 1U, 0U); } /* TestMethod("Test_CGT_UN__UInt32", 1U, 0U); */
        //[Fact] public void Test_CGT_UN__Int64() { Test("Test_CGT_UN__Int64", 1L, 1L); } /* TestMethod("Test_CGT_UN__Int64", 1L, 1L); */
        //[Fact] public void Test_CGT_UN__Int64() { Test("Test_CGT_UN__Int64", 0L, 1L); } /* TestMethod("Test_CGT_UN__Int64", 0L, 1L); */
        //[Fact] public void Test_CGT_UN__Int64() { Test("Test_CGT_UN__Int64", 1L, -1L); } /* TestMethod("Test_CGT_UN__Int64", 1L, -1L); */
        //[Fact] public void Test_CGT_UN__UInt64() { Test("Test_CGT_UN__UInt64", 1UL, 1UL); } /* TestMethod("Test_CGT_UN__UInt64", 1UL, 1UL); */
        //[Fact] public void Test_CGT_UN__UInt64() { Test("Test_CGT_UN__UInt64", 0UL, 1UL); } /* TestMethod("Test_CGT_UN__UInt64", 0UL, 1UL); */
        //[Fact] public void Test_CGT_UN__UInt64() { Test("Test_CGT_UN__UInt64", 1UL, 0UL); } /* TestMethod("Test_CGT_UN__UInt64", 1UL, 0UL); */
        //[Fact] public void Test_CGT_UN__IntPtr() { Test("Test_CGT_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CGT_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CGT_UN__IntPtr() { Test("Test_CGT_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CGT_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CGT_UN__IntPtr() { Test("Test_CGT_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CGT_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CGT_UN__UIntPtr() { Test("Test_CGT_UN__UIntPtr", new UIntPtr(1), new UIntPtr(1)); } /* TestMethod("Test_CGT_UN__UIntPtr", new UIntPtr(1), new UIntPtr(1)); */
        //[Fact] public void Test_CGT_UN__UIntPtr() { Test("Test_CGT_UN__UIntPtr", new UIntPtr(0), new UIntPtr(1)); } /* TestMethod("Test_CGT_UN__UIntPtr", new UIntPtr(0), new UIntPtr(1)); */
        //[Fact] public void Test_CGT_UN__UIntPtr() { Test("Test_CGT_UN__UIntPtr", new UIntPtr(1), new UIntPtr(0)); } /* TestMethod("Test_CGT_UN__UIntPtr", new UIntPtr(1), new UIntPtr(0)); */
        //[Fact] public void Test_CGT_UN__IntPtr_Int32() { Test("Test_CGT_UN__IntPtr_Int32", new IntPtr(1), 1); } /* TestMethod("Test_CGT_UN__IntPtr_Int32", new IntPtr(1), 1); */
        //[Fact] public void Test_CGT_UN__IntPtr_Int32() { Test("Test_CGT_UN__IntPtr_Int32", new IntPtr(0), 1); } /* TestMethod("Test_CGT_UN__IntPtr_Int32", new IntPtr(0), 1); */
        //[Fact] public void Test_CGT_UN__IntPtr_Int32() { Test("Test_CGT_UN__IntPtr_Int32", new IntPtr(1), 0); } /* TestMethod("Test_CGT_UN__IntPtr_Int32", new IntPtr(1), 0); */
        //[Fact] public void Test_CGT_UN__Int32_IntPtr() { Test("Test_CGT_UN__Int32_IntPtr", 1, new IntPtr(1)); } /* TestMethod("Test_CGT_UN__Int32_IntPtr", 1, new IntPtr(1)); */
        //[Fact] public void Test_CGT_UN__Int32_IntPtr() { Test("Test_CGT_UN__Int32_IntPtr", 0, new IntPtr(1)); } /* TestMethod("Test_CGT_UN__Int32_IntPtr", 0, new IntPtr(1)); */
        //[Fact] public void Test_CGT_UN__Int32_IntPtr() { Test("Test_CGT_UN__Int32_IntPtr", 1, new IntPtr(0)); } /* TestMethod("Test_CGT_UN__Int32_IntPtr", 1, new IntPtr(0)); */

        //[Fact] public void Test_CLT_UN__Int32() { Test("Test_CLT_UN__Int32", 1, 1); } /* TestMethod("Test_CLT_UN__Int32", 1, 1); */
        //[Fact] public void Test_CLT_UN__Int32() { Test("Test_CLT_UN__Int32", 0, 1); } /* TestMethod("Test_CLT_UN__Int32", 0, 1); */
        //[Fact] public void Test_CLT_UN__Int32() { Test("Test_CLT_UN__Int32", 1, -1); } /* TestMethod("Test_CLT_UN__Int32", 1, -1); */
        //[Fact] public void Test_CLT_UN__UInt32() { Test("Test_CLT_UN__UInt32", 1U, 1U); } /* TestMethod("Test_CLT_UN__UInt32", 1U, 1U); */
        //[Fact] public void Test_CLT_UN__UInt32() { Test("Test_CLT_UN__UInt32", 0U, 1U); } /* TestMethod("Test_CLT_UN__UInt32", 0U, 1U); */
        //[Fact] public void Test_CLT_UN__UInt32() { Test("Test_CLT_UN__UInt32", 1U, 0U); } /* TestMethod("Test_CLT_UN__UInt32", 1U, 0U); */
        //[Fact] public void Test_CLT_UN__Int64() { Test("Test_CLT_UN__Int64", 1L, 1L); } /* TestMethod("Test_CLT_UN__Int64", 1L, 1L); */
        //[Fact] public void Test_CLT_UN__Int64() { Test("Test_CLT_UN__Int64", 0L, 1L); } /* TestMethod("Test_CLT_UN__Int64", 0L, 1L); */
        //[Fact] public void Test_CLT_UN__Int64() { Test("Test_CLT_UN__Int64", 1L, -1L); } /* TestMethod("Test_CLT_UN__Int64", 1L, -1L); */
        //[Fact] public void Test_CLT_UN__UInt64() { Test("Test_CLT_UN__UInt64", 1UL, 1UL); } /* TestMethod("Test_CLT_UN__UInt64", 1UL, 1UL); */
        //[Fact] public void Test_CLT_UN__UInt64() { Test("Test_CLT_UN__UInt64", 0UL, 1UL); } /* TestMethod("Test_CLT_UN__UInt64", 0UL, 1UL); */
        //[Fact] public void Test_CLT_UN__UInt64() { Test("Test_CLT_UN__UInt64", 1UL, 0UL); } /* TestMethod("Test_CLT_UN__UInt64", 1UL, 0UL); */
        //[Fact] public void Test_CLT_UN__IntPtr() { Test("Test_CLT_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CLT_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CLT_UN__IntPtr() { Test("Test_CLT_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_CLT_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_CLT_UN__IntPtr() { Test("Test_CLT_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_CLT_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_CLT_UN__UIntPtr() { Test("Test_CLT_UN__UIntPtr", new UIntPtr(1), new UIntPtr(1)); } /* TestMethod("Test_CLT_UN__UIntPtr", new UIntPtr(1), new UIntPtr(1)); */
        //[Fact] public void Test_CLT_UN__UIntPtr() { Test("Test_CLT_UN__UIntPtr", new UIntPtr(0), new UIntPtr(1)); } /* TestMethod("Test_CLT_UN__UIntPtr", new UIntPtr(0), new UIntPtr(1)); */
        //[Fact] public void Test_CLT_UN__UIntPtr() { Test("Test_CLT_UN__UIntPtr", new UIntPtr(1), new UIntPtr(0)); } /* TestMethod("Test_CLT_UN__UIntPtr", new UIntPtr(1), new UIntPtr(0)); */
        //[Fact] public void Test_CLT_UN__IntPtr_Int32() { Test("Test_CLT_UN__IntPtr_Int32", new IntPtr(1), 1); } /* TestMethod("Test_CLT_UN__IntPtr_Int32", new IntPtr(1), 1); */
        //[Fact] public void Test_CLT_UN__IntPtr_Int32() { Test("Test_CLT_UN__IntPtr_Int32", new IntPtr(0), 1); } /* TestMethod("Test_CLT_UN__IntPtr_Int32", new IntPtr(0), 1); */
        //[Fact] public void Test_CLT_UN__IntPtr_Int32() { Test("Test_CLT_UN__IntPtr_Int32", new IntPtr(1), 0); } /* TestMethod("Test_CLT_UN__IntPtr_Int32", new IntPtr(1), 0); */
        //[Fact] public void Test_CLT_UN__Int32_IntPtr() { Test("Test_CLT_UN__Int32_IntPtr", 1, new IntPtr(1)); } /* TestMethod("Test_CLT_UN__Int32_IntPtr", 1, new IntPtr(1)); */
        //[Fact] public void Test_CLT_UN__Int32_IntPtr() { Test("Test_CLT_UN__Int32_IntPtr", 0, new IntPtr(1)); } /* TestMethod("Test_CLT_UN__Int32_IntPtr", 0, new IntPtr(1)); */
        //[Fact] public void Test_CLT_UN__Int32_IntPtr() { Test("Test_CLT_UN__Int32_IntPtr", 1, new IntPtr(0)); } /* TestMethod("Test_CLT_UN__Int32_IntPtr", 1, new IntPtr(0)); */

        [Fact] public void Test_BR() { Test("Test_BR"); } /* TestMethod("Test_BR"); */
        [Fact] public void Test_BR_S() { Test("Test_BR_S"); } /* TestMethod("Test_BR_S"); */
        [Fact] public void Test_BRFALSE() { Test("Test_BRFALSE"); } /* TestMethod("Test_BRFALSE"); */
        [Fact] public void Test_BRFALSE_S() { Test("Test_BRFALSE_S"); } /* TestMethod("Test_BRFALSE_S"); */
        [Fact] public void Test_BRTRUE() { Test("Test_BRTRUE"); } /* TestMethod("Test_BRTRUE"); */
        [Fact] public void Test_BRTRUE_S() { Test("Test_BRTRUE_S"); } /* TestMethod("Test_BRTRUE_S"); */
        [Fact] public void Test_BEQ() { Test("Test_BEQ"); } /* TestMethod("Test_BEQ"); */
        [Fact] public void Test_BEQ_S() { Test("Test_BEQ_S"); } /* TestMethod("Test_BEQ_S"); */
        [Fact] public void Test_BNE_UN() { Test("Test_BNE_UN"); } /* TestMethod("Test_BNE_UN"); */
        [Fact] public void Test_BNE_UN_S() { Test("Test_BNE_UN_S"); } /* TestMethod("Test_BNE_UN_S"); */
        [Fact] public void Test_BGE() { Test("Test_BGE"); } /* TestMethod("Test_BGE"); */
        [Fact] public void Test_BGE_S() { Test("Test_BGE_S"); } /* TestMethod("Test_BGE_S"); */
        [Fact] public void Test_BGE_UN() { Test("Test_BGE_UN"); } /* TestMethod("Test_BGE_UN"); */
        [Fact] public void Test_BGE_UN_S() { Test("Test_BGE_UN_S"); } /* TestMethod("Test_BGE_UN_S"); */
        [Fact] public void Test_BGT() { Test("Test_BGT"); } /* TestMethod("Test_BGT"); */
        [Fact] public void Test_BGT_S() { Test("Test_BGT_S"); } /* TestMethod("Test_BGT_S"); */
        [Fact] public void Test_BGT_UN() { Test("Test_BGT_UN"); } /* TestMethod("Test_BGT_UN"); */
        [Fact] public void Test_BGT_UN_S() { Test("Test_BGT_UN_S"); } /* TestMethod("Test_BGT_UN_S"); */
        [Fact] public void Test_BLE() { Test("Test_BLE"); } /* TestMethod("Test_BLE"); */
        [Fact] public void Test_BLE_S() { Test("Test_BLE_S"); } /* TestMethod("Test_BLE_S"); */
        [Fact] public void Test_BLE_UN() { Test("Test_BLE_UN"); } /* TestMethod("Test_BLE_UN"); */
        [Fact] public void Test_BLE_UN_S() { Test("Test_BLE_UN_S"); } /* TestMethod("Test_BLE_UN_S"); */
        [Fact] public void Test_BLT() { Test("Test_BLT"); } /* TestMethod("Test_BLT"); */
        [Fact] public void Test_BLT_S() { Test("Test_BLT_S"); } /* TestMethod("Test_BLT_S"); */
        [Fact] public void Test_BLT_UN() { Test("Test_BLT_UN"); } /* TestMethod("Test_BLT_UN"); */
        [Fact] public void Test_BLT_UN_S() { Test("Test_BLT_UN_S"); } /* TestMethod("Test_BLT_UN_S"); */
        //[Fact] public void Test_BRFALSE__Int32() { Test("Test_BRFALSE__Int32", 0); } /* TestMethod("Test_BRFALSE__Int32", 0); */
        //[Fact] public void Test_BRFALSE__Int32() { Test("Test_BRFALSE__Int32", 1); } /* TestMethod("Test_BRFALSE__Int32", 1); */
        //[Fact] public void Test_BRFALSE_S__Int32() { Test("Test_BRFALSE_S__Int32", 0); } /* TestMethod("Test_BRFALSE_S__Int32", 0); */
        //[Fact] public void Test_BRFALSE_S__Int32() { Test("Test_BRFALSE_S__Int32", 1); } /* TestMethod("Test_BRFALSE_S__Int32", 1); */
        //[Fact] public void Test_BRFALSE__Int64() { Test("Test_BRFALSE__Int64", 0L); } /* TestMethod("Test_BRFALSE__Int64", 0L); */
        //[Fact] public void Test_BRFALSE__Int64() { Test("Test_BRFALSE__Int64", 1L); } /* TestMethod("Test_BRFALSE__Int64", 1L); */
        //[Fact] public void Test_BRFALSE_S__Int64() { Test("Test_BRFALSE_S__Int64", 0L); } /* TestMethod("Test_BRFALSE_S__Int64", 0L); */
        //[Fact] public void Test_BRFALSE_S__Int64() { Test("Test_BRFALSE_S__Int64", 1L); } /* TestMethod("Test_BRFALSE_S__Int64", 1L); */
        //[Fact] public void Test_BRFALSE__Single() { Test("Test_BRFALSE__Single", 0.0f); } /* TestMethod("Test_BRFALSE__Single", 0.0f); */
        //[Fact] public void Test_BRFALSE__Single() { Test("Test_BRFALSE__Single", -0.0f); } /* TestMethod("Test_BRFALSE__Single", -0.0f); */
        //[Fact] public void Test_BRFALSE__Single() { Test("Test_BRFALSE__Single", 1.0f); } /* TestMethod("Test_BRFALSE__Single", 1.0f); */
        //[Fact] public void Test_BRFALSE_S__Single() { Test("Test_BRFALSE_S__Single", 0.0f); } /* TestMethod("Test_BRFALSE_S__Single", 0.0f); */
        //[Fact] public void Test_BRFALSE_S__Single() { Test("Test_BRFALSE_S__Single", -0.0f); } /* TestMethod("Test_BRFALSE_S__Single", -0.0f); */
        //[Fact] public void Test_BRFALSE_S__Single() { Test("Test_BRFALSE_S__Single", 1.0f); } /* TestMethod("Test_BRFALSE_S__Single", 1.0f); */
        //[Fact] public void Test_BRFALSE__Double() { Test("Test_BRFALSE__Double", 0.0d); } /* TestMethod("Test_BRFALSE__Double", 0.0d); */
        //[Fact] public void Test_BRFALSE__Double() { Test("Test_BRFALSE__Double", -0.0d); } /* TestMethod("Test_BRFALSE__Double", -0.0d); */
        //[Fact] public void Test_BRFALSE__Double() { Test("Test_BRFALSE__Double", 1.0d); } /* TestMethod("Test_BRFALSE__Double", 1.0d); */
        //[Fact] public void Test_BRFALSE_S__Double() { Test("Test_BRFALSE_S__Double", 0.0d); } /* TestMethod("Test_BRFALSE_S__Double", 0.0d); */
        //[Fact] public void Test_BRFALSE_S__Double() { Test("Test_BRFALSE_S__Double", -0.0d); } /* TestMethod("Test_BRFALSE_S__Double", -0.0d); */
        //[Fact] public void Test_BRFALSE_S__Double() { Test("Test_BRFALSE_S__Double", 1.0d); } /* TestMethod("Test_BRFALSE_S__Double", 1.0d); */
        //[Fact] public void Test_BRFALSE__IntPtr() { Test("Test_BRFALSE__IntPtr", new IntPtr(0)); } /* TestMethod("Test_BRFALSE__IntPtr", new IntPtr(0)); */
        //[Fact] public void Test_BRFALSE__IntPtr() { Test("Test_BRFALSE__IntPtr", new IntPtr(1)); } /* TestMethod("Test_BRFALSE__IntPtr", new IntPtr(1)); */
        //[Fact] public void Test_BRFALSE_S__IntPtr() { Test("Test_BRFALSE_S__IntPtr", new IntPtr(0)); } /* TestMethod("Test_BRFALSE_S__IntPtr", new IntPtr(0)); */
        //[Fact] public void Test_BRFALSE_S__IntPtr() { Test("Test_BRFALSE_S__IntPtr", new IntPtr(1)); } /* TestMethod("Test_BRFALSE_S__IntPtr", new IntPtr(1)); */
        //[Fact] public void Test_BRFALSE__Object() { Test("Test_BRFALSE__Object", new object[] { null }); } /* TestMethod("Test_BRFALSE__Object", new object[] { null }); */
        //[Fact] public void Test_BRFALSE__Object() { Test("Test_BRFALSE__Object", "hello"); } /* TestMethod("Test_BRFALSE__Object", "hello"); */
        //[Fact] public void Test_BRFALSE_S__Object() { Test("Test_BRFALSE_S__Object", new object[] { null }); } /* TestMethod("Test_BRFALSE_S__Object", new object[] { null }); */
        //[Fact] public void Test_BRFALSE_S__Object() { Test("Test_BRFALSE_S__Object", "hello"); } /* TestMethod("Test_BRFALSE_S__Object", "hello"); */
        //[Fact] public void Test_BRTRUE__Int32() { Test("Test_BRTRUE__Int32", 0); } /* TestMethod("Test_BRTRUE__Int32", 0); */
        //[Fact] public void Test_BRTRUE__Int32() { Test("Test_BRTRUE__Int32", 1); } /* TestMethod("Test_BRTRUE__Int32", 1); */
        //[Fact] public void Test_BRTRUE_S__Int32() { Test("Test_BRTRUE_S__Int32", 0); } /* TestMethod("Test_BRTRUE_S__Int32", 0); */
        //[Fact] public void Test_BRTRUE_S__Int32() { Test("Test_BRTRUE_S__Int32", 1); } /* TestMethod("Test_BRTRUE_S__Int32", 1); */
        //[Fact] public void Test_BRTRUE__Int64() { Test("Test_BRTRUE__Int64", 0L); } /* TestMethod("Test_BRTRUE__Int64", 0L); */
        //[Fact] public void Test_BRTRUE__Int64() { Test("Test_BRTRUE__Int64", 1L); } /* TestMethod("Test_BRTRUE__Int64", 1L); */
        //[Fact] public void Test_BRTRUE_S__Int64() { Test("Test_BRTRUE_S__Int64", 0L); } /* TestMethod("Test_BRTRUE_S__Int64", 0L); */
        //[Fact] public void Test_BRTRUE_S__Int64() { Test("Test_BRTRUE_S__Int64", 1L); } /* TestMethod("Test_BRTRUE_S__Int64", 1L); */
        //[Fact] public void Test_BRTRUE__Single() { Test("Test_BRTRUE__Single", 0.0f); } /* TestMethod("Test_BRTRUE__Single", 0.0f); */
        //[Fact] public void Test_BRTRUE__Single() { Test("Test_BRTRUE__Single", -0.0f); } /* TestMethod("Test_BRTRUE__Single", -0.0f); */
        //[Fact] public void Test_BRTRUE__Single() { Test("Test_BRTRUE__Single", 1.0f); } /* TestMethod("Test_BRTRUE__Single", 1.0f); */

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-0.0f)]
        [InlineData(1.0f)]
        public void Test_BRTRUE_S__Single(float value) { Test("Test_BRTRUE_S__Single", value); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(1.0d)]
        public void Test_BRTRUE__Double(double value) { Test("Test_BRTRUE__Double", value); }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-0.0d)]
        [InlineData(1.0d)]
        public void Test_BRTRUE_S__Double(double value) { Test("Test_BRTRUE_S__Double", value); }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Test_BRTRUE__IntPtr(int value) { Test("Test_BRTRUE__IntPtr", new IntPtr(value)); }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Test_BRTRUE_S__IntPtr(int value) { Test("Test_BRTRUE_S__IntPtr", new IntPtr(value)); }
        /*
        [Fact] public void Test_BRTRUE__Object() { Test("Test_BRTRUE__Object", new object[] { null }); } * TestMethod("Test_BRTRUE__Object", new object[] { null }); *
        [Fact] public void Test_BRTRUE__Object() { Test("Test_BRTRUE__Object", "hello"); } /* TestMethod("Test_BRTRUE__Object", "hello"); *
        [Fact] public void Test_BRTRUE_S__Object() { Test("Test_BRTRUE_S__Object", new object[] { null }); } /* TestMethod("Test_BRTRUE_S__Object", new object[] { null }); *
        [Fact] public void Test_BRTRUE_S__Object() { Test("Test_BRTRUE_S__Object", "hello"); } /* TestMethod("Test_BRTRUE_S__Object", "hello"); *

        TestMethod_BR("Test_BEQ__Int32", "Test_BNE_UN__Int32", 1, 1);
        TestMethod_BR("Test_BEQ__Int32", "Test_BNE_UN__Int32", 0, 1);
        TestMethod_BR("Test_BEQ__Int32", "Test_BNE_UN__Int32", 1, -1);
        TestMethod_BR("Test_BEQ__Int64", "Test_BNE_UN__Int64", 1L, 1L);
        TestMethod_BR("Test_BEQ__Int64", "Test_BNE_UN__Int64", 0L, 1L);
        TestMethod_BR("Test_BEQ__Int64", "Test_BNE_UN__Int64", 1L, -1L);
        TestMethod_BR("Test_BEQ__IntPtr", "Test_BNE_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L));
			TestMethod_BR("Test_BEQ__IntPtr", "Test_BNE_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L));
			TestMethod_BR("Test_BEQ__IntPtr", "Test_BNE_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L));

			TestMethod_BR("Test_BEQ_S__Int32", "Test_BNE_UN_S__Int32", 1, 1);
        TestMethod_BR("Test_BEQ_S__Int32", "Test_BNE_UN_S__Int32", 0, 1);
        TestMethod_BR("Test_BEQ_S__Int32", "Test_BNE_UN_S__Int32", 1, -1);
        TestMethod_BR("Test_BEQ_S__Int64", "Test_BNE_UN_S__Int64", 1L, 1L);
        TestMethod_BR("Test_BEQ_S__Int64", "Test_BNE_UN_S__Int64", 0L, 1L);
        TestMethod_BR("Test_BEQ_S__Int64", "Test_BNE_UN_S__Int64", 1L, -1L);
        TestMethod_BR("Test_BEQ_S__IntPtr", "Test_BNE_UN_S__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L));
			TestMethod_BR("Test_BEQ_S__IntPtr", "Test_BNE_UN_S__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L));
			TestMethod_BR("Test_BEQ_S__IntPtr", "Test_BNE_UN_S__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L));
			TestMethod_BR("Test_BEQ_S__Single", "Test_BNE_UN_S__Single", 1.0f, 1.0f);
        TestMethod_BR("Test_BEQ_S__Single", "Test_BNE_UN_S__Single", 0.0f, 1.0f);
        TestMethod_BR("Test_BEQ_S__Single", "Test_BNE_UN_S__Single", 1.0f, 0.0f);
        TestMethod_BR("Test_BEQ_S__Double", "Test_BNE_UN_S__Double", 1.0d, 1.0d);
        TestMethod_BR("Test_BEQ_S__Double", "Test_BNE_UN_S__Double", 0.0d, 1.0d);
        TestMethod_BR("Test_BEQ_S__Double", "Test_BNE_UN_S__Double", 1.0d, 0.0d);

        TestMethod_BR("Test_BGE__Int32", "Test_BLT__Int32", 1, 1);
        TestMethod_BR("Test_BGE__Int32", "Test_BLT__Int32", 0, 1);
        TestMethod_BR("Test_BGE__Int32", "Test_BLT__Int32", 1, -1);
        TestMethod_BR("Test_BGE__Int64", "Test_BLT__Int64", 1L, 1L);
        TestMethod_BR("Test_BGE__Int64", "Test_BLT__Int64", 0L, 1L);
        TestMethod_BR("Test_BGE__Int64", "Test_BLT__Int64", 1L, -1L);
        TestMethod_BR("Test_BGE__IntPtr", "Test_BLT__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L));
			TestMethod_BR("Test_BGE__IntPtr", "Test_BLT__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L));
			TestMethod_BR("Test_BGE__IntPtr", "Test_BLT__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L));
			TestMethod_BR("Test_BGE__Single", "Test_BLT__Single", 1.0f, 1.0f);
        TestMethod_BR("Test_BGE__Single", "Test_BLT__Single", 0.0f, 1.0f);
        TestMethod_BR("Test_BGE__Single", "Test_BLT__Single", 1.0f, 0.0f);
        TestMethod_BR("Test_BGE__Double", "Test_BLT__Double", 1.0d, 1.0d);
        TestMethod_BR("Test_BGE__Double", "Test_BLT__Double", 0.0d, 1.0d);
        TestMethod_BR("Test_BGE__Double", "Test_BLT__Double", 1.0d, 0.0d);

        TestMethod_BR("Test_BGE_S__Int32", "Test_BLT_S__Int32", 1, 1);
        TestMethod_BR("Test_BGE_S__Int32", "Test_BLT_S__Int32", 0, 1);
        TestMethod_BR("Test_BGE_S__Int32", "Test_BLT_S__Int32", 1, -1);
        TestMethod_BR("Test_BGE_S__Int64", "Test_BLT_S__Int64", 1L, 1L);
        TestMethod_BR("Test_BGE_S__Int64", "Test_BLT_S__Int64", 0L, 1L);
        TestMethod_BR("Test_BGE_S__Int64", "Test_BLT_S__Int64", 1L, -1L);
        TestMethod_BR("Test_BGE_S__IntPtr", "Test_BLT_S__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L));
			TestMethod_BR("Test_BGE_S__IntPtr", "Test_BLT_S__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L));
			TestMethod_BR("Test_BGE_S__IntPtr", "Test_BLT_S__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L));
			TestMethod_BR("Test_BGE_S__Single", "Test_BLT_S__Single", 1.0f, 1.0f);
        TestMethod_BR("Test_BGE_S__Single", "Test_BLT_S__Single", 0.0f, 1.0f);
        TestMethod_BR("Test_BGE_S__Single", "Test_BLT_S__Single", 1.0f, 0.0f);
        TestMethod_BR("Test_BGE_S__Double", "Test_BLT_S__Double", 1.0d, 1.0d);
        TestMethod_BR("Test_BGE_S__Double", "Test_BLT_S__Double", 0.0d, 1.0d);
        TestMethod_BR("Test_BGE_S__Double", "Test_BLT_S__Double", 1.0d, 0.0d);

        TestMethod_BR("Test_BGT__Int32", "Test_BLE__Int32", 1, 1);
        TestMethod_BR("Test_BGT__Int32", "Test_BLE__Int32", 0, 1);
        TestMethod_BR("Test_BGT__Int32", "Test_BLE__Int32", 1, -1);
        TestMethod_BR("Test_BGT__Int64", "Test_BLE__Int64", 1L, 1L);
        TestMethod_BR("Test_BGT__Int64", "Test_BLE__Int64", 0L, 1L);
        TestMethod_BR("Test_BGT__Int64", "Test_BLE__Int64", 1L, -1L);
        TestMethod_BR("Test_BGT__IntPtr", "Test_BLE__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L));
			TestMethod_BR("Test_BGT__IntPtr", "Test_BLE__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L));
			TestMethod_BR("Test_BGT__IntPtr", "Test_BLE__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L));
			TestMethod_BR("Test_BGT__Single", "Test_BLE__Single", 1.0f, 1.0f);
        TestMethod_BR("Test_BGT__Single", "Test_BLE__Single", 0.0f, 1.0f);
        TestMethod_BR("Test_BGT__Single", "Test_BLE__Single", 1.0f, 0.0f);
        TestMethod_BR("Test_BGT__Double", "Test_BLE__Double", 1.0d, 1.0d);
        TestMethod_BR("Test_BGT__Double", "Test_BLE__Double", 0.0d, 1.0d);
        TestMethod_BR("Test_BGT__Double", "Test_BLE__Double", 1.0d, 0.0d);

        TestMethod_BR("Test_BGT_S__Int32", "Test_BLE_S__Int32", 1, 1);
        TestMethod_BR("Test_BGT_S__Int32", "Test_BLE_S__Int32", 0, 1);
        TestMethod_BR("Test_BGT_S__Int32", "Test_BLE_S__Int32", 1, -1);
        TestMethod_BR("Test_BGT_S__Int64", "Test_BLE_S__Int64", 1L, 1L);
        TestMethod_BR("Test_BGT_S__Int64", "Test_BLE_S__Int64", 0L, 1L);
        TestMethod_BR("Test_BGT_S__Int64", "Test_BLE_S__Int64", 1L, -1L);
        TestMethod_BR("Test_BGT_S__IntPtr", "Test_BLE_S__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L));
			TestMethod_BR("Test_BGT_S__IntPtr", "Test_BLE_S__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L));
			TestMethod_BR("Test_BGT_S__IntPtr", "Test_BLE_S__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L));
			TestMethod_BR("Test_BGT_S__Single", "Test_BLE_S__Single", 1.0f, 1.0f);
        TestMethod_BR("Test_BGT_S__Single", "Test_BLE_S__Single", 0.0f, 1.0f);
        TestMethod_BR("Test_BGT_S__Single", "Test_BLE_S__Single", 1.0f, 0.0f);
        TestMethod_BR("Test_BGT_S__Double", "Test_BLE_S__Double", 1.0d, 1.0d);
        TestMethod_BR("Test_BGT_S__Double", "Test_BLE_S__Double", 0.0d, 1.0d);
        TestMethod_BR("Test_BGT_S__Double", "Test_BLE_S__Double", 1.0d, 0.0d);

        TestMethod_BR("Test_BGE_UN__Int32", "Test_BLT_UN__Int32", 1, 1);
        TestMethod_BR("Test_BGE_UN__Int32", "Test_BLT_UN__Int32", 0, 1);
        TestMethod_BR("Test_BGE_UN__Int32", "Test_BLT_UN__Int32", 1, 0);
        TestMethod_BR("Test_BGE_UN__Int64", "Test_BLT_UN__Int64", 1L, 1L);
        TestMethod_BR("Test_BGE_UN__Int64", "Test_BLT_UN__Int64", 0L, 1L);
        TestMethod_BR("Test_BGE_UN__Int64", "Test_BLT_UN__Int64", 1L, 0L);
        TestMethod_BR("Test_BGE_UN__IntPtr", "Test_BLT_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L));
			TestMethod_BR("Test_BGE_UN__IntPtr", "Test_BLT_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L));
			TestMethod_BR("Test_BGE_UN__IntPtr", "Test_BLT_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L));

			TestMethod_BR("Test_BGE_UN_S__Int32", "Test_BLT_UN_S__Int32", 1, 1);
        TestMethod_BR("Test_BGE_UN_S__Int32", "Test_BLT_UN_S__Int32", 0, 1);
        TestMethod_BR("Test_BGE_UN_S__Int32", "Test_BLT_UN_S__Int32", 1, 0);
        TestMethod_BR("Test_BGE_UN_S__Int64", "Test_BLT_UN_S__Int64", 1L, 1L);
        TestMethod_BR("Test_BGE_UN_S__Int64", "Test_BLT_UN_S__Int64", 0L, 1L);
        TestMethod_BR("Test_BGE_UN_S__Int64", "Test_BLT_UN_S__Int64", 1L, 0L);
        TestMethod_BR("Test_BGE_UN_S__IntPtr", "Test_BLT_UN_S__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L));
			TestMethod_BR("Test_BGE_UN_S__IntPtr", "Test_BLT_UN_S__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L));
			TestMethod_BR("Test_BGE_UN_S__IntPtr", "Test_BLT_UN_S__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L));
			TestMethod_BR("Test_BGE_UN_S__Single", "Test_BLT_UN_S__Single", 1.0f, 1.0f);
        TestMethod_BR("Test_BGE_UN_S__Single", "Test_BLT_UN_S__Single", 0.0f, 1.0f);
        TestMethod_BR("Test_BGE_UN_S__Single", "Test_BLT_UN_S__Single", 1.0f, 0.0f);
        TestMethod_BR("Test_BGE_UN_S__Double", "Test_BLT_UN_S__Double", 1.0d, 1.0d);
        TestMethod_BR("Test_BGE_UN_S__Double", "Test_BLT_UN_S__Double", 0.0d, 1.0d);
        TestMethod_BR("Test_BGE_UN_S__Double", "Test_BLT_UN_S__Double", 1.0d, 0.0d);

        TestMethod_BR("Test_BGT_UN__Int32", "Test_BLE_UN__Int32", 1, 1);
        TestMethod_BR("Test_BGT_UN__Int32", "Test_BLE_UN__Int32", 0, 1);
        TestMethod_BR("Test_BGT_UN__Int32", "Test_BLE_UN__Int32", 1, 0);
        TestMethod_BR("Test_BGT_UN__Int64", "Test_BLE_UN__Int64", 1L, 1L);
        TestMethod_BR("Test_BGT_UN__Int64", "Test_BLE_UN__Int64", 0L, 1L);
        TestMethod_BR("Test_BGT_UN__Int64", "Test_BLE_UN__Int64", 1L, 0L);
        TestMethod_BR("Test_BGT_UN__IntPtr", "Test_BLE_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L));
			TestMethod_BR("Test_BGT_UN__IntPtr", "Test_BLE_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L));
			TestMethod_BR("Test_BGT_UN__IntPtr", "Test_BLE_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L));

			TestMethod_BR("Test_BGT_UN_S__Int32", "Test_BLE_UN_S__Int32", 1, 1);
        TestMethod_BR("Test_BGT_UN_S__Int32", "Test_BLE_UN_S__Int32", 0, 1);
        TestMethod_BR("Test_BGT_UN_S__Int32", "Test_BLE_UN_S__Int32", 1, 0);
        TestMethod_BR("Test_BGT_UN_S__Int64", "Test_BLE_UN_S__Int64", 1L, 1L);
        TestMethod_BR("Test_BGT_UN_S__Int64", "Test_BLE_UN_S__Int64", 0L, 1L);
        TestMethod_BR("Test_BGT_UN_S__Int64", "Test_BLE_UN_S__Int64", 1L, 0L);
        TestMethod_BR("Test_BGT_UN_S__IntPtr", "Test_BLE_UN_S__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L));
			TestMethod_BR("Test_BGT_UN_S__IntPtr", "Test_BLE_UN_S__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L));
			TestMethod_BR("Test_BGT_UN_S__IntPtr", "Test_BLE_UN_S__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L));
			TestMethod_BR("Test_BGT_UN_S__Single", "Test_BLE_UN_S__Single", 1.0f, 1.0f);
        TestMethod_BR("Test_BGT_UN_S__Single", "Test_BLE_UN_S__Single", 0.0f, 1.0f);
        TestMethod_BR("Test_BGT_UN_S__Single", "Test_BLE_UN_S__Single", 1.0f, 0.0f);
        TestMethod_BR("Test_BGT_UN_S__Double", "Test_BLE_UN_S__Double", 1.0d, 1.0d);
        TestMethod_BR("Test_BGT_UN_S__Double", "Test_BLE_UN_S__Double", 0.0d, 1.0d);
        TestMethod_BR("Test_BGT_UN_S__Double", "Test_BLE_UN_S__Double", 1.0d, 0.0d);*/

        [Fact] public void Test_LEAVE() { Test("Test_LEAVE"); } /* TestMethod("Test_LEAVE"); */
        [Fact] public void Test_LEAVE_S() { Test("Test_LEAVE_S"); } /* TestMethod("Test_LEAVE_S"); */
        [Fact] public void Test_LEAVE__0() { Test("Test_LEAVE__0"); } /* TestMethod("Test_LEAVE__0"); */
        [Fact] public void Test_LEAVE_S__0() { Test("Test_LEAVE_S__0"); } /* TestMethod("Test_LEAVE_S__0"); */

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void Test_SWITCH__0(int value) { Test("Test_SWITCH__0", value); }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void Test_SWITCH__1(int value) { Test("Test_SWITCH__1", value); }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void Test_SWITCH__2(int value) { Test("Test_SWITCH__2", value); }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void Test_SWITCH__3(int value) { Test("Test_SWITCH__3", value); }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void Test_SWITCH__4(int value) { Test("Test_SWITCH__4", value); }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void Test_SWITCH__5(int value) { Test("Test_SWITCH__5", value); }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void Test_SWITCH__6(int value) { Test("Test_SWITCH__6", value); }

        /*[Fact] public void Test_SWITCH__6_IntPtr() { Test("Test_SWITCH__6_IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_SWITCH__6_IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); *
        [Fact] public void Test_SWITCH__6_IntPtr() { Test("Test_SWITCH__6_IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethod("Test_SWITCH__6_IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); *
        [Fact] public void Test_SWITCH__6_IntPtr() { Test("Test_SWITCH__6_IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod("Test_SWITCH__6_IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); *
        [Fact] public void Test_SWITCH__6_IntPtr() { Test("Test_SWITCH__6_IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_SWITCH__6_IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); *
        [Fact] public void Test_SWITCH__6_IntPtr() { Test("Test_SWITCH__6_IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_SWITCH__6_IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); *
        [Fact] public void Test_SWITCH__6_IntPtr() { Test("Test_SWITCH__6_IntPtr", IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L)); } /* TestMethod("Test_SWITCH__6_IntPtr", IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L)); *
        [Fact] public void Test_SWITCH__6_IntPtr() { Test("Test_SWITCH__6_IntPtr", IntPtr.Size == 4 ? new IntPtr(3) : new IntPtr(3L)); } /* TestMethod("Test_SWITCH__6_IntPtr", IntPtr.Size == 4 ? new IntPtr(3) : new IntPtr(3L)); *
        [Fact] public void Test_SWITCH__6_IntPtr() { Test("Test_SWITCH__6_IntPtr", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod("Test_SWITCH__6_IntPtr", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); *
        [Fact] public void Test_SWITCH__6_IntPtr() { Test("Test_SWITCH__6_IntPtr", IntPtr.Size == 4 ? new IntPtr(5) : new IntPtr(5L)); } /* TestMethod("Test_SWITCH__6_IntPtr", IntPtr.Size == 4 ? new IntPtr(5) : new IntPtr(5L)); *
        [Fact] public void Test_SWITCH__6_IntPtr() { Test("Test_SWITCH__6_IntPtr", IntPtr.Size == 4 ? new IntPtr(6) : new IntPtr(6L)); } /* TestMethod("Test_SWITCH__6_IntPtr", IntPtr.Size == 4 ? new IntPtr(6) : new IntPtr(6L)); *

        [Fact] public void Test_SWITCH__6_Int64() { Test("Test_SWITCH__6_Int64", 0L); } /* TestMethod("Test_SWITCH__6_Int64", 0L); *
        [Fact] public void Test_SWITCH__6_Int64() { Test("Test_SWITCH__6_Int64", -1L); } /* TestMethod("Test_SWITCH__6_Int64", -1L); *
        [Fact] public void Test_SWITCH__6_Int64() { Test("Test_SWITCH__6_Int64", 1L); } /* TestMethod("Test_SWITCH__6_Int64", 1L); *
        [Fact] public void Test_SWITCH__6_Int64() { Test("Test_SWITCH__6_Int64", long.MinValue); } /* TestMethod("Test_SWITCH__6_Int64", long.MinValue); *
        [Fact] public void Test_SWITCH__6_Int64() { Test("Test_SWITCH__6_Int64", long.MaxValue); } /* TestMethod("Test_SWITCH__6_Int64", long.MaxValue); *
        [Fact] public void Test_SWITCH__6_Int64() { Test("Test_SWITCH__6_Int64", 2L); } /* TestMethod("Test_SWITCH__6_Int64", 2L); *
        [Fact] public void Test_SWITCH__6_Int64() { Test("Test_SWITCH__6_Int64", 3L); } /* TestMethod("Test_SWITCH__6_Int64", 3L); 
        [Fact] public void Test_SWITCH__6_Int64() { Test("Test_SWITCH__6_Int64", 4L); } /* TestMethod("Test_SWITCH__6_Int64", 4L); *
        [Fact] public void Test_SWITCH__6_Int64() { Test("Test_SWITCH__6_Int64", 5L); } /* TestMethod("Test_SWITCH__6_Int64", 5L); *
        [Fact] public void Test_SWITCH__6_Int64() { Test("Test_SWITCH__6_Int64", 6L); } /* TestMethod("Test_SWITCH__6_Int64", 6L); *

        [Fact] public void Test_SWITCH__6_Single() { Test("Test_SWITCH__6_Single", 0.0f); } /* TestMethod("Test_SWITCH__6_Single", 0.0f); *
        [Fact] public void Test_SWITCH__6_Single() { Test("Test_SWITCH__6_Single", -1.0f); } /* TestMethod("Test_SWITCH__6_Single", -1.0f); *
        [Fact] public void Test_SWITCH__6_Single() { Test("Test_SWITCH__6_Single", 1.0f); } /* TestMethod("Test_SWITCH__6_Single", 1.0f); *
        [Fact] public void Test_SWITCH__6_Single() { Test("Test_SWITCH__6_Single", float.MinValue); } /* TestMethod("Test_SWITCH__6_Single", float.MinValue); *
        [Fact] public void Test_SWITCH__6_Single() { Test("Test_SWITCH__6_Single", float.MaxValue); } /* TestMethod("Test_SWITCH__6_Single", float.MaxValue); *
        [Fact] public void Test_SWITCH__6_Single() { Test("Test_SWITCH__6_Single", float.NaN); } /* TestMethod("Test_SWITCH__6_Single", float.NaN); *
        [Fact] public void Test_SWITCH__6_Single() { Test("Test_SWITCH__6_Single", float.NegativeInfinity); } /* TestMethod("Test_SWITCH__6_Single", float.NegativeInfinity); *
        [Fact] public void Test_SWITCH__6_Single() { Test("Test_SWITCH__6_Single", float.PositiveInfinity); } /* TestMethod("Test_SWITCH__6_Single", float.PositiveInfinity); *
        [Fact] public void Test_SWITCH__6_Single() { Test("Test_SWITCH__6_Single", float.Epsilon); } /* TestMethod("Test_SWITCH__6_Single", float.Epsilon); *
        [Fact] public void Test_SWITCH__6_Single() { Test("Test_SWITCH__6_Single", 2.0f); } /* TestMethod("Test_SWITCH__6_Single", 2.0f); *
        [Fact] public void Test_SWITCH__6_Single() { Test("Test_SWITCH__6_Single", 3.0f); } /* TestMethod("Test_SWITCH__6_Single", 3.0f); 
        [Fact] public void Test_SWITCH__6_Single() { Test("Test_SWITCH__6_Single", 4.0f); } /* TestMethod("Test_SWITCH__6_Single", 4.0f); *
        [Fact] public void Test_SWITCH__6_Single() { Test("Test_SWITCH__6_Single", 5.0f); } /* TestMethod("Test_SWITCH__6_Single", 5.0f); *
        [Fact] public void Test_SWITCH__6_Single() { Test("Test_SWITCH__6_Single", 6.0f); } /* TestMethod("Test_SWITCH__6_Single", 6.0f); *

        [Fact] public void Test_SWITCH__6_Double() { Test("Test_SWITCH__6_Double", 0.0d); } /* TestMethod("Test_SWITCH__6_Double", 0.0d); *
        [Fact] public void Test_SWITCH__6_Double() { Test("Test_SWITCH__6_Double", -1.0d); } /* TestMethod("Test_SWITCH__6_Double", -1.0d); *
        [Fact] public void Test_SWITCH__6_Double() { Test("Test_SWITCH__6_Double", 1.0d); } /* TestMethod("Test_SWITCH__6_Double", 1.0d); *
        [Fact] public void Test_SWITCH__6_Double() { Test("Test_SWITCH__6_Double", double.MinValue); } /* TestMethod("Test_SWITCH__6_Double", double.MinValue); *
        [Fact] public void Test_SWITCH__6_Double() { Test("Test_SWITCH__6_Double", double.MaxValue); } /* TestMethod("Test_SWITCH__6_Double", double.MaxValue); *
        [Fact] public void Test_SWITCH__6_Double() { Test("Test_SWITCH__6_Double", double.NaN); } /* TestMethod("Test_SWITCH__6_Double", double.NaN); *
        [Fact] public void Test_SWITCH__6_Double() { Test("Test_SWITCH__6_Double", double.NegativeInfinity); } /* TestMethod("Test_SWITCH__6_Double", double.NegativeInfinity); *
        [Fact] public void Test_SWITCH__6_Double() { Test("Test_SWITCH__6_Double", double.PositiveInfinity); } /* TestMethod("Test_SWITCH__6_Double", double.PositiveInfinity); *
        [Fact] public void Test_SWITCH__6_Double() { Test("Test_SWITCH__6_Double", double.Epsilon); } /* TestMethod("Test_SWITCH__6_Double", double.Epsilon); *
        [Fact] public void Test_SWITCH__6_Double() { Test("Test_SWITCH__6_Double", 2.0d); } /* TestMethod("Test_SWITCH__6_Double", 2.0d); *
        [Fact] public void Test_SWITCH__6_Double() { Test("Test_SWITCH__6_Double", 3.0d); } /* TestMethod("Test_SWITCH__6_Double", 3.0d); *
        [Fact] public void Test_SWITCH__6_Double() { Test("Test_SWITCH__6_Double", 4.0d); } /* TestMethod("Test_SWITCH__6_Double", 4.0d); *
        [Fact] public void Test_SWITCH__6_Double() { Test("Test_SWITCH__6_Double", 5.0d); } /* TestMethod("Test_SWITCH__6_Double", 5.0d); *
        [Fact] public void Test_SWITCH__6_Double() { Test("Test_SWITCH__6_Double", 6.0d); } /* TestMethod("Test_SWITCH__6_Double", 6.0d); *

        [Fact] public void Test_POP() { Test("Test_POP"); } /* TestMethod("Test_POP"); */
        [Fact] public void Test_DUP() { Test("Test_DUP", "hello", "bye"); } /* TestMethod("Test_DUP", "hello", "bye"); */

        [Fact] public void Test_AND__Int32() { Test("Test_AND__Int32", 0x5AA51234, 0x3FF37591); } /* TestMethod("Test_AND__Int32", 0x5AA51234, 0x3FF37591); */
        [Fact] public void Test_AND__Int64() { Test("Test_AND__Int64", 0x5AA5123467306AB8L, 0x3FF375919AE00BB6L); } /* TestMethod("Test_AND__Int64", 0x5AA5123467306AB8L, 0x3FF375919AE00BB6L); */
        [Fact] public void Test_AND__IntPtr() { Test("Test_AND__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x5AA51234) : new IntPtr(0x5AA5123467306AB8L), IntPtr.Size == 4 ? new IntPtr(0x3FF37591) : new IntPtr(0x3FF375919AE00BB6L)); } /* TestMethod("Test_AND__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x5AA51234) : new IntPtr(0x5AA5123467306AB8L), IntPtr.Size == 4 ? new IntPtr(0x3FF37591) : new IntPtr(0x3FF375919AE00BB6L)); */

        [Fact] public void Test_OR__Int32() { Test("Test_OR__Int32", 0x5AA51234, 0x3FF37591); } /* TestMethod("Test_OR__Int32", 0x5AA51234, 0x3FF37591); */
        [Fact] public void Test_OR__Int64() { Test("Test_OR__Int64", 0x5AA5123467306AB8L, 0x3FF375919AE00BB6L); } /* TestMethod("Test_OR__Int64", 0x5AA5123467306AB8L, 0x3FF375919AE00BB6L); */
        [Fact] public void Test_OR__IntPtr() { Test("Test_OR__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x5AA51234) : new IntPtr(0x5AA5123467306AB8L), IntPtr.Size == 4 ? new IntPtr(0x3FF37591) : new IntPtr(0x3FF375919AE00BB6L)); } /* TestMethod("Test_OR__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x5AA51234) : new IntPtr(0x5AA5123467306AB8L), IntPtr.Size == 4 ? new IntPtr(0x3FF37591) : new IntPtr(0x3FF375919AE00BB6L)); */

        [Fact] public void Test_XOR__Int32() { Test("Test_XOR__Int32", 0x5AA51234, 0x3FF37591); } /* TestMethod("Test_XOR__Int32", 0x5AA51234, 0x3FF37591); */
        [Fact] public void Test_XOR__Int64() { Test("Test_XOR__Int64", 0x5AA5123467306AB8L, 0x3FF375919AE00BB6L); } /* TestMethod("Test_XOR__Int64", 0x5AA5123467306AB8L, 0x3FF375919AE00BB6L); */
        [Fact] public void Test_XOR__IntPtr() { Test("Test_XOR__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x5AA51234) : new IntPtr(0x5AA5123467306AB8L), IntPtr.Size == 4 ? new IntPtr(0x3FF37591) : new IntPtr(0x3FF375919AE00BB6L)); } /* TestMethod("Test_XOR__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x5AA51234) : new IntPtr(0x5AA5123467306AB8L), IntPtr.Size == 4 ? new IntPtr(0x3FF37591) : new IntPtr(0x3FF375919AE00BB6L)); */

        [Fact] public void Test_SHL__Int32() { Test("Test_SHL__Int32", -0x5AA51234, 5); } /* TestMethod("Test_SHL__Int32", -0x5AA51234, 5); */
        [Fact] public void Test_SHL__Int64() { Test("Test_SHL__Int64", -0x5AA5123467306AB8L, 5); } /* TestMethod("Test_SHL__Int64", -0x5AA5123467306AB8L, 5); */
        [Fact] public void Test_SHL__IntPtr() { Test("Test_SHL__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x5AA51234) : new IntPtr(-0x5AA5123467306AB8L), 5); } /* TestMethod("Test_SHL__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x5AA51234) : new IntPtr(-0x5AA5123467306AB8L), 5); */

        [Fact] public void Test_SHR__Int32() { Test("Test_SHR__Int32", -0x5AA51234, 5); } /* TestMethod("Test_SHR__Int32", -0x5AA51234, 5); */
        [Fact] public void Test_SHR__Int64() { Test("Test_SHR__Int64", -0x5AA5123467306AB8L, 5); } /* TestMethod("Test_SHR__Int64", -0x5AA5123467306AB8L, 5); */
        [Fact] public void Test_SHR__IntPtr() { Test("Test_SHR__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x5AA51234) : new IntPtr(-0x5AA5123467306AB8L), 5); } /* TestMethod("Test_SHR__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x5AA51234) : new IntPtr(-0x5AA5123467306AB8L), 5); */

        [Fact] public void Test_SHR_UN__Int32() { Test("Test_SHR_UN__Int32", -0x5AA51234, 5); } /* TestMethod("Test_SHR_UN__Int32", -0x5AA51234, 5); */
        [Fact] public void Test_SHR_UN__Int64() { Test("Test_SHR_UN__Int64", -0x5AA5123467306AB8L, 5); } /* TestMethod("Test_SHR_UN__Int64", -0x5AA5123467306AB8L, 5); */
        [Fact] public void Test_SHR_UN__IntPtr() { Test("Test_SHR_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x5AA51234) : new IntPtr(-0x5AA5123467306AB8L), 5); } /* TestMethod("Test_SHR_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-0x5AA51234) : new IntPtr(-0x5AA5123467306AB8L), 5); */

        [Fact] public void Test_NOT__Int32() { Test("Test_NOT__Int32", 0x12345678); } /* TestMethod("Test_NOT__Int32", 0x12345678); */
        [Fact] public void Test_NOT__Int64() { Test("Test_NOT__Int64", 0x123456789ABCDEF0L); } /* TestMethod("Test_NOT__Int64", 0x123456789ABCDEF0L); */
        [Fact] public void Test_NOT__IntPtr() { Test("Test_NOT__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethod("Test_NOT__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */

        [Fact] public void Test_NEG__Int32() { Test("Test_NEG__Int32", 0x12345678); } /* TestMethod("Test_NEG__Int32", 0x12345678); */
        [Fact] public void Test_NEG__Int64() { Test("Test_NEG__Int64", 0x123456789ABCDEF0L); } /* TestMethod("Test_NEG__Int64", 0x123456789ABCDEF0L); */
        [Fact] public void Test_NEG__IntPtr() { Test("Test_NEG__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); } /* TestMethod("Test_NEG__IntPtr", IntPtr.Size == 4 ? new IntPtr(0x12345678) : new IntPtr(0x123456789ABCDEF0L)); */

        public void Dispose()
        {
            //AppDomain.Unload(_appDomain);
        }
        //[Fact] public void Test_NEG__Single() { Test("Test_NEG__Single", 123456.789f); } // TestMethod("Test_NEG__Single", 123456.789f); *
        //[Fact] public void Test_NEG__Single() { Test("Test_NEG__Single", float.MinValue); } /* TestMethod("Test_NEG__Single", float.MinValue); */
        //[Fact] public void Test_NEG__Single() { Test("Test_NEG__Single", float.MaxValue); } /* TestMethod("Test_NEG__Single", float.MaxValue); */
        //[Fact] public void Test_NEG__Single() { Test("Test_NEG__Single", float.NaN); } /* TestMethod("Test_NEG__Single", float.NaN); */
        //[Fact] public void Test_NEG__Single() { Test("Test_NEG__Single", float.NegativeInfinity); } /* TestMethod("Test_NEG__Single", float.NegativeInfinity); */
        //[Fact] public void Test_NEG__Single() { Test("Test_NEG__Single", float.PositiveInfinity); } /* TestMethod("Test_NEG__Single", float.PositiveInfinity); */
        //[Fact] public void Test_NEG__Single() { Test("Test_NEG__Single", float.Epsilon); } /* TestMethod("Test_NEG__Single", float.Epsilon); */
        //[Fact] public void Test_NEG__Double() { Test("Test_NEG__Double", 12345678910111213.14151617d); } /* TestMethod("Test_NEG__Double", 12345678910111213.14151617d); */
        //[Fact] public void Test_NEG__Double() { Test("Test_NEG__Double", double.MinValue); } /* TestMethod("Test_NEG__Double", double.MinValue); */
        //[Fact] public void Test_NEG__Double() { Test("Test_NEG__Double", double.MaxValue); } /* TestMethod("Test_NEG__Double", double.MaxValue); */
        //[Fact] public void Test_NEG__Double() { Test("Test_NEG__Double", double.NaN); } /* TestMethod("Test_NEG__Double", double.NaN); */
        //[Fact] public void Test_NEG__Double() { Test("Test_NEG__Double", double.NegativeInfinity); } /* TestMethod("Test_NEG__Double", double.NegativeInfinity); */
        //[Fact] public void Test_NEG__Double() { Test("Test_NEG__Double", double.PositiveInfinity); } /* TestMethod("Test_NEG__Double", double.PositiveInfinity); */
        //[Fact] public void Test_NEG__Double() { Test("Test_NEG__Double", double.Epsilon); } /* TestMethod("Test_NEG__Double", double.Epsilon); */

        //[Fact] public void Test_ADD__Int32() { Test("Test_ADD__Int32", int.MinValue, int.MaxValue); } /* TestMethod2("Test_ADD__Int32", int.MinValue, int.MaxValue); */
        //[Fact] public void Test_ADD__Int32() { Test("Test_ADD__Int32", -5, 4); } /* TestMethod2("Test_ADD__Int32", -5, 4); */
        //[Fact] public void Test_ADD__Int64() { Test("Test_ADD__Int64", long.MinValue, long.MaxValue); } /* TestMethod2("Test_ADD__Int64", long.MinValue, long.MaxValue); */
        //[Fact] public void Test_ADD__Int64() { Test("Test_ADD__Int64", -5L, 4L); } /* TestMethod2("Test_ADD__Int64", -5L, 4L); */
        //[Fact] public void Test_ADD__IntPtr() { Test("Test_ADD__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod2("Test_ADD__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_ADD__IntPtr() { Test("Test_ADD__IntPtr", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod2("Test_ADD__IntPtr", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_ADD__Single() { Test("Test_ADD__Single", float.MinValue, float.MaxValue); } /* TestMethod2("Test_ADD__Single", float.MinValue, float.MaxValue); */
        //[Fact] public void Test_ADD__Single() { Test("Test_ADD__Single", -5.0f, 4.0f); } /* TestMethod2("Test_ADD__Single", -5.0f, 4.0f); */
        //[Fact] public void Test_ADD__Double() { Test("Test_ADD__Double", double.MinValue, double.MaxValue); } /* TestMethod2("Test_ADD__Double", double.MinValue, double.MaxValue); */
        //[Fact] public void Test_ADD__Double() { Test("Test_ADD__Double", -5.0d, 4.0d); } /* TestMethod2("Test_ADD__Double", -5.0d, 4.0d); */
        //[Fact] public void Test_ADD__Int32_IntPtr() { Test("Test_ADD__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_ADD__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_ADD__Int32_IntPtr() { Test("Test_ADD__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_ADD__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_ADD__Int32_IntPtr() { Test("Test_ADD__Int32_IntPtr", -5, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod("Test_ADD__Int32_IntPtr", -5, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_ADD__Int32_IntPtr() { Test("Test_ADD__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L)); } /* TestMethod("Test_ADD__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L)); */
        //[Fact] public void Test_ADD__IntPtr_Int32() { Test("Test_ADD__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); } /* TestMethod("Test_ADD__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); */
        //[Fact] public void Test_ADD__IntPtr_Int32() { Test("Test_ADD__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); } /* TestMethod("Test_ADD__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); */
        //[Fact] public void Test_ADD__IntPtr_Int32() { Test("Test_ADD__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), 4); } /* TestMethod("Test_ADD__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), 4); */
        //[Fact] public void Test_ADD__IntPtr_Int32() { Test("Test_ADD__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -5); } /* TestMethod("Test_ADD__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -5); */
        //[Fact] public void Test_ADD__Int32() { Test("Test_ADD__Int32", int.MinValue, -1); } /* TestMethod2("Test_ADD__Int32", int.MinValue, -1); */
        //[Fact] public void Test_ADD__Int32() { Test("Test_ADD__Int32", int.MaxValue, 1); } /* TestMethod2("Test_ADD__Int32", int.MaxValue, 1); */
        //[Fact] public void Test_ADD__Int64() { Test("Test_ADD__Int64", long.MinValue, -1L); } /* TestMethod2("Test_ADD__Int64", long.MinValue, -1L); */
        //[Fact] public void Test_ADD__Int64() { Test("Test_ADD__Int64", long.MaxValue, 1L); } /* TestMethod2("Test_ADD__Int64", long.MaxValue, 1L); */
        //[Fact] public void Test_ADD__IntPtr() { Test("Test_ADD__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethod2("Test_ADD__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */
        //[Fact] public void Test_ADD__IntPtr() { Test("Test_ADD__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod2("Test_ADD__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */

        //[Fact] public void Test_ADD_OVF__Int32() { Test("Test_ADD_OVF__Int32", int.MinValue, int.MaxValue); } /* TestMethod2("Test_ADD_OVF__Int32", int.MinValue, int.MaxValue); */
        //[Fact] public void Test_ADD_OVF__Int32() { Test("Test_ADD_OVF__Int32", -5, 4); } /* TestMethod2("Test_ADD_OVF__Int32", -5, 4); */
        //[Fact] public void Test_ADD_OVF__Int64() { Test("Test_ADD_OVF__Int64", long.MinValue, long.MaxValue); } /* TestMethod2("Test_ADD_OVF__Int64", long.MinValue, long.MaxValue); */
        //[Fact] public void Test_ADD_OVF__Int64() { Test("Test_ADD_OVF__Int64", -5L, 4L); } /* TestMethod2("Test_ADD_OVF__Int64", -5L, 4L); */
        //[Fact] public void Test_ADD_OVF__IntPtr() { Test("Test_ADD_OVF__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod2("Test_ADD_OVF__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_ADD_OVF__IntPtr() { Test("Test_ADD_OVF__IntPtr", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod2("Test_ADD_OVF__IntPtr", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_ADD_OVF__Single() { Test("Test_ADD_OVF__Single", float.MinValue, float.MaxValue); } /* TestMethod2("Test_ADD_OVF__Single", float.MinValue, float.MaxValue); */
        //[Fact] public void Test_ADD_OVF__Single() { Test("Test_ADD_OVF__Single", -5.0f, 4.0f); } /* TestMethod2("Test_ADD_OVF__Single", -5.0f, 4.0f); */
        //[Fact] public void Test_ADD_OVF__Double() { Test("Test_ADD_OVF__Double", double.MinValue, double.MaxValue); } /* TestMethod2("Test_ADD_OVF__Double", double.MinValue, double.MaxValue); */
        //[Fact] public void Test_ADD_OVF__Double() { Test("Test_ADD_OVF__Double", -5.0d, 4.0d); } /* TestMethod2("Test_ADD_OVF__Double", -5.0d, 4.0d); */
        //[Fact] public void Test_ADD_OVF__Int32_IntPtr() { Test("Test_ADD_OVF__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_ADD_OVF__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_ADD_OVF__Int32_IntPtr() { Test("Test_ADD_OVF__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_ADD_OVF__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_ADD_OVF__Int32_IntPtr() { Test("Test_ADD_OVF__Int32_IntPtr", -5, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod("Test_ADD_OVF__Int32_IntPtr", -5, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_ADD_OVF__Int32_IntPtr() { Test("Test_ADD_OVF__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L)); } /* TestMethod("Test_ADD_OVF__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L)); */
        //[Fact] public void Test_ADD_OVF__IntPtr_Int32() { Test("Test_ADD_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); } /* TestMethod("Test_ADD_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); */
        //[Fact] public void Test_ADD_OVF__IntPtr_Int32() { Test("Test_ADD_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); } /* TestMethod("Test_ADD_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); */
        //[Fact] public void Test_ADD_OVF__IntPtr_Int32() { Test("Test_ADD_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), 4); } /* TestMethod("Test_ADD_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), 4); */
        //[Fact] public void Test_ADD_OVF__IntPtr_Int32() { Test("Test_ADD_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -5); } /* TestMethod("Test_ADD_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -5); */
        //                                                                                                                                                     // TestMethodEX2("Test_ADD_OVF__Int32", int.MinValue, -1);
        //                                                                                                                                                     // TestMethodEX2("Test_ADD_OVF__Int32", int.MaxValue, 1);
        //                                                                                                                                                     // TestMethodEX2("Test_ADD_OVF__Int64", long.MinValue, -1L);
        //                                                                                                                                                     // TestMethodEX2("Test_ADD_OVF__Int64", long.MaxValue, 1L);
        //                                                                                                                                                     // TestMethodEX2("Test_ADD_OVF__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L));
        //                                                                                                                                                     // TestMethodEX2("Test_ADD_OVF__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L));

        //[Fact] public void Test_ADD_OVF_UN__Int32() { Test("Test_ADD_OVF_UN__Int32", int.MinValue, int.MaxValue); } /* TestMethod2("Test_ADD_OVF_UN__Int32", int.MinValue, int.MaxValue); */
        //[Fact] public void Test_ADD_OVF_UN__Int32() { Test("Test_ADD_OVF_UN__Int32", -5, 4); } /* TestMethod2("Test_ADD_OVF_UN__Int32", -5, 4); */
        //[Fact] public void Test_ADD_OVF_UN__Int64() { Test("Test_ADD_OVF_UN__Int64", long.MinValue, long.MaxValue); } /* TestMethod2("Test_ADD_OVF_UN__Int64", long.MinValue, long.MaxValue); */
        //[Fact] public void Test_ADD_OVF_UN__Int64() { Test("Test_ADD_OVF_UN__Int64", -5L, 4L); } /* TestMethod2("Test_ADD_OVF_UN__Int64", -5L, 4L); */
        //[Fact] public void Test_ADD_OVF_UN__IntPtr() { Test("Test_ADD_OVF_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod2("Test_ADD_OVF_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_ADD_OVF_UN__IntPtr() { Test("Test_ADD_OVF_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod2("Test_ADD_OVF_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_ADD_OVF_UN__Int32_IntPtr() { Test("Test_ADD_OVF_UN__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_ADD_OVF_UN__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_ADD_OVF_UN__Int32_IntPtr() { Test("Test_ADD_OVF_UN__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_ADD_OVF_UN__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_ADD_OVF_UN__Int32_IntPtr() { Test("Test_ADD_OVF_UN__Int32_IntPtr", -5, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod("Test_ADD_OVF_UN__Int32_IntPtr", -5, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_ADD_OVF_UN__Int32_IntPtr() { Test("Test_ADD_OVF_UN__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L)); } /* TestMethod("Test_ADD_OVF_UN__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L)); */
        //[Fact] public void Test_ADD_OVF_UN__IntPtr_Int32() { Test("Test_ADD_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); } /* TestMethod("Test_ADD_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); */
        //[Fact] public void Test_ADD_OVF_UN__IntPtr_Int32() { Test("Test_ADD_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); } /* TestMethod("Test_ADD_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); */
        //[Fact] public void Test_ADD_OVF_UN__IntPtr_Int32() { Test("Test_ADD_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), 4); } /* TestMethod("Test_ADD_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), 4); */
        //[Fact] public void Test_ADD_OVF_UN__IntPtr_Int32() { Test("Test_ADD_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -5); } /* TestMethod("Test_ADD_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -5); */
        //                                                                                                                                                           // TestMethodEX2("Test_ADD_OVF_UN__Int32", 1, -1);
        //                                                                                                                                                           // TestMethodEX2("Test_ADD_OVF_UN__Int64", 1L, -1L);
        //                                                                                                                                                           // TestMethodEX2("Test_ADD_OVF_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L));

        //[Fact] public void Test_SUB__Int32() { Test("Test_SUB__Int32", int.MinValue, int.MaxValue); } /* TestMethod2("Test_SUB__Int32", int.MinValue, int.MaxValue); */
        //[Fact] public void Test_SUB__Int32() { Test("Test_SUB__Int32", -5, 4); } /* TestMethod2("Test_SUB__Int32", -5, 4); */
        //[Fact] public void Test_SUB__Int64() { Test("Test_SUB__Int64", long.MinValue, long.MaxValue); } /* TestMethod2("Test_SUB__Int64", long.MinValue, long.MaxValue); */
        //[Fact] public void Test_SUB__Int64() { Test("Test_SUB__Int64", -5L, 4L); } /* TestMethod2("Test_SUB__Int64", -5L, 4L); */
        //[Fact] public void Test_SUB__IntPtr() { Test("Test_SUB__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod2("Test_SUB__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_SUB__IntPtr() { Test("Test_SUB__IntPtr", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod2("Test_SUB__IntPtr", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_SUB__Single() { Test("Test_SUB__Single", float.MinValue, float.MaxValue); } /* TestMethod2("Test_SUB__Single", float.MinValue, float.MaxValue); */
        //[Fact] public void Test_SUB__Single() { Test("Test_SUB__Single", -5.0f, 4.0f); } /* TestMethod2("Test_SUB__Single", -5.0f, 4.0f); */
        //[Fact] public void Test_SUB__Double() { Test("Test_SUB__Double", double.MinValue, double.MaxValue); } /* TestMethod2("Test_SUB__Double", double.MinValue, double.MaxValue); */
        //[Fact] public void Test_SUB__Double() { Test("Test_SUB__Double", -5.0d, 4.0d); } /* TestMethod2("Test_SUB__Double", -5.0d, 4.0d); */
        //[Fact] public void Test_SUB__Int32_IntPtr() { Test("Test_SUB__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_SUB__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_SUB__Int32_IntPtr() { Test("Test_SUB__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_SUB__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_SUB__Int32_IntPtr() { Test("Test_SUB__Int32_IntPtr", -5, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod("Test_SUB__Int32_IntPtr", -5, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_SUB__Int32_IntPtr() { Test("Test_SUB__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L)); } /* TestMethod("Test_SUB__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L)); */
        //[Fact] public void Test_SUB__IntPtr_Int32() { Test("Test_SUB__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); } /* TestMethod("Test_SUB__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); */
        //[Fact] public void Test_SUB__IntPtr_Int32() { Test("Test_SUB__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); } /* TestMethod("Test_SUB__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); */
        //[Fact] public void Test_SUB__IntPtr_Int32() { Test("Test_SUB__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), 4); } /* TestMethod("Test_SUB__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), 4); */
        //[Fact] public void Test_SUB__IntPtr_Int32() { Test("Test_SUB__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -5); } /* TestMethod("Test_SUB__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -5); */
        //[Fact] public void Test_SUB__Int32() { Test("Test_SUB__Int32", int.MinValue, 1); } /* TestMethod2("Test_SUB__Int32", int.MinValue, 1); */
        //[Fact] public void Test_SUB__Int32() { Test("Test_SUB__Int32", int.MaxValue, -1); } /* TestMethod2("Test_SUB__Int32", int.MaxValue, -1); */
        //[Fact] public void Test_SUB__Int64() { Test("Test_SUB__Int64", long.MinValue, 1L); } /* TestMethod2("Test_SUB__Int64", long.MinValue, 1L); */
        //[Fact] public void Test_SUB__Int64() { Test("Test_SUB__Int64", long.MaxValue, -1L); } /* TestMethod2("Test_SUB__Int64", long.MaxValue, -1L); */
        //[Fact] public void Test_SUB__IntPtr() { Test("Test_SUB__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); } /* TestMethod2("Test_SUB__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L)); */
        //[Fact] public void Test_SUB__IntPtr() { Test("Test_SUB__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); } /* TestMethod2("Test_SUB__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L)); */

        //// TestMethodEX2("Test_SUB_OVF__Int32", int.MinValue, int.MaxValue);
        //[Fact] public void Test_SUB_OVF__Int32() { Test("Test_SUB_OVF__Int32", -5, 4); } /* TestMethod2("Test_SUB_OVF__Int32", -5, 4); */
        //                                                                                       // TestMethodEX2("Test_SUB_OVF__Int64", long.MinValue, long.MaxValue);
        //[Fact] public void Test_SUB_OVF__Int64() { Test("Test_SUB_OVF__Int64", -5L, 4L); } /* TestMethod2("Test_SUB_OVF__Int64", -5L, 4L); */
        //                                                                                         // TestMethodEX2("Test_SUB_OVF__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue));
        //[Fact] public void Test_SUB_OVF__IntPtr() { Test("Test_SUB_OVF__IntPtr", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod2("Test_SUB_OVF__IntPtr", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_SUB_OVF__Single() { Test("Test_SUB_OVF__Single", float.MinValue, float.MaxValue); } /* TestMethod2("Test_SUB_OVF__Single", float.MinValue, float.MaxValue); */
        //[Fact] public void Test_SUB_OVF__Single() { Test("Test_SUB_OVF__Single", -5.0f, 4.0f); } /* TestMethod2("Test_SUB_OVF__Single", -5.0f, 4.0f); */
        //[Fact] public void Test_SUB_OVF__Double() { Test("Test_SUB_OVF__Double", double.MinValue, double.MaxValue); } /* TestMethod2("Test_SUB_OVF__Double", double.MinValue, double.MaxValue); */
        //[Fact] public void Test_SUB_OVF__Double() { Test("Test_SUB_OVF__Double", -5.0d, 4.0d); } /* TestMethod2("Test_SUB_OVF__Double", -5.0d, 4.0d); */
        //[Fact] public void Test_SUB_OVF__Int32_IntPtr() { Test("Test_SUB_OVF__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethodEX("Test_SUB_OVF__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_SUB_OVF__Int32_IntPtr() { Test("Test_SUB_OVF__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethodEX("Test_SUB_OVF__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_SUB_OVF__Int32_IntPtr() { Test("Test_SUB_OVF__Int32_IntPtr", -5, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod("Test_SUB_OVF__Int32_IntPtr", -5, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_SUB_OVF__Int32_IntPtr() { Test("Test_SUB_OVF__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L)); } /* TestMethod("Test_SUB_OVF__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L)); */
        //[Fact] public void Test_SUB_OVF__IntPtr_Int32() { Test("Test_SUB_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); } /* TestMethodEX("Test_SUB_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); */
        //[Fact] public void Test_SUB_OVF__IntPtr_Int32() { Test("Test_SUB_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); } /* TestMethodEX("Test_SUB_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); */
        //[Fact] public void Test_SUB_OVF__IntPtr_Int32() { Test("Test_SUB_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), 4); } /* TestMethod("Test_SUB_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), 4); */
        //[Fact] public void Test_SUB_OVF__IntPtr_Int32() { Test("Test_SUB_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -5); } /* TestMethod("Test_SUB_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -5); */
        //                                                                                                                                                     // TestMethodEX2("Test_SUB_OVF__Int32", int.MinValue, 1);
        //                                                                                                                                                     // TestMethodEX2("Test_SUB_OVF__Int32", int.MaxValue, -1);
        //                                                                                                                                                     // TestMethodEX2("Test_SUB_OVF__Int64", long.MinValue, 1L);
        //                                                                                                                                                     // TestMethodEX2("Test_SUB_OVF__Int64", long.MaxValue, -1L);
        //                                                                                                                                                     // TestMethodEX2("Test_SUB_OVF__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L));
        //                                                                                                                                                     // TestMethodEX2("Test_SUB_OVF__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L));

        //// TestMethodEX2("Test_SUB_OVF_UN__Int32", int.MinValue, int.MaxValue);
        //// TestMethodEX2("Test_SUB_OVF_UN__Int32", -5, 4);
        //// TestMethodEX2("Test_SUB_OVF_UN__Int64", long.MinValue, long.MaxValue);
        //// TestMethodEX2("Test_SUB_OVF_UN__Int64", -5L, 4L);
        //// TestMethodEX2("Test_SUB_OVF_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue));
        //// TestMethodEX2("Test_SUB_OVF_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L));
        //[Fact] public void Test_SUB_OVF_UN__Int32_IntPtr() { Test("Test_SUB_OVF_UN__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethodEX("Test_SUB_OVF_UN__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_SUB_OVF_UN__Int32_IntPtr() { Test("Test_SUB_OVF_UN__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethodEX("Test_SUB_OVF_UN__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_SUB_OVF_UN__Int32_IntPtr() { Test("Test_SUB_OVF_UN__Int32_IntPtr", -5, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod("Test_SUB_OVF_UN__Int32_IntPtr", -5, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_SUB_OVF_UN__Int32_IntPtr() { Test("Test_SUB_OVF_UN__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L)); } /* TestMethodEX("Test_SUB_OVF_UN__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L)); */
        //[Fact] public void Test_SUB_OVF_UN__IntPtr_Int32() { Test("Test_SUB_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); } /* TestMethod("Test_SUB_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); */
        //[Fact] public void Test_SUB_OVF_UN__IntPtr_Int32() { Test("Test_SUB_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); } /* TestMethodEX("Test_SUB_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); */
        //[Fact] public void Test_SUB_OVF_UN__IntPtr_Int32() { Test("Test_SUB_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), 4); } /* TestMethod("Test_SUB_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), 4); */
        //[Fact] public void Test_SUB_OVF_UN__IntPtr_Int32() { Test("Test_SUB_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -5); } /* TestMethodEX("Test_SUB_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -5); */
        //                                                                                                                                                           // TestMethodEX2("Test_SUB_OVF_UN__Int32", 0, 1);
        //                                                                                                                                                           // TestMethodEX2("Test_SUB_OVF_UN__Int64", 0L, 1L);
        //                                                                                                                                                           // TestMethodEX2("Test_SUB_OVF_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L));

        //[Fact] public void Test_MUL__Int32() { Test("Test_MUL__Int32", int.MinValue, int.MaxValue); } /* TestMethod2("Test_MUL__Int32", int.MinValue, int.MaxValue); */
        //[Fact] public void Test_MUL__Int32() { Test("Test_MUL__Int32", -5, 4); } /* TestMethod2("Test_MUL__Int32", -5, 4); */
        //[Fact] public void Test_MUL__Int64() { Test("Test_MUL__Int64", long.MinValue, long.MaxValue); } /* TestMethod2("Test_MUL__Int64", long.MinValue, long.MaxValue); */
        //[Fact] public void Test_MUL__Int64() { Test("Test_MUL__Int64", -5L, 4L); } /* TestMethod2("Test_MUL__Int64", -5L, 4L); */
        //[Fact] public void Test_MUL__IntPtr() { Test("Test_MUL__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod2("Test_MUL__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_MUL__IntPtr() { Test("Test_MUL__IntPtr", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod2("Test_MUL__IntPtr", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_MUL__Single() { Test("Test_MUL__Single", float.MinValue, float.MaxValue); } /* TestMethod2("Test_MUL__Single", float.MinValue, float.MaxValue); */
        //[Fact] public void Test_MUL__Single() { Test("Test_MUL__Single", -5.0f, 4.0f); } /* TestMethod2("Test_MUL__Single", -5.0f, 4.0f); */
        //[Fact] public void Test_MUL__Double() { Test("Test_MUL__Double", double.MinValue, double.MaxValue); } /* TestMethod2("Test_MUL__Double", double.MinValue, double.MaxValue); */
        //[Fact] public void Test_MUL__Double() { Test("Test_MUL__Double", -5.0d, 4.0d); } /* TestMethod2("Test_MUL__Double", -5.0d, 4.0d); */
        //[Fact] public void Test_MUL__Int32_IntPtr() { Test("Test_MUL__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_MUL__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_MUL__Int32_IntPtr() { Test("Test_MUL__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_MUL__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_MUL__Int32_IntPtr() { Test("Test_MUL__Int32_IntPtr", -5, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod("Test_MUL__Int32_IntPtr", -5, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_MUL__Int32_IntPtr() { Test("Test_MUL__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L)); } /* TestMethod("Test_MUL__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L)); */
        //[Fact] public void Test_MUL__IntPtr_Int32() { Test("Test_MUL__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); } /* TestMethod("Test_MUL__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); */
        //[Fact] public void Test_MUL__IntPtr_Int32() { Test("Test_MUL__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); } /* TestMethod("Test_MUL__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); */
        //[Fact] public void Test_MUL__IntPtr_Int32() { Test("Test_MUL__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), 4); } /* TestMethod("Test_MUL__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), 4); */
        //[Fact] public void Test_MUL__IntPtr_Int32() { Test("Test_MUL__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -5); } /* TestMethod("Test_MUL__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -5); */
        //[Fact] public void Test_MUL__Int32() { Test("Test_MUL__Int32", int.MinValue, 2); } /* TestMethod2("Test_MUL__Int32", int.MinValue, 2); */
        //[Fact] public void Test_MUL__Int32() { Test("Test_MUL__Int32", int.MaxValue, 2); } /* TestMethod2("Test_MUL__Int32", int.MaxValue, 2); */
        //[Fact] public void Test_MUL__Int64() { Test("Test_MUL__Int64", long.MinValue, 2L); } /* TestMethod2("Test_MUL__Int64", long.MinValue, 2L); */
        //[Fact] public void Test_MUL__Int64() { Test("Test_MUL__Int64", long.MaxValue, 2L); } /* TestMethod2("Test_MUL__Int64", long.MaxValue, 2L); */
        //[Fact] public void Test_MUL__IntPtr() { Test("Test_MUL__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L)); } /* TestMethod2("Test_MUL__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L)); */
        //[Fact] public void Test_MUL__IntPtr() { Test("Test_MUL__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L)); } /* TestMethod2("Test_MUL__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L)); */

        //// TestMethodEX2("Test_MUL_OVF__Int32", int.MinValue, int.MaxValue);
        //[Fact] public void Test_MUL_OVF__Int32() { Test("Test_MUL_OVF__Int32", -5, 4); } /* TestMethod2("Test_MUL_OVF__Int32", -5, 4); */
        //                                                                                       // TestMethodEX2("Test_MUL_OVF__Int64", long.MinValue, long.MaxValue);
        //[Fact] public void Test_MUL_OVF__Int64() { Test("Test_MUL_OVF__Int64", -5L, 4L); } /* TestMethod2("Test_MUL_OVF__Int64", -5L, 4L); */
        //                                                                                         // TestMethodEX2("Test_MUL_OVF__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue));
        //[Fact] public void Test_MUL_OVF__IntPtr() { Test("Test_MUL_OVF__IntPtr", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod2("Test_MUL_OVF__IntPtr", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //                                                                                                                                                                                          // TestMethodEX2("Test_MUL_OVF__Single", float.MinValue, float.MaxValue);
        //[Fact] public void Test_MUL_OVF__Single() { Test("Test_MUL_OVF__Single", -5.0f, 4.0f); } /* TestMethod2("Test_MUL_OVF__Single", -5.0f, 4.0f); */
        //                                                                                               // TestMethodEX2("Test_MUL_OVF__Double", double.MinValue, double.MaxValue);
        //[Fact] public void Test_MUL_OVF__Double() { Test("Test_MUL_OVF__Double", -5.0d, 4.0d); } /* TestMethod2("Test_MUL_OVF__Double", -5.0d, 4.0d); */
        //[Fact] public void Test_MUL_OVF__Int32_IntPtr() { Test("Test_MUL_OVF__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethodEX("Test_MUL_OVF__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_MUL_OVF__Int32_IntPtr() { Test("Test_MUL_OVF__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethodEX("Test_MUL_OVF__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_MUL_OVF__Int32_IntPtr() { Test("Test_MUL_OVF__Int32_IntPtr", -5, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod("Test_MUL_OVF__Int32_IntPtr", -5, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_MUL_OVF__Int32_IntPtr() { Test("Test_MUL_OVF__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L)); } /* TestMethod("Test_MUL_OVF__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L)); */
        //[Fact] public void Test_MUL_OVF__IntPtr_Int32() { Test("Test_MUL_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); } /* TestMethodEX("Test_MUL_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); */
        //[Fact] public void Test_MUL_OVF__IntPtr_Int32() { Test("Test_MUL_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); } /* TestMethodEX("Test_MUL_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); */
        //[Fact] public void Test_MUL_OVF__IntPtr_Int32() { Test("Test_MUL_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), 4); } /* TestMethod("Test_MUL_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), 4); */
        //[Fact] public void Test_MUL_OVF__IntPtr_Int32() { Test("Test_MUL_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -5); } /* TestMethod("Test_MUL_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -5); */
        //                                                                                                                                                     // TestMethodEX2("Test_MUL_OVF__Int32", int.MinValue, 2);
        //                                                                                                                                                     // TestMethodEX2("Test_MUL_OVF__Int32", int.MaxValue, 2);
        //                                                                                                                                                     // TestMethodEX2("Test_MUL_OVF__Int64", long.MinValue, 2L);
        //                                                                                                                                                     // TestMethodEX2("Test_MUL_OVF__Int64", long.MaxValue, 2L);
        //                                                                                                                                                     // TestMethodEX2("Test_MUL_OVF__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L));
        //                                                                                                                                                     // TestMethodEX2("Test_MUL_OVF__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L));

        //// TestMethodEX2("Test_MUL_OVF_UN__Int32", int.MinValue, int.MaxValue);
        //// TestMethodEX2("Test_MUL_OVF_UN__Int32", -5, 4);
        //[Fact] public void Test_MUL_OVF_UN__Int32() { Test("Test_MUL_OVF_UN__Int32", 5, 4); } /* TestMethod2("Test_MUL_OVF_UN__Int32", 5, 4); */
        //                                                                                            // TestMethodEX2("Test_MUL_OVF_UN__Int64", long.MinValue, long.MaxValue);
        //                                                                                            // TestMethodEX2("Test_MUL_OVF_UN__Int64", -5L, 4L);
        //[Fact] public void Test_MUL_OVF_UN__Int64() { Test("Test_MUL_OVF_UN__Int64", 5L, 4L); } /* TestMethod2("Test_MUL_OVF_UN__Int64", 5L, 4L); */
        //                                                                                              // TestMethodEX2("Test_MUL_OVF_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue));
        //                                                                                              // TestMethodEX2("Test_MUL_OVF_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L));
        //[Fact] public void Test_MUL_OVF_UN__IntPtr() { Test("Test_MUL_OVF_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(5) : new IntPtr(5L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod2("Test_MUL_OVF_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(5) : new IntPtr(5L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_MUL_OVF_UN__Int32_IntPtr() { Test("Test_MUL_OVF_UN__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethodEX("Test_MUL_OVF_UN__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_MUL_OVF_UN__Int32_IntPtr() { Test("Test_MUL_OVF_UN__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethodEX("Test_MUL_OVF_UN__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_MUL_OVF_UN__Int32_IntPtr() { Test("Test_MUL_OVF_UN__Int32_IntPtr", -5, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethodEX("Test_MUL_OVF_UN__Int32_IntPtr", -5, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_MUL_OVF_UN__Int32_IntPtr() { Test("Test_MUL_OVF_UN__Int32_IntPtr", 5, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod("Test_MUL_OVF_UN__Int32_IntPtr", 5, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_MUL_OVF_UN__Int32_IntPtr() { Test("Test_MUL_OVF_UN__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L)); } /* TestMethodEX("Test_MUL_OVF_UN__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L)); */
        //[Fact] public void Test_MUL_OVF_UN__Int32_IntPtr() { Test("Test_MUL_OVF_UN__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(5) : new IntPtr(5L)); } /* TestMethod("Test_MUL_OVF_UN__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(5) : new IntPtr(5L)); */
        //[Fact] public void Test_MUL_OVF_UN__IntPtr_Int32() { Test("Test_MUL_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); } /* TestMethodEX("Test_MUL_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); */
        //[Fact] public void Test_MUL_OVF_UN__IntPtr_Int32() { Test("Test_MUL_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); } /* TestMethodEX("Test_MUL_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); */
        //[Fact] public void Test_MUL_OVF_UN__IntPtr_Int32() { Test("Test_MUL_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), 4); } /* TestMethodEX("Test_MUL_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-5) : new IntPtr(-5L), 4); */
        //[Fact] public void Test_MUL_OVF_UN__IntPtr_Int32() { Test("Test_MUL_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(5) : new IntPtr(5L), 4); } /* TestMethod("Test_MUL_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(5) : new IntPtr(5L), 4); */
        //[Fact] public void Test_MUL_OVF_UN__IntPtr_Int32() { Test("Test_MUL_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -5); } /* TestMethodEX("Test_MUL_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -5); */
        //[Fact] public void Test_MUL_OVF_UN__IntPtr_Int32() { Test("Test_MUL_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), 5); } /* TestMethod("Test_MUL_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), 5); */
        //                                                                                                                                                          // TestMethodEX2("Test_MUL_OVF_UN__Int32", -1, 2);
        //                                                                                                                                                          // TestMethodEX2("Test_MUL_OVF_UN__Int64", -1L, 2L);
        //                                                                                                                                                          // TestMethodEX2("Test_MUL_OVF_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1) : new IntPtr(-1L), IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L));

        //[Fact] public void Test_DIV__Int32() { Test("Test_DIV__Int32", int.MinValue, int.MaxValue); } /* TestMethod2("Test_DIV__Int32", int.MinValue, int.MaxValue); */
        //[Fact] public void Test_DIV__Int32() { Test("Test_DIV__Int32", -1234, 4); } /* TestMethod2("Test_DIV__Int32", -1234, 4); */
        //[Fact] public void Test_DIV__Int32() { Test("Test_DIV__Int32", 1, 0); } /* TestMethod2("Test_DIV__Int32", 1, 0); */
        //[Fact] public void Test_DIV__Int64() { Test("Test_DIV__Int64", long.MinValue, long.MaxValue); } /* TestMethod2("Test_DIV__Int64", long.MinValue, long.MaxValue); */
        //[Fact] public void Test_DIV__Int64() { Test("Test_DIV__Int64", -1234L, 4L); } /* TestMethod2("Test_DIV__Int64", -1234L, 4L); */
        //[Fact] public void Test_DIV__Int64() { Test("Test_DIV__Int64", 1L, 0L); } /* TestMethod2("Test_DIV__Int64", 1L, 0L); */
        //[Fact] public void Test_DIV__IntPtr() { Test("Test_DIV__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod2("Test_DIV__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_DIV__IntPtr() { Test("Test_DIV__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod2("Test_DIV__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_DIV__IntPtr() { Test("Test_DIV__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod2("Test_DIV__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_DIV__Single() { Test("Test_DIV__Single", float.MinValue, float.MaxValue); } /* TestMethod2("Test_DIV__Single", float.MinValue, float.MaxValue); */
        //[Fact] public void Test_DIV__Single() { Test("Test_DIV__Single", -1234.0f, 4.0f); } /* TestMethod2("Test_DIV__Single", -1234.0f, 4.0f); */
        //[Fact] public void Test_DIV__Single() { Test("Test_DIV__Single", 1.0f, 0.0f); } /* TestMethod2("Test_DIV__Single", 1.0f, 0.0f); */
        //[Fact] public void Test_DIV__Double() { Test("Test_DIV__Double", double.MinValue, double.MaxValue); } /* TestMethod2("Test_DIV__Double", double.MinValue, double.MaxValue); */
        //[Fact] public void Test_DIV__Double() { Test("Test_DIV__Double", -1234.0d, 4.0d); } /* TestMethod2("Test_DIV__Double", -1234.0d, 4.0d); */
        //[Fact] public void Test_DIV__Double() { Test("Test_DIV__Double", 1.0d, 0.0d); } /* TestMethod2("Test_DIV__Double", 1.0d, 0.0d); */
        //[Fact] public void Test_DIV__Int32_IntPtr() { Test("Test_DIV__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_DIV__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_DIV__Int32_IntPtr() { Test("Test_DIV__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_DIV__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_DIV__Int32_IntPtr() { Test("Test_DIV__Int32_IntPtr", -1234, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod("Test_DIV__Int32_IntPtr", -1234, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_DIV__Int32_IntPtr() { Test("Test_DIV__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L)); } /* TestMethod("Test_DIV__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L)); */
        //[Fact] public void Test_DIV__Int32_IntPtr() { Test("Test_DIV__Int32_IntPtr", 1, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_DIV__Int32_IntPtr", 1, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_DIV__IntPtr_Int32() { Test("Test_DIV__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); } /* TestMethod("Test_DIV__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); */
        //[Fact] public void Test_DIV__IntPtr_Int32() { Test("Test_DIV__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); } /* TestMethod("Test_DIV__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); */
        //[Fact] public void Test_DIV__IntPtr_Int32() { Test("Test_DIV__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L), 4); } /* TestMethod("Test_DIV__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L), 4); */
        //[Fact] public void Test_DIV__IntPtr_Int32() { Test("Test_DIV__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -1234); } /* TestMethod("Test_DIV__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -1234); */
        //[Fact] public void Test_DIV__IntPtr_Int32() { Test("Test_DIV__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), 0); } /* TestMethod("Test_DIV__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), 0); */
        //[Fact] public void Test_DIV__Int32() { Test("Test_DIV__Int32", int.MinValue, 2); } /* TestMethod2("Test_DIV__Int32", int.MinValue, 2); */
        //[Fact] public void Test_DIV__Int32() { Test("Test_DIV__Int32", int.MaxValue, 2); } /* TestMethod2("Test_DIV__Int32", int.MaxValue, 2); */
        //[Fact] public void Test_DIV__Int64() { Test("Test_DIV__Int64", long.MinValue, 2L); } /* TestMethod2("Test_DIV__Int64", long.MinValue, 2L); */
        //[Fact] public void Test_DIV__Int64() { Test("Test_DIV__Int64", long.MaxValue, 2L); } /* TestMethod2("Test_DIV__Int64", long.MaxValue, 2L); */
        //[Fact] public void Test_DIV__IntPtr() { Test("Test_DIV__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L)); } /* TestMethod2("Test_DIV__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L)); */
        //[Fact] public void Test_DIV__IntPtr() { Test("Test_DIV__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L)); } /* TestMethod2("Test_DIV__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L)); */

        //[Fact] public void Test_REM__Int32() { Test("Test_REM__Int32", int.MinValue, int.MaxValue); } /* TestMethod2("Test_REM__Int32", int.MinValue, int.MaxValue); */
        //[Fact] public void Test_REM__Int32() { Test("Test_REM__Int32", -1234, 4); } /* TestMethod2("Test_REM__Int32", -1234, 4); */
        //[Fact] public void Test_REM__Int32() { Test("Test_REM__Int32", 1, 0); } /* TestMethod2("Test_REM__Int32", 1, 0); */
        //[Fact] public void Test_REM__Int64() { Test("Test_REM__Int64", long.MinValue, long.MaxValue); } /* TestMethod2("Test_REM__Int64", long.MinValue, long.MaxValue); */
        //[Fact] public void Test_REM__Int64() { Test("Test_REM__Int64", -1234L, 4L); } /* TestMethod2("Test_REM__Int64", -1234L, 4L); */
        //[Fact] public void Test_REM__Int64() { Test("Test_REM__Int64", 1L, 0L); } /* TestMethod2("Test_REM__Int64", 1L, 0L); */
        //[Fact] public void Test_REM__IntPtr() { Test("Test_REM__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod2("Test_REM__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_REM__IntPtr() { Test("Test_REM__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod2("Test_REM__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_REM__IntPtr() { Test("Test_REM__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod2("Test_REM__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_REM__Single() { Test("Test_REM__Single", float.MinValue, float.MaxValue); } /* TestMethod2("Test_REM__Single", float.MinValue, float.MaxValue); */
        //[Fact] public void Test_REM__Single() { Test("Test_REM__Single", -1234.0f, 4.0f); } /* TestMethod2("Test_REM__Single", -1234.0f, 4.0f); */
        //[Fact] public void Test_REM__Single() { Test("Test_REM__Single", 1.0f, 0.0f); } /* TestMethod2("Test_REM__Single", 1.0f, 0.0f); */
        //[Fact] public void Test_REM__Double() { Test("Test_REM__Double", double.MinValue, double.MaxValue); } /* TestMethod2("Test_REM__Double", double.MinValue, double.MaxValue); */
        //[Fact] public void Test_REM__Double() { Test("Test_REM__Double", -1234.0d, 4.0d); } /* TestMethod2("Test_REM__Double", -1234.0d, 4.0d); */
        //[Fact] public void Test_REM__Double() { Test("Test_REM__Double", 1.0d, 0.0d); } /* TestMethod2("Test_REM__Double", 1.0d, 0.0d); */
        //[Fact] public void Test_REM__Int32_IntPtr() { Test("Test_REM__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_REM__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_REM__Int32_IntPtr() { Test("Test_REM__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_REM__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_REM__Int32_IntPtr() { Test("Test_REM__Int32_IntPtr", -1234, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod("Test_REM__Int32_IntPtr", -1234, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_REM__Int32_IntPtr() { Test("Test_REM__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L)); } /* TestMethod("Test_REM__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L)); */
        //[Fact] public void Test_REM__Int32_IntPtr() { Test("Test_REM__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_REM__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_REM__IntPtr_Int32() { Test("Test_REM__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); } /* TestMethod("Test_REM__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); */
        //[Fact] public void Test_REM__IntPtr_Int32() { Test("Test_REM__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); } /* TestMethod("Test_REM__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); */
        //[Fact] public void Test_REM__IntPtr_Int32() { Test("Test_REM__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L), 4); } /* TestMethod("Test_REM__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L), 4); */
        //[Fact] public void Test_REM__IntPtr_Int32() { Test("Test_REM__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -1234); } /* TestMethod("Test_REM__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -1234); */
        //[Fact] public void Test_REM__IntPtr_Int32() { Test("Test_REM__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), 0); } /* TestMethod("Test_REM__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), 0); */
        //[Fact] public void Test_REM__Int32() { Test("Test_REM__Int32", int.MinValue, 2); } /* TestMethod2("Test_REM__Int32", int.MinValue, 2); */
        //[Fact] public void Test_REM__Int32() { Test("Test_REM__Int32", int.MaxValue, 2); } /* TestMethod2("Test_REM__Int32", int.MaxValue, 2); */
        //[Fact] public void Test_REM__Int64() { Test("Test_REM__Int64", long.MinValue, 2L); } /* TestMethod2("Test_REM__Int64", long.MinValue, 2L); */
        //[Fact] public void Test_REM__Int64() { Test("Test_REM__Int64", long.MaxValue, 2L); } /* TestMethod2("Test_REM__Int64", long.MaxValue, 2L); */
        //[Fact] public void Test_REM__IntPtr() { Test("Test_REM__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L)); } /* TestMethod2("Test_REM__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L)); */
        //[Fact] public void Test_REM__IntPtr() { Test("Test_REM__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L)); } /* TestMethod2("Test_REM__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L)); */

        //[Fact] public void Test_DIV_UN__Int32() { Test("Test_DIV_UN__Int32", int.MinValue, int.MaxValue); } /* TestMethod2("Test_DIV_UN__Int32", int.MinValue, int.MaxValue); */
        //[Fact] public void Test_DIV_UN__Int32() { Test("Test_DIV_UN__Int32", -1234, 4); } /* TestMethod2("Test_DIV_UN__Int32", -1234, 4); */
        //[Fact] public void Test_DIV_UN__Int32() { Test("Test_DIV_UN__Int32", 1, 0); } /* TestMethod2("Test_DIV_UN__Int32", 1, 0); */
        //[Fact] public void Test_DIV_UN__Int64() { Test("Test_DIV_UN__Int64", long.MinValue, long.MaxValue); } /* TestMethod2("Test_DIV_UN__Int64", long.MinValue, long.MaxValue); */
        //[Fact] public void Test_DIV_UN__Int64() { Test("Test_DIV_UN__Int64", -1234L, 4L); } /* TestMethod2("Test_DIV_UN__Int64", -1234L, 4L); */
        //[Fact] public void Test_DIV_UN__Int64() { Test("Test_DIV_UN__Int64", 1L, 0L); } /* TestMethod2("Test_DIV_UN__Int64", 1L, 0L); */
        //[Fact] public void Test_DIV_UN__IntPtr() { Test("Test_DIV_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod2("Test_DIV_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_DIV_UN__IntPtr() { Test("Test_DIV_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod2("Test_DIV_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_DIV_UN__IntPtr() { Test("Test_DIV_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod2("Test_DIV_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_DIV_UN__Int32_IntPtr() { Test("Test_DIV_UN__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_DIV_UN__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_DIV_UN__Int32_IntPtr() { Test("Test_DIV_UN__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_DIV_UN__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_DIV_UN__Int32_IntPtr() { Test("Test_DIV_UN__Int32_IntPtr", -1234, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod("Test_DIV_UN__Int32_IntPtr", -1234, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_DIV_UN__Int32_IntPtr() { Test("Test_DIV_UN__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L)); } /* TestMethod("Test_DIV_UN__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L)); */
        //[Fact] public void Test_DIV_UN__Int32_IntPtr() { Test("Test_DIV_UN__Int32_IntPtr", 1, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_DIV_UN__Int32_IntPtr", 1, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_DIV_UN__IntPtr_Int32() { Test("Test_DIV_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); } /* TestMethod("Test_DIV_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); */
        //[Fact] public void Test_DIV_UN__IntPtr_Int32() { Test("Test_DIV_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); } /* TestMethod("Test_DIV_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); */
        //[Fact] public void Test_DIV_UN__IntPtr_Int32() { Test("Test_DIV_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L), 4); } /* TestMethod("Test_DIV_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L), 4); */
        //[Fact] public void Test_DIV_UN__IntPtr_Int32() { Test("Test_DIV_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -1234); } /* TestMethod("Test_DIV_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -1234); */
        //[Fact] public void Test_DIV_UN__IntPtr_Int32() { Test("Test_DIV_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), 0); } /* TestMethod("Test_DIV_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), 0); */
        //[Fact] public void Test_DIV_UN__Int32() { Test("Test_DIV_UN__Int32", int.MinValue, 2); } /* TestMethod2("Test_DIV_UN__Int32", int.MinValue, 2); */
        //[Fact] public void Test_DIV_UN__Int32() { Test("Test_DIV_UN__Int32", int.MaxValue, 2); } /* TestMethod2("Test_DIV_UN__Int32", int.MaxValue, 2); */
        //[Fact] public void Test_DIV_UN__Int64() { Test("Test_DIV_UN__Int64", long.MinValue, 2L); } /* TestMethod2("Test_DIV_UN__Int64", long.MinValue, 2L); */
        //[Fact] public void Test_DIV_UN__Int64() { Test("Test_DIV_UN__Int64", long.MaxValue, 2L); } /* TestMethod2("Test_DIV_UN__Int64", long.MaxValue, 2L); */
        //[Fact] public void Test_DIV_UN__IntPtr() { Test("Test_DIV_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L)); } /* TestMethod2("Test_DIV_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L)); */
        //[Fact] public void Test_DIV_UN__IntPtr() { Test("Test_DIV_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L)); } /* TestMethod2("Test_DIV_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L)); */

        //[Fact] public void Test_REM_UN__Int32() { Test("Test_REM_UN__Int32", int.MinValue, int.MaxValue); } /* TestMethod2("Test_REM_UN__Int32", int.MinValue, int.MaxValue); */
        //[Fact] public void Test_REM_UN__Int32() { Test("Test_REM_UN__Int32", -1234, 4); } /* TestMethod2("Test_REM_UN__Int32", -1234, 4); */
        //[Fact] public void Test_REM_UN__Int32() { Test("Test_REM_UN__Int32", 1, 0); } /* TestMethod2("Test_REM_UN__Int32", 1, 0); */
        //[Fact] public void Test_REM_UN__Int64() { Test("Test_REM_UN__Int64", long.MinValue, long.MaxValue); } /* TestMethod2("Test_REM_UN__Int64", long.MinValue, long.MaxValue); */
        //[Fact] public void Test_REM_UN__Int64() { Test("Test_REM_UN__Int64", -1234L, 4L); } /* TestMethod2("Test_REM_UN__Int64", -1234L, 4L); */
        //[Fact] public void Test_REM_UN__Int64() { Test("Test_REM_UN__Int64", 1L, 0L); } /* TestMethod2("Test_REM_UN__Int64", 1L, 0L); */
        //[Fact] public void Test_REM_UN__IntPtr() { Test("Test_REM_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod2("Test_REM_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_REM_UN__IntPtr() { Test("Test_REM_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod2("Test_REM_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L), IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_REM_UN__IntPtr() { Test("Test_REM_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod2("Test_REM_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_REM_UN__Int32_IntPtr() { Test("Test_REM_UN__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); } /* TestMethod("Test_REM_UN__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue)); */
        //[Fact] public void Test_REM_UN__Int32_IntPtr() { Test("Test_REM_UN__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); } /* TestMethod("Test_REM_UN__Int32_IntPtr", int.MaxValue, IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue)); */
        //[Fact] public void Test_REM_UN__Int32_IntPtr() { Test("Test_REM_UN__Int32_IntPtr", -1234, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); } /* TestMethod("Test_REM_UN__Int32_IntPtr", -1234, IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L)); */
        //[Fact] public void Test_REM_UN__Int32_IntPtr() { Test("Test_REM_UN__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L)); } /* TestMethod("Test_REM_UN__Int32_IntPtr", 4, IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L)); */
        //[Fact] public void Test_REM_UN__Int32_IntPtr() { Test("Test_REM_UN__Int32_IntPtr", 1, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_REM_UN__Int32_IntPtr", 1, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_REM_UN__IntPtr_Int32() { Test("Test_REM_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); } /* TestMethod("Test_REM_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), int.MaxValue); */
        //[Fact] public void Test_REM_UN__IntPtr_Int32() { Test("Test_REM_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); } /* TestMethod("Test_REM_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), int.MinValue); */
        //[Fact] public void Test_REM_UN__IntPtr_Int32() { Test("Test_REM_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L), 4); } /* TestMethod("Test_REM_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(-1234) : new IntPtr(-1234L), 4); */
        //[Fact] public void Test_REM_UN__IntPtr_Int32() { Test("Test_REM_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -1234); } /* TestMethod("Test_REM_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(4) : new IntPtr(4L), -1234); */
        //[Fact] public void Test_REM_UN__IntPtr_Int32() { Test("Test_REM_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), 0); } /* TestMethod("Test_REM_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(1) : new IntPtr(1L), 0); */
        //[Fact] public void Test_REM_UN__Int32() { Test("Test_REM_UN__Int32", int.MinValue, 2); } /* TestMethod2("Test_REM_UN__Int32", int.MinValue, 2); */
        //[Fact] public void Test_REM_UN__Int32() { Test("Test_REM_UN__Int32", int.MaxValue, 2); } /* TestMethod2("Test_REM_UN__Int32", int.MaxValue, 2); */
        //[Fact] public void Test_REM_UN__Int64() { Test("Test_REM_UN__Int64", long.MinValue, 2L); } /* TestMethod2("Test_REM_UN__Int64", long.MinValue, 2L); */
        //[Fact] public void Test_REM_UN__Int64() { Test("Test_REM_UN__Int64", long.MaxValue, 2L); } /* TestMethod2("Test_REM_UN__Int64", long.MaxValue, 2L); */
        //[Fact] public void Test_REM_UN__IntPtr() { Test("Test_REM_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L)); } /* TestMethod2("Test_REM_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MinValue) : new IntPtr(long.MinValue), IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L)); */
        //[Fact] public void Test_REM_UN__IntPtr() { Test("Test_REM_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L)); } /* TestMethod2("Test_REM_UN__IntPtr", IntPtr.Size == 4 ? new IntPtr(int.MaxValue) : new IntPtr(long.MaxValue), IntPtr.Size == 4 ? new IntPtr(2) : new IntPtr(2L)); */

        //[Fact] public void Test_BOX_UNBOX() { Test("Test_BOX_UNBOX"); } /* TestMethod("Test_BOX_UNBOX"); */
        //[Fact] public void Test_CASTCLASS() { Test("Test_CASTCLASS"); } /* TestMethod("Test_CASTCLASS"); */
        //[Fact] public void Test_ISINST() { Test("Test_ISINST"); } /* TestMethod("Test_ISINST"); */
        //[Fact] public void Test_SIZEOF() { Test("Test_SIZEOF", IntPtr.Size); } /* TestMethod("Test_SIZEOF", IntPtr.Size); */

        //[Fact] public void Test_NEWARR__Int32() { Test("Test_NEWARR__Int32", 0); } /* TestMethod("Test_NEWARR__Int32", 0); */
        //[Fact] public void Test_NEWARR__Int32() { Test("Test_NEWARR__Int32", 123); } /* TestMethod("Test_NEWARR__Int32", 123); */
        //[Fact] public void Test_NEWARR__IntPtr() { Test("Test_NEWARR__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_NEWARR__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_NEWARR__IntPtr() { Test("Test_NEWARR__IntPtr", IntPtr.Size == 4 ? new IntPtr(123) : new IntPtr(123L)); } /* TestMethod("Test_NEWARR__IntPtr", IntPtr.Size == 4 ? new IntPtr(123) : new IntPtr(123L)); */

        //[Fact] public void Test_LDLEN__Int32() { Test("Test_LDLEN__Int32", 0); } /* TestMethod("Test_LDLEN__Int32", 0); */
        //[Fact] public void Test_LDLEN__Int32() { Test("Test_LDLEN__Int32", 123); } /* TestMethod("Test_LDLEN__Int32", 123); */
        //[Fact] public void Test_LDLEN__IntPtr() { Test("Test_LDLEN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_LDLEN__IntPtr", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_LDLEN__IntPtr() { Test("Test_LDLEN__IntPtr", IntPtr.Size == 4 ? new IntPtr(123) : new IntPtr(123L)); } /* TestMethod("Test_LDLEN__IntPtr", IntPtr.Size == 4 ? new IntPtr(123) : new IntPtr(123L)); */

        //[Fact] public void Test_LDELEM_I__STELEM_I() { Test("Test_LDELEM_I__STELEM_I"); } /* TestMethod("Test_LDELEM_I__STELEM_I"); */
        //[Fact] public void Test_LDELEM_I1__STELEM_I1() { Test("Test_LDELEM_I1__STELEM_I1"); } /* TestMethod("Test_LDELEM_I1__STELEM_I1"); */
        //[Fact] public void Test_LDELEM_I2__STELEM_I2() { Test("Test_LDELEM_I2__STELEM_I2"); } /* TestMethod("Test_LDELEM_I2__STELEM_I2"); */
        //[Fact] public void Test_LDELEM_I4__STELEM_I4() { Test("Test_LDELEM_I4__STELEM_I4"); } /* TestMethod("Test_LDELEM_I4__STELEM_I4"); */
        //[Fact] public void Test_LDELEM_I8__STELEM_I8() { Test("Test_LDELEM_I8__STELEM_I8"); } /* TestMethod("Test_LDELEM_I8__STELEM_I8"); */
        //[Fact] public void Test_LDELEM_U1__STELEM_I1() { Test("Test_LDELEM_U1__STELEM_I1"); } /* TestMethod("Test_LDELEM_U1__STELEM_I1"); */
        //[Fact] public void Test_LDELEM_U2__STELEM_I2() { Test("Test_LDELEM_U2__STELEM_I2"); } /* TestMethod("Test_LDELEM_U2__STELEM_I2"); */
        //[Fact] public void Test_LDELEM_U4__STELEM_I4() { Test("Test_LDELEM_U4__STELEM_I4"); } /* TestMethod("Test_LDELEM_U4__STELEM_I4"); */
        //[Fact] public void Test_LDELEM_R4__STELEM_R4() { Test("Test_LDELEM_R4__STELEM_R4"); } /* TestMethod("Test_LDELEM_R4__STELEM_R4"); */
        //[Fact] public void Test_LDELEM_R8__STELEM_R8() { Test("Test_LDELEM_R8__STELEM_R8"); } /* TestMethod("Test_LDELEM_R8__STELEM_R8"); */
        //[Fact] public void Test_LDELEM_REF__STELEM_REF() { Test("Test_LDELEM_REF__STELEM_REF"); } /* TestMethod("Test_LDELEM_REF__STELEM_REF"); */
        //[Fact] public void Test_LDELEM__STELEM() { Test("Test_LDELEM__STELEM"); } /* TestMethod("Test_LDELEM__STELEM"); */

        //[Fact] public void Test_LDARGA() { Test("Test_LDARGA", 123, "hello"); } /* TestMethod("Test_LDARGA", 123, "hello"); */
        //[Fact] public void Test_LDARGA_S() { Test("Test_LDARGA_S", 123, "hello"); } /* TestMethod("Test_LDARGA_S", 123, "hello"); */
        //[Fact] public void Test_LDLOCA() { Test("Test_LDLOCA"); } /* TestMethod("Test_LDLOCA"); */
        //[Fact] public void Test_LDLOCA_S() { Test("Test_LDLOCA_S"); } /* TestMethod("Test_LDLOCA_S"); */
        //[Fact] public void Test_LDELEMA() { Test("Test_LDELEMA"); } /* TestMethod("Test_LDELEMA"); */
        //[Fact] public void Test_LDIND_STIND() { Test("Test_LDIND_STIND"); } /* TestMethod("Test_LDIND_STIND"); */

        //[Fact] public void Test_READONLY() { Test("Test_READONLY"); } /* TestMethod("Test_READONLY"); */
        //[Fact] public void Test_UNALIGNED() { Test("Test_UNALIGNED"); } /* TestMethod("Test_UNALIGNED"); */
        //[Fact] public void Test_VOLATILE() { Test("Test_VOLATILE"); } /* TestMethod("Test_VOLATILE"); */
        //[Fact] public void Test_UNALIGNED_VOLATILE() { Test("Test_UNALIGNED_VOLATILE"); } /* TestMethod("Test_UNALIGNED_VOLATILE"); */

        //[Fact] public void Test_INITOBJ() { Test("Test_INITOBJ"); } /* TestMethod("Test_INITOBJ"); */
        //[Fact] public void Test_LDFLD_STFLD_LDFLDA() { Test("Test_LDFLD_STFLD_LDFLDA"); } /* TestMethod("Test_LDFLD_STFLD_LDFLDA"); */
        //[Fact] public void Test_LDSFLD_STSFLD_LDSFLDA() { Test("Test_LDSFLD_STSFLD_LDSFLDA"); } /* TestMethod("Test_LDSFLD_STSFLD_LDSFLDA"); */
        //[Fact] public void Test_LDOBJ_STOBJ() { Test("Test_LDOBJ_STOBJ"); } /* TestMethod("Test_LDOBJ_STOBJ"); */
        //[Fact] public void Test_CPOBJ() { Test("Test_CPOBJ"); } /* TestMethod("Test_CPOBJ"); */

        //[Fact] public void Test_NEWOBJ__Struct() { Test("Test_NEWOBJ__Struct"); } /* TestMethod("Test_NEWOBJ__Struct"); */
        //[Fact] public void Test_NEWOBJ__Class() { Test("Test_NEWOBJ__Class"); } /* TestMethod("Test_NEWOBJ__Class"); */

        //[Fact] public void Test_CALL__Static_Struct() { Test("Test_CALL__Static_Struct"); } /* TestMethod("Test_CALL__Static_Struct"); */
        //[Fact] public void Test_CALL__Static_Class() { Test("Test_CALL__Static_Class"); } /* TestMethod("Test_CALL__Static_Class"); */
        //[Fact] public void Test_CALL__Instance_Struct() { Test("Test_CALL__Instance_Struct"); } /* TestMethod("Test_CALL__Instance_Struct"); */
        //[Fact] public void Test_CALL__Instance_Class() { Test("Test_CALL__Instance_Class"); } /* TestMethod("Test_CALL__Instance_Class"); */

        //[Fact] public void Test_LDTOKEN__Field() { Test("Test_LDTOKEN__Field"); } /* TestMethod("Test_LDTOKEN__Field"); */
        //[Fact] public void Test_LDTOKEN__Method() { Test("Test_LDTOKEN__Method"); } /* TestMethod("Test_LDTOKEN__Method"); */
        //[Fact] public void Test_LDTOKEN__Type() { Test("Test_LDTOKEN__Type"); } /* TestMethod("Test_LDTOKEN__Type"); */

        //[Fact] public void Test_LOCALLOC() { Test("Test_LOCALLOC"); } /* TestMethod("Test_LOCALLOC"); */

        //[Fact] public void Test_CKFINITE__Single() { Test("Test_CKFINITE__Single", 0.0f); } /* TestMethod("Test_CKFINITE__Single", 0.0f); */
        //[Fact] public void Test_CKFINITE__Single() { Test("Test_CKFINITE__Single", -0.0f); } /* TestMethod("Test_CKFINITE__Single", -0.0f); */
        //[Fact] public void Test_CKFINITE__Single() { Test("Test_CKFINITE__Single", float.MaxValue); } /* TestMethod("Test_CKFINITE__Single", float.MaxValue); */
        //[Fact] public void Test_CKFINITE__Single() { Test("Test_CKFINITE__Single", float.NaN); } /* TestMethodEX("Test_CKFINITE__Single", float.NaN); */
        //[Fact] public void Test_CKFINITE__Single() { Test("Test_CKFINITE__Single", float.NegativeInfinity); } /* TestMethodEX("Test_CKFINITE__Single", float.NegativeInfinity); */
        //[Fact] public void Test_CKFINITE__Single() { Test("Test_CKFINITE__Single", float.PositiveInfinity); } /* TestMethodEX("Test_CKFINITE__Single", float.PositiveInfinity); */
        //[Fact] public void Test_CKFINITE__Single() { Test("Test_CKFINITE__Single", float.Epsilon); } /* TestMethod("Test_CKFINITE__Single", float.Epsilon); */

        //[Fact] public void Test_CKFINITE__Double() { Test("Test_CKFINITE__Double", 0.0d); } /* TestMethod("Test_CKFINITE__Double", 0.0d); */
        //[Fact] public void Test_CKFINITE__Double() { Test("Test_CKFINITE__Double", -0.0d); } /* TestMethod("Test_CKFINITE__Double", -0.0d); */
        //[Fact] public void Test_CKFINITE__Double() { Test("Test_CKFINITE__Double", double.MaxValue); } /* TestMethod("Test_CKFINITE__Double", double.MaxValue); */
        //[Fact] public void Test_CKFINITE__Double() { Test("Test_CKFINITE__Double", double.NaN); } /* TestMethodEX("Test_CKFINITE__Double", double.NaN); */
        //[Fact] public void Test_CKFINITE__Double() { Test("Test_CKFINITE__Double", double.NegativeInfinity); } /* TestMethodEX("Test_CKFINITE__Double", double.NegativeInfinity); */
        //[Fact] public void Test_CKFINITE__Double() { Test("Test_CKFINITE__Double", double.PositiveInfinity); } /* TestMethodEX("Test_CKFINITE__Double", double.PositiveInfinity); */
        //[Fact] public void Test_CKFINITE__Double() { Test("Test_CKFINITE__Double", double.Epsilon); } /* TestMethod("Test_CKFINITE__Double", double.Epsilon); */

        //[Fact] public void Test_INITBLK() { Test("Test_INITBLK"); } /* TestMethod("Test_INITBLK"); */

        //[Fact] public void Test_AND__IntPtr_Int32() { Test("Test_AND__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); } /* TestMethod("Test_AND__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); */
        //[Fact] public void Test_AND__Int32_IntPtr() { Test("Test_AND__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_AND__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_OR__IntPtr_Int32() { Test("Test_OR__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); } /* TestMethod("Test_OR__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); */
        //[Fact] public void Test_OR__Int32_IntPtr() { Test("Test_OR__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_OR__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_XOR__IntPtr_Int32() { Test("Test_XOR__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); } /* TestMethod("Test_XOR__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); */
        //[Fact] public void Test_XOR__Int32_IntPtr() { Test("Test_XOR__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_XOR__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_ADD__IntPtr_Int32() { Test("Test_ADD__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); } /* TestMethod("Test_ADD__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); */
        //[Fact] public void Test_ADD__Int32_IntPtr() { Test("Test_ADD__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_ADD__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_ADD_OVF__IntPtr_Int32() { Test("Test_ADD_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); } /* TestMethod("Test_ADD_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); */
        //[Fact] public void Test_ADD_OVF__Int32_IntPtr() { Test("Test_ADD_OVF__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_ADD_OVF__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_ADD_OVF_UN__IntPtr_Int32() { Test("Test_ADD_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); } /* TestMethod("Test_ADD_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); */
        //[Fact] public void Test_ADD_OVF_UN__Int32_IntPtr() { Test("Test_ADD_OVF_UN__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_ADD_OVF_UN__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_SUB__IntPtr_Int32() { Test("Test_SUB__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); } /* TestMethod("Test_SUB__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); */
        //[Fact] public void Test_SUB__Int32_IntPtr() { Test("Test_SUB__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_SUB__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_SUB_OVF__IntPtr_Int32() { Test("Test_SUB_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); } /* TestMethod("Test_SUB_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); */
        //[Fact] public void Test_SUB_OVF__Int32_IntPtr() { Test("Test_SUB_OVF__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_SUB_OVF__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_SUB_OVF_UN__IntPtr_Int32() { Test("Test_SUB_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); } /* TestMethod("Test_SUB_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); */
        //[Fact] public void Test_SUB_OVF_UN__Int32_IntPtr() { Test("Test_SUB_OVF_UN__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_SUB_OVF_UN__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_MUL__IntPtr_Int32() { Test("Test_MUL__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); } /* TestMethod("Test_MUL__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); */
        //[Fact] public void Test_MUL__Int32_IntPtr() { Test("Test_MUL__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_MUL__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_MUL_OVF__IntPtr_Int32() { Test("Test_MUL_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); } /* TestMethod("Test_MUL_OVF__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); */
        //[Fact] public void Test_MUL_OVF__Int32_IntPtr() { Test("Test_MUL_OVF__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_MUL_OVF__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_MUL_OVF_UN__IntPtr_Int32() { Test("Test_MUL_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); } /* TestMethod("Test_MUL_OVF_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); */
        //[Fact] public void Test_MUL_OVF_UN__Int32_IntPtr() { Test("Test_MUL_OVF_UN__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_MUL_OVF_UN__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_DIV__IntPtr_Int32() { Test("Test_DIV__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); } /* TestMethod("Test_DIV__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); */
        //[Fact] public void Test_DIV__Int32_IntPtr() { Test("Test_DIV__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_DIV__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_DIV_UN__IntPtr_Int32() { Test("Test_DIV_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); } /* TestMethod("Test_DIV_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); */
        //[Fact] public void Test_DIV_UN__Int32_IntPtr() { Test("Test_DIV_UN__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_DIV_UN__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_REM__IntPtr_Int32() { Test("Test_REM__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); } /* TestMethod("Test_REM__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); */
        //[Fact] public void Test_REM__Int32_IntPtr() { Test("Test_REM__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_REM__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
        //[Fact] public void Test_REM_UN__IntPtr_Int32() { Test("Test_REM_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); } /* TestMethod("Test_REM_UN__IntPtr_Int32", IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L), int.MinValue); */
        //[Fact] public void Test_REM_UN__Int32_IntPtr() { Test("Test_REM_UN__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); } /* TestMethod("Test_REM_UN__Int32_IntPtr", int.MinValue, IntPtr.Size == 4 ? new IntPtr(0) : new IntPtr(0L)); */
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