using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    [DebuggerDisplay("{Name}")]
    public class ParameterContext : IParameterContext
    {
        private readonly Dictionary<ParameterRef, IParameter> _parameters = new Dictionary<ParameterRef, IParameter>();
        private readonly Dictionary<string, ParameterRef> _roots = new Dictionary<string, ParameterRef>();

        public string Name { get; set; } = string.Empty;

        public ParameterContext() : this(0)
        {
        }

        public ParameterContext(int generation)
        {
            Generation = generation;
        }

        private int _clonings = 0;

        public int Generation 
        {
            get; 
        }

        public IParameterContext Clone()
        {
            ParameterContext newContext = new ParameterContext(_clonings++);

            foreach (var p in _parameters)
            {
                newContext._parameters.Add(p.Key, p.Value.Clone(newContext));
            }

            foreach (var r in _roots)
            {
                newContext._roots[r.Key] = r.Value;
            }

            return newContext;
        }

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
