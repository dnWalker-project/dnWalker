using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.TestGeneration.Instance
{
    public class PrimitiveArgs
    {
        private int _value;

        public int Value
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

        public int Double()
        {
            return 2 * _value;
        }

        public void WriteValueToOutput()
        {
            System.Console.WriteLine(_value);
        }

        public int Add(int v)
        {
            return _value + v;
        }

        public void WriteToConsoleIfGreaterThanValue(int v)
        {
            if (v > _value)
            {
                Console.WriteLine("v >  value");
            }
            else
            {
                Console.WriteLine("v <= value");
            }
        }
    }
}
