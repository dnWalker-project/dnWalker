using System;
namespace Examples
{
    public class Equalities
    {
        public static void StringEquality(string a, string b)
        {
            if (a == b) Console.WriteLine("a == b");
            else Console.WriteLine("a != b");
        }

        public static void StringNullEquality(string x)
        {
            if (x == null) Console.WriteLine("x is null");
            else Console.WriteLine("x is not null");
        }

    }
}

