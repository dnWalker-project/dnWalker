using MMC.State;
using System;
using System.Collections.Generic;

namespace dnWalker.Traversal
{
    public class Path
    {
        private IDictionary<string, object> _attributes = new Dictionary<string, object>();

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
    }
}
