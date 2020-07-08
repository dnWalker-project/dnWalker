using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Traversal
{
    public class Segment
    {
        public Segment(int fromState, int toState)
        {
            FromState = fromState;
            ToState = toState;
        }

        public int FromState { get; }
        public int ToState { get; }

        public bool Terminal { get; set; }
    }
}
