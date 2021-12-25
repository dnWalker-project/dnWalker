using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Concolic.Features.Arrays
{
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

        public static void BranchIfLengthLowerThan5(int[] instance)
        {
            if (instance != null && instance.Length < 5)
            {
                Console.Out.WriteLine("instance is not null && length is lower than 5");
                return;
            }

            Console.Out.WriteLine("instance is null || length is lower than 5");
        }

        public static void BranchIfLengthGreaterThan5(int[] instance)
        {
            if (instance != null && instance.Length > 5)
            {
                Console.Out.WriteLine("instance is not null && length is greater than 5");
                return;
            }

            Console.Out.WriteLine("instance is null || length is greater than 5");
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
    }
}
