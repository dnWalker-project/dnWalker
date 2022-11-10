using dnlib.DotNet;

namespace dnWalker.Symbolic.Heap
{
    /// <summary>
    /// Represents a writable object heap node.
    /// </summary>
    public interface IObjectHeapNode : IHeapNode, IReadOnlyObjectHeapNode
    {
        void SetField(IField field, IValue value);
        void SetMethodResult(IMethod method, int invocation, IValue result);
    }
}
