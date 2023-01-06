namespace dnWalker.Concolic.Traversal
{
    public interface IConstraintFilter
    {
        void StartIteration(int i);
        
        bool UseConstraint(ConstraintNode node);
    }
}