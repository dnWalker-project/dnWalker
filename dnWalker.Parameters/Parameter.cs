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
        protected Parameter(IParameterContext context)
        {
            Context = context;
            Reference = context.GetParameterRef();
        }

        protected Parameter(IParameterContext context, ParameterRef reference)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Reference = reference;
        }

        public IParameterContext Context
        {
            get;
        }

        public ParameterRef Reference
        {
            get;
        }

        public abstract IParameter Clone(IParameterContext newContext);

        private ParameterAccessor? _accessor = null;

        public ParameterAccessor? Accessor
        {
            get
            {
                return _accessor;
            }
            set
            {
                if (_accessor is RootParameterAccessor rOld)
                {
                    Context.Roots.Remove(rOld.Expression);
                }

                _accessor = value;

                if (value is RootParameterAccessor rNew)
                {
                    Context.Roots.Add(rNew.Expression, Reference);
                }
            }
        }

    }
}
