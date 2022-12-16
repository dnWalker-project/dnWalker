using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Demonstrations.Primitive
{
    public class PrimitiveArguments
    {

        public static int Pow(int a, int n)
        {
            int z = 1;
            for (int i = 0; i < n; ++i)
            {
                z *= a;
            }
            return z;
        }

        public static double MixedNumbers(double x, int n)
        {
            if (x < n)
            {
                return x * n;
            }
            else if (x == n)
            {
                if (x >= 0)
                {
                    return x / n;
                }
            }

            return x + n;
        }

        public static int UsePow(double x, double y, double z)
        {
            if (Math.Pow(x, y) < z && z > 36)
            {
                return 0;
            }
            return 1;
        }

        public static int UseSin(double x, double y)
        {
            if (Math.Sin(x) >= y && y < -0.5)
            {
                return 0;
            }
            return 1;
        }

        public static int UseCos(double x, double y)
        {
            if (Math.Cos(x) >= y && y > 0.5)
            {
                return 0;
            }
            return 1;
        }

        public static int DivideByZero(int x, int y, int z)
        {
            if (5 * (x / y) + 3 >= z)
            {
                return 1;
            }

            return 0;
        }

        public static int RemainderByZero(int x, int y, int z)
        {
            if (5 * (x % y) + 3 >= z)
            {
                return 1;
            }

            return 0;
        }

        public static int Min3(int a, int b, int c)
        {
            if (a > b)
            {
                a = b;
            }
            if (a > c)
            {
                a = c;
            }
            return a;
        }
    }
}
