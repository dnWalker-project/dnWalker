using dnlib.DotNet;

using System.Diagnostics.CodeAnalysis;

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
        IValue GetElementOrDefault(int index);

        bool TryGetElement(int index, [NotNullWhen(true)] out IValue? value);

        IEnumerable<int> Indeces { get; }

        TypeSig ElementType { get; }

        int Length { get; }
        bool HasElements { get; }
    }
}
