using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Concolic.Features.Strings
{
    public class MethodsWithStringParameter
    {
        public static void BranchOnNull(string input)
        {
            if (input == null)
            {
                Console.Out.WriteLine("input is null");
            }
            else
            { 
                Console.Out.WriteLine("input is not null");
            }
        }

        public static void BranchOnNullOrEmpty(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                Console.Out.WriteLine("input is null or empty");
            }
            else
            {
                Console.Out.WriteLine("input is not null and not empty");
            }
        }

        public static void BranchOnLength(string input)
        {
            if (input.Length > 5)
            {
                Console.Out.WriteLine("input.Length > 5");
            }
            else
            {
                Console.Out.WriteLine("input.Length <= 5");
            }
        }

        public static void BranchOnConstEquality(string input)
        {
            if (input == "hello world")
            {
                Console.Out.WriteLine("input is hello world");
            }
            else
            {
                Console.Out.WriteLine("input is not hello world");
            }
        }

        public static void BranchOnNonConstEquality(string input1, string input2)
        {
            if (input1 == null || input2 == null)
            {
                Console.Out.WriteLine("input1 or input2 is null");
                return;
            }

            if (input1 == input2)
            {
                Console.Out.WriteLine("input1 == input2");
            }
            else
            {
                Console.Out.WriteLine("input1 != input2");
            }
        }

        public static void BranchOnCharAtStaticIndexConstEquality(string input)
        {
            if (input[3] == 'a')
            {
                Console.Out.WriteLine("input[3] == 'a'");
            }
            else
            {
                Console.Out.WriteLine("input[3] != 'a'");
            }
        }

        public static void BranchOnCharAtDynamicIndexDynamicEquality(string input, int index, char cmp)
        {
            if (input[index] == cmp)
            {
                Console.Out.WriteLine("input[$index] == $cmp");
            }
            else
            {
                Console.Out.WriteLine("input[$index] != $cmp");
            }
        }

        public static void BranchOnContainsConstString(string input)
        {
            if (input.Contains("HELLO_WORLD"))
            {
                Console.Out.WriteLine("input contains HELLO_WORLD");
            }
            else
            {
                Console.Out.WriteLine("input doesn't contain HELLO_WORLD");
            }
        }

        public static void BranchOnContainsDynamicString(string input, string containee)
        {
            if (String.IsNullOrEmpty(containee))
            {
                Console.Out.WriteLine("$containee is null or empty");
                return;
            }

            if (input.Contains(containee))
            {
                Console.Out.WriteLine("input contains $containee");
            }
            else
            {
                Console.Out.WriteLine("input doesn't contain $containee");
            }
        }

        public static void BranchOnContainsConstChar(string input)
        {
            if (input.Contains('Q'))
            {
                Console.Out.WriteLine("input contains 'Q'");
            }
            else
            {
                Console.Out.WriteLine("input doesn't contain 'Q'");
            }
        }

        public static void BranchOnContainsDynamicChar(string input, char symbol)
        {
            if (input.Contains(symbol))
            {
                Console.Out.WriteLine("input contains $symbol");
            }
            else
            {
                Console.Out.WriteLine("input doesn't contain $symbol");
            }
        }

        public static void BranchOnPrefixConstEquality(string input)
        {
            if (input.StartsWith("PREFIX"))
            {
                Console.Out.WriteLine("input starts with PREFIX");
            }
            else
            {
                Console.Out.WriteLine("input does not start with PREFIX");
            }
        }

        public static void BranchOnPrefixDynamicEquality(string input, string prefix)
        {
            if (input.StartsWith(prefix))
            {
                Console.Out.WriteLine("input starts with $prefix");
            }
            else
            {
                Console.Out.WriteLine("input does not start with $prefix");
            }
        }

        public static void BranchOnSuffixConstEquality(string input)
        {
            if (input.EndsWith("SUFFIX"))
            {
                Console.Out.WriteLine("input ends with SUFFIX");
            }
            else
            {
                Console.Out.WriteLine("input does not end with SUFFIX");
            }
        }

        public static void BranchOnSuffixDynamicEquality(string input, string suffix)
        {
            if (input.EndsWith(suffix))
            {
                Console.Out.WriteLine("input ends with $suffix");
            }
            else
            {
                Console.Out.WriteLine("input does not end with $suffix");
            }
        }

        public static void BranchOnSubstringEquality(string input)
        {
            if (input.Substring(5, 4) == "ABCD")
            {
                Console.Out.WriteLine("input[5..4] == ABCD");
            }
            else
            {
                Console.Out.WriteLine("input[5..4] != ABCD");
            }
        }
    }
}
