using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using System.Xml.Linq;

namespace dnWalker.Input.Xml
{
    internal static class XmlTokens
    {
        public const string SharedData = nameof(SharedData);
        public const string UserModels = nameof(UserModels);
        public const string UserModel = nameof(UserModel);

        public const string Reference = nameof(Reference);
        public const string Object = nameof(Object);
        public const string Array = nameof(Array);
        public const string Literal = nameof(Literal);

        public const string Id = nameof(Id);
        public const string Type = nameof(Type);

        public const string Length = nameof(Length);
        public const string Element = nameof(Element);
        public const string ElementType = nameof(ElementType);
        public const string Index = nameof(Index);

        public const string Member = nameof(Member);
        public const string Name = nameof(Name);
        public const string Invocation = nameof(Invocation);

        public const string Argument = nameof(Argument);
        public const string StaticMember = nameof(StaticMember);

        public const string EntryPoint = nameof(EntryPoint);
    }
}
