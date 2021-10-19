using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic
{
    public class ExplorerException : Exception
    {
        public ExplorerException()
        {
        }

        public ExplorerException(String message) : base(message)
        {
        }

        public ExplorerException(String message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExplorerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class MaxIterationsExceededException : ExplorerException
    {
        //public MaxIterationsExceededException()
        //{
        //}

        //public MaxIterationsExceededException(String message) : base(message)
        //{
        //}

        //public MaxIterationsExceededException(String message, Exception innerException) : base(message, innerException)
        //{
        //}

        public MaxIterationsExceededException(Int32 iterationCount) : base("Max iterations exceeded (" + iterationCount + ")")
        {
        }

        protected MaxIterationsExceededException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
