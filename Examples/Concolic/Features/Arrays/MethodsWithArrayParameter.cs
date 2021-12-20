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
            }
            else
            {
                Console.Out.WriteLine("instance is not null");
            }
        }

        public static void BranchIfNotNull(int[] instance)
        {
            if (instance != null)
            {
                Console.Out.WriteLine("instance is not null");
            }
            else
            {
                Console.Out.WriteLine("instance is null");
            }
        }

        public static void BranchIfLengthLowerThan5(int[] instance)
        {
            if (instance != null && instance.Length < 5)
            {
                Console.Out.WriteLine("instance is not null && length is lower than 5");
            }

            Console.Out.WriteLine("instance is null || length is lower than 5");
        }

        public static void BranchIfLengthGreaterThan5(int[] instance)
        {
            if (instance != null && instance.Length > 5)
            {
                Console.Out.WriteLine("instance is not null && length is greater than 5");
            }

            Console.Out.WriteLine("instance is null || length is greater than 5");
        }
    }
}
