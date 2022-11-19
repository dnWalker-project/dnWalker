using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Interface.Commands
{
    internal static class Command
    {

        internal static readonly ICommand ExitCommand = new ExitCommand();
        internal static readonly ICommand NoopCommand = new NoopCommand();

        internal static ICommand GetCommand(string commandString)
        {
            if (string.IsNullOrEmpty(commandString))
            {
                return NoopCommand;
            }

            IReadOnlyList<string> tokens = Tokenize(commandString);

            if (tokens.Count == 0) return NoopCommand;

            switch (tokens[0].ToLower())
            {
                case CommandTokens.Exit: return ExitCommand;
                case CommandTokens.Load:
                    {
                        if (tokens.Count < 2)
                        {
                            return new InvalidCommand(commandString, "load command must have at least 3 tokens: 'assembly/models' and 'file-specification'");
                        }
                        switch (tokens[1].ToLower()) 
                        {
                            case CommandTokens.Assembly: return new LoadAssemblyCommand(tokens.Skip(1));
                            case CommandTokens.Models: return new LoadModelsCommand(tokens.Skip(1));
                        }
                        return new InvalidCommand(commandString, "second parameter of the load command must be 'assembly' or 'models'");
                    }
                case CommandTokens.Explore:
                    {
                        if (tokens.Count < 2)
                        {
                            return new InvalidCommand(commandString, "explore command must have at least 2 tokens.");
                        }
                        return new ExploreCommand(tokens[1], tokens.Count == 3 ? tokens[2] : null);
                    }
            }

            if (string.IsNullOrWhiteSpace(tokens[0])) 
            {
                return NoopCommand;
            }


            return new UnknownCommand(commandString);
        }

        internal static IReadOnlyList<string> Tokenize(string commandString)
        {
            if (string.IsNullOrWhiteSpace(commandString)) return Array.Empty<string>();

            var ignoreSpace = false;

            var tokens = new List<string>();

            var lastSpace = 0;
            for (var i = 0; i < commandString.Length; ++i)
            {
                var c = commandString[i];
                if (c == ' ' && !ignoreSpace)
                {
                    tokens.Add(commandString.Substring(lastSpace, i - lastSpace).Trim(' ', '"'));
                    lastSpace = i;
                }
                else if (c == '"')
                {
                    ignoreSpace = !ignoreSpace;
                }
            }

            if (!ignoreSpace)
            {
                tokens.Add(commandString.Substring(lastSpace).Trim(' ', '"'));
            }

            return tokens;
        }
    }
}
