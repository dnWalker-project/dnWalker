using dnlib.DotNet.Emit;

using dnWalker.Concolic;
using dnWalker.Graphs;
using dnWalker.NativePeers;
using dnWalker.Parameters;

using Echo.ControlFlow;
using MMC.Data;
using MMC.State;
using MMC.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace dnWalker.Traversal
{
    [DebuggerDisplay("{Expression}")]
    public class PathConstraint
    {
        public Instruction Next { get; set; }
        public CILLocation Location { get; set; }
        public Expression Expression { get; set; }
    }


    public class Path : ICloneable
    {
        private IDictionary<string, object> _attributes = new Dictionary<string, object>();
        private IList<Segment> _segments = new List<Segment>();
        private IList<PathConstraint> _pathConstraints = new List<PathConstraint>();
        private IList<long> _visitedNodes = new List<long>();
        private IDictionary<IDataElement, IDictionary<string, object>> _properties = new Dictionary<IDataElement, IDictionary<string, object>>(new Eq());
        private IDictionary<Allocation, IDictionary<string, object>> _allocationProperties = new Dictionary<Allocation, IDictionary<string, object>>();
        private CILLocation _lastLocation;
        private IDictionary<CILLocation, int> _counter = new Dictionary<CILLocation, int>();

        public void Extend(int toState)
        {
            var fromState = _segments.LastOrDefault()?.ToState ?? 0;

            _segments.Add(new Segment(fromState, toState));
        }


        public bool TryGetPathAttribute<T>(string name, [NotNullWhen(true)]out T attribute)
        {
            if (_attributes.TryGetValue(name, out object o) && o is T t)
            {
                attribute = t;
                return true;
            }
            attribute = default;
            return false;
        }

        public void SetPathAttribute<T>(string name, T attribute)
        {
            _attributes[name] = attribute;
        }

        private class Eq : IEqualityComparer<IDataElement>
        {
            public bool Equals(IDataElement x, IDataElement y)
            {
                return x.GetType() == y.GetType() && x.HashCode == y.HashCode;
            }

            public int GetHashCode(IDataElement obj)
            {
                return obj.HashCode;
            }
        }
        
        public T SetObjectAttribute<T>(IDataElement dataElement, string attributeName, T attributeValue)
        {
            if (!_properties.TryGetValue(dataElement, out var dict))
            {
                dict = new Dictionary<string, object>();
                _properties.Add(dataElement, dict);
            }
            dict[attributeName] = attributeValue;
            return attributeValue;
        }

        public bool TryGetObjectAttribute<T>(IDataElement dataElement, string attributeName, [NotNullWhen(true)] out T attributeValue)
        {
            attributeValue = default;
            if (!_properties.TryGetValue(dataElement, out var dict) || !dict.TryGetValue(attributeName, out var value))
            {
                return false;
            }

            attributeValue = (T)value;
            return true;
        }

        public void SetAllocationAttribute<T>(Allocation allocation, string attributeName, T attributeValue)
        {
            if (!_allocationProperties.TryGetValue(allocation, out var dict))
            {
                dict = new Dictionary<string, object>();
                _allocationProperties.Add(allocation, dict);
            }
            dict[attributeName]= attributeValue;
        }

        public bool TryGetAllocationAttribute<T>(Allocation allocation, string attributeName, out T attributeValue)
        {
            attributeValue = default;
            if (!_allocationProperties.TryGetValue(allocation, out var dict) || !dict.TryGetValue(attributeName, out var value))
            {
                return false;
            }

            attributeValue = (T)value;
            return true;
        }

        public void AddVisitedNode(Node node)
        {
            if (!_visitedNodes.Contains(node.Offset))
            {
                _visitedNodes.Add(node.Offset);
            }
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


        public void AddPathConstraint(Expression expression, Instruction next, ExplicitActiveState cur)
        {
            //try
            //{
            //    expression = ExpressionOptimizer.visit(expression);
            //}
            //catch
            //{
            //    // exception is ignored, 3rd party 
            //}

            expression = expression.Optimize().Simplify();

            _pathConstraints.Add(
                new PathConstraint
                {
                    Expression = expression,
                    Location = new CILLocation(cur.CurrentLocation.Instruction, cur.CurrentLocation.Method),
                    Next = next
                });
        }

        public IReadOnlyCollection<PathConstraint> PathConstraints => new System.Collections.ObjectModel.ReadOnlyCollection<PathConstraint>(_pathConstraints);

        public Expression PathConstraint =>
            _pathConstraints.Any() ? 
            PathConstraints.Select(p => p.Expression).Aggregate((a, b) => Expression.And(a, b)) :
            null;

        public string PathConstraintString => PathConstraint?.ToString() ?? "True"; // if no constraint, the expression is True

        public string GetPathInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Path constraint: " + PathConstraintString);
            sb.AppendLine("Input:");
            sb.AppendLine("Output:");
            sb.AppendLine(Output);
            sb.AppendLine("Visited nodes:");
            foreach (var node in _visitedNodes)
            {
                sb.AppendLine(" - " + node.ToString());
            }
            return sb.ToString();
        }

        public bool Faulted => Exception != null;

        public void OnInstructionExecuted(CILLocation location)
        {
            _lastLocation = location;

            if (_counter.ContainsKey(location))
            {
                _counter[location]++;
            }
            else
            {
                _counter[location] = 1;
            }
        }

        public ExceptionInfo Exception { get; private set; }

        public string StackTrace { get; private set; }

        public string Output { get; private set; }

        public int Length => _segments.Count;

        public bool IsTerminated { get; private set; }

        public void Terminate(MMC.State.ThreadState threadState)
        {
            var fromState = _segments.LastOrDefault()?.ToState ?? 0;

            IsTerminated = true;

            var segment = new Segment(fromState, -1)
            {
                Terminal = threadState.UnhandledException != null
            };

            if (threadState.UnhandledException != null)
            {
                Exception = threadState.UnhandledException;//.Message;
                StackTrace = threadState.CallStack.ToString();
            }

            _segments.Add(segment);

            if (SystemConsole.TryGetConsoleOut(threadState.Cur, out ObjectReference consoleOut))
            {
                if (threadState.Cur.DynamicArea.Allocations[consoleOut] is AllocatedObject theObject)
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

        object ICloneable.Clone()
        {
            return new Path
            {
                _attributes = new Dictionary<string, object>(_attributes),
                _segments = new List<Segment>(_segments),
                _pathConstraints = new List<PathConstraint>(_pathConstraints),
                _visitedNodes = new List<long>(_visitedNodes),
                _properties = new Dictionary<IDataElement, IDictionary<string, object>>(_properties),
                _lastLocation = _lastLocation
            };
        }
    }
}
