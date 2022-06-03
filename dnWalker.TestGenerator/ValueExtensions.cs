using dnWalker.Symbolic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator
{
    public static class ValueExtensions
    {
        public static string GetLiteral(this IValue value, IDictionary<Location, string>? locationNames = null)
        {
            string result;
            if (value is Location location)
            {
                if (location == Location.Null)
                {
                    result = "null";
                }
                else if (locationNames == null || !locationNames.TryGetValue(location, out result))
                {
                    result = $"LOCATION_{location.Value:X8}";
                }
            }
            else
            {
                result = value.ToString()!;
            }
            return result;
        }
    }
}
