using dnlib.DotNet;

using System.Diagnostics.CodeAnalysis;

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
        IValue GetFieldOrDefault(IField field);

        bool TryGetField(IField field, [NotNullWhen(true)]out IValue? value);

        /// <summary>
        /// Gets the value of the method result.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        IValue GetMethodResult(IMethod method, int invocation);
        bool TryGetMethodResult(IMethod method, int invocation, [NotNullWhen(true)] out IValue? value);

        IEnumerable<IField> Fields { get; }
        IEnumerable<(IMethod method, int invocation)> MethodInvocations { get; }

        bool HasFields { get; }
        bool HasMethodInvocations { get; }
    }

    public static class ReadOnlyObjectHeapNodeExtensions
    {
        public static IEnumerable<(IMethod method, IValue[] results)> GetMethodResults(this IReadOnlyObjectHeapNode objectNode)
        {
            Dictionary<IMethod, int> maxInvocation = new Dictionary<IMethod, int>(MethodEqualityComparer.CompareDeclaringTypes);

            foreach ((IMethod method, int invocation) in objectNode.MethodInvocations)
            {
                UpdateInvocation(method, invocation);
            }

            List<(IMethod method, IValue[] results)> retList = new List<(IMethod,IValue[])>();

            foreach ((IMethod method, int max) in maxInvocation)
            {
                IValue[] results = new IValue[max];
                for (int i = 0; i < max; ++i)
                {
                    results[i] = objectNode.GetMethodResult(method, i + 1);
                }

                retList.Add((method, results));
            }

            return retList;
            
            void UpdateInvocation(IMethod method, int invocation)
            {
                if (maxInvocation.TryGetValue(method, out int currMax))
                {
                    if (currMax < invocation)
                    {
                        maxInvocation[method] = invocation;
                    }
                }
                else
                {
                    maxInvocation[method] = invocation;
                }
            }
        }
    }
}
