using dnWalker.Parameters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Parameters
{
    public static class ParameterContextExtensions
    {
        public static IEnumerable<IParameter> GetParametersSortedByDependency(this IParameterContext context)
        {
            IParameter[] result = new IParameter[context.Parameters.Count];

            Dictionary<ParameterRef, int> visited = new Dictionary<ParameterRef, int>();

            Queue<IParameter> queue = new Queue<IParameter>(context.Parameters.Count);
            foreach (IParameter p in context.Parameters.Values)
            {
                queue.Enqueue(p);
                visited[p.Reference] = 0;
            }


            int idx = 0;

            while (queue.Count > 0)
            {
                IParameter p = queue.Dequeue();
                ParameterRef r = p.Reference;

                int visitCount = visited[r];

                if (visitCount > result.Length)
                {
                    // we have visited this parameter more times than there is total number of parameters => is now sorting available?
                    // how to handle circular dependency?
                    throw new Exception("Could not find satisfiable sorting.");
                }

                visited[r] = visitCount + 1;

                if (p is IPrimitiveValueParameter)
                {
                    // it is not dependent on any other parameter => put it in the result...
                }
            }

            return result;
        }
    }
}
