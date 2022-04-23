﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Concolic.Features.PrimitiveValues
{
    public class MethodsWithPrimitiveValueParameter
    {
        public static void NoBranch(int x)
        {
            Console.Out.WriteLine("no branching");
        }

        public static void BranchIfPositive(int x)
        {
            if (x > 0)
            {
                Console.Out.WriteLine("x > 0");
            }
            else
            {
                Console.Out.WriteLine("x <= 0");
            }
        }

        public static void NestedBranching(int x)
        {
            if (x > 0)
            {
                Console.Out.WriteLine("(x > 0)");
                if (x < 5)
                {
                    Console.Out.WriteLine("(x < 5)");
                }
                else
                {
                    Console.Out.WriteLine("(x >= 5)");
                }
            }
            else
            {
                Console.Out.WriteLine("(x <= 0)");
                if (x < -3)
                {
                    Console.Out.WriteLine("(x < -3)");
                }
                else
                {
                    Console.Out.WriteLine("(x >= -3)");
                }
            }
        }

        public static void NestedBranchingUnsat(int x)
        {
            if (x > 0)
            {
                Console.Out.WriteLine("(x > 0)");
                if (x < -5)
                {
                    Console.Out.WriteLine("(x < -5)");
                }
                else
                {
                    Console.Out.WriteLine("(x >= -5)");
                }
            }
            else
            {
                Console.Out.WriteLine("(x <= 0)");
                if (x < -3)
                {
                    Console.Out.WriteLine("(x < -3)");
                }
                else
                {
                    Console.Out.WriteLine("(x >= -3)");
                }
            }
        }
    }
}
