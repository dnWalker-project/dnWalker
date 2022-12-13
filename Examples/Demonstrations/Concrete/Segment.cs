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

        public Segment Delete(int[] data)
        {
            if (data == Data)
            {
                // both the current data and the data to delete are null
                // delete self => return Next
                return Next;
            }

            if (Data != null && data != null &&
                Data.Length == data.Length)
            {
                bool equals = true;
                for (int i = 0; i < Data.Length; ++i) 
                {
                    equals = Data[i] == data[i];
                    if (!equals)
                    {
                        break;
                    }
                }

                if (equals)
                {
                    // the data sequences are equal
                    // delete self => return Next
                    return Next;
                }
            }

            // pass the deletion to the next, if exists
            Next = Next?.Delete(data);
            return this;
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
