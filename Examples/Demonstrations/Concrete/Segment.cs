using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Examples.Demonstrations.Concrete
{
    public class Segment
    {
        public Segment Next;

        public int[] Data;

        public int Foo(Segment other, int x)
        {
            if (x == 5 && Data != null)
            {
                if (Data.Length >= 4)
                {
                    Data[3] = x;
                    return 0;
                }
                return 1;
            }
            return 2;
        }

        public void Append(int[] data)
        {
            Segment s = this;
            while (s.Next != null) 
            {
                s = s.Next;
            }

            s.Insert(data);
        }

        public void Insert(int[] data)
        {
            Next = new Segment() 
            {
                Data = data, 
                Next = Next 
            };
        }

        public int Count()
        {
            int cnt = Data?.Length ?? 0;
            if (Next != null)
            {
                cnt += Next.Count();
            }
            return cnt;
        }
    }
}
