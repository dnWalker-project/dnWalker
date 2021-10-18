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
            // baseName: ROOT_OBJECT:SUB_OBJECT_1:...:SUB_OBJECT_N
            // fullName: ROOT_OBJECT:SUB_OBJECT_1:...:SUB_OBJECT_N:FIELD:SUB_SUB_OBJECT_1:SUB_SUB_OBJECT_2:...

            Int32 fieldNameStartIndex = baseName.Length;
            Int32 filedNameEndIndex = fullName.IndexOf(Delimiter, fieldNameStartIndex + 1);

            return fullName.Substring(fieldNameStartIndex, filedNameEndIndex - fieldNameStartIndex);
        }

        //public static String GetMethod(String baseName, String fullName)
        //{
        //    return GetAccessor(baseName, fullName);
        //}

        public static String ConstructField(String baseName, String fieldName)
        {
            return baseName + Delimiter + fieldName;
        }

        public static String ConstructMethod(String baseName, String methodName, Int32 callIndex)
        {
            return ConstructField(baseName, methodName) + CallIndexDelimiter + callIndex.ToString();
        }

        public static String ConstructIndex(String baseName, Int32 index)
        {
            return ConstructField(baseName, index.ToString());
        }

        public static String GetRootName(String fullName)
        {
            Int32 index = fullName.IndexOf(Delimiter);
            if (index < 0)
            {
                return fullName;
            }

            return fullName.Substring(0, index);
        }

        public static Boolean TryParseMethodName(String methodNameWithCallIndex, out String methodName, out Int32 callIndex)
        {
            Int32 i = methodNameWithCallIndex.IndexOf(CallIndexDelimiter);
            if (i > 0 && Int32.TryParse(methodNameWithCallIndex.Substring(i + 1), out callIndex))
            {
                methodName = methodNameWithCallIndex.Substring(0, i);
                return true;
            }

            methodName = methodNameWithCallIndex.Substring(0, i);
            callIndex = -1;
            return false;
        }
    }
}
