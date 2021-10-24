using System;

namespace Examples.Concolic.Simple
{
    public class Branches
    {
        public static void NoBranching(int x)
        {
            Console.Out.WriteLine("x = " + x);
        }

        public static void SingleBranching(int x)
        {
            if (x < 0)
            {
                Console.Out.WriteLine("x < 0");
            }
            else
            {
                Console.Out.WriteLine(" x >= 0");
            }
        }

        public static void SingleBranchingWithModification(int x)
        {
            x = -x;

            //if (-x < 0)
            if (x < 0)
            {
                Console.Out.WriteLine("x > 0");
            }
            else
            {
                Console.Out.WriteLine(" x <= 0");
            }
        }

        public static void MultipleBranchingWithMultipleParameters(int x, int y, int z)
        {
            if (x + y < z)
            {
                if (x > z)
                {
                    Console.Out.WriteLine("y < 0");
                }
                else
                {
                    Console.Out.WriteLine("cant say much anything");
                }
            }
            else if (x < z)
            {
                Console.Out.WriteLine("x < z");
            }
            else
            {
                Console.Out.WriteLine("dont know anything");
            }
        }

        // https://github.com/SymbolicPathFinder/jpf-symbc/blob/master/src/examples/simple/Branches.java

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void Branch(int x, int y)
        {
            if (x < 0)
            {
                x = -x;
            }
            if (y < 0)
            {
                y = -y;
            }
            if (x < y)
            {
                Console.Out.WriteLine("abs(x)<abs(y)");
            }
            else if (x == 0)
            {
                Console.Out.WriteLine("x==y==0");
            }
            else
            {
                Console.Out.WriteLine("x>=y>=0");
            }
        }



        public static void Branch_Equals(int x)
        {
            if (x == 5)
            {
                Console.Out.WriteLine("x == 5");
            }
            else
            {
                Console.Out.WriteLine("x != 5");
            }
        }


        public static void Branch_NotEquals(int x)
        {
            if (x != 5)
            {
                Console.Out.WriteLine("x != 5");
            }
            else
            {
                Console.Out.WriteLine("x == 5");
            }
        }

        public static void Branch_GreaterThan(int x)
        {
            if (x > 5)
            {
                Console.Out.WriteLine("x > 5");
            }
            else
            {
                Console.Out.WriteLine("x <= 5");
            }
        }

        public static void Branch_GreaterThanOrEquals(int x)
        {
            if (x >= 5)
            {
                Console.Out.WriteLine("x >= 5");
            }
            else
            {
                Console.Out.WriteLine("x < 5");
            }
        }

        public static void Branch_LowerThan(int x)
        {
            if (x < 5)
            {
                Console.Out.WriteLine("x < 5");
            }
            else
            {
                Console.Out.WriteLine("x >= 5");
            }
        }

        public static void Branch_LowerThanOrEquals(int x)
        {
            if (x <= 5)
            {
                Console.Out.WriteLine("x <= 5");
            }
            else
            {
                Console.Out.WriteLine("x > 5");
            }
        }
    }
}
