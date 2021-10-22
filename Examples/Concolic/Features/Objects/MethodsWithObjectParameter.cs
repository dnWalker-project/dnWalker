using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Concolic.Features.Objects
{
    public class TestClass
    {
        private int _myField;

        public int GetMyFieldValue()
        {
            return _myField;
        }

    }


    public class MethodsWithObjectParameter
    {
        public static void InvokeMethodWithFieldAccess(TestClass instance)
        {
            if (instance == null) return;

            if (instance.GetMyFieldValue() == 3)
            {
                Console.Out.WriteLine("instance:_myField == 3");
            }
            else
            {
                Console.Out.WriteLine("instance:_myField != 3");
            }
        }
    }
}