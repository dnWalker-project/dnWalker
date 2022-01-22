using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public static class ObjectPool<T>
        where T : class, new()
    {
        private static readonly Queue<T> _available = new Queue<T>();

        public static T Rent()
        {
            if (_available.Count == 0)
            {
                T retVal = new T();
                return retVal;
            }

            return _available.Dequeue();
        }

        public static void Return(T value)
        {
            _available.Enqueue(value);
        }
    }
}
