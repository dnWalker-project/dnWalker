using dnWalker.Parameters;
using dnWalker.TestGenerator.Reflection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator
{
    public abstract class ParameterInitializationInfo
    {
        private HashSet<string> _accesses = new HashSet<string>();

        protected ParameterInitializationInfo(ParameterRef reference, Type expectedType)
        {
            Reference = reference;
            ExpectedType = expectedType;
        }

        public ParameterRef Reference 
        {
            get; 
        }

        public Type ExpectedType
        {
            get;
        }

        public IReadOnlyCollection<string> Accesses
        {
            get
            {
                return _accesses;
            }
        }

        public static ParameterInitializationInfo Create(IParameter parameter)
        {
            ParameterInitializationInfo? info;

            if (parameter is IObjectParameter op)
            {
                info = new ObjectInitializationInfo(parameter.Reference, AppDomain.CurrentDomain.GetType(op.Type) ?? throw new Exception("Could not find expected type."));
            }
            else if (parameter is IArrayParameter ap)
            {
                info = new ArrayInitializationInfo(ap.Reference, AppDomain.CurrentDomain.GetType(ap.ElementType)?.MakeArrayType() ?? throw new Exception("Could not find expected type."));
            }
            else if (parameter is IPrimitiveValueParameter vp)
            {
                info = new PrimitiveValueInitializationInfo(vp.Reference, AppDomain.CurrentDomain.GetType(vp.Type) ?? throw new Exception("Could not find expected type."));
            }
            else if (parameter is IStructParameter sp)
            {
                info = new StructParameterInitializationInfo(sp.Reference, AppDomain.CurrentDomain.GetType(sp.Type) ?? throw new Exception("Could not find expected type."));
            }
            else
            {
                throw new ArgumentException($"Unexpected parameter type: {parameter.GetType()}", nameof(parameter));
            }

            //info.AddContext(parameter.Context);
            return info;
        }

        public virtual void AddContext(IParameterContext context)
        {
            string? access = Reference.Resolve(context)?.GetAccessString();
            if (access != null)
            {
                _accesses.Add(access);
            }
        }
    }
}
