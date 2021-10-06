using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic
{
    public class ExplorationException : Exception
    {
        public ExplorationException()
        {
        }

        public ExplorationException(String message) : base(message)
        {
        }

        public ExplorationException(String message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExplorationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class MaxIterationsExceededException : ExplorationException
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
