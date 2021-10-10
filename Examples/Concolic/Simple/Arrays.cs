using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Concolic.Simple
{
    public class Arrays
    {
        public static void BranchBasedOnArrayLength(Int32[] array)
        {
            if (array.Length == 3)
            {
                Console.Out.WriteLine("array.Lenght == 3");
            }
            else
            {
                Console.Out.WriteLine("array.Lenght != 3");
            }
        }

        public static void BranchBasedOnArrayElementAtDynamicIndex(Int32[] array, Int32 index)
        {
            if (index >= array.Length) throw new IndexOutOfRangeException();

            if (array[index] >= 10)
            {
                Console.Out.WriteLine("array[" + index + "] >= 10");
            }
            else
            {
                Console.Out.WriteLine("array[" + index + "] < 10");
            }
        }

        public static void BranchBasedOnArrayElementAtStaticIndex(Int32[] array)
        {
            
            if (1 >= array.Length) throw new IndexOutOfRangeException();

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
