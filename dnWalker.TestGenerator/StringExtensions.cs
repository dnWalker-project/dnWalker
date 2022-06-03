using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator
{
    public static class StringExtensions
    {
        public static string FirstCharToLower(this string str)
        {
            return string.Create(str.Length, str, static (span, str) =>
            {
                span[0] = char.ToLower(str[0]);
                str.AsSpan(1).CopyTo(span.Slice(1));
            });
        }
    }
}
