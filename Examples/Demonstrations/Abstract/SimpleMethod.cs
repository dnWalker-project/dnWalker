
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Examples.Demonstrations.Abstract
{
    public interface IBar
    {
        int GetValue();
    }

    public class SimpleMethod
    {
        public int Value;

        public int Foo(IBar bar)
        {
            if (2 * bar.GetValue() - 3 == Value)
            {
                return 4;
            }

            return 5;
        }

    }
}
