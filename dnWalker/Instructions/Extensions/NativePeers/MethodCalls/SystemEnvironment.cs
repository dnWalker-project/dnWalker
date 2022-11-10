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

namespace dnWalker.Instructions.Extensions.NativePeers.MethodCalls
{
    // TODO: should choose implementation based on running platform, e.i. framework - Environment.GetResourceString(), coreclr - SR.GetResourceString()
    [NativePeer("System.Environment", MatchMethods = true)]
    public class SystemEnvironment : CompiledMethodCallNativePeer<SystemEnvironment>
    {
        private static MethodInfo _getResourceStringMethodInfo;

        static SystemEnvironment()
        {
            Type systemResources = Type.GetType("System.SR");

            _getResourceStringMethodInfo = systemResources
                .GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Default)
                .FirstOrDefault(m => m.Name == "GetResourceString" && m.GetParameters().Length == 1);
        }

        public static string GetResourceString(string key)
        {
            var resourceValue = _getResourceStringMethodInfo?.Invoke(null, new object[] { key });// ?? string.Empty;
            return (string)resourceValue;
        }

        private static bool GetResourceString(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            var resourceValue = GetResourceString(((ConstantString)args[0]).Value);
            cur.EvalStack.Push(DataElement.CreateDataElement(resourceValue, cur.DefinitionProvider));
            returnValue = InstructionExecBase.nextRetval;
            return true;
        }
    }
}
