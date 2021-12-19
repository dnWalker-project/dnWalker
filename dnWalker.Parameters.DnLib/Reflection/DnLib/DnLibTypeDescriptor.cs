using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters.Reflection.DnLib
{
    public struct DnLibTypeDescriptor : ITypeDescriptor
    {
        private IType _type;

        public DnLibTypeDescriptor(IType type)
        {
            _type = type;
        }
    }
}
