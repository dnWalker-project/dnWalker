using dnWalker.Interface.Commands;
using dnWalker.Interface;
using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace dnWalker.Tests.Interface.Commands
{
    public class LoadModelsCommandTests
    {
        [Fact]
        public void LoadSucceeds()
        {
            Mock<IAppModel> modelMock = new Mock<IAppModel>();
            modelMock.Setup(m => m.LoadModels(It.IsAny<string>())).Returns(true);


            LoadModelsCommand cmd = new LoadModelsCommand("file1", "file2");

            cmd.Execute(modelMock.Object).Should().Be(CommandResult.Success);

            modelMock.Verify(m => m.LoadModels("file1"), Times.Once());
            modelMock.Verify(m => m.LoadModels("file2"), Times.Once());
        }

        [Fact]
        public void LoadFails()
        {
            Mock<IAppModel> modelMock = new Mock<IAppModel>();
            modelMock.Setup(m => m.LoadModels(It.IsAny<string>())).Returns(false);


            LoadModelsCommand cmd = new LoadModelsCommand("file1", "file2");

            cmd.Execute(modelMock.Object).Should().Be(CommandResult.BreakFail(-1));

            modelMock.Verify(m => m.LoadModels("file1"), Times.Once());
            modelMock.Verify(m => m.LoadModels("file2"), Times.Never());
        }
    }
}
