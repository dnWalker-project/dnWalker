using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Console.Out.WriteLine("a=" + a);

            //... lots of code here

            int b = random.Next(3);           // (3)
            Console.Out.WriteLine("  b=" + b);

            int c = a / (b + a - 2);                  // (4)
            Console.Out.WriteLine("    c=" + c);
        }
    }
}
