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
        private static int _nextId = 1;
        private static ParameterRef GetReferenceIdFor(IParameter instance)
        {
            // "random" ids => using the GetHashCode function
            // return RuntimeHelpers.GetHashCode(instance);
            return _nextId++;
        }


        protected Parameter(IParameterContext context)
        {
            Context = context;
            Reference = GetReferenceIdFor(this);
        }
        protected Parameter(IParameterContext context, ParameterRef reference)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Reference = reference == ParameterRef.Any ? GetReferenceIdFor(this) : reference;
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
