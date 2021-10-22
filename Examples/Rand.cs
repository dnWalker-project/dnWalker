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
            var random = new Random(42);      // (1)

            var a = random.Next(2);           // (2)
            Console.WriteLine("a=" + a);

            //... lots of code here

            var b = random.Next(3);           // (3)
            Console.WriteLine("  b=" + b);

            var c = a / (b + a - 2);                  // (4)
            Console.WriteLine("    c=" + c);
        }
    }
}
