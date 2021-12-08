using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public interface IItemOwnerParameter : IParameter
    {
        int Length { get; set; }

        IParameter?[] GetItems();

        bool TryGetItem(int index, [NotNullWhen(true)] out IParameter? parameter);
        void SetItem(int index, IParameter? parameter);
        void ClearItem(int index);
    }
}
