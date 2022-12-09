using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Examples.Demonstrations.Conrete
{
    public class Segment
    {
        private Segment _next;
        private int[] _data;

        public Segment Next
        {
            get
            {
                return _next;
            }
            set
            {
                _next = value;
            }
        }

        public int[] Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        public void Foo(Segment other, int x)
        {
            if (x == 5 && _data != null)
            {
                if (_data.Length >= 4)
                {
                    _data[3] = x;
                }
            }
        }

        public void Append(int[] data)
        {
            Segment s = this;
            while (s._next != null) 
            {
                s = s._next;
            }

            s.Insert(data);
        }

        public void Insert(int[] data)
        {
            _next = new Segment() 
            {
                _data = data, 
                _next = _next 
            };
        }

        public int Count()
        {
            int cnt = _data?.Length ?? 0;
            if (_next != null)
            {
                cnt += _next.Count();
            }
            return cnt;
        }
    }
}
