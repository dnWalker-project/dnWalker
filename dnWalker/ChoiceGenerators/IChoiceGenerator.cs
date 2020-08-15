using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMC.State;

namespace dnWalker.ChoiceGenerators
{
    public interface IChoiceGenerator
    {
        object GetNextChoice();

        bool HasMoreChoices { get; }

        IChoiceGenerator Previous { get; set; }

        void SetContext(ExplicitActiveState activeState);

        SchedulingData GetBacktrackData();
    }   
}
