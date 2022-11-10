using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Configuration
{
    public interface IConfiguration
    {
        TValue GetValue<TValue>(string key);
        void SetValue<TValue>(string key, TValue value);
    }
}
