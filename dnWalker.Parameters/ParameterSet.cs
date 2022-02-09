using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{

    public class ParameterSet : IParameterSet
    {
        private readonly Dictionary<ParameterRef, IParameter> _parameters = new Dictionary<ParameterRef, IParameter>();
        private readonly Dictionary<string, ParameterRef> _roots = new Dictionary<string, ParameterRef>();
        private readonly IParameterContext _context;
        public ParameterSet(IParameterContext context)
        {
            _context = context;
        }


        private int _lastParameterRef = 1;
        public virtual ParameterRef GetParameterRef()
        {
            return _lastParameterRef++;
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

        public IParameterContext Context
        {
            get
            {
                return _context;
            }
        }


        IReadOnlyDictionary<ParameterRef, IParameter> IReadOnlyParameterSet.Parameters
        {
            get
            {
                return _parameters;
            }
        }

        IReadOnlyDictionary<string, ParameterRef> IReadOnlyParameterSet.Roots
        {
            get
            {
                return _roots;
            }
        }
    }
}
