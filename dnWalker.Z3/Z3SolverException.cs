using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Z3
{
    public class Z3SolverException : Exception
    {
        public Z3SolverException()
        {
        }

        public Z3SolverException(string? message) : base(message)
        {
        }

        public Z3SolverException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected Z3SolverException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class UnsupportedExpressionException : Z3SolverException
    {
        private readonly string? _expression;

        public UnsupportedExpressionException(string? expression)
        {
            _expression = expression;
        }

        public UnsupportedExpressionException(string? expression, string? message) : base(message)
        {
            _expression = expression;
        }

        public UnsupportedExpressionException(string? expression, string? message, Exception? innerException) : base(message, innerException)
        {
            _expression = expression;
        }

        protected UnsupportedExpressionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _expression = info.GetString("expression");
        }

        public string? Expression
        {
            get
            {
                return _expression;
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("expression", _expression, typeof(string));
        }
    }
}
