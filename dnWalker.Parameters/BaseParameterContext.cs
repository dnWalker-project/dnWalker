using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public class BaseParameterContext : ParameterContext, IBaseParameterContext
    {
        private int _lastParameterRef = 1;

        public override ParameterRef GetParameterRef()
        {
            return _lastParameterRef++;
        }

        public IExecutionParameterContext CreateExecutionContext()
        {
            ExecutionParameterContext newContext = new ExecutionParameterContext(this);

            IDictionary<ParameterRef, IParameter> parameters = newContext.Parameters;
            foreach (var p in Parameters)
            {
                parameters.Add(p.Key, p.Value.Clone(newContext));
            }

            return newContext;
        }
    }
}
