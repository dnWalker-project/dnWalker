using System;

namespace Examples.Simple
{
    public class Branches
    {
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
