using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Concolic.Features.Strings
{
    public class MethodsWithStringParameter
    {
        public static void BranchOnLength(string input)
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

        public static void BranchOnEquality(string input)
        {
            if (input == "hello world")
            {
                Console.WriteLine("input is hello world");
            }
            else
            {
                Console.WriteLine("input is not hello world");
            }
        }

        public static void BranchOnPrefix(string input)
        {
            if (input == null)
            {
                Console.WriteLine("input is null");
                return;
            }

            if (input.StartsWith("some prefix"))
            {
                Console.WriteLine("input is: 'some prefix'.*");
            }
            else
            {
                Console.WriteLine("input is not: 'some prefix'.*");
            }
        }

        public static void BranchOnSuffix(string input)
        {
            if (input == null)
            {
                Console.WriteLine("input is null");
                return;
            }

            if (input.EndsWith("some suffix"))
            {
                Console.WriteLine("input is: .*'some prefix'");
            }
            else
            {
                Console.WriteLine("input is not: .*'some suffix'");
            }
        }

        public static void BranchOnContains(string input)
        {
            if (input == null)
            {
                Console.WriteLine("input is null");
                return;
            }

            if (input.Contains("hello world"))
            {
                Console.WriteLine("input contains 'hello world'");
            }
            else
            {
                Console.WriteLine("input does not contain 'hello world'");
            }
        }

        public static void BranchOnSubstringEquality(string input)
        {
            if (input == null || input.Length < 20)
            {
                Console.WriteLine("input is null or too short");
                return;
            }

            if (input.Substring(5, 10) == "AAAAAAAAAA")
            {
                Console.WriteLine("input is .{5}AAAAAAAAAA.{5}");
            }
            else
            {
                Console.WriteLine("input is not .{5}AAAAAAAAAA.{5}");
            }
        }
    }
}
