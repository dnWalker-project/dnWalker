using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public class StringParameter : ArrayParameter
    {
        public StringParameter(string localName) : base(TypeNames.CharTypeName, localName)
        {
        }

        public StringParameter(string localName, Parameter parent) : base(TypeNames.CharTypeName, localName, parent)
        {
        }

        public string? CreateString()
        {
            if (IsNull) return null;

            char[] charArray = new char[Length];
            for (int i = 0; i < charArray.Length; i++)
            {
                if (TryGetItem(i, out Parameter? p ) && p is CharParameter c)
                {
                    charArray[i] = c.Value;
                }
                charArray[i] = 'c'; // we do not care what is at this location...
            }

            return new string(charArray);
        }
    }
}
