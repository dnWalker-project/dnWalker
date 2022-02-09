using Examples.Concolic.Features.Objects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Concolic.Features.ReturnValue
{
    public class MethodsWithReturnValue
    {
        public static int GetSumIfTrueDiffIfFalse(int a, int b, bool flag)
        {
            if (flag)
            {
                if (a < 10)
                {
                    return a + b + 8;
                }
                else
                {
                    return a + b - 6;
                }
            }
            else
            {
                if (b < 10)
                {
                    return a - b + 8;
                }
                else
                {
                    return a - b - 6;
                }
            }
        }

        public static int ReturnTheArgument(int a)
        {
            return a;
        }

        public static TestClass ReturnNewIfNull_ItselfOtherwise(TestClass instance)
        {
            if (instance == null)
            {
                return new TestClass();
            }
            else
            {
                return instance;
            }
        }
    }
}
