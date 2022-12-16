using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Concolic.Features.MathOperations
{
    public static class MethodsWithMath
    {
        public static int WithMin(int x, int y, int z)
        {
            int a = Math.Min(x, y);
            if (a >= z)
            {
                return a;
            }
            else
            {
                return z;
            }
        }

        public static int WithMax(int x, int y, int z)
        {
            int a = Math.Min(x, y);
            if (a >= z)
            {
                return a;
            }
            else
            {
                return z;
            }
        }

        public static int WithAbs(int x, int y, int z)
        {
            int xx = Math.Abs(x);
            int yy = Math.Abs(y);
            int zz = Math.Abs(z);
            
            if (xx < yy)
            {
                return zz;
            }
            else
            {
                return yy;
            }
        }
    }
}
