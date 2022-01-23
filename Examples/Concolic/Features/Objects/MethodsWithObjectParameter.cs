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

        public void SetMyFieldValue(int myField)
        {
            _myField = myField;
        }

        public double OtherField;
    }


    public abstract class AbstractClass
    {
        private int _field;


        public int ConcreteMethod(int add)
        {
            return _field + add;
        }

        public abstract TestClass AbstractMethod();
    }


    public class MethodsWithObjectParameter
    {
        public static void DirectFieldAccess(TestClass instance)
        {
            if (instance == null)
            {
                Console.Out.WriteLine("instance is null");
                return;
            }

            if (instance.OtherField != 10)
            {
                Console.Out.WriteLine("instance.OtherField != 10");
                return;
            }
            else
            {
                Console.Out.WriteLine("instance.OtherField == 10");
                return;
            }
        }

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

        public static void BranchIfNull(TestClass instance)
        {
            if (instance == null)
            {
                Console.Out.WriteLine("instance is null");
            }
            else
            {
                Console.Out.WriteLine("instance is not null");
            }
        }

        public static void BranchIfNotNull(TestClass instance)
        {
            if (instance != null)
            {
                Console.Out.WriteLine("instance is not null");
            }
            else
            {
                Console.Out.WriteLine("instance is null");
            }
        }

        public static void AbstractClass_ConcreteMethod(AbstractClass instance)
        {
            if (instance == null)
            {
                Console.Out.WriteLine("instance is null");
                return;
            }

            if (instance.ConcreteMethod(5) < 10)
            {
                Console.Out.WriteLine("instance.ConcreteMethod() < 10");
            }
            else
            {
                Console.Out.WriteLine("instance.ConcreteMethod() >= 10");
            }
        }

        public static void AbstractClass_AbstractMethod(AbstractClass instance)
        {
            if (instance == null)
            {
                Console.Out.WriteLine("instance is null");
                return;
            }

            TestClass testInstance = instance.AbstractMethod();

            if (testInstance == null)
            {
                Console.Out.WriteLine("instance.AbstractMethod()|0| is null");
                return;
            }

            if (testInstance.OtherField < 5)
            {
                Console.Out.WriteLine("instance.AbstractMethod()|0|.OtherField < 5");
            }
            else
            {
                Console.Out.WriteLine("instance.AbstractMethod()|0|.OtherField >= 5");
            }


            testInstance = instance.AbstractMethod();

            if (testInstance == null)
            {
                Console.Out.WriteLine("instance.AbstractMethod()|1| is null");
                return;
            }
            if (testInstance.OtherField < 6)
            {
                Console.Out.WriteLine("instance.AbstractMethod()|1|.OtherField < 5");
            }
            else
            {
                Console.Out.WriteLine("instance.AbstractMethod()|1|.OtherField >= 5");
            }
        }

        public static int ArgumentIdentityComparer(TestClass obj1, TestClass obj2)
        {
            if (obj1 == null || obj2 == null)
            {
                return -1;
            }

            if (obj1 == obj2)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static void EditFieldsBasedOnArgumentIdenity(TestClass obj1, TestClass obj2)
        {
            int identical = ArgumentIdentityComparer(obj1, obj2);

            if (identical < 0)
            {
                Console.WriteLine("obj1 or obj2 is null");
                return;
            }
            else 
            {
                Console.WriteLine("obj1 and obj2 != null");


                if (obj1.OtherField < 3)
                {
                    Console.WriteLine("obj1.OtherField < 3");
                }

                if (identical == 1)
                {
                    Console.WriteLine("obj1 == obj2");
                }
            }

        }
    }
}