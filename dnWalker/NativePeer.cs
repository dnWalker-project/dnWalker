using dnlib.DotNet;
using MMC.Data;
using MMC.InstructionExec;
using MMC.State;
using System;

namespace dnWalker
{
    public abstract class NativePeer
    {
        public static NativePeer Get(TypeDef typeDef)
        {
            string fullName = typeDef.FullName;

            if (typeDef.FullName == typeof(Environment).FullName)
            {
                return new NativePeers.SystemEnvironment();
            }

            if (typeDef.FullName == typeof(double).FullName)
            {
                return new NativePeers.SystemDouble();
            }

            if (typeDef.FullName == typeof(Console).FullName)
            {
                return new NativePeers.SystemConsole();
            }

            if (typeDef.FullName == typeof(System.IO.TextWriter).FullName
                || typeDef.FullName == typeof(NativePeers.SystemIOTextWriterImpl).FullName)
            {
                return new NativePeers.SystemIOTextWriter();
            }

            if (typeDef.FullName == typeof(float).FullName)
            {
                return new NativePeers.SystemSingle();
            }

            /*if (methodDef.DeclaringType.FullName == typeof(IntPointer).FullName)
            {
                return new NativePeers.SystemSingle(methodDef);
            }*/

            if (typeDef.FullName == typeof(RuntimeTypeHandle).FullName)
            {
                return new NativePeers.SystemRuntimeTypeHandle();
            }

            if (typeDef.FullName == typeof(RuntimeMethodHandle).FullName)
            {
                return new NativePeers.SystemRuntimeMethodHandle();
            }

            if (typeDef.FullName == typeof(RuntimeFieldHandle).FullName)
            {
                return new NativePeers.SystemRuntimeFieldHandle();
            }

            if (typeDef.FullName == typeof(string).FullName)
            {
                return new NativePeers.SystemString();
            }

            if (typeDef.FullName == typeof(System.Threading.Thread).FullName)
            {
                return new NativePeers.SystemThreadingThread();
            }

            if (typeDef.FullName == typeof(System.Threading.Monitor).FullName)
            {
                return new NativePeers.SystemThreadingMonitor();
            }

            if (typeDef.FullName == typeof(Random).FullName)
            {
                return new NativePeers.SystemRandom();
            }

            if (typeDef.FullName == typeof(Math).FullName)
            {
                return new NativePeers.SystemMath();
            }

            if (fullName == typeof(IntPtr).FullName)
            {
                return new NativePeers.SystemIntPtr();
            }

            if (fullName == typeof(UIntPtr).FullName)
            {
                return new NativePeers.SystemUIntPtr();
            }

            return null;
        }

        public NativePeer()
        {
        }

        public abstract bool TryGetValue(MethodDef methodDef, DataElementList args, ExplicitActiveState cur, out IIEReturnValue iieReturnValue);

        public virtual bool TryConstruct(MethodDef methodDef, DataElementList args, ExplicitActiveState cur)
        {
            if (methodDef.DeclaringType.FullName == typeof(string).FullName)
            {
                return new NativePeers.SystemString().TryConstruct(methodDef, args, cur);
            }

            return false;
        }
    }
}
