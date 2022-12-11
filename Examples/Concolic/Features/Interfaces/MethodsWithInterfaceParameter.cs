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
        public static void InvokeInterfaceMethodWithArgs(IMyInterface instance)
        {
            if (instance == null)
            {
                Console.Out.WriteLine("instance is null");
                return;
            }

            if (instance.AbstractMethodWithArgs(3) == 5) 
            {
                Console.Out.WriteLine("instance.AbstractMethodWithArgs == 5, because the argument is less than 5");
            }

            if (instance.AbstractMethodWithArgs(6) == 10)
            {
                Console.Out.WriteLine("instance.AbstractMethodWithArgs == 10, because the argument is greater than or equal to 5");
            }
        }
        public static void InvokeInterfaceMethodWithArgsDynamic(IMyInterface instance, int x)
        {
            if (instance == null)
            {
                Console.Out.WriteLine("instance is null");
                return;
            }

            int r = instance.AbstractMethodWithArgs(x);
            if (r == 5)
            {
                Console.Out.WriteLine("instance.AbstractMethodWithArgs == 5, because the argument is less than 5");
            }
            else if (r == 10)
            {
                Console.Out.WriteLine("instance.AbstractMethodWithArgs == 10, because the argument is greater than or equal to 5");
            }
            else
            {
                Console.Out.WriteLine("And this should never have happened.");
            }
        }
    }
}
