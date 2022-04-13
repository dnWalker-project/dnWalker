using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.TestGeneration
{
    public class FieldSchema
    {
        private int _primitiveValueField;
        private FieldSchema _refField;

        public static void PrimitiveValueChanged_Fresh(FieldSchema instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            instance._primitiveValueField = 15;
        }

        public static void PrimitiveValueChanged_Input(FieldSchema instance, int value)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            instance._primitiveValueField = value;
        }

        public static void RefFieldChanged_Input(FieldSchema instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            instance._refField = instance;
        }

        public static void RefFieldChanged_Fresh(FieldSchema instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            instance._refField = new FieldSchema();
        }

        public static void RefFieldChanged_Null(FieldSchema instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            if (instance._refField != null) instance._refField = null;
        }
    }
}
