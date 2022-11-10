using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace dnWalker.Interface
{
    internal class InteractiveRunner : RunnerBase
    {
        public InteractiveRunner(Options options) : base(options)
        {
        }

        public override int Run()
        {
            return RunCommands(new CommandsReader(Console.In));
        }

        private class CommandsReader : IEnumerable<string>, IEnumerator<string>
        {
            private readonly TextReader _reader;

            public CommandsReader(TextReader reader)
            {
                _reader = reader;
            }

            public IEnumerator<string> GetEnumerator()
            {
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this;
            }

            private string _line;

            public string Current
            {
                get
                {
                    return _line;
                }
            }

            public bool MoveNext()
            {
                do
                {
                    _line = _reader.ReadLine().Trim();

                } while (string.IsNullOrWhiteSpace(_line));

                return true;
            }

            public void Reset()
            {
            }

            object IEnumerator.Current
            {
                get
                {
                    return this;
                }
            }

            public void Dispose()
            {
                _reader.Dispose();
            }
        }
    }
}