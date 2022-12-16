
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Examples.Demonstrations.AbstractData
{
    public interface IBar
    {
        int GetValue();

        int GetValueWithArgs(double x, double y);
    }

    public class SimpleMethod
    {
        public int Value;

        public int MethodNoArgs(IBar bar)
        {
            if (2 * bar.GetValue() - 3 == Value)
            {
                return 4;
            }

            return 5;
        }

        public int MethodWithArgs(IBar bar, double x, double y) 
        {
            if (bar.GetValueWithArgs(x, y) != Value)
            {
                return 1;
            }
            return 0;
        }
    }
}
