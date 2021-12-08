using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public class StringParameter : ArrayParameter, IStringParameter
    {
        public StringParameter() : base(TypeNames.CharTypeName)
        {
        }

        public StringParameter(int id) : base(TypeNames.CharTypeName, id)
        {
        }

        public string? CreateString()
        {
            if (IsNull) return null;

            char[] charArray = new char[Length];
            for (int i = 0; i < charArray.Length; i++)
            {
                if (TryGetItem(i, out IParameter? p ) && p is ICharParameter c)
                {
                    charArray[i] = c.Value;
                }
                charArray[i] = 'c'; // we do not care what is at this location...
            }

            return new string(charArray);
        }
    }
}
