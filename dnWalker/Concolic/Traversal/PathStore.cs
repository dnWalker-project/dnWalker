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
            _methodExlorers = new Dictionary<MethodDef, MethodExplorer>();
            _entryPoint = entryPoint;
        }

        public override void AddPathConstraint(Expression expression, Instruction next, ExplicitActiveState cur)
        {
            base.AddPathConstraint(expression, next, cur);

            GetMethodExplorer(cur.CurrentLocation).OnConstraint(expression, next, cur);
        }

        public void OnInstructionExecuted(CILLocation location)
        {
            GetMethodExplorer(location).OnInstructionExecuted(location, CurrentPath);

            CurrentPath.OnInstructionExecuted(location);            

            /*if (location.Method != entryPoint)
            {
                return;
            }

            if (prev.Instruction != null)
            {

            }

            prev = location;*/
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
            var methodExplorer = GetMethodExplorer(new CILLocation(null, _entryPoint));
            return methodExplorer.GetCoverage();
        }
    }
}
