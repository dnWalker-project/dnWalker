using MMC;
using MMC.Collections;
using MMC.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker
{
    internal class StateStorage : FastHashtable<CollapsedState, int>
    {
        public StateStorage(int power) : base(power)
        {
        }

        public StateEventHandler StateRevisited { get; set; }
        public StateEventHandler StateConstructed { get; set; }
    }
}
