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
				Console.Out.WriteLine(" x > 0");
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
	}
}
