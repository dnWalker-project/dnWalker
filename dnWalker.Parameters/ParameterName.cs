using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public readonly struct ParameterName : IEquatable<ParameterName>
    {
        private readonly string[]? _accessors;
        private readonly int _index;

        public static readonly ParameterName Empty = new ParameterName();

        public bool IsEmpty
        {
            get { return _accessors == null || _accessors.Length == 0; }
        }

        public string FullName
        {
            get
            {
                if (IsEmpty) return string.Empty;

                return string.Join(ParameterNameUtils.Delimiter, _accessors!.Take(_index + 1));
            }
        }

        public string LocalName
        {
            get
            {
                return IsEmpty ? throw new Exception("This is an Empty ParameterName") : _accessors![_index];
            }
        }

        public string RootName
        {
            get
            {
                return IsEmpty ? throw new Exception("This is an Empty ParameterName") : _accessors![0];
            }
        }

        public bool IsRootName
        {
            get
            {
                return !IsEmpty && _index == 0;
            }
        }

        public bool IsLeafName
        {
            get
            {
                return !IsEmpty && _index == _accessors!.Length - 1;
            }
        }

        public bool TryGetNext(out ParameterName parameterName)
        {
            if (IsEmpty || IsLeafName)
            {
                parameterName = Empty;
                return false;
            }

            parameterName = new ParameterName(_accessors!, _index + 1);
            return true;
        }

        public bool TryGetPrevious(out ParameterName parameterName)
        {
            if (IsEmpty || IsRootName)
            {
                parameterName = Empty;
                return false;
            }

            parameterName = new ParameterName(_accessors!, _index - 1);
            return true;
        }

        public bool TryGetRoot(out ParameterName parameterName)
        {
            if (IsEmpty)
            {
                parameterName = Empty;
                return false;
            }

            parameterName = new ParameterName(_accessors!, 0);
            return true;
        }

        public bool TryGetLeaf(out ParameterName parameterName)
        {
            if (IsEmpty)
            {
                parameterName = Empty;
                return false;
            }

            parameterName = new ParameterName(_accessors!, _accessors!.Length - 1);
            return true;
        }

        public bool TryGetField([NotNullWhen(true)] out string? field)
        {
            if (IsEmpty)
            {
                field = null;
                return false; 
            }

            // check if local name is not an index or method result
            string localName = LocalName;
            Debug.Assert(!string.IsNullOrWhiteSpace(localName));

            if (int.TryParse(localName, out int _) ||
                localName.Contains(ParameterNameUtils.CallIndexDelimiter))
            {
                field = null;
                return false;
            }

            field = localName;
            return true;
        }

        public ParameterName WithField(string field)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Cannot work with an empty parameter name");
            }

            if (string.IsNullOrWhiteSpace(field))
            {
                throw new ArgumentException($"'{nameof(field)}' cannot be null or whitespace.", nameof(field));
            }

            if (int.TryParse(field, out int _))
            {
                throw new ArgumentException($"'{nameof(field)}' cannot be an integer.", nameof(field));
            }

            if (field.Contains(ParameterNameUtils.CallIndexDelimiter))
            {
                throw new ArgumentException($"'{nameof(field)}' cannot be a method result accessor.", nameof(field));
            }

            return WithAccessor(field);
        }


        public bool TryGetIndex(out int index)
        {
            if (IsEmpty)
            {
                index = -1;
                return false;
            }

            if (int.TryParse(LocalName, out index) && index >= 0)
            {
                return true;
            }
            return false;
        }

        public ParameterName WithIndex(int index)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Cannot work with an empty parameter name");
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index must be nonnegative integer", nameof(index));
            }

            return WithAccessor(index.ToString());
        }

        public bool TryGetMethodResult([NotNullWhen(true)] out string? methodName, out int callNumber)
        {
            if (IsEmpty)
            {
                callNumber = -1;
                methodName = null;
                return false;
            }

            string localName = LocalName;
            int delim = localName.IndexOf(ParameterNameUtils.CallIndexDelimiter);
            if (delim == -1 || delim == 0) // no | or at the first character => empty method name!!
            {
                callNumber = -1;
                methodName = null;
                return false;
            }

            if (int.TryParse(localName.Substring(delim + 1), out callNumber) && callNumber >= 0)
            {
                methodName = localName.Substring(0, delim);
                return true;
            }
            methodName = null;
            return false;
        }

        public ParameterName WithMethodResult(string methodName, int callNumber)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Cannot work with an empty parameter name");
            }

            if (callNumber < 0)
            {
                throw new ArgumentException("callNumber must be nonnegative integer", nameof(callNumber));
            }

            if (string.IsNullOrWhiteSpace(methodName))
            {
                throw new ArgumentException($"'{nameof(methodName)}' cannot be null or whitespace.", nameof(methodName));
            }

            return WithAccessor($"{methodName}{ParameterNameUtils.CallIndexDelimiter}{callNumber}");
        }

        public ParameterName WithAccessor(string accessor)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Cannot work with an empty parameter name");
            }

            int newIndex = _index + 1;

            if (_accessors!.Length > newIndex && _accessors![newIndex] == accessor)
            {
                return new ParameterName(_accessors, newIndex);
            }
            else
            {
                string[] accessors = new string[newIndex + 1];
                Array.Copy(_accessors!, 0, accessors, 0, newIndex);
                accessors[newIndex] = accessor;

                return new ParameterName(accessors, newIndex);
            }
        }

        private ParameterName(string[] accessors, int index)
        {
            if (accessors == null || accessors.Length == 0)
            {
                throw new ArgumentException("accessors cannot be null or empty");
            }

            if (index >= accessors.Length || index < 0)
            {
                throw new IndexOutOfRangeException("index");
            }

            _accessors = accessors;
            _index = index; ;
        }

        private ParameterName(string fullName)
        {
            _accessors = fullName.Split(ParameterNameUtils.Delimiter);
            _index = _accessors.Length - 1;
        }

        public IEnumerable<ParameterName> TraversFromRoot()
        {
            return new FromRootParameterNameTraversal(this);
        }
        public IEnumerable<ParameterName> TraversToRoot()
        {
            return new ToRootParameterNameTraversal(this);
        }


        public static implicit operator ParameterName(string? fullName)
        {
            return string.IsNullOrWhiteSpace(fullName) ? Empty : new ParameterName(fullName);
        }

        public static implicit operator string?(ParameterName parameterName)
        {
            return parameterName.ToString();
        }

        #region Equality & HashCode
        public override bool Equals(object? obj)
        {
            return obj is ParameterName name && Equals(name);
        }

        public bool Equals(ParameterName other)
        {
            if (_index == other._index)
            {
                if (!IsEmpty || other.IsEmpty) // if at least one of them is empty => no need to do the ancestor check
                {
                    for (int i = 0; i < _index; i++)
                    {
                        if (_accessors![i] != other._accessors![i])
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return IsEmpty && other.IsEmpty; // if both of them are empty, they should equal
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            if (IsEmpty)
            {
                return HashCode.Combine(_index);
            }
            else
            {
                int accessHash = 0;
                for (int i = 0; i < _index; i++)
                {
                    accessHash ^= _accessors![i].GetHashCode();
                }

                return HashCode.Combine(accessHash, _index);
            }
        }

        public static bool operator ==(ParameterName left, ParameterName right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ParameterName left, ParameterName right)
        {
            return !(left == right);
        }
        #endregion Equality & HashCode

        #region Traversals
        private struct FromRootParameterNameTraversal : IEnumerable<ParameterName>, IEnumerator<ParameterName>
        {
            private readonly int _maxIndex;
            private ParameterName _current;
            private readonly ParameterName _source;

            public FromRootParameterNameTraversal(ParameterName parameterName)
            {
                if (parameterName.IsEmpty)
                {
                    throw new InvalidOperationException("Cannot travers over empty parameter nane");
                }

                _maxIndex = parameterName._index;
                _source = parameterName;
                _current = Empty;

                //parameterName.TryGetRoot(out _current);
            }

            public IEnumerator<ParameterName> GetEnumerator()
            {
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this;
            }

            public ParameterName Current
            {
                get
                {
                    return _current;
                }
            }

            public bool MoveNext()
            {
                if (_current.IsEmpty)
                {
                    return _source.TryGetRoot(out _current) && _current._index <= _maxIndex;
                }
                else
                {
                    return _current.TryGetNext(out _current) && _current._index <= _maxIndex;
                }
            }

            public void Reset()
            {
                _current.TryGetRoot(out _current);
            }

            object IEnumerator.Current
            {
                get
                {
                    return _current;
                }
            }

            public void Dispose()
            {
                Reset();
            }
        }
        private struct ToRootParameterNameTraversal : IEnumerable<ParameterName>, IEnumerator<ParameterName>
        {
            private ParameterName _current;
            private readonly ParameterName _source;

            public ToRootParameterNameTraversal(ParameterName parameterName)
            {
                if (parameterName.IsEmpty)
                {
                    throw new InvalidOperationException("Cannot travers over empty parameter nane");
                }

                _source = parameterName;
                _current = Empty;
            }

            public IEnumerator<ParameterName> GetEnumerator()
            {
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this;
            }

            public ParameterName Current
            {
                get
                {
                    return _current;
                }
            }

            public bool MoveNext()
            {
                if (_current.IsEmpty)
                {
                    _current = _source;
                    return true;
                }
                else
                {
                    return _current.TryGetPrevious(out _current);
                }
            }

            public void Reset()
            {
                _current.TryGetLeaf(out _current);
            }

            object IEnumerator.Current
            {
                get
                {
                    return _current;
                }
            }

            public void Dispose()
            {
                Reset();
            }
        }
        #endregion Traversals
    }
}
