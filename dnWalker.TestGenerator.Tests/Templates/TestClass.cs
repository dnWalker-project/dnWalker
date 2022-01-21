using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Tests.Templates
{
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
    }

    public static class TestClassMembers
    {
        private static readonly Type _type = typeof(TestClass);

        public static MethodInfo GetStaticNonGeneric_PositionalAndOptional()
        {
            return _type.GetMethod(nameof(TestClass.StaticNonGenericMethodWithPositionalAndOptionalArguments));
        }
        public static MethodInfo GetStaticGeneric_PositionalAndOptional<T>()
        {
            return _type.GetMethod(nameof(TestClass.StaticGenericMethodWithPositionalAndOptionalArguments)).MakeGenericMethod(typeof(T));
        }

        public static MethodInfo GetStaticNonGeneric_Positional()
        {
            return _type.GetMethod(nameof(TestClass.StaticNonGenericMethodWithPositionalArguments));
        }
        public static MethodInfo GetStaticGeneric_Positional<T1, T2>()
        {
            return _type.GetMethod(nameof(TestClass.StaticGenericMethodWithPositionalArguments)).MakeGenericMethod(typeof(T1), typeof(T2));
        }

        public static MethodInfo GetStaticNonGeneric_Optional()
        {
            return _type.GetMethod(nameof(TestClass.StaticNonGenericMethodWithOptionalArguments));
        }
        public static MethodInfo GetStaticGeneric_Optional<T1, T2>()
        {
            return _type.GetMethod(nameof(TestClass.StaticGenericMethodWithOptionalArguments)).MakeGenericMethod(typeof(T1), typeof(T2));
        }

        public static MethodInfo GetStaticNonGeneric()
        {
            return _type.GetMethod(nameof(TestClass.StaticNonGenericMethod));
        }
        public static MethodInfo GetStaticGeneric<T>()
        {
            return _type.GetMethod(nameof(TestClass.StaticGenericMethod)).MakeGenericMethod(typeof(T));
        }
    }

    public static class GenericTestClassMembers
    {
        public static MethodInfo GetStaticNonGeneric_PositionalAndOptional<T>()
        {
			Type type = typeof(GenericTestClass<>).MakeGenericType(typeof(T));
            return type.GetMethod(nameof(GenericTestClass<T>.StaticNonGenericMethodWithPositionalAndOptionalArguments));
        }
        public static MethodInfo GetStaticGeneric_PositionalAndOptional<T, T2>()
        {
			Type type = typeof(GenericTestClass<>).MakeGenericType(typeof(T));
            return type.GetMethod(nameof(GenericTestClass<T>.StaticGenericMethodWithPositionalAndOptionalArguments)).MakeGenericMethod(typeof(T2));
        }

        public static MethodInfo GetStaticNonGeneric_Positional<T>()
        {
			Type type = typeof(GenericTestClass<>).MakeGenericType(typeof(T));
            return type.GetMethod(nameof(GenericTestClass<T>.StaticNonGenericMethodWithPositionalArguments));
        }
        public static MethodInfo GetStaticGeneric_Positional<T, T2, T3>()
        {
			Type type = typeof(GenericTestClass<>).MakeGenericType(typeof(T));
            return type.GetMethod(nameof(GenericTestClass<T>.StaticGenericMethodWithPositionalArguments)).MakeGenericMethod(typeof(T2), typeof(T3));
        }

        public static MethodInfo GetStaticNonGeneric_Optional<T>()
        {
			Type type = typeof(GenericTestClass<>).MakeGenericType(typeof(T));
            return type.GetMethod(nameof(GenericTestClass<T>.StaticNonGenericMethodWithOptionalArguments));
        }
        public static MethodInfo GetStaticGeneric_Optional<T, T2, T3>()
        {
			Type type = typeof(GenericTestClass<>).MakeGenericType(typeof(T));
            return type.GetMethod(nameof(GenericTestClass<T>.StaticGenericMethodWithOptionalArguments)).MakeGenericMethod(typeof(T2), typeof(T3));
        }

        public static MethodInfo GetStaticNonGeneric<T>()
        {
            Type type = typeof(GenericTestClass<>).MakeGenericType(typeof(T));
            return type.GetMethod(nameof(GenericTestClass<T>.StaticNonGenericMethod));
        }
        public static MethodInfo GetStaticGeneric<T, T2>()
        {
            Type type = typeof(GenericTestClass<>).MakeGenericType(typeof(T));
            return type.GetMethod(nameof(GenericTestClass<T>.StaticGenericMethod)).MakeGenericMethod(typeof(T2));
        }
    }
}
