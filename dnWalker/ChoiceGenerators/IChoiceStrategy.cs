using MMC.State;

namespace dnWalker.ChoiceGenerators
{
    public interface IChoiceStrategy
    {
        bool HasOptions { get; }
        IChoiceGenerator ChoiceGenerator { get; }
        IChoiceGenerator Back();
        void DoBacktrack(out bool @continue);
        bool CanBreak();
        void RegisterChoiceGenerator(IChoiceGenerator choiceGenerator);
        ThreadState GetExecutingThread();
    }
}
