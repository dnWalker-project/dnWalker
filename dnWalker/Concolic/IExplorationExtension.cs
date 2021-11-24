namespace dnWalker.Concolic
{
    public interface IExplorationExtension
    {
        void Register(Explorer explorer);
        void Unregister(Explorer explorer);
    }
}
