namespace dnWalker.Symbolic.Heap
{
    /// <summary>
    /// Represents a read only array heap node.
    /// </summary>
    public interface IReadOnlyArrayHeapNode : IReadOnlyHeapNode
    {
        /// <summary>
        /// Gets the value of the element located at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IValue GetElement(int index);

        IEnumerable<int> Indeces { get; }

        int Length { get; }
    }
}
