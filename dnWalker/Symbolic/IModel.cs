using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using dnWalker.Symbolic.Heap;

namespace dnWalker.Symbolic
{
    /// <summary>
    /// Provides valuation for the variables.
    /// </summary>
    public interface IModel : IReadOnlyModel
    {
        new IModel Clone();

        IReadOnlyModel IReadOnlyModel.Clone()
        {
            return Clone();
        }

        void SetValue(IVariable variable, IValue value);

        new IHeapInfo HeapInfo { get; }

        IReadOnlyHeapInfo IReadOnlyModel.HeapInfo
        {
            get
            {
                return HeapInfo;
            }
        }

    }
}
