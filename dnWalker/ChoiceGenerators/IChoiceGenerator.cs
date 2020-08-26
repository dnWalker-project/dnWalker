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
