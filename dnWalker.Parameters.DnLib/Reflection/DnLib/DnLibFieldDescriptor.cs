using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using dnlib.DotNet;

namespace dnWalker.Parameters.Reflection.DnLib
{
    public readonly struct DnLibFieldDescriptor : IFieldDescriptor
    {
        private readonly IField _field;

        public DnLibFieldDescriptor(IField field)
        {
            _field = field;
        }

        public ITypeDescriptor Type
        {
            get
            {
                return _field.FieldSig.Type.AsTypeDescriptor();
            }
        }


        public string Name
        {
            get
            {
                return _field.Name;
            }
        }
    }
}
