using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.TestGeneration.Static
{
    public static class PrimitiveArgs
    {
        public static bool IsGreaterThan5(int x)
        {
            if (x > 5)
            {
                Console.WriteLine("x >  5");
                return true;
            }
            else
            {
                Console.WriteLine("x <= 5");
                return false;
            }
        }

        public static int MaxOfThree(int x, int y, int z)
        {
            if (x > y)
            {
                if (x > z)
                {
                    Console.WriteLine("x > z AND x > y");
                    return x;
                }
                else
                {
                    Console.WriteLine("x > y AND x <= z");
                    return z;
                }
            }
            else
            {
                if (y > z)
                {
                    Console.WriteLine("x <= y AND y > z");
                    return y;
                }
                else
                {
                    Console.WriteLine("x <= y AND y <= z");
                    return z;
                }
            }

        }
    }
}
