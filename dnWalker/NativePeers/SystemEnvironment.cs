﻿using MMC.InstructionExec;
using dnlib.DotNet;
using MMC.Data;
using MMC.State;
using System.Reflection;
using System.Linq;

namespace dnWalker.NativePeers
{
    public class SystemEnvironment : NativePeer
    {
        private static MethodInfo _getResourceStringMethodInfo;

        static SystemEnvironment()
        {
            _getResourceStringMethodInfo = typeof(System.Environment)
                .GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Default)
                .FirstOrDefault(m => m.Name == "GetResourceString" && m.GetParameters().Length == 1);
        }

        public static string GetResourceString(string key)
        {
            var resourceValue = _getResourceStringMethodInfo.Invoke(null, new object[] { key });
            return (string)resourceValue;
        }

        public override bool TryGetValue(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue iieReturnValue)
        {
            switch (method.Name)
            {
                case "GetResourceString":
                    var resourceValue = GetResourceString(((ConstantString)args[0]).Value);
                    cur.EvalStack.Push(DataElement.CreateDataElement(resourceValue, cur.DefinitionProvider));
                    iieReturnValue = InstructionExecBase.nextRetval;
                    return true;
            }

            iieReturnValue = null;
            return false;
        }
    }
}
