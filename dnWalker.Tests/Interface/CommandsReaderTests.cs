using dnWalker.Interface;
using dnWalker.Interface.Commands;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Interface
{
    public class CommandsReaderTests
    {
        [Fact]
        public void TestEnumerate()
        {
            const string CommandsString =
            """
            ; comments should be skipped
            load assembly path/to/assembly
            load models path/to/models

            explore my_method
            explore other_method output.xml

            load assembly path1 path2 path2
            load models
            explore
            ;exit
            """;

            new CommandsReader(new StringReader(CommandsString))
                .Should().BeEquivalentTo(new ICommand[]
                {
                    new LoadAssemblyCommand("path/to/assembly"),
                    new LoadModelsCommand("path/to/models"),
                    new ExploreCommand("my_method", null),
                    new ExploreCommand("other_method", "output.xml"),
                    new LoadAssemblyCommand("path1", "path2", "path2"),
                    new InvalidCommand("load models", Command.ErrorMessage.LoadTokenCount),
                    new InvalidCommand("explore", Command.ErrorMessage.ExploreTokenCount),
                    // new ExitCommand() // exit command has no members and EquivalentTo throw exception...
                }, options => options.RespectingRuntimeTypes()) ;
        }
    }
}
