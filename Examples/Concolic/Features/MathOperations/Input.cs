using System;

namespace Examples.Concolic.Features.MathOperations
{
    public static class Ext
    {
        public static double ToRadians(this double val)
        {
            return (System.Math.PI / 180) * val;
        }
    }

    /// <summary>
    /// https://github.com/psycopaths/jdart/blob/master/src/examples/features/math/Input.java
    /// </summary>
    public class Input
    {
        public static void foo(double d)
        {
            var f = System.Math.Ceiling(d);
            if (f == 10)
                Console.Out.WriteLine("Math.ceil(" + d + ") == 10");
            else
                Console.Out.WriteLine("Math.ceil(" + d + ") != 10");
        }        

        public static void bar(double d1, double d2)
        {
            var s = 2.2.ToRadians();
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

        public static void main(string[] args)
        {
            Console.Out.WriteLine("-------- In main!");
            var inst = new Input();
            try
            {
                Input.foo(3.14159);
            }
            catch (Exception t)
            {
                Console.Out.WriteLine("Caught the rascal <" + t.Message + "> redhanded!");
            }

            Input.bar(1, 2);
        }
    }
}
