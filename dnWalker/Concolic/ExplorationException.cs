﻿using System;
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

        public ExplorationException(string message) : base(message)
        {
        }

        public ExplorationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExplorationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class MaxIterationsExceededException : ExplorationException
    {
        public MaxIterationsExceededException(int iterationCount) : base("Max iterations exceeded (" + iterationCount + ")")
        {
        }

        protected MaxIterationsExceededException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
