using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using MMC.Data;
using MMC.State;

namespace dnWalker.NativePeers
{
    public class SystemRuntimeFieldHandle : NativePeer
    {
        public SystemRuntimeFieldHandle(MethodDef meth) : base(meth)
        {
        }

        public override bool TryGetValue(DataElementList args, ExplicitActiveState cur, out IDataElement dataElement)
        {
            dataElement = null;

            if (_method.FullName == "System.IntPtr System.RuntimeFieldHandle::get_Value()")
            {
                var value = args[0];
                switch (value)
                {
                    case LocalVariablePointer localVariablePointer:
                        args = new DataElementList(1);
                        args[0] = localVariablePointer.Value;
                        return TryGetValue(args, cur, out dataElement);
                    case FieldHandle fieldHandle:
                        dataElement = cur.DefinitionProvider.CreateDataElement(fieldHandle.Value.GetHashCode());
                        return true;
                    default:
                        throw new NotImplementedException(value.GetType().FullName);
                }
            }

            throw new NotImplementedException("Native peer for " + _method.FullName);
        }
    }
}
