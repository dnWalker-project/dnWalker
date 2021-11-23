using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.TestGeneration.Static
{
    public class ComplexArgs
    {
        public static int Return_Sum_Of_IntFields(ClassWithManyFields instance)
        {
            return instance.I1 + instance.I2 + instance.I3 + instance.I4;
        }

        public static ClassWithManyFields Get_ManyFields_If_Any_IntegerField_Matches_Arg(ClassReferencingAnotherClass myObject, int arg)
        {
            if (myObject.IntegerField1 == arg || myObject.IntegerField2 == arg)
            {
                return myObject.ManyFields;
            }
            else
            {
                return null;
            }
        }
    }
}
