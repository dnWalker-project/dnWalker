using dnlib.DotNet;
using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.NativePeers
{
    internal static class ReflectionHelpers
    {
        private static readonly Type MethodDefType = typeof(MethodDef);
        private static readonly Type ExplicitActiveStateType = typeof(ExplicitActiveState);
        private static readonly Type DataElementType = typeof(IDataElement);

        private static readonly Type[] HandlerMethodSig = new Type[] { MethodDefType, typeof(DataElementList), ExplicitActiveStateType, typeof(IIEReturnValue).MakeByRefType() };

        public static IEnumerable<string> GetHandlerMethodNames(Type srcType)
        {
            return GetHandlerMethods(srcType)
                .Select(m => m.Name);
        }

        public static IEnumerable<MethodInfo> GetHandlerMethods(Type srcType)
        {
            return srcType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                .Where(m => IsHandlerMethod(m));
        }

        private static bool IsHandlerMethod(MethodInfo m)
        {
            ParameterInfo[] p = m.GetParameters();
            return m.ReturnType == typeof(bool) && m.GetParameters().Select(p => p.ParameterType).SequenceEqual(HandlerMethodSig);
        }
    }
}
