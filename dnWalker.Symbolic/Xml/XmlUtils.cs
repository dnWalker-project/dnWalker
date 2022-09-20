using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dnWalker.Symbolic.Xml
{
    public class MissingElementException : Exception
    {
        public MissingElementException(string context, string elementName)
        {
            ElementName = elementName;
            Context = context;
        }

        protected MissingElementException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ElementName = info.GetString(nameof(ElementName)) ?? string.Empty;
            Context = info.GetString(nameof(Context)) ?? string.Empty;
        }

        public string ElementName { get; }

        public string Context { get; }

        public override string Message
        {
            get
            {
                return $"'{Context}' XML must contain an '{ElementName}' element.";
            }
        }
    }
    public class MissingAttributeException : Exception
    {
        public MissingAttributeException(string context, string attributeName)
        {
            AttributeName = attributeName;
            Context = context;
        }

        protected MissingAttributeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            AttributeName = info.GetString(nameof(AttributeName)) ?? string.Empty;
            Context = info.GetString(nameof(Context)) ?? string.Empty;
        }

        public string AttributeName { get; }

        public string Context { get; }

        public override string Message
        {
            get
            {
                return $"'{Context}' XML must contain an '{AttributeName}' attribute.";
            }
        }
    }

    public static class XmlUtils
    {
        public static XElement EnsureNotNull(XElement? element, string? name = null)
        {
            if (element == null)
            {
                throw new Exception($"Invalid XML, missing the '{name}' element.");
            }
            return element;
        }
        public static XAttribute EnsureNotNull(XAttribute? attribute, string? name = null)
        {
            if (attribute == null)
            {
                throw new Exception($"Invalid XML, missing the '{name}' attribute.");
            }
            return attribute;
        }

        public static XElement GetElement(XElement parent, string name)
        {
            return EnsureNotNull(parent.Element(name), name);
        }
        public static XAttribute GetAttribute(XElement parent, string name)
        {
            return EnsureNotNull(parent.Attribute(name), name);
        }
    }
}
