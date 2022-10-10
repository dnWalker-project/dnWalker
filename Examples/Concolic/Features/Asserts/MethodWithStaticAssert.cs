using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Concolic.Features.Asserts
{
    public class MethodsWithAssert
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool MethodWithStaticAssertViolation()
        {
            Debug.Assert(false, "Should always fail.");
            return true;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool MethodWithStaticAssertPass()
        {
            Debug.Assert(true, "Should always pass.");
            return true;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool MethodWithDynamicAssert(int value)
        {
            Debug.Assert(value == 0, "Should fail if value is 0.");
            return true;
        }
    }
}
