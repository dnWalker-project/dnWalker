using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public abstract class Parameter : IParameter
    {
        protected Parameter(IParameterSet context, TypeSignature type)
        {
            Set = context;
            Type = type;
            Reference = context.GetParameterRef();
        }

        protected Parameter(IParameterSet context, TypeSignature type, ParameterRef reference)
        {
            Set = context ?? throw new ArgumentNullException(nameof(context));
            Type = type;
            Reference = reference;
        }

        public IParameterSet Set
        {
            get;
        }

        public ParameterRef Reference
        {
            get;
        }

        public abstract IParameter CloneData(IParameterSet newContext);


        private readonly List<ParameterAccessor> _accessors = new List<ParameterAccessor>();

        public IList<ParameterAccessor> Accessors
        {
            get
            {
                return _accessors;
            }
        }
        public TypeSignature Type { get; }
    }
}
