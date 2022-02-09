using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public class ExecutionParameterContext : ParameterContext, IExecutionParameterContext
    {
        private readonly IBaseParameterContext? _baseContext;

        public ExecutionParameterContext(IBaseParameterContext? baseContext)
        {
            _baseContext = baseContext;
        }

        public IBaseParameterContext? BaseContext
        {
            get
            {
                return _baseContext;
            }
        }

        private int _lastParameterRef = int.MaxValue;

        public override ParameterRef GetParameterRef()
        {
            return _lastParameterRef--;
        }
    }
}
