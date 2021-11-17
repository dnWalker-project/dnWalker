using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.SUT.Instance
{
    public class ObjectAndPrimitiveArgs
    {
        public class Item
        {
            private int _id;
            private double _itemValue;

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

            public double ItemValue
            {
                get
                {
                    return _itemValue;
                }

                set
                {
                    _itemValue = value;
                }
            }
        }


        private double _rate;
        public double Rate
        {
            get
            {
                return _rate;
            }

            set
            {
                _rate = value;
            }
        }

        public double IfCorrectIdMultiplyValue(Item item, int id)
        {
            if (item == null)
            {
                Console.WriteLine("ERROR! null reference.");
                return double.NaN;
            }

            if (item.Id == id)
            {
                return item.ItemValue * _rate;
            }
            else
            {
                return item.ItemValue;
            }
        }
    }
}
