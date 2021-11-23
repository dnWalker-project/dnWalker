using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Concolic.Features.Arrays
{
    /// <summary>
    /// https://github.com/psycopaths/jdart/blob/master/src/examples/features/arrays/Input.java
    /// </summary>
    public class Input
    {
        static public int m1(char[] c, int n)
        {
            var str = new string(c);
            Console.Out.WriteLine("Parameters - " + str + " " + n);
            var state = 0;
            //    if(c == null || c.length == 0)  {
            //      return -1;
            //    }
            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] == '[') state = 1;
                else if (state == 1 & c[i] == '{') state = 2;
                else if (state == 2 & c[i] == '<') state = 3;
                else if (state == 3 & c[i] == '*')
                {
                    state = 4;
                    if (c.Length == 15)
                    {
                        state = state + n;
                    }
                }
            }

            return 1;
        }

        static public void m2(int i, char[] c)
        {
            var str = new string(c);
            Console.Out.WriteLine("In TestMe2. Parameters = " + str + " " + i);
            if (i == 0)
            {
                if (c[0] == c[1])
                {
                    Console.Out.WriteLine("c[0] == c[1]!");
                }
                else
                {
                    Console.Out.WriteLine("c[0] != c[1]!");
                }
            }
            else if ((i >= 1) && (i <= c.Length - 2) && c[i] != c[i + 1])
            {
                Console.Out.WriteLine("c[i] != c[i + 1]!");
            }
            else
            { }
            //      assert(false);
        }

        public void m3(int i, double[] d)
        {
            Console.Out.WriteLine("In TestMe3. Parameters = " + i + " " + d[0] + " " + d[1]);
            int k;
            if (i >= 0 && i < 2 && d[i] == 3.141)
            {
                Console.Out.WriteLine("i >= 0 and i < 2 and d[0] == 3.141");
            }
            else if (i == 2)
            {
                for (k = 0; k < i; k++)
                    if (d[k] == d[k + 1])
                        Console.Out.WriteLine("k = " + k + " d[k] == d[k + 1]");
                    else
                        Console.Out.WriteLine("k = " + k + " d[k] != d[k + 1]");
            }
            else
                System.Diagnostics.Debug.Assert(false);
        }

        static public void m4(int i, float d0, float d1, float d2)
        {
            Console.Out.WriteLine("In TestMe4. Parameters = " + i + " " + d0 + " " + d1 + " " + d2);
            int k;
            if (i >= 0 && i < 2 && d1 == 3.141)
            {
                Console.Out.WriteLine("i >= 0 and i < 2 and d1 == 3.141");
            }
            else if (i == 2)
            {
                for (k = 0; k < i; k++)
                    if (k == 0)
                    {
                        if (d0 == d1)
                            Console.Out.WriteLine("k = " + k + " d0 == d1");
                        else
                            Console.Out.WriteLine("k = " + k + " d0 != d1");
                    }
                    else
                    {
                        if (d1 == d2)
                            Console.Out.WriteLine("k = " + k + " d1 == d2");
                        else
                            Console.Out.WriteLine("k = " + k + " d1 != d2");
                    }
            }
            else
                System.Diagnostics.Debug.Assert(false);
        }

        public static void main(string[] args)
        {
            Console.Out.WriteLine("-------- In main!");
            char[] c = { 'a', 'b', '{', '}', 'c', 'd' };
            m1(c, 0);
            var uber = new Input();
            double[] d = { 1.0, 2, 3, 4, 5, 6, 7, 8 };

            m2(10, c);

            try
            {
                uber.m3(1, d);
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine(e.ToString());
            }

            try
            {
                float d0 = 1;
                float d1 = 2;
                float d2 = 3;
                m4(1, d0, d1, d2);
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine(e.ToString());
            }
        }
    }
}