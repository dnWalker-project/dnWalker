using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Concolic.Features.Arrays
{
    public class TestClass
    {

    }

    public static class MethodsWithArrayParameter
    {


        public static void BranchIfNull(int[] instance)
        {
            if (instance == null)
            {
                Console.Out.WriteLine("instance is null");
                return;
            }
            else
            {
                Console.Out.WriteLine("instance is not null");
                return;
            }
        }

        public static void BranchIfNotNull(int[] instance)
        {
            if (instance != null)
            {
                Console.Out.WriteLine("instance is not null");
                return;
            }
            else
            {
                Console.Out.WriteLine("instance is null");
                return;
            }
        }

        public static void BranchIfLengthLessThan5(int[] instance)
        {
            if (instance != null && instance.Length < 5)
            {
                Console.Out.WriteLine("instance is not null && length is less than 5");
                return;
            }

            Console.Out.WriteLine("instance is null || length is greater than 5");
        }

        public static void BranchIfLengthGreaterThan5(int[] instance)
        {
            if (instance != null && instance.Length > 5)
            {
                Console.Out.WriteLine("instance is not null && length is greater than 5");
                return;
            }

            Console.Out.WriteLine("instance is null || length is less than 5");
        }

        public static void BranchIfItemAtStaticIndexIsGreaterThan5(double[] array)
        {
            if (array == null)
            {
                Console.Out.WriteLine("array is null");
                return;
            }

            if (array.Length < 4)
            {
                Console.Out.WriteLine("length is less than 4");
                return;
            }

            if (array[3] > 5)
            {
                Console.Out.WriteLine("array[3] > 5");
                return;
            }
            else
            {
                Console.Out.WriteLine("array[3] <= 5");
                return;
            }
        }

        public static void BranchIfItemAtDynamicIndexIsGreaterThan5(double[] array, int index)
        {
            if (array == null)
            {
                Console.Out.WriteLine("array is null");
                return;
            }

            if (array.Length <= index)
            {
                Console.Out.WriteLine("length is less than index");
                return;
            }

            if (array[index] > 5)
            {
                Console.Out.WriteLine("array[index] > 5");
                return;
            }
            else
            {
                Console.Out.WriteLine("array[index] <= 5");
                return;
            }
        }

        public static void SetElement_InputParameter(TestClass[] arr, TestClass value)
        {
            if (arr == null || arr.Length < 3) return;
            arr[2] = value;
        }

        public static void SetElement_ConstructParameter_Object(TestClass[] arr)
        {
            if (arr == null || arr.Length < 3) return;
            arr[2] = new TestClass();
        }

        public static void SetElement_ConstructParameter_Primitive(int[] arr)
        {
            if (arr == null || arr.Length < 3) return;
            arr[0] = 2;
            arr[1] = 4;
            arr[2] = 5;
        }

        public static void SetElement_ConstructParameter_Array(TestClass[][] arr2D)
        {
            if (arr2D == null || arr2D.Length < 3) return;

            TestClass[] arr = new TestClass[5];
            arr2D[2] = arr;
        }

        public static void SetElement_ConstructParameter_Null(TestClass[] arr)
        {
            if (arr == null || arr.Length < 3) return;
            arr[1] = null;
        }
    }
}
