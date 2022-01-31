using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Tests.Templates
{
    public class TopLevel
    {
        public class SubLevel
        {
        }

        public class GenericSubLevel<T>
        {

        }
    }

    public class GenericTopLevel<TTop>
    {
        public class SubLevel
        {

        }

        public class GenericSubLevel<TSub>
        {

        }
    }


    internal class TestClass
    {
        public static void StaticNonGenericMethodWithPositionalAndOptionalArguments(int index, string message = null)
        {

        }

        public static void StaticGenericMethodWithPositionalAndOptionalArguments<T>(T data, string message = null)
        {

        }

        public static void StaticNonGenericMethodWithPositionalArguments(int index, string data)
        { 
        
        }

        public static void StaticGenericMethodWithPositionalArguments<T1, T2>(T1 index, T2 data)
        {

        }

        public static void StaticNonGenericMethodWithOptionalArguments(int index = 0, string data = null)
        {

        }

        public static void StaticGenericMethodWithOptionalArguments<T1, T2>(T1 index = default(T1), T2 data = default(T2))
        {

        }

        public static void StaticNonGenericMethod()
        {

        }

        public static void StaticGenericMethod<T1>()
        {

        }


        public void NonGenericMethodWithPositionalAndOptionalArguments(int index, string message = null)
        {

        }

        public void GenericMethodWithPositionalAndOptionalArguments<T>(T data, string message = null)
        {

        }

        public void NonGenericMethodWithPositionalArguments(int index, string data)
        {

        }

        public void GenericMethodWithPositionalArguments<T1, T2>(T1 index, T2 data)
        {

        }

        public void NonGenericMethodWithOptionalArguments(int index = 0, string data = null)
        {

        }

        public void GenericMethodWithOptionalArguments<T1, T2>(T1 index = default(T1), T2 data = default(T2))
        {

        }

        public void NonGenericMethod()
        {

        }

        public void GenericMethod<T1>()
        {

        }
    }

    internal class GenericTestClass<T>
    {
        public static void StaticNonGenericMethodWithPositionalAndOptionalArguments(int index, string message = null)
        {
        
        }

        public static void StaticGenericMethodWithPositionalAndOptionalArguments<T2>(T2 data, string message = null)
        {

        }

        public static void StaticNonGenericMethodWithPositionalArguments(int index, string data)
        {

        }

        public static void StaticGenericMethodWithPositionalArguments<T1, T2>(T1 index, T2 data)
        {

        }

        public static void StaticNonGenericMethodWithOptionalArguments(int index = 0, string data = null)
        {

        }

        public static void StaticGenericMethodWithOptionalArguments<T1, T2>(T1 index = default(T1), T2 data = default(T2))
        {

        }

        public static void StaticNonGenericMethod()
        {

        }

        public static void StaticGenericMethod<T1>()
        {

        }

        public void NonGenericMethodWithPositionalAndOptionalArguments(int index, string message = null)
        {

        }

        public void GenericMethodWithPositionalAndOptionalArguments<T2>(T2 data, string message = null)
        {

        }

        public void NonGenericMethodWithPositionalArguments(int index, string data)
        {

        }

        public void GenericMethodWithPositionalArguments<T1, T2>(T1 index, T2 data)
        {

        }

        public void NonGenericMethodWithOptionalArguments(int index = 0, string data = null)
        {

        }

        public void GenericMethodWithOptionalArguments<T1, T2>(T1 index = default(T1), T2 data = default(T2))
        {

        }

        public void NonGenericMethod()
        {

        }

        public void GenericMethod<T1>()
        {

        }
    }
}
