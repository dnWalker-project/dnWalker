using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters.Xml
{
    public static class XmlTokens
    {
        public const string XmlObject = "Object";
        public const string XmlArray = "Array";
        public const string XmlType = "Type";
        public const string XmlPrimitiveValue = "PrimitiveValue";
        public const string XmlStruct = "Struct";

        public const string XmlReference = "Reference";
        public const string XmlValue = "Value";
        public const string XmlElementType = "ElementType";
        public const string XmlIndex = "Index";
        public const string XmlItem = "Item";
        public const string XmlLength = "Length";
        public const string XmlIsNull = "IsNull";
        public const string XmlName = "Name";
        public const string XmlField = "Field";
        public const string XmlMethodResult = "MethodResult";
        public const string XmlMethodSignature = "Signature";
        public const string XmlInvocation = "Invocation";
        public const string XmlUnknown = "Unknown";
        public const string XmlParameterContext = "ParameterContext";

        public const string XmlAccessor = "Accessor";
        public const string XmlNoAccessor = "None";
        public const string XmlMethodArgumentRoot = "MethodArgument";
        public const string XmlStaticFieldRoot = "StaticField";

        public const string XmlFullName = "FullName";
    }
}
