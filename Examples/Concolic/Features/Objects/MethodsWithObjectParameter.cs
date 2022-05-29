﻿using System;
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

        public TestClass[] RefArray;
        public int[] PrimitiveArray;
        public TestClass TCField;
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

        public static void InvokeMethodWithFieldAccessWithoutNullCheck(TestClass instance)
        {
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

        public static void InvokeConcreteMethodOnAbstractClass(AbstractClass instance)
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

        public static void InvokeAbstractMethodOnAbstractClass(AbstractClass instance)
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
                Console.Out.WriteLine("obj1 or obj2 is null");
                return;
            }
            else 
            {
                Console.Out.WriteLine("obj1 and obj2 != null");


                if (obj1.OtherField < 3)
                {
                    Console.Out.WriteLine("obj1.OtherField < 3");
                }

                if (identical == 1)
                {
                    Console.Out.WriteLine("obj1 == obj2");
                }
            }

        }

        public static void SetFieldToInputPrimitive(TestClass obj, double value)
        {
            if (value < 5.5)
            {
                Console.Out.WriteLine("Value too low...");
                return;
            }

            obj.OtherField = value;
        }
        public static void SetFieldToInputObject(TestClass obj, TestClass value)
        {
            if (value == null)
            {
                Console.Out.WriteLine("Value is null");
                return;
            }

            obj.TCField = value;
        }

        public static void SetFieldToFreshPrimitive(TestClass obj)
        {
            obj.OtherField = 3.14;
        }

        public static void SetFieldToFreshObject(TestClass obj)
        {
            obj.TCField = new TestClass();
        }

        public static void SetFieldToFreshPrimitiveArray(TestClass obj, int i)
        {
            if (i < 10)
            {
                Console.Out.WriteLine("Value too low...");
                return;
            }

            obj.PrimitiveArray = new int[] {i, i - 1, i + 1};
        }

        public static void SetFieldToFreshRefArray(TestClass obj)
        {
            obj.RefArray = new TestClass[] { obj, null, new TestClass() };
        }

        public static void SetFieldToNull(TestClass obj)
        {
            obj.PrimitiveArray = null;
            obj.RefArray = null;
            obj.TCField = null;
        }
    }
}