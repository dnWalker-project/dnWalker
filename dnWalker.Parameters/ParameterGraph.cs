using System.Collections.Generic;

namespace dnWalker.Parameters
{
    public static class ParameterGraph
    {

        /// <summary>
        /// Gets the closest root parameter of this parameter, using a BFS. 
        /// Traversal can be limited by the max depth parameter.
        /// </summary>
        public static IParameter? GetRoot(this IParameter parameter, int maxDepth = -1)
        {
            if (maxDepth == -1) maxDepth = int.MaxValue;

            IReadOnlyParameterSet set = parameter.Set;
            Queue<(IParameter parameter, int depth)> frontier = new Queue<(IParameter parameter, int depth)>();
            HashSet<ParameterRef> visited = new HashSet<ParameterRef>();

            frontier.Enqueue((parameter, maxDepth));

            while (frontier.Count > 0)
            {
                (IParameter p, int depth) = frontier.Dequeue();

                if (depth <= 0) break;

                ParameterRef thisRef = p.Reference;

                visited.Add(thisRef);

                foreach (ParameterAccessor accessor in p.Accessors)
                {
                    if (accessor is RootParameterAccessor root)
                    {
                        return p; // we have found a root parameter
                    } 
                    else if (accessor is ParentChildParameterAccessor parentChild)
                    {
                        ParameterRef parentRef = parentChild.ParentRef;

                        if (visited.Contains(parentRef))
                        {
                            continue;
                        }

                        frontier.Enqueue((parentRef.Resolve(set), depth - 1));
                    }
                }
            }

            return null;
        }
    }
}