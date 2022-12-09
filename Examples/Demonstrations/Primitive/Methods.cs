using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Demonstrations.Primitive
{
    public class Methods
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

        public static int Foo(int x, int y)
        {
            int a = x + y;

            if (x < 0)
            {
                a = -x + y;
            }

            if (a < x + y)
            {
                return 1;
            }

            if (a == 2 * y + 314) throw new InvalidOperationException();
            return 0;
        }
    }
}
