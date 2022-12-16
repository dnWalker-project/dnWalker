using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Demonstrations.ConcreteData
{
    public class Arrays
    {
        public static int IndexOf(int value, int[] array)
        {
            if (array == null) throw new ArgumentNullException("array");

            for (int i = 0; i < array.Length; ++i)
            {
                if (array[i] == value) return i;
            }
            return -1;
        }
    }
}

