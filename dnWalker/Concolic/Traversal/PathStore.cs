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
        private IList<Expression> _expressions = new List<Expression>();

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

        public IDictionary<string, object> GetNextInputValues(ISolver solver, IList<ParameterExpression> parameters)
        {
            if (!_expressions.Contains(CurrentPath.PathConstraint))
            {
                _expressions.Add(CurrentPath.PathConstraint);
            }

            var expressionToSolve = GetNextPathConstraint();
            if (expressionToSolve == null)
            {
                return null;
            }            

            var next = solver.Solve(expressionToSolve, parameters);
            if (next == null)
            {
                var length = int.MaxValue;
                Path shortestPath = null;
                foreach (var path in Paths)
                {
                    if (path.PathConstraints.Count < length)
                    {
                        length = path.PathConstraints.Count;
                        shortestPath = path;
                    }
                }

                expressionToSolve = Flip(
                    new System.Collections.ObjectModel.ReadOnlyCollection<PathConstraint>(
                        shortestPath.PathConstraints.Take(shortestPath.PathConstraints.Count - 1).ToList()));

                next = solver.Solve(expressionToSolve, parameters);
                if (next == null)
                {
                    return null;
                }
            }

            _expressions.Add(expressionToSolve);

            return next;
        }

        public Expression GetNextPathConstraint()
        {
            return Flip(CurrentPath.PathConstraints);
        }

        public Expression Flip(IReadOnlyCollection<PathConstraint> pathConstraints)
        {
            var pc = new List<Expression>();

            var flipped = false;

            foreach (var pathConstraint in pathConstraints.Reverse())
            {
                var methodExplorer = GetMethodExplorer(pathConstraint.Location);
                if (flipped)
                {
                    pc.Insert(0, pathConstraint.Expression);
                    continue;
                }

                var expression = methodExplorer.Flip(pathConstraint);
                if (expression != pathConstraint.Expression)
                {
                    flipped = true;
                }

                pc.Insert(0, expression);
            }

            if (!flipped)
            {
                return null;
            }

            //var pc = path.PathConstraints.Select(p => p.Expression).Take(path.PathConstraints.Count - 1).ToList();
            //pc.Add(Expression.Not(path.PathConstraints.Last().Expression));
            return ExpressionOptimizer.visit(pc.Aggregate((a, b) => Expression.And(a, b)));
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
            MethodDef method = location.Method;
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
