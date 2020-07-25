using dnWalker.NativePeers;
using MMC.State;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dnWalker.Traversal
{
    public class Path
    {
        private IDictionary<string, object> _attributes = new Dictionary<string, object>();

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

            AllocatedObject theObject = threadState.Cur.DynamicArea.Allocations[SystemConsole.OutTextWriterRef] as AllocatedObject;
            Output = ((IConvertible)theObject.Fields[0]).ToString(System.Globalization.CultureInfo.CurrentCulture);

            _segments.Add(segment);
        }
    }
}
