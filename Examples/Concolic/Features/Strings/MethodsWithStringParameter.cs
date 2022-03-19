using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Concolic.Features.Strings
{
    public class MethodsWithStringParameter
    {
        public static void BranchBasedOnLength(string input)
        {
            if (null == input)
            {
                Console.WriteLine("input is null");
            }

            if (input.Length > 5)
            {
                Console.WriteLine("input has length greater than 5");
            }
            else
            {
                Console.WriteLine("input has length lower than or equal to 5");
            }
        }
    }
}
