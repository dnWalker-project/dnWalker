using dnWalker.DataElements;
using dnWalker.TypeSystem;

using FluentAssertions;

using MMC;
using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.Tests.Interpreter.Concrete
{
    public abstract class ConcreteTestBase : InterpreterTestBase
    {
        protected ConcreteTestBase(ITestOutputHelper output) : base(output)
        {
        }

        protected void Test(string methodName, params object[] args)
        {
            TestAndCompare(GetFullMethodName(methodName), args);
        }


        private object Test(string methodName, out ExceptionInfo unhandledException, params object[] args)
        {
            var explorer = Utils.GetModelChecker(methodName, args, DefinitionProvider);

            explorer.Run();

            unhandledException = explorer.GetUnhandledException();
            return explorer.ActiveState.CurrentThread.RetValue;
        }

        private void TestAndCompare(string methodName, object[] args)
        {
            // run in model checker
            var modelCheckerResult = Test(methodName, out var modelCheckerException, args);

            var methodInfo = Utils.GetMethodInfo(methodName);

            // run in CLR
            object? res2;
            Exception? ex2 = null;
            try
            {
                res2 = methodInfo.Invoke(null, args);
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

            // compare results
            // unhandled exception
            if (ex2 != null)
            {
                modelCheckerException?.Type.FullName.Should().Be(ex2?.GetType().FullName, modelCheckerException?.ToString());
                //modelCheckerException?.Message.Should().BeEquivalentTo(ex2?.Message); // have only the field Exception._message but the property could change it
                return;
            }

            // return type
            if (methodInfo.ReturnType != typeof(void))
            {
                if (modelCheckerResult != null && modelCheckerResult.Equals(ObjectReference.Null))
                {
                    modelCheckerResult = null;
                }

                if (modelCheckerResult == null && res2 == null)
                {
                    return;
                }

                if (modelCheckerException == null && modelCheckerResult == null && res2 != null)
                {
                    modelCheckerResult.Should().Be(res2);
                    return;
                }

                if (modelCheckerResult.GetType() == res2.GetType() && modelCheckerResult.GetType() == typeof(IntPtr))
                {
                    modelCheckerResult = ((IntPtr)modelCheckerResult).ToInt64();
                    res2 = ((IntPtr)res2).ToInt64();
                }

                if (modelCheckerResult.GetType() == res2.GetType() && modelCheckerResult.GetType() == typeof(UIntPtr))
                {
                    modelCheckerResult = ((UIntPtr)modelCheckerResult).ToUInt64();
                    res2 = ((UIntPtr)res2).ToUInt64();
                }

                if (methodInfo.ReturnType == typeof(IntPtr))
                {
                    var rr1 = (IConvertible)modelCheckerResult;
                    var rr2 = (IntPtr)res2;

                    Convert.ChangeType(rr1, typeof(long)).Should().BeEquivalentTo(rr2.ToInt64());
                    return;
                }

                if (methodInfo.ReturnType == typeof(UIntPtr))
                {
                    var rr1 = (IConvertible)modelCheckerResult;
                    var rr2 = (UIntPtr)res2;

                    Convert.ChangeType(rr1, typeof(ulong)).Should().BeEquivalentTo(rr2.ToUInt64());
                    return;
                }

                modelCheckerResult.Should().NotBeNull("value returned from model checker should not be null, but " + res2.ToString());
                res2.Should().NotBeNull();

                if (!methodInfo.ReturnType.IsValueType && methodInfo.ReturnType != typeof(string))
                {
                    if (res2 == null && modelCheckerResult != null)
                    {
                        modelCheckerResult.Should().BeNull("method returned a null reference.");
                        return;
                    }

                    if (res2 != null && modelCheckerResult == null)
                    {
                        modelCheckerResult.Should().NotBeNull("method did not return a null reference.");
                        return;
                    }

                    modelCheckerResult.Should().BeAssignableTo<ReturnValue>();
                    (modelCheckerResult as IComparable)?.CompareTo(res2).Should().Be(0);
                    return;
                }

                modelCheckerResult.Should().BeAssignableTo<IConvertible>();
                res2.Should().BeAssignableTo<IConvertible>();

                var modelCheckerReturnValue = Convert.ChangeType(modelCheckerResult, methodInfo.ReturnType);
                var r2 = Convert.ChangeType(res2, methodInfo.ReturnType);

                modelCheckerReturnValue.Should().BeEquivalentTo(r2);
            }
        }
    }
}
