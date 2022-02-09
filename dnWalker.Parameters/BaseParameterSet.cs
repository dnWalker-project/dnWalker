using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public class BaseParameterSet : ParameterSet, IBaseParameterSet
    {
        public BaseParameterSet(IParameterContext context) : base(context)
        {
        }

        public IExecutionParameterSet CreateExecutionSet()
        {
            ExecutionParameterSet newSet = new ExecutionParameterSet(this);

            IDictionary<ParameterRef, IParameter> parameters = newSet.Parameters;
            foreach (var p in Parameters)
            {
                parameters.Add(p.Key, p.Value.Clone(newSet));
            }

            foreach (var r in Roots)
            {
                newSet.Roots.Add(r);
            }

            return newSet;
        }
    }
}
