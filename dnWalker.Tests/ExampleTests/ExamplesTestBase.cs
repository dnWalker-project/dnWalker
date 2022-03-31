using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using dnWalker.Traversal;
using dnWalker.TypeSystem;

using MMC;
using Xunit.Abstractions;

namespace dnWalker.Tests.ExampleTests
{
    //public abstract class ExamplesTestBase : TestBase
    public abstract class ExamplesTestBase : InterpreterTests.InterpreterTestBase
    {
        //protected const string ExamplesAssemblyFileFormat = @"..\..\..\..\Examples\bin\{0}\net5.0\Examples.dll";
        protected const string ExamplesAssemblyFileFormat = @"..\..\..\..\Examples\bin\{0}\net48\Examples.dll";

        protected ExamplesTestBase(ITestOutputHelper testOutputHelper, IDefinitionProvider definitionProvider) : base(testOutputHelper, definitionProvider)
        {
            string testClassName = typeof(ExamplesTestBase).Name;
            OverrideConcolicExplorerBuilderInitialization(b =>
            {
                //dnWalker.Concolic.XmlExplorationExporter xmlExporter = new dnWalker.Concolic.XmlExplorationExporter(testClassName + "{SUT}.xml");
                dnWalker.Concolic.FlowGraphWriter graphExporter = new dnWalker.Concolic.FlowGraphWriter() { OutputFile = testClassName + "{SUT}.dot" };

                //b.With(xmlExporter);
                b.With(graphExporter);
            });
        }

        public static ArgsProvider Args()
        {
            return new ArgsProvider();
        }

        public class ArgsProvider : IDictionary<string, object>
        {
            private readonly IDictionary<string, object> _data = new Dictionary<string, object>();

            public ArgsProvider Set<T>(string key, T value)
            {
                _data.Add(key, value);

                return this;
            }

            void IDictionary<string, object>.Add(string key, object value)
            {
                _data.Add(key, value);
            }

            bool IDictionary<string, object>.ContainsKey(string key)
            {
                return _data.ContainsKey(key);
            }

            bool IDictionary<string, object>.Remove(string key)
            {
                return _data.Remove(key);
            }

            bool IDictionary<string, object>.TryGetValue(string key, out object value)
            {
                return _data.TryGetValue(key, out value);
            }

            object IDictionary<string, object>.this[string key]
            {
                get
                {
                    return _data[key];
                }

                set
                {
                    _data[key] = value;
                }
            }

            ICollection<string> IDictionary<string, object>.Keys
            {
                get
                {
                    return this._data.Keys;
                }
            }

            ICollection<object> IDictionary<string, object>.Values
            {
                get
                {
                    return _data.Values;
                }
            }

            void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
            {
                _data.Add(item);
            }

            void ICollection<KeyValuePair<string, object>>.Clear()
            {
                _data.Clear();
            }

            bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
            {
                return (_data.Contains(item));
            }

            void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
            {
                _data.CopyTo(array, arrayIndex);
            }

            bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
            {
                return _data.Remove(item);
            }

            int ICollection<KeyValuePair<string, object>>.Count
            {
                get
                {
                    return _data.Count;
                }
            }

            bool ICollection<KeyValuePair<string, object>>.IsReadOnly
            {
                get
                {
                    return _data.IsReadOnly;
                }
            }

            IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
            {
                return _data.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _data.GetEnumerator();
            }
        }
    }
}
