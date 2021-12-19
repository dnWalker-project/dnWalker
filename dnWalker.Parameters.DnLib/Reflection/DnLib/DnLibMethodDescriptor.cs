using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters.Reflection.DnLib
{
    public struct DnLibMethodDescriptor : IMethodDescriptor
    {
        private IMethod _method;

        public DnLibMethodDescriptor(IMethod method)
        {
            _method = method;
        }

        public string Name
        {
            get
            {
                return _method.Name;
            }
        }

        public ITypeDescriptor ReturnType
        {
            get
            {
                return _method.ResolveMethodDef().ReturnType.AsTypeDescriptor();
            }
        }

        public IMethodArgumentDescriptor[] Arguments
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ITypeDescriptor DeclaringType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsStatic
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsAbstract
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
