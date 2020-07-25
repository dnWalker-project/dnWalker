using System;

namespace Examples
{
    class Rand
    {
        /// <summary>
        /// https://github.com/javapathfinder/jpf-core/wiki/Random-Example
        /// </summary>
        public static void Go()
        {
            Random random = new Random(42);      // (1)

            int a = random.Next(2);           // (2)
            Console.WriteLine("a=" + a);

            //... lots of code here

            int b = random.Next(3);           // (3)
            Console.WriteLine("  b=" + b);

            int c = a / (b + a - 2);                  // (4)
            Console.WriteLine("    c=" + c);
        }
    }
}
