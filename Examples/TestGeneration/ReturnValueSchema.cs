using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.TestGeneration
{
    public class ReturnValueSchema
    {
        public int ValueField;
        public ReturnValueSchema RefField;

        public static int PrimitiveValue(int input)
        {
            if (input < 0)
            {
                return input;
            }
            else
            {
                return input + 10;
            }
        }

        public static int[] ArrayOfPrimitives(int[] array)
        {
            if (array == null || array.Length == 0)
            {
                int[] ret = new int[6];
                ret[0] = 0;
                ret[1] = 1;
                ret[2] = 2;
                ret[3] = 3;
                ret[4] = 4;
                ret[5] = 5;
                return ret;
            }
            array[0] = 10;
            return array;
        }

        public static ReturnValueSchema ReferenceTypeValue(ReturnValueSchema instance)
        {
            if (instance == null)
            {
                return new ReturnValueSchema()
                {
                    ValueField = 10
                };
            }

            if (instance.ValueField == 5)
            {
                return null;
            }

            instance.RefField = instance;
            instance.ValueField = -1;

            return instance;
        }


        public static ReturnValueSchema[] ReferenceTypeArrayValue(ReturnValueSchema[] array)
        {
            if (array == null || array.Length == 0)
            {
                return new ReturnValueSchema[] { new ReturnValueSchema() { ValueField = 10 }, null, new ReturnValueSchema() { ValueField = 5 }, null };
            }

            if (array.Length == 2)
            {
                return null;
            }

            array[0] = new ReturnValueSchema() { ValueField = -1 };
            return array;
        }
    }
}
