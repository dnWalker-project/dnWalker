using dnlib.DotNet.Emit;
using dnWalker.NativePeers;
using Echo.ControlFlow;
using MMC.Data;
using MMC.State;
using MMC.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace dnWalker.Traversal
{
    public class PathConstraint
    {
        public Expression Expression { get; set; }
        public Instruction Next { get; set; }
        public CILLocation Location { get; set; }
    }

    public class Path
    {
        private IDictionary<string, object> _attributes = new Dictionary<string, object>();
        //private readonly IDictionary<ControlFlowNode<Instruction>, Expression> _nodeExpressions;
        //_nodeExpressions = new Dictionary<ControlFlowNode<Instruction>, Expression>();

        private IList<Segment> _segments = new List<Segment>();

        public void Extend(int toState)
        {
            var fromState = _segments.LastOrDefault()?.ToState ?? 0;

            _segments.Add(new Segment(fromState, toState));
        }

        public T Get<T>(string name)
        {
            if (_attributes.TryGetValue(name, out var o) && o is T t)
            {
                return t;
            }

            throw new ArgumentException(name);
        }

        public T SetObjectAttribute<T>(Allocation alloc, string attributeName, T attributeValue)
        {
            var key = alloc != null ? $"{alloc.GetHashCode()}:{attributeName}" : attributeName;
            _attributes[key] = attributeValue;
            return attributeValue;
        }

        public Path BacktrackTo(int id)
        {
            if (id == _segments.Last().ToState)
            {
                return null;
            }

            return new Path
            {
                _segments = _segments.TakeWhile(s => s.ToState != id).Union(_segments.Where(s => s.ToState == id)).ToList()
            };
        }

        public bool TryGetObjectAttribute<T>(Allocation alloc, string attributeName, out T attributeValue)
        {
            var key = alloc != null ? $"{alloc.GetHashCode()}:{attributeName}" : attributeName;
            if (_attributes.TryGetValue(key, out var o) && o is T t)
            {
                attributeValue = t;
                return true;
            }

            attributeValue = default;
            return false;
        }

        private IList<PathConstraint> _pathConstraints;

        public void AddPathConstraint(Expression expression, Instruction next, ExplicitActiveState cur)
        {
            _pathConstraints = _pathConstraints ?? new List<PathConstraint>();
            _pathConstraints.Add(
                new PathConstraint
                {
                    Expression = ExpressionOptimizer.visit(expression),
                    Location = new CILLocation(cur.CurrentLocation.Instruction, cur.CurrentLocation.Method),
                    Next = next
                });
        }

        public IReadOnlyCollection<PathConstraint> PathConstraints => new System.Collections.ObjectModel.ReadOnlyCollection<PathConstraint>(_pathConstraints);

        public string PathConstraintString => string.Join("; ", PathConstraints.Select(pc => pc.Expression));

        public bool Faulted => Exception != null;

        public string Exception { get; private set; }

        public string StackTrace { get; private set; }

        public string Output { get; private set; }

        public int Length => _segments.Count;

        public void Terminate(ThreadState threadState)
        {
            var fromState = _segments.LastOrDefault()?.ToState ?? 0;

            var segment = new Segment(fromState, -1)
            {
                Terminal = threadState.UnhandledException != null
            };

            if (threadState.UnhandledException != null)
            {
                Exception = threadState.UnhandledException.Message;
                StackTrace = threadState.CallStack.ToString();
            }

            _segments.Add(segment);

            if (SystemConsole.OutTextWriterRef.Equals(ObjectReference.Null))
            {
                return;
            }

            if (threadState.Cur.DynamicArea.Allocations[SystemConsole.OutTextWriterRef] is AllocatedObject theObject)
            {
                var field = theObject.Fields[0];
                if (field.Equals(ObjectReference.Null))
                {
                    return;

                }

                if (field is IConvertible c)
                {
                    Output = c.ToString(System.Globalization.CultureInfo.CurrentCulture);
                }
                else
                {
                    throw new Exception(field.WrapperName);
                }
            }
        }
    }
}
