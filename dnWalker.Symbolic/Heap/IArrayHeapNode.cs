namespace dnWalker.Symbolic.Heap
{
    /// <summary>
    /// Represents a writable array heap node.
    /// </summary>
    public interface IArrayHeapNode : IHeapNode, IReadOnlyArrayHeapNode
    {
        new int Length { get; set; }

        int IReadOnlyArrayHeapNode.Length
        {
            get
            {
                return Length;
            }
        }

        void SetElement(int index, IValue value);
    }
}
