using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Explorations.Xml
{
    internal static class XmlTokens
    {
        public const string Exploration = "Exploration";
        public const string AssemblyName = "AssemblyName";
        public const string AssemblyFileName = "AssemblyFileName";
        public const string MethodSignature = "MethodSignature";
        public const string Solver = "Solver";
        public const string Start = "Start";
        public const string End = "End";

        public const string Iteration = "Iteration";
        public const string BaseSet = "BaseSet";
        public const string ExecutionSet = "ExecutionSet";

        public const string DateTimeFormat = "yyyy/MM/dd HH:mm:ss";

        public const string PathConstraint = "PathConstraint";
        public const string Failed = "Failed";
        public const string Exception = "Exception";
        public const string StandardOutput = "StandardOutput";
        public const string ErrorOutput = "ErrorOutput";
    }
}
