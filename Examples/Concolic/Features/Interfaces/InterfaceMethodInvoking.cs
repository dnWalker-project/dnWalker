using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Concolic.Features.Interfaces
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
            if (valueProvider == null)
            {
                Console.Out.WriteLine("valueProvider == null");
                return;
            }
            else
            {
                Console.Out.WriteLine("valueProvider != null");
            }

            if (valueProvider.GetIntValue() <= 5)
            {
                Console.Out.WriteLine("valueProvider.GetIntValue() <= 5");
            }
            else
            {
                Console.Out.WriteLine("valueProvider.GetIntValue() > 5");
            }

            if (valueProvider.GetIntValue() < 2)
            {

            }
        }
        public static void BranchingBasedOnUnpureValueProvider(INotPureValueProvider valueProvider, int p)
        {
            if (valueProvider == null)
            {
                Console.Out.WriteLine("valueProvider == null");
                return;
            }
            else
            {
                Console.Out.WriteLine("valueProvider != null");
            }

            if (valueProvider.GetIntValue(p) <= 5)
            {
                Console.Out.WriteLine("valueProvider.GetIntValue(p) <= 5");
            }
            else
            {
                Console.Out.WriteLine("valueProvider.GetIntValue(p) > 5");
            }
        }

        public static void MethodInvokedMultipleTimes(IPureValueProvider valueProvider)
        {
            if (valueProvider == null)
            {
                Console.Out.WriteLine("valueProvider == null");
                return;
            }

            var value1 = valueProvider.GetDoubleValue();
            var value2 = valueProvider.GetDoubleValue();

            if (value1 >= value2)
            {
                Console.Out.WriteLine("value1 >= value2");
            }
            else
            {
                Console.Out.WriteLine("value1 < value2");
            }
        }
    }
}
