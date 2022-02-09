namespace dnWalker.Concolic
{
    public interface IExplorationExtension
    {
        void Register(IExplorer explorer);
        void Unregister(IExplorer explorer);
    }
}
