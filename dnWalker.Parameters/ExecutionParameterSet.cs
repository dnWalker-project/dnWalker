using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public class ExecutionParameterSet : ParameterSet, IExecutionParameterSet
    {
        private readonly IBaseParameterSet? _baseSet;

        public ExecutionParameterSet(IBaseParameterSet baseSet) : base(baseSet.Context)
        {
            _baseSet = baseSet;
        }

        public IBaseParameterSet? BaseSet
        {
            get
            {
                return _baseSet;
            }
        }

        private int _lastParameterRef = int.MaxValue;

        public override ParameterRef GetParameterRef()
        {
            return _lastParameterRef--;
        }
    }
}
