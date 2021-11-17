using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.SUT.Instance
{

    public class ObjectArgs
    {
        public class Item
        {
            private int _id;
            private double _value;

            public int Id
            {
                get
                {
                    return _id;
                }

                set
                {
                    _id = value;
                }
            }

            public double Value
            {
                get
                {
                    return _value;
                }

                set
                {
                    _value = value;
                }
            }
        }


        private double _value;

        public double Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        public double GetItemValuePlusValue(Item item)
        {
            if (item == null)
            {
                Console.WriteLine("ERROR! null reference.");
                return double.NaN;
            }

            return item.Value + _value;
        }

        public double GetItemValueIfCorrectId(Item item)
        {
            if (item == null)
            {
                Console.WriteLine("ERROR! null reference.");
                return double.NaN;
            }

            if (item.Id > 1)
            {
                return item.Value;
            }
            else
            {
                return double.NaN;
            }

        }

        public double GetMaxValue(Item item)
        {
            if (item == null)
            {
                Console.WriteLine("ERROR! null reference.");
                return double.NaN;
            }

            if (item.Value > _value)
            {
                return item.Value;
            }
            else
            {
                return _value;
            }
        }
    }
}
