﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Interpreter
{
    public static class IntepreterTests
    {
        public static int Test_SWITCH__6_Double(double A_0)
        {
            switch (A_0)
            {
                case 0.0d: return 1;
                case -1.0d: return 2;
                case 1.0d: return 3;
                case double.MinValue: return 4;
                case double.MaxValue: return 5;
                case double.NaN: return 6;
                case double.NegativeInfinity: return 7;
                //case double.PositiveInfinity: return 8;
                //case double.Epsilon: return 9;
                case 2.0d: return 10;
                case 3.0d: return 11;
                case 4.0d: return 12;
                case 5.0d: return 13;
                case 6.0d: return 14;
                default:
                    return 15;
            }
        }

        public static int Test_SWITCH__6_Single(float A_0)
        {
            switch (A_0)
            {
                case 0.0f: return 1;
                case -1.0f: return 2;
                case 1.0f: return 3;
                case float.MinValue: return 4;
                case float.MaxValue: return 5;
                case float.NaN: return 6;
                //case float.NegativeInfinity: return 7;
                case float.PositiveInfinity: return 8;
                case float.Epsilon: return 9;
                case 2.0f: return 10;
                case 3.0f: return 11;
                case 4.0f: return 12;
                case 5.0f: return 13;
                case 6.0f: return 14;
                default:
                    return 15;
            }
        }

        public static int Test_SWITCH__6_Int64(long A_0)
        {
            switch (A_0)
            {
                case 3L:
                    return 3;
                default:
                    return 8;
                case 1L:
                    return 1;
                case 0L:
                    return 0;
                case 2L:
                case 5L:
                    return 2;
            }
        }
    }
}
