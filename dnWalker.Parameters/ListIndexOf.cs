using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public static class ListIndexOf
    {
        /// <summary>
        /// Returns index of first occurrence which conform to the predicate, -1 if no such item was found. Optionally start and end index can be set. If -1, the full range will be used.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static int IndexOf<T>(this IReadOnlyList<T> list, Func<T, bool> predicate, int startIndex = -1, int endIndex = -1)
        {
            startIndex = startIndex < 0 ? 0 : startIndex;
            endIndex = endIndex < 0 ? list.Count : startIndex;

            for (int i = startIndex; i < endIndex; ++i)
            {
                if (predicate(list[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Returns index of first occurrence which conform to the predicate, -1 if no such item was found. Optionally start and end index can be set. If -1, the full range will be used.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static int IndexOf<T>(this IList<T> list, Func<T, bool> predicate, int startIndex = -1, int endIndex = -1)
        {
            startIndex = startIndex < 0 ? 0 : startIndex;
            endIndex = endIndex < 0 ? list.Count : startIndex;

            for (int i = startIndex; i < endIndex; ++i)
            {
                if (predicate(list[i]))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
