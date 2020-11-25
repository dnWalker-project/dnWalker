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

        public PathStore()
        {
            _methodExlorers = new Dictionary<MethodDef, MethodExplorer>();
        }

        public override void AddPathConstraint(Expression expression, Instruction next, ExplicitActiveState cur)
        {
            base.AddPathConstraint(expression, next, cur);

            GetMethodExplorer(cur.CurrentLocation).OnConstraint(expression, next, cur);
        }

        public override Expression GetNextPathConstraint(Path path)
        {
            var pc = path.PathConstraints.Select(p => p.Expression).Take(path.PathConstraints.Count - 1).ToList();
            pc.Add(Expression.Not(path.PathConstraints.Last().Expression));
            return pc.Aggregate((a, b) => Expression.And(a, b));
        }

        public void OnInstructionExecuted(CILLocation location)
        {
            GetMethodExplorer(location).OnInstructionExecuted(location);

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
            MethodDef method = location.Method;
            if (!_methodExlorers.TryGetValue(method, out var explorer))
            {
                explorer = new MethodExplorer(method);
                _methodExlorers.Add(method, explorer);
            }

            return explorer;
        }
    }
}
