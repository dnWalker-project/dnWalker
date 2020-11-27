using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Concolic.Features.Math
{
    public static class Ext
    {
        public static double ToRadians(this double val)
        {
            return (System.Math.PI / 180) * val;
        }
    }

    // https://github.com/psycopaths/jdart/blob/master/src/examples/features/math/Input.java
    public class Input
    {
        public void foo(double d)
        {
            double f = System.Math.Ceiling(d);
            if (f == 10)
                Console.Out.WriteLine("Math.ceil(" + d + ") == 10");
            else
                Console.Out.WriteLine("Math.ceil(" + d + ") != 10");
        }        

        public void bar(double d1, double d2)
        {
            double s = 2.2.ToRadians();
            if (d1 > d2)
            {
                if (System.Math.Sqrt(d1) * s >= 0)
                {

                }
                else
                {
                    System.Diagnostics.Debug.Assert(false);
                }
            }
        }

        public static void main(String[] args)
        {
            Console.Out.WriteLine("-------- In main!");
            Input inst = new Input();
            try
            {
                inst.foo(3.14159);
            }
            catch (Exception t)
            {
                Console.Out.WriteLine("Caught the rascal <" + t.Message + "> redhanded!");
            }

            inst.bar(1, 2);
        }
    }
}
