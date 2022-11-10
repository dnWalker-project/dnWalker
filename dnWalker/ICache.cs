using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker
{
    public interface ICache<TKey, TValue>
        where TKey : notnull
    {
        public TValue Get(TKey key);
        public void Clear();
    }
}
