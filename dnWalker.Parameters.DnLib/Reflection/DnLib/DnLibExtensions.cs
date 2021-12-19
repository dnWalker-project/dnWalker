using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters.Reflection.DnLib
{
    public static class DnLibExtensions
    {
        public static ITypeDescriptor AsTypeDescriptor(this IType type)
        {
            return new DnLibTypeDescriptor(type);
        }

        public static IFieldDescriptor AsFieldDescriptor(this IField field)
        {
            return new DnLibFieldDescriptor(field);
        }

        public static IMethodDescriptor AsMethodDescriptor(this IMethodDescriptor method)
        {
            return new DnLibMethodDescriptor(method);
        }
    }
}
