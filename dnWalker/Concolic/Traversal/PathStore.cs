using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnWalker.Traversal;
using MMC.State;
using MMC.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace dnWalker.Concolic.Traversal
{
    public class PathStore : dnWalker.Traversal.PathStore
    {
        private readonly IDictionary<MethodDef, MethodExplorer> _methodExlorers;
        private readonly MethodDef _entryPoint;

        public Coverage Coverage => GetCodeCoverage();

        public PathStore(MethodDef entryPoint)
        {
            _methodExlorers = new Dictionary<MethodDef, MethodExplorer>(MethodEqualityComparer.CompareDeclaringTypes);
            _entryPoint = entryPoint;
        }

        public void OnInstructionExecuted(CILLocation location)
        {
            GetMethodExplorer(location).OnInstructionExecuted(location, CurrentPath);

            CurrentPath.OnInstructionExecuted(location);
        }

        [DebuggerStepThrough]
        private MethodExplorer GetMethodExplorer(CILLocation location)
        {
            var method = location.Method;
            if (!_methodExlorers.TryGetValue(method, out var explorer))
            {
                explorer = new MethodExplorer(method);
                _methodExlorers.Add(method, explorer);
            }

            return explorer;
        }

        private Coverage GetCodeCoverage()
        {
            var methodExplorer = GetMethodExplorer(new CILLocation(_entryPoint.Body.Instructions[0], _entryPoint));
            return methodExplorer.GetCoverage();
        }
    }
}
