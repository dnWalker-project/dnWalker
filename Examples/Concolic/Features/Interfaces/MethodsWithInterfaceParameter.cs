using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Concolic.Features.Interfaces
{
    public interface IMyInterface
    {
        int AbstractMethodWithArgs(int a1);

        int AbstractMethod();

        // cannot compile under .net framework
        //public int ConcreteMethod(int b)
        //{
        //    return AbstractMethodWithArgs(b) + b;
        //}
    }

    public class MethodsWithInterfaceParameter
    {
        public static void InvokeInterfaceMethod(IMyInterface instance)
        {
            if (instance == null)
            {
                Console.Out.WriteLine("instance is null");
                return;
            }

            if (instance.AbstractMethod() == 3)
            {
                Console.Out.WriteLine("instance.AbstractMethod()|0| == 3");
            }
            else
            {
                Console.Out.WriteLine("instance.AbstractMethod()|0| != 3");
            }


            if (instance.AbstractMethod() > 134)
            {
                Console.Out.WriteLine("instance.AbstractMethod()|1| > 134");
            }
            else
            {
                Console.Out.WriteLine("instance.AbstractMethod()|1| <= 134");
            }
        }
    }
}
