using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Concolic.MethodInvoking
{
    public interface IPureValueProvider
    {
        int GetIntValue();
        double GetDoubleValue();
        //string GetStringValue();
    }

    public interface INotPureValueProvider
    {
        int GetIntValue(int x);
        double GetDoubleValue(int x);
        //string GetStringValue(int x);
    }

    public class InterfaceMethodInvoking
    {
        public static void BranchingBasedOnPureValueProvider(IPureValueProvider valueProvider)
        {
            if (valueProvider.GetIntValue() <= 5)
            {
                Console.Out.WriteLine("valueProvider.GetIntValue() <= 5");
            }
            else
            {
                Console.Out.WriteLine("valueProvider.GetIntValue() > 5");
            }
        }
        public static void BranchingBasedOnUnpureValueProvider(INotPureValueProvider valueProvider, int p)
        {
            if (valueProvider.GetIntValue(p) <= 5)
            {
                Console.Out.WriteLine("valueProvider.GetIntValue(p) <= 5");
            }
            else
            {
                Console.Out.WriteLine("valueProvider.GetIntValue(p) > 5");
            }
        }
    }
}
