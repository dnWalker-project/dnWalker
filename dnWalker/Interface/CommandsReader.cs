using dnWalker.Interface.Commands;

using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace dnWalker.Interface
{
    internal class CommandsReader : IEnumerable<ICommand>, IEnumerator<ICommand>
    {
        private readonly TextReader _reader;

        public CommandsReader(TextReader reader)
        {
            _reader = reader;
        }

        public IEnumerator<ICommand> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        private ICommand _current;

        public ICommand Current
        {
            get
            {
                return _current;
            }
        }

        public bool MoveNext()
        {
            string line;
            do
            {
                line = _reader.ReadLine()?.Trim();

            } while (line != null && 
                     (string.IsNullOrWhiteSpace(line) || 
                      line.StartsWith(';')));

            if (line == null) return false;

            _current = Command.GetCommand(line);

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