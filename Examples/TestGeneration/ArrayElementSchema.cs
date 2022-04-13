using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.TestGeneration
{
    public class ArrayElementSchema
    {
        private int[] _valArray;
        private ArrayElementSchema[] _refArray;
        private int _id;

        public static void PrimitiveArrayAsMethodArgument(int[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (array.Length == 0) throw new ArgumentException(nameof(array));
            array[0] = 10;
        }

        public static void PrimitiveArrayAsFieldValue(ArrayElementSchema instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (instance._valArray == null) throw new ArgumentException(nameof(instance));
            if (instance._valArray.Length == 0) throw new ArgumentException(nameof(instance));

            instance._valArray[0] = 15;
        }

        public static void ReferenceArrayAsMethodArgument_Input(ArrayElementSchema[] array, ArrayElementSchema instance)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (array.Length == 0) throw new ArgumentException(nameof(array));
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            array[0] = instance;
        }

        public static void ReferenceArrayAsMethodArgument_Null(ArrayElementSchema[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (array.Length == 0) throw new ArgumentException(nameof(array));

            array[0] = null;
        }

        public static void ReferenceArrayAsMethodArgument_Fresh(ArrayElementSchema[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (array.Length == 0) throw new ArgumentException(nameof(array));

            array[0] = new ArrayElementSchema() { _id = 5 };
        }
    }
}
