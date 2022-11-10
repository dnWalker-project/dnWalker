using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Concolic;
using dnWalker.Graphs;
using dnWalker.Instructions.Extensions.NativePeers.MethodCalls;
using dnWalker.NativePeers;
using dnWalker.Symbolic;

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
    public class Path : ICloneable
    {
        private IDictionary<string, object> _attributes = new Dictionary<string, object>();
        private IList<Segment> _segments = new List<Segment>();

        private IList<CILLocation> _visitedNodes = new List<CILLocation>();
        private IDictionary<IDataElement, IDictionary<string, object>> _properties = new Dictionary<IDataElement, IDictionary<string, object>>(new Eq());
        private IDictionary<Allocation, IDictionary<string, object>> _allocationProperties = new Dictionary<Allocation, IDictionary<string, object>>();
        
        private readonly ICache<MethodDef, MethodTracer> _methodTracers;

        public Path(ICache<MethodDef, MethodTracer> methodTracers)
        {
            _methodTracers = methodTracers ?? throw new ArgumentNullException(nameof(methodTracers));
        }

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


        public Path BacktrackTo(int id)
        {
            if (id == _segments.Last().ToState)
            {
                return null;
            }

            return new Path(_methodTracers)
            {
                _segments = _segments.TakeWhile(s => s.ToState != id).Union(_segments.Where(s => s.ToState == id)).ToList()
            };
        }

        public string GetPathInfo()
        {
            var sb = new StringBuilder();
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
            if (location == CILLocation.None) return;

            _visitedNodes.Add(location);
            _methodTracers.Get(location.Method).OnInstructionExecuted(location.Instruction);
        }

        public void OnExceptionThrown(CILLocation location, TypeDef exceptionType)
        {
            if (location == CILLocation.None) return;

            _visitedNodes.Add(location);
            _methodTracers.Get(location.Method).OnExceptionThrown(location.Instruction, exceptionType);
        }


        public ExceptionInfo Exception { get; private set; }

        public string StackTrace { get; private set; }

        public string Output { get; private set; }

        public int Length => _segments.Count;

        public bool IsTerminated { get; private set; }

        public IList<Segment> Segments => _segments;
        public IList<CILLocation> VisitedNodes => _visitedNodes;

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
            return new Path(_methodTracers)
            {
                _attributes = new Dictionary<string, object>(_attributes),
                _segments = new List<Segment>(_segments),
                _visitedNodes = new List<CILLocation>(_visitedNodes),
                _properties = new Dictionary<IDataElement, IDictionary<string, object>>(_properties)
            };
        }
    }
}
