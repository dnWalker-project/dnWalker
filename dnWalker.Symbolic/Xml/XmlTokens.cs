using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Xml
{
    internal static class XmlTokens
    {
        public const string Exploration = nameof(Exploration);
        public const string AssemblyName = nameof(AssemblyName);
        public const string AssemblyFileName = nameof(AssemblyFileName);
        public const string MethodSignature = nameof(MethodSignature);
        public const string Solver = nameof(Solver);
        public const string Start = nameof(Start);
        public const string End = nameof(End);

        public const string Iteration = nameof(Iteration);
        public const string BaseSet = nameof(BaseSet);
        public const string ExecutionSet = nameof(ExecutionSet);

        public const string PathConstraint = nameof(PathConstraint);
        public const string Failed = nameof(Failed);
        public const string Exception = nameof(Exception);
        public const string StandardOutput = nameof(StandardOutput);
        public const string ErrorOutput = nameof(ErrorOutput);

        public const string DateTimeFormat = "yyyy/MM/dd HH:mm:ss.fff";


        public const string Model = nameof(Model);
        public const string InputModel = nameof(InputModel);
        public const string OutputModel = nameof(OutputModel);

        public const string Variables = nameof(Variables);
        public const string Value = nameof(Value);
        public const string MethodArgument = nameof(MethodArgument);
        public const string StaticField = nameof(StaticField);
        public const string ReturnValue = nameof(ReturnValue);

        public const string Name = nameof(Name);
        public const string FieldName = nameof(FieldName);
        public const string Type = nameof(Type);

        public const string IsDirty = nameof(IsDirty);

        public const string Heap = nameof(Heap);

        public const string ObjectNode = nameof(ObjectNode);
        public const string ArrayNode = nameof(ArrayNode);

        public const string InstanceField = nameof(InstanceField);
        public const string DeclaringType = nameof(DeclaringType);
        public const string MethodResult = nameof(MethodResult);
        public const string Method = nameof(Method);
        public const string Invocation = nameof(Invocation);
        public const string ArrayElement = nameof(ArrayElement);
        public const string Index = nameof(Index);

        public const string Location = nameof(Location);
        public const string Length = nameof(Length);

        public const string NegativeInfinity = "-INF";
        public const string PositiveInfinity = "+INF";
        public const string NaN = "NAN";
    }
}
