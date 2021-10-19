using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Parameters
{
    public static class ParameterName
    {
        public static readonly String Delimiter = ":";
        public static readonly String CallIndexDelimiter = "|";

        public static String GetAccessor(String baseName, String fullName)
        {
            if (baseName == null) throw new ArgumentNullException(nameof(baseName));
            if (fullName == null) throw new ArgumentNullException(nameof(fullName));

            if (!fullName.StartsWith(baseName)) throw new ArgumentException("baseName must be beginning of the fullName");

            // baseName: ROOT_OBJECT:SUB_OBJECT_1:...:SUB_OBJECT_N
            // fullName: ROOT_OBJECT:SUB_OBJECT_1:...:SUB_OBJECT_N:FIELD:SUB_SUB_OBJECT_1:SUB_SUB_OBJECT_2:...

            String[] delimiters = new String[] { Delimiter };

            String restOfFullName;

            if (baseName.Length == 0)
            {
                restOfFullName = fullName;
            }
            else
            {
                restOfFullName = fullName.Substring(baseName.Length);
                if (!restOfFullName.StartsWith(Delimiter) && baseName.Length > 0)
                {
                    throw new ArgumentException("baseName must be beginning of the fullName");
                }
                restOfFullName = restOfFullName.Substring(Delimiter.Length);
            }

            Int32 idx = restOfFullName.IndexOf(Delimiter, Delimiter.Length);

            if (idx < 0)
            {
                // no new delimiter
                return restOfFullName;
            }
            else
            {
                return restOfFullName.Substring(0, idx);
            }


            //Int32 fieldNameStartIndex = baseName.Length;
            //Int32 filedNameEndIndex = fullName.IndexOf(Delimiter, fieldNameStartIndex + 1);

            //if (fieldNameStartIndex > 0) fieldNameStartIndex += Delimiter.Length;

            //return fullName.Substring(fieldNameStartIndex, filedNameEndIndex - fieldNameStartIndex);
        }

        //public static String GetMethod(String baseName, String fullName)
        //{
        //    return GetAccessor(baseName, fullName);
        //}

        public static String ConstructField(String baseName, String fieldName)
        {
            if (String.IsNullOrWhiteSpace(baseName)) throw new ArgumentNullException(nameof(baseName));
            if (String.IsNullOrWhiteSpace(fieldName)) throw new ArgumentNullException(nameof(fieldName));

            return baseName + Delimiter + fieldName;
        }

        public static String ConstructMethod(String baseName, String methodName, Int32 callIndex)
        {
            if (String.IsNullOrWhiteSpace(baseName)) throw new ArgumentNullException(nameof(baseName));
            if (String.IsNullOrWhiteSpace(methodName)) throw new ArgumentNullException(nameof(methodName));
            if (callIndex < 1) throw new ArgumentException("CallIndex must be a positive value.", nameof(callIndex));

            return ConstructField(baseName, methodName) + CallIndexDelimiter + callIndex.ToString();
        }

        public static String ConstructIndex(String baseName, Int32 index)
        {
            if (String.IsNullOrWhiteSpace(baseName)) throw new ArgumentNullException(nameof(baseName));

            return ConstructField(baseName, index.ToString());
        }

        public static String GetRootName(String fullName)
        {
            if (String.IsNullOrWhiteSpace(fullName)) throw new ArgumentNullException(nameof(fullName));

            Int32 index = fullName.IndexOf(Delimiter);
            if (index < 0)
            {
                return fullName;
            }

            return fullName.Substring(0, index);
        }

        public static Boolean TryParseMethodName(String methodNameWithCallIndex, out String methodName, out Int32 callIndex)
        {
            if (String.IsNullOrWhiteSpace(methodNameWithCallIndex) ||
                methodNameWithCallIndex.Contains(Delimiter))
            {
                methodName = null;
                callIndex = 0;
                return false;
            }

            Int32 i = methodNameWithCallIndex.IndexOf(CallIndexDelimiter);
            if (i > 0 && Int32.TryParse(methodNameWithCallIndex.Substring(i + 1), out callIndex))
            {
                methodName = methodNameWithCallIndex.Substring(0, i);
                return true;
            }

            methodName = null;// methodNameWithCallIndex.Substring(0, i);
            callIndex = -1;
            return false;
        }
    }
}
