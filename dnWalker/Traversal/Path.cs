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

        public void Terminate(ThreadState threadState)
        {
            if (threadState.UnhandledException != null)
            {
                Exception = threadState.UnhandledException.Message;
                StackTrace = threadState.CallStack.ToString();
            }

            AllocatedObject theObject = threadState.Cur.DynamicArea.Allocations[SystemConsole.OutTextWriterRef] as AllocatedObject;
            Output = ((IConvertible)theObject.Fields[0]).ToString(System.Globalization.CultureInfo.CurrentCulture);
        }
    }
}
