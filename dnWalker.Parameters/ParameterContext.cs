using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public abstract class ParameterContext : IParameterContext
    {
        private readonly Dictionary<ParameterRef, IParameter> _parameters = new Dictionary<ParameterRef, IParameter>();
        private readonly Dictionary<string, ParameterRef> _roots = new Dictionary<string, ParameterRef>();

        protected ParameterContext()
        {
        }


        public abstract ParameterRef GetParameterRef();

        public IDictionary<ParameterRef, IParameter> Parameters
        {
            get 
            {
                return _parameters;
            }
        }

        public IDictionary<string, ParameterRef> Roots
        {
            get
            {
                return _roots;
            }
        }
    }
}
