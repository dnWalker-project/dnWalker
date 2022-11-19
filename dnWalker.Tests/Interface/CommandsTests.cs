using dnWalker.Interface.Commands;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Interface
{
    public class CommandsTests
    {
        [Theory]
        [InlineData("", new string[0])] // no tokens
        [InlineData("token", new string[] { "token" })] // single token
        [InlineData("token token2 token3", new string[] { "token", "token2",  "token3" })] // multiple tokens
        [InlineData("\"token\" token2 token3", new string[] { "token", "token2",  "token3" })] // multiple tokens with quatation mark
        [InlineData("\"token token2\" token3", new string[] { "token token2",  "token3" })] // multiple tokens with space
        public void TokenizeTests(string cmdString, string[] tokens)
        {
            var resTokens = Command.Tokenize(cmdString);

            resTokens.Should().BeEquivalentTo(tokens);
        }

        [Theory]
        [InlineData(typeof(NoopCommand), "")]
        [InlineData(typeof(NoopCommand), null)]
        [InlineData(typeof(ExitCommand), "Exit")]
        [InlineData(typeof(ExploreCommand), "explore methodXYZ", "methodXYZ", (string?)null)]
        [InlineData(typeof(ExploreCommand), "explore methodXYZ out", "methodXYZ", "out")]
        [InlineData(typeof(LoadAssemblyCommand), "load assembly path/to/assembly", "path/to/assembly")]
        [InlineData(typeof(LoadModelsCommand), "load models path/to/models", "path/to/models")]
        [InlineData(typeof(UnknownCommand), "asdfh klasjdhf lasdhf lakjdhf kahsdf;ah df")]
        public void GetCommandTests(Type commandType, string cmdText, params string?[] args)
        {
            ICommand command = Command.GetCommand(cmdText);
            command.Should().BeOfType(commandType);
        }
    }
}
