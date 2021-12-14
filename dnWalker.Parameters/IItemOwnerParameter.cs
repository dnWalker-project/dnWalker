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
        int? Length { get; set; }

        string ElementTypeName { get; }

        IParameter?[] GetItems();

        bool TryGetItem(int index, [NotNullWhen(true)] out IParameter? parameter);
        void SetItem(int index, IParameter? parameter);
        void ClearItem(int index);
    }

    public static class ItemOwnerParameterExtensions
    {
        public static int GetLength(this IItemOwnerParameter itemOwner)
        {
            return itemOwner.Length ?? itemOwner.GetItems().Length;
        }
    }
}
