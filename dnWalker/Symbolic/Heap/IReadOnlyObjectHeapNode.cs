using dnlib.DotNet;

namespace dnWalker.Symbolic.Heap
{
    /// <summary>
    /// Represents a read only object heap node.
    /// </summary>
    public interface IReadOnlyObjectHeapNode : IReadOnlyHeapNode
    {
        /// <summary>
        /// Gets the value of the specified field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        IValue GetField(IField field);

        /// <summary>
        /// Gets the value of the method result.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        IValue GetMethodResult(IMethod method, int invocation);
    }
}
