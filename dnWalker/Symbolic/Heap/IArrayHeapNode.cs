namespace dnWalker.Symbolic.Heap
{
    /// <summary>
    /// Represents a writable array heap node.
    /// </summary>
    public interface IArrayHeapNode : IHeapNode, IReadOnlyArrayHeapNode
    {

        void SetElement(int index, IValue value);
    }
}
