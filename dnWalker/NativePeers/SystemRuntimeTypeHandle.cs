using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using MMC.Data;
using MMC.State;

namespace dnWalker.NativePeers
{
    public class SystemRuntimeTypeHandle : NativePeer
    {
        public SystemRuntimeTypeHandle(MethodDef meth) : base(meth)
        {
        }

        public override bool TryGetValue(DataElementList args, ExplicitActiveState cur, out IDataElement dataElement)
        {
            dataElement = null;

            if (_method.FullName == "System.IntPtr System.RuntimeTypeHandle::get_Value()")
            {
                var value = args[0];
                switch (value)
                {
                    case LocalVariablePointer localVariablePointer:
                        args = new DataElementList(1);
                        args[0] = localVariablePointer.Value;
                        return TryGetValue(args, cur, out dataElement);
                    case TypePointer typePointer:
                        var type = typePointer.Type;
                        if (cur.DefinitionProvider.TryGetTypeHandle(type, out var handle))
                        {
                            dataElement = cur.DefinitionProvider.CreateDataElement(handle.Value);
                            return true;
                        }

                        throw new NotImplementedException(type.FullName);
                    default:
                        throw new NotImplementedException(value.GetType().FullName);
                }
            }

            throw new NotImplementedException("Native peer for " + _method.FullName);
        }
    }
}
