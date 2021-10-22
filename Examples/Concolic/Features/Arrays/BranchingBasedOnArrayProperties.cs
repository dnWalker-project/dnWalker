using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Concolic.Features.Arrays
{
    public class BranchingBasedOnArrayProperties
    {
        public static void BranchBasedOnArrayLength(int[] array)
        {
            if (array == null)
            {
                Console.Out.WriteLine("Argument null Exception!");
                return;
            }

            if (array.Length == 3)
            {
                Console.Out.WriteLine("array.Length == 3");
            }
            else
            {
                Console.Out.WriteLine("array.Length != 3");
            }
        }

        public static void BranchBasedOnArrayElementAtDynamicIndex(int[] array, int index)
        {
            if (array == null)
            {
                Console.Out.WriteLine("Argument null Exception!");
                return;
            }

            if (index >= array.Length)
            {
                Console.Out.WriteLine("Index Out Of Range Exception!");
                return;
            }

            if (array[index] >= 10)
            {
                Console.Out.WriteLine("array[" + index + "] >= 10");
            }
            else
            {
                Console.Out.WriteLine("array[" + index + "] < 10");
            }
        }

        public static void BranchBasedOnArrayElementAtStaticIndex(int[] array)
        {
            if (array == null)
            {
                Console.Out.WriteLine("Argument null Exception!");
                return;
            }

            if (array.Length <= 1)
            {
                Console.Out.WriteLine("Index Out Of Range Exception!");
                return;
            }


            if (array[1] >= 10)
            {
                Console.Out.WriteLine("array[" + 1 + "] >= 10");
            }
            else
            {
                Console.Out.WriteLine("array[" + 1 + "] < 10");
            }
        }
    }
}
