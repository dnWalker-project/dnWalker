using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public interface IItemOwner
    {
        /// <summary>
        /// Gets, sets the length trait. If null or lower than 0, we do not care about the length.
        /// </summary>
        int? Length { get; set; }

        ParameterRef[] GetItems();

        bool TryGetItem(int index, out ParameterRef itemRef);

        void SetItem(int index, ParameterRef itemRef);

        void ClearItem(int index);

        void MoveTo(IItemOwner other)
        {
            ParameterRef[] items = GetItems();
            for (int i = 0; i < items.Length; ++i)
            {
                if (items[i] == ParameterRef.Empty) continue;

                other.SetItem(i, items[i]);
                ClearItem(i);
            }
        }
    }

    public interface IItemOwnerParameter : IItemOwner, IParameter
    {
    }

    public static class ItemOwnerParameterExtensions
    {
        /// <summary>
        /// Gets the length of array which would be created from the <paramref name="itemOwner"/>.
        /// </summary>
        /// <param name="itemOwner"></param>
        /// <returns></returns>
        public static int GetLength(this IItemOwnerParameter itemOwner)
        {
            return itemOwner.Length is null or < 0 ? itemOwner.GetItems().Length : itemOwner.Length.Value;
        }

        public static bool TryGetItem(this IItemOwnerParameter itemOwner, int index, [NotNullWhen(true)] out IParameter? parameter)
        {
            if (itemOwner.TryGetItem(index, out ParameterRef reference) &&
                reference.TryResolve(itemOwner.Context, out parameter))
            {
                return true;
            }

            parameter = null;
            return false;
        }

        public static bool TryGetItem<TParameter>(this IItemOwnerParameter itemOwner, int index, [NotNullWhen(true)] out TParameter? parameter)
            where TParameter : class, IParameter
        {
            if (itemOwner.TryGetItem(index, out ParameterRef reference) &&
                reference.TryResolve(itemOwner.Context, out parameter))
            {
                return true;
            }

            parameter = null;
            return false;
        }

        public static void SetItem(this IItemOwnerParameter itemOwner, int index, IParameter? parameter)
        {
            itemOwner.SetItem(index, parameter?.Reference ?? ParameterRef.Empty);
        }

        public static IParameter?[] GetItems(this IItemOwnerParameter itemOwner)
        {
            ParameterRef[] refs = itemOwner.GetItems();
            IParameter?[] items = new IParameter[refs.Length];

            int i = 0;
            foreach (IParameter? p in refs.Select(r => r.Resolve(itemOwner.Context)))
            {
                items[i] = p;
                ++i;
            }

            return items;
        }
    }
}
