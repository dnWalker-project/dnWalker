using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{

    public class TypeSystemException : Exception
    {
        public TypeSystemException()
        {
        }

        public TypeSystemException(string message) : base(message)
        {
        }

        public TypeSystemException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TypeSystemException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class TypeException : TypeSystemException
    {
        public TypeException()
        {
        }

        public TypeException(string typeName)
        {
            _typeName = typeName;
        }

        protected TypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _typeName = info.GetString(nameof(TypeName));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(TypeName), _typeName);
        }


        private readonly string? _typeName;

        public override string Message
        {
            get
            {
                return $"Could not find the '{_typeName}'.";
            }
        }

        public string? TypeName
        {
            get
            {
                return _typeName;
            }
        }
    }

    public class TypeNotFoundException : TypeException
    {
        public TypeNotFoundException()
        {
        }

        public TypeNotFoundException(string typeName) : base(typeName)
        {
        }

        protected TypeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class TypeNotSupportForSize : TypeException
    {
        public TypeNotSupportForSize()
        {
        }

        public TypeNotSupportForSize(string typeName) : base(typeName)
        {
        }

        protected TypeNotSupportForSize(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class MemberNotFoundException : TypeException
    {
        public MemberNotFoundException()
        {
        }

        public MemberNotFoundException(string typeName, string memberName) : base(typeName)
        {
            MemberName = memberName;
        }

        protected MemberNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            MemberName = info.GetString(nameof(MemberName));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(MemberName), MemberName);
        }

        public string? MemberName { get; }

        public override string Message
        {
            get
            {
                return $"Could not find the '{MemberName}' on '{TypeName}'";
            }
        }
    }
}
