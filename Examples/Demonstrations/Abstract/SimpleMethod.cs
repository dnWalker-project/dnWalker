
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
        private int _value;

        public int Value => _value;

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
